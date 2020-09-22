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
using System.Net;
using System.Text;
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
        private static string rndString = getRandomString(12);

        // Stats
        public long bytesIn = 0;
        public long bytesInCompressed = 0;
        public long bytesOut = 0;
        public long bytesOutCompressed = 0;

        // Upload state
        public FileTransferStatusForm transferStatusForm = null;
        public bool uploadActive = false;
        public bool uploadStop = false;
        public int uploadFileArrayPtr = -1;
        public ArrayList uploadFileArray;
        public DirectoryInfo uploadLocalPath;
        public string uploadRemotePath;
        public FileStream uploadFileStream = null;
        public long uploadFilePtr = 0;
        public long uploadFileSize = 0;

        // Download state
        public bool downloadActive = false;
        public bool downloadStop = false;
        public int downloadFileArrayPtr = -1;
        public ArrayList downloadFileArray;
        public ArrayList downloadFileSizeArray;
        public DirectoryInfo downloadLocalPath;
        public string downloadRemotePath;
        public FileStream downloadFileStream = null;
        public long downloadFilePtr = 0;
        public long downloadFileSize = 0;

        public FileViewer(MeshCentralServer server, NodeClass node)
        {
            InitializeComponent();
            if (node != null) { this.Text += " - " + node.name; }
            this.node = node;
            this.server = server;
            UpdateStatus();

            rightListView.Columns[0].Width = rightListView.Width - rightListView.Columns[1].Width - 22;
        }

        public bool updateLocalFileView()
        {
            leftListView.Items.Clear();
            if (localFolder == null)
            {
                localRootButton.Enabled = false;
                localNewFolderButton.Enabled = false;
                localDeleteButton.Enabled = false;
                try
                {
                    DriveInfo[] drives = DriveInfo.GetDrives();
                    foreach (DriveInfo drive in drives)
                    {
                        ListViewItem x = new ListViewItem(drive.Name, 0);
                        x.Tag = drive;
                        leftListView.Items.Add(x);
                    }
                    localUpButton.Enabled = false;
                    localLabel.Text = "Local";
                    mainToolTip.SetToolTip(localLabel, "Local");
                }
                catch (Exception) { return false; }
            }
            else
            {
                localRootButton.Enabled = true;
                localNewFolderButton.Enabled = true;
                localDeleteButton.Enabled = false;
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
                    mainToolTip.SetToolTip(localLabel, "Local - " + localFolder.FullName);
                }
                catch (Exception) { return false; }
            }
            updateTransferButtons();
            return true;
        }

        public class ListViewItemSortClass : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(((ListViewItem)x).SubItems[0].Text, ((ListViewItem)y).SubItems[0].Text));
            }
        }

        private delegate void updateRemoteFileViewHandler();
        public void updateRemoteFileView()
        {
            if (this.InvokeRequired) { this.Invoke(new updateRemoteFileViewHandler(updateRemoteFileView)); return; }
            rightListView.Items.Clear();

            if ((remoteFolder == null) || (remoteFolder == "")) {
                remoteLabel.Text = "Remote";
                mainToolTip.SetToolTip(remoteLabel, "Remote");
            } else {
                if (node.agentid < 5)
                {
                    remoteLabel.Text = "Remote - " + remoteFolder.Replace("/", "\\");
                    mainToolTip.SetToolTip(remoteLabel, "Remote - " + remoteFolder.Replace("/", "\\"));
                }
                else
                {
                    remoteLabel.Text = "Remote - " + remoteFolder;
                    mainToolTip.SetToolTip(remoteLabel, "Remote - " + remoteFolder);
                }
            }

            remoteRefreshButton.Enabled = true;
            remoteRootButton.Enabled = !((remoteFolder == null) || (remoteFolder == ""));
            remoteUpButton.Enabled = !((remoteFolder == null) || (remoteFolder == ""));
            if (node.agentid < 5) {
                remoteNewFolderButton.Enabled = !((remoteFolder == null) || (remoteFolder == ""));
                remoteDeleteButton.Enabled = (!((remoteFolder == null) || (remoteFolder == ""))) && (rightListView.SelectedItems.Count > 0);
            } else {
                remoteNewFolderButton.Enabled = true;
                remoteDeleteButton.Enabled = (rightListView.SelectedItems.Count > 0);
            }

            if (remoteFolderList != null)
            {
                ArrayList sortlist = new ArrayList();

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
                        sortlist.Add(new ListViewItem(fileName, 0)); // Drive
                    } else if (fileIcon == 2) {
                        sortlist.Add(new ListViewItem(fileName, 1)); // Folder
                    }
                }
                sortlist.Sort(new ListViewItemSortClass());
                foreach (ListViewItem l in sortlist) { rightListView.Items.Add(l); }
                sortlist.Clear();

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
                        sortlist.Add(new ListViewItem(si, 2)); // File
                    }
                }
                sortlist.Sort(new ListViewItemSortClass());
                foreach (ListViewItem l in sortlist) { rightListView.Items.Add(l); }
            }
            updateTransferButtons();
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
            updateTransferButtons();
        }

        private void requestRemoteFolder(string path)
        {
            // Send LS command
            string cmd = "{\"action\":\"ls\",\"reqid\":1,\"path\":\"" + path.Replace("\\","/") + "\"}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);
        }

        private void requestCreateFolder(string path)
        {
            // Send MKDIR command
            string cmd = "{\"action\":\"mkdir\",\"reqid\":2,\"path\":\"" + path.Replace("\\", "/") + "\"}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);
        }

        private void requestRename(string path, string oldname, string newname)
        {
            // Send RENAME command
            string cmd = "{\"action\":\"rename\",\"reqid\":3,\"path\":\"" + path.Replace("\\", "/") + "\",\"oldname\":\"" + oldname + "\",\"newname\":\"" + newname + "\"}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);
        }

        private void requestDelete(string path, string[] files, bool recursive)
        {
            // Send RM command
            string delfiles = "";
            foreach (string file in files) { if (delfiles.Length != 0) { delfiles += ","; } delfiles += "\"" + file + "\""; }
            string cmd = "{\"action\":\"rm\",\"reqid\":4,\"path\":\"" + path.Replace("\\", "/") + "\",\"rec\":" + recursive.ToString().ToLower() + ",\"delfiles\":[" + delfiles + "]}";
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

                // Ask for root level
                requestRemoteFolder("");
                return;
            }
            if (state != 3) return;

            // Parse the received JSON
            Dictionary<string, object> jsonAction = new Dictionary<string, object>();
            jsonAction = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(data);
            if (jsonAction == null) return;

            if (jsonAction.ContainsKey("action") && (jsonAction["action"].GetType() == typeof(string)))
            {
                string action = jsonAction["action"].ToString();
                switch (action)
                {
                    case "download":
                        {
                            if (downloadStop) { downloadCancel(); return; }

                            string sub = null;
                            if (jsonAction.ContainsKey("sub")) { sub = (string)jsonAction["sub"]; }
                            if (sub == "start")
                            {
                                // Send DOWNLOAD startack command
                                string cmd = "{\"action\":\"download\",\"sub\":\"startack\",\"id\":" + (downloadFileArrayPtr + 1000) + "}";
                                byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
                                wc.SendBinary(bincmd, 0, bincmd.Length);
                            }
                            else if (sub == "cancel")
                            {
                                // Cancel the download
                                downloadCancel();
                            }
                            break;
                        }
                }
            }
            else if (jsonAction.ContainsKey("type") && (jsonAction["type"].GetType() == typeof(string))) {
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
                string action = null;
                if (jsonAction.ContainsKey("action")) { action = (string)jsonAction["action"]; }

                if (action == "uploadstart")
                {
                    if (uploadStop) { uploadCancel(); return; }
                    uploadNextPart(false);
                    for (var i = 0; i < 8; i++) { uploadNextPart(true); }
                }
                else if (action == "uploadack")
                {
                    if (uploadStop) { uploadCancel(); return; }
                    uploadNextPart(false);
                }
                else if (action == "uploaddone")
                {
                    if (uploadFileArray.Count > uploadFileArrayPtr + 1)
                    {
                        // Upload the next file
                        uploadFilePtr = 0;
                        uploadFileSize = 0;
                        if (uploadFileStream != null) { uploadFileStream.Close(); uploadFileStream = null; }
                        uploadFileArrayPtr++;
                        uploadNextFile();
                    }
                    else
                    {
                        // Done with all files
                        uploadActive = false;
                        uploadStop = false;
                        uploadFileArrayPtr = -1;
                        uploadFileArray = null;
                        uploadLocalPath = null;
                        uploadRemotePath = null;
                        uploadFilePtr = 0;
                        uploadFileSize = 0;
                        closeTransferDialog();
                        remoteRefresh();
                    }
                }
                else if (action == "uploaderror")
                {
                    uploadCancel();
                }
                else if (reqid == 1)
                {
                    // Result of a LS command
                    if (jsonAction.ContainsKey("path")) { remoteFolder = (string)jsonAction["path"]; }
                    if (jsonAction.ContainsKey("dir")) { remoteFolderList = (ArrayList)jsonAction["dir"]; }
                    updateRemoteFileView();
                }
            } else
            {
                if (downloadActive) {
                    if (downloadStop) { downloadCancel(); return; }
                    downloadGotBinaryData(data, offset, length);
                }
            }
        }

        private delegate void remoteRefreshHandler();

        private void remoteRefresh()
        {
            if (this.InvokeRequired) { this.Invoke(new remoteRefreshHandler(remoteRefresh)); return; }
            updateTimer.Enabled = true;
        }

        private delegate void localRefreshHandler();

        private void localRefresh()
        {
            if (this.InvokeRequired) { this.Invoke(new localRefreshHandler(localRefresh)); return; }
            updateLocalFileView();
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
                    remoteRefreshButton.Enabled = false;
                    remoteUpButton.Enabled = false;
                    remoteRootButton.Enabled = false;
                    remoteNewFolderButton.Enabled = false;
                    remoteDeleteButton.Enabled = false;
                    remoteFolder = null;
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
            if (wc != null)
            {
                // Disconnect
                state = 0;
                wc.Dispose();
                wc = null;
                UpdateStatus();
            }
            node.fileViewer = null;
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

        private void rightListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = rightListView.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                string r = remoteFolder;
                if ((item.ImageIndex == 0) || (item.ImageIndex == 1)) {
                    if ((r == null) || (r == "")) {
                        r = item.Text;
                    } else {
                        if (remoteFolder.EndsWith("/")) { r = remoteFolder + item.Text; } else { r = remoteFolder + "/" + item.Text; }
                    }
                    requestRemoteFolder(r);
                }
            }
        }

        private void remoteUpButton_Click(object sender, EventArgs e)
        {
            string r = remoteFolder;
            if (r.EndsWith("/")) { r = r.Substring(0, r.Length - 1); }
            int i = r.LastIndexOf("/");
            if (i >= 0)
            {
                r = r.Substring(0, i + 1);
            } else
            {
                r = "";
            }
            requestRemoteFolder(r);
        }

        private void leftRefreshButton_Click(object sender, EventArgs e)
        {
            updateLocalFileView();
        }

        private void rightRefreshButton_Click(object sender, EventArgs e)
        {
            requestRemoteFolder(remoteFolder);
        }

        private void remoteNewFolderButton_Click(object sender, EventArgs e)
        {
            if (remoteFolder == null) return;
            FilenamePromptForm f = new FilenamePromptForm("Create Folder", "");
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                string r;
                if (remoteFolder.EndsWith("/")) { r = remoteFolder + f.filename; } else { r = remoteFolder + "/" + f.filename; }
                requestCreateFolder(r);
                remoteRefresh();
            }
        }

        private void localRootButton_Click(object sender, EventArgs e)
        {
            localFolder = null;
            updateLocalFileView();
        }

        private void remoteRootButton_Click(object sender, EventArgs e)
        {
            requestRemoteFolder("");
        }

        private void rightListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (node.agentid < 5)
            {
                remoteDeleteButton.Enabled = (!((remoteFolder == null) || (remoteFolder == ""))) && (rightListView.SelectedItems.Count > 0);
            }
            else
            {
                remoteDeleteButton.Enabled = (rightListView.SelectedItems.Count > 0);
            }
            updateTransferButtons();
        }

        private void rightListView_Resize(object sender, EventArgs e)
        {
            if (rightListView.Columns[0].Width != (rightListView.Width - rightListView.Columns[1].Width - 22))
            {
                rightListView.Columns[0].Width = rightListView.Width - rightListView.Columns[1].Width - 22;
            }
        }

        private void rightListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (rightListView.Columns[0].Width != (rightListView.Width - rightListView.Columns[1].Width - 22))
            {
                rightListView.Columns[0].Width = rightListView.Width - rightListView.Columns[1].Width - 22;
            }
        }

        private void leftListView_Resize(object sender, EventArgs e)
        {
            if (leftListView.Columns[0].Width != (leftListView.Width - leftListView.Columns[1].Width - 22))
            {
                leftListView.Columns[0].Width = leftListView.Width - leftListView.Columns[1].Width - 22;
            }
        }

        private void leftListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (leftListView.Columns[0].Width != (leftListView.Width - leftListView.Columns[1].Width - 22))
            {
                leftListView.Columns[0].Width = leftListView.Width - leftListView.Columns[1].Width - 22;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            updateTimer.Enabled = false;
            if (remoteFolder != null) { requestRemoteFolder(remoteFolder); }
        }

        private void remoteDeleteButton_Click(object sender, EventArgs e)
        {
            bool rec = false;
            ArrayList filesArray = new ArrayList();
            foreach (ListViewItem l in rightListView.SelectedItems) { filesArray.Add(l.Text); if (l.ImageIndex == 1) { rec = true; } }
            string[] files = (string[])filesArray.ToArray(typeof(string));
            string msg = string.Format("Remove {0} items?", files.Length);
            if (files.Length == 1) { msg = "Remove 1 item?"; }
            FileDeletePromptForm f = new FileDeletePromptForm(msg, rec);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                requestDelete(remoteFolder, files, f.recursive);
                remoteRefresh();
            }
        }

        private void remoteContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((rightListView.SelectedItems.Count == 0) || ((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))))
            {
                deleteToolStripMenuItem.Visible = toolStripMenuItem1.Visible = renameToolStripMenuItem.Visible = false;
            }
            else if (rightListView.SelectedItems.Count == 1)
            {
                deleteToolStripMenuItem.Visible = toolStripMenuItem1.Visible = renameToolStripMenuItem.Visible = true;
            }
            else if (rightListView.SelectedItems.Count > 1)
            {
                renameToolStripMenuItem.Visible = false;
                deleteToolStripMenuItem.Visible = toolStripMenuItem1.Visible = true;
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string oldname = rightListView.SelectedItems[0].Text;
            if ((rightListView.SelectedItems.Count != 1) || (remoteFolder == null)) return;
            FilenamePromptForm f = new FilenamePromptForm("Rename", oldname);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                if (oldname == f.filename) return;
                //string r;
                //if (remoteFolder.EndsWith("/")) { r = remoteFolder + f.filename; } else { r = remoteFolder + "/" + f.filename; }
                requestRename(remoteFolder, oldname, f.filename);
                remoteRefresh();
            }
        }

        private delegate void updateTransferButtonsHandler();

        private void updateTransferButtons()
        {
            if (this.InvokeRequired) { this.Invoke(new updateTransferButtonsHandler(updateTransferButtons)); return; }
            if ((wc == null) || (wc.State != webSocketClient.ConnectionStates.Connected))
            {
                uploadButton.Enabled = false;
                downloadButton.Enabled = false;
            }
            else
            {
                // Set upload button
                bool uploadAllowed = true;
                if (localFolder == null) { uploadAllowed = false; }
                if (leftListView.SelectedItems.Count == 0) { uploadAllowed = false; }
                foreach (ListViewItem l in leftListView.SelectedItems) { if (l.ImageIndex != 2) { uploadAllowed = false; } }
                if ((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { uploadAllowed = false; }
                uploadButton.Enabled = uploadAllowed;

                // Set download button
                bool downloadAllowed = true;
                if (localFolder == null) { downloadAllowed = false; }
                if (rightListView.SelectedItems.Count == 0) { downloadAllowed = false; }
                foreach (ListViewItem l in rightListView.SelectedItems) { if (l.ImageIndex != 2) { downloadAllowed = false; } }
                if ((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { downloadAllowed = false; }
                downloadButton.Enabled = downloadAllowed;
            }
        }

        private void leftListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTransferButtons();
            localDeleteButton.Enabled = ((localFolder != null) && (leftListView.SelectedItems.Count > 0));
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            if (uploadActive || downloadActive) return;
            uploadFileArrayPtr = 0;
            uploadFileArray = new ArrayList();
            foreach (ListViewItem l in leftListView.SelectedItems) { if (l.ImageIndex == 2) { uploadFileArray.Add(l.Text); } }
            uploadLocalPath = localFolder;
            uploadRemotePath = remoteFolder;
            uploadActive = true;
            uploadStop = false;
            uploadNextFile();

            // Show transfer status dialog
            transferStatusForm = new FileTransferStatusForm(this);
            transferStatusForm.Show(this);
    }

        private void uploadNextFile()
        {
            string localFilePath, localFileName;
            if (uploadLocalPath != null)
            {
                localFilePath = Path.Combine(uploadLocalPath.FullName, (string)uploadFileArray[uploadFileArrayPtr]);
                localFileName = (string)uploadFileArray[uploadFileArrayPtr];
            }
            else
            {
                localFilePath = (string)uploadFileArray[uploadFileArrayPtr];
                localFileName = Path.GetFileName(localFilePath);
            }
            uploadFileStream = File.OpenRead(localFilePath);
            uploadFileSize = new FileInfo(localFilePath).Length;
            uploadFilePtr = 0;

            // Send UPLOAD command
            string cmd = "{\"action\":\"upload\",\"reqid\":" + (uploadFileArrayPtr + 1000) + ",\"path\":\"" + uploadRemotePath + "\",\"name\":\"" + localFileName + "\",\"size\":" + uploadFileSize + "}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);
        }

        public void uploadCancel()
        {
            if (uploadActive == false) return;

            // Send UPLOADCANCEL command
            string cmd = "{\"action\":\"uploadcancel\",\"reqid\":" + (uploadFileArrayPtr + 1000) + "}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);

            // Clear state
            uploadActive = false;
            uploadStop = false;
            uploadFileArrayPtr = -1;
            uploadFileArray = null;
            uploadLocalPath = null;
            uploadRemotePath = null;
            uploadFilePtr = 0;
            uploadFileSize = 0;
            if (uploadFileStream != null) { uploadFileStream.Close(); uploadFileStream = null; }
            closeTransferDialog();
            remoteRefresh();
        }

        private void uploadNextPart(bool dataPriming)
        {
            if (uploadActive == false) return;
            byte[] buffer = new byte[16385];
            int len = uploadFileStream.Read(buffer, 1, buffer.Length - 1);
            if (dataPriming && (len == 0)) return;
            uploadFilePtr += len;

            if (len == 0) {
                // Send UPLOADDONE command
                string cmd = "{\"action\":\"uploaddone\",\"reqid\":" + (uploadFileArrayPtr + 1000) + "}";
                byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
                wc.SendBinary(bincmd, 0, bincmd.Length);
            } else {
                // Send part of the file
                // If starts with 0 or {, add a zero char at the start of the send, this will indicate that it's not a JSON command.
                if ((buffer[1] == 123) || (buffer[1] == 0)) { wc.SendBinary(buffer, 0, len + 1); } else { wc.SendBinary(buffer, 1, len); }
            }
        }

        private delegate void closeTransferDialogHandler();

        private void closeTransferDialog()
        {
            if (transferStatusForm == null) return;
            if (this.InvokeRequired) { this.Invoke(new closeTransferDialogHandler(closeTransferDialog)); return; }
            transferStatusForm.Close(); transferStatusForm = null;
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (uploadActive || downloadActive) return;
            downloadFileArrayPtr = 0;
            downloadFileArray = new ArrayList();
            downloadFileSizeArray = new ArrayList();
            foreach (ListViewItem l in rightListView.SelectedItems) {
                if (l.ImageIndex == 2) {
                    downloadFileArray.Add(l.Text);
                    downloadFileSizeArray.Add(int.Parse(l.SubItems[1].Text));
                }
            }
            downloadLocalPath = localFolder;
            downloadRemotePath = remoteFolder;
            downloadActive = true;
            downloadStop = false;
            downloadNextFile();

            // Show transfer status dialog
            transferStatusForm = new FileTransferStatusForm(this);
            transferStatusForm.Show(this);
        }

        private void downloadNextFile()
        {
            string localFilePath;
            localFilePath = Path.Combine(downloadLocalPath.FullName, (string)downloadFileArray[downloadFileArrayPtr]);
            try { downloadFileStream = File.OpenWrite(localFilePath); } catch (Exception) { return; }
            downloadFileSize = (int)downloadFileSizeArray[downloadFileArrayPtr];
            downloadFilePtr = 0;

            string r;
            if (downloadRemotePath.EndsWith("/")) { r = downloadRemotePath + downloadFileArray[downloadFileArrayPtr]; } else { r = downloadRemotePath + "/" + downloadFileArray[downloadFileArrayPtr]; }

            // Send DOWNLOAD command
            string cmd = "{\"action\":\"download\",\"sub\":\"start\",\"id\":" + (downloadFileArrayPtr + 1000) + ",\"path\":\"" + r + "\"}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);
        }

        private void downloadGotBinaryData(byte[] data, int offset, int length)
        {
            if ((length < 4) || (downloadFileStream == null)) return;
            if (length > 4)
            {
                // Save part to disk
                downloadFileStream.Write(data, offset + 4, length - 4);
                downloadFilePtr += (length - 4);
            }
            int controlBits = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, offset));
            if ((controlBits & 1) != 0)
            {
                if (downloadFileStream != null) { downloadFileStream.Close(); downloadFileStream = null; }
                downloadFilePtr = 0;
                downloadFileSize = 0;

                if (downloadFileArray.Count > downloadFileArrayPtr + 1)
                {
                    // Download the next file
                    downloadFileArrayPtr++;
                    downloadNextFile();
                }
                else
                {
                    // Done with all files
                    downloadActive = false;
                    downloadStop = false;
                    downloadFileArrayPtr = -1;
                    downloadFileArray = null;
                    downloadLocalPath = null;
                    downloadRemotePath = null;
                    closeTransferDialog();
                    localRefresh();
                }
            }
            else
            {
                // Send DOWNLOAD command
                string cmd = "{\"action\":\"download\",\"sub\":\"ack\",\"id\":" + (downloadFileArrayPtr + 1000) + "}";
                byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
                wc.SendBinary(bincmd, 0, bincmd.Length);
            }
        }


        public void downloadCancel()
        {
            if (downloadActive == false) return;

            // Send DOWNLOAD command
            string cmd = "{\"action\":\"download\",\"sub\":\"stop\",\"id\":" + (downloadFileArrayPtr + 1000) + "}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);

            // Done with all files
            if (downloadFileStream != null) { downloadFileStream.Close(); downloadFileStream = null; }
            downloadFilePtr = 0;
            downloadFileSize = 0;
            downloadActive = false;
            downloadStop = false;
            downloadFileArrayPtr = -1;
            downloadFileArray = null;
            downloadLocalPath = null;
            downloadRemotePath = null;
            closeTransferDialog();
            localRefresh();
        }

        private void localNewFolderButton_Click(object sender, EventArgs e)
        {
            if (localFolder == null) return;
            FilenamePromptForm f = new FilenamePromptForm("Create Folder", "");
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                Directory.CreateDirectory(Path.Combine(localFolder.FullName, f.filename));
                updateLocalFileView();
            }
        }

        private void localDeleteButton_Click(object sender, EventArgs e)
        {
            bool rec = false;
            ArrayList filesArray = new ArrayList();
            foreach (ListViewItem l in leftListView.SelectedItems) { filesArray.Add(l.Text); if (l.ImageIndex == 1) { rec = true; } }
            string[] files = (string[])filesArray.ToArray(typeof(string));
            string msg = string.Format("Remove {0} items?", files.Length);
            if (files.Length == 1) { msg = "Remove 1 item?"; }
            FileDeletePromptForm f = new FileDeletePromptForm(msg, rec);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in filesArray)
                {
                    try {
                        string fullpath = Path.Combine(localFolder.FullName, file);
                        FileAttributes attr = File.GetAttributes(fullpath);
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory) { Directory.Delete(fullpath, f.recursive); } else { File.Delete(fullpath); }
                    } catch (Exception) { }
                }
                updateLocalFileView();
            }
        }

        private void rightListView_DragEnter(object sender, DragEventArgs e)
        {
            if (uploadActive || downloadActive) return;
            if ((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { return; }
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void rightListView_DragDrop(object sender, DragEventArgs e)
        {
            if (uploadActive || downloadActive) return;
            if ((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { return; }
            uploadFileArrayPtr = 0;
            uploadFileArray = new ArrayList();
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) { uploadFileArray.Add(file); }
            uploadLocalPath = null;
            uploadRemotePath = remoteFolder;
            uploadActive = true;
            uploadStop = false;
            uploadNextFile();

            // Show transfer status dialog
            transferStatusForm = new FileTransferStatusForm(this);
            transferStatusForm.Show(this);
        }

        private void leftListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ArrayList files = new ArrayList();
                foreach (ListViewItem l in leftListView.SelectedItems) { if (l.ImageIndex == 2) { files.Add(Path.Combine(localFolder.FullName, l.Text)); } }
                if (files.Count > 0)
                {
                    leftListView.DoDragDrop(new DataObject(DataFormats.FileDrop, (string[])files.ToArray(typeof(string))), DragDropEffects.Copy);
                }
            }
        }

        private void rightListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ArrayList XdownloadFileArray = new ArrayList();
                ArrayList XdownloadFileSizeArray = new ArrayList();
                foreach (ListViewItem l in rightListView.SelectedItems)
                {
                    if (l.ImageIndex == 2)
                    {
                        XdownloadFileArray.Add(l.Text);
                        XdownloadFileSizeArray.Add(int.Parse(l.SubItems[1].Text));
                    }
                }
                if (XdownloadFileArray.Count > 0)
                {
                    DataObject dataObj = new DataObject();
                    dataObj.SetData("Type", "MeshCentralRouterRemoteFiles-" + rndString);
                    dataObj.SetData("RemoteFiles", XdownloadFileArray);
                    dataObj.SetData("RemoteSizes", XdownloadFileSizeArray);
                    dataObj.SetData("RemoteFolder", remoteFolder);
                    rightListView.DoDragDrop(dataObj, DragDropEffects.Copy);
                }
            }
        }

        private void leftListView_DragEnter(object sender, DragEventArgs e)
        {
            if (uploadActive || downloadActive || (localFolder == null)) return;
            if ((e.Data.GetDataPresent("Type") == true) && ((string)e.Data.GetData("Type") == ("MeshCentralRouterRemoteFiles-" + rndString))) { e.Effect = DragDropEffects.Copy; }
        }

        private void leftListView_DragDrop(object sender, DragEventArgs e)
        {
            if (uploadActive || downloadActive) return;
            if ((e.Data.GetDataPresent("Type") == false) || ((string)e.Data.GetData("Type") != ("MeshCentralRouterRemoteFiles-" + rndString))) return;
            downloadFileArrayPtr = 0;
            downloadFileArray = (ArrayList)e.Data.GetData("RemoteFiles");
            downloadFileSizeArray = (ArrayList)e.Data.GetData("RemoteSizes");
            downloadLocalPath = localFolder;
            downloadRemotePath = (string)e.Data.GetData("RemoteFolder");
            downloadActive = true;
            downloadStop = false;
            downloadNextFile();

            // Show transfer status dialog
            transferStatusForm = new FileTransferStatusForm(this);
            transferStatusForm.Show(this);
        }
        private static string getRandomString(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[(((length * 6) + 7) / 8)];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
