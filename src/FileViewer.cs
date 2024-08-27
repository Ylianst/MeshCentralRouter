/*
Copyright 2009-2021 Intel Corporation

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
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Linq;

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
    private bool skipExistingFiles = false;
    private FileDialogMsgForm msgForm = null;
    private bool localSortAscending = true;
    private bool remoteSortAscending = true;

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
    public Hashtable uploadFileDuplicateArray;  // Name --> Size
    public DirectoryInfo uploadLocalPath;
    public string uploadRemotePath;
    public FileStream uploadFileStream = null;
    public long uploadFilePtr = 0;
    public long uploadFileStartPtr = 0;
    public long uploadFileSize = 0;
    public DateTime uploadFileStartTime = DateTime.MinValue;
    public string uploadFileName = null;

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
    public DateTime downloadFileStartTime = DateTime.MinValue;

    public FileViewer(MeshCentralServer server, NodeClass node)
    {
      InitializeComponent();
      Translate.TranslateControl(this);
      if(node != null) { this.Text += " - " + node.name; }
      this.node = node;
      this.server = server;
      UpdateStatus();

      rightListView.Columns[0].Width = rightListView.Width - rightListView.Columns[1].Width - 150;

      // Load the local path from the registry
      string lp = Settings.GetRegValue("LocalPath", "");
      if((lp != "") && (Directory.Exists(lp))) { localFolder = new DirectoryInfo(lp); }

      // Add ColumnClick event handlers
      leftListView.ColumnClick += new ColumnClickEventHandler(LeftListView_ColumnClick);
      rightListView.ColumnClick += new ColumnClickEventHandler(RightListView_ColumnClick);

      // Update the path display for the first time
      UpdateLocalPathDisplay();
    }

    private void UpdateLocalPathDisplay()
    {
        localDirectoryPath.Items.Clear();
        ToolStripLabel fixedLabel = new ToolStripLabel("Local -");
        localDirectoryPath.Items.Add(fixedLabel);
        if (localFolder == null)
        {
            return;
        }

        string[] parts = localFolder.FullName.Split(Path.DirectorySeparatorChar);
        for (int i = 0; i < parts.Length; i++)
        {
            ToolStripButton dirButton = new ToolStripButton(parts[i]);
            int index = i; // Local copy for the lambda
            dirButton.Click += (sender, e) => LocalPathButtonClicked(parts.Take(index + 1).ToArray());
            localDirectoryPath.Items.Add(dirButton);

            if (i < parts.Length - 1)
            {
                ToolStripLabel separatorLabel = new ToolStripLabel(" / ");
                localDirectoryPath.Items.Add(separatorLabel);
            }
        }
    }

    private void UpdateRemotePathDisplay()
    {
        if (bDontupdateRemoteFileView) return;
        remoteDirectoryPath.Items.Clear();
        ToolStripLabel fixedLabel = new ToolStripLabel("Remote -");
        remoteDirectoryPath.Items.Add(fixedLabel);
        if (remoteFolder == null)
        {
            return;
        }

        string[] parts = remoteFolder.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length; i++)
        {
            ToolStripButton dirButton = new ToolStripButton(parts[i]);
            int index = i; // Local copy for the lambda
            dirButton.Click += (sender, e) => RemotePathButtonClicked(parts.Take(index + 1).ToArray());
            remoteDirectoryPath.Items.Add(dirButton);

            if (i < parts.Length - 1)
            {
                ToolStripLabel separatorLabel = new ToolStripLabel(" / ");
                remoteDirectoryPath.Items.Add(separatorLabel);
            }
        }
    }


    private void LocalPathButtonClicked(string[] parts)
    {
        string path = string.Join(Path.DirectorySeparatorChar.ToString(), parts);
        DirectoryInfo old = localFolder;
        localFolder = new DirectoryInfo(path);
        if (updateLocalFileView() == false)
        {
            localFolder = old;
            updateLocalFileView();
        }
        Settings.SetRegValue("LocalPath", (localFolder == null) ? "" : localFolder.FullName);
        UpdateLocalPathDisplay();
    }

    private void RemotePathButtonClicked(string[] parts)
    {
        string path = string.Join(Path.DirectorySeparatorChar.ToString(), parts);
        string old = remoteFolder;
        remoteFolder = path;
        requestRemoteFolder(path); // This will also call UpdateRemotePathDisplay
    }


    private void LeftListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
        if (localFolder == null) return;

        if (e.Column == 1) // Size column
        {
            localSortAscending = !localSortAscending;
            leftListView.ListViewItemSorter = new ListViewItemComparer(e.Column, localSortAscending, isNumeric: true);
        }
        else if (e.Column == 2) // Date column
        {
            localSortAscending = !localSortAscending;
            leftListView.ListViewItemSorter = new ListViewItemComparer(e.Column, localSortAscending, isDate: true);
        }
        else // Name column or other columns
        {
            localSortAscending = !localSortAscending;
            leftListView.ListViewItemSorter = new ListViewItemComparer(e.Column, localSortAscending);
        }
    }

    private void RightListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
        if (remoteFolderList == null) return;

        if (e.Column == 1) // Size column
        {
            remoteSortAscending = !remoteSortAscending;
            rightListView.ListViewItemSorter = new ListViewItemComparer(e.Column, remoteSortAscending, isNumeric: true);
        }
        else if (e.Column == 2) // Date column
        {
            remoteSortAscending = !remoteSortAscending;
            rightListView.ListViewItemSorter = new ListViewItemComparer(e.Column, remoteSortAscending, isDate: true);
        }
        else // Name column or other columns
        {
            remoteSortAscending = !remoteSortAscending;
            rightListView.ListViewItemSorter = new ListViewItemComparer(e.Column, remoteSortAscending);
        }
    }

    public class ListViewItemComparer : IComparer
    {
        private int col;
        private bool ascending;
        private bool isNumeric;
        private bool isDate;

        public ListViewItemComparer(int column, bool ascending, bool isNumeric = false, bool isDate = false)
        {
            this.col = column;
            this.ascending = ascending;
            this.isNumeric = isNumeric;
            this.isDate = isDate;
        }

        public int Compare(object x, object y)
        {
            int returnVal = 0;
            ListViewItem item1 = (ListViewItem)x;
            ListViewItem item2 = (ListViewItem)y;

            if (isNumeric)
            {
                long size1 = long.Parse(item1.SubItems[col].Text == "" ? "0" : item1.SubItems[col].Text);
                long size2 = long.Parse(item2.SubItems[col].Text == "" ? "0" : item2.SubItems[col].Text);
                returnVal = size1.CompareTo(size2);
            }
            else if (isDate)
            {
                DateTime date1 = DateTime.Parse(item1.SubItems[col].Text);
                DateTime date2 = DateTime.Parse(item2.SubItems[col].Text);
                returnVal = date1.CompareTo(date2);
            }
            else
            {
                returnVal = String.Compare(item1.SubItems[col].Text, item2.SubItems[col].Text);
            }

            if (!ascending) returnVal = -returnVal;

            return returnVal;
        }
    }


        public bool updateLocalFileView()
    {
      // Save the list of selected items
      List<String> selectedItems = new List<String>();
      foreach(ListViewItem l in leftListView.SelectedItems) { selectedItems.Add(l.Text); }

      // Refresh the list
      leftListView.Items.Clear();
      if(localFolder == null)
      {
        localRootButton.Enabled = false;
        localNewFolderButton.Enabled = false;
        localDeleteButton.Enabled = false;
        try
        {
          DriveInfo[] drives = DriveInfo.GetDrives();
          foreach(DriveInfo drive in drives)
          {
            ListViewItem x = new ListViewItem(drive.Name, 0);
            x.Tag = drive;
            leftListView.Items.Add(x);
          }
          localUpButton.Enabled = false;
          //localLabel.Text = Translate.T(Properties.Resources.Local);
          //mainToolTip.SetToolTip(localLabel, Translate.T(Properties.Resources.Local));
        }
        catch(Exception) { return false; }
      }
      else
      {
        localRootButton.Enabled = true;
        localNewFolderButton.Enabled = true;
        localDeleteButton.Enabled = false;
        try
        {
          DirectoryInfo[] directories = localFolder.GetDirectories();
          foreach(DirectoryInfo directory in directories)
          {
            string[] si = new string[3];
            si[0] = directory.Name;
            si[1] = "";  // Skipping size of directory because it is very compute consuming
            si[2] = directory.LastWriteTime.ToString(System.Globalization.CultureInfo.CurrentCulture);  // Add the date information
            ListViewItem x = new ListViewItem(si, 1);
            x.Tag = directory;
            leftListView.Items.Add(x);
          }
          FileInfo[] files = localFolder.GetFiles();
          foreach(FileInfo file in files)
          {
            if(file.Attributes.HasFlag(FileAttributes.Hidden)) continue;
            string[] si = new string[3];
            si[0] = file.Name;
            si[1] = "" + file.Length;
            si[2] = file.LastWriteTime.ToString(System.Globalization.CultureInfo.CurrentCulture);  // Add the date information
            ListViewItem x = new ListViewItem(si, 2);
            x.Tag = file;
            leftListView.Items.Add(x);
          }
          localUpButton.Enabled = true;
          //localLabel.Text = string.Format(Translate.T(Properties.Resources.LocalPlus), localFolder.FullName);
          //mainToolTip.SetToolTip(localLabel, string.Format(Translate.T(Properties.Resources.LocalPlus), localFolder.FullName));
        }
        catch(Exception) { return false; }
      }
      updateTransferButtons();

      // Reselect items
      foreach(ListViewItem l in leftListView.Items) { l.Selected = selectedItems.Contains(l.Text); }

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
    public bool bDontupdateRemoteFileView = false; // Flynn
    public void updateRemoteFileView()
    {
      if(bDontupdateRemoteFileView) return;  // Flynn
      if(this.InvokeRequired) { this.Invoke(new updateRemoteFileViewHandler(updateRemoteFileView)); return; }

      // Save the list of selected items
      List<String> selectedItems = new List<String>();
      foreach(ListViewItem l in rightListView.SelectedItems) { selectedItems.Add(l.Text); }

      rightListView.Items.Clear();

      if((remoteFolder == null) || (remoteFolder == ""))
      {
        //remoteLabel.Text = Translate.T(Properties.Resources.Remote);
        //mainToolTip.SetToolTip(remoteLabel, Translate.T(Properties.Resources.Remote));
      }
      else
      {
        if(node.agentid < 5)
        {
          //remoteLabel.Text = string.Format(Translate.T(Properties.Resources.RemotePlus), remoteFolder.Replace("/", "\\"));
          //mainToolTip.SetToolTip(remoteLabel, string.Format(Translate.T(Properties.Resources.RemotePlus), remoteFolder.Replace("/", "\\")));
        }
        else
        {
          //remoteLabel.Text = string.Format(Translate.T(Properties.Resources.RemotePlus), remoteFolder);
          //mainToolTip.SetToolTip(remoteLabel, string.Format(Translate.T(Properties.Resources.RemotePlus), remoteFolder));
        }
      }

      remoteRefreshButton.Enabled = true;
      remoteRootButton.Enabled = !((remoteFolder == null) || (remoteFolder == ""));
      remoteUpButton.Enabled = !((remoteFolder == null) || (remoteFolder == ""));
      if(node.agentid < 5)
      {
        remoteNewFolderButton.Enabled = !((remoteFolder == null) || (remoteFolder == ""));
        remoteDeleteButton.Enabled = remoteZipButton.Enabled = (!((remoteFolder == null) || (remoteFolder == ""))) && (rightListView.SelectedItems.Count > 0);
      }
      else
      {
        remoteNewFolderButton.Enabled = true;
        remoteDeleteButton.Enabled = remoteZipButton.Enabled = (rightListView.SelectedItems.Count > 0);
      }

      if(remoteFolderList != null)
      {
        ArrayList sortlist = new ArrayList();

        // Display all folders
        for(int i = 0; i < remoteFolderList.Count; i++)
        {
          Dictionary<string, object> fileItem = (Dictionary<string, object>)remoteFolderList[i];
          int fileIcon = 0;
          string fileName = null;
          string fileDate = null;
          long fileSize = -1;
          if(fileItem.ContainsKey("t")) { fileIcon = (int)fileItem["t"]; }
          if(fileItem.ContainsKey("n")) { fileName = (string)fileItem["n"]; }
          if(fileItem.ContainsKey("d")) { fileDate = (string)fileItem["d"]; }
          if(fileItem.ContainsKey("s"))
          {
            if(fileItem["s"].GetType() == typeof(System.Int32)) { fileSize = (int)fileItem["s"]; }
            if(fileItem["s"].GetType() == typeof(System.Int64)) { fileSize = (long)fileItem["s"]; }
          }
          if(fileIcon == 1)
          {
            string[] si = new string[3];
            si[0] = fileName;
            si[1] = ""; // Skipping size of directory because it is very compute consuming
            si[2] = fileDate != null ? DateTime.TryParse(fileDate, out DateTime parsedDate) ? parsedDate.ToString(System.Globalization.CultureInfo.CurrentCulture) : "" : ""; // Add the date information
            sortlist.Add(new ListViewItem(si, 0)); // Drive
          }
          else if(fileIcon == 2)
          {
            string[] si = new string[3];
            si[0] = fileName;
            si[1] = ""; // Skipping size of directory because it is very compute consuming
            si[2] = fileDate != null ? DateTime.TryParse(fileDate, out DateTime parsedDate) ? parsedDate.ToString(System.Globalization.CultureInfo.CurrentCulture) : "" : ""; // Add the date information
            sortlist.Add(new ListViewItem(si, 1)); // Folder
          }
        }
        sortlist.Sort(new ListViewItemSortClass());
        foreach(ListViewItem l in sortlist) { rightListView.Items.Add(l); }
        sortlist.Clear();

        // Display all files
        for(int i = 0; i < remoteFolderList.Count; i++)
        {
          Dictionary<string, object> fileItem = (Dictionary<string, object>)remoteFolderList[i];
          int fileIcon = 0;
          string fileName = null;
          string fileDate = null;
          long fileSize = -1;
          if(fileItem.ContainsKey("t")) { fileIcon = (int)fileItem["t"]; }
          if(fileItem.ContainsKey("n")) { fileName = (string)fileItem["n"]; }
          if(fileItem.ContainsKey("d")) { fileDate = (string)fileItem["d"]; }
          if(fileItem.ContainsKey("s"))
          {
            if(fileItem["s"].GetType() == typeof(System.Int32)) { fileSize = (int)fileItem["s"]; }
            if(fileItem["s"].GetType() == typeof(System.Int64)) { fileSize = (long)fileItem["s"]; }
          }
          if(fileIcon == 3)
          {
            // File
            string[] si = new string[3];
            si[0] = fileName;
            si[1] = "" + fileSize;
            si[2] = fileDate != null ? DateTime.TryParse(fileDate, out DateTime parsedDate) ? parsedDate.ToString(System.Globalization.CultureInfo.CurrentCulture) : "" : ""; // Add the date information
            sortlist.Add(new ListViewItem(si, 2)); // File
          }
        }
        sortlist.Sort(new ListViewItemSortClass());
        foreach(ListViewItem l in sortlist) { rightListView.Items.Add(l); }
      }
      updateTransferButtons();

      // Reselect items
      foreach(ListViewItem l in rightListView.Items) { l.Selected = selectedItems.Contains(l.Text); }
    }

    private void Server_onStateChanged(int state)
    {
      UpdateStatus();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      updateLocalFileView();

      // Restore Window Location
      string locationStr = Settings.GetRegValue("filelocation", "");
      if(locationStr != null)
      {
        string[] locationSplit = locationStr.Split(',');
        if(locationSplit.Length == 4)
        {
          try
          {
            var x = int.Parse(locationSplit[0]);
            var y = int.Parse(locationSplit[1]);
            var w = int.Parse(locationSplit[2]);
            var h = int.Parse(locationSplit[3]);
            Point p = new Point(x, y);
            if(isPointVisibleOnAScreen(p))
            {
              Location = p;
              if((w > 50) && (h > 50)) { Size = new Size(w, h); }
            }
          }
          catch(Exception) { }
        }
      }
    }

    private void MenuItemExit_Click(object sender, EventArgs e)
    {
      Close();
    }

    public void MenuItemConnect_Click(object sender, EventArgs e)
    {
      if((wc != null) || (node == null)) return;
      byte[] randomid = new byte[10];
      rand.GetBytes(randomid);
      randomIdHex = BitConverter.ToString(randomid).Replace("-", string.Empty);

      state = 1;
      string ux = server.wsurl.ToString().Replace("/control.ashx", "/");
      int i = ux.IndexOf("?");
      if(i >= 0) { ux = ux.Substring(0, i); }
      Uri u = new Uri(ux + "meshrelay.ashx?browser=1&p=5&nodeid=" + node.nodeid + "&id=" + randomIdHex + "&auth=" + server.authCookie);
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
      switch(wsstate)
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

            string u = "*" + server.wsurl.AbsolutePath.Replace("control.ashx", "meshrelay.ashx") + "?p=5&nodeid=" + node.nodeid + "&id=" + randomIdHex + "&rauth=" + server.rauthCookie;
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
      string cmd = "{\"action\":\"ls\",\"reqid\":1,\"path\":\"" + path.Replace("\\", "/") + "\"}";
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

    public void requestCancel()
    {
      string cmd = "{\"action\":\"cancel\"}";
      byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
      wc.SendBinary(bincmd, 0, bincmd.Length);
      updateMsgForm(null, null, 0);
    }

    private void requestCreateZipFileFolder(string path, string zip, string[] files)
    {
      // Send ZIP command
      string cmd = "{\"action\":\"zip\",\"reqid\":5,\"path\":\"" + path.Replace("\\", "/") + "\",\"output\":\"" + zip.Replace("\\", "/") + "\",\"files\":[";
      bool first = true;
      foreach(string file in files)
      {
        if(first) { first = false; } else { cmd += ","; }
        cmd += "\"" + file + "\"";
      }
      cmd += "]}";
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
      foreach(string file in files) { if(delfiles.Length != 0) { delfiles += ","; } delfiles += "\"" + file + "\""; }
      string cmd = "{\"action\":\"rm\",\"reqid\":4,\"path\":\"" + path.Replace("\\", "/") + "\",\"rec\":" + recursive.ToString().ToLower() + ",\"delfiles\":[" + delfiles + "]}";
      byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
      wc.SendBinary(bincmd, 0, bincmd.Length);
    }

    private void Wc_onStringData(webSocketClient sender, string data, int orglen)
    {
      bytesIn += data.Length;
      bytesInCompressed += orglen;

      if((state == 2) && ((data == "c") || (data == "cr")))
      {
        if(data == "cr") { sessionIsRecorded = true; }
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
      if(state != 3) return;

      // Parse the received JSON
      Dictionary<string, object> jsonAction = new Dictionary<string, object>();
      jsonAction = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(data);
      if(jsonAction == null) return;

      if(jsonAction.ContainsKey("action") && (jsonAction["action"].GetType() == typeof(string)))
      {
        string action = jsonAction["action"].ToString();
        switch(action)
        {
          case "download":
            {
              if(downloadStop) { downloadCancel(); return; }

              string sub = null;
              if(jsonAction.ContainsKey("sub")) { sub = (string)jsonAction["sub"]; }
              if(sub == "start")
              {
                // Send DOWNLOAD startack command
                string cmd = "{\"action\":\"download\",\"sub\":\"startack\",\"id\":" + (downloadFileArrayPtr + 1000) + "}";
                byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
                wc.SendBinary(bincmd, 0, bincmd.Length);
              }
              else if(sub == "cancel")
              {
                // Unable to download this file
                if(transferStatusForm != null) { try { transferStatusForm.addErrorMessage(String.Format(Translate.T(Properties.Resources.ErrorDownloadingFileX), downloadFileArray[downloadFileArrayPtr].ToString())); } catch { } }

                // Send DOWNLOAD command
                string cmd = "{\"action\":\"download\",\"sub\":\"stop\",\"id\":" + (downloadFileArrayPtr + 1000) + "}";
                byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
                wc.SendBinary(bincmd, 0, bincmd.Length);
                if(downloadFileStream != null) { downloadFileStream.Close(); downloadFileStream = null; } // Close the file
                try { File.Delete(Path.Combine(downloadLocalPath.FullName, Path.GetFileName((String)downloadFileArray[downloadFileArrayPtr]).ToString())); } catch(Exception) { }

                // Go to next file
                if(downloadFileArray.Count > downloadFileArrayPtr + 1)
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
              break;
            }
        }
      }
      else if(jsonAction.ContainsKey("type") && (jsonAction["type"].GetType() == typeof(string)))
      {
        string action = jsonAction["type"].ToString();
        switch(action)
        {
          case "metadata":
            {
              if((jsonAction.ContainsKey("users") == false) || (jsonAction["users"] == null)) return;
              Dictionary<string, object> usersex = (Dictionary<string, object>)jsonAction["users"];
              userSessions = new Dictionary<string, int>();
              foreach(string user in usersex.Keys) { userSessions.Add(user, (int)usersex[user]); }
              UpdateStatus();
              break;
            }
          case "console":
            {
              string msg = null;
              int msgid = -1;
              if((jsonAction.ContainsKey("msg")) && (jsonAction["msg"] != null)) { msg = jsonAction["msg"].ToString(); }
              if(jsonAction.ContainsKey("msgid")) { msgid = (int)jsonAction["msgid"]; }
              if(msgid == 1) { msg = Translate.T(Properties.Resources.WaitingForUserToGrantAccess); }
              if(msgid == 2) { msg = Translate.T(Properties.Resources.Denied); }
              if(msgid == 3) { msg = Translate.T(Properties.Resources.FailedToStartRemoteTerminalSession); }
              if(msgid == 4) { msg = Translate.T(Properties.Resources.Timeout); }
              if(msgid == 5) { msg = Translate.T(Properties.Resources.ReceivedInvalidNetworkData); }
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

      if(state != 3) return;

      if(data[offset] == 123)
      {
        // Parse the received JSON
        Dictionary<string, object> jsonAction = new Dictionary<string, object>();
        jsonAction = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(UTF8Encoding.UTF8.GetString(data, offset, length));
        if(jsonAction == null) return;
        int reqid = 0;
        if(jsonAction.ContainsKey("reqid")) { reqid = (int)jsonAction["reqid"]; }
        string action = null;
        if(jsonAction.ContainsKey("action")) { action = (string)jsonAction["action"]; }

        if(action == "uploadstart")
        {
          if(uploadStop) { uploadCancel(); return; }
          uploadNextPart(false);
          for(var i = 0; i < 8; i++) { uploadNextPart(true); }
        }
        else if(action == "uploadack")
        {
          if(uploadStop) { uploadCancel(); return; }
          uploadNextPart(false);
        }
        else if((action == "uploaddone") || (action == "uploaderror"))
        {
          // Clean up current upload
          uploadFilePtr = 0;
          uploadFileStartPtr = 0;
          uploadFileSize = 0;
          if(uploadFileStream != null) { uploadFileStream.Close(); uploadFileStream = null; }

          // If this is an error, show it in the dialog
          if((action == "uploaderror") && (transferStatusForm != null)) { transferStatusForm.addErrorMessage(String.Format(Translate.T(Properties.Resources.ErrorUploadingFileX), uploadFileName)); }

          // Check if another file needs to be uploaded
          if(uploadFileArray.Count > (uploadFileArrayPtr + 1))
          {
            // Upload the next file
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
            uploadFileDuplicateArray = null;
            uploadLocalPath = null;
            uploadRemotePath = null;
            uploadFilePtr = 0;
            uploadFileStartPtr = 0;
            uploadFileSize = 0;
            uploadFileName = null;
            closeTransferDialog();
            remoteRefresh();
          }
        }
        else if(action == "uploadhash")
        {
          if(uploadStop) { uploadCancel(); return; }
          string name = null;
          if(jsonAction.ContainsKey("name")) { name = (string)jsonAction["name"]; }
          string path = null;
          if(jsonAction.ContainsKey("path")) { path = (string)jsonAction["path"]; }
          string remoteHashHex = null;
          if(jsonAction.ContainsKey("hash")) { remoteHashHex = (string)jsonAction["hash"]; }
          long remoteFileSize = 0;
          if(jsonAction.ContainsKey("tag"))
          {
            if(jsonAction["tag"].GetType() == typeof(int)) { remoteFileSize = (int)jsonAction["tag"]; }
            if(jsonAction["tag"].GetType() == typeof(long)) { remoteFileSize = (long)jsonAction["tag"]; }
          }
          if((uploadRemotePath != path) || (uploadFileName != name)) { uploadCancel(); return; }

          // Hash the local file
          string localHashHex = null;
          try
          {
            string filePath = Path.Combine(localFolder.FullName, uploadFileName);
            using(SHA384 SHA384 = SHA384Managed.Create())
            {
              uploadFileStream.Seek(0, SeekOrigin.Begin);
              byte[] buf = new byte[65536];
              long ptr = 0;
              int len = 1;
              while(len != 0)
              {
                int l = buf.Length;
                if(l > (remoteFileSize - ptr)) { l = (int)(remoteFileSize - ptr); }
                if(l == 0) { len = 0; } else { len = uploadFileStream.Read(buf, 0, l); }
                if(len > 0) { SHA384.TransformBlock(buf, 0, len, buf, 0); }
                ptr += len;
              }
              SHA384.TransformFinalBlock(buf, 0, 0);
              localHashHex = BitConverter.ToString(SHA384.Hash).Replace("-", string.Empty);
              uploadFileStream.Seek(0, SeekOrigin.Begin);
            }
          }
          catch(Exception) { }

          if((localHashHex != null) && (localHashHex.Equals(remoteHashHex)))
          {
            if(remoteFileSize == uploadFileStream.Length)
            {
              // Files are the same length, skip the file.
              // Check if another file needs to be uploaded
              if(uploadFileArray.Count > (uploadFileArrayPtr + 1))
              {
                // Upload the next file
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
                uploadFileDuplicateArray = null;
                uploadLocalPath = null;
                uploadRemotePath = null;
                uploadFilePtr = 0;
                uploadFileStartPtr = 0;
                uploadFileSize = 0;
                uploadFileName = null;
                closeTransferDialog();
                remoteRefresh();
              }
            }
            else
            {
              // Files are not the same length, append the rest
              uploadFileStream.Seek(remoteFileSize, SeekOrigin.Begin);
              uploadFilePtr = uploadFileStartPtr = remoteFileSize;

              // Send UPLOAD command with append turned on
              string cmd = "{\"action\":\"upload\",\"reqid\":" + (uploadFileArrayPtr + 1000) + ",\"path\":\"" + uploadRemotePath + "\",\"name\":\"" + name + "\",\"size\":" + uploadFileSize + ",\"append\":true}";
              byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
              wc.SendBinary(bincmd, 0, bincmd.Length);
            }
          }
          else
          {
            // Send UPLOAD command
            string cmd = "{\"action\":\"upload\",\"reqid\":" + (uploadFileArrayPtr + 1000) + ",\"path\":\"" + uploadRemotePath + "\",\"name\":\"" + name + "\",\"size\":" + uploadFileSize + "}";
            byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
            wc.SendBinary(bincmd, 0, bincmd.Length);
          }
        }
        else if((action == "dialogmessage"))
        {
          // Dialog box message
          string msg = null;
          string file = null;
          int progress = 0;
          if(jsonAction.ContainsKey("msg") && (jsonAction["msg"] == null)) { msg = ""; }
          else if(jsonAction.ContainsKey("msg") && (jsonAction["msg"].GetType() == typeof(string))) { msg = (string)jsonAction["msg"]; }
          if(jsonAction.ContainsKey("file") && (jsonAction["file"].GetType() == typeof(string))) { file = (string)jsonAction["file"]; }
          if(jsonAction.ContainsKey("progress") && (jsonAction["progress"].GetType() == typeof(System.Int32))) { progress = (int)jsonAction["progress"]; }
          updateMsgForm(msg, file, progress);
        }
        else if(reqid == 1)
        {
          // Result of a LS command
          if(jsonAction.ContainsKey("path")) { remoteFolder = (string)jsonAction["path"]; }
          if(jsonAction.ContainsKey("dir")) { remoteFolderList = (ArrayList)jsonAction["dir"]; }
          updateRemoteFileView();
          UpdateRemotePathDisplay();
        }
      }
      else
      {
        if(downloadActive)
        {
          if(downloadStop) { downloadCancel(); return; }
          downloadGotBinaryData(data, offset, length);
        }
      }
    }

    public delegate void updateMsgFormHandler(string msg, string file, int progress);

    private void updateMsgForm(string msg, string file, int progress)
    {
      if(this.InvokeRequired) { this.Invoke(new updateMsgFormHandler(updateMsgForm), msg, file, progress); return; }
      if((msg == null) || (msg == ""))
      {
        // Close the dialog box
        if(msgForm != null) { msgForm.Close(); msgForm = null; remoteRefresh(); }
      }
      else
      {
        // Open or update the dialog box
        if(msgForm == null)
        {
          msgForm = new FileDialogMsgForm(this);
          msgForm.Show(this);
          msgForm.UpdateStatus(msg, file, progress);
          if(msgForm.StartPosition == FormStartPosition.CenterParent)
          {
            var x = Location.X + (Width - msgForm.Width) / 2;
            var y = Location.Y + (Height - msgForm.Height) / 2;
            msgForm.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
          }
        }
        else
        {
          msgForm.UpdateStatus(msg, file, progress);
        }
      }
    }

    private delegate void remoteRefreshHandler();

    private void remoteRefresh()
    {
      if(this.InvokeRequired) { this.Invoke(new remoteRefreshHandler(remoteRefresh)); return; }
      updateTimer.Enabled = true;
    }

    private delegate void localRefreshHandler();

    private void localRefresh()
    {
      if(this.InvokeRequired) { this.Invoke(new localRefreshHandler(localRefresh)); return; }
      updateLocalFileView();
    }

    private void MenuItemDisconnect_Click(object sender, EventArgs e)
    {
      if(wc != null)
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
      if(this.IsDisposed) return;
      if(this.InvokeRequired) { this.Invoke(new UpdateStatusHandler(UpdateStatus)); return; }

      //if (kvmControl == null) return;
      switch(state)
      {
        case 0: // Disconnected
          mainToolStripStatusLabel.Text = Translate.T(Properties.Resources.Disconnected);
          connectButton.Text = Translate.T(Properties.Resources.Connect);
          remoteRefreshButton.Enabled = false;
          remoteUpButton.Enabled = false;
          remoteRootButton.Enabled = false;
          remoteNewFolderButton.Enabled = false;
          remoteDeleteButton.Enabled = false;
          remoteZipButton.Enabled = false;
          remoteFolder = null;
          break;
        case 1: // Connecting
          mainToolStripStatusLabel.Text = Translate.T(Properties.Resources.Connecting);
          connectButton.Text = Translate.T(Properties.Resources.Disconnect);
          break;
        case 2: // Setup
          mainToolStripStatusLabel.Text = Translate.T(Properties.Resources.Setup);
          connectButton.Text = Translate.T(Properties.Resources.Disconnect);
          break;
        case 3: // Connected
          string label = Translate.T(Properties.Resources.Connected);
          if(sessionIsRecorded) { label += Translate.T(Properties.Resources.RecordedSession); }
          if((userSessions != null) && (userSessions.Count > 1)) { label += string.Format(Translate.T(Properties.Resources.AddXUsers), userSessions.Count); }
          label += ".";
          mainToolStripStatusLabel.Text = label;
          connectButton.Text = Translate.T(Properties.Resources.Disconnect);
          break;
      }

      rightListView.Enabled = (state == 3);
      if(state != 3) { rightListView.Items.Clear(); }
    }

    private void Main_FormClosing(object sender, FormClosingEventArgs e)
    {
      if(wc != null)
      {
        // Disconnect
        state = 0;
        wc.Dispose();
        wc = null;
        UpdateStatus();
      }
      node.fileViewer = null;

      // Clean up any downloads
      if(downloadFileStream != null) { downloadFileStream.Close(); downloadFileStream = null; }
      downloadFilePtr = 0;
      downloadFileSize = 0;
      downloadActive = false;
      downloadStop = false;
      downloadFileArrayPtr = -1;
      downloadFileArray = null;
      downloadLocalPath = null;
      downloadRemotePath = null;

      // Clean up any uploads
      uploadActive = false;
      uploadStop = false;
      uploadFileArrayPtr = -1;
      uploadFileArray = null;
      uploadFileDuplicateArray = null;
      uploadLocalPath = null;
      uploadRemotePath = null;
      uploadFilePtr = 0;
      uploadFileStartPtr = 0;
      uploadFileSize = 0;
      if(uploadFileStream != null) { uploadFileStream.Close(); uploadFileStream = null; }

      // Save window location
      Settings.SetRegValue("filelocation", Location.X + "," + Location.Y + "," + Size.Width + "," + Size.Height);
    }

    public delegate void displayMessageHandler(string msg);
    public void displayMessage(string msg)
    {
      if(this.InvokeRequired) { this.Invoke(new displayMessageHandler(displayMessage), msg); return; }
      if(msg == null)
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
      if(wc != null)
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
      if(item != null)
      {
        if(item.Tag.GetType() == typeof(DriveInfo))
        {
          DirectoryInfo old = localFolder;
          localFolder = ((DriveInfo)item.Tag).RootDirectory;
          if(updateLocalFileView() == false) { localFolder = old; updateLocalFileView(); }
          Settings.SetRegValue("LocalPath", (localFolder == null) ? "" : localFolder.FullName);
        }
        else if(item.Tag.GetType() == typeof(DirectoryInfo))
        {
          DirectoryInfo old = localFolder;
          localFolder = (DirectoryInfo)item.Tag;
          if(updateLocalFileView() == false) { localFolder = old; updateLocalFileView(); }
          Settings.SetRegValue("LocalPath", (localFolder == null) ? "" : localFolder.FullName);
        }
        UpdateLocalPathDisplay();
      }
    }

    private void localUpButton_Click(object sender, EventArgs e)
    {
      localFolder = localFolder.Parent;
      Settings.SetRegValue("LocalPath", (localFolder == null) ? "" : localFolder.FullName);
      updateLocalFileView();
      UpdateLocalPathDisplay();
    }

    private void rightListView_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      ListViewItem item = rightListView.GetItemAt(e.X, e.Y);
      if(item != null)
      {
        string r = remoteFolder;
        if((item.ImageIndex == 0) || (item.ImageIndex == 1))
        {
          if((r == null) || (r == ""))
          {
            r = item.Text;
          }
          else
          {
            if(remoteFolder.EndsWith("/")) { r = remoteFolder + item.Text; } else { r = remoteFolder + "/" + item.Text; }
          }
          requestRemoteFolder(r);
        }
      }
    }

    private void remoteUpButton_Click(object sender, EventArgs e)
    {
      //string r = remoteFolder;
      //if(r.EndsWith("/")) { r = r.Substring(0, r.Length - 1); }
      if (remoteFolder.EndsWith("/")) { remoteFolder = remoteFolder.Substring(0, remoteFolder.Length - 1); }
      int i = remoteFolder.LastIndexOf("/");
      //if(i >= 0) { r = r.Substring(0, i + 1); } else { r = ""; }
      if (i >= 0) { remoteFolder = remoteFolder.Substring(0, i + 1); } else { remoteFolder = ""; }
      //requestRemoteFolder(r);
      requestRemoteFolder(remoteFolder); // This will also call UpdateRemotePathDisplay
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
      if(remoteFolder == null) return;
      FilenamePromptForm f = new FilenamePromptForm(Translate.T(Properties.Resources.CreateFolder), "");
      if(f.ShowDialog(this) == DialogResult.OK)
      {
        string r;
        if(remoteFolder.EndsWith("/")) { r = remoteFolder + f.filename; } else { r = remoteFolder + "/" + f.filename; }
        requestCreateFolder(r);
        remoteRefresh();
      }
    }

    private void remoteZipButton_Click(object sender, EventArgs e)
    {
      if(remoteFolder == null) return;
      FilenamePromptForm f = new FilenamePromptForm(Translate.T(Properties.Resources.ZipSelectedFiles), "");
      if(f.ShowDialog(this) == DialogResult.OK)
      {
        string r = f.filename;
        if(!r.ToLower().EndsWith(".zip")) { r += ".zip"; }
        ArrayList filesArray = new ArrayList();
        foreach(ListViewItem l in rightListView.SelectedItems) { filesArray.Add(l.Text); }
        string[] files = (string[])filesArray.ToArray(typeof(string));
        requestCreateZipFileFolder(remoteFolder, r, files);
      }
    }

    private void localRootButton_Click(object sender, EventArgs e)
    {
      localFolder = null;
      Settings.SetRegValue("LocalPath", "");
      updateLocalFileView();
      UpdateLocalPathDisplay();
    }

    private void remoteRootButton_Click(object sender, EventArgs e)
    {
      requestRemoteFolder("");
      //UpdateRemotePathDisplay();
    }

    private void rightListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if(node.agentid < 5)
      {
        remoteDeleteButton.Enabled = remoteZipButton.Enabled = (!((remoteFolder == null) || (remoteFolder == ""))) && (rightListView.SelectedItems.Count > 0);
      }
      else
      {
        remoteDeleteButton.Enabled = remoteZipButton.Enabled = (rightListView.SelectedItems.Count > 0);
      }
      updateTransferButtons();
    }

    private void rightListView_Resize(object sender, EventArgs e)
    {
      if(rightListView.Columns[0].Width != (rightListView.Width - rightListView.Columns[1].Width - 150))
      {
        rightListView.Columns[0].Width = rightListView.Width - rightListView.Columns[1].Width - 150;
      }
    }

    private void leftListView_Resize(object sender, EventArgs e)
    {
      if(leftListView.Columns[0].Width != (leftListView.Width - leftListView.Columns[1].Width - 150))
      {
        leftListView.Columns[0].Width = leftListView.Width - leftListView.Columns[1].Width - 150;
      }
    }

    private void updateTimer_Tick(object sender, EventArgs e)
    {
      updateTimer.Enabled = false;
      if(remoteFolder != null) { requestRemoteFolder(remoteFolder); }
    }

    private void remoteDeleteButton_Click(object sender, EventArgs e)
    {
      bool rec = false;
      ArrayList filesArray = new ArrayList();
      foreach(ListViewItem l in rightListView.SelectedItems) { filesArray.Add(l.Text); if(l.ImageIndex == 1) { rec = true; } }
      string[] files = (string[])filesArray.ToArray(typeof(string));
      string msg = string.Format(Translate.T(Properties.Resources.RemoveXItems), files.Length);
      if(files.Length == 1) { msg = Translate.T(Properties.Resources.Remove1Item); }
      FileDeletePromptForm f = new FileDeletePromptForm(msg, rec);
      if(f.ShowDialog(this) == DialogResult.OK)
      {
        requestDelete(remoteFolder, files, f.recursive);
        remoteRefresh();
      }
    }

    private void remoteContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if((rightListView.SelectedItems.Count == 0) || ((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))))
      {
        deleteToolStripMenuItem.Visible = toolStripMenuItem1.Visible = renameToolStripMenuItem.Visible = compressToolStripMenuItem.Visible = false;
      }
      else if(rightListView.SelectedItems.Count == 1)
      {
        deleteToolStripMenuItem.Visible = toolStripMenuItem1.Visible = renameToolStripMenuItem.Visible = compressToolStripMenuItem.Visible = true;
      }
      else if(rightListView.SelectedItems.Count > 1)
      {
        renameToolStripMenuItem.Visible = false;
        deleteToolStripMenuItem.Visible = toolStripMenuItem1.Visible = compressToolStripMenuItem.Visible = true;
      }
    }

    private void renameToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string oldname = rightListView.SelectedItems[0].Text;
      if((rightListView.SelectedItems.Count != 1) || (remoteFolder == null)) return;
      FilenamePromptForm f = new FilenamePromptForm(Translate.T(Properties.Resources.Rename), oldname);
      if(f.ShowDialog(this) == DialogResult.OK)
      {
        if(oldname == f.filename) return;
        //string r;
        //if (remoteFolder.EndsWith("/")) { r = remoteFolder + f.filename; } else { r = remoteFolder + "/" + f.filename; }
        requestRename(remoteFolder, oldname, f.filename);
        remoteRefresh();
      }
    }

    private delegate void updateTransferButtonsHandler();

    private void updateTransferButtons()
    {
      if(this.InvokeRequired) { this.Invoke(new updateTransferButtonsHandler(updateTransferButtons)); return; }
      if((wc == null) || (wc.State != webSocketClient.ConnectionStates.Connected))
      {
        uploadButton.Enabled = false;
        downloadButton.Enabled = false;
      }
      else
      {
        // Set upload button
        bool uploadAllowed = true;
        if(localFolder == null) { uploadAllowed = false; }
        if(leftListView.SelectedItems.Count == 0) { uploadAllowed = false; }
        foreach(ListViewItem l in leftListView.SelectedItems) { if(l.ImageIndex != 2) { uploadAllowed = false; } }
        if((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { uploadAllowed = false; }
        uploadButton.Enabled = uploadAllowed;

        // Set download button
        bool downloadAllowed = true;
        if(localFolder == null) { downloadAllowed = false; }
        if(rightListView.SelectedItems.Count == 0) { downloadAllowed = false; }
        foreach(ListViewItem l in rightListView.SelectedItems) { if(l.ImageIndex != 1 && l.ImageIndex != 2) { downloadAllowed = false; } } // Flynn
        if((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { downloadAllowed = false; }
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
      // If a transfer is currently active, ignore this.
      if(uploadActive || downloadActive || (transferStatusForm != null)) return;

      // If any files are going to be overwritten
      int overWriteCount = 0;
      foreach(ListViewItem l in leftListView.SelectedItems)
      {
        if(l.ImageIndex == 2)
        {
          string filename = l.Text;

          foreach(ListViewItem l2 in rightListView.Items)
          {
            if(l2.ImageIndex == 2)
            {
              string filename2 = l2.Text;
              if(node.agentid < 5) { filename = filename.ToLower(); filename2 = filename2.ToLower(); }
              if(filename.Equals(filename2)) { overWriteCount++; }
            }
          }
        }
      }

      skipExistingFiles = true;
      if(overWriteCount > 0)
      {
        FileConfirmOverwriteForm f = new FileConfirmOverwriteForm();
        if(overWriteCount == 1) { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteOneFile), overWriteCount); } else { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteXfiles), overWriteCount); }
        if(f.ShowDialog(this) != DialogResult.OK) return;
        skipExistingFiles = f.skipExistingFiles;
      }

      uploadFileArrayPtr = 0;
      uploadFileArray = new ArrayList();
      uploadFileDuplicateArray = new Hashtable();

      foreach(ListViewItem l in leftListView.SelectedItems)
      {
        if(l.ImageIndex == 2)
        {
          bool overwrite = false;
          bool overwriteNotLarger = false;
          long remoteLength = 0;
          string filename = l.Text;
          foreach(ListViewItem l2 in rightListView.Items)
          {
            if(l2.ImageIndex == 2)
            {
              string filename2 = l2.Text;
              if(node.agentid < 5) { filename = filename.ToLower(); filename2 = filename2.ToLower(); }
              if(filename.Equals(filename2))
              {
                overwrite = true;
                if(skipExistingFiles == false)
                {
                  long localLength = new FileInfo(Path.Combine(localFolder.FullName, l.Text)).Length;
                  remoteLength = long.Parse(l2.SubItems[1].Text);
                  if(localLength >= remoteLength) { overwriteNotLarger = true; }
                }
                break;
              }
            }
          }
          if((skipExistingFiles == false) || (overwrite == false))
          {
            uploadFileArray.Add(l.Text);
            if(overwriteNotLarger) { uploadFileDuplicateArray.Add(Path.Combine(localFolder.FullName, l.Text), remoteLength); }
          }
        }
      }

      if(uploadFileArray.Count == 0) return;
      uploadLocalPath = localFolder;
      uploadRemotePath = remoteFolder;
      uploadActive = true;
      uploadStop = false;

      // Show transfer status dialog
      transferStatusForm = new FileTransferStatusForm(this);
      transferStatusForm.Show(this);

      uploadNextFile();
    }

    private void uploadNextFile()
    {
      if((uploadFileArray == null) || (uploadFileArray.Count == 0)) return;

      string localFilePath, localFileName;
      if(uploadLocalPath != null)
      {
        localFilePath = Path.Combine(uploadLocalPath.FullName, (string)uploadFileArray[uploadFileArrayPtr]);
        localFileName = (string)uploadFileArray[uploadFileArrayPtr];
      }
      else
      {
        localFilePath = (string)uploadFileArray[uploadFileArrayPtr];
        localFileName = Path.GetFileName(localFilePath);
      }
      try { uploadFileStream = File.OpenRead(localFilePath); }
      catch(Exception)
      {
        // Display the error
        if(transferStatusForm != null) { transferStatusForm.addErrorMessage(String.Format(Translate.T(Properties.Resources.UnableToOpenFileX), localFileName)); }

        // Skip to the next file
        // Check if another file needs to be uploaded
        if(uploadFileArray.Count > (uploadFileArrayPtr + 1))
        {
          // Upload the next file
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
          uploadFileDuplicateArray = null;
          uploadLocalPath = null;
          uploadRemotePath = null;
          uploadFilePtr = 0;
          uploadFileStartPtr = 0;
          uploadFileSize = 0;
          uploadFileName = null;
          closeTransferDialog();
          remoteRefresh();
        }
        return;
      }
      uploadFileSize = new FileInfo(localFilePath).Length;
      uploadFilePtr = 0;
      uploadFileStartPtr = 0;
      uploadFileStartTime = DateTime.Now;
      uploadFileName = localFileName;

      // Check if the files already exist on the remote side
      if(uploadFileDuplicateArray[localFilePath] != null)
      {
        // Send UPLOADHASH command
        string cmd = "{\"action\":\"uploadhash\",\"reqid\":" + (uploadFileArrayPtr + 1000) + ",\"path\":\"" + uploadRemotePath + "\",\"name\":\"" + localFileName + "\",\"tag\":" + uploadFileDuplicateArray[localFilePath] + "}";
        byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
        wc.SendBinary(bincmd, 0, bincmd.Length);
      }
      else
      {
        // Send UPLOAD command
        string cmd = "{\"action\":\"upload\",\"reqid\":" + (uploadFileArrayPtr + 1000) + ",\"path\":\"" + uploadRemotePath + "\",\"name\":\"" + localFileName + "\",\"size\":" + uploadFileSize + "}";
        byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
        wc.SendBinary(bincmd, 0, bincmd.Length);
      }
    }

    public void uploadCancel()
    {
      if(uploadActive == false) return;

      // Send UPLOADCANCEL command
      string cmd = "{\"action\":\"uploadcancel\",\"reqid\":" + (uploadFileArrayPtr + 1000) + "}";
      byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
      wc.SendBinary(bincmd, 0, bincmd.Length);

      // Clear state
      uploadActive = false;
      uploadStop = false;
      uploadFileArrayPtr = -1;
      uploadFileArray = null;
      uploadFileDuplicateArray = null;
      uploadLocalPath = null;
      uploadRemotePath = null;
      uploadFilePtr = 0;
      uploadFileStartPtr = 0;
      uploadFileSize = 0;
      if(uploadFileStream != null) { uploadFileStream.Close(); uploadFileStream = null; }
      closeTransferDialog();
      remoteRefresh();
    }

    private void uploadNextPart(bool dataPriming)
    {
      if(uploadActive == false) return;
      byte[] buffer = new byte[16385];
      int len = uploadFileStream.Read(buffer, 1, buffer.Length - 1);
      if(dataPriming && (len == 0)) return;
      uploadFilePtr += len;

      if(len == 0)
      {
        // Send UPLOADDONE command
        string cmd = "{\"action\":\"uploaddone\",\"reqid\":" + (uploadFileArrayPtr + 1000) + "}";
        byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
        wc.SendBinary(bincmd, 0, bincmd.Length);
      }
      else
      {
        // Send part of the file
        // If starts with 0 or {, add a zero char at the start of the send, this will indicate that it's not a JSON command.
        if((buffer[1] == 123) || (buffer[1] == 0)) { wc.SendBinary(buffer, 0, len + 1); } else { wc.SendBinary(buffer, 1, len); }
      }
    }

    private delegate void closeTransferDialogHandler();

    private void closeTransferDialog()
    {
      if(transferStatusForm == null) return;
      if(this.InvokeRequired) { this.Invoke(new closeTransferDialogHandler(closeTransferDialog)); return; }
      if(transferStatusForm.showingError == false)
      {
        // Everything was succesful, close the form
        transferStatusForm.Close();
        transferStatusForm = null;
      }
      else
      {
        // Error are displayed, keep the form open
        transferStatusForm.transferCompleted();
      }

      if (downloadActive) remoteFolder = strDownloadRel;
      remoteRefresh();
    }

    private void downloadButton_Click(object sender, EventArgs e)
    {
      // If a transfer is currently active, ignore this.
      if(uploadActive || downloadActive || (transferStatusForm != null)) return;

      strDownloadRel = remoteFolder;

      skipExistingFiles = false;

      // Perform the download
      downloadFileArrayPtr = 0;
      downloadFileArray = new ArrayList();
      downloadFileSizeArray = new ArrayList();

      bDontupdateRemoteFileView = true;

      foreach(ListViewItem l in rightListView.SelectedItems)
      {
        if(l.ImageIndex == 1) // Folder
        {
          RekursivelyCollectDownloadFiles(strDownloadRel+"/"+l.Text);
        }
        else if(l.ImageIndex == 2) // File
        {
            downloadFileArray.Add(strDownloadRel+"/"+l.Text);
            downloadFileSizeArray.Add(int.Parse(l.SubItems[1].Text));          
        }
      }
      bDontupdateRemoteFileView = false;

      #region SKIP existing?
      downloadLocalPath = localFolder;
      // If any files are going to be overwritten
      int overWriteCount = 0;
      for(int i = 0; i < downloadFileArray.Count; i++)
      {
        string localFilePath;
        String strDownloadFileString = (string)downloadFileArray[i];
        if(strDownloadRel.Length > 0)
        {
          strDownloadFileString = strDownloadFileString.Substring(strDownloadRel.Length);
          if(strDownloadFileString.StartsWith("/")) strDownloadFileString = strDownloadFileString.Substring(1);
        }
        localFilePath = Path.Combine(downloadLocalPath.FullName, strDownloadFileString.Replace("/", "\\"));

        if(File.Exists(localFilePath) && (new System.IO.FileInfo(localFilePath).Length == Convert.ToInt64(downloadFileSizeArray[i]))) { overWriteCount++; }
      }

      if(overWriteCount > 0)
      {
        FileConfirmOverwriteForm f = new FileConfirmOverwriteForm();
        if(overWriteCount == 1) { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteOneFile), overWriteCount); } else { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteXfiles), overWriteCount); }
        if (f.ShowDialog(this) != DialogResult.OK)
        {
          remoteFolder = strDownloadRel;
          return;
        }
        skipExistingFiles = f.skipExistingFiles;
      }

      if(skipExistingFiles)
      {
        for(int i = 0; i < downloadFileArray.Count; i++)
        {
          string localFilePath;
          String strDownloadFileString = (string)downloadFileArray[i];
          if(strDownloadRel.Length > 0)
          {
            strDownloadFileString = strDownloadFileString.Substring(strDownloadRel.Length);
            if(strDownloadFileString.StartsWith("/")) strDownloadFileString = strDownloadFileString.Substring(1);
          }
          localFilePath = Path.Combine(downloadLocalPath.FullName, strDownloadFileString.Replace("/", "\\"));

          if(File.Exists(localFilePath) && (new System.IO.FileInfo(localFilePath).Length == Convert.ToInt64(downloadFileSizeArray[i])))
          {
            downloadFileArray.RemoveAt(i);
            downloadFileSizeArray.RemoveAt(i);
            i--;
          }
        }
      }





      #endregion



      if(downloadFileArray.Count == 0) return;
      downloadLocalPath = localFolder;
      downloadRemotePath = remoteFolder;
      downloadActive = true;
      downloadStop = false;

      // Show transfer status dialog
      transferStatusForm = new FileTransferStatusForm(this);
      transferStatusForm.Show(this);

      downloadNextFile();
    }

    private void RekursivelyCollectDownloadFiles(String strFolder)
    {
      requestRemoteFolder(strFolder);
      DateTime to = DateTime.Now.AddSeconds(5);
      while(DateTime.Now < to && remoteFolder != strFolder)
        Application.DoEvents();
      List<Fle> FileList = new List<Fle>();
      for(int i = 0; i < remoteFolderList.Count; i++)
      {
        Dictionary<string, object> fileItem = (Dictionary<string, object>)remoteFolderList[i];
        Fle file = new Fle();
        file.Size = -1;
        file.Path = strFolder.Replace("//", "/");
        if(fileItem.ContainsKey("t")) { file.Icon = (int)fileItem["t"]; }
        if(fileItem.ContainsKey("n")) { file.Name = (string)fileItem["n"]; }
        if(fileItem.ContainsKey("d")) { file.Date = (string)fileItem["d"]; }
        if(fileItem.ContainsKey("s"))
        {
          if(fileItem["s"].GetType() == typeof(System.Int32)) { file.Size = (int)fileItem["s"]; }
          if(fileItem["s"].GetType() == typeof(System.Int64)) { file.Size = (long)fileItem["s"]; }
        }
        FileList.Add(file); // Folder
      }

      for(int i = 0; i < FileList.Count; i++)
      {
        Fle file = FileList[i];
        if(file.Icon == 2) // Folder
        {
          RekursivelyCollectDownloadFiles(file.Path+"/"+file.Name);
        }
        else if(file.Icon == 3) // File
        {
          if(true) // Abfrage Skip
          {
            downloadFileArray.Add(file.Path+"/"+file.Name);
            downloadFileSizeArray.Add(file.Size);
          }
        }
      }
    }

    String strDownloadRel = "";
    private void downloadNextFile()
    {
      if((downloadFileArray == null) || (downloadFileArray.Count == 0)) return;

      string localFilePath;
      String strDownloadFileString = (string)downloadFileArray[downloadFileArrayPtr];
      if(strDownloadRel.Length > 0)
      {
        strDownloadFileString = strDownloadFileString.Substring(strDownloadRel.Length);
        if(strDownloadFileString.StartsWith("/")) strDownloadFileString = strDownloadFileString.Substring(1);
      }
      localFilePath = Path.Combine(downloadLocalPath.FullName, strDownloadFileString.Replace("/", "\\"));
      if(!Directory.Exists(Path.GetDirectoryName(localFilePath))) Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));
      try { downloadFileStream = File.OpenWrite(localFilePath); }
      catch(Exception)
      {
        // Download error, show it in the dialog
        FileInfo f = new FileInfo(localFilePath);
        if(transferStatusForm != null) { transferStatusForm.addErrorMessage(String.Format(Translate.T(Properties.Resources.UnableToWriteFileX), f.Name)); }

        // Unable to download the file, skip it.
        if(downloadFileArray.Count > downloadFileArrayPtr + 1)
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
        return;
      }

      if(downloadFileSizeArray[downloadFileArrayPtr].GetType() == typeof(System.Int32)) { downloadFileSize = (int)downloadFileSizeArray[downloadFileArrayPtr]; }
      if(downloadFileSizeArray[downloadFileArrayPtr].GetType() == typeof(System.Int64)) { downloadFileSize = (long)downloadFileSizeArray[downloadFileArrayPtr]; }

      downloadFilePtr = 0;
      downloadFileStartTime = DateTime.Now;

      string r;
      if(downloadRemotePath.EndsWith("/")) { r = downloadRemotePath + Path.GetFileName((String)downloadFileArray[downloadFileArrayPtr]); } else { r = downloadRemotePath + "/" + Path.GetFileName((String)downloadFileArray[downloadFileArrayPtr]); }
      r=(String)downloadFileArray[downloadFileArrayPtr];
      // Send DOWNLOAD command
      string cmd = "{\"action\":\"download\",\"sub\":\"start\",\"id\":" + (downloadFileArrayPtr + 1000) + ",\"path\":\"" + r + "\"}";
      byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
      wc.SendBinary(bincmd, 0, bincmd.Length);
    }

    private void downloadGotBinaryData(byte[] data, int offset, int length)
    {
      if((length < 4) || (downloadFileStream == null)) return;
      if(length > 4)
      {
        // Save part to disk
        downloadFileStream.Write(data, offset + 4, length - 4);
        downloadFilePtr += (length - 4);
      }
      int controlBits = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, offset));
      if((controlBits & 1) != 0)
      {
        if(downloadFileStream != null) { downloadFileStream.Close(); downloadFileStream = null; }
        downloadFilePtr = 0;
        downloadFileSize = 0;

        if(downloadFileArray.Count > downloadFileArrayPtr + 1)
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
      if(downloadActive == false) return;

      // Send DOWNLOAD command
      string cmd = "{\"action\":\"download\",\"sub\":\"stop\",\"id\":" + (downloadFileArrayPtr + 1000) + "}";
      byte[] bincmd = UTF8Encoding.UTF8.GetBytes(cmd);
      wc.SendBinary(bincmd, 0, bincmd.Length);

      // Done with all files
      if(downloadFileStream != null) { downloadFileStream.Close(); downloadFileStream = null; }
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
      if(localFolder == null) return;
      FilenamePromptForm f = new FilenamePromptForm(Translate.T(Properties.Resources.CreateFolder), "");
      if(f.ShowDialog(this) == DialogResult.OK)
      {
        Directory.CreateDirectory(Path.Combine(localFolder.FullName, f.filename));
        updateLocalFileView();
      }
    }

    private void localDeleteButton_Click(object sender, EventArgs e)
    {
      bool rec = false;
      ArrayList filesArray = new ArrayList();
      foreach(ListViewItem l in leftListView.SelectedItems) { filesArray.Add(l.Text); if(l.ImageIndex == 1) { rec = true; } }
      string[] files = (string[])filesArray.ToArray(typeof(string));
      string msg = string.Format(Translate.T(Properties.Resources.RemoveXItems), files.Length);
      if(files.Length == 1) { msg = Translate.T(Properties.Resources.Remove1Item); }
      FileDeletePromptForm f = new FileDeletePromptForm(msg, rec);
      if(f.ShowDialog(this) == DialogResult.OK)
      {
        foreach(string file in filesArray)
        {
          try
          {
            string fullpath = Path.Combine(localFolder.FullName, file);
            FileAttributes attr = File.GetAttributes(fullpath);
            if((attr & FileAttributes.Directory) == FileAttributes.Directory) { Directory.Delete(fullpath, f.recursive); } else { File.Delete(fullpath); }
          }
          catch(Exception) { }
        }
        updateLocalFileView();
      }
    }

    private void rightListView_DragEnter(object sender, DragEventArgs e)
    {
      if(uploadActive || downloadActive || (transferStatusForm != null)) return;
      if((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { return; }
      if(e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
    }

    private void rightListView_DragDrop(object sender, DragEventArgs e)
    {
      if(uploadActive || downloadActive || (transferStatusForm != null)) return;
      if((node.agentid < 5) && ((remoteFolder == null) || (remoteFolder == ""))) { return; }
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

      // If any files are going to be overwritten
      int overWriteCount = 0;
      foreach(string file in files)
      {
        FileInfo f = new FileInfo(file);
        string filename = f.Name;
        foreach(ListViewItem l2 in rightListView.Items)
        {
          if(l2.ImageIndex == 2)
          {
            string filename2 = l2.Text;
            if(node.agentid < 5) { filename = filename.ToLower(); filename2 = filename2.ToLower(); }
            if(filename.Equals(filename2)) { overWriteCount++; }
          }
        }
      }
      skipExistingFiles = true;
      if(overWriteCount > 0)
      {
        FileConfirmOverwriteForm f = new FileConfirmOverwriteForm();
        if(overWriteCount == 1) { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteOneFile), overWriteCount); } else { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteXfiles), overWriteCount); }
        if(f.ShowDialog(this) != DialogResult.OK) return;
        skipExistingFiles = f.skipExistingFiles;
      }

      // Perform the upload
      uploadFileArrayPtr = 0;
      uploadFileArray = new ArrayList();
      uploadFileDuplicateArray = new Hashtable();

      foreach(string file in files)
      {
        bool overwrite = false;
        bool overwriteNotLarger = false;
        long remoteLength = 0;
        string filename = Path.GetFileName(file);
        foreach(ListViewItem l2 in rightListView.Items)
        {
          if(l2.ImageIndex == 2)
          {
            string filename2 = l2.Text;
            if(node.agentid < 5) { filename = filename.ToLower(); filename2 = filename2.ToLower(); }
            if(filename.Equals(filename2))
            {
              overwrite = true;
              if(skipExistingFiles == false)
              {
                long localLength = new FileInfo(file).Length;
                remoteLength = long.Parse(l2.SubItems[1].Text);
                if(localLength >= remoteLength) { overwriteNotLarger = true; }
              }
              break;
            }
          }
        }
        if((skipExistingFiles == false) || (overwrite == false))
        {
          uploadFileArray.Add(file);
          if(overwriteNotLarger) { uploadFileDuplicateArray.Add(file, remoteLength); }
        }
      }

      if(uploadFileArray.Count == 0) return;
      uploadLocalPath = null;
      uploadRemotePath = remoteFolder;
      uploadActive = true;
      uploadStop = false;

      // Show transfer status dialog
      transferStatusForm = new FileTransferStatusForm(this);
      transferStatusForm.Show(this);

      uploadNextFile();
    }

    private void leftListView_MouseMove(object sender, MouseEventArgs e)
    {
      if(e.Button == MouseButtons.Left)
      {
        ArrayList files = new ArrayList();
        foreach(ListViewItem l in leftListView.SelectedItems) { if(l.ImageIndex == 2) { files.Add(Path.Combine(localFolder.FullName, l.Text)); } }
        if(files.Count > 0)
        {
          leftListView.DoDragDrop(new DataObject(DataFormats.FileDrop, (string[])files.ToArray(typeof(string))), DragDropEffects.Copy);
        }
      }
    }

    private void rightListView_MouseMove(object sender, MouseEventArgs e)
    {
      if(e.Button == MouseButtons.Left)
      {
        ArrayList XdownloadFileArray = new ArrayList();
        ArrayList XdownloadFileSizeArray = new ArrayList();
        foreach(ListViewItem l in rightListView.SelectedItems)
        {
          if(l.ImageIndex == 2)
          {
            XdownloadFileArray.Add(l.Text);
            XdownloadFileSizeArray.Add(int.Parse(l.SubItems[1].Text));
          }
        }
        if(XdownloadFileArray.Count > 0)
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
      if(uploadActive || downloadActive || (localFolder == null) || (transferStatusForm != null)) return;
      if((e.Data.GetDataPresent("Type") == true) && ((string)e.Data.GetData("Type") == ("MeshCentralRouterRemoteFiles-" + rndString))) { e.Effect = DragDropEffects.Copy; }
    }

    private void leftListView_DragDrop(object sender, DragEventArgs e)
    {
      if(uploadActive || downloadActive || (transferStatusForm != null)) return;
      if((e.Data.GetDataPresent("Type") == false) || ((string)e.Data.GetData("Type") != ("MeshCentralRouterRemoteFiles-" + rndString))) return;

      ArrayList files = (ArrayList)e.Data.GetData("RemoteFiles");

      // If any files are going to be overwritten
      int overWriteCount = 0;
      foreach(string file in files)
      {
        string filename = file;
        foreach(ListViewItem l2 in leftListView.Items)
        {
          if(l2.ImageIndex == 2)
          {
            string filename2 = l2.Text;
            if(node.agentid < 5) { filename = filename.ToLower(); filename2 = filename2.ToLower(); }
            if(filename.Equals(filename2)) { overWriteCount++; }
          }
        }
      }
      skipExistingFiles = true;
      if(overWriteCount > 0)
      {
        FileConfirmOverwriteForm f = new FileConfirmOverwriteForm();
        if(overWriteCount == 1) { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteOneFile), overWriteCount); } else { f.mainTextLabel = String.Format(Translate.T(Properties.Resources.OverwriteXfiles), overWriteCount); }
        if(f.ShowDialog(this) != DialogResult.OK) return;
        skipExistingFiles = f.skipExistingFiles;
      }

      // Perform downloads
      downloadFileArrayPtr = 0;
      downloadFileArray = (ArrayList)e.Data.GetData("RemoteFiles");
      downloadFileSizeArray = (ArrayList)e.Data.GetData("RemoteSizes");

      if(skipExistingFiles == true)
      {
        ArrayList downloadFileArray2 = new ArrayList();
        ArrayList downloadFileSizeArray2 = new ArrayList();

        for(int i = 0; i < downloadFileArray.Count; i++)
        {
          bool overwrite = false;
          string filename = (string)downloadFileArray[i];
          foreach(ListViewItem l2 in leftListView.Items)
          {
            if(l2.ImageIndex == 2)
            {
              string filename2 = l2.Text;
              if(node.agentid < 5) { filename = filename.ToLower(); filename2 = filename2.ToLower(); }
              if(filename.Equals(filename2)) { overwrite = true; }
            }
          }
          if(overwrite == false)
          {
            downloadFileArray2.Add(downloadFileArray[i]);
            downloadFileSizeArray2.Add(downloadFileSizeArray[i]);
          }
        }

        downloadFileArray = downloadFileArray2;
        downloadFileSizeArray = downloadFileSizeArray2;
      }

      if(downloadFileArray.Count == 0) return;

      downloadLocalPath = localFolder;
      downloadRemotePath = (string)e.Data.GetData("RemoteFolder");
      downloadActive = true;
      downloadStop = false;

      // Show transfer status dialog
      transferStatusForm = new FileTransferStatusForm(this);
      transferStatusForm.Show(this);

      downloadNextFile();
    }
    private static string getRandomString(int length)
    {
      using(var rng = new RNGCryptoServiceProvider())
      {
        var bytes = new byte[(((length * 6) + 7) / 8)];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
      }
    }

    bool isPointVisibleOnAScreen(Point p)
    {
      foreach(Screen s in Screen.AllScreens) { if((p.X < s.Bounds.Right) && (p.X > s.Bounds.Left) && (p.Y > s.Bounds.Top) && (p.Y < s.Bounds.Bottom)) return true; }
      return false;
    }

    private void localContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if((leftListView.SelectedItems.Count == 0) || (localFolder == null))
      {
        deleteToolStripMenuItem1.Visible = toolStripMenuItem2.Visible = renameToolStripMenuItem1.Visible = false;
      }
      else if(leftListView.SelectedItems.Count == 1)
      {
        deleteToolStripMenuItem1.Visible = toolStripMenuItem2.Visible = renameToolStripMenuItem1.Visible = true;
      }
      else if(leftListView.SelectedItems.Count > 1)
      {
        renameToolStripMenuItem1.Visible = false;
        deleteToolStripMenuItem1.Visible = toolStripMenuItem2.Visible = true;
      }
    }

    private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if((leftListView.SelectedItems.Count != 1) || (localFolder == null)) return;
      string oldname = leftListView.SelectedItems[0].Text;
      FilenamePromptForm f = new FilenamePromptForm(Translate.T(Properties.Resources.Rename), oldname);
      if(f.ShowDialog(this) == DialogResult.OK)
      {
        if(oldname == f.filename) return;
        FileInfo fileinfo = new FileInfo(Path.Combine(localFolder.FullName, oldname));
        if(fileinfo.Exists == false) return;
        try { fileinfo.MoveTo(Path.Combine(localFolder.FullName, f.filename)); } catch(Exception) { }
        localRefresh();
      }
    }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }

    public class Fle
  {
    public String Path = "";
    public String Name = "";
    public String Date = "";
    public long Size = 0;
    public int Icon = 0;
  }
}
