namespace MeshCentralRouter
{
    partial class FileTransferStatusForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileTransferStatusForm));
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainGroupBox = new System.Windows.Forms.GroupBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.mainLabel1 = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.mainLabel2 = new System.Windows.Forms.Label();
            this.mainGroupBox.SuspendLayout();
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
            // mainGroupBox
            // 
            resources.ApplyResources(this.mainGroupBox, "mainGroupBox");
            this.mainGroupBox.Controls.Add(this.mainLabel2);
            this.mainGroupBox.Controls.Add(this.progressBar2);
            this.mainGroupBox.Controls.Add(this.progressBar1);
            this.mainGroupBox.Controls.Add(this.mainLabel1);
            this.mainGroupBox.Name = "mainGroupBox";
            this.mainGroupBox.TabStop = false;
            // 
            // progressBar2
            // 
            resources.ApplyResources(this.progressBar2, "progressBar2");
            this.progressBar2.Name = "progressBar2";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // mainLabel1
            // 
            resources.ApplyResources(this.mainLabel1, "mainLabel1");
            this.mainLabel1.Name = "mainLabel1";
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 500;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // mainLabel2
            // 
            resources.ApplyResources(this.mainLabel2, "mainLabel2");
            this.mainLabel2.Name = "mainLabel2";
            // 
            // FileTransferStatusForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.mainGroupBox);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileTransferStatusForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileTransferStatusForm_FormClosing);
            this.Load += new System.EventHandler(this.FileTransferStatusForm_Load);
            this.mainGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox mainGroupBox;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label mainLabel1;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label mainLabel2;
    }
}