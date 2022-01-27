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
        public static string SrvIP = "www.sus-gaming.de";
        public static Socket FileSock;
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
            IPHostEntry host = Dns.GetHostEntry(SrvIP);
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
            if(result == DialogResult.No)
            {
                e.Cancel = (result == DialogResult.No);
            }
            else
            {
                Application.Exit();
            }
        }

        private void btnBlockAccess_Click(object sender, EventArgs e)
        {
            SendData("///CMD_BLOCK_ADRESS");
            string adress = txtBlockAccess.Text;
            if (adress.Substring(0, 4) == "www.")
            {
                SendData(txtBlockAccess.Text);
            }
            else
            {
                SendData("www." + txtBlockAccess.Text);
            }
        }

        private void btnRunProcess_Click(object sender, EventArgs e)
        {
            SendData("///CMD_RUN_PROCESS");
            SendData(txtOpenFile.Text);
        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            SendData("///CMD_UPLOAD_FILE");
        }

        private void btnLimitTime_Click(object sender, EventArgs e)
        {
            SendData("///CMD_LIMIT_TIME");
            SendData(txtLimitTime.Text);
        }

        public void CheckConnection(object source, ElapsedEventArgs e)
        {
            
        }

        private void btnKillProcess_Click(object sender, EventArgs e)
        {
            SendData("///CMD_KILL_PROCESS");
            SendData(txtTaskKill.Text);
        }

        private void btnRemoveBlock_Click(object sender, EventArgs e)
        {
            SendData("///CMD_FREE_ADRESS");
            string adress = txtRemoveBlock.Text;
            if (adress.Substring(0, 4) == "www.")
            {
                SendData(txtRemoveBlock.Text);
            }
            else
            {
                SendData("www." + txtRemoveBlock.Text);
            }
        }
        bool row = true;
        private void btnShowList_Click(object sender, EventArgs e)
        {
            if (row == true)
            {
                SendData("///CMD_SEND_GL");
                GetGameList();
                Spiele.Visible = true;
                txtBanGame.Visible = true;
                btnBanGame.Visible = true;
                btnShowList.Text = "Schließen";
                row = false;
            }
            else
            {
                Spiele.Visible = false;
                txtBanGame.Visible = false;
                btnBanGame.Visible = false;
                btnShowList.Text = "Spiele zur Überwachung hinzufügen";
                row = true;
            }
        }
        private void GetGameList()
        {
            Spiele.Items.Clear();
            var GameCount = ReceiveData();
            int GameCountInt = Convert.ToInt16(GameCount);
            for (int i = 1; i <= GameCountInt; i++)
            {
                Spiele.Items.Add(ReceiveData());
            }
        }

        private void btnBanGame_Click(object sender, EventArgs e)
        {
            SendData("///CMD_GAME_ADD");
            SendData(txtBanGame.Text);
            txtBanGame.Text = "";
            GetGameList();
        }

        private void Spiele_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendData("///CMD_GAME_RM");
            SendData(Spiele.SelectedItem.ToString());
            GetGameList();
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            SendData("///CMD_RC_FILE");
            Thread.Sleep(100);
            long filesize = new System.IO.FileInfo(UploadFileSelector.FileName).Length;
            SendData(filesize.ToString());
            Thread.Sleep(100);
            SendData(UploadFileSelector.SafeFileName);
            ConnectFileSock();
            FileSock.SendFile(UploadFileSelector.FileName);
            FileSock.Close();
        }

        private void btnSelFile_Click(object sender, EventArgs e)
        {
            UploadFileSelector.ShowDialog();
            txtUploadFile.Text = UploadFileSelector.FileName;
        }
        private void ConnectFileSock()
        {
            IPHostEntry host = Dns.GetHostEntry(SrvIP);
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11002);
            FileSock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            FileSock.Connect(remoteEP);
        }
    }
}
