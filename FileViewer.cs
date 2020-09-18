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
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public partial class FileViewer : Form
    {
        private MeshCentralServer server = null;
        private NodeClass node = null;
        private int state = 0;
        private RandomNumberGenerator rand = RandomNumberGenerator.Create();
        private string randomIdHex = null;
        public webSocketClient wc = null;
        public Dictionary<string, int> userSessions = null;
        public bool sessionIsRecorded = false;
        public DirectoryInfo localFolder = null;
        public string remoteFolder = null;
        public ArrayList remoteFolderList = null;

        // Stats
        public long bytesIn = 0;
        public long bytesInCompressed = 0;
        public long bytesOut = 0;
        public long bytesOutCompressed = 0;

        public FileViewer(MeshCentralServer server, NodeClass node)
        {
            InitializeComponent();
            if (node != null) { this.Text += " - " + node.name; }
            this.node = node;
            this.server = server;
            UpdateStatus();
        }

        public bool updateLocalFileView()
        {
            leftListView.Items.Clear();
            if (localFolder == null)
            {
                try {
                    DriveInfo[] drives = DriveInfo.GetDrives();
                    foreach (DriveInfo drive in drives)
                    {
                        ListViewItem x = new ListViewItem(drive.Name, 0);
                        x.Tag = drive;
                        leftListView.Items.Add(x);
                    }
                    localUpButton.Enabled = false;
                    localLabel.Text = "Local";
                }
                catch (Exception) { return false; }
            }
            else
            {
                try
                {
                    DirectoryInfo[] directories = localFolder.GetDirectories();
                    foreach (DirectoryInfo directory in directories)
                    {
                        ListViewItem x = new ListViewItem(directory.Name, 1);
                        x.Tag = directory;
                        leftListView.Items.Add(x);
                    }
                    FileInfo[] files = localFolder.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        if (file.Attributes.HasFlag(System.IO.FileAttributes.Hidden)) continue;
                        string[] si = new string[2];
                        si[0] = file.Name;
                        si[1] = "" + file.Length;
                        ListViewItem x = new ListViewItem(si, 2);
                        x.Tag = file;
                        leftListView.Items.Add(x);
                    }
                    localUpButton.Enabled = true;
                    localLabel.Text = "Local - " + localFolder.FullName;
                }
                catch (Exception) { return false; }
            }
            return true;
        }

        private delegate void updateRemoteFileViewHandler();
        public void updateRemoteFileView()
        {
            if (this.InvokeRequired) { this.Invoke(new updateRemoteFileViewHandler(updateRemoteFileView)); return; }
            rightListView.Items.Clear();

            if ((remoteFolder == null) || (remoteFolder == "")) {
                remoteLabel.Text = "Remote";
            } else {
                if (node.agentid < 5)
                {
                    remoteLabel.Text = "Remote - " + remoteFolder.Replace("/", "\\");
                }
                else
                {
                    remoteLabel.Text = "Remote - " + remoteFolder;
                }
            }

            remoteUpButton.Enabled = !((remoteFolder == null) || (remoteFolder == ""));

            if (remoteFolderList != null)
            {
                // Display all folders
                for (int i = 0; i < remoteFolderList.Count; i++)
                {
                    Dictionary<string, object> fileItem = (Dictionary<string, object>)remoteFolderList[i];
                    int fileIcon = 0;
                    string fileName = null;
                    string fileDate = null;
                    int fileSize = -1;
                    if (fileItem.ContainsKey("t")) { fileIcon = (int)fileItem["t"]; }
                    if (fileItem.ContainsKey("n")) { fileName = (string)fileItem["n"]; }
                    if (fileItem.ContainsKey("d")) { fileDate = (string)fileItem["d"]; }
                    if (fileItem.ContainsKey("s")) { fileSize = (int)fileItem["s"]; }
                    if (fileIcon == 1) {
                        // Drive
                        ListViewItem x = new ListViewItem(fileName, 0);
                        rightListView.Items.Add(x);
                    } else if (fileIcon == 2) {
                        // Folder
                        ListViewItem x = new ListViewItem(fileName, 1);
                        rightListView.Items.Add(x);
                    }
                }

                // Display all files
                for (int i = 0; i < remoteFolderList.Count; i++)
                {
                    Dictionary<string, object> fileItem = (Dictionary<string, object>)remoteFolderList[i];
                    int fileIcon = 0;
                    string fileName = null;
                    string fileDate = null;
                    int fileSize = -1;
                    if (fileItem.ContainsKey("t")) { fileIcon = (int)fileItem["t"]; }
                    if (fileItem.ContainsKey("n")) { fileName = (string)fileItem["n"]; }
                    if (fileItem.ContainsKey("d")) { fileDate = (string)fileItem["d"]; }
                    if (fileItem.ContainsKey("s")) { fileSize = (int)fileItem["s"]; }
                    if (fileIcon == 3)
                    {
                        // File
                        string[] si = new string[2];
                        si[0] = fileName;
                        si[1] = "" + fileSize;
                        ListViewItem x = new ListViewItem(si, 2);
                        rightListView.Items.Add(x);
                    }
                }
            }
        }

        private void Server_onStateChanged(int state)
        {
            UpdateStatus();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            updateLocalFileView();
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void MenuItemConnect_Click(object sender, EventArgs e)
        {
            if ((wc != null) || (node == null)) return;
            byte[] randomid = new byte[10];
            rand.GetBytes(randomid);
            randomIdHex = BitConverter.ToString(randomid).Replace("-", string.Empty);

            state = 1;
            string ux = server.wsurl.ToString().Replace("/control.ashx", "/");
            int i = ux.IndexOf("?");
            if (i >= 0) { ux = ux.Substring(0, i); }
            Uri u = new Uri(ux + "meshrelay.ashx?browser=1&p=5&nodeid=" + node.nodeid + "&id=" + randomIdHex + "&auth=" + server.authCookie);
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
                        string u = "*/meshrelay.ashx?p=5&nodeid=" + node.nodeid + "&id=" + randomIdHex + "&rauth=" + server.rauthCookie;
                        server.sendCommand("{ \"action\": \"msg\", \"type\": \"tunnel\", \"nodeid\": \"" + node.nodeid + "\", \"value\": \"" + u.ToString() + "\", \"usage\": 5 }");
                        displayMessage(null);
                        break;
                    }
            }
            UpdateStatus();
        }

        private void requestRemoteFolder(string path)
        {
            // Send initial LS command
            string cmd = "{\"action\":\"ls\",\"reqid\":1,\"path\":\"" + path.Replace("\\","/") + "\"}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);
        }

        private void Wc_onStringData(webSocketClient sender, string data, int orglen)
        {
            bytesIn += data.Length;
            bytesInCompressed += orglen;

            if ((state == 2) && ((data == "c") || (data == "cr")))
            {
                if (data == "cr") { sessionIsRecorded = true; }
                state = 3;
                UpdateStatus();
                displayMessage(null);

                // Send protocol
                string cmd = "5";
                byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
                wc.SendBinary(bincmd, 0, bincmd.Length);

                requestRemoteFolder("");
                //requestRemoteFolder("C:\\");

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

            if (data[offset] == 123) {
                // Parse the received JSON
                Dictionary<string, object> jsonAction = new Dictionary<string, object>();
                jsonAction = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(UTF8Encoding.UTF8.GetString(data, offset, length));
                if (jsonAction == null) return;
                int reqid = 0;
                if (jsonAction.ContainsKey("reqid")) { reqid = (int)jsonAction["reqid"]; }

                // Result of a LS command
                if (reqid == 1)
                {
                    if (jsonAction.ContainsKey("path")) { remoteFolder = (string)jsonAction["path"]; }
                    if (jsonAction.ContainsKey("dir")) { remoteFolderList = (ArrayList)jsonAction["dir"]; }
                    updateRemoteFileView();
                }
            }
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
                    connectButton.Text = "Connect";
                    break;
                case 1: // Connecting
                    mainToolStripStatusLabel.Text = "Connecting...";
                    connectButton.Text = "Disconnect";
                    break;
                case 2: // Setup
                    mainToolStripStatusLabel.Text = "Setup...";
                    connectButton.Text = "Disconnect";
                    break;
                case 3: // Connected
                    string label = "Connected";
                    if (sessionIsRecorded) { label += ", Recorded Session"; }
                    if ((userSessions != null) && (userSessions.Count > 1)) { label += string.Format(", {0} users", userSessions.Count); }
                    label += ".";
                    mainToolStripStatusLabel.Text = label;
                    connectButton.Text = "Disconnect";
                    break;
            }

            rightListView.Enabled = (state == 3);
            if (state != 3) { rightListView.Items.Clear(); }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

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
                consoleTimer.Enabled = true;
            }
        }
        private void consoleTimer_Tick(object sender, EventArgs e)
        {
            consoleMessage.Visible = false;
            consoleTimer.Enabled = false;
        }

        private void connectButton_Click(object sender, EventArgs e)
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

        private void leftListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = leftListView.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                if (item.Tag.GetType() == typeof(DriveInfo)) {
                    DirectoryInfo old = localFolder;
                    localFolder = ((DriveInfo)item.Tag).RootDirectory;
                    if (updateLocalFileView() == false) { localFolder = old; updateLocalFileView(); }
                }
                else if (item.Tag.GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo old = localFolder;
                    localFolder = (DirectoryInfo)item.Tag;
                    if (updateLocalFileView() == false) { localFolder = old; updateLocalFileView(); }
                }
            }
        }

        private void localUpButton_Click(object sender, EventArgs e)
        {
            localFolder = localFolder.Parent;
            updateLocalFileView();
        }
    }
}
