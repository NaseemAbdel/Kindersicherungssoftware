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
        int TimeLimit; //Integer mit der restlichen Spielezeit in Minuten
        public System.Timers.Timer GameTimer = new System.Timers.Timer(); //Timer für die restliche Spielezeit
        public System.Timers.Timer gamechecker = new System.Timers.Timer(); //Timer der jede 5 Sekunden auf Spiele überprüft
        string hostspath = @"C:\WINDOWS\System32\drivers\etc\hosts"; //Der Pfad zur Windows-Hosts Datei
        string gamecfgpath = @"C:\Windows\Kindersicherungsprogramm\cfgs\game.ks"; //Der Pfad zur game.ks (Config) Datei
        
        //game.ks Aufbau:
        //1.Zeile: restliche Spielezeit
        //Restlichen Zeilen: Spiele (Prozessnamen)

        Socket sock; //Der Socket für die Datenübertragung
        public static Socket FileSock; //Socket für die Übertragung von Dateien
        public BackgroundWorker ReceiverThread = new BackgroundWorker(); //Hintergrundthread zum Erhalten von Kommandos
        public BackgroundWorker AsyncMSGBOX = new BackgroundWorker(); //Hintergrundthread für asyncrone MessageBoxen
        List<string> gamecfglist; //Spieleliste
        bool GTrunning = false; //Boolean der Anzeigt ob der Spieletimer läuft
        string SrvIP = "www.sus-gaming.de"; //Die Domain vom Server

        public void Launch()
        {
            InitThreads(); //Funktion die die Threads
            connect(); //Verbindung zum Server
            GetGameList(); //List die aktuellste Spieleliste aus der game.ks Datei
            InitTimers();  //Funktion die die Timer einstellt
        }

        private void InitTimers()
        {
            GameTimer.Elapsed += new ElapsedEventHandler(GTimer); //Funktion zuweisen die beim Tick vom GameTimer ausgeführt wird
            gamechecker.Elapsed += new ElapsedEventHandler(ChkGames); //Funktion zuweisen die beim Tick vom gamechecker ausgeführt wird
            GetGameList();
            TimeLimit = Convert.ToInt16(gamecfglist[0]); //Erste Zeile der Spieleliste (restliche Spielezeit) zum Integer konvertieren und dem Spielezeit Integer zuweisen
            GameTimer.Interval = 60000; //Tickinterval von GameTimer auf 1 Minute stellen
            gamechecker.Interval = 5000; //Tickinternal vom gamechecker auf 5 Sekunden stellen
            gamechecker.Start(); //Den gamechecker-Timer starten
        }

        public void InitThreads()
        {
            this.ReceiverThread.DoWork +=
                new DoWorkEventHandler(ReceiverThread_DoWork); //Funktion zuweisen die beim Starten des ReceiverThreads ausgeführt wird
            this.AsyncMSGBOX.DoWork +=
                new DoWorkEventHandler(asyncmsg_DoWork); //Funktion zuweisen die beim Starten der AsyncMSGBOX ausgeführt wird
        }
        private void GetGameList()
        {
            gamecfglist = File.ReadAllLines(gamecfgpath).ToList(); //Alle Zeilen der game.ks einlesen und in der Spieleliste einfügen
        }
        
        public void connect()
        {
            IPHostEntry host = Dns.GetHostEntry(SrvIP); //Domain auflösen und die erhaltenden IPs der HostEntry zuweisen
            IPAddress ipAddress = host.AddressList[0]; //Erste IP der IP-Adressen Variable zuweisen
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000); //IPEndPoint mit der IP-Adresse und dem Port erstellen
            sock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //TCP Socket erstellen
            sock.Connect(remoteEP); //Verbindung zum Server aufbauen
            ReceiverThread.RunWorkerAsync(); //ReceiverThread starten
        }
        private void ReceiverThread_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                string data = ReceiveData(); //ReceiveData Funktion aufrufen und erhaltenden String dem neuen String "data" zuweisen
                chkcmd(data); //Funktion chkcmd mit Übergabewert "data" aufrufen
            } while (true);
        }
        private void asyncmsg_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show(e.Argument as string);
        }
        private string ReceiveData() //Funktion zum Empfangen von Strings
        {
            string data = null; //Leeren String erstellen
            byte[] bs = null; //Leeres Byte-Array erstellen
            bs = new byte[1024]; //Byte-Array auf 1024 Bytes erweitern
            int bsint = sock.Receive(bs); //Bytes vom Socket empfangen und in das Byte-Array schreiben & Anzahl der Bytes in Integer bsint schreiben
            data += Encoding.ASCII.GetString(bs, 0, bsint); //Bytes aus dem Array in einem String konvertieren
            return data; //Erhaltenden String zurückgeben
        }
        private void chkcmd(string data) //Funktion zum überprüfen, ob es sich bei dem String um ein Kommando handelt
        {
            try
            {
                if (data.Substring(0, 6) == "///CMD") //Falls die ersten 6 Zeichen des Strings "///CMD" sind, wird die FUnktion runcmd mit Übergabewert data aufgerufen
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

        private void runcmd(string data) //Funktion zum ausführen eines Kommandos
        {
            switch (data) //Führt das Kommando des Strings aus
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
                        Process.Start("shutdown", "/s /f /t 0"); //Führt den Windows Shutdown Befehl mit den Startoptionen /s (Shutdown), /f (Erzwingen) und /t 0 (in 0 Sekunden) aus
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
                case "///CMD_RC_FILE":
                    {
                        ReceiveFile();
                        break;
                    }
            }

        }
        private void StartProcess()
        {
            string processname = ReceiveData(); //Empfange Programmnamen bzw. Pfad + Programmnamen

            Process.Start(processname); //Programm starten
        }
        private void KillProcess()
        {
            string processname = ReceiveData(); //Empfange Prozessnamen

            Process[] Processes = Process.GetProcessesByName(processname); //Alle Prozesse mit den empfangenen Namen auflisten und in eine Processliste einfügen

            foreach (Process process in Processes) //Für jeden Prozess in der Liste beenden
                process.Kill();
        }
        private void BlockSite() //Funktion zum Blocken von Websiten
        {
            string site = ReceiveData(); //Empfange Websitedomain
            List<string> hostsfile = File.ReadAllLines(hostspath).ToList(); //Hosts-Datei einlesen und in eine Liste einfügen
            hostsfile.Add("127.0.0.1 " + site); //Die Loopback-IP und die Websitedomain in einer neuen Zeile zur Liste hinzufügen
            File.WriteAllLines(hostspath, hostsfile); //Die Hosts-Datei mit dem Listeninhalt überschreiben
            Process.Start("ipconfig", "/flushdns"); //Den DNS-Cache leeren
        }
        private void UnblockSite() //Funktion zum Entsperren von Websiten (Ähnlich wie BlockSite bloß mit einer Änderung
        {
            string site = ReceiveData();
            List<string> hostsfile = File.ReadAllLines(hostspath).ToList();
            hostsfile.Remove("127.0.0.1 " + site); //Entferne die blockierte Website aus der Liste
            File.WriteAllLines(hostspath, hostsfile);
            Process.Start("ipconfig", "/flushdns");
        }
        private void LimitTime() //Verändert die übrige Spielezeit
        {
            string time = ReceiveData(); //Eingestellte Spielezeit empfangen
            gamecfglist = File.ReadAllLines(gamecfgpath).ToList(); 
            gamecfglist[0] = time; //Die erste Zeile der Config mit der neuen Zeit ersetzen
            File.WriteAllLines(gamecfgpath, gamecfglist);
            TimeLimit = Convert.ToInt16(gamecfglist[0]); //Setze das TimeLimit fest
        }
        private void AddGame() //Funktion zum Hinzufügen eines Spiels in die Spieleliste
        {
            string game = ReceiveData(); 
            gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
            gamecfglist.Add(game); //Spielprozess in die GameList hinzufügen
            File.WriteAllLines(gamecfgpath, gamecfglist);
            SendGameList();
        }
        private void RemoveGame() //Funktion zum Entfernen eines Spiels aus die Spieleliste
        {
            string game = ReceiveData();
            gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
            gamecfglist.Remove(game); //Spielprozess aus der GameList entfernen
            File.WriteAllLines(gamecfgpath, gamecfglist);
            SendGameList();
        }
        private void SendGameList() //Sendet die Spieleliste zum Sender
        {
            int gamecount = gamecfglist.Count() - 1; //Zählt die Anzahl der Spiele
            SendData(gamecount.ToString()); //Sendet die Anzahl der Spiele an den Sender
            for (int i = 1; i <= gamecount; i++) //Loop der jeden Spielprozess einzeln an den Sender sendet
            {
                SendData(gamecfglist[i]);
                Thread.Sleep(100); //100ms Pause damit der Sender die einzelnen Namen unterscheiden kann
            }
        }
        private void ChkGames(object source, ElapsedEventArgs e) //Überprüft ob Spiele ausgeführt werden, und schließt sie falls die Zeit abgelaufen ist
        {
            var games = new List<Process>(); //Prozessliste für Spiele
            foreach (string gamename in gamecfglist) //Fügt alle laufenden Prozesse mit den Prozessnamen der game.ks in die Prozessliste
            {
                games.AddRange(Process.GetProcessesByName(gamename));
            }
            if (TimeLimit == 0 && games.Count > 0) //Falls ein Spiel läuft und das Zeitlimit abgelaufen ist, werden alle Spiele geschlossen
            {
                foreach (Process game in games)
                {
                    game.Kill();
                }
                AsyncMSGBOX.RunWorkerAsync("Spielzeit abgelaufen"); //Öffnet eine MessageBox mit der Nachricht "Spielzeit abgelaufen" ohne das Programm anzuhalten
            }
            else
            {
                if (games.Count > 0 && GTrunning == false) //Falls ein Spiel ausgeführt wird und der GameTimer nicht läuft, wird der GameTimer aktiviert
                {
                    GameTimer.Start();
                    GTrunning = true;
                }
                else if (games.Count == 0 && GTrunning == true) //Falls kein Spiel ausgeführt wird und der GameTimer läuft, wird der GameTimer deaktiviert
                {
                    GameTimer.Stop();
                    GTrunning = false;
                }
            }

        }
        private void SendData(string data) //Funktion um Text an den Sender bzw. Server zu senden
        {
            byte[] databytes = Encoding.ASCII.GetBytes(data); //Konvertiert den String zu Bytes
            sock.Send(databytes); //Sendet Bytes übers Socket
        }
        private void GTimer(object source, ElapsedEventArgs e) //Der Spieletimer der jede Minute eine Minute Spielzeit abzieht
        {
            if (TimeLimit > 0)
            {
                TimeLimit--; //Eine Minute der Spielzeit abziehen
                //Schreibt Restzeit in die game.ks
                List<string> gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
                gamecfglist[0] = TimeLimit.ToString();
                File.WriteAllLines(gamecfgpath, gamecfglist);
                //Schreibt Restzeit in die game.ks
            }
            else
            {
                GameTimer.Stop();
                GTrunning = false;
                //Schreibt Restzeit in die game.ks
                List<string> gamecfglist = File.ReadAllLines(gamecfgpath).ToList();
                gamecfglist[0] = TimeLimit.ToString();
                File.WriteAllLines(gamecfgpath, gamecfglist);
                //Schreibt Restzeit in die game.ks
            }

        }
        private void ReceiveFile()
        {
            long filesize = Convert.ToInt64(ReceiveData());
            string filename = ReceiveData();
            ConnectFileSock();
            var file = new List<byte>();
            byte[] buffer;
            do
            {
                buffer = new byte[1];
                FileSock.Receive(buffer);
                file.AddRange(buffer);
            } while (file.Count != filesize);

            byte[] rcbytes = file.ToArray();
            string savepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + filename;
            File.WriteAllBytes(savepath, rcbytes);
            
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
