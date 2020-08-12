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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomtofitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendCtrlAltDelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.topPanel = new System.Windows.Forms.Panel();
            this.displaySelectComboBox = new System.Windows.Forms.ComboBox();
            this.settingsButton = new System.Windows.Forms.Button();
            this.zoomButton = new System.Windows.Forms.Button();
            this.cadButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.resizeKvmControl = new MeshCentralRouter.KVMResizeControl();
            this.mainMenu.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.viewToolStripMenuItem,
            this.actionsToolStripMenuItem});
            resources.ApplyResources(this.mainMenu, "mainMenu");
            this.mainMenu.Name = "mainMenu";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemConnect,
            this.MenuItemDisconnect,
            this.toolStripMenuItem5,
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.MenuItemExit});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.DropDownOpening += new System.EventHandler(this.toolStripMenuItem2_DropDownOpening);
            // 
            // MenuItemConnect
            // 
            this.MenuItemConnect.Name = "MenuItemConnect";
            resources.ApplyResources(this.MenuItemConnect, "MenuItemConnect");
            this.MenuItemConnect.Click += new System.EventHandler(this.MenuItemConnect_Click);
            // 
            // MenuItemDisconnect
            // 
            this.MenuItemDisconnect.Name = "MenuItemDisconnect";
            resources.ApplyResources(this.MenuItemDisconnect, "MenuItemDisconnect");
            this.MenuItemDisconnect.Click += new System.EventHandler(this.MenuItemDisconnect_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // MenuItemExit
            // 
            this.MenuItemExit.Name = "MenuItemExit";
            resources.ApplyResources(this.MenuItemExit, "MenuItemExit");
            this.MenuItemExit.Click += new System.EventHandler(this.MenuItemExit_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomtofitToolStripMenuItem,
            this.statusToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.toolStripMenuItem6,
            this.refreshToolStripMenuItem,
            this.pauseToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            this.viewToolStripMenuItem.DropDownOpening += new System.EventHandler(this.viewToolStripMenuItem_DropDownOpening);
            // 
            // zoomtofitToolStripMenuItem
            // 
            this.zoomtofitToolStripMenuItem.CheckOnClick = true;
            this.zoomtofitToolStripMenuItem.Name = "zoomtofitToolStripMenuItem";
            resources.ApplyResources(this.zoomtofitToolStripMenuItem, "zoomtofitToolStripMenuItem");
            this.zoomtofitToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.zoomtofitToolStripMenuItem_CheckStateChanged);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Checked = true;
            this.statusToolStripMenuItem.CheckOnClick = true;
            this.statusToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            resources.ApplyResources(this.statusToolStripMenuItem, "statusToolStripMenuItem");
            this.statusToolStripMenuItem.CheckedChanged += new System.EventHandler(this.statusToolStripMenuItem_CheckedChanged);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.CheckOnClick = true;
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            resources.ApplyResources(this.debugToolStripMenuItem, "debugToolStripMenuItem");
            this.debugToolStripMenuItem.Click += new System.EventHandler(this.debugToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            resources.ApplyResources(this.refreshToolStripMenuItem, "refreshToolStripMenuItem");
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            resources.ApplyResources(this.pauseToolStripMenuItem, "pauseToolStripMenuItem");
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendCtrlAltDelToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            resources.ApplyResources(this.actionsToolStripMenuItem, "actionsToolStripMenuItem");
            // 
            // sendCtrlAltDelToolStripMenuItem
            // 
            this.sendCtrlAltDelToolStripMenuItem.Name = "sendCtrlAltDelToolStripMenuItem";
            resources.ApplyResources(this.sendCtrlAltDelToolStripMenuItem, "sendCtrlAltDelToolStripMenuItem");
            this.sendCtrlAltDelToolStripMenuItem.Click += new System.EventHandler(this.sendCtrlAltDelToolStripMenuItem_Click);
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
            this.topPanel.Controls.Add(this.displaySelectComboBox);
            this.topPanel.Controls.Add(this.settingsButton);
            this.topPanel.Controls.Add(this.zoomButton);
            this.topPanel.Controls.Add(this.cadButton);
            this.topPanel.Controls.Add(this.connectButton);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
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
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // zoomButton
            // 
            resources.ApplyResources(this.zoomButton, "zoomButton");
            this.zoomButton.Name = "zoomButton";
            this.zoomButton.TabStop = false;
            this.zoomButton.UseVisualStyleBackColor = true;
            this.zoomButton.Click += new System.EventHandler(this.zoomButton_Click);
            // 
            // cadButton
            // 
            resources.ApplyResources(this.cadButton, "cadButton");
            this.cadButton.Name = "cadButton";
            this.cadButton.TabStop = false;
            this.cadButton.UseVisualStyleBackColor = true;
            this.cadButton.Click += new System.EventHandler(this.sendCtrlAltDelToolStripMenuItem_Click);
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.Name = "connectButton";
            this.connectButton.TabStop = false;
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.MenuItemDisconnect_Click);
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
            this.Controls.Add(this.resizeKvmControl);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenu);
            this.Name = "KVMViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip mainMenu;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem MenuItemConnect;
        private ToolStripMenuItem MenuItemExit;
        private ToolStripMenuItem MenuItemDisconnect;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainToolStripStatusLabel;
        private Timer updateTimer;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem statusToolStripMenuItem;
        private ToolStripMenuItem debugToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private KVMResizeControl resizeKvmControl;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem zoomtofitToolStripMenuItem;
        private ToolStripMenuItem actionsToolStripMenuItem;
        private ToolStripMenuItem sendCtrlAltDelToolStripMenuItem;
        private Panel topPanel;
        private Button connectButton;
        private Button cadButton;
        private Button zoomButton;
        private Button settingsButton;
        private ComboBox displaySelectComboBox;
        private ToolStripSeparator toolStripMenuItem6;








    }
}

