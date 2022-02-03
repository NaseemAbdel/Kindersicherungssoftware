using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace ProjektKS
{
    public partial class LogMeIn : Form
    {
        Kindersicherung kindersicherung = new Kindersicherung();
        public static Socket sock;
        public LogMeIn()
        {
            InitializeComponent();
            kindersicherung.SrvConnect(); //Stellt Verbindung mit Server her
            txtPasswort.PasswordChar = '*'; //Versteckt das Passwort beim Reinladen des Programms 
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            kindersicherung.SendData(txtName.Text); //Übergibt Namen und Passwort an den Server
            kindersicherung.SendData(txtPasswort.Text); 
            string loginanswer = kindersicherung.ReceiveData(); //Der Server vergleicht die Daten und sendet eine Antwort zurück
            if (loginanswer == "///CMD_AUTH_SUCCESSFUL") //LogIn = Erfolgreich
            {
                kindersicherung.Show(); //Öffnet das Hauptfenster
                Hide(); //Versteckt das aktuelle Fenster
                kindersicherung.ReceiverThread.RunWorkerAsync(); 
            }
            else if (loginanswer == "///CMD_AUTH_FAILED") //LogIn = Fehlgeschlagen
            {
                MessageBox.Show("ungültige Login-Daten","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); //Error Meldung 
                txtName.Text = ""; //Textfelder werden geleert
                txtPasswort.Text = "";
            }

        }

        private void cbPasswort_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPasswort.Checked) //Wenn die Checkbox geklickt wird, wird das Passwort sichtbar gemacht
            {
                txtPasswort.PasswordChar = '\0';
            }
            else //Wenn der Haken entfernt wird, wird das Passwort wieder versteckt
            {
                txtPasswort.PasswordChar = '*';
            }
        }

        private void LogMeIn_Load(object sender, EventArgs e)
        {

        }
    }
}
