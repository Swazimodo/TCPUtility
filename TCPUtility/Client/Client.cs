using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using TCPUtility.Transport;

namespace TCPUtility.Client
{
    public class Client
    {
        #region member variables

        Socket _socket;                             //tcp socket into port 1666 of server
        byte[] _bBuff;                              //buffer to dump data into
        MemoryStream _ms = new MemoryStream();      //used for recieving data
        BinaryFormatter _bf = new BinaryFormatter();
        ConnectionState _state = ConnectionState.Disconnected;

        int _iFrames;              //number of frames recieved
        int _iFragments;           //number of fragments encountered
        int _iRecieves;            //number of recieve calls
        int _iBytesTotal;          //number of bytes recieved so far

        #endregion

        #region Properties

        public DataRouting DataHandlers { get; private set; }

        #endregion

        #region Events

        public delVoidVoid ConnectionClosed { get; set; }
        public delVoidVoid ConnectionEstablished { get; set; }

        #endregion

        #region constructor

        public Client()
        {
            _bBuff = new byte[1500];
            DataHandlers = new DataRouting();
        }

        public Client(int bufferSize)
        {
            _bBuff = new byte[bufferSize];
            DataHandlers = new DataRouting();
        }

        #endregion

        #region public methods

        public void Connect(string url, int port)
        {
            //do not connect if already connected
            if (_state != ConnectionState.Disconnected) return;
            _state = ConnectionState.Connecting;

            //reset stats for new connection
            _iFrames = 0;
            _iFragments = 0;
            _iRecieves = 0;
            _iBytesTotal = 0;

            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.BeginConnect(url, port, cbConnectDone, _socket);
            }
            catch (Exception)
            {
                Console.WriteLine("Connect: connect failed");
                _state = ConnectionState.Disconnected; ;
            }
        }

        public void Disconnect()
        {
            _socket.Disconnect(false);
            _socket.Dispose();
            _state = ConnectionState.Disconnected;

            try
            {
                ConnectionClosed?.Invoke();
            }
            catch (Exception)
            {
                //dont trust user code
                Console.WriteLine("Calling client ConnectionClosed event handler threw an exception");
            }
        }

        public void SendData(BaseDataPackage data)
        {
            MemoryStream msend = new MemoryStream();
            try
            {
                _bf.Serialize(msend, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendData: Serialize");
                msend.Dispose();
                return;
            }

            try
            {
                _socket.Send(msend.GetBuffer(), 0, (int)msend.Length, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendData: Send");
            }
        }

        #endregion

        #region private methods

        private void cbConnectDone(IAsyncResult arr)
        {
            try
            {
                Socket soc = ((Socket)arr.AsyncState);
                soc.EndConnect(arr);
                //Invoke(new delVoidVoid(ConnectDone), null);
                ConnectDone();
            }
            catch (Exception)
            {
                Console.WriteLine("cbConnectDone: connect failed");
            }
        }

        private void ConnectDone()
        {
            try
            {
                _socket.BeginReceive(_bBuff, 0, _bBuff.Length, SocketFlags.None, cbRxDone, _socket);

                try
                {
                    ConnectionEstablished?.Invoke();
                }
                catch (Exception)
                {
                    //dont trust user code
                    Console.WriteLine("Calling client ConnectionEstablished event handler threw an exception");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("ConnectDone: connect failed");
            }
        }

        //callback for data recieved
        private void cbRxDone(IAsyncResult arr)
        {
            Socket soc = ((Socket)arr.AsyncState);
            try
            {
                int iNumRecieved;
                _iBytesTotal += iNumRecieved = soc.EndReceive(arr);
                //Invoke(new delVoidInt(RxDone), iNumRecieved);
                RxDone(iNumRecieved);
            }
            catch (Exception)
            {
                Console.WriteLine("cbRxDone: connect failed");
                //close connection
                _socket.Close();
                _socket = null;
            }
        }

        //you gots data
        private void RxDone(int iNumBytes)
        {
            BaseDataPackage data;
            // save the current read position
            long lPos = _ms.Position;

            // seek to the end to append the data
            _ms.Seek(0, SeekOrigin.End);

            // append the new data to the end of the stream
            _ms.Write(_bBuff, 0, iNumBytes);

            // restore the read position back to where it started
            _ms.Position = lPos;

            // show byte count received
            //Console.WriteLine("RxDone: Added " + iNumBytes.ToString() +
            //" bytes... for a total of " + (_ms.Length - _ms.Position).ToString());
            _iRecieves++;
            do
            {
                // attempt to extract 1 or more complete frames
                // save the stream position in case the
                // deserialization fails, and it has to be reset
                long lStartPos = _ms.Position;
                try
                {
                    // attempt to deserialize an object at this position
                    object o = _bf.Deserialize(_ms);
                    // no exception, so process the received frame and move on
                    if (o is BaseDataPackage)
                    {
                        data = (o as BaseDataPackage);
                        _iFrames++;
                        DataHandlers.CallMethod(data);
                    }
                    else
                    {
                        Console.WriteLine("RxDone: unknown frame");
                    }
                }
                catch (System.Runtime.Serialization.SerializationException)
                {
                    // deserialize failed, so move the read position back
                    // assume more data will show up that this item needs to be deserialized
                    _ms.Position = lStartPos;

                    // show that this time, a full object could not be pulled
                    Console.WriteLine("RxDone: Could not deserialize.... yet...");

                    _iFragments++;

                    // get out of the destacking loop
                    break;
                }
            }
            while (_ms.Position < _ms.Length);

            //update stats lables
            //if (_iBytesTotal < 1024)
            //    lBytes.Text = "Bytes RX'ed: B" + _iBytesTotal.ToString();
            //else
            //    if (_iBytesTotal < 1048576)
            //    lBytes.Text = "Bytes RX'ed: kB" + (_iBytesTotal / 1024.0).ToString("f2");
            //else
            //        if (m_iBytesTotal < 1073741824)
            //    lBytes.Text = "Bytes RX'ed: MB" + (_iBytesTotal / 1048576.0).ToString("f2");
            //else
            //    lBytes.Text = "Bytes RX'ed: MB" + (_iBytesTotal / 1073741824.0).ToString("f2");

            //lDestackAvg.Text = "Destack Avg.: " + (_iFrames / (double)_iRecieves).ToString("f2");
            //lFragments.Text = "Fragments: " + _iFragments.ToString();
            //lFrames.Text = "Frames RX'ed: " + _iFrames.ToString();

            // if all data has been read from the rx memorystream, reset it
            // otherwise it will continue to hold all data EVER received
            if (_ms.Position == _ms.Length)
            {
                _ms.Position = 0;
                _ms.SetLength(0);
                Console.WriteLine("RxDone: Resetting RX Memorystream...");
            }
            // do optional trim, if buffer pounding is a concern
            // start another rx operation

            try
            {
                _socket.BeginReceive(_bBuff, 0, _bBuff.Length, SocketFlags.None, cbRxDone, _socket);
            }
            catch (Exception)
            {
                Console.WriteLine("RxDone:BeginReceive");
            }
        }

        #endregion

        #region definitions

        public enum ConnectionState { Disconnected, Connecting, Connected }

        public delegate void delVoidVoid();
        private delegate void delVoidInt(int i);

        #endregion
    }
}
