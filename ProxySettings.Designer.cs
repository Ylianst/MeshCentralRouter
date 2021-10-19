namespace MeshCentralRouter
{
    partial class ProxySettings
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
            this.SaveProxyConfig = new System.Windows.Forms.Button();
            this.manualHttpProxyHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.manualHttpProxyPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.manualHttpProxyUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.manualHttpProxyPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.useManualProxySettings = new System.Windows.Forms.CheckBox();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SaveProxyConfig
            // 
            this.SaveProxyConfig.Location = new System.Drawing.Point(124, 404);
            this.SaveProxyConfig.Name = "SaveProxyConfig";
            this.SaveProxyConfig.Size = new System.Drawing.Size(75, 23);
            this.SaveProxyConfig.TabIndex = 1;
            this.SaveProxyConfig.Text = "Ok";
            this.SaveProxyConfig.UseVisualStyleBackColor = true;
            this.SaveProxyConfig.Click += new System.EventHandler(this.SaveProxyConfig_Click);
            // 
            // manualHttpProxyHost
            // 
            this.manualHttpProxyHost.Location = new System.Drawing.Point(104, 141);
            this.manualHttpProxyHost.Name = "manualHttpProxyHost";
            this.manualHttpProxyHost.Size = new System.Drawing.Size(203, 22);
            this.manualHttpProxyHost.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Http proxy host";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // manualHttpProxyPort
            // 
            this.manualHttpProxyPort.Location = new System.Drawing.Point(104, 208);
            this.manualHttpProxyPort.Name = "manualHttpProxyPort";
            this.manualHttpProxyPort.Size = new System.Drawing.Size(203, 22);
            this.manualHttpProxyPort.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Http proxy port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Http proxy username";
            // 
            // manualHttpProxyUsername
            // 
            this.manualHttpProxyUsername.Location = new System.Drawing.Point(104, 286);
            this.manualHttpProxyUsername.Name = "manualHttpProxyUsername";
            this.manualHttpProxyUsername.Size = new System.Drawing.Size(203, 22);
            this.manualHttpProxyUsername.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Http proxy password";
            // 
            // manualHttpProxyPassword
            // 
            this.manualHttpProxyPassword.Location = new System.Drawing.Point(104, 362);
            this.manualHttpProxyPassword.Name = "manualHttpProxyPassword";
            this.manualHttpProxyPassword.PasswordChar = '*';
            this.manualHttpProxyPassword.Size = new System.Drawing.Size(203, 22);
            this.manualHttpProxyPassword.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(13, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(396, 69);
            this.label5.TabIndex = 10;
            this.label5.Text = "Manually configure an HTTP proxy server to use. Username and password are optiona" +
    "l. Only basic auth is supported.";
            this.label5.UseMnemonic = false;
            // 
            // useManualProxySettings
            // 
            this.useManualProxySettings.AutoSize = true;
            this.useManualProxySettings.Location = new System.Drawing.Point(43, 81);
            this.useManualProxySettings.Name = "useManualProxySettings";
            this.useManualProxySettings.Size = new System.Drawing.Size(194, 21);
            this.useManualProxySettings.TabIndex = 11;
            this.useManualProxySettings.Text = "use manual proxy settings";
            this.useManualProxySettings.UseVisualStyleBackColor = true;
            this.useManualProxySettings.CheckedChanged += new System.EventHandler(this.useManualProxySettings_CheckedChanged);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(232, 404);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 12;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ProxySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 450);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.useManualProxySettings);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.manualHttpProxyPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.manualHttpProxyUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.manualHttpProxyPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.manualHttpProxyHost);
            this.Controls.Add(this.SaveProxyConfig);
            this.Name = "ProxySettings";
            this.Text = "Proxy Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SaveProxyConfig;
        private System.Windows.Forms.TextBox manualHttpProxyHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox manualHttpProxyPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox manualHttpProxyUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox manualHttpProxyPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox useManualProxySettings;
        private System.Windows.Forms.Button cancel;
    }
}