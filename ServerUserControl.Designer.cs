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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerUserControl));
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
            resources.ApplyResources(this.serverInfoLabel, "serverInfoLabel");
            this.serverInfoLabel.Name = "serverInfoLabel";
            this.serverInfoLabel.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // serverNameLabel
            // 
            resources.ApplyResources(this.serverNameLabel, "serverNameLabel");
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // serverPictureBox
            // 
            this.serverPictureBox.Image = global::MeshCentralRouter.Properties.Resources.MeshCentral;
            resources.ApplyResources(this.serverPictureBox, "serverPictureBox");
            this.serverPictureBox.Name = "serverPictureBox";
            this.serverPictureBox.TabStop = false;
            this.serverPictureBox.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // serverContextMenuStrip
            // 
            this.serverContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem});
            this.serverContextMenuStrip.Name = "serverContextMenuStrip";
            resources.ApplyResources(this.serverContextMenuStrip, "serverContextMenuStrip");
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // backTimer
            // 
            this.backTimer.Interval = 2000;
            this.backTimer.Tick += new System.EventHandler(this.backTime_Tick);
            // 
            // ServerUserControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.serverContextMenuStrip;
            this.Controls.Add(this.serverInfoLabel);
            this.Controls.Add(this.serverNameLabel);
            this.Controls.Add(this.serverPictureBox);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "ServerUserControl";
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
