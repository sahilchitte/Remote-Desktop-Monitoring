using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace client_remote_desktop
{
    public partial class Form1 : Form
    {
        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainstream;
        private int portNumber;
        

        private static Image GrabDesktop()
        {
            Rectangle bound = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bound.Width, bound.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(screenshot);
            graphics.CopyFromScreen(bound.X, bound.Y, 0, 0, bound.Size, CopyPixelOperation.SourceCopy);

            return screenshot;
        }

        private void SendDesktopImage()
        {
            BinaryFormatter binformatter = new BinaryFormatter();
            mainstream = client.GetStream();
            binformatter.Serialize(mainstream, GrabDesktop());
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnShare.Enabled = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            portNumber = int.Parse(txtPort.Text);

            try
            {
                client.Connect(txtIP.Text, portNumber);
                btnConnect.Text = "Connected";
                MessageBox.Show("Connected Successfully");
                btnConnect.Enabled = false;
                btnShare.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to Connect");
                btnConnect.Text = "Not Connected";
            }
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            if (btnShare.Text.StartsWith("Share"))
            {
                timer1.Start();
                btnShare.Text = "Stop Sharing";
            }
            else
            {
                timer1.Stop();
                btnShare.Text = "Share My Screen";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendDesktopImage();
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
