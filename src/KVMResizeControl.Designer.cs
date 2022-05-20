namespace MeshCentralRouter
{
    partial class KVMResizeControl
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
            this.rightScrollPanel = new System.Windows.Forms.Panel();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.cornerBlockPanel = new System.Windows.Forms.Panel();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.clientPanel = new System.Windows.Forms.Panel();
            this.kvmControl = new MeshCentralRouter.KVMControl();
            this.rightScrollPanel.SuspendLayout();
            this.clientPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // rightScrollPanel
            // 
            this.rightScrollPanel.Controls.Add(this.vScrollBar);
            this.rightScrollPanel.Controls.Add(this.cornerBlockPanel);
            this.rightScrollPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightScrollPanel.Location = new System.Drawing.Point(1159, 0);
            this.rightScrollPanel.Name = "rightScrollPanel";
            this.rightScrollPanel.Size = new System.Drawing.Size(17, 789);
            this.rightScrollPanel.TabIndex = 12;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar.LargeChange = 100;
            this.vScrollBar.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 772);
            this.vScrollBar.TabIndex = 10;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
            // 
            // cornerBlockPanel
            // 
            this.cornerBlockPanel.BackColor = System.Drawing.SystemColors.Control;
            this.cornerBlockPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cornerBlockPanel.Location = new System.Drawing.Point(0, 772);
            this.cornerBlockPanel.Name = "cornerBlockPanel";
            this.cornerBlockPanel.Size = new System.Drawing.Size(17, 17);
            this.cornerBlockPanel.TabIndex = 11;
            this.cornerBlockPanel.Visible = false;
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.LargeChange = 100;
            this.hScrollBar.Location = new System.Drawing.Point(0, 772);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(1159, 17);
            this.hScrollBar.TabIndex = 13;
            this.hScrollBar.ValueChanged += new System.EventHandler(this.hScrollBar_ValueChanged);
            // 
            // clientPanel
            // 
            this.clientPanel.Controls.Add(this.kvmControl);
            this.clientPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientPanel.Location = new System.Drawing.Point(0, 0);
            this.clientPanel.Name = "clientPanel";
            this.clientPanel.Size = new System.Drawing.Size(1159, 772);
            this.clientPanel.TabIndex = 15;
            // 
            // kvmControl
            // 
            this.kvmControl.AutoSendClipboard = false;
            this.kvmControl.BackColor = System.Drawing.Color.LightCoral;
            this.kvmControl.CompressionLevel = 60;
            this.kvmControl.FrameRate = 100;
            this.kvmControl.Location = new System.Drawing.Point(33, 31);
            this.kvmControl.Margin = new System.Windows.Forms.Padding(4);
            this.kvmControl.Name = "kvmControl";
            this.kvmControl.RemoteKeyboardMap = false;
            this.kvmControl.ScaleFactor = 1D;
            this.kvmControl.ScalingLevel = 1024;
            this.kvmControl.Size = new System.Drawing.Size(450, 314);
            this.kvmControl.SwamMouseButtons = false;
            this.kvmControl.TabIndex = 14;
            this.kvmControl.DesktopSizeChanged += new System.EventHandler(this.kvmControl_DesktopSizeChanged);
            this.kvmControl.Resize += new System.EventHandler(this.kvmControl_Resize);
            // 
            // KVMResizeControl
            // 
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.clientPanel);
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.rightScrollPanel);
            this.Name = "KVMResizeControl";
            this.Size = new System.Drawing.Size(1176, 789);
            this.Resize += new System.EventHandler(this.ResizeKVMControl_Resize);
            this.rightScrollPanel.ResumeLayout(false);
            this.clientPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel rightScrollPanel;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.Panel cornerBlockPanel;
        private System.Windows.Forms.HScrollBar hScrollBar;
        private KVMControl kvmControl;
        private System.Windows.Forms.Panel clientPanel;
    }
}
