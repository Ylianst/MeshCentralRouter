namespace MeshCentralRouter
{
    partial class AddRelayMapForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddRelayMapForm));
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
            this.remoteIpTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupComboBox = new System.Windows.Forms.ComboBox();
            this.udpRadioButton = new System.Windows.Forms.RadioButton();
            this.tcpRadioButton = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.localNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.remoteNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // localNumericUpDown
            // 
            resources.ApplyResources(this.localNumericUpDown, "localNumericUpDown");
            this.localNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.localNumericUpDown.Name = "localNumericUpDown";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // nodeComboBox
            // 
            resources.ApplyResources(this.nodeComboBox, "nodeComboBox");
            this.nodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nodeComboBox.FormattingEnabled = true;
            this.nodeComboBox.Name = "nodeComboBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // remoteNumericUpDown
            // 
            resources.ApplyResources(this.remoteNumericUpDown, "remoteNumericUpDown");
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
            this.remoteNumericUpDown.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // appComboBox
            // 
            resources.ApplyResources(this.appComboBox, "appComboBox");
            this.appComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.appComboBox.FormattingEnabled = true;
            this.appComboBox.Items.AddRange(new object[] {
            resources.GetString("appComboBox.Items"),
            resources.GetString("appComboBox.Items1"),
            resources.GetString("appComboBox.Items2"),
            resources.GetString("appComboBox.Items3"),
            resources.GetString("appComboBox.Items4"),
            resources.GetString("appComboBox.Items5")});
            this.appComboBox.Name = "appComboBox";
            this.appComboBox.SelectedIndexChanged += new System.EventHandler(this.appComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // remoteIpTextBox
            // 
            resources.ApplyResources(this.remoteIpTextBox, "remoteIpTextBox");
            this.remoteIpTextBox.Name = "remoteIpTextBox";
            this.remoteIpTextBox.TextChanged += new System.EventHandler(this.remoteIpTextBox_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupComboBox
            // 
            resources.ApplyResources(this.groupComboBox, "groupComboBox");
            this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupComboBox.FormattingEnabled = true;
            this.groupComboBox.Name = "groupComboBox";
            this.groupComboBox.SelectedIndexChanged += new System.EventHandler(this.groupComboBox_SelectedIndexChanged);
            // 
            // udpRadioButton
            // 
            resources.ApplyResources(this.udpRadioButton, "udpRadioButton");
            this.udpRadioButton.Name = "udpRadioButton";
            this.udpRadioButton.UseVisualStyleBackColor = true;
            // 
            // tcpRadioButton
            // 
            resources.ApplyResources(this.tcpRadioButton, "tcpRadioButton");
            this.tcpRadioButton.Checked = true;
            this.tcpRadioButton.Name = "tcpRadioButton";
            this.tcpRadioButton.TabStop = true;
            this.tcpRadioButton.UseVisualStyleBackColor = true;
            this.tcpRadioButton.CheckedChanged += new System.EventHandler(this.tcpRadioButton_CheckedChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // AddRelayMapForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.udpRadioButton);
            this.Controls.Add(this.tcpRadioButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupComboBox);
            this.Controls.Add(this.remoteIpTextBox);
            this.Controls.Add(this.label5);
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddRelayMapForm";
            this.Load += new System.EventHandler(this.AddRelayMapForm_Load);
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
        private System.Windows.Forms.TextBox remoteIpTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox groupComboBox;
        private System.Windows.Forms.RadioButton udpRadioButton;
        private System.Windows.Forms.RadioButton tcpRadioButton;
        private System.Windows.Forms.Label label7;
    }
}