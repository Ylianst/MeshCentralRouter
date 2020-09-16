/*
Copyright 2009-2020 Intel Corporation

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
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public partial class KVMViewer : Form
    {
        private KVMControl kvmControl = null;
        private KVMStats kvmStats = null;
        private MeshCentralServer server = null;
        private NodeClass node = null;
        private int state = 0;
        private RandomNumberGenerator rand = RandomNumberGenerator.Create();
        private string randomIdHex = null;
        private bool sessionIsRecorded = false;
        public webSocketClient wc = null;
        public Dictionary<string, int> userSessions = null;

        // Stats
        public long bytesIn = 0;
        public long bytesInCompressed = 0;
        public long bytesOut = 0;
        public long bytesOutCompressed = 0;

        public KVMViewer(MeshCentralServer server, NodeClass node)
        {
            InitializeComponent();
            this.Text += " - " + node.name;
            this.node = node;
            this.server = server;
            kvmControl = resizeKvmControl.KVM;
            kvmControl.parent = this;
            kvmControl.DesktopSizeChanged += KvmControl_DesktopSizeChanged;
            resizeKvmControl.ZoomToFit = true;
            UpdateStatus();
            this.MouseWheel += MainForm_MouseWheel;
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
            wc.onStateChanged += Wc_onStateChanged;
            wc.onBinaryData += Wc_onBinaryData;
            wc.onStringData += Wc_onStringData;
            wc.Start(u, server.wshash);
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
                        string u = "*/meshrelay.ashx?p=2&nodeid=" + node.nodeid + "&id=" + randomIdHex + "&rauth=" + server.rauthCookie;
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
                kvmControl.Send("2");
                kvmControl.SendCompressionLevel();
                kvmControl.SendPause(false);
                kvmControl.SendRefresh();
                UpdateStatus();
                displayMessage(null);
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
                        Dictionary <string, object> usersex = (Dictionary<string, object>)jsonAction["users"];
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
                        if (msgid == 1) { msg = "Waiting for user to grant access..."; }
                        if (msgid == 2) { msg = "Denied"; }
                        if (msgid == 3) { msg = "Failed to start remote terminal session"; } // , {0} ({1})
                        if (msgid == 4) { msg = "Timeout"; }
                        if (msgid == 5) { msg = "Received invalid network data"; }
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
                state = 0;
                wc.Dispose();
                wc = null;
                UpdateStatus();
            }
            else
            {
                // Connect
                MenuItemConnect_Click(null, null);
            }
            displayMessage(null);
        }


        public delegate void UpdateStatusHandler();

        private void UpdateStatus()
        {
            if (this.InvokeRequired) { this.Invoke(new UpdateStatusHandler(UpdateStatus)); return; }

            //if (kvmControl == null) return;
            switch (state)
            {
                case 0: // Disconnected
                    mainToolStripStatusLabel.Text = "Disconnected";
                    displaySelectComboBox.Visible = false;
                    kvmControl.Visible = false;
                    kvmControl.screenWidth = 0;
                    kvmControl.screenHeight = 0;
                    connectButton.Text = "Connect";
                    break;
                case 1: // Connecting
                    mainToolStripStatusLabel.Text = "Connecting...";
                    displaySelectComboBox.Visible = false;
                    kvmControl.Visible = false;
                    connectButton.Text = "Disconnect";
                    break;
                case 2: // Setup
                    mainToolStripStatusLabel.Text = "Setup...";
                    displaySelectComboBox.Visible = false;
                    kvmControl.Visible = false;
                    connectButton.Text = "Disconnect";
                    break;
                case 3: // Connected
                    string label = "Connected";
                    if (sessionIsRecorded) { label += ", Recorded Session"; }
                    if ((userSessions != null) && (userSessions.Count > 1)) { label += string.Format(", {0} users", userSessions.Count); }
                    label += ".";
                    mainToolStripStatusLabel.Text = label;
                    connectButton.Text = "Disconnect";
                    kvmControl.SendCompressionLevel();
                    break;
            }

            cadButton.Enabled = (state == 3);
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
            using (KVMSettingsForm form = new KVMSettingsForm())
            {
                form.Compression = kvmControl.CompressionLevel;
                form.Scaling = kvmControl.ScalingLevel;
                form.FrameRate = kvmControl.FrameRate;
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    kvmControl.SetCompressionParams(form.Compression, form.Scaling, form.FrameRate);
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

        private void resizeKvmControl_DisplaysReceived(object sender, EventArgs e)
        {
            if (kvmControl == null || kvmControl.displays.Count == 0) return;

            if (kvmControl.displays.Count > 0)
            {
                displaySelectComboBox.Visible = true;
                displaySelectComboBox.Items.Clear();
                displaySelectComboBox.Items.AddRange(kvmControl.displays.ToArray());
                if (kvmControl.currentDisp == 0xFFFF)
                {
                    displaySelectComboBox.SelectedItem = "All Displays";
                }
                else
                {
                    displaySelectComboBox.SelectedItem = "Display " + kvmControl.currentDisp;
                }
            }
            else
            {
                displaySelectComboBox.Visible = false;
                displaySelectComboBox.Items.Clear();
            }
        }

        private void displaySelectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string displayText = displaySelectComboBox.SelectedItem.ToString();
            int displaynum = 0;
            if (displayText == "All Displays") displaynum = 0xFFFF;
            if (displaynum != 0 || int.TryParse(displayText.Substring(8), out displaynum))
            {
                if (kvmControl != null) kvmControl.SendDisplay(displaynum);
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
            kvmControl.SendWindowsKey();
        }

        private void charmButton_Click(object sender, EventArgs e)
        {
            //kvmControl.SendCharmsKey();
        }

        public static void saveToRegistry(string name, string value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenSource\MeshRouter", name, value); } catch (Exception) { }
        }
        public static string loadFromRegistry(string name)
        {
            try { return Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenSource\MeshRouter", name, "").ToString(); } catch (Exception) { return ""; }
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
            } else
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
            string textData = (string)Clipboard.GetData(DataFormats.Text);
            if (textData != null)
            {
                textData = textData.Replace("\\", "\\\\").Replace("\"", "\\\"");
                server.sendCommand("{\"action\":\"msg\",\"type\":\"setclip\",\"nodeid\":\"" + node.nodeid + "\",\"data\":\"" + textData + "\"}");
            }
        }
    }
}
