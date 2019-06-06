namespace MeshCentralRouter
{
    partial class ServerUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serverInfoLabel = new System.Windows.Forms.Label();
            this.serverNameLabel = new System.Windows.Forms.Label();
            this.serverPictureBox = new System.Windows.Forms.PictureBox();
            this.serverContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.serverPictureBox)).BeginInit();
            this.serverContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverInfoLabel
            // 
            this.serverInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverInfoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverInfoLabel.Location = new System.Drawing.Point(65, 30);
            this.serverInfoLabel.Name = "serverInfoLabel";
            this.serverInfoLabel.Size = new System.Drawing.Size(420, 18);
            this.serverInfoLabel.TabIndex = 11;
            this.serverInfoLabel.Text = "Server information";
            this.serverInfoLabel.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverNameLabel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverNameLabel.Location = new System.Drawing.Point(65, 10);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(420, 18);
            this.serverNameLabel.TabIndex = 10;
            this.serverNameLabel.Text = "ServerName";
            this.serverNameLabel.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // serverPictureBox
            // 
            this.serverPictureBox.Image = global::MeshCentralRouter.Properties.Resources.MeshCentral;
            this.serverPictureBox.Location = new System.Drawing.Point(0, 0);
            this.serverPictureBox.Name = "serverPictureBox";
            this.serverPictureBox.Size = new System.Drawing.Size(59, 59);
            this.serverPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.serverPictureBox.TabIndex = 9;
            this.serverPictureBox.TabStop = false;
            this.serverPictureBox.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // serverContextMenuStrip
            // 
            this.serverContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem});
            this.serverContextMenuStrip.Name = "serverContextMenuStrip";
            this.serverContextMenuStrip.Size = new System.Drawing.Size(153, 48);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.infoToolStripMenuItem.Text = "&Info...";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // backTimer
            // 
            this.backTimer.Interval = 2000;
            this.backTimer.Tick += new System.EventHandler(this.backTime_Tick);
            // 
            // ServerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.serverContextMenuStrip;
            this.Controls.Add(this.serverInfoLabel);
            this.Controls.Add(this.serverNameLabel);
            this.Controls.Add(this.serverPictureBox);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "ServerUserControl";
            this.Size = new System.Drawing.Size(485, 57);
            this.Load += new System.EventHandler(this.ServerUserControl_Load);
            this.Click += new System.EventHandler(this.serverButton_Click);
            ((System.ComponentModel.ISupportInitialize)(this.serverPictureBox)).EndInit();
            this.serverContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label serverInfoLabel;
        private System.Windows.Forms.Label serverNameLabel;
        private System.Windows.Forms.PictureBox serverPictureBox;
        private System.Windows.Forms.ContextMenuStrip serverContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.Timer backTimer;
    }
}
