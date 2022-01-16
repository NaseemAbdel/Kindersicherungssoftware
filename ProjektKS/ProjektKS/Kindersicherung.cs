using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Timers;

namespace ProjektKS
{
    public partial class Kindersicherung : Form
    {
        public static Socket sock;
        public System.Timers.Timer tmrCHKConnection = new System.Timers.Timer();
        public Kindersicherung()
        {
            InitializeComponent();
            
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            SendData("///CMD_SHUTDOWN");
        }

        public void ReceiverThread_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                string data = ReceiveData();
            } while (true);

        }
        public string ReceiveData()
        {
            string data = null;
            byte[] bs = null;
            bs = new byte[1024];
            int bsint = sock.Receive(bs);
            data += Encoding.ASCII.GetString(bs, 0, bsint);
            return data;
        }
        public void SendData(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            sock.Send(data);
        }
        public void SrvConnect()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            IPHostEntry host = Dns.GetHostEntry("www.sus-gaming.de");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11001);
            sock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(remoteEP);
            tmrCHKConnection.Elapsed += new ElapsedEventHandler(CheckConnection);
            tmrCHKConnection.Interval = 3000;
        }

        private void Kindersicherung_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Möchten Sie das Programm schließen?", "Bestätigung", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                e.Cancel = (result == DialogResult.No);
            }
        }

        private void btnBlockAccess_Click(object sender, EventArgs e)
        {

        }

        private void btnRunProcess_Click(object sender, EventArgs e)
        {
            SendData("///CMD_RUN_PROCESS");
            SendData(txtOpenFile.Text);
        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {

        }

        private void btnLimitTime_Click(object sender, EventArgs e)
        {

        }

        public void CheckConnection(object source, ElapsedEventArgs e)
        {
            
        }

        private void btnKillProcess_Click(object sender, EventArgs e)
        {
            SendData("///CMD_KILL_PROCESS");
            SendData(txtTaskKill.Text);
        }
    }
}
