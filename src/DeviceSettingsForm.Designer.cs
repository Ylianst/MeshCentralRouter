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
            this.checkForUpdatedCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.checkForUpdatedCheckBox);
            this.groupBox1.Controls.Add(this.systemTrayCheckBox);
            this.groupBox1.Controls.Add(this.doubleClickComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // systemTrayCheckBox
            // 
            resources.ApplyResources(this.systemTrayCheckBox, "systemTrayCheckBox");
            this.systemTrayCheckBox.Name = "systemTrayCheckBox";
            this.systemTrayCheckBox.UseVisualStyleBackColor = true;
            // 
            // doubleClickComboBox
            // 
            this.doubleClickComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.doubleClickComboBox.FormattingEnabled = true;
            this.doubleClickComboBox.Items.AddRange(new object[] {
            resources.GetString("doubleClickComboBox.Items"),
            resources.GetString("doubleClickComboBox.Items1"),
            resources.GetString("doubleClickComboBox.Items2"),
            resources.GetString("doubleClickComboBox.Items3"),
            resources.GetString("doubleClickComboBox.Items4"),
            resources.GetString("doubleClickComboBox.Items5"),
            resources.GetString("doubleClickComboBox.Items6"),
            resources.GetString("doubleClickComboBox.Items7")});
            resources.ApplyResources(this.doubleClickComboBox, "doubleClickComboBox");
            this.doubleClickComboBox.Name = "doubleClickComboBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // exp_KeyboardHookCheckBox
            // 
            resources.ApplyResources(this.exp_KeyboardHookCheckBox, "exp_KeyboardHookCheckBox");
            this.exp_KeyboardHookCheckBox.Name = "exp_KeyboardHookCheckBox";
            this.exp_KeyboardHookCheckBox.UseVisualStyleBackColor = true;
            this.exp_KeyboardHookCheckBox.CheckedChanged += new System.EventHandler(this.exp_KeyboardHookCheckBox_CheckedChanged);
            // 
            // exp_KeyboardHookPriorityCheckBox
            // 
            resources.ApplyResources(this.exp_KeyboardHookPriorityCheckBox, "exp_KeyboardHookPriorityCheckBox");
            this.exp_KeyboardHookPriorityCheckBox.Name = "exp_KeyboardHookPriorityCheckBox";
            this.exp_KeyboardHookPriorityCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.exp_KeyboardHookPriorityCheckBox);
            this.groupBox2.Controls.Add(this.exp_KeyboardHookCheckBox);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkForUpdatedCheckBox
            // 
            resources.ApplyResources(this.checkForUpdatedCheckBox, "checkForUpdatedCheckBox");
            this.checkForUpdatedCheckBox.Name = "checkForUpdatedCheckBox";
            this.checkForUpdatedCheckBox.UseVisualStyleBackColor = true;
            // 
            // DeviceSettingsForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceSettingsForm";
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
        private System.Windows.Forms.CheckBox checkForUpdatedCheckBox;
    }
}