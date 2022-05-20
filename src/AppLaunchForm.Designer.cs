namespace MeshCentralRouter
{
    partial class AppLaunchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppLaunchForm));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.applicationComboBox = new System.Windows.Forms.ComboBox();
            this.folderButton = new System.Windows.Forms.Button();
            this.appNameLabel = new System.Windows.Forms.Label();
            this.appLinkLabel = new System.Windows.Forms.LinkLabel();
            this.appPathTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
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
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.applicationComboBox);
            this.groupBox1.Controls.Add(this.folderButton);
            this.groupBox1.Controls.Add(this.appNameLabel);
            this.groupBox1.Controls.Add(this.appLinkLabel);
            this.groupBox1.Controls.Add(this.appPathTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // applicationComboBox
            // 
            resources.ApplyResources(this.applicationComboBox, "applicationComboBox");
            this.applicationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.applicationComboBox.FormattingEnabled = true;
            this.applicationComboBox.Name = "applicationComboBox";
            this.applicationComboBox.SelectedIndexChanged += new System.EventHandler(this.applicationComboBox_SelectedIndexChanged);
            // 
            // folderButton
            // 
            resources.ApplyResources(this.folderButton, "folderButton");
            this.folderButton.Image = global::MeshCentralRouter.Properties.Resources.Folder36;
            this.folderButton.Name = "folderButton";
            this.folderButton.UseVisualStyleBackColor = true;
            this.folderButton.Click += new System.EventHandler(this.folderPictureBox_Click);
            // 
            // appNameLabel
            // 
            resources.ApplyResources(this.appNameLabel, "appNameLabel");
            this.appNameLabel.Name = "appNameLabel";
            // 
            // appLinkLabel
            // 
            resources.ApplyResources(this.appLinkLabel, "appLinkLabel");
            this.appLinkLabel.Name = "appLinkLabel";
            this.appLinkLabel.TabStop = true;
            this.appLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.appLinkLabel_LinkClicked);
            // 
            // appPathTextBox
            // 
            resources.ApplyResources(this.appPathTextBox, "appPathTextBox");
            this.appPathTextBox.Name = "appPathTextBox";
            this.appPathTextBox.TextChanged += new System.EventHandler(this.appPathTextBox_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "exe";
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // AppLaunchForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppLaunchForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label appNameLabel;
        private System.Windows.Forms.LinkLabel appLinkLabel;
        private System.Windows.Forms.TextBox appPathTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button folderButton;
        private System.Windows.Forms.ComboBox applicationComboBox;
    }
}