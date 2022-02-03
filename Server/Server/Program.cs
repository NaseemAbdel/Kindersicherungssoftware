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
        public System.Timers.Timer scchkTimer = new System.Timers.Timer();
        public System.Timers.Timer ecchkTimer = new System.Timers.Timer();
        public BackgroundWorker EFW = new BackgroundWorker();
        public BackgroundWorker SFW = new BackgroundWorker();
        public BackgroundWorker ESock = new BackgroundWorker();
        public BackgroundWorker SSock = new BackgroundWorker();
         
        
        Socket sock;
        Socket listensock;
        Socket sock2;
        Socket listensock2;
        Socket FileSock;
        Socket listenfilesock;
        string SrvIP = "192.168.178.122";
        bool FileSockInit = false;

        public void Launch()
        {
            InitThreads();
            ESock.RunWorkerAsync();
            SSock.RunWorkerAsync();
        }
        public void InitThreads()
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
