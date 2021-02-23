namespace MeshCentralRouter
{
    partial class DeviceSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceSettingsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.systemTrayCheckBox = new System.Windows.Forms.CheckBox();
            this.doubleClickComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.exp_KeyboardHookCheckBox = new System.Windows.Forms.CheckBox();
            this.exp_KeyboardHookPriorityCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.systemTrayCheckBox);
            this.groupBox1.Controls.Add(this.doubleClickComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 97);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // systemTrayCheckBox
            // 
            this.systemTrayCheckBox.AutoSize = true;
            this.systemTrayCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.systemTrayCheckBox.Location = new System.Drawing.Point(14, 70);
            this.systemTrayCheckBox.Name = "systemTrayCheckBox";
            this.systemTrayCheckBox.Size = new System.Drawing.Size(123, 17);
            this.systemTrayCheckBox.TabIndex = 2;
            this.systemTrayCheckBox.Text = "Show on system tray";
            this.systemTrayCheckBox.UseVisualStyleBackColor = true;
            // 
            // doubleClickComboBox
            // 
            this.doubleClickComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.doubleClickComboBox.FormattingEnabled = true;
            this.doubleClickComboBox.Items.AddRange(new object[] {
            "Add Map...",
            "Add Relay Map...",
            "Remote Desktop...",
            "Remote Files...",
            "HTTP",
            "HTTPS",
            "SSH",
            "SCP"});
            this.doubleClickComboBox.Location = new System.Drawing.Point(14, 43);
            this.doubleClickComboBox.Name = "doubleClickComboBox";
            this.doubleClickComboBox.Size = new System.Drawing.Size(267, 21);
            this.doubleClickComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Double Click Action";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.okButton.Location = new System.Drawing.Point(150, 193);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cancelButton.Location = new System.Drawing.Point(231, 193);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // exp_KeyboardHookCheckBox
            // 
            this.exp_KeyboardHookCheckBox.AutoSize = true;
            this.exp_KeyboardHookCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.exp_KeyboardHookCheckBox.Location = new System.Drawing.Point(14, 22);
            this.exp_KeyboardHookCheckBox.Name = "exp_KeyboardHookCheckBox";
            this.exp_KeyboardHookCheckBox.Size = new System.Drawing.Size(161, 17);
            this.exp_KeyboardHookCheckBox.TabIndex = 3;
            this.exp_KeyboardHookCheckBox.Text = "Enhanced keyboard capture";
            this.exp_KeyboardHookCheckBox.UseVisualStyleBackColor = true;
            this.exp_KeyboardHookCheckBox.CheckedChanged += new System.EventHandler(this.exp_KeyboardHookCheckBox_CheckedChanged);
            // 
            // exp_KeyboardHookPriorityCheckBox
            // 
            this.exp_KeyboardHookPriorityCheckBox.AutoSize = true;
            this.exp_KeyboardHookPriorityCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.exp_KeyboardHookPriorityCheckBox.Location = new System.Drawing.Point(14, 45);
            this.exp_KeyboardHookPriorityCheckBox.Name = "exp_KeyboardHookPriorityCheckBox";
            this.exp_KeyboardHookPriorityCheckBox.Size = new System.Drawing.Size(149, 17);
            this.exp_KeyboardHookPriorityCheckBox.TabIndex = 4;
            this.exp_KeyboardHookPriorityCheckBox.Text = "Forward all keyboard keys";
            this.exp_KeyboardHookPriorityCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.exp_KeyboardHookPriorityCheckBox);
            this.groupBox2.Controls.Add(this.exp_KeyboardHookCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 72);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Desktop Settings";
            // 
            // DeviceSettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(318, 228);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox doubleClickComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox systemTrayCheckBox;
        private System.Windows.Forms.CheckBox exp_KeyboardHookCheckBox;
        private System.Windows.Forms.CheckBox exp_KeyboardHookPriorityCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}