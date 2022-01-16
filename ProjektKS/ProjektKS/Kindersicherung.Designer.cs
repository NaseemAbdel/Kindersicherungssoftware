
namespace ProjektKS
{
    partial class Kindersicherung
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kindersicherung));
            this.btnShutdown = new System.Windows.Forms.Button();
            this.txtBlockAccess = new System.Windows.Forms.TextBox();
            this.ReceiverThread = new System.ComponentModel.BackgroundWorker();
            this.btnBlockAccess = new System.Windows.Forms.Button();
            this.btnRunProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLimitTime = new System.Windows.Forms.TextBox();
            this.btnLimitTime = new System.Windows.Forms.Button();
            this.pbConnected = new System.Windows.Forms.PictureBox();
            this.lblConnected = new System.Windows.Forms.Label();
            this.lblNotConnected = new System.Windows.Forms.Label();
            this.pbNotConnected = new System.Windows.Forms.PictureBox();
            this.txtOpenFile = new System.Windows.Forms.TextBox();
            this.txtTaskKill = new System.Windows.Forms.TextBox();
            this.btnKillProcess = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnected)).BeginInit();
            this.SuspendLayout();
            // 
            // btnShutdown
            // 
            this.btnShutdown.BackColor = System.Drawing.Color.Gray;
            this.btnShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShutdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnShutdown.Location = new System.Drawing.Point(12, 68);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(113, 36);
            this.btnShutdown.TabIndex = 0;
            this.btnShutdown.Text = "PC herunterfahren";
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // txtBlockAccess
            // 
            this.txtBlockAccess.BackColor = System.Drawing.Color.Gray;
            this.txtBlockAccess.Location = new System.Drawing.Point(12, 110);
            this.txtBlockAccess.Name = "txtBlockAccess";
            this.txtBlockAccess.Size = new System.Drawing.Size(113, 23);
            this.txtBlockAccess.TabIndex = 1;
            // 
            // btnBlockAccess
            // 
            this.btnBlockAccess.BackColor = System.Drawing.Color.Gray;
            this.btnBlockAccess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBlockAccess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnBlockAccess.Location = new System.Drawing.Point(131, 110);
            this.btnBlockAccess.Name = "btnBlockAccess";
            this.btnBlockAccess.Size = new System.Drawing.Size(115, 23);
            this.btnBlockAccess.TabIndex = 2;
            this.btnBlockAccess.Text = "Adresse Blockieren";
            this.btnBlockAccess.UseVisualStyleBackColor = false;
            this.btnBlockAccess.Click += new System.EventHandler(this.btnBlockAccess_Click);
            // 
            // btnRunProcess
            // 
            this.btnRunProcess.BackColor = System.Drawing.Color.Gray;
            this.btnRunProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRunProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnRunProcess.Location = new System.Drawing.Point(133, 147);
            this.btnRunProcess.Name = "btnRunProcess";
            this.btnRunProcess.Size = new System.Drawing.Size(113, 23);
            this.btnRunProcess.TabIndex = 3;
            this.btnRunProcess.Text = "Programm starten";
            this.btnRunProcess.UseVisualStyleBackColor = false;
            this.btnRunProcess.Click += new System.EventHandler(this.btnRunProcess_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(12, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Zeitlimit für Spiele in Minuten";
            // 
            // txtLimitTime
            // 
            this.txtLimitTime.BackColor = System.Drawing.Color.Gray;
            this.txtLimitTime.Location = new System.Drawing.Point(12, 284);
            this.txtLimitTime.Name = "txtLimitTime";
            this.txtLimitTime.Size = new System.Drawing.Size(30, 23);
            this.txtLimitTime.TabIndex = 6;
            // 
            // btnLimitTime
            // 
            this.btnLimitTime.BackColor = System.Drawing.Color.Gray;
            this.btnLimitTime.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLimitTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnLimitTime.Location = new System.Drawing.Point(48, 284);
            this.btnLimitTime.Name = "btnLimitTime";
            this.btnLimitTime.Size = new System.Drawing.Size(77, 23);
            this.btnLimitTime.TabIndex = 7;
            this.btnLimitTime.Text = "Speichern";
            this.btnLimitTime.UseVisualStyleBackColor = false;
            this.btnLimitTime.Click += new System.EventHandler(this.btnLimitTime_Click);
            // 
            // pbConnected
            // 
            this.pbConnected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbConnected.Image = global::ProjektKS.Properties.Resources.greencheck;
            this.pbConnected.Location = new System.Drawing.Point(302, 69);
            this.pbConnected.Name = "pbConnected";
            this.pbConnected.Size = new System.Drawing.Size(187, 177);
            this.pbConnected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbConnected.TabIndex = 8;
            this.pbConnected.TabStop = false;
            this.pbConnected.Visible = false;
            // 
            // lblConnected
            // 
            this.lblConnected.AutoSize = true;
            this.lblConnected.ForeColor = System.Drawing.Color.Lime;
            this.lblConnected.Location = new System.Drawing.Point(342, 253);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(99, 15);
            this.lblConnected.TabIndex = 9;
            this.lblConnected.Text = "Verbindung stabil";
            this.lblConnected.Visible = false;
            // 
            // lblNotConnected
            // 
            this.lblNotConnected.AutoSize = true;
            this.lblNotConnected.ForeColor = System.Drawing.Color.Red;
            this.lblNotConnected.Location = new System.Drawing.Point(323, 253);
            this.lblNotConnected.Name = "lblNotConnected";
            this.lblNotConnected.Size = new System.Drawing.Size(141, 15);
            this.lblNotConnected.TabIndex = 10;
            this.lblNotConnected.Text = "Verbindung abgebrochen";
            this.lblNotConnected.Visible = false;
            // 
            // pbNotConnected
            // 
            this.pbNotConnected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbNotConnected.Image = ((System.Drawing.Image)(resources.GetObject("pbNotConnected.Image")));
            this.pbNotConnected.Location = new System.Drawing.Point(302, 68);
            this.pbNotConnected.Name = "pbNotConnected";
            this.pbNotConnected.Size = new System.Drawing.Size(187, 177);
            this.pbNotConnected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbNotConnected.TabIndex = 11;
            this.pbNotConnected.TabStop = false;
            this.pbNotConnected.Visible = false;
            // 
            // txtOpenFile
            // 
            this.txtOpenFile.BackColor = System.Drawing.Color.Gray;
            this.txtOpenFile.Location = new System.Drawing.Point(12, 147);
            this.txtOpenFile.Name = "txtOpenFile";
            this.txtOpenFile.Size = new System.Drawing.Size(113, 23);
            this.txtOpenFile.TabIndex = 12;
            // 
            // txtTaskKill
            // 
            this.txtTaskKill.BackColor = System.Drawing.Color.Gray;
            this.txtTaskKill.Location = new System.Drawing.Point(12, 185);
            this.txtTaskKill.Name = "txtTaskKill";
            this.txtTaskKill.Size = new System.Drawing.Size(113, 23);
            this.txtTaskKill.TabIndex = 14;
            // 
            // btnKillProcess
            // 
            this.btnKillProcess.BackColor = System.Drawing.Color.Gray;
            this.btnKillProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnKillProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnKillProcess.Location = new System.Drawing.Point(133, 185);
            this.btnKillProcess.Name = "btnKillProcess";
            this.btnKillProcess.Size = new System.Drawing.Size(113, 23);
            this.btnKillProcess.TabIndex = 13;
            this.btnKillProcess.Text = "Programm starten";
            this.btnKillProcess.UseVisualStyleBackColor = false;
            this.btnKillProcess.Click += new System.EventHandler(this.btnKillProcess_Click);
            // 
            // Kindersicherung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtTaskKill);
            this.Controls.Add(this.btnKillProcess);
            this.Controls.Add(this.txtOpenFile);
            this.Controls.Add(this.pbNotConnected);
            this.Controls.Add(this.lblNotConnected);
            this.Controls.Add(this.lblConnected);
            this.Controls.Add(this.pbConnected);
            this.Controls.Add(this.btnLimitTime);
            this.Controls.Add(this.txtLimitTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRunProcess);
            this.Controls.Add(this.btnBlockAccess);
            this.Controls.Add(this.txtBlockAccess);
            this.Controls.Add(this.btnShutdown);
            this.Name = "Kindersicherung";
            this.Text = "Kindersicherung";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Kindersicherung_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbConnected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnected)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.TextBox txtBlockAccess;
        public System.ComponentModel.BackgroundWorker ReceiverThread;
        private System.Windows.Forms.Button btnBlockAccess;
        private System.Windows.Forms.Button btnRunProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLimitTime;
        private System.Windows.Forms.Button btnLimitTime;
        private System.Windows.Forms.PictureBox pbConnected;
        private System.Windows.Forms.Label lblConnected;
        private System.Windows.Forms.Label lblNotConnected;
        private System.Windows.Forms.PictureBox pbNotConnected;
        private System.Windows.Forms.TextBox txtOpenFile;
        private System.Windows.Forms.TextBox txtTaskKill;
        private System.Windows.Forms.Button btnKillProcess;
    }
}