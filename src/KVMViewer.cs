/*
Copyright 2009-2022 Intel Corporation

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace MeshCentralRouter
{
    public partial class KVMViewer : Form
    {
        private MainForm parent = null;
        private KVMControl kvmControl = null;
        private KVMStats kvmStats = null;
        private MeshCentralServer server = null;
        private NodeClass node = null;
        private int state = 0;
        private RandomNumberGenerator rand = RandomNumberGenerator.Create();
        private string randomIdHex = null;
        private bool sessionIsRecorded = false;
        public int consentFlags = 0;
        public webSocketClient wc = null;
        public Dictionary<string, int> userSessions = null;
        private string lastClipboardSent = null;
        private DateTime lastClipboardTime = DateTime.MinValue;
        public string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        private bool splitMode = false;
        private KVMViewerExtra[] extraDisplays = null;
        private System.Windows.Forms.Timer delayedConnectionTimer = null;
        private bool localAutoReconnect = true;
        private Dictionary<int, Button> displaySelectionButtons = new Dictionary<int, Button>();

        // Stats
        public long bytesIn = 0;
        public long bytesInCompressed = 0;
        public long bytesOut = 0;
        public long bytesOutCompressed = 0;

        public KVMViewer(MainForm parent, MeshCentralServer server, NodeClass node)
        {
            this.parent = parent;
            InitializeComponent();
            Translate.TranslateControl(this);
            this.Text += " - " + node.name;
            this.node = node;
            this.server = server;
            kvmControl = resizeKvmControl.KVM;
            kvmControl.parent = this;
            kvmControl.DesktopSizeChanged += KvmControl_DesktopSizeChanged;
            kvmControl.ScreenAreaUpdated += KvmControl_ScreenAreaUpdated;
            resizeKvmControl.ZoomToFit = true;
            UpdateStatus();
            this.MouseWheel += MainForm_MouseWheel;
            parent.ClipboardChanged += Parent_ClipboardChanged;

            mainToolTip.SetToolTip(connectButton, Translate.T(Properties.Resources.ToggleRemoteDesktopConnection, lang));
            mainToolTip.SetToolTip(cadButton, Translate.T(Properties.Resources.SendCtrlAltDelToRemoteDevice, lang));
            mainToolTip.SetToolTip(settingsButton, Translate.T(Properties.Resources.ChangeRemoteDesktopSettings, lang));
            mainToolTip.SetToolTip(clipOutboundButton, Translate.T(Properties.Resources.PushLocaClipboardToRemoteDevice, lang));
            mainToolTip.SetToolTip(clipInboundButton, Translate.T(Properties.Resources.PullClipboardFromRemoteDevice, lang));
            mainToolTip.SetToolTip(zoomButton, Translate.T(Properties.Resources.ToggleZoomToFitMode, lang));
            mainToolTip.SetToolTip(statsButton, Translate.T(Properties.Resources.DisplayConnectionStatistics, lang));

            // Load remote desktop settings
            int CompressionLevel = 60;
            try { CompressionLevel = int.Parse(Settings.GetRegValue("kvmCompression", "60")); } catch (Exception) { }
            int ScalingLevel = 1024;
            try { ScalingLevel = int.Parse(Settings.GetRegValue("kvmScaling", "1024")); } catch (Exception) { }
            int FrameRate = 100;
            try { FrameRate = int.Parse(Settings.GetRegValue("kvmFrameRate", "100")); } catch (Exception) { }
            kvmControl.SetCompressionParams(CompressionLevel, ScalingLevel, FrameRate);
            kvmControl.SwamMouseButtons = Settings.GetRegValue("kvmSwamMouseButtons", "0").Equals("1");
            kvmControl.RemoteKeyboardMap = Settings.GetRegValue("kvmSwamMouseButtons", "0").Equals("1");
            kvmControl.AutoSendClipboard = Settings.GetRegValue("kvmAutoClipboard", "0").Equals("1");
            kvmControl.AutoReconnect = Settings.GetRegValue("kvmAutoReconnect", "0").Equals("1");
        }

        private void KvmControl_ScreenAreaUpdated(Bitmap desktop, Rectangle r)
        {
            if (extraDisplays == null) return;
            foreach (KVMViewerExtra x in extraDisplays) {
                if (x != null) { x.UpdateScreenArea(desktop, r); }
            }
        }

        private void Parent_ClipboardChanged()
        {
            if (state != 3) return;
            if (kvmControl.AutoSendClipboard) { SendClipboard(); }
        }

        private delegate void SendClipboardHandler();

        private void SendClipboard()
        {
            if (this.InvokeRequired) { this.Invoke(new SendClipboardHandler(SendClipboard)); return; }
            string textData = (string)Clipboard.GetData(DataFormats.Text);
            if (textData != null)
            {
                if ((DateTime.Now.Subtract(lastClipboardTime).TotalSeconds < 20) && (lastClipboardSent != null) && (lastClipboardSent.Equals(textData))) return; // Don't resend clipboard if same and sent in last 20 seconds. This avoids clipboard loop.
                string textData2 = textData.Replace("\\", "\\\\").Replace("\"", "\\\"");
                server.sendCommand("{\"action\":\"msg\",\"type\":\"setclip\",\"nodeid\":\"" + node.nodeid + "\",\"data\":\"" + textData2 + "\"}");
                lastClipboardTime = DateTime.Now;
                lastClipboardSent = textData;
            }
        }

        public void TryAutoConnect()
        {
            if ((localAutoReconnect == false) || (kvmControl.AutoReconnect == false)) return;
            if ((state == 0) && (wc == null) && (delayedConnectionTimer == null)) {
                // Hold half a second before trying to connect
                delayedConnectionTimer = new System.Windows.Forms.Timer(this.components);
                delayedConnectionTimer.Tick += new EventHandler(updateTimerTick);
                delayedConnectionTimer.Interval = 500;
                delayedConnectionTimer.Enabled = true;
            }
        }

        private void updateTimerTick(object sender, EventArgs e)
        {
            delayedConnectionTimer.Dispose();
            delayedConnectionTimer = null;
            if ((state == 0) && (wc == null)) { MenuItemConnect_Click(this, null); }
        }

        private void KvmControl_DesktopSizeChanged(object sender, EventArgs e)
        {
            kvmControl.Visible = true;
        }

        private void Server_onStateChanged(int state)
        {
            UpdateStatus();
        }

        void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0) return;
            Control c = this.GetChildAtPoint(e.Location);
            if (c != null && c == resizeKvmControl) resizeKvmControl.MouseWheelEx(sender, e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(820, 480);
            resizeKvmControl.CenterKvmControl(false);
            topPanel.Visible = true;

            // Restore Window Location
            string locationStr = Settings.GetRegValue("kvmlocation", "");
            if (locationStr != null)
            {
                string[] locationSplit = locationStr.Split(',');
                if (locationSplit.Length == 4)
                {
                    try
                    {
                        var x = int.Parse(locationSplit[0]);
                        var y = int.Parse(locationSplit[1]);
                        var w = int.Parse(locationSplit[2]);
                        var h = int.Parse(locationSplit[3]);
                        Point p = new Point(x, y);
                        if (isPointVisibleOnAScreen(p))
                        {
                            Location = p;
                            if ((w > 50) && (h > 50)) { Size = new Size(w, h); }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        public void OnScreenChanged()
        {
            resizeKvmControl.CenterKvmControl(true);
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            node.desktopViewer = null;
            closeKvmStats();
            Close();
        }

        public void MenuItemConnect_Click(object sender, EventArgs e)
        {
            if (wc != null) return;
            byte[] randomid = new byte[10];
            rand.GetBytes(randomid);
            randomIdHex = BitConverter.ToString(randomid).Replace("-", string.Empty);

            state = 1;
            string ux = server.wsurl.ToString().Replace("/control.ashx", "/");
            int i = ux.IndexOf("?");
            if (i >= 0) { ux = ux.Substring(0, i); }
            Uri u = new Uri(ux + "meshrelay.ashx?browser=1&p=2&nodeid=" + node.nodeid + "&id=" + randomIdHex + "&auth=" + server.authCookie);
            wc = new webSocketClient();
            wc.debug = server.debug;
            wc.onStateChanged += Wc_onStateChanged;
            wc.onBinaryData += Wc_onBinaryData;
            wc.onStringData += Wc_onStringData;
            wc.TLSCertCheck = webSocketClient.TLSCertificateCheck.Fingerprint;
            wc.Start(u, server.wshash, null);
        }

        private void Wc_onStateChanged(webSocketClient sender, webSocketClient.ConnectionStates wsstate)
        {
            switch (wsstate)
            {
                case webSocketClient.ConnectionStates.Disconnected:
                    {
                        // Disconnect
                        state = 0;
                        wc.Dispose();
                        wc = null;
                        kvmControl.DetacheKeyboard();
                        break;
                    }
                case webSocketClient.ConnectionStates.Connecting:
                    {
                        state = 1;
                        displayMessage(null);
                        break;
                    }
                case webSocketClient.ConnectionStates.Connected:
                    {
                        // Reset stats
                        bytesIn = 0;
                        bytesInCompressed = 0;
                        bytesOut = 0;
                        bytesOutCompressed = 0;

                        state = 2;

                        string u = "*" + server.wsurl.AbsolutePath.Replace("control.ashx", "meshrelay.ashx") + "?p=2&nodeid=" + node.nodeid + "&id=" + randomIdHex + "&rauth=" + server.rauthCookie;
                        server.sendCommand("{ \"action\": \"msg\", \"type\": \"tunnel\", \"nodeid\": \"" + node.nodeid + "\", \"value\": \"" + u.ToString() + "\", \"usage\": 2 }");
                        displayMessage(null);
                        break;
                    }
            }
            UpdateStatus();
        }

        private void Wc_onStringData(webSocketClient sender, string data, int orglen)
        {
            bytesIn += data.Length;
            bytesInCompressed += orglen;

            if ((state == 2) && ((data == "c") || (data == "cr")))
            {
                if (data == "cr") { sessionIsRecorded = true; }
                state = 3;

                // Send any connection options here
                if (consentFlags != 0) { kvmControl.Send("{ \"type\": \"options\", \"consent\": " + consentFlags + " }"); }

                // Send remote desktop protocol (2)
                kvmControl.Send("2");
                kvmControl.SendCompressionLevel();
                kvmControl.SendPause(false);
                kvmControl.SendRefresh();
                UpdateStatus();
                displayMessage(null);

                // Send clipboard
                if (kvmControl.AutoSendClipboard) { SendClipboard(); }

                return;
            }
            if (state != 3) return;

            // Parse the received JSON
            Dictionary<string, object> jsonAction = new Dictionary<string, object>();
            jsonAction = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(data);
            if ((jsonAction == null) || (jsonAction.ContainsKey("type") == false) || (jsonAction["type"].GetType() != typeof(string))) return;

            string action = jsonAction["type"].ToString();
            switch (action)
            {
                case "metadata":
                    {
                        if ((jsonAction.ContainsKey("users") == false) || (jsonAction["users"] == null)) return;
                        Dictionary<string, object> usersex = (Dictionary<string, object>)jsonAction["users"];
                        userSessions = new Dictionary<string, int>();
                        foreach (string user in usersex.Keys) { userSessions.Add(user, (int)usersex[user]); }
                        UpdateStatus();
                        break;
                    }
                case "console":
                    {
                        string msg = null;
                        int msgid = -1;
                        if ((jsonAction.ContainsKey("msg")) && (jsonAction["msg"] != null)) { msg = jsonAction["msg"].ToString(); }
                        if (jsonAction.ContainsKey("msgid")) { msgid = (int)jsonAction["msgid"]; }
                        if (msgid == 1) { msg = Translate.T(Properties.Resources.WaitingForUserToGrantAccess, lang); }
                        if (msgid == 2) { msg = Translate.T(Properties.Resources.Denied, lang); }
                        if (msgid == 3) { msg = Translate.T(Properties.Resources.FailedToStartRemoteDesktopSession, lang); }
                        if (msgid == 4) { msg = Translate.T(Properties.Resources.Timeout, lang); }
                        if (msgid == 5) { msg = Translate.T(Properties.Resources.ReceivedInvalidNetworkData, lang); }
                        displayMessage(msg);
                        break;
                    }
            }
        }

        private void Wc_onBinaryData(webSocketClient sender, byte[] data, int offset, int length, int orglen)
        {
            bytesIn += length;
            bytesInCompressed += orglen;

            if (state != 3) return;
            kvmControl.ProcessData(data, offset, length);
        }

        private void MenuItemDisconnect_Click(object sender, EventArgs e)
        {
            if (wc != null)
            {
                // Disconnect
                if (splitMode) { splitButton_Click(this, null); }
                splitButton.Visible = false;
                state = 0;
                wc.Dispose();
                wc = null;
                UpdateStatus();
                localAutoReconnect = false;
            }
            else
            {
                // Connect
                if (sender != null) { consentFlags = 0; }
                MenuItemConnect_Click(null, null);
                kvmControl.AttachKeyboard();
                localAutoReconnect = true;
            }
            displayMessage(null);
        }


        public delegate void UpdateStatusHandler();

        private void UpdateStatus()
        {
            if (this.InvokeRequired) { try { this.Invoke(new UpdateStatusHandler(UpdateStatus)); } catch (Exception) { } return; }

            //if (kvmControl == null) return;
            switch (state)
            {
                case 0: // Disconnected
                    mainToolStripStatusLabel.Text = Translate.T(Properties.Resources.Disconnected, lang);
                    extraButtonsPanel.Visible = false;
                    kvmControl.Visible = false;
                    kvmControl.screenWidth = 0;
                    kvmControl.screenHeight = 0;
                    connectButton.Text = Translate.T(Properties.Resources.Connect, lang);
                    break;
                case 1: // Connecting
                    mainToolStripStatusLabel.Text = Translate.T(Properties.Resources.Connecting, lang);
                    extraButtonsPanel.Visible = false;
                    kvmControl.Visible = false;
                    connectButton.Text = Translate.T(Properties.Resources.Disconnect, lang);
                    break;
                case 2: // Setup
                    mainToolStripStatusLabel.Text = "Setup...";
                    extraButtonsPanel.Visible = false;
                    kvmControl.Visible = false;
                    connectButton.Text = Translate.T(Properties.Resources.Disconnect, lang);
                    break;
                case 3: // Connected
                    string label = Translate.T(Properties.Resources.Connected, lang);
                    if (sessionIsRecorded) { label += Translate.T(Properties.Resources.RecordedSession, lang); }
                    if ((userSessions != null) && (userSessions.Count > 1)) { label += string.Format(Translate.T(Properties.Resources.AddXUsers, lang), userSessions.Count); }
                    label += ".";
                    mainToolStripStatusLabel.Text = label;
                    connectButton.Text = Translate.T(Properties.Resources.Disconnect, lang);
                    kvmControl.SendCompressionLevel();
                    break;
            }

            cadButton.Enabled = (state == 3);
            openRemoteFilesButton.Enabled = (state == 3 && (node.agentcaps & 4) != 0);
            if ((kvmControl.AutoSendClipboard) && ((server.features2 & 0x1000) == 0)) // 0x1000 Clipboard Set
            {
                clipInboundButton.Visible = false;
                clipOutboundButton.Visible = false;
            }
            else
            {
                clipInboundButton.Visible = ((server.features2 & 0x0800) == 0); // 0x0800 Clipboard Get
                clipOutboundButton.Visible = ((server.features2 & 0x1000) == 0); // 0x1000 Clipboard Set
            }
            clipInboundButton.Enabled = (state == 3);
            clipOutboundButton.Enabled = (state == 3);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (wc != null)
            {
                // Disconnect
                state = 0;
                wc.Dispose();
                wc = null;
                UpdateStatus();
            }
            node.desktopViewer = null;
            closeKvmStats();

            // Save window location
            Settings.SetRegValue("kvmlocation", Location.X + "," + Location.Y + "," + Size.Width + "," + Size.Height);

            // Close any extra windows
            extraScreenClosed();
        }

        private void toolStripMenuItem2_DropDownOpening(object sender, EventArgs e)
        {
            //MenuItemConnect.Enabled = (kvmControl.State == KVMControl.ConnectState.Disconnected);
            //MenuItemDisconnect.Enabled = (kvmControl.State != KVMControl.ConnectState.Disconnected);
            //serverConnectToolStripMenuItem.Enabled = (server == null && kvmControl.State == KVMControl.ConnectState.Disconnected);
            //serviceDisconnectToolStripMenuItem.Enabled = (server != null && server.CurrentState != MeshSwarmServer.State.Disconnected);
        }

        private void kvmControl_StateChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kvmControl == null) return;
            using (KVMSettingsForm form = new KVMSettingsForm(server.features2))
            {
                form.Compression = kvmControl.CompressionLevel;
                form.Scaling = kvmControl.ScalingLevel;
                form.FrameRate = kvmControl.FrameRate;
                form.SwamMouseButtons = kvmControl.SwamMouseButtons;
                form.RemoteKeyboardMap = kvmControl.RemoteKeyboardMap;
                form.AutoSendClipboard = kvmControl.AutoSendClipboard;
                form.AutoReconnect = kvmControl.AutoReconnect;
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    kvmControl.SetCompressionParams(form.Compression, form.Scaling, form.FrameRate);
                    kvmControl.SwamMouseButtons = form.SwamMouseButtons;
                    kvmControl.RemoteKeyboardMap = form.RemoteKeyboardMap;
                    kvmControl.AutoReconnect = form.AutoReconnect;

                    Settings.SetRegValue("kvmCompression", kvmControl.CompressionLevel.ToString());
                    Settings.SetRegValue("kvmScaling", kvmControl.ScalingLevel.ToString());
                    Settings.SetRegValue("kvmFrameRate", kvmControl.FrameRate.ToString());
                    Settings.SetRegValue("kvmSwamMouseButtons", kvmControl.SwamMouseButtons ? "1" : "0");
                    Settings.SetRegValue("kvmRemoteKeyboardMap", kvmControl.RemoteKeyboardMap ? "1" : "0");
                    Settings.SetRegValue("kvmAutoReconnect", kvmControl.AutoReconnect ? "1" : "0");

                    if (kvmControl.AutoSendClipboard != form.AutoSendClipboard)
                    {
                        kvmControl.AutoSendClipboard = form.AutoSendClipboard;
                        Settings.SetRegValue("kvmAutoClipboard", kvmControl.AutoSendClipboard ? "1" : "0");
                        if (kvmControl.AutoSendClipboard == true) { Parent_ClipboardChanged(); }
                    }
                    UpdateStatus();
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (kvmControl != null) kvmControl.SendPause(WindowState == FormWindowState.Minimized);
        }

        private void sendCtrlAltDelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kvmControl != null) kvmControl.SendCtrlAltDel();
        }

        private void debugButton_Click(object sender, EventArgs e)
        {
            if (kvmControl != null) kvmControl.debugmode = !kvmControl.debugmode;
        }

        private void zoomButton_Click(object sender, EventArgs e)
        {
            resizeKvmControl.ZoomToFit = !resizeKvmControl.ZoomToFit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resizeKvmControl_DisplaysReceived(object sender, EventArgs e)
        {
            if (kvmControl == null || kvmControl.displays.Count == 0) return;

            if (kvmControl.displays.Count > 0)
            {
                extraButtonsPanel.Visible = true;
                extraButtonsPanel.Controls.Clear();
                displaySelectionButtons.Clear();
                foreach (ushort displayNum in kvmControl.displays)
                {
                    if (displayNum == 0xFFFF)
                    {
                        Button b = new Button();
                        b.ImageList = displaySelectorImageList;
                        b.ImageIndex = (kvmControl.currentDisp == displayNum) ? 2 : 3; // All displayes
                        b.Width = 32;
                        b.Height = 32;
                        mainToolTip.SetToolTip(b, Translate.T(Properties.Resources.AllDisplays, lang));
                        b.Click += new System.EventHandler(this.displaySelectComboBox_SelectionChangeCommitted);
                        b.Tag = displayNum;
                        b.Dock = DockStyle.Left;
                        extraButtonsPanel.Controls.Add(b);
                        displaySelectionButtons.Add(displayNum, b);
                    }
                    else
                    {
                        Button b = new Button();
                        b.ImageList = displaySelectorImageList;
                        b.ImageIndex = (kvmControl.currentDisp == displayNum) ? 0 : 1; // One display grayed out
                        b.Width = 32;
                        b.Height = 32;
                        mainToolTip.SetToolTip(b, string.Format(Translate.T(Properties.Resources.DisplayX, lang), displayNum));
                        b.Click += new System.EventHandler(this.displaySelectComboBox_SelectionChangeCommitted);
                        b.Tag = displayNum;
                        b.Dock = DockStyle.Left;
                        extraButtonsPanel.Controls.Add(b);
                        displaySelectionButtons.Add(displayNum, b);
                    }
                }
            }
            else
            {
                extraButtonsPanel.Visible = false;
                extraButtonsPanel.Controls.Clear();
                displaySelectionButtons.Clear();
            }

            // If there are many displays and all displays is selected, enable split/join button.
            splitButton.Visible = ((kvmControl.currentDisp == 65535) && (kvmControl.displays.Count > 1));
        }

        private void displaySelectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (splitMode) { splitButton_Click(this, null); }
            if (kvmControl != null) {
                ushort displayNum = (ushort)((Button)sender).Tag;
                kvmControl.SendDisplay(displayNum);
            }
        }

        private void resizeKvmControl_TouchEnabledChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void commandsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            //if (server == null) return;
            //if (commandsToolStripMenuItem.Checked) server.ShowCommandViewer(); else server.HideCommandViewer();
        }

        private void emulateTouchToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            //kvmControl.emulateTouch = emulateTouchToolStripMenuItem.Checked;
        }

        private void packetsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            //kvmControl.ShowPackets(packetsToolStripMenuItem.Checked);
        }

        private void kVMCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //kvmControl.ShowCommands(kVMCommandsToolStripMenuItem.Checked);
        }

        private void winButton_Click(object sender, EventArgs e)
        {
            //kvmControl.SendWindowsKey();
        }

        private void charmButton_Click(object sender, EventArgs e)
        {
            //kvmControl.SendCharmsKey();
        }

        public delegate void displayMessageHandler(string msg);
        public void displayMessage(string msg)
        {
            if (this.InvokeRequired) { this.Invoke(new displayMessageHandler(displayMessage), msg); return; }
            if (msg == null)
            {
                consoleMessage.Visible = false;
                consoleTimer.Enabled = false;
            }
            else
            {
                consoleMessage.Text = msg;
                consoleMessage.Visible = true;
                //consoleTimer.Enabled = true;
            }
        }

        private void consoleTimer_Tick(object sender, EventArgs e)
        {
            consoleMessage.Visible = false;
            consoleTimer.Enabled = false;
        }

        private void statsButton_Click(object sender, EventArgs e)
        {
            if (kvmStats == null)
            {
                kvmStats = new KVMStats(this);
                kvmStats.Show(this);
            }
            else
            {
                kvmStats.Focus();
            }
        }

        public void closeKvmStats()
        {
            if (kvmStats == null) return;
            kvmStats.Close();
            kvmStats = null;
        }

        private void clipInboundButton_Click(object sender, EventArgs e)
        {
            //string textData = "abc";
            //Clipboard.SetData(DataFormats.Text, (Object)textData);
            server.sendCommand("{\"action\":\"msg\",\"type\":\"getclip\",\"nodeid\":\"" + node.nodeid + "\"}");
        }

        private void clipOutboundButton_Click(object sender, EventArgs e)
        {
            SendClipboard();
        }

        private void resizeKvmControl_Enter(object sender, EventArgs e)
        {
            kvmControl.AttachKeyboard();
        }

        private void resizeKvmControl_Leave(object sender, EventArgs e)
        {
            kvmControl.DetacheKeyboard();
        }

        private void KVMViewer_Deactivate(object sender, EventArgs e)
        {
            kvmControl.DetacheKeyboard();
        }

        private void KVMViewer_Activated(object sender, EventArgs e)
        {
            kvmControl.AttachKeyboard();
        }

        bool isPointVisibleOnAScreen(Point p)
        {
            foreach (Screen s in Screen.AllScreens) { if ((p.X < s.Bounds.Right) && (p.X > s.Bounds.Left) && (p.Y > s.Bounds.Top) && (p.Y < s.Bounds.Bottom)) return true; }
            return false;
        }

        private void askConsentBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consentFlags = 0x0008 + 0x0040; // Consent Prompt + Privacy bar
            MenuItemDisconnect_Click(null, null);
        }

        private void askConsentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consentFlags = 0x0008; // Consent Prompt
            MenuItemDisconnect_Click(null, null);
        }

        private void privacyBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consentFlags = 0x0040; // Privacy bar
            MenuItemDisconnect_Click(null, null);
        }

        private void consentContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (wc != null) { e.Cancel = true; }
        }

        public void extraScreenClosed()
        {
            if (splitMode) { splitButton_Click(this, null); }
        }

        private void splitButton_Click(object sender, EventArgs e)
        {
            if (splitMode)
            {
                kvmControl.cropDisplay(Point.Empty, Rectangle.Empty);
                splitButton.Text = Translate.T(Properties.Resources.Split, lang);
                splitMode = false;
                if (extraDisplays != null)
                {
                    // Close all extra displays
                    for (int i = 0; i < extraDisplays.Length; i++)
                    {
                        KVMViewerExtra extraDisplay = extraDisplays[i];
                        extraDisplay.Close();
                    }
                    extraDisplays = null;
                }
            }
            else if ((kvmControl.displayInfo != null) && (kvmControl.displayInfo.Length > 1))
            {
                int minx = 0;
                int miny = 0;
                foreach (Rectangle r in kvmControl.displayInfo) { if (r.X < minx) { minx = r.X; } if (r.Y < miny) { miny = r.Y; } }
                kvmControl.cropDisplay(new Point(minx, miny), kvmControl.displayInfo[0]);
                splitButton.Text = Translate.T(Properties.Resources.Join, lang);
                splitMode = true;

                // Open extra displays
                extraDisplays = new KVMViewerExtra[kvmControl.displayInfo.Length - 1];
                for (int i = 1; i < kvmControl.displayInfo.Length; i++)
                {
                    KVMViewerExtra extraDisplay = new KVMViewerExtra(parent, this, node, kvmControl, i);
                    extraDisplays[i - 1] = extraDisplay;
                    extraDisplay.Show(parent);
                }
            }
        }

        private void openRemoteFilesButton_Click(object sender, EventArgs e)
        {
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device

            if (node.fileViewer == null)
            {
                node.fileViewer = new FileViewer(server, node);
                node.fileViewer.Show();
                node.fileViewer.MenuItemConnect_Click(null, null);
            }
            else
            {
                node.fileViewer.Focus();
            }
        }

        private void chatButton_Click(object sender, EventArgs e)
        {
            if (kvmControl == null) return;
            server.sendCommand("{\"action\":\"meshmessenger\",\"nodeid\":\"" + node.nodeid + "\"}");
            string url = "https://" + server.serverinfo["name"];
            if (server.serverinfo.TryGetValue("port", out var value1) && value1 is int portNumber && portNumber != 443)
            {
                url += ":" + portNumber;
            }
            if (server.serverinfo.TryGetValue("domainsuffix", out var value) && value is string domainSuffix && !string.IsNullOrEmpty(domainSuffix))
            {
                url += "/" + domainSuffix;
            }
            url += "/messenger?id=meshmessenger/" + Uri.EscapeDataString(node.nodeid) + "/" + Uri.EscapeDataString(server.userid) + "&title=" + node.name;
            if ((server.authCookie != null) && (server.authCookie != "")) { url += "&auth=" + server.authCookie; }
            try
            {
                if (server.debug) { try { File.AppendAllText("debug.log", "Opening chat window locally using ProcessStartInfo\r\n"); } catch (Exception) { } }
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception)
            {
                try
                {
                    if (server.debug) { try { File.AppendAllText("debug.log", "Opening chat window locally using cmd\r\n"); } catch (Exception) { } }
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                catch (Exception)
                {
                    if (server.debug) { try { File.AppendAllText("debug.log", "Failed to open chat window locally\r\n"); } catch (Exception) { } }
                }
            }
        }
    }
}
