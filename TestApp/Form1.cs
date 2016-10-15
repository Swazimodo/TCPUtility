using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using TCPUtility.Transport;

namespace TestApp
{
    public partial class Form1 : Form
    {
        TCPUtility.Server.Server server = null;
        TCPUtility.Client.Client client = null;

        public Form1()
        {
            InitializeComponent();
        }

        //////////////Server Content//////////////
        private void bServerStart_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new TCPUtility.Server.Server(5, 31415);
                //register data types that will be accepted
                server.DataHandlers.RegisterHandler(typeof(TestData), new TCPUtility.Server.DataRouting.IncomingData(serverDataHandler));
                //register event handlers
                server.ServerStarted = serverStarted;
                server.ServerHaulted = serverHaulted;
            }
            server.ServerStart();
        }

        private void bServerEnd_Click(object sender, EventArgs e)
        {
            server.ServerShutdown();
        }

        private void bServerSendAll_Click(object sender, EventArgs e)
        {
            TestData data = new TestData(5);
            server.SendToAll(data);
        }

        private void bServerSendOne_Click(object sender, EventArgs e)
        {
            TestData data = new TestData(6);
            var ids = server.Clients;
            if (ids.Count == 0)
                return;
            Guid id = ids[0];
            server.SendClient(data, id);
        }

        private void serverDataHandler(BaseDataPackage data, Guid id)
        {
            TestData d = data.Unbox();
            Console.WriteLine("serverDataHandler: Server Data- " + d.MyNum);
        }

        private void serverStarted()
        {
            gbClient.Enabled = true;
            bServerStart.Enabled = false;
            bServerEnd.Enabled = true;
            bServerSendAll.Enabled = true;
            bServerSendOne.Enabled = true;
        }

        private void serverHaulted()
        {
            gbClient.Enabled = false;
            bServerStart.Enabled = true;
            bServerEnd.Enabled = false;
            bServerSendAll.Enabled = false;
            bServerSendOne.Enabled = false;
        }

        //////////////Client content///////
        private void bClientStart_Click(object sender, EventArgs e)
        {
            if (client == null)
            {
                client = new TCPUtility.Client.Client();
                //register data types that will be accepted
                client.DataHandlers.RegisterHandler(typeof(TestData), new TCPUtility.Client.DataRouting.IncomingData(clientDataHandler));
                //register event handlers
                client.ConnectionEstablished = connected;
                client.ConnectionClosed = disconnected;
            }
            client.Connect("127.0.0.1", 31415);
        }

        private void bClientEnd_Click(object sender, EventArgs e)
        {
            client.Disconnect();
        }

        private void bClientSend_Click(object sender, EventArgs e)
        {
            TestData data = new TestData(7);
            client.SendData(data);
        }

        private void clientDataHandler(BaseDataPackage data)
        {
            TestData d = data.Unbox();
            Console.WriteLine("serverDataHandler: Server Data- " + d.MyNum);
        }

        private void connected()
        {
            bClientStart.Enabled = false;
            bClientEnd.Enabled = true;
            bClientSend.Enabled = true;
        }

        private void disconnected()
        {
            bClientStart.Enabled = true;
            bClientEnd.Enabled = false;
            bClientSend.Enabled = false;
        }
    }
}
