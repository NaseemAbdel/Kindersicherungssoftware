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
using System.IO;
using System.Threading;
using System.Timers;



namespace EmpfängerKS
{
    class Program
    {
        static void Main(string[] args)
        {
            var Fkts = new Fkts();
            Fkts.Launch();
            Application.Run();
        }
    }

    class Fkts
    {
        int TimeLimit;
        public System.Timers.Timer GameTimer = new System.Timers.Timer();
        public System.Timers.Timer gamechecker = new System.Timers.Timer();
        string hostspath = @"C:\WINDOWS\System32\drivers\etc\hosts";
        string gamecfgpath = @"C:\Windows\Kindersicherungsprogramm\cfgs\game.ks";
        Socket sock;
        public BackgroundWorker ReceiverThread = new BackgroundWorker();
        public BackgroundWorker AsyncMSGBOX = new BackgroundWorker();
        List<string> gamecfglist;
        bool GTrunning = false;

        public void Launch()
        {
            InitThreads();
            connect();
            GetGameList();
            InitTimers();
        }

        private void InitTimers()
        {
            GameTimer.Elapsed += new ElapsedEventHandler(GTimer);
            gamechecker.Elapsed += new ElapsedEventHandler(ChkGames);
            List<string> gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
            TimeLimit = Convert.ToInt16(gamecfglist[0]);
            GameTimer.Interval = 60000;
            gamechecker.Interval = 5000;
            gamechecker.Start();
        }

        public void InitThreads()
        {
            this.ReceiverThread.DoWork +=
                new DoWorkEventHandler(ReceiverThread_DoWork);
            this.AsyncMSGBOX.DoWork +=
                new DoWorkEventHandler(asyncmsg_DoWork);
        }
        private void GetGameList()
        {
            gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
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
        private void asyncmsg_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show(e.Argument as string);
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
                case "///CMD_BLOCK_ADRESS":
                    {
                        BlockSite();
                        break;
                    }
                case "///CMD_FREE_ADRESS":
                    {
                        UnblockSite();
                        break;
                    }
                case "///CMD_LIMIT_TIME":
                    {
                        LimitTime();
                        break;
                    }
                case "///CMD_GAME_ADD":
                    {
                        AddGame();
                        break;
                    }
                case "///CMD_GAME_RM":
                    {
                        RemoveGame();
                        break;
                    }
                case "///CMD_SEND_GL":
                    {
                        SendGameList();
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
        private void BlockSite()
        {
            string site = ReceiveData();
            List<string> hostsfile = File.ReadAllLines(hostspath).ToList();
            hostsfile.Add("127.0.0.1 " + site);
            File.WriteAllLines(hostspath, hostsfile);
            Process.Start("ipconfig", "/flushdns");
        }
        private void UnblockSite()
        {
            string site = ReceiveData();
            List<string> hostsfile = File.ReadAllLines(hostspath).ToList();
            hostsfile.Remove("127.0.0.1 " + site);
            File.WriteAllLines(hostspath, hostsfile);
            Process.Start("ipconfig", "/flushdns");
        }
        private void LimitTime()
        {
            string time = ReceiveData();
            List<string> gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
            gamecfglist[0] = time;
            File.WriteAllLines(gamecfgpath, gamecfglist);
            TimeLimit = Convert.ToInt16(gamecfglist[0]);
        }
        private void AddGame()
        {
            string game = ReceiveData();
            gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
            gamecfglist.Add(game);
            File.WriteAllLines(gamecfgpath, gamecfglist);
            SendGameList();
        }
        private void RemoveGame()
        {
            string game = ReceiveData();
            gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
            gamecfglist.Remove(game);
            File.WriteAllLines(gamecfgpath, gamecfglist);
            SendGameList();
        }
        private void SendGameList()
        {
            int gamecount = gamecfglist.Count() - 1;
            SendData(gamecount.ToString());
            for (int i = 1; i <= gamecount; i++)
            {
                SendData(gamecfglist[i]);
                Thread.Sleep(100);
            }
        }
        private void ChkGames(object source, ElapsedEventArgs e)
        {
            var games = new List<Process>();
            foreach (string gamename in gamecfglist)
            {
                games.AddRange(Process.GetProcessesByName(gamename));
            }
            if (TimeLimit == 0 && games.Count > 0)
            {
                foreach (Process game in games)
                {
                    game.Kill();
                }
                AsyncMSGBOX.RunWorkerAsync("Spielzeit abgelaufen");
            }
            else
            {
                if (games.Count > 0 && GTrunning == false)
                {
                    GameTimer.Start();
                    GTrunning = true;
                }
                else if (games.Count == 0 && GTrunning == true)
                {
                    GameTimer.Stop();
                    GTrunning = false;
                }
            }

        }
        private void SendData(string data)
        {
            byte[] databytes = Encoding.ASCII.GetBytes(data);
            sock.Send(databytes);
        }
        private void GTimer(object source, ElapsedEventArgs e)
        {
            if (TimeLimit > 0)
            {
                TimeLimit--;
                List<string> gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
                gamecfglist[0] = TimeLimit.ToString();
                File.WriteAllLines(gamecfgpath, gamecfglist);
            }
            else
            {
                GameTimer.Stop();
                GTrunning = false;
                List<string> gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
                gamecfglist[0] = TimeLimit.ToString();
                File.WriteAllLines(gamecfgpath, gamecfglist);
            }
            
        }
        
    }
} 
