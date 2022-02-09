
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
            this.pbNotConnected = new System.Windows.Forms.PictureBox();
            this.txtOpenFile = new System.Windows.Forms.TextBox();
            this.txtTaskKill = new System.Windows.Forms.TextBox();
            this.btnKillProcess = new System.Windows.Forms.Button();
            this.btnRemoveBlock = new System.Windows.Forms.Button();
            this.txtRemoveBlock = new System.Windows.Forms.TextBox();
            this.txtUploadFile = new System.Windows.Forms.TextBox();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.Spiele = new System.Windows.Forms.ListBox();
            this.btnShowList = new System.Windows.Forms.Button();
            this.txtBanGame = new System.Windows.Forms.TextBox();
            this.btnBanGame = new System.Windows.Forms.Button();
            this.btnSelFile = new System.Windows.Forms.Button();
            this.UploadFileSelector = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnected)).BeginInit();
            this.SuspendLayout();
            // 
            // btnShutdown
            // 
            this.btnShutdown.BackColor = System.Drawing.Color.Gray;
            this.btnShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShutdown.ForeColor = System.Drawing.Color.Red;
            this.btnShutdown.Location = new System.Drawing.Point(10, 51);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(97, 39);
            this.btnShutdown.TabIndex = 0;
            this.btnShutdown.Text = "PC herunterfahren";
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // txtBlockAccess
            // 
            this.txtBlockAccess.BackColor = System.Drawing.Color.Gray;
            this.txtBlockAccess.ForeColor = System.Drawing.Color.Red;
            this.txtBlockAccess.Location = new System.Drawing.Point(10, 95);
            this.txtBlockAccess.Name = "txtBlockAccess";
            this.txtBlockAccess.Size = new System.Drawing.Size(97, 20);
            this.txtBlockAccess.TabIndex = 1;
            // 
            // btnBlockAccess
            // 
            this.btnBlockAccess.BackColor = System.Drawing.Color.Gray;
            this.btnBlockAccess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBlockAccess.ForeColor = System.Drawing.Color.Red;
            this.btnBlockAccess.Location = new System.Drawing.Point(112, 95);
            this.btnBlockAccess.Name = "btnBlockAccess";
            this.btnBlockAccess.Size = new System.Drawing.Size(109, 20);
            this.btnBlockAccess.TabIndex = 2;
            this.btnBlockAccess.Text = "Adresse blockieren";
            this.btnBlockAccess.UseVisualStyleBackColor = false;
            this.btnBlockAccess.Click += new System.EventHandler(this.btnBlockAccess_Click);
            // 
            // btnRunProcess
            // 
            this.btnRunProcess.BackColor = System.Drawing.Color.Gray;
            this.btnRunProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRunProcess.ForeColor = System.Drawing.Color.Red;
            this.btnRunProcess.Location = new System.Drawing.Point(113, 147);
            this.btnRunProcess.Name = "btnRunProcess";
            this.btnRunProcess.Size = new System.Drawing.Size(108, 20);
            this.btnRunProcess.TabIndex = 3;
            this.btnRunProcess.Text = "Programm starten";
            this.btnRunProcess.UseVisualStyleBackColor = false;
            this.btnRunProcess.Click += new System.EventHandler(this.btnRunProcess_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(457, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Zeitlimit für Spiele in Minuten";
            // 
            // txtLimitTime
            // 
            this.txtLimitTime.BackColor = System.Drawing.Color.Gray;
            this.txtLimitTime.ForeColor = System.Drawing.Color.Red;
            this.txtLimitTime.Location = new System.Drawing.Point(457, 69);
            this.txtLimitTime.Name = "txtLimitTime";
            this.txtLimitTime.Size = new System.Drawing.Size(26, 20);
            this.txtLimitTime.TabIndex = 6;
            // 
            // btnLimitTime
            // 
            this.btnLimitTime.BackColor = System.Drawing.Color.Gray;
            this.btnLimitTime.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLimitTime.ForeColor = System.Drawing.Color.Red;
            this.btnLimitTime.Location = new System.Drawing.Point(488, 69);
            this.btnLimitTime.Name = "btnLimitTime";
            this.btnLimitTime.Size = new System.Drawing.Size(66, 20);
            this.btnLimitTime.TabIndex = 7;
            this.btnLimitTime.Text = "Speichern";
            this.btnLimitTime.UseVisualStyleBackColor = false;
            this.btnLimitTime.Click += new System.EventHandler(this.btnLimitTime_Click);
            // 
            // pbConnected
            // 
            this.pbConnected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbConnected.Image = global::ProjektKS.Properties.Resources.greencheck;
            this.pbConnected.Location = new System.Drawing.Point(259, 60);
            this.pbConnected.Name = "pbConnected";
            this.pbConnected.Size = new System.Drawing.Size(160, 153);
            this.pbConnected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbConnected.TabIndex = 8;
            this.pbConnected.TabStop = false;
            this.pbConnected.Visible = false;
            // 
            // lblConnected
            // 
            this.lblConnected.AutoSize = true;
            this.lblConnected.ForeColor = System.Drawing.Color.Lime;
            this.lblConnected.Location = new System.Drawing.Point(256, 216);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(173, 13);
            this.lblConnected.TabIndex = 9;
            this.lblConnected.Text = "Verbindung zm Empfänger getrennt";
            this.lblConnected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblConnected.Visible = false;
            // 
            // pbNotConnected
            // 
            this.pbNotConnected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbNotConnected.Image = ((System.Drawing.Image)(resources.GetObject("pbNotConnected.Image")));
            this.pbNotConnected.Location = new System.Drawing.Point(259, 60);
            this.pbNotConnected.Name = "pbNotConnected";
            this.pbNotConnected.Size = new System.Drawing.Size(160, 153);
            this.pbNotConnected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbNotConnected.TabIndex = 11;
            this.pbNotConnected.TabStop = false;
            this.pbNotConnected.Visible = false;
            // 
            // txtOpenFile
            // 
            this.txtOpenFile.BackColor = System.Drawing.Color.Gray;
            this.txtOpenFile.ForeColor = System.Drawing.Color.Red;
            this.txtOpenFile.Location = new System.Drawing.Point(10, 148);
            this.txtOpenFile.Name = "txtOpenFile";
            this.txtOpenFile.Size = new System.Drawing.Size(97, 20);
            this.txtOpenFile.TabIndex = 12;
            // 
            // txtTaskKill
            // 
            this.txtTaskKill.BackColor = System.Drawing.Color.Gray;
            this.txtTaskKill.ForeColor = System.Drawing.Color.Red;
            this.txtTaskKill.Location = new System.Drawing.Point(10, 174);
            this.txtTaskKill.Name = "txtTaskKill";
            this.txtTaskKill.Size = new System.Drawing.Size(97, 20);
            this.txtTaskKill.TabIndex = 14;
            // 
            // btnKillProcess
            // 
            this.btnKillProcess.BackColor = System.Drawing.Color.Gray;
            this.btnKillProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnKillProcess.ForeColor = System.Drawing.Color.Red;
            this.btnKillProcess.Location = new System.Drawing.Point(112, 174);
            this.btnKillProcess.Name = "btnKillProcess";
            this.btnKillProcess.Size = new System.Drawing.Size(117, 20);
            this.btnKillProcess.TabIndex = 13;
            this.btnKillProcess.Text = "Programm schließen";
            this.btnKillProcess.UseVisualStyleBackColor = false;
            this.btnKillProcess.Click += new System.EventHandler(this.btnKillProcess_Click);
            // 
            // btnRemoveBlock
            // 
            this.btnRemoveBlock.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveBlock.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveBlock.ForeColor = System.Drawing.Color.Red;
            this.btnRemoveBlock.Location = new System.Drawing.Point(112, 121);
            this.btnRemoveBlock.Name = "btnRemoveBlock";
            this.btnRemoveBlock.Size = new System.Drawing.Size(109, 20);
            this.btnRemoveBlock.TabIndex = 16;
            this.btnRemoveBlock.Text = "Adresse freigeben";
            this.btnRemoveBlock.UseVisualStyleBackColor = false;
            this.btnRemoveBlock.Click += new System.EventHandler(this.btnRemoveBlock_Click);
            // 
            // txtRemoveBlock
            // 
            this.txtRemoveBlock.BackColor = System.Drawing.Color.Gray;
            this.txtRemoveBlock.ForeColor = System.Drawing.Color.Red;
            this.txtRemoveBlock.Location = new System.Drawing.Point(10, 121);
            this.txtRemoveBlock.Name = "txtRemoveBlock";
            this.txtRemoveBlock.Size = new System.Drawing.Size(97, 20);
            this.txtRemoveBlock.TabIndex = 15;
            // 
            // txtUploadFile
            // 
            this.txtUploadFile.BackColor = System.Drawing.Color.Gray;
            this.txtUploadFile.ForeColor = System.Drawing.Color.Red;
            this.txtUploadFile.Location = new System.Drawing.Point(10, 200);
            this.txtUploadFile.Name = "txtUploadFile";
            this.txtUploadFile.Size = new System.Drawing.Size(97, 20);
            this.txtUploadFile.TabIndex = 18;
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.BackColor = System.Drawing.Color.Gray;
            this.btnUploadFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUploadFile.ForeColor = System.Drawing.Color.Red;
            this.btnUploadFile.Location = new System.Drawing.Point(181, 200);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(72, 20);
            this.btnUploadFile.TabIndex = 17;
            this.btnUploadFile.Text = "Hochladen";
            this.btnUploadFile.UseVisualStyleBackColor = false;
            this.btnUploadFile.Click += new System.EventHandler(this.btnUploadFile_Click);
            // 
            // Spiele
            // 
            this.Spiele.AccessibleName = "";
            this.Spiele.BackColor = System.Drawing.Color.Gray;
            this.Spiele.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Spiele.ForeColor = System.Drawing.Color.Red;
            this.Spiele.FormattingEnabled = true;
            this.Spiele.Location = new System.Drawing.Point(604, 95);
            this.Spiele.Name = "Spiele";
            this.Spiele.Size = new System.Drawing.Size(70, 104);
            this.Spiele.TabIndex = 19;
            this.Spiele.Tag = "";
            this.Spiele.Visible = false;
            this.Spiele.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Spiele_MouseDoubleClick);
            // 
            // btnShowList
            // 
            this.btnShowList.BackColor = System.Drawing.Color.Gray;
            this.btnShowList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowList.ForeColor = System.Drawing.Color.Red;
            this.btnShowList.Location = new System.Drawing.Point(486, 95);
            this.btnShowList.Name = "btnShowList";
            this.btnShowList.Size = new System.Drawing.Size(112, 50);
            this.btnShowList.TabIndex = 20;
            this.btnShowList.Text = "Spiele zur Überwachung hinzufügen";
            this.btnShowList.UseVisualStyleBackColor = false;
            this.btnShowList.Click += new System.EventHandler(this.btnShowList_Click);
            // 
            // txtBanGame
            // 
            this.txtBanGame.BackColor = System.Drawing.Color.Gray;
            this.txtBanGame.Location = new System.Drawing.Point(41, 328);
            this.txtBanGame.Name = "txtBanGame";
            this.txtBanGame.Size = new System.Drawing.Size(110, 20);
            this.txtBanGame.TabIndex = 21;
            this.txtBanGame.Visible = false;
            // 
            // btnBanGame
            // 
            this.btnBanGame.BackColor = System.Drawing.Color.Gray;
            this.btnBanGame.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBanGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnBanGame.Location = new System.Drawing.Point(41, 354);
            this.btnBanGame.Name = "btnBanGame";
            this.btnBanGame.Size = new System.Drawing.Size(110, 20);
            this.btnBanGame.TabIndex = 22;
            this.btnBanGame.Text = "Hinzufügen";
            this.btnBanGame.UseVisualStyleBackColor = false;
            this.btnBanGame.Visible = false;
            this.btnBanGame.Click += new System.EventHandler(this.btnBanGame_Click);
            // 
            // btnSelFile
            // 
            this.btnSelFile.BackColor = System.Drawing.Color.Gray;
            this.btnSelFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelFile.ForeColor = System.Drawing.Color.Red;
            this.btnSelFile.Location = new System.Drawing.Point(112, 200);
            this.btnSelFile.Name = "btnSelFile";
            this.btnSelFile.Size = new System.Drawing.Size(63, 20);
            this.btnSelFile.TabIndex = 23;
            this.btnSelFile.Text = "Browse";
            this.btnSelFile.UseVisualStyleBackColor = false;
            this.btnSelFile.Click += new System.EventHandler(this.btnSelFile_Click);
            // 
            // UploadFileSelector
            // 
            this.UploadFileSelector.FileName = "openFileDialog1";
            // 
            // Kindersicherung
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(686, 390);
            this.Controls.Add(this.btnSelFile);
            this.Controls.Add(this.btnBanGame);
            this.Controls.Add(this.txtBanGame);
            this.Controls.Add(this.btnShowList);
            this.Controls.Add(this.Spiele);
            this.Controls.Add(this.txtUploadFile);
            this.Controls.Add(this.btnUploadFile);
            this.Controls.Add(this.btnRemoveBlock);
            this.Controls.Add(this.txtRemoveBlock);
            this.Controls.Add(this.txtTaskKill);
            this.Controls.Add(this.btnKillProcess);
            this.Controls.Add(this.txtOpenFile);
            this.Controls.Add(this.pbNotConnected);
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
        private System.Windows.Forms.PictureBox pbNotConnected;
        private System.Windows.Forms.TextBox txtOpenFile;
        private System.Windows.Forms.TextBox txtTaskKill;
        private System.Windows.Forms.Button btnKillProcess;
        private System.Windows.Forms.Button btnRemoveBlock;
        private System.Windows.Forms.TextBox txtRemoveBlock;
        private System.Windows.Forms.TextBox txtUploadFile;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.ListBox Spiele;
        private System.Windows.Forms.Button btnShowList;
        private System.Windows.Forms.TextBox txtBanGame;
        private System.Windows.Forms.Button btnBanGame;
        private System.Windows.Forms.Button btnSelFile;
        private System.Windows.Forms.OpenFileDialog UploadFileSelector;
    }
}