using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;

using TCPUtility.Transport;

namespace TCPUtility.Server
{
    public class Server
    {
        #region Member variables

        //events call into this context to ensure that there isnt any funny thead issues for consumers of this appliation
        SynchronizationContext _context;
        //socket listening on any ip and 1666 port
        Socket _listenSock = null;//= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //list to hold all information about each connection
        List<ClientReference> _clients = new List<ClientReference>();
        //queue of data to send to all clients
        Queue<BaseDataPackage> _groupDataQueue = new Queue<BaseDataPackage>();
        //queue of data to send to specific client
        Queue<KeyValuePair<ClientReference, BaseDataPackage>> _clientDataQueue = new Queue<KeyValuePair<ClientReference, BaseDataPackage>>();
        //thread pushing out data from the _groupDataQueue queue
        Thread _thrSend;
        //serializes data
        BinaryFormatter _bf = new BinaryFormatter();

        bool _startError = false;
        int _maxConnections;
        int _portNumber;

        #endregion

        #region Public Properties

        /// <summary>
        /// checks if the port is bound and if the worker threads are running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                bool isRunning = true;

                if (_thrSend == null || !_thrSend.IsAlive) isRunning = false;
                if (_listenSock == null || !_listenSock.IsBound) isRunning = false;
                //if (_startError) isRunning = false;

                return isRunning;
            }
        }

        public int PortNumber { get { return _portNumber; } }
        public int MaxConnections { get { return _maxConnections; } }

        public DataRouting DataHandlers { get; private set; }

        public List<Guid> Clients
        {
            get
            {
                List<Guid> clients = new List<Guid>();
                lock (_clients)
                {
                    clients = _clients.Select(x => x.Id).ToList();
                }
                return clients;
            }
        }

        public int ClientCount
        {
            get
            {
                lock (_clients)
                {
                    return _clients.Count;
                }
            }
        }

        #endregion

        #region Events

        public delVoidVoid ServerStarted { get; set; }
        public delVoidVoid ServerHaulted { get; set; }
        public delVoidGuid ClientConnected { get; set; }
        public delVoidGuid ClientDisconnected { get; set; }

        #endregion

        #region Constructors

        public Server(int maxConnections, int portNumber)
        {
            _context = SynchronizationContext.Current;
            _maxConnections = maxConnections;
            _portNumber = portNumber;
            DataHandlers = new DataRouting();
        }

        public Server(int maxConnections, int portNumber, int bufferSize)
        {
            _context = SynchronizationContext.Current;
            _maxConnections = maxConnections;
            _portNumber = portNumber;
            DataHandlers = new DataRouting();
            ClientReference.BufferSize = 1500;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Open port start listening for connections
        /// </summary>
        /// <exception cref="Exception">Thown if server does not startup correctly</exception>
        public void ServerStart()
        {
            if (IsRunning) return;
            _startError = false;

            //start up the thread if needed
            if (_thrSend == null || _thrSend.ThreadState == ThreadState.Aborted)
            {
                _thrSend = new Thread(new ThreadStart(SendLoop));
                _thrSend.IsBackground = true;
            }
            if (!_thrSend.IsAlive)
                _thrSend.Start();

            //verify that connection is disposed and then recreate it
            if (_listenSock != null)
                _listenSock.Dispose();
            _listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _listenSock.Bind(new IPEndPoint(IPAddress.Any, _portNumber));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerStart:bind " + ex.ToString());
                _startError = true;
            }

            try
            {
                _listenSock.Listen(_maxConnections);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerStart:Listen " + ex.ToString());
                _startError = true;
            }

            try
            {
                _listenSock.BeginAccept(cbAcceptDone, _listenSock);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerStart:BeginAccept " + ex.ToString());
                _startError = true;
            }

            if (!_startError)
            {
                try
                {
                    //dispatch call into the proper thread
                    _context.Post(s =>
                    {
                        ServerStarted?.Invoke();
                    }, null);
                    //ServerStarted?.Invoke();
                }
                catch (Exception ex)
                {
                    //dont trust user code
                    Console.WriteLine("Calling server ServerStarted event handler threw an exception");
                }
            }
            else
                throw new Exception("Unable to start server");
        }

        public void ServerShutdown()
        {
            //kill worker thread
            if (_thrSend != null)
                _thrSend.Abort();

            //drop all clients
            lock (_clients)
            {
                foreach (var client in _clients)
                {
                    client.sock.Close();
                    client.sock.Dispose();
                }
                _clients.Clear();
            }

            //close listening socket
            _listenSock.Close();
            _listenSock.Dispose();

            //clear out all send queues
            lock (_clientDataQueue)
            {
                _clientDataQueue.Clear();
            }
            lock (_groupDataQueue)
            {
                _groupDataQueue.Clear();
            }

            try
            {
                //dispatch call into the proper thread
                _context.Post(s =>
                {
                    ServerHaulted?.Invoke();
                }, null);
                //ServerHaulted?.Invoke();
            }
            catch (Exception ex)
            {
                //dont trust user code
                Console.WriteLine("Calling server ServerHaulted event handler threw an exception");
            }
        }

        /// <summary>
        /// send data to all connected clients
        /// </summary>
        /// <param name="data">Data will be added to a queue</param>
        public void SendToAll(BaseDataPackage data)
        {
            lock (_groupDataQueue)
            {
                _groupDataQueue.Enqueue(data);
            }
        }

        /// <summary>
        /// send a specific client data
        /// </summary>
        /// <param name="data">Data that will be sent to one client</param>
        /// <param name="clientId">ID of client</param>
        public void SendClient(BaseDataPackage data, Guid clientId)
        {
            KeyValuePair<ClientReference, BaseDataPackage> kvp = 
                new KeyValuePair<ClientReference, BaseDataPackage>(GetClientById(clientId), data);
            lock (_clientDataQueue)
            {
                _clientDataQueue.Enqueue(kvp);
            }
        }

        #endregion

        #region private methods

        private ClientReference GetClientById(Guid id)
        {
            ClientReference client = null;
            lock (_clients)
            {
                client = _clients.Find(x => x.Id == id);
            }
            return client;
        }

        /// <summary>
        /// Accept a new connection
        /// </summary>
        private void cbAcceptDone(IAsyncResult ar)
        {
            Socket sSock = (Socket)(ar.AsyncState);
            try
            {
                Socket sockNew = sSock.EndAccept(ar);

                //call data accept, as there was a connection established
                //Invoke(new delVoidSock(DataAccept), sockNew);
                DataAccept(sockNew);
                Console.WriteLine("cbAcceptDone:DataAccept ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("cbAcceptDone:ConnNotSuccess" + ex.ToString());
            }

            try
            {
                //Get more connections!
                _listenSock.BeginAccept(cbAcceptDone, _listenSock);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AcceptMoreConn:BeginAccept " + ex.ToString());
            }
        }

        /// <summary>
        /// start accepting data for a specific client
        /// </summary>
        private void DataAccept(Socket sock)
        {
            ClientReference client = new ClientReference(sock);
            lock (_clients)
            {
                _clients.Add(client);
            }

            try
            {
                //dispatch call into the proper thread
                _context.Post(s =>
                {
                    ClientConnected?.Invoke(client.Id);
                }, null);
                //ClientConnected?.Invoke(client.Id);
            }
            catch (Exception ex)
            {
                //dont trust user code
                Console.WriteLine("Calling server ClientConnected event handler threw an exception");
            }

            try
            {
                //connection successful, so begin receiving data sent from client
                sock.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, cbRxDone, client);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataAccept:BeginReceive " + ex.ToString());
            }
        }

        /// <summary>
        /// Data showed up from the client
        /// </summary>
        private void cbRxDone(IAsyncResult ar)
        {
            ClientReference client = (ClientReference)(ar.AsyncState);
            try
            {
                client.BytesReceived = client.sock.EndReceive(ar);
                ClientReference.TotalBytes += client.BytesReceived;
            }
            catch (Exception ex)
            {
                //set client up for removale
                Console.WriteLine("cbRxDone:EndReceive " + ex.ToString());
                client.BytesReceived = 0;
            }
            //Invoke(new delVoidClientReference(DataReceived), client);
            DataReceived(client);
        }

        /// <summary>
        /// Check if data is a complete package
        /// </summary>
        private void DataReceived(ClientReference client)
        {
            //remove client if it is required
            if (client.BytesReceived == 0)
            {
                try
                {
                    //dispatch call into the proper thread
                    _context.Post(s =>
                    {
                        ClientDisconnected?.Invoke(client.Id);
                    }, null);
                    //ClientDisconnected?.Invoke(client.Id);
                }
                catch (Exception ex)
                {
                    //dont trust user code
                    Console.WriteLine("Calling server ClientDisconnected event handler threw an exception");
                }

                lock (_clients)
                {
                    _clients.Remove(client);
                }

                return;
            }

            //LineSegment LS; //Getting ready for some Lines received
            //List<LineSegment> lsList = new List<LineSegment>();
            //Putting the memory stream pointer in the correct place
            long lPos = client.ms.Position;
            client.ms.Seek(0, SeekOrigin.End);
            client.ms.Write(client.Buffer, 0, client.BytesReceived);
            client.ms.Position = lPos;

            do //Defragmenting copy pasta
            {
                long lStartPos = client.ms.Position;
                try
                {
                    object o = _bf.Deserialize(client.ms); //deserialize into object to check for types

                    if (o is BaseDataPackage)
                    {
                        //Data received is of the correct type
                        ClientReference.TotalRecieved++;
                        client.Recieved++;
                        //calls the method registered to handle this package
                        DataHandlers.CallMethod(_context, (o as BaseDataPackage), client.Id);
                    }
                    else
                        //what the heck was this type? Someone trying to break our server!
                        Console.WriteLine("unknown shiz");
                }
                catch (SerializationException)
                {
                    //Get out of loop and wait for more data
                    Console.WriteLine("fragmentation of data");
                    client.ms.Position = lStartPos;
                    client.Fragments++; //increases the number of fragments that happened per connection

                    break;
                }
            }
            while (client.ms.Position < client.ms.Length);

            //client.RecieveCalls++;


            //display traffic information
            //string s;
            //if (ClientReference.TotalBytes < 1024)
            //    s = "Bytes RX'ed: B" + ClientReference.TotalBytes.ToString();
            //else
            //    if (ClientReference.TotalBytes < 1048576)
            //    s = "Bytes RX'ed: kB" + (ClientReference.TotalBytes / 1024.0).ToString("f2");
            //else
            //        if (ClientReference.TotalBytes < 1073741824)
            //    s = "Bytes RX'ed: MB" + (ClientReference.TotalBytes / 1048576.0).ToString("f2");
            //else
            //    s = "Bytes RX'ed: MB" + (ClientReference.TotalBytes / 1073741824.0).ToString("f2");

            //Text = "TolalBytes: " + s + " TotalFrames: " + CSam.TotalRecieved.ToString();

            //all data in stream has been read, reset stream
            if (client.ms.Position == client.ms.Length)
            {
                //reset the memory stream pointer and length
                client.ms.Position = 0;
                client.ms.SetLength(0);
                Console.WriteLine("Resetting RX Memory stream");
            }

            try
            {
                //Receive more stuff!
                client.sock.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, cbRxDone, client);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataReceived:BeginReceive at end " + ex.ToString());
            }
        }

        /// <summary>
        /// watch queue and push data to all clients
        /// </summary>
        private void SendLoop()
        {
            try
            {
                while (true)
                {
                    //send group data
                    lock (_groupDataQueue)
                    {
                        if (_groupDataQueue.Count != 0)
                        {
                            MemoryStream ms;
                            BaseDataPackage data;
                            while (_groupDataQueue.Count != 0)
                            {
                                data = _groupDataQueue.Dequeue();
                                ms = new MemoryStream();
                                _bf.Serialize(ms, data);
                                lock (_clients)
                                {
                                    if(_clients.Count == 0)
                                    {
                                        //there are no clients drop data
                                        _groupDataQueue.Clear();
                                        continue;
                                    }
                                    foreach (ClientReference client in _clients)
                                    {
                                        try
                                        {
                                            //client.sock.Send(ms.GetBuffer(), 0, (int)ms.Length, SocketFlags.None);
                                            client.sock.BeginSend(ms.GetBuffer(), 0, (int)ms.Length, SocketFlags.None, null, client);
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("DataReceived: client probably disconnected");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //send specific client data
                    lock (_clientDataQueue)
                    {
                        if (_clientDataQueue.Count != 0)
                        {
                            MemoryStream ms;
                            KeyValuePair<ClientReference, BaseDataPackage> kvp;
                            while (_clientDataQueue.Count != 0)
                            {
                                kvp = _clientDataQueue.Dequeue();
                                ms = new MemoryStream();
                                _bf.Serialize(ms, kvp.Value);
                                try
                                {
                                    kvp.Key.sock.BeginSend(ms.GetBuffer(), 0, (int)ms.Length, SocketFlags.None, null, kvp.Key);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("DataReceived: client probably disconnected");
                                }
                            }
                        }
                    }
                    Thread.Sleep(0);
                }
            }
            catch (ThreadAbortException)
            {
                // Clean-up code can go here.
                // If there is no Finally clause, ThreadAbortException is
                // re-thrown by the system at the end of the Catch clause. 
            }
        }

        #endregion

        #region definitions

        private delegate void delVoidClientReference(ClientReference client);
        private delegate void delVoidSock(Socket s);
        public delegate void delVoidVoid();
        public delegate void delVoidGuid(Guid id);

        #endregion
    }
}
