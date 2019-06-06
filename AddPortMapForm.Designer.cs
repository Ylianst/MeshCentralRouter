namespace MeshCentralRouter
{
    partial class AddPortMapForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPortMapForm));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.localNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nodeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.remoteNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.appComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tcpRadioButton = new System.Windows.Forms.RadioButton();
            this.udpRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.localNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.remoteNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(288, 167);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(207, 167);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // localNumericUpDown
            // 
            this.localNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localNumericUpDown.Location = new System.Drawing.Point(157, 33);
            this.localNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.localNumericUpDown.Name = "localNumericUpDown";
            this.localNumericUpDown.Size = new System.Drawing.Size(206, 20);
            this.localNumericUpDown.TabIndex = 2;
            this.localNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Local Port";
            // 
            // nodeComboBox
            // 
            this.nodeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nodeComboBox.FormattingEnabled = true;
            this.nodeComboBox.Location = new System.Drawing.Point(157, 86);
            this.nodeComboBox.Name = "nodeComboBox";
            this.nodeComboBox.Size = new System.Drawing.Size(206, 21);
            this.nodeComboBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Remote Device";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Remote Port";
            // 
            // remoteNumericUpDown
            // 
            this.remoteNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remoteNumericUpDown.Location = new System.Drawing.Point(157, 140);
            this.remoteNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.remoteNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.remoteNumericUpDown.Name = "remoteNumericUpDown";
            this.remoteNumericUpDown.Size = new System.Drawing.Size(206, 20);
            this.remoteNumericUpDown.TabIndex = 6;
            this.remoteNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.remoteNumericUpDown.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Application";
            // 
            // appComboBox
            // 
            this.appComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.appComboBox.FormattingEnabled = true;
            this.appComboBox.Items.AddRange(new object[] {
            "Custom",
            "HTTP",
            "HTTPS",
            "RDP",
            "PuTTY",
            "WinSCP"});
            this.appComboBox.Location = new System.Drawing.Point(157, 113);
            this.appComboBox.Name = "appComboBox";
            this.appComboBox.Size = new System.Drawing.Size(206, 21);
            this.appComboBox.TabIndex = 5;
            this.appComboBox.SelectedIndexChanged += new System.EventHandler(this.appComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Device Group";
            // 
            // groupComboBox
            // 
            this.groupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FormattingEnabled = true;
            this.groupComboBox.Location = new System.Drawing.Point(157, 59);
            this.groupComboBox.Name = "groupComboBox";
            this.groupComboBox.Size = new System.Drawing.Size(206, 21);
            this.groupComboBox.TabIndex = 3;
            this.groupComboBox.SelectedIndexChanged += new System.EventHandler(this.groupComboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Protocol";
            // 
            // tcpRadioButton
            // 
            this.tcpRadioButton.AutoSize = true;
            this.tcpRadioButton.Checked = true;
            this.tcpRadioButton.Location = new System.Drawing.Point(157, 9);
            this.tcpRadioButton.Name = "tcpRadioButton";
            this.tcpRadioButton.Size = new System.Drawing.Size(46, 17);
            this.tcpRadioButton.TabIndex = 1;
            this.tcpRadioButton.TabStop = true;
            this.tcpRadioButton.Text = "TCP";
            this.tcpRadioButton.UseVisualStyleBackColor = true;
            this.tcpRadioButton.CheckedChanged += new System.EventHandler(this.tcpRadioButton_CheckedChanged);
            // 
            // udpRadioButton
            // 
            this.udpRadioButton.AutoSize = true;
            this.udpRadioButton.Location = new System.Drawing.Point(209, 9);
            this.udpRadioButton.Name = "udpRadioButton";
            this.udpRadioButton.Size = new System.Drawing.Size(48, 17);
            this.udpRadioButton.TabIndex = 1;
            this.udpRadioButton.Text = "UDP";
            this.udpRadioButton.UseVisualStyleBackColor = true;
            this.udpRadioButton.CheckedChanged += new System.EventHandler(this.tcpRadioButton_CheckedChanged);
            // 
            // AddPortMapForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(375, 202);
            this.Controls.Add(this.udpRadioButton);
            this.Controls.Add(this.tcpRadioButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.appComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.remoteNumericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nodeComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.localNumericUpDown);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddPortMapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Port Mapping";
            this.Load += new System.EventHandler(this.AddPortMapForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.localNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.remoteNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.NumericUpDown localNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox nodeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown remoteNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox appComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox groupComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton tcpRadioButton;
        private System.Windows.Forms.RadioButton udpRadioButton;
    }
}