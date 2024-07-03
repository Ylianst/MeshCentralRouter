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
            this.remoteRefreshButton = new System.Windows.Forms.Button();
            this.remoteUpButton = new System.Windows.Forms.Button();
            this.localRefreshButton = new System.Windows.Forms.Button();
            this.localUpButton = new System.Windows.Forms.Button();
            this.remoteNewFolderButton = new System.Windows.Forms.Button();
            this.remoteRootButton = new System.Windows.Forms.Button();
            this.localRootButton = new System.Windows.Forms.Button();
            this.remoteDeleteButton = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.uploadButton = new System.Windows.Forms.Button();
            this.localDeleteButton = new System.Windows.Forms.Button();
            this.localNewFolderButton = new System.Windows.Forms.Button();
            this.remoteZipButton = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.mainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.consoleMessage = new System.Windows.Forms.Label();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.rightListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.remoteContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileIconImageList = new System.Windows.Forms.ImageList(this.components);
            this.rightTopPanel = new System.Windows.Forms.Panel();
            this.remoteDirectoryPath = new System.Windows.Forms.ToolStrip();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.leftListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.localContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.leftTopPanel = new System.Windows.Forms.Panel();
            this.localDirectoryPath = new System.Windows.Forms.ToolStrip();
            this.topPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.remoteContextMenuStrip.SuspendLayout();
            this.rightTopPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.localContextMenuStrip.SuspendLayout();
            this.leftTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
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
            // remoteRefreshButton
            // 
            resources.ApplyResources(this.remoteRefreshButton, "remoteRefreshButton");
            this.remoteRefreshButton.Image = global::MeshCentralRouter.Properties.Resources.refresh16;
            this.remoteRefreshButton.Name = "remoteRefreshButton";
            this.mainToolTip.SetToolTip(this.remoteRefreshButton, resources.GetString("remoteRefreshButton.ToolTip"));
            this.remoteRefreshButton.UseVisualStyleBackColor = true;
            this.remoteRefreshButton.Click += new System.EventHandler(this.rightRefreshButton_Click);
            // 
            // remoteUpButton
            // 
            resources.ApplyResources(this.remoteUpButton, "remoteUpButton");
            this.remoteUpButton.Image = global::MeshCentralRouter.Properties.Resources.folderup16;
            this.remoteUpButton.Name = "remoteUpButton";
            this.mainToolTip.SetToolTip(this.remoteUpButton, resources.GetString("remoteUpButton.ToolTip"));
            this.remoteUpButton.UseVisualStyleBackColor = true;
            this.remoteUpButton.Click += new System.EventHandler(this.remoteUpButton_Click);
            // 
            // localRefreshButton
            // 
            this.localRefreshButton.Image = global::MeshCentralRouter.Properties.Resources.refresh16;
            resources.ApplyResources(this.localRefreshButton, "localRefreshButton");
            this.localRefreshButton.Name = "localRefreshButton";
            this.mainToolTip.SetToolTip(this.localRefreshButton, resources.GetString("localRefreshButton.ToolTip"));
            this.localRefreshButton.UseVisualStyleBackColor = true;
            this.localRefreshButton.Click += new System.EventHandler(this.leftRefreshButton_Click);
            // 
            // localUpButton
            // 
            resources.ApplyResources(this.localUpButton, "localUpButton");
            this.localUpButton.Image = global::MeshCentralRouter.Properties.Resources.folderup16;
            this.localUpButton.Name = "localUpButton";
            this.mainToolTip.SetToolTip(this.localUpButton, resources.GetString("localUpButton.ToolTip"));
            this.localUpButton.UseVisualStyleBackColor = true;
            this.localUpButton.Click += new System.EventHandler(this.localUpButton_Click);
            // 
            // remoteNewFolderButton
            // 
            resources.ApplyResources(this.remoteNewFolderButton, "remoteNewFolderButton");
            this.remoteNewFolderButton.Image = global::MeshCentralRouter.Properties.Resources.foldernew16;
            this.remoteNewFolderButton.Name = "remoteNewFolderButton";
            this.mainToolTip.SetToolTip(this.remoteNewFolderButton, resources.GetString("remoteNewFolderButton.ToolTip"));
            this.remoteNewFolderButton.UseVisualStyleBackColor = true;
            this.remoteNewFolderButton.Click += new System.EventHandler(this.remoteNewFolderButton_Click);
            // 
            // remoteRootButton
            // 
            resources.ApplyResources(this.remoteRootButton, "remoteRootButton");
            this.remoteRootButton.Image = global::MeshCentralRouter.Properties.Resources.folderroot16;
            this.remoteRootButton.Name = "remoteRootButton";
            this.mainToolTip.SetToolTip(this.remoteRootButton, resources.GetString("remoteRootButton.ToolTip"));
            this.remoteRootButton.UseVisualStyleBackColor = true;
            this.remoteRootButton.Click += new System.EventHandler(this.remoteRootButton_Click);
            // 
            // localRootButton
            // 
            resources.ApplyResources(this.localRootButton, "localRootButton");
            this.localRootButton.Image = global::MeshCentralRouter.Properties.Resources.folderroot16;
            this.localRootButton.Name = "localRootButton";
            this.mainToolTip.SetToolTip(this.localRootButton, resources.GetString("localRootButton.ToolTip"));
            this.localRootButton.UseVisualStyleBackColor = true;
            this.localRootButton.Click += new System.EventHandler(this.localRootButton_Click);
            // 
            // remoteDeleteButton
            // 
            resources.ApplyResources(this.remoteDeleteButton, "remoteDeleteButton");
            this.remoteDeleteButton.Image = global::MeshCentralRouter.Properties.Resources.delete16;
            this.remoteDeleteButton.Name = "remoteDeleteButton";
            this.mainToolTip.SetToolTip(this.remoteDeleteButton, resources.GetString("remoteDeleteButton.ToolTip"));
            this.remoteDeleteButton.UseVisualStyleBackColor = true;
            this.remoteDeleteButton.Click += new System.EventHandler(this.remoteDeleteButton_Click);
            // 
            // downloadButton
            // 
            resources.ApplyResources(this.downloadButton, "downloadButton");
            this.downloadButton.Image = global::MeshCentralRouter.Properties.Resources.arrowleft16;
            this.downloadButton.Name = "downloadButton";
            this.mainToolTip.SetToolTip(this.downloadButton, resources.GetString("downloadButton.ToolTip"));
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // uploadButton
            // 
            resources.ApplyResources(this.uploadButton, "uploadButton");
            this.uploadButton.Image = global::MeshCentralRouter.Properties.Resources.arrowright16;
            this.uploadButton.Name = "uploadButton";
            this.mainToolTip.SetToolTip(this.uploadButton, resources.GetString("uploadButton.ToolTip"));
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // localDeleteButton
            // 
            resources.ApplyResources(this.localDeleteButton, "localDeleteButton");
            this.localDeleteButton.Image = global::MeshCentralRouter.Properties.Resources.delete16;
            this.localDeleteButton.Name = "localDeleteButton";
            this.mainToolTip.SetToolTip(this.localDeleteButton, resources.GetString("localDeleteButton.ToolTip"));
            this.localDeleteButton.UseVisualStyleBackColor = true;
            this.localDeleteButton.Click += new System.EventHandler(this.localDeleteButton_Click);
            // 
            // localNewFolderButton
            // 
            resources.ApplyResources(this.localNewFolderButton, "localNewFolderButton");
            this.localNewFolderButton.Image = global::MeshCentralRouter.Properties.Resources.foldernew16;
            this.localNewFolderButton.Name = "localNewFolderButton";
            this.mainToolTip.SetToolTip(this.localNewFolderButton, resources.GetString("localNewFolderButton.ToolTip"));
            this.localNewFolderButton.UseVisualStyleBackColor = true;
            this.localNewFolderButton.Click += new System.EventHandler(this.localNewFolderButton_Click);
            // 
            // remoteZipButton
            // 
            resources.ApplyResources(this.remoteZipButton, "remoteZipButton");
            this.remoteZipButton.Image = global::MeshCentralRouter.Properties.Resources.folderzip;
            this.remoteZipButton.Name = "remoteZipButton";
            this.mainToolTip.SetToolTip(this.remoteZipButton, resources.GetString("remoteZipButton.ToolTip"));
            this.remoteZipButton.UseVisualStyleBackColor = true;
            this.remoteZipButton.Click += new System.EventHandler(this.remoteZipButton_Click);
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topPanel.Controls.Add(this.statsButton);
            this.topPanel.Controls.Add(this.connectButton);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripStatusLabel});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
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
            this.panel1.Controls.Add(this.downloadButton);
            this.panel1.Controls.Add(this.uploadButton);
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
            this.rightListView.AllowDrop = true;
            this.rightListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.rightListView.ContextMenuStrip = this.remoteContextMenuStrip;
            resources.ApplyResources(this.rightListView, "rightListView");
            this.rightListView.FullRowSelect = true;
            this.rightListView.HideSelection = false;
            this.rightListView.Name = "rightListView";
            this.rightListView.SmallImageList = this.fileIconImageList;
            this.rightListView.UseCompatibleStateImageBehavior = false;
            this.rightListView.View = System.Windows.Forms.View.Details;
            this.rightListView.SelectedIndexChanged += new System.EventHandler(this.rightListView_SelectedIndexChanged);
            this.rightListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.rightListView_DragDrop);
            this.rightListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.rightListView_DragEnter);
            this.rightListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rightListView_MouseDoubleClick);
            this.rightListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rightListView_MouseMove);
            this.rightListView.Resize += new System.EventHandler(this.rightListView_Resize);
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // remoteContextMenuStrip
            // 
            this.remoteContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.compressToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.refreshToolStripMenuItem});
            this.remoteContextMenuStrip.Name = "remoteContextMenuStrip";
            resources.ApplyResources(this.remoteContextMenuStrip, "remoteContextMenuStrip");
            this.remoteContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.remoteContextMenuStrip_Opening);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            resources.ApplyResources(this.renameToolStripMenuItem, "renameToolStripMenuItem");
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // compressToolStripMenuItem
            // 
            this.compressToolStripMenuItem.Name = "compressToolStripMenuItem";
            resources.ApplyResources(this.compressToolStripMenuItem, "compressToolStripMenuItem");
            this.compressToolStripMenuItem.Click += new System.EventHandler(this.remoteZipButton_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.remoteDeleteButton_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            resources.ApplyResources(this.refreshToolStripMenuItem, "refreshToolStripMenuItem");
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.rightRefreshButton_Click);
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
            this.rightTopPanel.Controls.Add(this.remoteDirectoryPath);
            this.rightTopPanel.Controls.Add(this.remoteZipButton);
            this.rightTopPanel.Controls.Add(this.remoteDeleteButton);
            this.rightTopPanel.Controls.Add(this.remoteRootButton);
            this.rightTopPanel.Controls.Add(this.remoteNewFolderButton);
            this.rightTopPanel.Controls.Add(this.remoteRefreshButton);
            this.rightTopPanel.Controls.Add(this.remoteUpButton);
            resources.ApplyResources(this.rightTopPanel, "rightTopPanel");
            this.rightTopPanel.Name = "rightTopPanel";
            // 
            // remoteDirectoryPath
            // 
            resources.ApplyResources(this.remoteDirectoryPath, "remoteDirectoryPath");
            this.remoteDirectoryPath.GripMargin = new System.Windows.Forms.Padding(0);
            this.remoteDirectoryPath.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.remoteDirectoryPath.Name = "remoteDirectoryPath";
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
            this.leftListView.AllowDrop = true;
            this.leftListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader6});
            this.leftListView.ContextMenuStrip = this.localContextMenuStrip;
            resources.ApplyResources(this.leftListView, "leftListView");
            this.leftListView.FullRowSelect = true;
            this.leftListView.HideSelection = false;
            this.leftListView.Name = "leftListView";
            this.leftListView.SmallImageList = this.fileIconImageList;
            this.leftListView.UseCompatibleStateImageBehavior = false;
            this.leftListView.View = System.Windows.Forms.View.Details;
            this.leftListView.SelectedIndexChanged += new System.EventHandler(this.leftListView_SelectedIndexChanged);
            this.leftListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.leftListView_DragDrop);
            this.leftListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.leftListView_DragEnter);
            this.leftListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.leftListView_MouseDoubleClick);
            this.leftListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.leftListView_MouseMove);
            this.leftListView.Resize += new System.EventHandler(this.leftListView_Resize);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // localContextMenuStrip
            // 
            this.localContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem1,
            this.deleteToolStripMenuItem1,
            this.toolStripMenuItem2,
            this.refreshToolStripMenuItem1});
            this.localContextMenuStrip.Name = "localContextMenuStrip";
            resources.ApplyResources(this.localContextMenuStrip, "localContextMenuStrip");
            this.localContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.localContextMenuStrip_Opening);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            resources.ApplyResources(this.renameToolStripMenuItem1, "renameToolStripMenuItem1");
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameToolStripMenuItem1_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            resources.ApplyResources(this.deleteToolStripMenuItem1, "deleteToolStripMenuItem1");
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.localDeleteButton_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // refreshToolStripMenuItem1
            // 
            this.refreshToolStripMenuItem1.Name = "refreshToolStripMenuItem1";
            resources.ApplyResources(this.refreshToolStripMenuItem1, "refreshToolStripMenuItem1");
            this.refreshToolStripMenuItem1.Click += new System.EventHandler(this.leftRefreshButton_Click);
            // 
            // leftTopPanel
            // 
            this.leftTopPanel.Controls.Add(this.localDirectoryPath);
            this.leftTopPanel.Controls.Add(this.localDeleteButton);
            this.leftTopPanel.Controls.Add(this.localNewFolderButton);
            this.leftTopPanel.Controls.Add(this.localRootButton);
            this.leftTopPanel.Controls.Add(this.localRefreshButton);
            this.leftTopPanel.Controls.Add(this.localUpButton);
            resources.ApplyResources(this.leftTopPanel, "leftTopPanel");
            this.leftTopPanel.Name = "leftTopPanel";
            // 
            // localDirectoryPath
            // 
            resources.ApplyResources(this.localDirectoryPath, "localDirectoryPath");
            this.localDirectoryPath.GripMargin = new System.Windows.Forms.Padding(0);
            this.localDirectoryPath.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.localDirectoryPath.Name = "localDirectoryPath";
            this.localDirectoryPath.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // FileViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.statusStrip);
            this.Name = "FileViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.topPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.remoteContextMenuStrip.ResumeLayout(false);
            this.rightTopPanel.ResumeLayout(false);
            this.rightTopPanel.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            this.localContextMenuStrip.ResumeLayout(false);
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
        private StatusStrip statusStrip;
        private ToolStripStatusLabel mainToolStripStatusLabel;
        private Label consoleMessage;
        private TableLayoutPanel mainTableLayoutPanel;
        private Panel panel1;
        private Panel rightPanel;
        private ListView rightListView;
        private Panel leftPanel;
        private ListView leftListView;
        private Panel rightTopPanel;
        private Panel leftTopPanel;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ImageList fileIconImageList;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button remoteUpButton;
        private Button localUpButton;
        private Button localRefreshButton;
        private Button remoteRefreshButton;
        private Button remoteNewFolderButton;
        private Button remoteRootButton;
        private Button localRootButton;
        private Button remoteDeleteButton;
        private ContextMenuStrip remoteContextMenuStrip;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private Button uploadButton;
        private Button downloadButton;
        private Button localDeleteButton;
        private Button localNewFolderButton;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ContextMenuStrip localContextMenuStrip;
        private ToolStripMenuItem renameToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem refreshToolStripMenuItem1;
        private ToolStripMenuItem compressToolStripMenuItem;
        private Button remoteZipButton;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ToolStrip remoteDirectoryPath;
        private ToolStrip localDirectoryPath;
    }
}
