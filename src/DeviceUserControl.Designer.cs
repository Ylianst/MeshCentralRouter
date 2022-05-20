namespace MeshCentralRouter
{
    partial class DeviceUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceUserControl));
            this.devicePictureBox = new System.Windows.Forms.PictureBox();
            this.deviceNameLabel = new System.Windows.Forms.Label();
            this.deviceStatusLabel = new System.Windows.Forms.Label();
            this.rdpButton = new System.Windows.Forms.Button();
            this.rdpContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setRDPPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceImageList = new System.Windows.Forms.ImageList(this.components);
            this.disabledDeviceImageList = new System.Windows.Forms.ImageList(this.components);
            this.httpsButton = new System.Windows.Forms.Button();
            this.altPortContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.httpButton = new System.Windows.Forms.Button();
            this.scpButton = new System.Windows.Forms.Button();
            this.sshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.devicePictureBox)).BeginInit();
            this.rdpContextMenuStrip.SuspendLayout();
            this.altPortContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // devicePictureBox
            // 
            resources.ApplyResources(this.devicePictureBox, "devicePictureBox");
            this.devicePictureBox.Name = "devicePictureBox";
            this.devicePictureBox.TabStop = false;
            // 
            // deviceNameLabel
            // 
            resources.ApplyResources(this.deviceNameLabel, "deviceNameLabel");
            this.deviceNameLabel.Name = "deviceNameLabel";
            // 
            // deviceStatusLabel
            // 
            resources.ApplyResources(this.deviceStatusLabel, "deviceStatusLabel");
            this.deviceStatusLabel.Name = "deviceStatusLabel";
            // 
            // rdpButton
            // 
            resources.ApplyResources(this.rdpButton, "rdpButton");
            this.rdpButton.ContextMenuStrip = this.rdpContextMenuStrip;
            this.rdpButton.Name = "rdpButton";
            this.rdpButton.UseVisualStyleBackColor = true;
            this.rdpButton.Click += new System.EventHandler(this.rdpButton_Click);
            // 
            // rdpContextMenuStrip
            // 
            this.rdpContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setRDPPortToolStripMenuItem});
            this.rdpContextMenuStrip.Name = "rdpContextMenuStrip";
            resources.ApplyResources(this.rdpContextMenuStrip, "rdpContextMenuStrip");
            // 
            // setRDPPortToolStripMenuItem
            // 
            this.setRDPPortToolStripMenuItem.Name = "setRDPPortToolStripMenuItem";
            resources.ApplyResources(this.setRDPPortToolStripMenuItem, "setRDPPortToolStripMenuItem");
            this.setRDPPortToolStripMenuItem.Click += new System.EventHandler(this.setRDPPortToolStripMenuItem_Click);
            // 
            // deviceImageList
            // 
            this.deviceImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("deviceImageList.ImageStream")));
            this.deviceImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.deviceImageList.Images.SetKeyName(0, "icons01.png");
            this.deviceImageList.Images.SetKeyName(1, "icons02.png");
            this.deviceImageList.Images.SetKeyName(2, "icons03.png");
            this.deviceImageList.Images.SetKeyName(3, "icons04.png");
            this.deviceImageList.Images.SetKeyName(4, "icons05.png");
            this.deviceImageList.Images.SetKeyName(5, "icons06.png");
            this.deviceImageList.Images.SetKeyName(6, "icons07.png");
            this.deviceImageList.Images.SetKeyName(7, "icons08.png");
            // 
            // disabledDeviceImageList
            // 
            this.disabledDeviceImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("disabledDeviceImageList.ImageStream")));
            this.disabledDeviceImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.disabledDeviceImageList.Images.SetKeyName(0, "icons01t.png");
            this.disabledDeviceImageList.Images.SetKeyName(1, "icons02t.png");
            this.disabledDeviceImageList.Images.SetKeyName(2, "icons03t.png");
            this.disabledDeviceImageList.Images.SetKeyName(3, "icons04t.png");
            this.disabledDeviceImageList.Images.SetKeyName(4, "icons05t.png");
            this.disabledDeviceImageList.Images.SetKeyName(5, "icons06t.png");
            this.disabledDeviceImageList.Images.SetKeyName(6, "icons07t.png");
            this.disabledDeviceImageList.Images.SetKeyName(7, "icons08t.png");
            // 
            // httpsButton
            // 
            resources.ApplyResources(this.httpsButton, "httpsButton");
            this.httpsButton.ContextMenuStrip = this.altPortContextMenuStrip;
            this.httpsButton.Name = "httpsButton";
            this.httpsButton.UseVisualStyleBackColor = true;
            this.httpsButton.Click += new System.EventHandler(this.httpsButton_Click);
            // 
            // altPortContextMenuStrip
            // 
            this.altPortContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.altPortContextMenuStrip.Name = "rdpContextMenuStrip";
            resources.ApplyResources(this.altPortContextMenuStrip, "altPortContextMenuStrip");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // httpButton
            // 
            resources.ApplyResources(this.httpButton, "httpButton");
            this.httpButton.ContextMenuStrip = this.altPortContextMenuStrip;
            this.httpButton.Name = "httpButton";
            this.httpButton.UseVisualStyleBackColor = true;
            this.httpButton.Click += new System.EventHandler(this.httpButton_Click);
            // 
            // scpButton
            // 
            resources.ApplyResources(this.scpButton, "scpButton");
            this.scpButton.ContextMenuStrip = this.altPortContextMenuStrip;
            this.scpButton.Name = "scpButton";
            this.scpButton.UseVisualStyleBackColor = true;
            this.scpButton.Click += new System.EventHandler(this.scpButton_Click);
            // 
            // sshButton
            // 
            resources.ApplyResources(this.sshButton, "sshButton");
            this.sshButton.ContextMenuStrip = this.altPortContextMenuStrip;
            this.sshButton.Name = "sshButton";
            this.sshButton.UseVisualStyleBackColor = true;
            this.sshButton.Click += new System.EventHandler(this.sshButton_Click);
            // 
            // DeviceUserControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.scpButton);
            this.Controls.Add(this.sshButton);
            this.Controls.Add(this.httpButton);
            this.Controls.Add(this.httpsButton);
            this.Controls.Add(this.rdpButton);
            this.Controls.Add(this.deviceStatusLabel);
            this.Controls.Add(this.deviceNameLabel);
            this.Controls.Add(this.devicePictureBox);
            this.Name = "DeviceUserControl";
            ((System.ComponentModel.ISupportInitialize)(this.devicePictureBox)).EndInit();
            this.rdpContextMenuStrip.ResumeLayout(false);
            this.altPortContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox devicePictureBox;
        private System.Windows.Forms.Label deviceNameLabel;
        private System.Windows.Forms.Label deviceStatusLabel;
        private System.Windows.Forms.Button rdpButton;
        private System.Windows.Forms.ImageList deviceImageList;
        private System.Windows.Forms.ImageList disabledDeviceImageList;
        private System.Windows.Forms.Button httpsButton;
        private System.Windows.Forms.Button httpButton;
        private System.Windows.Forms.Button scpButton;
        private System.Windows.Forms.Button sshButton;
        private System.Windows.Forms.ContextMenuStrip rdpContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem setRDPPortToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip altPortContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
