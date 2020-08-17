namespace MeshCentralRouter
{
    partial class MapUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapUserControl));
            this.devicePictureBox = new System.Windows.Forms.PictureBox();
            this.deviceNameLabel = new System.Windows.Forms.Label();
            this.routingStatusLabel = new System.Windows.Forms.Label();
            this.appButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.deviceImageList = new System.Windows.Forms.ImageList(this.components);
            this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.devicePictureBox)).BeginInit();
            this.mainContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // devicePictureBox
            // 
            this.devicePictureBox.Image = global::MeshCentralRouter.Properties.Resources.icons01;
            resources.ApplyResources(this.devicePictureBox, "devicePictureBox");
            this.devicePictureBox.Name = "devicePictureBox";
            this.devicePictureBox.TabStop = false;
            // 
            // deviceNameLabel
            // 
            resources.ApplyResources(this.deviceNameLabel, "deviceNameLabel");
            this.deviceNameLabel.Name = "deviceNameLabel";
            // 
            // routingStatusLabel
            // 
            resources.ApplyResources(this.routingStatusLabel, "routingStatusLabel");
            this.routingStatusLabel.Name = "routingStatusLabel";
            // 
            // appButton
            // 
            resources.ApplyResources(this.appButton, "appButton");
            this.appButton.Name = "appButton";
            this.appButton.UseVisualStyleBackColor = true;
            this.appButton.Click += new System.EventHandler(this.appButton_Click);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
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
            // mainContextMenuStrip
            // 
            this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statsToolStripMenuItem});
            this.mainContextMenuStrip.Name = "mainContextMenuStrip";
            resources.ApplyResources(this.mainContextMenuStrip, "mainContextMenuStrip");
            // 
            // statsToolStripMenuItem
            // 
            this.statsToolStripMenuItem.Name = "statsToolStripMenuItem";
            resources.ApplyResources(this.statsToolStripMenuItem, "statsToolStripMenuItem");
            this.statsToolStripMenuItem.Click += new System.EventHandler(this.statsToolStripMenuItem_Click);
            // 
            // MapUserControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ContextMenuStrip = this.mainContextMenuStrip;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.appButton);
            this.Controls.Add(this.routingStatusLabel);
            this.Controls.Add(this.deviceNameLabel);
            this.Controls.Add(this.devicePictureBox);
            this.Name = "MapUserControl";
            ((System.ComponentModel.ISupportInitialize)(this.devicePictureBox)).EndInit();
            this.mainContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox devicePictureBox;
        private System.Windows.Forms.Label deviceNameLabel;
        private System.Windows.Forms.Label routingStatusLabel;
        private System.Windows.Forms.Button appButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ImageList deviceImageList;
        private System.Windows.Forms.ContextMenuStrip mainContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem statsToolStripMenuItem;
    }
}
