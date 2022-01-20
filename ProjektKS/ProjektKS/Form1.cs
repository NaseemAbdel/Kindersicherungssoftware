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
            kindersicherung.SrvConnect();
            txtPasswort.PasswordChar = '*';
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            kindersicherung.SendData(txtName.Text);
            kindersicherung.SendData(txtPasswort.Text);
            string loginanswer = kindersicherung.ReceiveData();
            if (loginanswer == "///CMD_AUTH_SUCCESSFUL")
            {
                kindersicherung.Show();
                Hide();
                kindersicherung.ReceiverThread.RunWorkerAsync();
                kindersicherung.tmrCHKConnection.Start();
            }
            else if (loginanswer == "///CMD_AUTH_FAILED")
            {
                MessageBox.Show("ungültige Login-Daten");
                txtName.Text = "";
                txtPasswort.Text = "";
            }

        }

        private void cbPasswort_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPasswort.Checked)
            {
                txtPasswort.PasswordChar = '\0';
            }
            else
            {
                txtPasswort.PasswordChar = '*';
            }
        }
    }
}
