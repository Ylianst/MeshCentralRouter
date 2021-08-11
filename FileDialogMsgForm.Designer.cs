namespace MeshCentralRouter
{
    partial class FileDialogMsgForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileDialogMsgForm));
            this.mainGroupBox = new System.Windows.Forms.GroupBox();
            this.mainLabel2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.mainLabel1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGroupBox
            // 
            this.mainGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainGroupBox.Controls.Add(this.mainLabel2);
            this.mainGroupBox.Controls.Add(this.progressBar1);
            this.mainGroupBox.Controls.Add(this.mainLabel1);
            this.mainGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mainGroupBox.Name = "mainGroupBox";
            this.mainGroupBox.Size = new System.Drawing.Size(345, 98);
            this.mainGroupBox.TabIndex = 5;
            this.mainGroupBox.TabStop = false;
            this.mainGroupBox.Text = "Compression Progress";
            // 
            // mainLabel2
            // 
            this.mainLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mainLabel2.Location = new System.Drawing.Point(14, 43);
            this.mainLabel2.Name = "mainLabel2";
            this.mainLabel2.Size = new System.Drawing.Size(314, 13);
            this.mainLabel2.TabIndex = 3;
            this.mainLabel2.Text = "---";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.progressBar1.Location = new System.Drawing.Point(17, 65);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(311, 16);
            this.progressBar1.TabIndex = 2;
            // 
            // mainLabel1
            // 
            this.mainLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mainLabel1.Location = new System.Drawing.Point(14, 23);
            this.mainLabel1.Name = "mainLabel1";
            this.mainLabel1.Size = new System.Drawing.Size(314, 13);
            this.mainLabel1.TabIndex = 1;
            this.mainLabel1.Text = "---";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(282, 116);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // FileDialogMsgForm
            // 
            this.AcceptButton = this.cancelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 151);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.mainGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileDialogMsgForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Operation";
            this.mainGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mainGroupBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label mainLabel1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label mainLabel2;
    }
}