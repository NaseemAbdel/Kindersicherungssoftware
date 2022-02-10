﻿using System;
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

        private void EFW_DoWork(object sender, DoWorkEventArgs e)
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
        private void SFW_DoWork(object sender, DoWorkEventArgs e)
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
            IPHostEntry host = Dns.GetHostEntry(SrvIP);
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            listensock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listensock.Bind(remoteEP);
            listensock.Listen(1);
            sock = listensock.Accept();
            Console.WriteLine("Empfänger verbunden");
            ecchkTimer.Start();
            Econnected = true;
            EFW.RunWorkerAsync();
        }
        private void SSock_DoWork(object sender, DoWorkEventArgs e)
        {
            IPHostEntry host = Dns.GetHostEntry(SrvIP);
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11001);
            listensock2 = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listensock2.Bind(remoteEP);
            listensock2.Listen(1);
            sock2 = listensock2.Accept();
            Console.WriteLine("Sender verbunden");
            ChkPW();
        }
        private void SSockReconnect()
        {
            listensock2.Listen(1);
            sock2 = listensock2.Accept();
            Console.WriteLine("Sender verbunden");
            ChkPW();
        }
        private void ESockReconnect()
        {
            listensock.Listen(1);
            sock = listensock.Accept();
            Console.WriteLine("Empfänger verbunden");
            ecchkTimer.Start();
            Econnected = true;
            EFW.RunWorkerAsync();
        }
        private void ChkPW()
        {
            
            int trycount = 0;
            bool auth = false;
            string username = "Eltern";
            string password = "fktkacken";
            scchkTimer.Start();
            do
                {
                    string receivedUN = ReceiveData(sock2);
                    string receivedPW = ReceiveData(sock2);
                
                if (receivedUN == "" || receivedPW == "")
                {
                    Console.WriteLine("Operation 4 fehlgeschlagen");
                    Console.WriteLine("Verbindung zum Sender getrennt");
                    SSockReconnect();
                    return;
                }
                    if (username == receivedUN && password == receivedPW)
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
            
        
        private string ReceiveData(Socket rcsocket)
        {
            try
            {
                string data = null;
                byte[] bs = null;
                bs = new byte[1024];
                int bsint = rcsocket.Receive(bs);
                data += Encoding.ASCII.GetString(bs, 0, bsint);
                return data;
            }
            catch
            {
                Console.WriteLine("Operation 2 fehlgeschlagen");
                return "";
            }
        }
        private void SendData(Socket sndsocket, string data)
        {
            try
            {
                byte[] databytes = Encoding.ASCII.GetBytes(data);
                sndsocket.Send(databytes);
            }
            catch
            {
                Console.WriteLine("Operation 3 fehlgeschlagen");
               
            }
        }
        private void scchk(object source, ElapsedEventArgs e)
        {
            if (sock2.Connected == false)
            {
                Console.WriteLine("Verbindung zum Sender getrennt");
                scchkTimer.Stop();
                SSockReconnect();
            }
            
        }
        private void ecchk(object source, ElapsedEventArgs e)
        {
            if (sock.Connected == false)
            {
                Console.WriteLine("Verbindung zum Empfänger getrennt");
                ecchkTimer.Stop();
                Econnected = false;
                ESockReconnect();
            }

        }
        private void FileToEmpfänger()
        {
            long filesize = Convert.ToInt64(ReceiveData(sock2));
            string filename = ReceiveData(sock2);
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
            SendData(sock, "///CMD_RC_FILE");
            Thread.Sleep(100);
            SendData(sock, filesize.ToString());
            Thread.Sleep(100);
            SendData(sock, filename);
            Console.WriteLine("Reconnecting Socket");
            ReconnectFileSock();
            Console.WriteLine("Sending Bytes");
            byte[] sndbyte = file.ToArray();
            FileSock.Send(sndbyte);
            FileSock.Close();
            Console.WriteLine("Transfer Complete");
            
        }
        private void FileToSender()
        {
            string filename = ReceiveData(sock2);
            long filesize = Convert.ToInt64(ReceiveData(sock));
            
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
            SendData(sock2, "///CMD_RC_FILE2");
            Thread.Sleep(100);
            SendData(sock2, filesize.ToString());
            Thread.Sleep(100);
            SendData(sock2, filename);
            Console.WriteLine("Reconnecting Socket");
            ReconnectFileSock();
            Console.WriteLine("Sending Bytes");
            byte[] sndbyte = file.ToArray();
            FileSock.Send(sndbyte);
            FileSock.Close();
            Console.WriteLine("Transfer Complete");

        }
        private void CreateFileSock()
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
        private void ReconnectFileSock()
        {
            listenfilesock.Listen(1);
            FileSock = listenfilesock.Accept();
        }

    }
}
