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

        private void Kindersicherung_FormClosing(object sender, FormClosingEventArgs e) //Überprüft, ob das Programm wirklich geschlossen werden soll
        {
            DialogResult result = MessageBox.Show("Möchten Sie das Programm schließen?", "Bestätigung", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); // Ja / Nein-Frage, ob man sich sicher ist
            if(result == DialogResult.No) 
            {
                e.Cancel = (result == DialogResult.No); //Wenn nein gedrückt wird, bleibt das Programm offen
            }
            else
            {
                Application.Exit(); //Wenn ja gedrückt wird, wird das Programm geschlossen
            }
        }

        private void btnBlockAccess_Click(object sender, EventArgs e) //Blockiert bestimmte Internetseiten
        {
            SendData("///CMD_BLOCK_ADRESS"); //Sendet den Befehl zum Blockieren einer Internetseite an den Empfänger
            string adress = txtBlockAccess.Text; 
            if (adress.Substring(0, 4) == "www.") //Überprüft, ob die ersten 4 Buchstaben der Adresse 'www.' sind
            {
                SendData(txtBlockAccess.Text); //Wenn dem so ist, wird der String an den Server geleitet 
            }
            else
            {
                SendData("www." + txtBlockAccess.Text); //Wenn dem nicht so ist, wird vor dem String ein 'www.' angehängt und an den Server geleitet
            }
        }

        private void btnRunProcess_Click(object sender, EventArgs e)
        {
            SendData("///CMD_RUN_PROCESS"); //Sendet den Befehl zum Öffnen eines Programms an den Server
            SendData(txtOpenFile.Text); //Übergibt das genaue Programm, welches geöffnet werden soll
        }

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            SendData("///CMD_UPLOAD_FILE");
        }

        private void btnLimitTime_Click(object sender, EventArgs e)
        {
            SendData("///CMD_LIMIT_TIME"); //Sendet den Befehl für das Zeitlimit von Videospielen an den Server
            SendData(txtLimitTime.Text); //Übergibt die Dauer des Limits in Minuten
        }

        public void CheckConnection(object source, ElapsedEventArgs e)
        {
            
        }

        private void btnKillProcess_Click(object sender, EventArgs e)
        {
            SendData("///CMD_KILL_PROCESS"); //Sendet den Befehl zum Beenden eines Programms an den Server
            SendData(txtTaskKill.Text); //Übergibt das genaue Programm, das beendet werden soll 
        }

        private void btnRemoveBlock_Click(object sender, EventArgs e)
        {
            SendData("///CMD_FREE_ADRESS"); //Sendet den Befehl zum Aufheben der Sperre für bestimmte Internetseiten an den Server
            string adress = txtRemoveBlock.Text;
            if (adress.Substring(0, 4) == "www.") //Überprüft, ob die ersten 4 Buchstaben der Adresse 'www.' sind
            {
                SendData(txtRemoveBlock.Text); //Wenn dem so ist, wird der String an den Server geleitet
            }
            else
            {
                SendData("www." + txtRemoveBlock.Text); //Wenn dem nicht so ist, wird vor dem String ein 'www.' angehängt und an den Server geleitet
            }
        }
        bool row = true; //Ein Boolean, der eine Reihenfolge bestimmt
        private void btnShowList_Click(object sender, EventArgs e) //Auf Knopfdrück wird die Liste der überwachten Spiele angezeigt und wieder versteckt
        {
            if (row == true) //Wenn die Reigenfolge auf true steht
            {
                SendData("///CMD_SEND_GL"); //Sendet die Anfrage der Spieleliste an den Serber
                GetGameList(); //Ruft die Funktion GetGameList auf
                Spiele.Visible = true; 
                txtBanGame.Visible = true; //macht sämtliche Dinge sichtbar
                btnBanGame.Visible = true;
                btnShowList.Text = "Schließen"; //Die Beschriftung des Buttons wird auf 'schließen' geändert
                row = false; //Setzt die Reihenfolge auf false
            }
            else //Wenn die Reihenfolge auf falsch steht
            {
                Spiele.Visible = false;
                txtBanGame.Visible = false; //macht sämtliche Dinge unsichtbar
                btnBanGame.Visible = false;
                btnShowList.Text = "Spiele zur Überwachung hinzufügen"; //Die Beschriftung des Buttons wird zurückgeändert
                row = true; //Setzt die Reihenfolge auf true
            }
        }
        private void GetGameList()
        {
            Spiele.Items.Clear(); //Leert die Listbox, in der die Spiele stehen
            var GameCount = ReceiveData(); //Speichert die Anzahl der überwachten Spiele 
            int GameCountInt = Convert.ToInt16(GameCount); 
            for (int i = 1; i <= GameCountInt; i++) //Führt eine Loop aus, die so lange durchzählt, bis i größer oder gleichgroß wie die Anzahl der Spiele ist
            {
                Spiele.Items.Add(ReceiveData()); //Schreibt die aktuell überwachten Spiele nacheinander in die Listbox
            }
        }

        private void btnBanGame_Click(object sender, EventArgs e)
        {
            SendData("///CMD_GAME_ADD"); //Sendet den Befehl zum Hinzufügen eines Spieles an den Server
            SendData(txtBanGame.Text); //Sendet die Bezeichnung des Spieles an den Server
            txtBanGame.Text = ""; //Leert das Textfeld
            GetGameList(); //Aktualisiert die Listbox
        }

        private void Spiele_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendData("///CMD_GAME_RM"); //Sendet den Befehl zum Entfernen eines Spiels aus der Überwachung an den Server
            SendData(Spiele.SelectedItem.ToString()); //Convertiert die Bezeichnung des Spiels aus der Listbox in einen String und sendet diesen an den Server
            GetGameList(); //Aktualisiert die Listbox
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
