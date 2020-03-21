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
            this.deviceImageList = new System.Windows.Forms.ImageList(this.components);
            this.disabledDeviceImageList = new System.Windows.Forms.ImageList(this.components);
            this.httpsButton = new System.Windows.Forms.Button();
            this.httpButton = new System.Windows.Forms.Button();
            this.scpButton = new System.Windows.Forms.Button();
            this.sshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.devicePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // devicePictureBox
            // 
            this.devicePictureBox.Image = global::MeshCentralRouter.Properties.Resources.icons01;
            this.devicePictureBox.Location = new System.Drawing.Point(6, 3);
            this.devicePictureBox.Name = "devicePictureBox";
            this.devicePictureBox.Size = new System.Drawing.Size(50, 50);
            this.devicePictureBox.TabIndex = 0;
            this.devicePictureBox.TabStop = false;
            // 
            // deviceNameLabel
            // 
            this.deviceNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceNameLabel.Location = new System.Drawing.Point(62, 12);
            this.deviceNameLabel.Name = "deviceNameLabel";
            this.deviceNameLabel.Size = new System.Drawing.Size(243, 15);
            this.deviceNameLabel.TabIndex = 1;
            this.deviceNameLabel.Text = "ComputerName";
            // 
            // deviceStatusLabel
            // 
            this.deviceStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceStatusLabel.Location = new System.Drawing.Point(62, 32);
            this.deviceStatusLabel.Name = "deviceStatusLabel";
            this.deviceStatusLabel.Size = new System.Drawing.Size(243, 15);
            this.deviceStatusLabel.TabIndex = 2;
            this.deviceStatusLabel.Text = "Device Status";
            // 
            // rdpButton
            // 
            this.rdpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdpButton.Location = new System.Drawing.Point(375, 6);
            this.rdpButton.Name = "rdpButton";
            this.rdpButton.Size = new System.Drawing.Size(58, 47);
            this.rdpButton.TabIndex = 3;
            this.rdpButton.Text = "RDP";
            this.rdpButton.UseVisualStyleBackColor = true;
            this.rdpButton.Click += new System.EventHandler(this.rdpButton_Click);
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
            // 
            // httpsButton
            // 
            this.httpsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.httpsButton.Location = new System.Drawing.Point(311, 6);
            this.httpsButton.Name = "httpsButton";
            this.httpsButton.Size = new System.Drawing.Size(58, 23);
            this.httpsButton.TabIndex = 4;
            this.httpsButton.Text = "HTTPS";
            this.httpsButton.UseVisualStyleBackColor = true;
            this.httpsButton.Click += new System.EventHandler(this.httpsButton_Click);
            // 
            // httpButton
            // 
            this.httpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.httpButton.Location = new System.Drawing.Point(311, 30);
            this.httpButton.Name = "httpButton";
            this.httpButton.Size = new System.Drawing.Size(58, 23);
            this.httpButton.TabIndex = 5;
            this.httpButton.Text = "HTTP";
            this.httpButton.UseVisualStyleBackColor = true;
            this.httpButton.Click += new System.EventHandler(this.httpButton_Click);
            // 
            // scpButton
            // 
            this.scpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scpButton.Location = new System.Drawing.Point(375, 30);
            this.scpButton.Name = "scpButton";
            this.scpButton.Size = new System.Drawing.Size(58, 23);
            this.scpButton.TabIndex = 7;
            this.scpButton.Text = "SCP";
            this.scpButton.UseVisualStyleBackColor = true;
            this.scpButton.Click += new System.EventHandler(this.scpButton_Click);
            // 
            // sshButton
            // 
            this.sshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sshButton.Location = new System.Drawing.Point(375, 6);
            this.sshButton.Name = "sshButton";
            this.sshButton.Size = new System.Drawing.Size(58, 23);
            this.sshButton.TabIndex = 6;
            this.sshButton.Text = "SSH";
            this.sshButton.UseVisualStyleBackColor = true;
            this.sshButton.Click += new System.EventHandler(this.sshButton_Click);
            // 
            // DeviceUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
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
            this.Size = new System.Drawing.Size(441, 60);
            ((System.ComponentModel.ISupportInitialize)(this.devicePictureBox)).EndInit();
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
    }
}
