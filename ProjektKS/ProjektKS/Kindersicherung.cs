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
using System.IO;
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
            SendData("///CMD_SHUTDOWN"); //Sendet den Befehl zum Herunterfahren des PC's an den Server
        }

        public void ReceiverThread_DoWork(object sender, DoWorkEventArgs e) //War eine Testfunktion (wird aktuell nicht gebraucht)
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
            bs = new byte[1024]; //Neues Byte-Array mit einer größe von 1024 Bytes
            int bsint = sock.Receive(bs); //Anzahl der Bytes werden in einem Integer gespeichert
            data += Encoding.ASCII.GetString(bs, 0, bsint); //Die Bytes werden zu einem String decodiert
            return data;
        }
        public void SendData(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg); //Der übergebene String wird in Bytes konvertiert und in ein Byte-Array geschrieben
            sock.Send(data); //Das Byte-Array wird an den Server gesendet
        }
        public void SrvConnect()
        {
            Control.CheckForIllegalCrossThreadCalls = false; //Überwachung, ob Async-Threads auf das Formular zugreifen wird deaktiviert 
            IPHostEntry host = Dns.GetHostEntry(SrvIP); //Die Server IP wird als IPHostEntry deklariert
            IPAddress ipAddress = host.AddressList[0]; //Die Server IP wird nun als IPAdresse gespeichert
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11001); //IP-Endpoint mit der IPAdresse und dem Port erstellen
            sock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //Es wird ein neuer Socket erstellt
            sock.Connect(remoteEP); //Der Socket verbindet sich
            tmrCHKConnection.Elapsed += new ElapsedEventHandler(CheckConnection);  //5 Sekunden Timer, der auf ein Signal des Empfängers reagiert  
            tmrCHKConnection.Interval = 5000;
            
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
            Thread.Sleep(100);
            long filesize = new System.IO.FileInfo(UploadFileSelector.FileName).Length;
            SendData(filesize.ToString());
            Thread.Sleep(100);
            SendData(UploadFileSelector.SafeFileName);
            ConnectFileSock();
            FileSock.SendFile(UploadFileSelector.FileName);
            FileSock.Close();
        }

        private void btnLimitTime_Click(object sender, EventArgs e)
        {
            SendData("///CMD_LIMIT_TIME"); //Sendet den Befehl für das Zeitlimit von Videospielen an den Server
            SendData(txtLimitTime.Text); //Übergibt die Dauer des Limits in Minuten
        }

        public void CheckConnection(object source, ElapsedEventArgs e)
        {
            SendData("///CMD_EconnectCHK"); //Sendet den Befehl zur Überprüfung der Verbindung des Emofängers an den Server
            string data = ReceiveData(); //Die Antwort des Servers wird in einem string gespeichert
            if (data == "///CMD_Econnected") //Wenn der Empfänger ein Signal sendet
            {
                lblConnected.Text = "Verbindung stabil"; //Text des Labels wird geändert
                lblConnected.ForeColor = Color.Green; //Textfarbe des Labels wird grün gemacht
                pbConnected.Visible = true; //Die grüne PictureBox wird sichtbar gemacht
                pbNotConnected.Visible = false; //Die rote PictureBox wird unsichtbar gemacht
            }
            else if (data == "///CMD_Edisconnected") //Wenn der Empfänger kein Signal sendet
            {
                lblConnected.Text = "Verbindung getrennt"; //Text des Labels wird geändert
                lblConnected.ForeColor = Color.Red; //Textfarbe des Labels wird rot gemacht
                pbConnected.Visible = false; //Die grüne PictureBox wird unsichtbar gemacht
                pbNotConnected.Visible = true; //Die rote PictureBox wird sichtbar gemacht
            }
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
            SendData("///CMD_RC_FILE"); //Sendet den Befehl zum Empfangen einer Datei an den Server
            Thread.Sleep(100); //100ms Auszeit, um Datenkollision zu verhindern und damit der Server die einzelnen Elemente erkennen kann
            long filesize = new System.IO.FileInfo(UploadFileSelector.FileName).Length; //Die größe der Datei wird in einem Long-Integer gespeichert, sodass auch größere Dateien übertragen werden können
            SendData(filesize.ToString()); //Die größe der Datei wird an den Server gesendet
            Thread.Sleep(100); //100ms Auszeit
            SendData(UploadFileSelector.SafeFileName); //Der Name der Datei wird an den Server übergeben
            ConnectFileSock(); //Ruft die Funktion auf, die den FileSocket verbindet
            FileSock.SendFile(UploadFileSelector.FileName); //Die eigentliche Datei wird übertragen
            FileSock.Close(); //Der FileSocket wird geschlossen
        }
        private void btnSelFile_Click(object sender, EventArgs e)
        {
            UploadFileSelector.ShowDialog(); //öffnet einen normalen Fileselector 
            txtUploadFile.Text = UploadFileSelector.FileName; 
        }
        private void ConnectFileSock()
        {
            IPHostEntry host = Dns.GetHostEntry(SrvIP); //Server IP wird als IPHostEntry deklariert
            IPAddress ipAddress = host.AddressList[0]; //Server IP wird nun als IPAdresse gespeichert
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11002); //IP-Endpoint mit der IPAdresse und dem Port erstellen
            FileSock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //Neuen Filesocket erstellen
            FileSock.Connect(remoteEP); //Der Filesocket verbindet sich  
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
        private void Ordner_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendData("///CMD_CD"); //Sendet den Befehl um alle Dateien innerhalb des ausgewählten Ordners anzuzeigen
            SendData(Ordner.SelectedItem.ToString()); //Übergibt den Namen des gewünschten Odners
            GetFolderList();
            GetDataList(); //Öffnet die Funktion, die die Dateien in eine Listbox schreibt
        }

        bool rowdl = true; //Reihenfolge, genau das selbe Prinzip wie bei den Spielen
        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            if(rowdl == true) 
            {
                SendData("///CMD_DL_FILE"); //Sendet Anfrage an Server um Ordner auf den Desktop anzusehen
                btnDownloadFile.Text = "Ordner anzeigen"; //Text des Knopfes ändern
                Ordner.Visible = true; //Ordner Listbox sichtbar machen
                Dateien.Visible = true; //Dateien Listbox sichtbar machen
                rowdl = false; //Reihenfolge auf falsch setzen
                GetFolderList(); //Ruft die Funktion 
                GetDataList();
            }
            else
            {
                btnDownloadFile.Text = "Ordner verstecken"; //Text des Knopfes ändern
                Ordner.Visible = false; //Ordner Listbox unsichtbar machen
                Dateien.Visible = false; //Dateien Listbox unsichtbar machen
                rowdl = true; //Reihenfolge auf true setzen
            }
        }
        private void GetFolderList()
        {
            Ordner.Items.Clear(); //Leert die Listbox, in der die Ordner stehen
            var FolderCount = ReceiveData(); //Speichert die Anzahl der Ordner
            int FolderCountInt = Convert.ToInt16(FolderCount);
            for (int i = 1; i <= FolderCountInt; i++) //Führt eine Loop aus, die so lange durchzählt, bis i größer oder gleichgroß wie die Anzahl der Ordner ist
            {
                Ordner.Items.Add(ReceiveData()); //Schreibt die Ordner in die Listbox
            }
        }
        private void GetDataList()
        {
            Dateien.Items.Clear(); //Leert die Listbox, in der die Dateien stehen
            var DataCount = ReceiveData(); //Speichert die Anzahl der Dateien
            int DataCountInt = Convert.ToInt16(DataCount);
            for (int i = 1; i <= DataCountInt; i++) //Führt eine Loop aus, die so lange durchzählt, bis i größer oder gleichgroß wie die Anzahl der Dateien ist
            {
                Dateien.Items.Add(ReceiveData()); //Schreibt die Dateinamen in die Listbox
            }
        }
        private void Dateien_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tmrCHKConnection.Stop();  //Beendet den Timer, der die Verbindung überprüft
            SendData("///CMD_RC_FILE2"); //Sendet den Befehl zum Empfangen einer Datei an den Server
            SendData(Dateien.SelectedItem.ToString()); //Konvertiert den Namen der ausgewählten Datei zu einem String und übergibt diesen an den Server
            if(ReceiveData() == "///CMD_READY") //Wenn der Server bereit ist
            {
                ReceiveFile(); //Öffnet die Funktion, die die Datei empfängt
            }
            tmrCHKConnection.Start(); //Startet den Timer, der die Verbindung überprüft
        }
    }
}
