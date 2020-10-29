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
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Principal;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;
using System.Drawing;

namespace MeshCentralRouter
{
    public partial class MainForm : Form
    {
        public int currentPanel = 0;
        public DateTime refreshTime = DateTime.Now;
        public MeshCentralServer meshcentral = null;
        public X509Certificate2 lastBadConnectCert = null;
        public string title;
        public string[] args;
        public bool debug = false;
        public bool tlsdump = false;
        public bool autoLogin = false;
        public bool ignoreCert = false;
        public bool inaddrany = false;
        public bool forceExit = false;
        public bool sendEmailToken = false;
        public bool sendSMSToken = false;
        public Uri authLoginUrl = null;
        public Process installProcess = null;
        public string acceptableCertHash = null;
        public ArrayList mappingsToSetup = null;
        public bool deviceListViewMode = true;

        public bool isRouterHooked()
        {
            try
            {
                return (
                    ((string)Registry.GetValue(@"HKEY_CLASSES_ROOT\mcrouter", "", null) == "MeshCentral Router") &&
                    ((string)Registry.GetValue(@"HKEY_CLASSES_ROOT\mcrouter\shell\open\command", "", null) == "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"")
                );
            }
            catch (Exception) { }
            return false;
        }

        public void hookRouter()
        {
            try
            {
                Registry.SetValue(@"HKEY_CLASSES_ROOT\mcrouter", "", "MeshCentral Router");
                Registry.SetValue(@"HKEY_CLASSES_ROOT\mcrouter", "URL Protocol", "");
                Registry.SetValue(@"HKEY_CLASSES_ROOT\mcrouter\shell\open\command", "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
            }
            catch (Exception) { }
        }
        public void unHookRouter()
        {
            try { Registry.ClassesRoot.DeleteSubKeyTree("mcrouter"); } catch (Exception) { }
        }

        private void hookRouterEx()
        {
            if (IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName, "-install");
                startInfo.Verb = "runas";
                try { installProcess = System.Diagnostics.Process.Start(startInfo); } catch (Exception) { return; }
                installTimer.Enabled = true;
                installButton.Visible = false;
            } else {
                hookRouter();
                installButton.Visible = !isRouterHooked();
            }
        }

        private void installTimer_Tick(object sender, EventArgs e)
        {
            if ((installProcess == null) || (installProcess.HasExited == true))
            {
                installTimer.Enabled = false;
                installButton.Visible = !isRouterHooked();
            }
        }

        private bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void DeleteSubKeyTree(RegistryKey key, string subkey) { if (key.OpenSubKey(subkey) == null) { return; } DeleteSubKeyTree(key, subkey); }

        public class DeviceComparer : IComparer
        {
            public int Compare(Object a, Object b)
            {
                string ax = ((DeviceUserControl)a).node.name.ToLower();
                string bx = ((DeviceUserControl)b).node.name.ToLower();
                return bx.CompareTo(ax);
            }
        }
        public class DeviceGroupComparer : IComparer
        {
            public int Compare(Object a, Object b)
            {
                string ax = ((DeviceUserControl)a).mesh.name.ToLower() + ", " + ((DeviceUserControl)a).node.name.ToLower();
                string bx = ((DeviceUserControl)a).mesh.name.ToLower() + ", " + ((DeviceUserControl)b).node.name.ToLower();
                return bx.CompareTo(ax);
            }
        }

        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        public void setRegValue(string name, string value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value); } catch (Exception) { }
        }
        public string getRegValue(string name, string value)
        {
            try {return Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value).ToString(); } catch (Exception) { return value; }
        }

        public MainForm(string[] args)
        {
            this.args = args;
            InitializeComponent();
            mainPanel.Controls.Add(panel1);
            mainPanel.Controls.Add(panel2);
            mainPanel.Controls.Add(panel3);
            mainPanel.Controls.Add(panel4);
            pictureBox1.SendToBack();
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            versionLabel.Text = "v" + version.Major + "." + version.Minor + "." + version.Build;

            serverNameComboBox.Text = getRegValue("ServerName", "");
            userNameTextBox.Text = getRegValue("UserName", "");
            title = this.Text;

            int argflags = 0;
            foreach (string arg in this.args) {
                if (arg.ToLower() == "-oldstyle") { deviceListViewMode = false; }
                if (arg.ToLower() == "-install") { hookRouter(); forceExit = true; return; }
                if (arg.ToLower() == "-uninstall") { unHookRouter(); forceExit = true; return; }
                if (arg.ToLower() == "-debug") { debug = true; }
                if (arg.ToLower() == "-tlsdump") { tlsdump = true; }
                if (arg.ToLower() == "-ignorecert") { ignoreCert = true; }
                if (arg.ToLower() == "-all") { inaddrany = true; }
                if (arg.ToLower() == "-inaddrany") { inaddrany = true; }
                if (arg.ToLower() == "-tray") { notifyIcon.Visible = true; this.ShowInTaskbar = false; this.MinimizeBox = false; }
                if (arg.Length > 6 && arg.Substring(0, 6).ToLower() == "-host:") { serverNameComboBox.Text = arg.Substring(6); argflags |= 1; }
                if (arg.Length > 6 && arg.Substring(0, 6).ToLower() == "-user:") { userNameTextBox.Text = arg.Substring(6); argflags |= 2; }
                if (arg.Length > 6 && arg.Substring(0, 6).ToLower() == "-pass:") { passwordTextBox.Text = arg.Substring(6); argflags |= 4; }
                if (arg.Length > 8 && arg.Substring(0, 8).ToLower() == "-search:") { searchTextBox.Text = arg.Substring(8); }
                if (arg.Length > 11 && arg.Substring(0, 11).ToLower() == "mcrouter://") { authLoginUrl = new Uri(arg); }
                if ((arg.Length > 1) && (arg[0] != '-') && (arg.ToLower().EndsWith(".mcrouter"))) { try { argflags |= loadMappingFile(File.ReadAllText(arg), 1); } catch (Exception) { } }
                if (arg.ToLower() == "-localfiles") { FileViewer fileViewer = new FileViewer(meshcentral, null); fileViewer.Show(); }
            }
            autoLogin = (argflags == 7);

            // Set automatic port map values
            if (authLoginUrl != null) {
                string autoNodeId = null;
                int autoRemotePort = 0;
                int autoProtocol = 0;
                int autoAppId = 0;
                bool autoExit = false;
                try
                {
                    // Automatic mappings
                    autoNodeId = getValueFromQueryString(authLoginUrl.Query, "nodeid");
                    autoRemotePort = int.Parse(getValueFromQueryString(authLoginUrl.Query, "remoteport"));
                    autoProtocol = int.Parse(getValueFromQueryString(authLoginUrl.Query, "protocol"));
                    autoAppId = int.Parse(getValueFromQueryString(authLoginUrl.Query, "appid"));
                    autoExit = (getValueFromQueryString(authLoginUrl.Query, "autoexit") == "1");
                }
                catch (Exception) { }
                if ((autoRemotePort != 0) && (autoProtocol != 0) && (autoNodeId != null)) {
                    Dictionary<string, object> map = new Dictionary<string, object>();
                    map.Add("nodeId", autoNodeId);
                    map.Add("remotePort", autoRemotePort);
                    map.Add("localPort", 0);
                    map.Add("protocol", autoProtocol);
                    map.Add("appId", autoAppId);
                    map.Add("autoExit", autoExit);
                    map.Add("launch", 1);
                    mappingsToSetup = new ArrayList();
                    mappingsToSetup.Add(map);
                    devicesTabControl.SelectedIndex = 1;
                }
            }

            // Check MeshCentral .mcrouter hook
            installButton.Visible = !isRouterHooked();
        }

        private void setPanel(int newPanel)
        {
            if (currentPanel == newPanel) return;
            if (newPanel == 4) { updatePanel4(); }
            panel1.Visible = (newPanel == 1);
            panel2.Visible = (newPanel == 2);
            panel3.Visible = (newPanel == 3);
            panel4.Visible = (newPanel == 4);
            currentPanel = newPanel;

            // Setup stuff
            if (newPanel == 1) { tokenRememberCheckBox.Checked = false; }
            nextButton2.Enabled = (tokenTextBox.Text.Replace(" ", "") != "");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load registry settings
            showGroupNamesToolStripMenuItem.Checked = (getRegValue("Show Group Names", "1") == "1");
            showOfflineDevicesToolStripMenuItem.Checked = (getRegValue("Show Offline Devices", "1") == "1");
            if (getRegValue("Device Sort", "Name") == "Name") {
                sortByNameToolStripMenuItem.Checked = true;
                sortByGroupToolStripMenuItem.Checked = false;
            } else {
                sortByNameToolStripMenuItem.Checked = false;
                sortByGroupToolStripMenuItem.Checked = true;
            }

            //Text += " - v" + Application.ProductVersion;
            //installPathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Open Source", "MeshCentral");
            //serverModeComboBox.SelectedIndex = 0;
            //windowColor = serverNameTextBox.BackColor;
            setPanel(1);
            updatePanel1(null, null);
            SendMessage(searchTextBox.Handle, EM_SETCUEBANNER, 0, Properties.Resources.SearchPlaceHolder);

            // Start the multicast scanner
            //scanner = new MeshDiscovery();
            //scanner.OnNotify += Scanner_OnNotify;
            //scanner.MulticastPing();

            if (autoLogin || (authLoginUrl != null)) { nextButton1_Click(null, null); }

            // Setup the device list view panel
            if (deviceListViewMode)
            {
                devicesListView.Top = 0;
                devicesListView.Left = 0;
                devicesListView.Width = devicesPanel.Width;
                devicesListView.Height = devicesPanel.Height;
                devicesListView.Visible = true;
                devicesPanel.AutoScroll = false;
                toolStripMenuItem2.Visible = false;
                sortByNameToolStripMenuItem.Visible = false;
                sortByGroupToolStripMenuItem.Visible = false;
            }

            // Restore Window Location
            string locationStr = getRegValue("location", "");
            if (locationStr != null)
            {
                string[] locationSplit = locationStr.Split(',');
                if (locationSplit.Length == 2)
                {
                    try
                    {
                        var x = int.Parse(locationSplit[0]);
                        var y = int.Parse(locationSplit[1]);
                        Point p = new Point(x, y);
                        if (isPointVisibleOnAScreen(p)) { Location = p; }
                    }
                    catch (Exception) { }
                }
            }
        }

        bool isPointVisibleOnAScreen(Point p)
        {
            foreach (Screen s in Screen.AllScreens) { if ((p.X < s.Bounds.Right) && (p.X > s.Bounds.Left) && (p.Y > s.Bounds.Top) && (p.Y < s.Bounds.Bottom)) return true; }
            return false;
        }

        private void updatePanel1(object sender, EventArgs e)
        {
            bool ok = true;
            if (serverNameComboBox.Text.Length == 0) { ok = false; }
            if (userNameTextBox.Text.Length == 0) { ok = false; }
            if (passwordTextBox.Text.Length == 0) { ok = false; }
            nextButton1.Enabled = ok;
        }

        private void updatePanel2(object sender, EventArgs e)
        {
            bool ok = true;
            if (tokenTextBox.Text.Length == 0) { ok = false; }
            nextButton2.Enabled = ok;
        }

        private void updatePanel4()
        {
            //ServerState s = readServerStateEx(installPathTextBox.Text);
            //if (s.state == ServerStateEnum.Running) { label7.Text = "MeshCentral is running this computer."; }
            //else if (s.state == ServerStateEnum.Unknown) { label7.Text = "MeshCentral is installed on this computer."; }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forceExit = true;
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((notifyIcon.Visible == true) && (forceExit == false)) { e.Cancel = true; Visible = false; }
            setRegValue("Location", Location.X + "," + Location.Y);
        }

        private void backButton5_Click(object sender, EventArgs e)
        {
            meshcentral.disconnect();
        }

        private static string getValueFromQueryString(string query, string name)
        {
            if ((query == null) || (name == null)) return null;
            int i = query.IndexOf("?" + name + "=");
            if (i == -1) { i = query.IndexOf("&" + name + "="); }
            if (i == -1) return null;
            string r = query.Substring(i + name.Length + 2);
            i = r.IndexOf("&");
            if (i >= 0) { r = r.Substring(0, i); }
            return r;
        }

        private void nextButton1_Click(object sender, EventArgs e)
        {
            // Attempt to login
            addButton.Enabled = false;
            addRelayButton.Enabled = false;
            openWebSiteButton.Visible = false;
            meshcentral = new MeshCentralServer();
            meshcentral.debug = debug;
            meshcentral.tlsdump = tlsdump;
            meshcentral.ignoreCert = ignoreCert;
            if (acceptableCertHash != null) { meshcentral.okCertHash2 = acceptableCertHash; }
            meshcentral.onStateChanged += Meshcentral_onStateChanged;
            meshcentral.onNodesChanged += Meshcentral_onNodesChanged;
            meshcentral.onLoginTokenChanged += Meshcentral_onLoginTokenChanged;
            meshcentral.onClipboardData += Meshcentral_onClipboardData;
            meshcentral.onTwoFactorCookie += Meshcentral_onTwoFactorCookie;
            if (lastBadConnectCert != null)
            {
                meshcentral.okCertHash = lastBadConnectCert.GetCertHashString();
            }
            else
            {
                string ignoreCert = getRegValue("IgnoreCert", null);
                if (ignoreCert != null) { meshcentral.okCertHash = ignoreCert; }
            }

            // Load two factor cookie if present
            string twoFactorCookie = getRegValue("TwoFactorCookie", null);
            if ((twoFactorCookie != null) && (twoFactorCookie != "")) { twoFactorCookie = "cookie=" + twoFactorCookie; } else { twoFactorCookie = null; }

            Uri serverurl = null;
            if (authLoginUrl != null) {
                try {
                    meshcentral.okCertHash2 = getValueFromQueryString(authLoginUrl.Query, "t");
                    string loginkey = getValueFromQueryString(authLoginUrl.Query, "key");
                    string urlstring = "wss://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath + "?auth=" + getValueFromQueryString(authLoginUrl.Query, "c");
                    if (loginkey != null) { urlstring += ("&key=" + loginkey); }
                    serverurl = new Uri(urlstring);
                } catch (Exception) { }
                meshcentral.connect(serverurl, null, null, null);
            } else {
                int keyIndex = serverNameComboBox.Text.IndexOf("?key=");
                if (keyIndex >= 0)
                {
                    string hostname = serverNameComboBox.Text.Substring(0, keyIndex);
                    string loginkey = serverNameComboBox.Text.Substring(keyIndex + 5);
                    try { serverurl = new Uri("wss://" + hostname + "/control.ashx?key=" + loginkey); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie);
                }
                else
                {
                    try { serverurl = new Uri("wss://" + serverNameComboBox.Text + "/control.ashx"); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie);
                }
            }
        }

        private void Meshcentral_onClipboardData(string nodeid, string data)
        {
            if (this.InvokeRequired) { this.Invoke(new MeshCentralServer.onClipboardDataHandler(Meshcentral_onClipboardData), nodeid, data); return; }
            Clipboard.SetData(DataFormats.Text, (Object)data);
        }

        private void Meshcentral_onTwoFactorCookie(string cookie)
        {
            if (this.InvokeRequired) { this.Invoke(new MeshCentralServer.twoFactorCookieHandler(Meshcentral_onTwoFactorCookie), cookie); return; }
            setRegValue("TwoFactorCookie", cookie);
        }

        private void nextButton3_Click(object sender, EventArgs e)
        {
            // If we need to remember this certificate
            if (rememberCertCheckBox.Checked) { setRegValue("IgnoreCert", lastBadConnectCert.GetCertHashString()); }

            // Attempt to login, ignore bad cert.
            addButton.Enabled = false;
            addRelayButton.Enabled = false;
            openWebSiteButton.Visible = false;
            meshcentral = new MeshCentralServer();
            meshcentral.debug = debug;
            meshcentral.tlsdump = tlsdump;
            meshcentral.ignoreCert = ignoreCert;
            meshcentral.onStateChanged += Meshcentral_onStateChanged;
            meshcentral.onNodesChanged += Meshcentral_onNodesChanged;
            meshcentral.onLoginTokenChanged += Meshcentral_onLoginTokenChanged;
            meshcentral.onClipboardData += Meshcentral_onClipboardData;
            meshcentral.onTwoFactorCookie += Meshcentral_onTwoFactorCookie;
            meshcentral.okCertHash = lastBadConnectCert.GetCertHashString();

            Uri serverurl = null;
            if (authLoginUrl != null) {
                meshcentral.okCertHash2 = getValueFromQueryString(authLoginUrl.Query, "t");
                string loginkey = getValueFromQueryString(authLoginUrl.Query, "key");
                string urlstring = "wss://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath + "?auth=" + getValueFromQueryString(authLoginUrl.Query, "c");
                if (loginkey != null) { urlstring += ("&key=" + loginkey); }
                serverurl = new Uri(urlstring);
                meshcentral.connect(serverurl, null, null, null);
            } else {
                // Load two factor cookie if present
                string twoFactorCookie = getRegValue("TwoFactorCookie", null);
                if ((twoFactorCookie != null) && (twoFactorCookie != "")) { twoFactorCookie = "cookie=" + twoFactorCookie; } else { twoFactorCookie = null; }
                int keyIndex = serverNameComboBox.Text.IndexOf("?key=");
                if (keyIndex >= 0)
                {
                    string hostname = serverNameComboBox.Text.Substring(0, keyIndex);
                    string loginkey = serverNameComboBox.Text.Substring(keyIndex + 5);
                    try { serverurl = new Uri("wss://" + hostname + "/control.ashx?key=" + loginkey); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie);
                }
                else
                {
                    try { serverurl = new Uri("wss://" + serverNameComboBox.Text + "/control.ashx"); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie);
                }
            }
        }

        private void Meshcentral_onLoginTokenChanged()
        {
            if (this.InvokeRequired) { this.Invoke(new MeshCentralServer.onLoginTokenChangedHandler(Meshcentral_onLoginTokenChanged)); return; }
            openWebSiteButton.Visible = true;
        }

        private void Meshcentral_onNodesChanged(bool fullRefresh)
        {
            if (this.InvokeRequired) { this.Invoke(new MeshCentralServer.onNodeListChangedHandler(Meshcentral_onNodesChanged), fullRefresh); return; }
            addRelayButton.Enabled = addButton.Enabled = ((meshcentral.nodes != null) && (meshcentral.nodes.Count > 0));

            // Update any active mappings
            foreach (Control c in mapPanel.Controls)
            {
                if (c.GetType() == typeof(MapUserControl))
                {
                    MapUserControl cc = (MapUserControl)c;
                    cc.UpdateInfo();
                }
            }

            updateDeviceList(); // Update list of devices
            addArgMappings();
            reconnectUdpMaps();

            // Setup any automatic mappings
            if ((fullRefresh == true) && (mappingsToSetup != null)) { setupMappings(); }
        }

        private void updateDeviceList()
        {
            string search = searchTextBox.Text.ToLower();
            if (deviceListViewMode)
            {
                devicesListView.SuspendLayout();
                devicesListView.Items.Clear();

                ArrayList controlsToAdd = new ArrayList();
                if (meshcentral.nodes != null)
                {
                    foreach (NodeClass node in meshcentral.nodes.Values)
                    {
                        if (node.agentid == -1) { continue; }
                        ListViewItem device;
                        if (node.listitem == null)
                        {
                            device = new ListViewItem(node.name);
                            device.SubItems.Add(node.getStateString());
                            device.Tag = node;
                            node.listitem = device;
                        } else {
                            device = node.listitem;
                            device.SubItems[0].Text = node.name;
                            device.SubItems[1].Text = node.getStateString();
                        }

                        if ((node.meshid != null) && meshcentral.meshes.ContainsKey(node.meshid)) { node.mesh = (MeshClass)meshcentral.meshes[node.meshid]; }
                        if ((showGroupNamesToolStripMenuItem.Checked) && (node.mesh != null)) {
                            device.SubItems[0].Text = node.mesh.name + " - " + node.name;
                        } else {
                            device.SubItems[0].Text = node.name;
                        }

                        bool connVisible = ((showOfflineDevicesToolStripMenuItem.Checked) || ((node.conn & 1) != 0));
                        int imageIndex = (node.icon - 1) * 2;
                        if ((node.conn & 1) == 0) { imageIndex++; }
                        device.ImageIndex = imageIndex;
                        if (connVisible && ((search == "") || (device.SubItems[0].Text.ToLower().IndexOf(search) >= 0))) { controlsToAdd.Add(device); }
                    }

                    // Add all controls at once to make it fast.
                    if (controlsToAdd.Count > 0) {
                        devicesListView.Items.AddRange((ListViewItem[])controlsToAdd.ToArray(typeof(ListViewItem)));
                    }
                }

                devicesListView.ResumeLayout();
            }
            else
            {
                devicesPanel.SuspendLayout();

                // Untag all devices
                foreach (Control c in devicesPanel.Controls)
                {
                    if (c.GetType() == typeof(DeviceUserControl)) { ((DeviceUserControl)c).present = false; }
                }

                ArrayList controlsToAdd = new ArrayList();
                if (meshcentral.nodes != null)
                {
                    foreach (NodeClass node in meshcentral.nodes.Values)
                    {
                        if (node.agentid == -1) { continue; }
                        if (node.control == null)
                        {
                            // Add a new device
                            DeviceUserControl device = new DeviceUserControl();
                            if ((node.meshid != null) && meshcentral.meshes.ContainsKey(node.meshid)) { device.mesh = (MeshClass)meshcentral.meshes[node.meshid]; }
                            device.node = node;
                            device.parent = this;
                            device.Dock = DockStyle.Top;
                            device.present = true;
                            node.control = device;
                            device.UpdateInfo();
                            device.Visible = (search == "") || (node.name.ToLower().IndexOf(search) >= 0);
                            controlsToAdd.Add(device);
                        }
                        else
                        {
                            // Tag the device as present
                            if (node.control != null)
                            {
                                node.control.present = true;
                                node.control.UpdateInfo();
                            }
                        }
                    }
                }
                // Add all controls at once to make it fast.
                if (controlsToAdd.Count > 0) { devicesPanel.Controls.AddRange((DeviceUserControl[])controlsToAdd.ToArray(typeof(DeviceUserControl))); }

                // Clear all untagged devices
                bool removed;
                do
                {
                    removed = false;
                    foreach (Control c in devicesPanel.Controls)
                    {
                        if ((c.GetType() == typeof(DeviceUserControl)) && ((DeviceUserControl)c).present == false)
                        {
                            devicesPanel.Controls.Remove(c); c.Dispose(); removed = true;
                        }
                    }
                } while (removed == true);

                // Filter devices
                int visibleDevices = 0;
                foreach (Control c in devicesPanel.Controls)
                {
                    if (c.GetType() == typeof(DeviceUserControl))
                    {
                        NodeClass n = ((DeviceUserControl)c).node;
                        bool connVisible = ((showOfflineDevicesToolStripMenuItem.Checked) || ((n.conn & 1) != 0));
                        if ((search == "") || (n.name.ToLower().IndexOf(search) >= 0) || (showGroupNamesToolStripMenuItem.Checked && (((DeviceUserControl)c).mesh.name.ToLower().IndexOf(search) >= 0)))
                        {
                            c.Visible = connVisible;
                            visibleDevices++;
                        }
                        else
                        {
                            c.Visible = false;
                        }
                    }
                }

                // Sort devices
                ArrayList sortlist = new ArrayList();
                foreach (Control c in devicesPanel.Controls) { if (c.GetType() == typeof(DeviceUserControl)) { sortlist.Add(c); } }
                if (sortByNameToolStripMenuItem.Checked)
                {
                    DeviceComparer comp = new DeviceComparer();
                    sortlist.Sort(comp);
                }
                else
                {
                    DeviceGroupComparer comp = new DeviceGroupComparer();
                    sortlist.Sort(comp);
                }
                remoteAllDeviceControls();
                devicesPanel.Controls.AddRange((DeviceUserControl[])sortlist.ToArray(typeof(DeviceUserControl)));

                devicesPanel.ResumeLayout();
                noDevicesLabel.Visible = (sortlist.Count == 0);
                noSearchResultsLabel.Visible = ((sortlist.Count > 0) && (visibleDevices == 0));
            }
        }

        private void remoteAllDeviceControls()
        {
            ArrayList removelist = new ArrayList();
            foreach (Control c in devicesPanel.Controls) { if (c.GetType() == typeof(DeviceUserControl)) { removelist.Add(c); } }
            foreach (Control c in removelist) { devicesPanel.Controls.Remove(c); }
        }

        public bool getShowGroupNames() { return showGroupNamesToolStripMenuItem.Checked; }

        private void Meshcentral_onStateChanged(int state)
        {
            if (meshcentral == null) return;
            if (this.InvokeRequired) { this.Invoke(new MeshCentralServer.onStateChangedHandler(Meshcentral_onStateChanged), state); return; }

            if (state == 0) {
                if (meshcentral.disconnectMsg == "tokenrequired") {
                    emailTokenButton.Visible = (meshcentral.disconnectEmail2FA == true) && (meshcentral.disconnectEmail2FASent == false);
                    tokenEmailSentLabel.Visible = (meshcentral.disconnectEmail2FASent == true) || (meshcentral.disconnectSms2FASent == true);
                    smsTokenButton.Visible = ((meshcentral.disconnectSms2FA == true) && (meshcentral.disconnectSms2FASent == false));
                    if (meshcentral.disconnectEmail2FASent) { tokenEmailSentLabel.Text = Properties.Resources.EmailSent; }
                    if (meshcentral.disconnectSms2FASent) { tokenEmailSentLabel.Text = Properties.Resources.SmsSent; }
                    if ((meshcentral.disconnectEmail2FA == true) && (meshcentral.disconnectEmail2FASent == false)) {
                        smsTokenButton.Left = emailTokenButton.Left + emailTokenButton.Width + 5;
                    } else {
                        smsTokenButton.Left = emailTokenButton.Left;
                    }
                    tokenTextBox.Text = "";
                    if (meshcentral.twoFactorCookieDays > 0) {
                        tokenRememberCheckBox.Visible = true;
                        tokenRememberCheckBox.Text = string.Format(Properties.Resources.DontAskXDays, meshcentral.twoFactorCookieDays);
                    } else {
                        tokenRememberCheckBox.Visible = false;
                    }

                    setPanel(2);
                    tokenTextBox.Focus();
                } else { setPanel(1); }

                if ((meshcentral.disconnectMsg != null) && meshcentral.disconnectMsg.StartsWith("noauth")) {
                    stateLabel.Text = Properties.Resources.InvalidUsernameOrPassword;
                    stateLabel.Visible = true;
                    stateClearTimer.Enabled = true;
                    serverNameComboBox.Focus();
                }
                else if ((meshcentral.disconnectMsg != null) && meshcentral.disconnectMsg.StartsWith("emailvalidationrequired"))
                {
                    stateLabel.Text = Properties.Resources.EmailVerificationRequired;
                    stateLabel.Visible = true;
                    stateClearTimer.Enabled = true;
                    serverNameComboBox.Focus();
                }
                else if (meshcentral.disconnectMsg == "cert") {
                    lastBadConnectCert = meshcentral.disconnectCert;
                    certDetailsTextBox.Text = "---Issuer---\r\n" + lastBadConnectCert.Issuer.Replace(", ", "\r\n") + "\r\n\r\n---Subject---\r\n" + lastBadConnectCert.Subject.Replace(", ", "\r\n");
                    setPanel(3);
                    certDetailsButton.Focus();
                }
                else if (meshcentral.disconnectMsg == null) { stateLabel.Text = Properties.Resources.UnableToConnect; stateLabel.Visible = true; stateClearTimer.Enabled = true; serverNameComboBox.Focus(); }

                // Clean up the UI
                nextButton1.Enabled = true;
                serverNameComboBox.Enabled = true;
                userNameTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
                this.Text = title;

                // Clean up all mappings
                foreach (Control c in mapPanel.Controls) {
                    if (c.GetType() == typeof(MapUserControl)) { ((MapUserControl)c).Dispose(); }
                }
                mapPanel.Controls.Clear();
                noMapLabel.Visible = true;

                // Clean up all devices (old style)
                foreach (Control c in devicesPanel.Controls) {
                    if (c.GetType() == typeof(DeviceUserControl)) { ((DeviceUserControl)c).Dispose(); }
                }
                remoteAllDeviceControls();

                // Clean up all devices (new style)
                devicesListView.Items.Clear();
                noSearchResultsLabel.Visible = false;
                noDevicesLabel.Visible = true;
                if ((meshcentral != null) && (meshcentral.nodes != null))
                {
                    foreach (NodeClass n in meshcentral.nodes.Values) {
                        if (n.desktopViewer != null) { n.desktopViewer.Close(); }
                        if (n.fileViewer != null) { n.fileViewer.Close(); }
                    }
                }

                // Clean up the server
                cookieRefreshTimer.Enabled = false;
                meshcentral.onStateChanged -= Meshcentral_onStateChanged;
                meshcentral.onNodesChanged -= Meshcentral_onNodesChanged;
                meshcentral = null;
                authLoginUrl = null;
            } else if (state == 1) {
                stateLabel.Visible = false;
                //setPanel(1);
                nextButton1.Enabled = false;
                serverNameComboBox.Enabled = false;
                userNameTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
                cookieRefreshTimer.Enabled = false;
            } else if (state == 2) {
                meshcentral.disconnectMsg = "connected";
                stateLabel.Visible = false;
                setPanel(4);
                addButton.Focus();
                if (authLoginUrl == null)
                {
                    setRegValue("ServerName", serverNameComboBox.Text);
                    setRegValue("UserName", userNameTextBox.Text);
                }
                if (meshcentral.username != null) {
                    this.Text = title + " - " + meshcentral.username;
                } else {
                    this.Text = title + " - " + userNameTextBox.Text;
                }
                cookieRefreshTimer.Enabled = true;

                // If we need to remember the 2nd factor, ask for a cookie now.
                if (tokenRememberCheckBox.Checked) { meshcentral.sendCommand("{\"action\":\"twoFactorCookie\"}"); }
            }
        }

        private void reconnectUdpMaps()
        {
            foreach (Control c in mapPanel.Controls)
            {
                if (c == noMapLabel) continue;
                MapUserControl map = (MapUserControl)c;
                if ((map.protocol == 2) && (map.mapper.totalConnectCounter == 0))
                {
                    // This is an unconnected UDP map, check if the target node is connected.
                    foreach (NodeClass n in meshcentral.nodes.Values) {
                        if ((map.node == n) && ((n.conn & 1) != 0))
                        {
                            // Reconnect the UDP map
                            map.Start();
                        }
                    }
                }
            }
        }

        private ArrayList processedArgs = new ArrayList();
        private void addArgMappings()
        {
            // Add mappings
            for (int i = 0; i < this.args.Length; i++)
            {
                if (processedArgs.Contains(i)) { continue; } // This map was already added
                string arg = this.args[i];

                if (arg.Length > 5 && arg.Substring(0, 5).ToLower() == "-map:")
                {
                    string[] x = arg.Substring(5).Split(':');
                    if (x.Length == 5)
                    {
                        // Protocol
                        int protocol = 0;
                        if (x[0].ToLower() == "tcp") { protocol = 1; }
                        if (x[0].ToLower() == "udp") { protocol = 2; }
                        if (protocol == 0) continue;

                        // LocalPort
                        ushort localPort = 0;
                        if (ushort.TryParse(x[1], out localPort) == false) continue;

                        // Node
                        string nodename = x[2];
                        NodeClass node = null;
                        foreach (NodeClass n in meshcentral.nodes.Values) { if (((n.conn & 1) != 0) && (n.name.ToLower() == nodename.ToLower())) { node = n; } }
                        if (node == null) continue;

                        // AppId
                        int appId = 0;
                        if (protocol == 1)
                        {
                            if (x[3].ToLower() == "http") { appId = 1; }
                            else if (x[3].ToLower() == "https") { appId = 2; }
                            else if (x[3].ToLower() == "rdp") { appId = 3; }
                            else if (x[3].ToLower() == "putty") { appId = 4; }
                            else if (x[3].ToLower() == "winscp") { appId = 5; }
                        }

                        // RemotePort
                        ushort remotePort = 0;
                        if (ushort.TryParse(x[4], out remotePort) == false) continue;

                        // Add a new port map
                        MapUserControl map = new MapUserControl();
                        map.xdebug = debug;
                        map.inaddrany = inaddrany;
                        map.protocol = protocol;
                        map.localPort = (int)localPort;
                        map.remotePort = (int)remotePort;
                        map.appId = appId;
                        map.node = node;
                        if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443); } else { map.host = serverNameComboBox.Text; }
                        map.authCookie = meshcentral.authCookie;
                        map.certhash = meshcentral.wshash;
                        map.parent = this;
                        map.Dock = DockStyle.Top;
                        map.Start();

                        mapPanel.Controls.Add(map);
                        noMapLabel.Visible = false;
                        processedArgs.Add(i);
                    }
                }
                else if (arg.Length > 10 && arg.Substring(0, 10).ToLower() == "-relaymap:")
                {
                    string[] x = arg.Substring(10).Split(':');
                    if (x.Length == 6)
                    {
                        // Protocol
                        int protocol = 0;
                        if (x[0].ToLower() == "tcp") { protocol = 1; }
                        if (x[0].ToLower() == "udp") { protocol = 2; }
                        if (protocol == 0) continue;

                        // LocalPort
                        ushort localPort = 0;
                        if (ushort.TryParse(x[1], out localPort) == false) continue;

                        // Node
                        string nodename = x[2];
                        NodeClass node = null;
                        foreach (NodeClass n in meshcentral.nodes.Values) { if (((n.conn & 1) != 0) && (n.name.ToLower() == nodename.ToLower())) { node = n; } }
                        if (node == null) continue;

                        // AppId
                        int appId = 0;
                        if (protocol == 1)
                        {
                            if (x[3].ToLower() == "http") { appId = 1; }
                            else if (x[3].ToLower() == "https") { appId = 2; }
                            else if (x[3].ToLower() == "rdp") { appId = 3; }
                            else if (x[3].ToLower() == "putty") { appId = 4; }
                            else if (x[3].ToLower() == "winscp") { appId = 5; }
                        }

                        // Remote host
                        IPAddress remoteIp;
                        if (IPAddress.TryParse(x[4], out remoteIp) == false) continue;

                        // RemotePort
                        ushort remotePort = 0;
                        if (ushort.TryParse(x[5], out remotePort) == false) continue;

                        // Add a new port map
                        MapUserControl map = new MapUserControl();
                        map.xdebug = debug;
                        map.inaddrany = inaddrany;
                        map.protocol = protocol;
                        map.localPort = (int)localPort;
                        map.remoteIP = remoteIp.ToString();
                        map.remotePort = (int)remotePort;
                        map.appId = appId;
                        map.node = node;
                        if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443); } else { map.host = serverNameComboBox.Text; }
                        map.authCookie = meshcentral.authCookie;
                        map.certhash = meshcentral.wshash;
                        map.parent = this;
                        map.Dock = DockStyle.Top;
                        map.Start();

                        mapPanel.Controls.Add(map);
                        noMapLabel.Visible = false;
                        processedArgs.Add(i);
                    }
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddPortMapForm form = new AddPortMapForm(meshcentral);

            if (sender == null)
            {
                if (devicesListView.SelectedItems.Count != 1) { return; }
                ListViewItem selecteditem = devicesListView.SelectedItems[0];
                NodeClass node = (NodeClass)selecteditem.Tag;
                if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
                form.setNode(node);
            }

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // Add a new port map
                MapUserControl map = new MapUserControl();
                map.xdebug = debug;
                map.inaddrany = inaddrany;
                map.name = form.getName();
                map.protocol = form.getProtocol();
                map.localPort = form.getLocalPort();
                map.remotePort = form.getRemotePort();
                map.appId = form.getAppId();
                map.node = form.getNode();
                if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443); } else { map.host = serverNameComboBox.Text; }
                map.authCookie = meshcentral.authCookie;
                map.certhash = meshcentral.wshash;
                map.parent = this;
                map.Dock = DockStyle.Top;
                map.Start();

                mapPanel.Controls.Add(map);
                noMapLabel.Visible = false;
            }
        }

        public void removeMap(MapUserControl map)
        {
            mapPanel.Controls.Remove(map);
            noMapLabel.Visible = (mapPanel.Controls.Count <= 1);
        }

        private void backButton2_Click(object sender, EventArgs e)
        {
            setPanel(1);
        }

        private void nextButton2_Click(object sender, EventArgs e)
        {
            if ((tokenTextBox.Text.Replace(" ", "") == "") && (sendEmailToken == false) && (sendSMSToken == false)) return;

            // Attempt to login with token
            addButton.Enabled = false;
            addRelayButton.Enabled = false;
            openWebSiteButton.Visible = false;

            Uri serverurl = null;
            if (authLoginUrl != null) {
                string loginkey = getValueFromQueryString(authLoginUrl.Query, "key");
                string urlstring = "wss://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath + "?auth=" + getValueFromQueryString(authLoginUrl.Query, "c");
                if (loginkey != null) { urlstring += ("&key=" + loginkey); }
                serverurl = new Uri(urlstring);
            } else {
                int keyIndex = serverNameComboBox.Text.IndexOf("?key=");
                if (keyIndex >= 0)
                {
                    string hostname = serverNameComboBox.Text.Substring(0, keyIndex);
                    string loginkey = serverNameComboBox.Text.Substring(keyIndex + 5);
                    serverurl = new Uri("wss://" + hostname + "/control.ashx?key=" + loginkey);
                }
                else
                {
                    serverurl = new Uri("wss://" + serverNameComboBox.Text + "/control.ashx");
                }
            }

            meshcentral = new MeshCentralServer();
            meshcentral.debug = debug;
            meshcentral.tlsdump = tlsdump;
            meshcentral.ignoreCert = ignoreCert;
            if (lastBadConnectCert != null) {
                meshcentral.okCertHash = lastBadConnectCert.GetCertHashString();
            } else {
                string ignoreCert = getRegValue("IgnoreCert", null);
                if (ignoreCert != null) { meshcentral.okCertHash = ignoreCert; }
            }
            meshcentral.onStateChanged += Meshcentral_onStateChanged;
            meshcentral.onNodesChanged += Meshcentral_onNodesChanged;
            meshcentral.onLoginTokenChanged += Meshcentral_onLoginTokenChanged;
            meshcentral.onClipboardData += Meshcentral_onClipboardData;
            meshcentral.onTwoFactorCookie += Meshcentral_onTwoFactorCookie;
            if (sendEmailToken == true)
            {
                sendEmailToken = false;
                meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, "**email**");
            }
            else if (sendSMSToken == true)
            {
                sendSMSToken = false;
                meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, "**sms**");
            }
            else
            {
                meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, tokenTextBox.Text.Replace(" ", ""));
            }
        }

        private void tokenTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { nextButton2_Click(this, null); e.Handled = true; }
        }

        private void serverNameComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { userNameTextBox.Focus(); e.Handled = true; }
        }

        private void userNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { passwordTextBox.Focus(); e.Handled = true; }
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) { e.Handled = true; if (nextButton1.Enabled) { nextButton1_Click(this, null); } }
        }

        private void stateClearTimer_Tick(object sender, EventArgs e)
        {
            stateLabel.Visible = false;
            stateClearTimer.Enabled = false;
        }

        private void licenseLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.apache.org/licenses/LICENSE-2.0.html");
        }

        private void backButton3_Click(object sender, EventArgs e)
        {
            setPanel(1);
        }

        private void certDetailsButton_Click(object sender, EventArgs e)
        {
            X509Certificate2UI.DisplayCertificate(lastBadConnectCert);
        }

        private void addRelayMapButton_Click(object sender, EventArgs e)
        {
            AddRelayMapForm form = new AddRelayMapForm(meshcentral);

            if (sender == null)
            {
                if (devicesListView.SelectedItems.Count != 1) { return; }
                ListViewItem selecteditem = devicesListView.SelectedItems[0];
                NodeClass node = (NodeClass)selecteditem.Tag;
                if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
                form.setNode(node);
            }

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // Add a new port map
                MapUserControl map = new MapUserControl();
                map.xdebug = debug;
                map.inaddrany = inaddrany;
                map.name = form.getName();
                map.protocol = form.getProtocol();
                map.localPort = form.getLocalPort();
                map.remotePort = form.getRemotePort();
                map.remoteIP = form.getRemoteIP();
                map.appId = form.getAppId();
                map.node = form.getNode();
                if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443); } else { map.host = serverNameComboBox.Text; }
                map.authCookie = meshcentral.authCookie;
                map.certhash = meshcentral.wshash;
                map.parent = this;
                map.Dock = DockStyle.Top;
                map.Start();

                mapPanel.Controls.Add(map);
                noMapLabel.Visible = false;
            }
        }

        private void helpPictureBox_Click(object sender, EventArgs e)
        {
            new MappingHelpForm().ShowDialog(this);
        }

        private void openWebSiteButton_Click(object sender, EventArgs e)
        {
            if (meshcentral.loginCookie != null) {
                Uri serverurl = null;
                if (authLoginUrl != null) {
                    serverurl = new Uri("https://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath + "?login=" + meshcentral.loginCookie);
                } else {
                    serverurl = new Uri("https://" + serverNameComboBox.Text + "?login=" + meshcentral.loginCookie);
                }
                System.Diagnostics.Process.Start(serverurl.ToString());
            }
        }

        private void cookieRefreshTimer_Tick(object sender, EventArgs e)
        {
            meshcentral.refreshCookies();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.Visible == false) { this.Visible = true; } else { this.Visible = false; this.Focus(); }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            forceExit = true;
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.Focus();
        }

        private void settingsPictureBox_Click(object sender, EventArgs e)
        {
            SettingsForm f = new SettingsForm();
            f.BindAllInterfaces = inaddrany;
            f.ShowSystemTray = (notifyIcon.Visible == true);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                inaddrany = f.BindAllInterfaces;
                if (f.ShowSystemTray) {
                    notifyIcon.Visible = true;
                    this.ShowInTaskbar = false;
                    this.MinimizeBox = false;
                } else {
                    notifyIcon.Visible = false;
                    this.ShowInTaskbar = true;
                    this.MinimizeBox = true;
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (deviceListViewMode) {
                // Filter devices
                updateDeviceList();
            } else {
                // Filter devices
                int visibleDevices = 0;
                int deviceCount = 0;
                string search = searchTextBox.Text.ToLower();
                foreach (Control c in devicesPanel.Controls)
                {
                    if (c.GetType() == typeof(DeviceUserControl))
                    {
                        deviceCount++;
                        NodeClass n = ((DeviceUserControl)c).node;
                        bool connVisible = ((showOfflineDevicesToolStripMenuItem.Checked) || ((n.conn & 1) != 0));
                        if ((search == "") || (n.name.ToLower().IndexOf(search) >= 0) || (showGroupNamesToolStripMenuItem.Checked && (((DeviceUserControl)c).mesh.name.ToLower().IndexOf(search) >= 0)))
                        {
                            //if ((search == "") || (n.name.ToLower().IndexOf(search) >= 0)) {
                            c.Visible = connVisible;
                            visibleDevices++;
                        }
                        else
                        {
                            c.Visible = false;
                        }
                    }
                }

                noDevicesLabel.Visible = (deviceCount == 0);
                noSearchResultsLabel.Visible = ((deviceCount > 0) && (visibleDevices == 0));
            }
        }

        private void devicesTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchTextBox.Visible = (devicesTabControl.SelectedIndex == 0);
            if (devicesTabControl.SelectedIndex == 0) {
                menuLabel.ContextMenuStrip = mainContextMenuStrip;
            } else {
                menuLabel.ContextMenuStrip = mappingsContextMenuStrip;
            }
        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27) { searchTextBox.Text = ""; e.Handled = true; }
        }

        public void QuickMap(int protocol, int port, int appId, NodeClass node)
        {
            // See if we already have the right port mapping
            foreach (Control c in mapPanel.Controls)
            {
                if (c.GetType() == typeof(MapUserControl))
                {
                    MapUserControl cc = (MapUserControl)c;
                    if ((cc.protocol == protocol) && (cc.remotePort == port) && (cc.appId == appId) && (cc.node == node))
                    {
                        // Found a match
                        cc.appButton_Click(this, null);
                        return;
                    }
                }
            }

            // Add a new port map
            MapUserControl map = new MapUserControl();
            map.xdebug = debug;
            map.inaddrany = false; // Loopback only
            map.protocol = protocol; // 1 = TCP, 2 = UDP
            map.localPort = 0; // Any
            map.remotePort = port; // HTTP
            map.appId = appId; // 0 = Custom, 1 = HTTP, 2 = HTTPS, 3 = RDP, 4 = PuTTY, 5 = WinSCP
            map.node = node;
            if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443); } else { map.host = serverNameComboBox.Text; }
            map.authCookie = meshcentral.authCookie;
            map.certhash = meshcentral.wshash;
            map.parent = this;
            map.Dock = DockStyle.Top;
            map.Start();
            mapPanel.Controls.Add(map);
            noMapLabel.Visible = false;
            map.appButton_Click(this, null);
        }

        private void emailTokenButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Properties.Resources.SendTokenEmail, Properties.Resources.TwoFactorAuthentication, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sendEmailToken = true;
                sendSMSToken = false;
                nextButton2_Click(this, null);
            }
        }

        private void smsTokenButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Properties.Resources.SendTokenSMS, Properties.Resources.TwoFactorAuthentication, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sendEmailToken = false;
                sendSMSToken = true;
                nextButton2_Click(this, null);
            }
        }

        private void tokenTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            nextButton2.Enabled = (tokenTextBox.Text.Replace(" ","") != "");
        }

        private void tokenTextBox_TextChanged(object sender, EventArgs e)
        {
            nextButton2.Enabled = (tokenTextBox.Text.Replace(" ", "") != "");
        }

        private void menuLabel_Click(object sender, EventArgs e)
        {
            if (devicesTabControl.SelectedIndex == 0)
            {
                mainContextMenuStrip.Show(menuLabel, menuLabel.PointToClient(Cursor.Position));
            }
            else
            {
                mappingsContextMenuStrip.Show(menuLabel, menuLabel.PointToClient(Cursor.Position));
            }
        }

        private void showGroupNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGroupNamesToolStripMenuItem.Checked = !showGroupNamesToolStripMenuItem.Checked;
            setRegValue("Show Group Names", showGroupNamesToolStripMenuItem.Checked ? "1" : "0");
            updateDeviceList();
        }

        private void hideOfflineDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showOfflineDevicesToolStripMenuItem.Checked = !showOfflineDevicesToolStripMenuItem.Checked;
            setRegValue("Show Offline Devices", showOfflineDevicesToolStripMenuItem.Checked?"1":"0");
            updateDeviceList();
        }

        private void sortByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortByNameToolStripMenuItem.Checked = true;
            sortByGroupToolStripMenuItem.Checked = false;
            setRegValue("Device Sort", "Name");
            updateDeviceList();
        }

        private void sortByGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortByNameToolStripMenuItem.Checked = false;
            sortByGroupToolStripMenuItem.Checked = true;
            setRegValue("Device Sort", "Group");
            updateDeviceList();
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            InstallForm form = new InstallForm();
            if (form.ShowDialog(this) == DialogResult.OK) { hookRouterEx(); }
        }

        private void ChangeLanguage(string lang)
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture == new System.Globalization.CultureInfo(lang)) return;
            if (MessageBox.Show(this, Properties.Resources.LanguagesChanging, Properties.Resources.LanguagesTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                Close();
            }
        }

        private void openMappingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openMapFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try { loadMappingFile(File.ReadAllText(openMapFileDialog.FileName), 2); } catch (Exception) { }
            }
        }

        private int loadMappingFile(string data, int mode)
        {
            int argFlags = 3;
            Dictionary<string, object> jsonAction = new Dictionary<string, object>();
            jsonAction = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(data);
            if ((jsonAction == null) || (jsonAction["hostname"].GetType() != typeof(string)) || (jsonAction["username"].GetType() != typeof(string)) || (jsonAction["certhash"].GetType() != typeof(string))) return 0;
            if (mode == 1)
            {
                serverNameComboBox.Text = jsonAction["hostname"].ToString();
                userNameTextBox.Text = jsonAction["username"].ToString();
                if (jsonAction.ContainsKey("password")) { passwordTextBox.Text = jsonAction["password"].ToString(); argFlags |= 4; }
                acceptableCertHash = jsonAction["certhash"].ToString();
            }
            if (jsonAction["mappings"] != null)
            {
                ArrayList mappings = (ArrayList)jsonAction["mappings"];
                if (mappings.Count > 0) {
                    mappingsToSetup = mappings;
                    if (mode == 2) { setupMappings(); }
                }
            }
            return argFlags;
        }

        private void setupMappings()
        {
            foreach (Dictionary<string, object> x in mappingsToSetup)
            {
                // Find the node
                string nodeId = (string)x["nodeId"];
                NodeClass node = null;
                try { node = meshcentral.nodes[nodeId]; } catch (Exception) { }
                if (node == null) continue;

                // Add a new port map
                MapUserControl map = new MapUserControl();
                map.xdebug = debug;
                map.inaddrany = inaddrany;
                if (x.ContainsKey("name")) { map.name = x["name"].ToString(); } else { map.name = ""; }
                map.protocol = (int)x["protocol"];
                map.localPort = (int)x["localPort"];
                if (x.ContainsKey("remoteIP")) { map.remoteIP = (string)x["remoteIP"]; }
                map.remotePort = (int)x["remotePort"];
                map.appId = (int)x["appId"];
                if (x.ContainsKey("autoExit")) { map.autoexit = (bool)x["autoExit"]; }
                map.node = node;
                if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443); } else { map.host = serverNameComboBox.Text; }
                map.authCookie = meshcentral.authCookie;
                map.certhash = meshcentral.wshash;
                map.parent = this;
                map.Dock = DockStyle.Top;
                map.Start();

                mapPanel.Controls.Add(map);
                noMapLabel.Visible = false;

                // Launch any executable
                if (x.ContainsKey("launch")) {
                    if (x["launch"].GetType() == typeof(int)) { map.appButton_Click(this, null); }
                    if (x["launch"].GetType() == typeof(string))
                    {
                        try
                        {
                            string lanuchString = (string)x["launch"];
                            lanuchString = lanuchString.Replace("{port}", x["localPort"].ToString());
                            System.Diagnostics.Process.Start(lanuchString);
                        }
                        catch (Exception) { }
                    }
                }
            }
            mappingsToSetup = null;
        }

        private void saveMappingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveMapFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string text = "{\r\n  \"hostname\": \"" + serverNameComboBox.Text + "\",\r\n  \"username\": \"" + userNameTextBox.Text + "\",\r\n  \"certhash\": \"" + meshcentral.certHash + "\",\r\n  \"mappings\":[\r\n";
                var mapCounter = 0;
                foreach (Control c in mapPanel.Controls)
                {
                    if (c.GetType() != typeof(MapUserControl)) continue;
                    MapUserControl mapCtrl = (MapUserControl)c;
                    MeshMapper map = ((MapUserControl)c).mapper;
                    if (mapCounter == 0) { text += "    {\r\n"; } else { text += ",\r\n    {\r\n"; }
                    text += "      \"nodeName\": \"" + mapCtrl.node.name + "\",\r\n";
                    if ((mapCtrl.name != null) && (mapCtrl.name != "")) { text += "      \"name\": \"" + mapCtrl.name + "\",\r\n"; }
                    text += "      \"meshId\": \"" + mapCtrl.node.meshid + "\",\r\n";
                    text += "      \"nodeId\": \"" + mapCtrl.node.nodeid + "\",\r\n";
                    text += "      \"appId\": " + mapCtrl.appId + ",\r\n";
                    text += "      \"protocol\": " + map.protocol + ",\r\n";
                    text += "      \"localPort\": " + map.localport + ",\r\n";
                    if (map.remoteip != null) { text += "      \"remoteIP\": \"" + map.remoteip + "\",\r\n"; }
                    text += "      \"remotePort\": " + map.remoteport + "\r\n";
                    text += "    }";
                    mapCounter++;
                }
                if (mapCounter > 0) { text += "\r\n  ]\r\n}"; } else { text += "  ]\r\n}"; }
                File.WriteAllText(saveMapFileDialog.FileName, text);
            }
        }

        private void mapPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == false) return;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if ((s.Length != 1) || (s[0].ToLower().EndsWith(".mcrouter") == false)) return;
            e.Effect = DragDropEffects.All;
        }

        private void mapPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == false) return;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if ((s.Length != 1) || (s[0].ToLower().EndsWith(".mcrouter") == false)) return;
            try { loadMappingFile(File.ReadAllText(s[0]), 2); } catch (Exception) { }
        }

        private void devicesContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { e.Cancel = true; return; } // Device not selected
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { e.Cancel = true; return; } // Agent not connected on this device
            if (node.agentid < 6)
            {
                // Windows OS
                sshToolStripMenuItem.Visible = false;
                scpToolStripMenuItem.Visible = false;
                rdpToolStripMenuItem.Visible = true;
            }
            else
            {
                // Other OS
                sshToolStripMenuItem.Visible = true;
                scpToolStripMenuItem.Visible = true;
                rdpToolStripMenuItem.Visible = false;
            }
            remoteDesktopToolStripMenuItem.Visible = ((node.agentcaps & 1) != 0); // Only display remote desktop if it's supported by the agent
        }

        private void httpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            QuickMap(1, 80, 1, node); // HTTP
        }

        private void httpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            QuickMap(1, 443, 2, node); // HTTPS
        }

        private void rdpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            int rdpport = 3389;
            if (node.rdpport != 0) { rdpport = node.rdpport; }
            QuickMap(1, rdpport, 3, node); // RDP
        }

        private void sshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            QuickMap(1, 22, 4, node); // Putty
        }

        private void scpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            QuickMap(1, 22, 5, node); // WinSCP
        }

        private void addMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            addButton_Click(null, null);
        }

        private void addRelayMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            addRelayMapButton_Click(null, null);
        }

        private void devicesListView_DoubleClick(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            addButton_Click(null, null);
        }

        private void remoteDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            if (node.desktopViewer == null)
            {
                node.desktopViewer = new KVMViewer(meshcentral, node);
                node.desktopViewer.Show();
                node.desktopViewer.MenuItemConnect_Click(null, null);
            } else {
                node.desktopViewer.Focus();
            }
        }

        private void remoteFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            if (node.fileViewer == null)
            {
                node.fileViewer = new FileViewer(meshcentral, node);
                node.fileViewer.Show();
                node.fileViewer.MenuItemConnect_Click(null, null);
            }
            else
            {
                node.fileViewer.Focus();
            }
        }

        /*
        private delegate void displayMessageHandler(string msg, int buttons, string extra, int progress);
        private void displayMessage(string msg, int buttons = 0, string extra = "", int progress = 0)
        {
            if (this.InvokeRequired) { this.Invoke(new displayMessageHandler(displayMessage), msg, buttons, extra, progress); return; }
            if (msg != null) { statusLabel.Text = msg; loadingLabel.Text = msg; }
            statusLabel2.Text = extra;
            label4.Text = extra;
            nextButton3.Enabled = (buttons == 1);
            backButton3.Enabled = (buttons == 2);
            mainProgressBar.Visible = (progress > 0);
            if (progress >= 0) { mainProgressBar.Value = progress; }
            if (buttons == 3) { setPanel(6); }
            linkLabel1.Visible = (progress == -1);
            advConfigButton.Visible = (progress == -1);
        }
        */

    }
}
