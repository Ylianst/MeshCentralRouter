using System.Windows.Forms;

namespace MeshCentralRouter
{

    public class BlankPanel : System.Windows.Forms.Panel
    {
        public BlankPanel()
        {

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }
    }

    partial class KVMViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KVMViewer));
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.topPanel = new System.Windows.Forms.Panel();
            this.clipOutboundButton = new System.Windows.Forms.Button();
            this.clipInboundButton = new System.Windows.Forms.Button();
            this.statsButton = new System.Windows.Forms.Button();
            this.displaySelectComboBox = new System.Windows.Forms.ComboBox();
            this.settingsButton = new System.Windows.Forms.Button();
            this.zoomButton = new System.Windows.Forms.Button();
            this.cadButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.consoleMessage = new System.Windows.Forms.Label();
            this.consoleTimer = new System.Windows.Forms.Timer(this.components);
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.resizeKvmControl = new MeshCentralRouter.KVMResizeControl();
            this.mainStatusStrip.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripStatusLabel,
            this.toolStripStatusLabel1});
            resources.ApplyResources(this.mainStatusStrip, "mainStatusStrip");
            this.mainStatusStrip.Name = "mainStatusStrip";
            // 
            // mainToolStripStatusLabel
            // 
            this.mainToolStripStatusLabel.Name = "mainToolStripStatusLabel";
            resources.ApplyResources(this.mainToolStripStatusLabel, "mainToolStripStatusLabel");
            this.mainToolStripStatusLabel.Spring = true;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topPanel.Controls.Add(this.clipOutboundButton);
            this.topPanel.Controls.Add(this.clipInboundButton);
            this.topPanel.Controls.Add(this.statsButton);
            this.topPanel.Controls.Add(this.displaySelectComboBox);
            this.topPanel.Controls.Add(this.settingsButton);
            this.topPanel.Controls.Add(this.zoomButton);
            this.topPanel.Controls.Add(this.cadButton);
            this.topPanel.Controls.Add(this.connectButton);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // clipOutboundButton
            // 
            resources.ApplyResources(this.clipOutboundButton, "clipOutboundButton");
            this.clipOutboundButton.Image = global::MeshCentralRouter.Properties.Resources.icon_clipboard_out;
            this.clipOutboundButton.Name = "clipOutboundButton";
            this.clipOutboundButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.clipOutboundButton, resources.GetString("clipOutboundButton.ToolTip"));
            this.clipOutboundButton.UseVisualStyleBackColor = true;
            this.clipOutboundButton.Click += new System.EventHandler(this.clipOutboundButton_Click);
            // 
            // clipInboundButton
            // 
            resources.ApplyResources(this.clipInboundButton, "clipInboundButton");
            this.clipInboundButton.Image = global::MeshCentralRouter.Properties.Resources.icon_clipboard_in;
            this.clipInboundButton.Name = "clipInboundButton";
            this.clipInboundButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.clipInboundButton, resources.GetString("clipInboundButton.ToolTip"));
            this.clipInboundButton.UseVisualStyleBackColor = true;
            this.clipInboundButton.Click += new System.EventHandler(this.clipInboundButton_Click);
            // 
            // statsButton
            // 
            resources.ApplyResources(this.statsButton, "statsButton");
            this.statsButton.Name = "statsButton";
            this.statsButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.statsButton, resources.GetString("statsButton.ToolTip"));
            this.statsButton.UseVisualStyleBackColor = true;
            this.statsButton.Click += new System.EventHandler(this.statsButton_Click);
            // 
            // displaySelectComboBox
            // 
            this.displaySelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.displaySelectComboBox, "displaySelectComboBox");
            this.displaySelectComboBox.Name = "displaySelectComboBox";
            this.displaySelectComboBox.TabStop = false;
            this.displaySelectComboBox.SelectionChangeCommitted += new System.EventHandler(this.displaySelectComboBox_SelectionChangeCommitted);
            // 
            // settingsButton
            // 
            resources.ApplyResources(this.settingsButton, "settingsButton");
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.settingsButton, resources.GetString("settingsButton.ToolTip"));
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // zoomButton
            // 
            resources.ApplyResources(this.zoomButton, "zoomButton");
            this.zoomButton.Image = global::MeshCentralRouter.Properties.Resources.ZoomToFit;
            this.zoomButton.Name = "zoomButton";
            this.zoomButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.zoomButton, resources.GetString("zoomButton.ToolTip"));
            this.zoomButton.UseVisualStyleBackColor = true;
            this.zoomButton.Click += new System.EventHandler(this.zoomButton_Click);
            // 
            // cadButton
            // 
            resources.ApplyResources(this.cadButton, "cadButton");
            this.cadButton.Name = "cadButton";
            this.cadButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.cadButton, resources.GetString("cadButton.ToolTip"));
            this.cadButton.UseVisualStyleBackColor = true;
            this.cadButton.Click += new System.EventHandler(this.sendCtrlAltDelToolStripMenuItem_Click);
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.Name = "connectButton";
            this.connectButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.connectButton, resources.GetString("connectButton.ToolTip"));
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.MenuItemDisconnect_Click);
            // 
            // consoleMessage
            // 
            resources.ApplyResources(this.consoleMessage, "consoleMessage");
            this.consoleMessage.ForeColor = System.Drawing.Color.Black;
            this.consoleMessage.Name = "consoleMessage";
            // 
            // consoleTimer
            // 
            this.consoleTimer.Interval = 5000;
            this.consoleTimer.Tick += new System.EventHandler(this.consoleTimer_Tick);
            // 
            // resizeKvmControl
            // 
            this.resizeKvmControl.BackColor = System.Drawing.Color.Gray;
            resources.ApplyResources(this.resizeKvmControl, "resizeKvmControl");
            this.resizeKvmControl.Name = "resizeKvmControl";
            this.resizeKvmControl.ZoomToFit = false;
            this.resizeKvmControl.StateChanged += new System.EventHandler(this.kvmControl_StateChanged);
            this.resizeKvmControl.DisplaysReceived += new System.EventHandler(this.resizeKvmControl_DisplaysReceived);
            // 
            // KVMViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.consoleMessage);
            this.Controls.Add(this.resizeKvmControl);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.mainStatusStrip);
            this.Name = "KVMViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainToolStripStatusLabel;
        private Timer updateTimer;
        private KVMResizeControl resizeKvmControl;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Panel topPanel;
        private Button connectButton;
        private Button cadButton;
        private Button zoomButton;
        private Button settingsButton;
        private ComboBox displaySelectComboBox;
        private Label consoleMessage;
        private Timer consoleTimer;
        private Button statsButton;
        private Button clipInboundButton;
        private Button clipOutboundButton;
        private ToolTip mainToolTip;
    }
}

