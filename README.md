# TCPUtility
Create client/server C# applications that use TCP communication without dealing with any of the communication headache.
Thread management, data serialization, socket connections are all managed for you.
Define your own data types and handlers and have them just work.

# Setup Data transfer classes
## define data types that will be transferred between client and server
    //Example data class that transfers an int
	[Serializable]
    public class TestData : BaseDataPackage
    {
        public int MyNum { get; set; }

        public TestData(int num)
        {
            MyNum = num;
        }
    }
   
# Server Setup 
## create server
    //set number of allowed connections and a port number
	var server = new TCPUtility.Server.Server(5, 31415);

## register handler for each data type supported - this delegate will then be called whenever this type of data is transferred
    server.DataHandlers.RegisterHandler(typeof(TestData), new TCPUtility.Server.DataRouting.IncomingData(serverDataHandler));
    
    //example handler
    private void serverDataHandler(BaseDataPackage data, Guid id)
    {
        TestData d = data.Unbox();
        Console.WriteLine("serverDataHandler: Server Data- " + d.MyNum);
    }
    
## start server and shutdown server commands
    server.ServerStart();
    server.ServerShutdown();
    
## send data to one specific connected client
    private void bSendAll_Click(object sender, EventArgs e)
    {
        TestData data = new TestData(5);
        server.SendToAll(data);
    }
    
## send data to all connected clients
    private void bSendOne_Click(object sender, EventArgs e)
    {
        TestData data = new TestData(6);
        
        //grab the first client to show its possible
        //this would likely be done in a data handler to reply back to package that was received
        var ids = server.Clients;
        if (ids.Count == 0)
            return;
        Guid id = ids[0];
        
        server.SendClient(data, id);
    }
    
# Client Setup
## create a new client
    var client = new TCPUtility.Client.Client();

## register handler for each data type supported - this delegate will then be called whenever this type of data is transferred
    client.DataHandlers.RegisterHandler(typeof(TestData), new TCPUtility.Client.DataRouting.IncomingData(clientDataHandler));
    
    //example data handler
    private void clientDataHandler(BaseDataPackage data)
    {
        TestData d = data.Unbox();
        Console.WriteLine("serverDataHandler: Server Data- " + d.MyNum);
    }

## connect the client to a server - specify url and port number
    client.Connect("127.0.0.1", 31415);

## send data to the server
    private void bSend_Click(object sender, EventArgs e)
    {
        TestData data = new TestData(7);
        client.SendData(data);
    }

## disconnect client
    client.Disconnect();
