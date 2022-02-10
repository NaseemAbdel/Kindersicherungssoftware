
namespace ProjektKS
{
    partial class LogMeIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPasswort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.cbPasswort = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 27.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(268, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Log In";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 20.25F);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(287, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.Gray;
            this.txtName.ForeColor = System.Drawing.Color.GhostWhite;
            this.txtName.Location = new System.Drawing.Point(294, 166);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(76, 20);
            this.txtName.TabIndex = 2;
            // 
            // txtPasswort
            // 
            this.txtPasswort.BackColor = System.Drawing.Color.Gray;
            this.txtPasswort.ForeColor = System.Drawing.Color.GhostWhite;
            this.txtPasswort.Location = new System.Drawing.Point(294, 231);
            this.txtPasswort.Name = "txtPasswort";
            this.txtPasswort.Size = new System.Drawing.Size(76, 20);
            this.txtPasswort.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 20.25F);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(270, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 37);
            this.label3.TabIndex = 3;
            this.label3.Text = "Passwort";
            // 
            // btnLogIn
            // 
            this.btnLogIn.BackColor = System.Drawing.Color.Gray;
            this.btnLogIn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLogIn.ForeColor = System.Drawing.Color.Red;
            this.btnLogIn.Location = new System.Drawing.Point(294, 257);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(75, 23);
            this.btnLogIn.TabIndex = 5;
            this.btnLogIn.Text = "einloggen";
            this.btnLogIn.UseVisualStyleBackColor = false;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // cbPasswort
            // 
            this.cbPasswort.AutoSize = true;
            this.cbPasswort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbPasswort.ForeColor = System.Drawing.Color.Red;
            this.cbPasswort.Location = new System.Drawing.Point(375, 234);
            this.cbPasswort.Name = "cbPasswort";
            this.cbPasswort.Size = new System.Drawing.Size(115, 17);
            this.cbPasswort.TabIndex = 6;
            this.cbPasswort.Text = "Passwort anzeigen";
            this.cbPasswort.UseVisualStyleBackColor = false;
            this.cbPasswort.CheckedChanged += new System.EventHandler(this.cbPasswort_CheckedChanged);
            // 
            // LogMeIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(686, 390);
            this.Controls.Add(this.cbPasswort);
            this.Controls.Add(this.btnLogIn);
            this.Controls.Add(this.txtPasswort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "LogMeIn";
            this.Text = "LogMeIn";
            this.Load += new System.EventHandler(this.LogMeIn_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPasswort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.CheckBox cbPasswort;
    }
}

