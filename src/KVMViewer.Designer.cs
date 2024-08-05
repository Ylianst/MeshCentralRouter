﻿using System.Windows.Forms;

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
            this.chatButton = new System.Windows.Forms.Button();
            this.consentContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.askConsentBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.askConsentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.privacyBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRemoteFilesButton = new System.Windows.Forms.Button();
            this.extraButtonsPanel = new System.Windows.Forms.Panel();
            this.splitButton = new System.Windows.Forms.Button();
            this.clipOutboundButton = new System.Windows.Forms.Button();
            this.clipInboundButton = new System.Windows.Forms.Button();
            this.statsButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.zoomButton = new System.Windows.Forms.Button();
            this.cadButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.consoleMessage = new System.Windows.Forms.Label();
            this.consoleTimer = new System.Windows.Forms.Timer(this.components);
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.displaySelectorImageList = new System.Windows.Forms.ImageList(this.components);
            this.resizeKvmControl = new MeshCentralRouter.KVMResizeControl();
            this.mainStatusStrip.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.consentContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.BackColor = System.Drawing.SystemColors.Menu;
            this.mainStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.topPanel.Controls.Add(this.chatButton);
            this.topPanel.Controls.Add(this.openRemoteFilesButton);
            this.topPanel.Controls.Add(this.extraButtonsPanel);
            this.topPanel.Controls.Add(this.splitButton);
            this.topPanel.Controls.Add(this.clipOutboundButton);
            this.topPanel.Controls.Add(this.clipInboundButton);
            this.topPanel.Controls.Add(this.statsButton);
            this.topPanel.Controls.Add(this.settingsButton);
            this.topPanel.Controls.Add(this.zoomButton);
            this.topPanel.Controls.Add(this.cadButton);
            this.topPanel.Controls.Add(this.connectButton);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // chatButton
            // 
            this.chatButton.ContextMenuStrip = this.consentContextMenuStrip;
            resources.ApplyResources(this.chatButton, "chatButton");
            this.chatButton.Name = "chatButton";
            this.chatButton.TabStop = false;
            this.chatButton.UseVisualStyleBackColor = true;
            this.chatButton.Click += new System.EventHandler(this.chatButton_Click);
            // 
            // consentContextMenuStrip
            // 
            this.consentContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.consentContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.askConsentBarToolStripMenuItem,
            this.askConsentToolStripMenuItem,
            this.privacyBarToolStripMenuItem});
            this.consentContextMenuStrip.Name = "consentContextMenuStrip";
            resources.ApplyResources(this.consentContextMenuStrip, "consentContextMenuStrip");
            this.consentContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.consentContextMenuStrip_Opening);
            // 
            // askConsentBarToolStripMenuItem
            // 
            this.askConsentBarToolStripMenuItem.Name = "askConsentBarToolStripMenuItem";
            resources.ApplyResources(this.askConsentBarToolStripMenuItem, "askConsentBarToolStripMenuItem");
            this.askConsentBarToolStripMenuItem.Click += new System.EventHandler(this.askConsentBarToolStripMenuItem_Click);
            // 
            // askConsentToolStripMenuItem
            // 
            this.askConsentToolStripMenuItem.Name = "askConsentToolStripMenuItem";
            resources.ApplyResources(this.askConsentToolStripMenuItem, "askConsentToolStripMenuItem");
            this.askConsentToolStripMenuItem.Click += new System.EventHandler(this.askConsentToolStripMenuItem_Click);
            // 
            // privacyBarToolStripMenuItem
            // 
            this.privacyBarToolStripMenuItem.Name = "privacyBarToolStripMenuItem";
            resources.ApplyResources(this.privacyBarToolStripMenuItem, "privacyBarToolStripMenuItem");
            this.privacyBarToolStripMenuItem.Click += new System.EventHandler(this.privacyBarToolStripMenuItem_Click);
            // 
            // openRemoteFilesButton
            // 
            resources.ApplyResources(this.openRemoteFilesButton, "openRemoteFilesButton");
            this.openRemoteFilesButton.Name = "openRemoteFilesButton";
            this.openRemoteFilesButton.TabStop = false;
            this.openRemoteFilesButton.UseVisualStyleBackColor = true;
            this.openRemoteFilesButton.Click += new System.EventHandler(this.openRemoteFilesButton_Click);
            // 
            // extraButtonsPanel
            // 
            resources.ApplyResources(this.extraButtonsPanel, "extraButtonsPanel");
            this.extraButtonsPanel.Name = "extraButtonsPanel";
            // 
            // splitButton
            // 
            resources.ApplyResources(this.splitButton, "splitButton");
            this.splitButton.Name = "splitButton";
            this.splitButton.TabStop = false;
            this.splitButton.UseVisualStyleBackColor = true;
            this.splitButton.Click += new System.EventHandler(this.splitButton_Click);
            // 
            // clipOutboundButton
            // 
            resources.ApplyResources(this.clipOutboundButton, "clipOutboundButton");
            this.clipOutboundButton.Image = global::MeshCentralRouter.Properties.Resources.iconClipboardOut;
            this.clipOutboundButton.Name = "clipOutboundButton";
            this.clipOutboundButton.TabStop = false;
            this.clipOutboundButton.UseVisualStyleBackColor = true;
            this.clipOutboundButton.Click += new System.EventHandler(this.clipOutboundButton_Click);
            // 
            // clipInboundButton
            // 
            resources.ApplyResources(this.clipInboundButton, "clipInboundButton");
            this.clipInboundButton.Image = global::MeshCentralRouter.Properties.Resources.iconClipboardIn;
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
            this.statsButton.UseVisualStyleBackColor = true;
            this.statsButton.Click += new System.EventHandler(this.statsButton_Click);
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
            this.zoomButton.Image = global::MeshCentralRouter.Properties.Resources.ZoomToFit;
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
            this.connectButton.ContextMenuStrip = this.consentContextMenuStrip;
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.Name = "connectButton";
            this.connectButton.TabStop = false;
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
            // displaySelectorImageList
            // 
            this.displaySelectorImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("displaySelectorImageList.ImageStream")));
            this.displaySelectorImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.displaySelectorImageList.Images.SetKeyName(0, "icon-monitor1.png");
            this.displaySelectorImageList.Images.SetKeyName(1, "icon-monitor1b.png");
            this.displaySelectorImageList.Images.SetKeyName(2, "icon-monitor2.png");
            this.displaySelectorImageList.Images.SetKeyName(3, "icon-monitor2b.png");
            // 
            // resizeKvmControl
            // 
            this.resizeKvmControl.BackColor = System.Drawing.Color.Gray;
            resources.ApplyResources(this.resizeKvmControl, "resizeKvmControl");
            this.resizeKvmControl.Name = "resizeKvmControl";
            this.resizeKvmControl.ZoomToFit = false;
            this.resizeKvmControl.StateChanged += new System.EventHandler(this.kvmControl_StateChanged);
            this.resizeKvmControl.DisplaysReceived += new System.EventHandler(this.resizeKvmControl_DisplaysReceived);
            this.resizeKvmControl.Enter += new System.EventHandler(this.resizeKvmControl_Enter);
            this.resizeKvmControl.Leave += new System.EventHandler(this.resizeKvmControl_Leave);
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
            this.Activated += new System.EventHandler(this.KVMViewer_Activated);
            this.Deactivate += new System.EventHandler(this.KVMViewer_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.consentContextMenuStrip.ResumeLayout(false);
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
        private Label consoleMessage;
        private Timer consoleTimer;
        private Button statsButton;
        private Button clipInboundButton;
        private Button clipOutboundButton;
        private ToolTip mainToolTip;
        private ContextMenuStrip consentContextMenuStrip;
        private ToolStripMenuItem askConsentBarToolStripMenuItem;
        private ToolStripMenuItem askConsentToolStripMenuItem;
        private ToolStripMenuItem privacyBarToolStripMenuItem;
        private Button splitButton;
        private Panel extraButtonsPanel;
        private ImageList displaySelectorImageList;
        private Button openRemoteFilesButton;
        private Button chatButton;
    }
}

