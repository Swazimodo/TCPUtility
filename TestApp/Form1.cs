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

using NetworkUtility.Tcp.Transport;

namespace TestApp
{
    public partial class Form1 : Form
    {
        NetworkUtility.Tcp.Server.Server server = null;
        NetworkUtility.Tcp.Client.Client client = null;

        public Form1()
        {
            InitializeComponent();
        }

        //////////////Server Content//////////////
        private void bServerStart_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new NetworkUtility.Tcp.Server.Server(5, 31415);
                //register data types that will be accepted
                server.DataHandlers.RegisterHandler(typeof(ButtonClickData), new NetworkUtility.Tcp.Server.DataRouting.IncomingData(serverDataHandler));
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
            ButtonClickData data = new ButtonClickData("Sent to all clients");
            server.SendToAll(data);
        }

        private void bServerSendOne_Click(object sender, EventArgs e)
        {
            ButtonClickData data = new ButtonClickData("Send to one client");
            var ids = server.Clients;
            if (ids.Count == 0)
                return;
            Guid id = ids[0];
            server.SendClient(data, id);
        }

        private void serverDataHandler(BaseDataPackage data, Guid id)
        {
            ButtonClickData d = data.Unbox();
            tbServerData.Text = d.ButtonClicked;
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
                client = new NetworkUtility.Tcp.Client.Client();
                //register data types that will be accepted
                client.DataHandlers.RegisterHandler(typeof(ButtonClickData), new NetworkUtility.Tcp.Client.DataRouting.IncomingData(clientDataHandler));
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
            ButtonClickData data = new ButtonClickData("Send data from client");
            client.SendData(data);
        }

        private void clientDataHandler(BaseDataPackage data)
        {
            ButtonClickData d = data.Unbox();
            tbClientData.Text = d.ButtonClicked;
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
