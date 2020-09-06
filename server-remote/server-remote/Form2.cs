using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;


namespace server_remote
{
    public partial class Form2 : Form
    {
        private readonly int port;
        private TcpClient client;
        private TcpListener server;
        private NetworkStream mainstream;

        private readonly Thread listening;
        private readonly Thread GetImage;
        public Form2(int Port)
        {
            port = Port;
            client = new TcpClient();
            listening = new Thread(startlistening);
            GetImage = new Thread(receiveimage);


            InitializeComponent();
        }

        private void startlistening()
        {
            while(!client.Connected)
            {
                server.Start();
                client = server.AcceptTcpClient();

            }
            GetImage.Start();
        }

        private void stoplistening()
        {
            server.Stop();
            client = null;
            if (listening.IsAlive) listening.Abort();
            if (GetImage.IsAlive) listening.Abort();
        }

        private void receiveimage()
        {
            BinaryFormatter binformatter = new BinaryFormatter();
            while(client.Connected)
            {
                mainstream = client.GetStream();
                pictureBox1.Image = (Image)binformatter.Deserialize(mainstream);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            server = new TcpListener(IPAddress.Any, port);
            listening.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            stoplistening();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
