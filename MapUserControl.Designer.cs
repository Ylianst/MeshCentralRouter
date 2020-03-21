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
            this.deviceNameLabel.Size = new System.Drawing.Size(290, 15);
            this.deviceNameLabel.TabIndex = 1;
            this.deviceNameLabel.Text = "ComputerName";
            // 
            // routingStatusLabel
            // 
            this.routingStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.routingStatusLabel.Location = new System.Drawing.Point(62, 32);
            this.routingStatusLabel.Name = "routingStatusLabel";
            this.routingStatusLabel.Size = new System.Drawing.Size(290, 15);
            this.routingStatusLabel.TabIndex = 2;
            this.routingStatusLabel.Text = "Routing Status";
            // 
            // appButton
            // 
            this.appButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.appButton.Location = new System.Drawing.Point(358, 6);
            this.appButton.Name = "appButton";
            this.appButton.Size = new System.Drawing.Size(75, 23);
            this.appButton.TabIndex = 3;
            this.appButton.Text = "Open...";
            this.appButton.UseVisualStyleBackColor = true;
            this.appButton.Click += new System.EventHandler(this.appButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(358, 30);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Remove";
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
            // 
            // MapUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.appButton);
            this.Controls.Add(this.routingStatusLabel);
            this.Controls.Add(this.deviceNameLabel);
            this.Controls.Add(this.devicePictureBox);
            this.Name = "MapUserControl";
            this.Size = new System.Drawing.Size(441, 60);
            ((System.ComponentModel.ISupportInitialize)(this.devicePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox devicePictureBox;
        private System.Windows.Forms.Label deviceNameLabel;
        private System.Windows.Forms.Label routingStatusLabel;
        private System.Windows.Forms.Button appButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ImageList deviceImageList;
    }
}
