using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Threading;

namespace Server
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
        public System.Timers.Timer scchkTimer = new System.Timers.Timer(); //Timer der auf Verbindungsunterbrechungen des Senders überprüft
        public System.Timers.Timer ecchkTimer = new System.Timers.Timer(); //Timer der auf Verbindungsunterbrechungen des Empfängers überprüft
        public BackgroundWorker EFW = new BackgroundWorker(); //Background-Thread zum Weiterleitung von Strings vom Empfänger zum Sender
        public BackgroundWorker SFW = new BackgroundWorker(); //Background-Thread zum Weiterleitung von Strings vom Sender zum Empfänger
        public BackgroundWorker ESock = new BackgroundWorker(); //Background-Thread der den Socket für den Empfänger erstellt
        public BackgroundWorker SSock = new BackgroundWorker(); //Background-Thread der den Socket für den Sender erstellt
        public bool Econnected = false; //Boolean der angibt ob der Empfänger verbunden ist
         
        
        Socket sock; //Socket für den Empfänger
        Socket listensock; //Temporärer Socket der auf Verbindung vom Empfänger wartet
        Socket sock2; //Socket für den Sender
        Socket listensock2; //Temporärer Socket der auf Verbindung vom Sender wartet
        Socket FileSock; //Socket für Dateienübertragung
        Socket listenfilesock; //Temporärer Socket der auf Verbindung vom Empfänger bzw. Sender wartet
        string SrvIP = "192.168.178.122"; //Die IP des Raspberrys in meinem Netzwerk
        bool FileSockInit = false; //Boolean der angibt ob der Socket für die Dateiübertragung bereits erstellt wurde

        public void Launch()
        {
            InitThreads();
            ESock.RunWorkerAsync();
            SSock.RunWorkerAsync();
        }
        public void InitThreads() //Stellt die Threads und Timer ein
        {
            this.EFW.DoWork +=
                new DoWorkEventHandler(EFW_DoWork);
            this.SFW.DoWork +=
                new DoWorkEventHandler(SFW_DoWork);
            this.ESock.DoWork +=
                new DoWorkEventHandler(ESock_DoWork);
            this.SSock.DoWork +=
                new DoWorkEventHandler(SSock_DoWork);
            scchkTimer.Elapsed += new ElapsedEventHandler(scchk);
            scchkTimer.Interval = 3000;
            ecchkTimer.Elapsed += new ElapsedEventHandler(ecchk);
            ecchkTimer.Interval = 3000;
        }

        private void EFW_DoWork(object sender, DoWorkEventArgs e) //Leitet Strings vom Empfänger an den Sender
        {
            do
            {
                try
                {
                    string data = ReceiveData(sock);
                    if (data == "")
                    {
                        return;
                    }
                    SendData(sock2, data);
                }
                catch
                {
                Console.WriteLine("Operation 6 fehlgeschlagen");
                }
            } while (true);
        }
        private void SFW_DoWork(object sender, DoWorkEventArgs e) //Verarbeitet die vom Sender geschickten Befehle, bzw. sendet sie an den Empfänger weiter
        {
            do
            {
                try
                {
                    string data = ReceiveData(sock2);
                    if (data == "")
                    {
                        return;
                    }
                    else if (data == "///CMD_RC_FILE")
                    {
                        FileToEmpfänger();
                    }
                    else if (data == "///CMD_RC_FILE2")
                    {
                        FileToSender();
                    }
                    else if (data == "///CMD_EconnectCHK")
                    {
                        if (Econnected == false)
                        {
                            SendData(sock2, "///CMD_Edisconnected");
                        }
                        else
                        {
                            SendData(sock2, "///CMD_Econnected");
                        }
                    }
                    else
                    {
                        SendData(sock, data);
                    }
                }
                catch
                {
                    Console.WriteLine("Operation 1 fehlgeschlagen");

                }
            } while (true);
            
        }
        private void ESock_DoWork(object sender, DoWorkEventArgs e)
        { 
            IPHostEntry host = Dns.GetHostEntry(SrvIP); //Server IP wird als IPHostEntry deklariert
            IPAddress ipAddress = host.AddressList[0]; //Server IP wird nun als IPAdresse gespeichert
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000); //IP-Endpoint mit der IPAdresse und dem Port erstellen
            listensock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //Neuen Socket erstellen
            listensock.Bind(remoteEP); //Verbindet den Endpunkt mit dem temporären Socket
            listensock.Listen(1); //Warte auf Verbindung
            sock = listensock.Accept();
            Console.WriteLine("Empfänger verbunden"); //Um zu in der Console zu sehen was der Server macht
            ecchkTimer.Start();
            Econnected = true;
            EFW.RunWorkerAsync();
        }
        private void SSock_DoWork(object sender, DoWorkEventArgs e)
        {
            IPHostEntry host = Dns.GetHostEntry(SrvIP); //Server IP wird als IPHostEntry deklariert
            IPAddress ipAddress = host.AddressList[0]; //Server IP wird nun als IPAdresse gespeichert
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11001); //IP-Endpoint mit der IPAdresse und dem Port erstellen
            listensock2 = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //Neuen Socket erstellen
            listensock2.Bind(remoteEP); //Verbindet den Endpunkt mit dem temporären Socket
            listensock2.Listen(1); //Warte auf Verbindung
            sock2 = listensock2.Accept();
            Console.WriteLine("Sender verbunden"); //Um zu in der Console zu sehen was der Server macht
            ChkPW();
        }
        private void SSockReconnect()
        {
            listensock2.Listen(1); //Warte auf Verbindung
            sock2 = listensock2.Accept();
            Console.WriteLine("Sender verbunden"); //Um zu in der Console zu sehen was der Server macht
            ChkPW();
        }
        private void ESockReconnect()
        {
            listensock.Listen(1); //Warte auf Verbindung
            sock = listensock.Accept();
            Console.WriteLine("Empfänger verbunden"); //Um zu in der Console zu sehen was der Server macht
            ecchkTimer.Start();
            Econnected = true;
            EFW.RunWorkerAsync();
        }
        private void ChkPW() //Funktion um die Anmeldedaten zu überprüfen
        {
            
            int trycount = 0;
            bool auth = false;
            string username = "Eltern";
            string password = "fktpasswort";
            scchkTimer.Start();
            do
                {
                    string receivedUN = ReceiveData(sock2);
                    string receivedPW = ReceiveData(sock2);
                
                if (receivedUN == "" || receivedPW == "") //Wenn das eingegebene Passwort und Nutzername leer ist
                {
                    Console.WriteLine("Operation 4 fehlgeschlagen");
                    Console.WriteLine("Verbindung zum Sender getrennt");
                    SSockReconnect();
                    return;
                }
                    if (username == receivedUN && password == receivedPW)  //Wenn die LogIn-Daten übereinstimmen
                    {
                        SendData(sock2, "///CMD_AUTH_SUCCESSFUL");
                        Console.WriteLine("Sender authentifiziert");
                        auth = true;
                        break;
                    }
                    else
                    {
                        SendData(sock2, "///CMD_AUTH_FAILED");
                        Console.WriteLine(trycount + ". Senderauthentifizierung fehlgeschlagen");

                        trycount += 1;
                    }
                } while (trycount < 3);


                if (auth == true)
                {
                    SFW.RunWorkerAsync();
                }
                else
                {
                    Console.WriteLine("Verbindung zum Sender getrennt (Authentifizierung fehlgeschlagen)");
                sock2.Close();
                }
            }
            
        
        private string ReceiveData(Socket rcsocket) //Funktion für das Empfangen von Daten
        {
            try
            {
                string data = null;
                byte[] bs = null;
                bs = new byte[1024]; //Neues Byte-Array mit einer größe von 1024 Bytes
                int bsint = rcsocket.Receive(bs); //Anzahl der Bytes werden in einem Integer gespeichert
                data += Encoding.ASCII.GetString(bs, 0, bsint); //Die Bytes werden zu einem String decodiert
                return data;
            }
            catch
            {
                Console.WriteLine("Operation 2 fehlgeschlagen");
                return "";
            }
        }
        private void SendData(Socket sndsocket, string data) //Funktion für das Senden von Daten
        {
            try
            {
                byte[] databytes = Encoding.ASCII.GetBytes(data); //Der übergebene String wird in Bytes konvertiert und in ein Byte-Array geschrieben
                sndsocket.Send(databytes); //Das Byte-Array wird an den Sender gesendet
            }
            catch
            {
                Console.WriteLine("Operation 3 fehlgeschlagen");
               
            }
        }
        private void scchk(object source, ElapsedEventArgs e) //Timergesteuerte Funktion die getrennte Verbindungen wiederherstellt
        {
            if (sock2.Connected == false) //Falls Verbindung zum Sender getrennt wurde, warte auf neue Verbindung
            {
                Console.WriteLine("Verbindung zum Sender getrennt");
                scchkTimer.Stop();
                SSockReconnect();
            }
            
        }
        private void ecchk(object source, ElapsedEventArgs e) //Das gleiche wie scchk, bloß für den Empfänger
        {
            if (sock.Connected == false)
            {
                Console.WriteLine("Verbindung zum Empfänger getrennt");
                ecchkTimer.Stop();
                Econnected = false;
                ESockReconnect();
            }

        }
        private void FileToEmpfänger() //Sendet Dateien zum Empfänger
        {
            long filesize = Convert.ToInt64(ReceiveData(sock2)); //Empfange Dateigröße
            string filename = ReceiveData(sock2); //Empfange Dateinamen
            Console.WriteLine("Creating Socket");
            if (FileSockInit == true) //Falls der FileSocket schon erstellt wurde, reconnecte es, ansonsten erstelle ein neues
            {
                ReconnectFileSock();
            }
            else
            {
                CreateFileSock();
            }
            
            Console.WriteLine("Receiving Bytes");
            var file = new List<byte>();
            byte[] buffer;
                do //Loop der die Bytes der Datei einzeln in eine Byte-Liste einfügt, bis alle Bytes übertragen worden sind
                {
                    buffer = new byte[1];
                    FileSock.Receive(buffer);
                    file.AddRange(buffer);
                } while (file.Count != filesize);
           
            
            FileSock.Close();
            SendData(sock, "///CMD_RC_FILE"); //Bereite Empfänger auf Dateiübertragung vor
            Thread.Sleep(100);
            SendData(sock, filesize.ToString()); //Sende Dateigröße zum Empfänger
            Thread.Sleep(100);
            SendData(sock, filename); //Sende Dateinamen zum Empfänger
            Console.WriteLine("Reconnecting Socket");
            ReconnectFileSock();
            Console.WriteLine("Sending Bytes");
            byte[] sndbyte = file.ToArray(); //Konvertiert Byte-Liste in ein sendbares Byte-Array
            FileSock.Send(sndbyte);
            FileSock.Close();
            Console.WriteLine("Transfer Complete");
            
        }
        private void FileToSender() //Das gleiche wie FileToEmpfänger bloß umgekehrt
        {
            string filename = ReceiveData(sock2);
            SendData(sock, "///CMD_RC_FILE2");
            Thread.Sleep(100);
            SendData(sock, filename);
            long filesize = Convert.ToInt64(ReceiveData(sock));
            string sfilename = ReceiveData(sock);
            Console.WriteLine("Creating Socket");
            if (FileSockInit == true)
            {
                ReconnectFileSock();
            }
            else
            {
                CreateFileSock();
            }

            Console.WriteLine("Receiving Bytes");
            var file = new List<byte>();
            byte[] buffer;
            do
            {
                buffer = new byte[1];
                FileSock.Receive(buffer);
                file.AddRange(buffer);
            } while (file.Count != filesize);


            FileSock.Close();
            SendData(sock2, "///CMD_READY"); //Sage Sender, dass der Server bereit zum Übertragen ist
            Thread.Sleep(100);
            SendData(sock2, filesize.ToString());
            Thread.Sleep(100);
            SendData(sock2, sfilename);
            Console.WriteLine("Reconnecting Socket");
            ReconnectFileSock();
            Console.WriteLine("Sending Bytes");
            byte[] sndbyte = file.ToArray();
            FileSock.Send(sndbyte);
            FileSock.Close();
            Console.WriteLine("Transfer Complete");

        }
        private void CreateFileSock() //Erstellt ein Socket für die Dateiübertragung
        {
            FileSockInit = true;
            IPHostEntry host = Dns.GetHostEntry(SrvIP);
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11002);
            listenfilesock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenfilesock.Bind(remoteEP);
            listenfilesock.Listen(1);
            FileSock = listenfilesock.Accept();
        }
        private void ReconnectFileSock() //FileSocket wieder auf Listening stellen
        {
            listenfilesock.Listen(1);
            FileSock = listenfilesock.Accept();
        }

    }
}
