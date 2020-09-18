using System.Windows.Forms;

namespace MeshCentralRouter
{

    partial class FileViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileViewer));
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.consoleTimer = new System.Windows.Forms.Timer(this.components);
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statsButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.consoleMessage = new System.Windows.Forms.Label();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.rightListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileIconImageList = new System.Windows.Forms.ImageList(this.components);
            this.rightTopPanel = new System.Windows.Forms.Panel();
            this.remoteLabel = new System.Windows.Forms.Label();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.leftListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.leftTopPanel = new System.Windows.Forms.Panel();
            this.localLabel = new System.Windows.Forms.Label();
            this.localUpButton = new System.Windows.Forms.Button();
            this.remoteUpButton = new System.Windows.Forms.Button();
            this.localRefreshButton = new System.Windows.Forms.Button();
            this.remoteRefreshButton = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.rightTopPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.leftTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 1000;
            // 
            // consoleTimer
            // 
            this.consoleTimer.Interval = 5000;
            this.consoleTimer.Tick += new System.EventHandler(this.consoleTimer_Tick);
            // 
            // statsButton
            // 
            resources.ApplyResources(this.statsButton, "statsButton");
            this.statsButton.Name = "statsButton";
            this.statsButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.statsButton, resources.GetString("statsButton.ToolTip"));
            this.statsButton.UseVisualStyleBackColor = true;
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.Name = "connectButton";
            this.connectButton.TabStop = false;
            this.mainToolTip.SetToolTip(this.connectButton, resources.GetString("connectButton.ToolTip"));
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topPanel.Controls.Add(this.statsButton);
            this.topPanel.Controls.Add(this.connectButton);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripStatusLabel});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // mainToolStripStatusLabel
            // 
            this.mainToolStripStatusLabel.Name = "mainToolStripStatusLabel";
            resources.ApplyResources(this.mainToolStripStatusLabel, "mainToolStripStatusLabel");
            this.mainToolStripStatusLabel.Spring = true;
            // 
            // consoleMessage
            // 
            resources.ApplyResources(this.consoleMessage, "consoleMessage");
            this.consoleMessage.ForeColor = System.Drawing.Color.Black;
            this.consoleMessage.Name = "consoleMessage";
            // 
            // mainTableLayoutPanel
            // 
            resources.ApplyResources(this.mainTableLayoutPanel, "mainTableLayoutPanel");
            this.mainTableLayoutPanel.Controls.Add(this.panel1, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.rightPanel, 1, 0);
            this.mainTableLayoutPanel.Controls.Add(this.leftPanel, 0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // rightPanel
            // 
            this.rightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rightPanel.Controls.Add(this.rightListView);
            this.rightPanel.Controls.Add(this.rightTopPanel);
            resources.ApplyResources(this.rightPanel, "rightPanel");
            this.rightPanel.Name = "rightPanel";
            // 
            // rightListView
            // 
            this.rightListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            resources.ApplyResources(this.rightListView, "rightListView");
            this.rightListView.FullRowSelect = true;
            this.rightListView.Name = "rightListView";
            this.rightListView.SmallImageList = this.fileIconImageList;
            this.rightListView.UseCompatibleStateImageBehavior = false;
            this.rightListView.View = System.Windows.Forms.View.Details;
            this.rightListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rightListView_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // fileIconImageList
            // 
            this.fileIconImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("fileIconImageList.ImageStream")));
            this.fileIconImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.fileIconImageList.Images.SetKeyName(0, "drive16.png");
            this.fileIconImageList.Images.SetKeyName(1, "folder16.png");
            this.fileIconImageList.Images.SetKeyName(2, "file16.png");
            // 
            // rightTopPanel
            // 
            this.rightTopPanel.Controls.Add(this.remoteRefreshButton);
            this.rightTopPanel.Controls.Add(this.remoteUpButton);
            this.rightTopPanel.Controls.Add(this.remoteLabel);
            resources.ApplyResources(this.rightTopPanel, "rightTopPanel");
            this.rightTopPanel.Name = "rightTopPanel";
            // 
            // remoteLabel
            // 
            resources.ApplyResources(this.remoteLabel, "remoteLabel");
            this.remoteLabel.Name = "remoteLabel";
            // 
            // leftPanel
            // 
            this.leftPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.leftPanel.Controls.Add(this.consoleMessage);
            this.leftPanel.Controls.Add(this.leftListView);
            this.leftPanel.Controls.Add(this.leftTopPanel);
            resources.ApplyResources(this.leftPanel, "leftPanel");
            this.leftPanel.Name = "leftPanel";
            // 
            // leftListView
            // 
            this.leftListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            resources.ApplyResources(this.leftListView, "leftListView");
            this.leftListView.FullRowSelect = true;
            this.leftListView.Name = "leftListView";
            this.leftListView.SmallImageList = this.fileIconImageList;
            this.leftListView.UseCompatibleStateImageBehavior = false;
            this.leftListView.View = System.Windows.Forms.View.Details;
            this.leftListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.leftListView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // leftTopPanel
            // 
            this.leftTopPanel.Controls.Add(this.localRefreshButton);
            this.leftTopPanel.Controls.Add(this.localUpButton);
            this.leftTopPanel.Controls.Add(this.localLabel);
            resources.ApplyResources(this.leftTopPanel, "leftTopPanel");
            this.leftTopPanel.Name = "leftTopPanel";
            // 
            // localLabel
            // 
            resources.ApplyResources(this.localLabel, "localLabel");
            this.localLabel.Name = "localLabel";
            // 
            // localUpButton
            // 
            resources.ApplyResources(this.localUpButton, "localUpButton");
            this.localUpButton.Name = "localUpButton";
            this.localUpButton.UseVisualStyleBackColor = true;
            this.localUpButton.Click += new System.EventHandler(this.localUpButton_Click);
            // 
            // remoteUpButton
            // 
            resources.ApplyResources(this.remoteUpButton, "remoteUpButton");
            this.remoteUpButton.Name = "remoteUpButton";
            this.remoteUpButton.UseVisualStyleBackColor = true;
            this.remoteUpButton.Click += new System.EventHandler(this.remoteUpButton_Click);
            // 
            // localRefreshButton
            // 
            resources.ApplyResources(this.localRefreshButton, "localRefreshButton");
            this.localRefreshButton.Name = "localRefreshButton";
            this.localRefreshButton.UseVisualStyleBackColor = true;
            this.localRefreshButton.Click += new System.EventHandler(this.leftRefreshButton_Click);
            // 
            // remoteRefreshButton
            // 
            resources.ApplyResources(this.remoteRefreshButton, "remoteRefreshButton");
            this.remoteRefreshButton.Name = "remoteRefreshButton";
            this.remoteRefreshButton.UseVisualStyleBackColor = true;
            this.remoteRefreshButton.Click += new System.EventHandler(this.rightRefreshButton_Click);
            // 
            // FileViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FileViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.topPanel.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.rightTopPanel.ResumeLayout(false);
            this.rightTopPanel.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            this.leftTopPanel.ResumeLayout(false);
            this.leftTopPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Timer updateTimer;
        private Timer consoleTimer;
        private ToolTip mainToolTip;
        private Panel topPanel;
        private Button statsButton;
        private Button connectButton;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel mainToolStripStatusLabel;
        private Label consoleMessage;
        private TableLayoutPanel mainTableLayoutPanel;
        private Panel panel1;
        private Panel rightPanel;
        private ListView rightListView;
        private Panel leftPanel;
        private ListView leftListView;
        private Panel rightTopPanel;
        private Label remoteLabel;
        private Panel leftTopPanel;
        private Label localLabel;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ImageList fileIconImageList;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button remoteUpButton;
        private Button localUpButton;
        private Button localRefreshButton;
        private Button remoteRefreshButton;
    }
}

