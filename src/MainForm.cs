﻿/*
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
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace MeshCentralRouter
{
    public partial class MainForm : Form
    {
        private int initialHeight;
        private int argflags;
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
        public bool sendMsgToken = false;
        public bool allowUpdates = Settings.GetRegValue("CheckForUpdates", true);
        public bool collapseDeviceGroup = Settings.GetRegValue("CollapseDeviceGroups", true);
        public Uri authLoginUrl = null;
        public Process installProcess = null;
        public string acceptableCertHash = null;
        public ArrayList mappingsToSetup = null;
        public bool deviceListViewMode = true;
        public Process autoExitProc = null;
        public int deviceDoubleClickAction = 0;
        public FileInfo nativeSshPath = null;
        public LocalPipeServer localPipeServer = null;
        private IntPtr nextClipboardViewer = IntPtr.Zero;

        public delegate void ClipboardChangedHandler();
        public event ClipboardChangedHandler ClipboardChanged;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    if (ClipboardChanged != null) { ClipboardChanged(); }
                    Win32Api.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer) { nextClipboardViewer = m.LParam; } else { Win32Api.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam); }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

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

        private void unHookRouterEx()
        {
            if (IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName, "-uninstall");
                startInfo.Verb = "runas";
                try { installProcess = System.Diagnostics.Process.Start(startInfo); } catch (Exception) { return; }
                installTimer.Enabled = true;
            }
            else
            {
                unHookRouter();
            }
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
            }
            else
            {
                hookRouter();
            }
        }

        private void installTimer_Tick(object sender, EventArgs e)
        {
            if ((installProcess == null) || (installProcess.HasExited == true))
            {
                installTimer.Enabled = false;
                installButton.Visible = true;
                if (isRouterHooked())
                {
                    installButton.Text = "Uninstall";
                }
                else
                {
                    installButton.Text = "Install";
                }
            }
        }

        private bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

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

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (meshcentral.ignoreCert) return true;
            if (meshcentral.connectionState < 2)
            {
                // Normal certificate check
                if (chain.Build(new X509Certificate2(certificate)) == true) { meshcentral.certHash = webSocketClient.GetMeshKeyHash(certificate); return true; }
                if ((meshcentral.okCertHash != null) && ((meshcentral.okCertHash == certificate.GetCertHashString()) || (meshcentral.okCertHash == webSocketClient.GetMeshKeyHash(certificate)) || (meshcentral.okCertHash == webSocketClient.GetMeshCertHash(certificate)))) { meshcentral.certHash = webSocketClient.GetMeshKeyHash(certificate); return true; }
                if ((meshcentral.okCertHash2 != null) && ((meshcentral.okCertHash2 == certificate.GetCertHashString()) || (meshcentral.okCertHash2 == webSocketClient.GetMeshKeyHash(certificate)) || (meshcentral.okCertHash2 == webSocketClient.GetMeshCertHash(certificate)))) { meshcentral.certHash = webSocketClient.GetMeshKeyHash(certificate); return true; }
                meshcentral.certHash = null;
                meshcentral.disconnectMsg = "cert";
                meshcentral.disconnectCert = new X509Certificate2(certificate);
            }
            else
            {
                if ((meshcentral.certHash != null) && ((meshcentral.certHash == certificate.GetCertHashString()) || (meshcentral.certHash == webSocketClient.GetMeshKeyHash(certificate)) || (meshcentral.certHash == webSocketClient.GetMeshCertHash(certificate)))) { return true; }
            }
            return false;
        }

        public MainForm(string[] args)
        {
            // Set TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteCertificateValidationCallback);

            this.args = args;
            InitializeComponent();
            Translate.TranslateControl(this);
            Translate.TranslateListView(devicesListView);
            Translate.TranslateContextMenu(trayIconContextMenuStrip);
            Translate.TranslateContextMenu(mainContextMenuStrip);
            Translate.TranslateContextMenu(mappingsContextMenuStrip);
            Translate.TranslateContextMenu(devicesContextMenuStrip);
            mainPanel.Controls.Add(panel1);
            mainPanel.Controls.Add(panel2);
            mainPanel.Controls.Add(panel3);
            mainPanel.Controls.Add(panel4);
            pictureBox1.SendToBack();
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            versionLabel.Text = "v" + version.Major + "." + version.Minor + "." + version.Build;

            // Prevent edgecase where the hook priority can be on and the hook disabled causing havoc
            if (!Settings.GetRegValue("Exp_KeyboardHook", false)) { Settings.SetRegValue("Exp_KeyboardHookPriority", false); }

            serverNameComboBox.Text = Settings.GetRegValue("ServerName", "");
            userNameTextBox.Text = Settings.GetRegValue("UserName", "");
            notifyIcon.Visible = Settings.GetRegValue("NotifyIcon", false);

            title = this.Text;
            initialHeight = this.Height;

            argflags = 0;
            string update = null;
            string delete = null;
            foreach (string arg in this.args)
            {
                if (arg.ToLower() == "-oldstyle") { deviceListViewMode = false; }
                if (arg.ToLower() == "-install") { hookRouter(); forceExit = true; return; }
                if (arg.ToLower() == "-uninstall") { unHookRouter(); forceExit = true; return; }
                if (arg.ToLower() == "-noupdate") { allowUpdates = false; return; }
                if (arg.ToLower() == "-debug") { debug = true; }
                if (arg.ToLower() == "-tlsdump") { tlsdump = true; }
                if (arg.ToLower() == "-ignorecert") { ignoreCert = true; }
                if (arg.ToLower() == "-all") { inaddrany = true; }
                if (arg.ToLower() == "-inaddrany") { inaddrany = true; }
                if (arg.ToLower() == "-tray") { notifyIcon.Visible = true; }
                if (arg.ToLower() == "-native") { webSocketClient.nativeWebSocketFirst = true; }
                if (arg.Length > 6 && arg.Substring(0, 6).ToLower() == "-host:") { serverNameComboBox.Text = arg.Substring(6); argflags |= 1; }
                if (arg.Length > 6 && arg.Substring(0, 6).ToLower() == "-user:") { userNameTextBox.Text = arg.Substring(6); argflags |= 2; }
                if (arg.Length > 6 && arg.Substring(0, 6).ToLower() == "-pass:") { passwordTextBox.Text = arg.Substring(6); argflags |= 4; }
                if (arg.Length > 8 && arg.Substring(0, 8).ToLower() == "-search:") { searchTextBox.Text = arg.Substring(8); }
                if (arg.Length > 8 && arg.Substring(0, 8).ToLower() == "-update:") { update = arg.Substring(8); }
                if (arg.Length > 8 && arg.Substring(0, 8).ToLower() == "-delete:") { delete = arg.Substring(8); }
                if (arg.Length > 11 && arg.Substring(0, 11).ToLower() == "mcrouter://") { authLoginUrl = new Uri(arg); }
                if ((arg.Length > 1) && (arg[0] != '-') && (arg.ToLower().EndsWith(".mcrouter"))) { try { argflags |= loadMappingFile(File.ReadAllText(arg), 1); } catch (Exception) { } }
                if (arg.ToLower() == "-localfiles") { FileViewer fileViewer = new FileViewer(meshcentral, null); fileViewer.Show(); }
            }
            autoLogin = (argflags == 7);
            this.MinimizeBox = !notifyIcon.Visible;
            this.MinimumSize = new Size(this.Width, initialHeight);
            this.MaximumSize = new Size(this.Width, 1080);
            this.MaximizeBox = false;
            this.ResizeEnd += MainForm_ResizeEnd;
            this.devicesListView.Dock = DockStyle.Fill;

            if (update != null)
            {
                // New args
                ArrayList args2 = new ArrayList();
                foreach (string a in args) { if (a.StartsWith("-update:") == false) { args2.Add(a); } }

                // Remove ".update.exe" and copy
                System.Threading.Thread.Sleep(1000);
                File.Copy(Assembly.GetEntryAssembly().Location, update, true);
                System.Threading.Thread.Sleep(1000);
                Process.Start(update, string.Join(" ", (string[])args2.ToArray(typeof(string))) + " -delete:" + Assembly.GetEntryAssembly().Location);
                this.forceExit = true;
                Application.Exit();
                return;
            }

            if (delete != null) { try { System.Threading.Thread.Sleep(1000); File.Delete(delete); } catch (Exception) { } }

            // Set automatic port map values
            if (authLoginUrl != null)
            {
                // Check if we are locked to a server
                if ((Program.LockToHostname != null) && (Program.LockToHostname != authLoginUrl.Host))
                {
                    forceExit = true;
                    MessageBox.Show(string.Format(Properties.Resources.SignedExecutableServerLockError, Program.LockToHostname), Properties.Resources.MeshCentralRouter, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }

                string autoName = null;
                string autoNodeId = null;
                string autoRemoteIp = null;
                int autoRemotePort = 0;
                int autoLocalPort = 0;
                int autoProtocol = 0;
                int autoAppId = 0;
                string autoAppIdStr = null;
                bool autoExit = false;
                try
                {
                    // Automatic mappings
                    autoName = getValueFromQueryString(authLoginUrl.Query, "name");
                    autoNodeId = getValueFromQueryString(authLoginUrl.Query, "nodeid");
                    autoRemoteIp = getValueFromQueryString(authLoginUrl.Query, "remoteip");
                    int.TryParse(getValueFromQueryString(authLoginUrl.Query, "remoteport"), out autoRemotePort);
                    int.TryParse(getValueFromQueryString(authLoginUrl.Query, "protocol"), out autoProtocol);
                    autoExit = (getValueFromQueryString(authLoginUrl.Query, "autoexit") == "1");
                    string localPortStr = getValueFromQueryString(authLoginUrl.Query, "localport");
                    if (localPortStr != null) { autoLocalPort = int.Parse(localPortStr); }
                    autoAppIdStr = getValueFromQueryString(authLoginUrl.Query, "appid");
                    int.TryParse(autoAppIdStr, out autoAppId);
                }
                catch (Exception) { }
                if (((autoRemotePort != 0) && (autoProtocol != 0) && (autoNodeId != null)) || ((autoNodeId != null) && ((autoAppId == 6) || (autoAppId == 7))))
                {
                    Dictionary<string, object> map = new Dictionary<string, object>();
                    if (autoName != null) { map.Add("name", HttpUtility.UrlDecode(autoName)); }
                    map.Add("nodeId", autoNodeId);
                    if (autoRemoteIp != null) { map.Add("remoteIP", autoRemoteIp); }
                    map.Add("remotePort", autoRemotePort);
                    map.Add("localPort", autoLocalPort);
                    map.Add("protocol", autoProtocol);
                    map.Add("appId", autoAppId);
                    map.Add("appIdStr", autoAppIdStr);
                    map.Add("autoExit", autoExit);
                    map.Add("launch", 1);
                    mappingsToSetup = new ArrayList();
                    mappingsToSetup.Add(map);
                    if ((autoAppId != 6) && (autoAppId != 7)) { devicesTabControl.SelectedIndex = 1; }
                }
            }

            // Check MeshCentral .mcrouter hook
            installButton.Visible = true;
            if (isRouterHooked())
            {
                installButton.Text = "Uninstall";
            }
            else
            {
                installButton.Text = "Install";
            }


            // Right click action
            deviceDoubleClickAction = int.Parse(Settings.GetRegValue("DevDoubleClickClickAction", "0"));
            setDoubleClickDeviceAction();

            // Load customizations
            bool showLicense = true;
            FileInfo selfExe = new FileInfo(Assembly.GetExecutingAssembly().Location);
            if (File.Exists(Path.Combine(selfExe.Directory.FullName, @"customization\topbanner.png"))) { try { pictureBox1.Image = (Bitmap)Image.FromFile(Path.Combine(selfExe.Directory.FullName, @"customization\topbanner.png")); showLicense = false; } catch (Exception) { } }
            if (File.Exists(Path.Combine(selfExe.Directory.FullName, @"customization\logo.png"))) { try { pictureBox2.Image = pictureBox6.Image = (Bitmap)Image.FromFile(Path.Combine(selfExe.Directory.FullName, @"customization\logo.png")); showLicense = false; } catch (Exception) { } }
            if (File.Exists(Path.Combine(selfExe.Directory.FullName, @"customization\bottombanner.png"))) { try { pictureBox3.Image = pictureBox4.Image = pictureBox5.Image = pictureBox7.Image = (Bitmap)Image.FromFile(Path.Combine(selfExe.Directory.FullName, @"customization\bottombanner.png")); showLicense = false; } catch (Exception) { } }
            licenseLinkLabel.Visible = showLicense;
            connectionSettings.Visible = true;
            try
            {
                if (File.Exists(Path.Combine(selfExe.Directory.FullName, @"customization\customize.txt")))
                {
                    string[] lines = File.ReadAllLines(Path.Combine(selfExe.Directory.FullName, @"customization\customize.txt"));
                    if (lines[0] != "") { this.Text = lines[0]; }
                    if (lines[1] != "") { label1.Text = lines[1]; }
                }
            }
            catch (Exception) { }

            // Check if Windows SSH is present
            FileInfo nativeSshPath = new FileInfo(Path.Combine(Environment.SystemDirectory, "OpenSSH\\ssh.exe"));
            if (nativeSshPath.Exists) { this.nativeSshPath = nativeSshPath; }

            // Listen to clipboard events
            nextClipboardViewer = Win32Api.SetClipboardViewer(this.Handle);
        }

        private void setDoubleClickDeviceAction()
        {
            addMapToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 0) ? FontStyle.Bold : FontStyle.Regular);
            addRelayMapToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 1) ? FontStyle.Bold : FontStyle.Regular);
            remoteDesktopToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 2) ? FontStyle.Bold : FontStyle.Regular);
            remoteFilesToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 3) ? FontStyle.Bold : FontStyle.Regular);
            httpToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 4) ? FontStyle.Bold : FontStyle.Regular);
            httpsToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 5) ? FontStyle.Bold : FontStyle.Regular);
            rdpToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 6) ? FontStyle.Bold : FontStyle.Regular);
            sshToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 7) ? FontStyle.Bold : FontStyle.Regular);
            scpToolStripMenuItem.Font = new Font("Segoe UI", 9, (deviceDoubleClickAction == 8) ? FontStyle.Bold : FontStyle.Regular);

            askConsentBarToolStripMenuItem.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            askConsentToolStripMenuItem.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            privacyBarToolStripMenuItem.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        private void setPanel(int newPanel)
        {
            if (currentPanel == newPanel) return;
            if (newPanel == 4)
            {
                this.Height = Settings.GetRegValue("WindowHeight", this.Height);
                this.FormBorderStyle = FormBorderStyle.Sizable;

                updatePanel4();
            }
            else
            {
                if (currentPanel == 4 && (argflags & 4) != 4)
                    passwordTextBox.Text = "";

                this.Height = initialHeight;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }

            panel1.Visible = (newPanel == 1);
            panel2.Visible = (newPanel == 2);
            panel3.Visible = (newPanel == 3);
            panel4.Visible = (newPanel == 4);
            currentPanel = newPanel;

            // Setup stuff
            if (newPanel == 1) { tokenRememberCheckBox.Checked = false; }
            nextButton2.Enabled = (tokenTextBox.Text.Replace(" ", "") != "");
            if (currentPanel == 4) { devicesTabControl.Focus(); }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load registry settings
            showGroupNamesToolStripMenuItem.Checked = (Settings.GetRegValue("Show Group Names", "1") == "1");
            showOfflineDevicesToolStripMenuItem.Checked = (Settings.GetRegValue("Show Offline Devices", "1") == "1");
            if (Settings.GetRegValue("Device Sort", "Name") == "Name")
            {
                sortByNameToolStripMenuItem.Checked = true;
                sortByGroupToolStripMenuItem.Checked = false;
            }
            else
            {
                sortByNameToolStripMenuItem.Checked = false;
                sortByGroupToolStripMenuItem.Checked = true;
            }

            //Text += " - v" + Application.ProductVersion;
            //installPathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Open Source", "MeshCentral");
            //serverModeComboBox.SelectedIndex = 0;
            //windowColor = serverNameTextBox.BackColor;
            setPanel(1);
            updatePanel1(null, null);
            SendMessage(searchTextBox.Handle, EM_SETCUEBANNER, 0, Translate.T(Properties.Resources.SearchPlaceHolder));

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
            string locationStr = Settings.GetRegValue("location", "");
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

            // Set the focus
            if (userNameTextBox.Text != "") { passwordTextBox.Select(); }
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
            forceExit = !notifyIcon.Visible;
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
// ignore close-to-tray on debug-builds
#if !DEBUG
            if ((notifyIcon.Visible == true) && (currentPanel == 4) && (forceExit == false)) { e.Cancel = true; Visible = false; }
#endif
            Settings.SetRegValue("Location", Location.X + "," + Location.Y);
        }

        private void backButton5_Click(object sender, EventArgs e)
        {
            cancelAutoClose();
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
            // Check if we are locked to a server
            if (Program.LockToHostname != null)
            {
                bool ok = true;
                if (authLoginUrl != null)
                {
                    ok = (Program.LockToHostname == authLoginUrl.Host);
                }
                else
                {
                    string host = serverNameComboBox.Text;
                    int i = host.IndexOf("?key=");
                    if (i >= 0) { host = host.Substring(0, i); }
                    i = host.IndexOf(":");
                    if (i >= 0) { host = host.Substring(0, i); }
                    ok = (Program.LockToHostname == host);
                }
                if (ok == false)
                {
                    MessageBox.Show(string.Format(Properties.Resources.SignedExecutableServerLockError, Program.LockToHostname), Properties.Resources.MeshCentralRouter, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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
            if (allowUpdates) { meshcentral.onToolUpdate += Meshcentral_onToolUpdate; }
            if (lastBadConnectCert != null)
            {
                meshcentral.okCertHash = lastBadConnectCert.GetCertHashString();
            }
            else
            {
                string ignoreCert = Settings.GetRegValue("IgnoreCert", null);
                if (ignoreCert != null) { meshcentral.okCertHash = ignoreCert; }
            }

            // Load two factor cookie if present
            string twoFactorCookie = Settings.GetRegValue("TwoFactorCookie", null);
            if ((twoFactorCookie != null) && (twoFactorCookie != "")) { twoFactorCookie = "cookie=" + twoFactorCookie; } else { twoFactorCookie = null; }

            Uri serverurl = null;
            if (authLoginUrl != null)
            {
                try
                {
                    meshcentral.okCertHash2 = getValueFromQueryString(authLoginUrl.Query, "t");
                    string loginkey = getValueFromQueryString(authLoginUrl.Query, "key");
                    string urlstring = "wss://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath + "?auth=" + getValueFromQueryString(authLoginUrl.Query, "c");
                    if (loginkey != null) { urlstring += ("&key=" + loginkey); }
                    serverurl = new Uri(urlstring);
                }
                catch (Exception) { }
                meshcentral.connect(serverurl, null, null, null, getClientAuthCertificate());
            }
            else
            {
                int keyIndex = serverNameComboBox.Text.IndexOf("?key=");
                if (keyIndex >= 0)
                {
                    string hostname = serverNameComboBox.Text.Substring(0, keyIndex);
                    string loginkey = serverNameComboBox.Text.Substring(keyIndex + 5);
                    try { serverurl = new Uri("wss://" + hostname + "/control.ashx?key=" + loginkey); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie, getClientAuthCertificate());
                }
                else
                {
                    try { serverurl = new Uri("wss://" + serverNameComboBox.Text + "/control.ashx"); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie, getClientAuthCertificate());
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
            if (debug) { try { File.AppendAllText("debug.log", "Saving 2FA cookie\r\n"); } catch (Exception) { } }
            Settings.SetRegValue("TwoFactorCookie", cookie);
        }

        private void nextButton3_Click(object sender, EventArgs e)
        {
            // If we need to remember this certificate
            if (rememberCertCheckBox.Checked) { Settings.SetRegValue("IgnoreCert", lastBadConnectCert.GetCertHashString()); }

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
            if (allowUpdates) { meshcentral.onToolUpdate += Meshcentral_onToolUpdate; }
            meshcentral.okCertHash = lastBadConnectCert.GetCertHashString();

            Uri serverurl = null;
            if (authLoginUrl != null)
            {
                meshcentral.okCertHash2 = getValueFromQueryString(authLoginUrl.Query, "t");
                string loginkey = getValueFromQueryString(authLoginUrl.Query, "key");
                string urlstring = "wss://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath + "?auth=" + getValueFromQueryString(authLoginUrl.Query, "c");
                if (loginkey != null) { urlstring += ("&key=" + loginkey); }
                serverurl = new Uri(urlstring);
                meshcentral.connect(serverurl, null, null, null, getClientAuthCertificate());
            }
            else
            {
                // Load two factor cookie if present
                string twoFactorCookie = Settings.GetRegValue("TwoFactorCookie", null);
                if ((twoFactorCookie != null) && (twoFactorCookie != "")) { twoFactorCookie = "cookie=" + twoFactorCookie; } else { twoFactorCookie = null; }
                int keyIndex = serverNameComboBox.Text.IndexOf("?key=");
                if (keyIndex >= 0)
                {
                    string hostname = serverNameComboBox.Text.Substring(0, keyIndex);
                    string loginkey = serverNameComboBox.Text.Substring(keyIndex + 5);
                    try { serverurl = new Uri("wss://" + hostname + "/control.ashx?key=" + loginkey); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie, getClientAuthCertificate());
                }
                else
                {
                    try { serverurl = new Uri("wss://" + serverNameComboBox.Text + "/control.ashx"); } catch (Exception) { }
                    meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, twoFactorCookie, getClientAuthCertificate());
                }
            }
        }

        private void Meshcentral_onToolUpdate(string url, string hash, int size, string serverhash)
        {
            if (this.InvokeRequired) { this.Invoke(new MeshCentralServer.toolUpdateHandler(Meshcentral_onToolUpdate), url, hash, size, serverhash); return; }
            UpdateForm f = new UpdateForm(url, hash, size, args, serverhash);
            forceExit = true;
            if (f.ShowDialog(this) != DialogResult.OK) { forceExit = !notifyIcon.Visible; }
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

            updateDeviceList(false); // Update list of devices
            addArgMappings();
            reconnectUdpMaps();

            // Setup any automatic mappings
            if ((fullRefresh == true) && (mappingsToSetup != null)) { setupMappings(); }

            // Reconnect any auto-reconnect nodes
            foreach (NodeClass node in meshcentral.nodes.Values)
            {
                if (((node.conn & 1) != 0) && (node.desktopViewer != null)) { node.desktopViewer.TryAutoConnect(); }
            }
        }

        class GroupComparer : IComparer
        {
            public int Compare(object objA, object objB)
            {
                return ((ListViewGroup)objA).Header.CompareTo(((ListViewGroup)objB).Header);
            }
        }

        private void updateDeviceList(bool forceGroupChanged)
        {
            string search = searchTextBox.Text.ToLower();
            if (deviceListViewMode)
            {
                devicesListView.SuspendLayout();
                devicesListView.Items.Clear();

                bool bGroupChanged = forceGroupChanged;
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
                        }
                        else
                        {
                            device = node.listitem;
                            device.SubItems[0].Text = node.name;
                            device.SubItems[1].Text = node.getStateString();
                        }

                        // Fetch the device group name
                        if ((node.meshid != null) && meshcentral.meshes.ContainsKey(node.meshid)) { node.mesh = (MeshClass)meshcentral.meshes[node.meshid]; }
                        string meshName = (node.mesh != null) ? node.mesh.name : Properties.Resources.IndividualDevices;

                        // Set the device name
                        if ((showGroupNamesToolStripMenuItem.Checked) && (node.mesh != null)) {
                            device.SubItems[0].Text = meshName + " - " + node.name;
                        } else {
                            device.SubItems[0].Text = node.name;
                        }

                        // See if this device can be added to an existing group
                        bool bGroupExisting = false;
                        for (int i = 0; i < devicesListView.Groups.Count; i++)
                        {
                            // if ((node.mesh != null && devicesListView.Groups[i].Header == node.mesh.name) || (node.mesh == null && devicesListView.Groups[i].Header == ""))
                            if (((string)devicesListView.Groups[i].Tag) == node.meshid)
                            {
                                bGroupExisting = true;
                                node.listitem.Group = devicesListView.Groups[i];
                                devicesListView.Groups[i].Header = meshName; // Set this again just in case the device group name changed
                                break;
                            }
                        }

                        // If a device group does not exist, create it
                        if (!bGroupExisting)
                        {
                            ListViewGroup grp = devicesListView.Groups.Add(devicesListView.Groups.Count.ToString(), meshName);
                            grp.Tag = node.meshid;
                            node.listitem.Group = grp;
                            bGroupChanged = true;
                        }

                        bool connVisible = ((showOfflineDevicesToolStripMenuItem.Checked) || ((node.conn & 1) != 0)) || (node.mtype == 3);
                        int imageIndex = (node.icon - 1) * 2;
                        if (((node.conn & 1) == 0) && (node.mtype != 3)) { imageIndex++; }
                        device.ImageIndex = imageIndex;

                        string userSearch = null;
                        string lowerSearch = search.ToLower();
                        if (lowerSearch.StartsWith("u:")) { userSearch = lowerSearch.Substring(2); }
                        if (lowerSearch.StartsWith("user:")) { userSearch = lowerSearch.Substring(5); }
                        if (userSearch == null)
                        {
                            // Normal search
                            if (connVisible && ((search == "") || (device.SubItems[0].Text.ToLower().IndexOf(search) >= 0))) { controlsToAdd.Add(device); }
                        }
                        else
                        {
                            // User search
                            bool userSearchMatch = false;
                            if (node.users != null) { foreach (string user in node.users) { if (user.ToLower().IndexOf(userSearch) >= 0) { userSearchMatch = true; } } }
                            if (connVisible && ((userSearch == "") || userSearchMatch)) { controlsToAdd.Add(device); }
                        }
                    }

                    // If groups have changes update them
                    if (bGroupChanged)
                    {
                        ListViewGroup[] groups = new ListViewGroup[this.devicesListView.Groups.Count];
                        this.devicesListView.Groups.CopyTo(groups, 0);
                        Array.Sort(groups, new GroupComparer());

                        this.devicesListView.BeginUpdate();
                        this.devicesListView.Groups.Clear();
                        this.devicesListView.Groups.AddRange(groups);
                        this.devicesListView.EndUpdate();

                        foreach (ListViewGroup lvg in devicesListView.Groups)
                        {
                            if (collapseDeviceGroup)
                            {
                                ListViewExtended.setGrpState(lvg, ListViewGroupState.Collapsible | ListViewGroupState.Collapsed);
                            }
                            else
                            {
                                ListViewExtended.setGrpState(lvg, ListViewGroupState.Collapsible | ListViewGroupState.Normal);
                            }
                        }
                    }

                    // Add all controls at once to make it fast.
                    if (controlsToAdd.Count > 0)
                    {
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

            if (state == 0)
            {
                if (meshcentral.disconnectMsg == "tokenrequired")
                {
                    emailTokenButton.Visible = (meshcentral.disconnectEmail2FA == true) && (meshcentral.disconnectEmail2FASent == false);
                    tokenEmailSentLabel.Visible = (meshcentral.disconnectEmail2FASent == true) || (meshcentral.disconnectSms2FASent == true) || (meshcentral.disconnectMsg2FASent == true);
                    smsTokenButton.Visible = ((meshcentral.disconnectSms2FA == true) && (meshcentral.disconnectSms2FASent == false));
                    msgTokenButton.Visible = ((meshcentral.disconnectMsg2FA == true) && (meshcentral.disconnectMsg2FASent == false));
                    if (meshcentral.disconnectEmail2FASent) { tokenEmailSentLabel.Text = Translate.T(Properties.Resources.EmailSent); }
                    if (meshcentral.disconnectSms2FASent) { tokenEmailSentLabel.Text = Translate.T(Properties.Resources.SmsSent); }
                    if (meshcentral.disconnectMsg2FASent) { tokenEmailSentLabel.Text = Translate.T(Properties.Resources.MessageSent); }
                    if ((meshcentral.disconnectEmail2FA == true) && (meshcentral.disconnectEmail2FASent == false))
                    {
                        smsTokenButton.Left = emailTokenButton.Left + emailTokenButton.Width + 5;
                    }
                    else
                    {
                        smsTokenButton.Left = emailTokenButton.Left;
                    }
                    tokenTextBox.Text = "";
                    if (meshcentral.twoFactorCookieDays > 0)
                    {
                        tokenRememberCheckBox.Visible = true;
                        tokenRememberCheckBox.Text = string.Format(Translate.T(Properties.Resources.DontAskXDays), meshcentral.twoFactorCookieDays);
                    }
                    else
                    {
                        tokenRememberCheckBox.Visible = false;
                    }

                    setPanel(2);
                    tokenTextBox.Focus();
                }
                else { setPanel(1); }

                if ((meshcentral.disconnectMsg != null) && meshcentral.disconnectMsg.StartsWith("noauth"))
                {
                    stateLabel.Text = Translate.T(Properties.Resources.InvalidUsernameOrPassword);
                    stateLabel.Visible = true;
                    stateClearTimer.Enabled = true;
                    serverNameComboBox.Focus();
                }
                else if ((meshcentral.disconnectMsg != null) && meshcentral.disconnectMsg.StartsWith("notools"))
                {
                    stateLabel.Text = Translate.T(Properties.Resources.NoToolsAllowed);
                    stateLabel.Visible = true;
                    stateClearTimer.Enabled = true;
                    serverNameComboBox.Focus();
                }
                else if ((meshcentral.disconnectMsg != null) && meshcentral.disconnectMsg.StartsWith("emailvalidationrequired"))
                {
                    stateLabel.Text = Translate.T(Properties.Resources.EmailVerificationRequired);
                    stateLabel.Visible = true;
                    stateClearTimer.Enabled = true;
                    serverNameComboBox.Focus();
                }
                else if (meshcentral.disconnectMsg == "cert")
                {
                    lastBadConnectCert = meshcentral.disconnectCert;
                    certDetailsTextBox.Text = "---Issuer---\r\n" + lastBadConnectCert.Issuer.Replace(", ", "\r\n") + "\r\n\r\n---Subject---\r\n" + lastBadConnectCert.Subject.Replace(", ", "\r\n");
                    setPanel(3);
                    certDetailsButton.Focus();
                }
                else if (meshcentral.disconnectMsg == null) { stateLabel.Text = Translate.T(Properties.Resources.UnableToConnect); stateLabel.Visible = true; stateClearTimer.Enabled = true; serverNameComboBox.Focus(); }

                // Clean up the UI
                nextButton1.Enabled = true;
                serverNameComboBox.Enabled = true;
                userNameTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
                this.Text = title;

                // Clean up all mappings
                foreach (Control c in mapPanel.Controls)
                {
                    if (c.GetType() == typeof(MapUserControl)) { ((MapUserControl)c).Dispose(); }
                }
                mapPanel.Controls.Clear();
                noMapLabel.Visible = true;

                // Clean up all devices (old style)
                foreach (Control c in devicesPanel.Controls)
                {
                    if (c.GetType() == typeof(DeviceUserControl)) { ((DeviceUserControl)c).Dispose(); }
                }
                remoteAllDeviceControls();

                // Clean up all devices (new style)
                devicesListView.Items.Clear();
                noSearchResultsLabel.Visible = false;
                noDevicesLabel.Visible = true;
                if ((meshcentral != null) && (meshcentral.nodes != null))
                {
                    foreach (NodeClass n in meshcentral.nodes.Values)
                    {
                        if (n.desktopViewer != null) { n.desktopViewer.Close(); }
                        if (n.fileViewer != null) { n.fileViewer.Close(); }
                    }
                }

                // Clean up single instance pipe server
                if (localPipeServer != null) { localPipeServer.Dispose(); localPipeServer = null; }

                // Clean up the server
                cookieRefreshTimer.Enabled = false;
                meshcentral.onStateChanged -= Meshcentral_onStateChanged;
                meshcentral.onNodesChanged -= Meshcentral_onNodesChanged;
                meshcentral = null;
                authLoginUrl = null;
            }
            else if (state == 1)
            {
                stateLabel.Visible = false;
                //setPanel(1);
                nextButton1.Enabled = false;
                serverNameComboBox.Enabled = false;
                userNameTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
                cookieRefreshTimer.Enabled = false;
            }
            else if (state == 2)
            {
                meshcentral.disconnectMsg = "connected";
                stateLabel.Visible = false;
                setPanel(4);
                addButton.Focus();
                if (authLoginUrl == null)
                {
                    Settings.SetRegValue("ServerName", serverNameComboBox.Text);
                    Settings.SetRegValue("UserName", userNameTextBox.Text);
                }
                if (meshcentral.username != null)
                {
                    this.Text = title + " - " + meshcentral.username;
                }
                else
                {
                    this.Text = title + " - " + userNameTextBox.Text;
                }
                cookieRefreshTimer.Enabled = true;

                // If we need to remember the 2nd factor, ask for a cookie now.
                if (debug) { try { File.AppendAllText("debug.log", "Requesting 2FA cookie in 2 and half seconds time\r\n"); } catch (Exception) { } }
                if (tokenRememberCheckBox.Checked) { Task.Delay(2500).ContinueWith((task) => { meshcentral.sendCommand("{\"action\":\"twoFactorCookie\"}"); }); }

                // Setup single instance pipe server
                if (authLoginUrl != null)
                {
                    string urlstring = "wss://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath;
                    localPipeServer = new LocalPipeServer(Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(urlstring))); //  + "" + meshcentral.certHash
                    localPipeServer.onArgs += LocalPipeServer_onArgs;
                }
            }
        }

        private delegate void LocalPipeServerOnArgsHandler(string args);

        private void LocalPipeServer_onArgs(string args)
        {
            if (args.StartsWith("mcrouter://") == false) return;
            if (this.InvokeRequired) { this.Invoke(new LocalPipeServerOnArgsHandler(LocalPipeServer_onArgs), args); return; }

            Uri authLoginUrl2 = new Uri(args);

            // Check if we are locked to a server
            if ((Program.LockToHostname != null) && (Program.LockToHostname != authLoginUrl2.Host))
            {
                MessageBox.Show(string.Format(Properties.Resources.SignedExecutableServerLockError, Program.LockToHostname), Properties.Resources.MeshCentralRouter, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Set automatic port map values
            if (authLoginUrl2 != null)
            {
                string autoName = null;
                string autoNodeId = null;
                string autoRemoteIp = null;
                int autoRemotePort = 0;
                int autoLocalPort = 0;
                int autoProtocol = 0;
                int autoAppId = 0;
                string autoAppIdStr = null;
                bool autoExit = false;
                try
                {
                    // Automatic mappings
                    autoName = getValueFromQueryString(authLoginUrl.Query, "name");
                    autoNodeId = getValueFromQueryString(authLoginUrl2.Query, "nodeid");
                    autoRemoteIp = getValueFromQueryString(authLoginUrl2.Query, "remoteip");
                    int.TryParse(getValueFromQueryString(authLoginUrl2.Query, "remoteport"), out autoRemotePort);
                    int.TryParse(getValueFromQueryString(authLoginUrl2.Query, "protocol"), out autoProtocol);
                    autoAppIdStr = getValueFromQueryString(authLoginUrl2.Query, "appid");
                    int.TryParse(autoAppIdStr, out autoAppId);
                    autoExit = (getValueFromQueryString(authLoginUrl2.Query, "autoexit") == "1");
                    string localPortStr = getValueFromQueryString(authLoginUrl.Query, "localport");
                    if (localPortStr != null) { autoLocalPort = int.Parse(localPortStr); }
                }
                catch (Exception) { }
                if (((autoRemotePort != 0) && (autoProtocol != 0) && (autoNodeId != null)) || ((autoNodeId != null) && ((autoAppId == 6) || (autoAppId == 7))))
                {
                    Dictionary<string, object> map = new Dictionary<string, object>();
                    if (autoName != null) { map.Add("name", HttpUtility.UrlDecode(autoName)); }
                    map.Add("nodeId", autoNodeId);
                    if (autoRemoteIp != null) { map.Add("remoteIP", autoRemoteIp); }
                    map.Add("remotePort", autoRemotePort);
                    map.Add("localPort", autoLocalPort);
                    map.Add("protocol", autoProtocol);
                    map.Add("appId", autoAppId);
                    map.Add("appIdStr", autoAppIdStr);
                    map.Add("autoExit", autoExit);
                    map.Add("launch", 1);
                    mappingsToSetup = new ArrayList();
                    mappingsToSetup.Add(map);
                    if ((autoAppId != 6) && (autoAppId != 7)) { devicesTabControl.SelectedIndex = 1; }
                    setupMappings();
                    cancelAutoClose();
                }
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
                    foreach (NodeClass n in meshcentral.nodes.Values)
                    {
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
                        if (appId == 0) { map.appIdStr = x[3].ToLower(); }
                        map.node = node;
                        if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.AbsolutePath.Replace("/control.ashx", ""); } else { map.host = serverNameComboBox.Text; }
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
                        if (appId == 0) { map.appIdStr = x[3].ToLower(); }
                        map.node = node;
                        if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.AbsolutePath.Replace("/control.ashx", ""); } else { map.host = serverNameComboBox.Text; }
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
            cancelAutoClose();
            AddPortMapForm form = new AddPortMapForm(meshcentral);

            if (sender == null)
            {
                if (devicesListView.SelectedItems.Count != 1) { return; }
                ListViewItem selecteditem = devicesListView.SelectedItems[0];
                NodeClass node = (NodeClass)selecteditem.Tag;
                if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
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
                map.appIdStr = form.getAppIdStr();
                map.node = form.getNode();
                if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.AbsolutePath.Replace("/control.ashx", ""); } else { map.host = serverNameComboBox.Text; }
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
            if ((tokenTextBox.Text.Replace(" ", "") == "") && (sendEmailToken == false) && (sendSMSToken == false) && (sendMsgToken == false)) return;

            // Attempt to login with token
            addButton.Enabled = false;
            addRelayButton.Enabled = false;
            openWebSiteButton.Visible = false;

            Uri serverurl = null;
            if (authLoginUrl != null)
            {
                string loginkey = getValueFromQueryString(authLoginUrl.Query, "key");
                string urlstring = "wss://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.LocalPath + "?auth=" + getValueFromQueryString(authLoginUrl.Query, "c");
                if (loginkey != null) { urlstring += ("&key=" + loginkey); }
                serverurl = new Uri(urlstring);
            }
            else
            {
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
            if (lastBadConnectCert != null)
            {
                meshcentral.okCertHash = lastBadConnectCert.GetCertHashString();
            }
            else
            {
                string ignoreCert = Settings.GetRegValue("IgnoreCert", null);
                if (ignoreCert != null) { meshcentral.okCertHash = ignoreCert; }
            }
            meshcentral.onStateChanged += Meshcentral_onStateChanged;
            meshcentral.onNodesChanged += Meshcentral_onNodesChanged;
            meshcentral.onLoginTokenChanged += Meshcentral_onLoginTokenChanged;
            meshcentral.onClipboardData += Meshcentral_onClipboardData;
            meshcentral.onTwoFactorCookie += Meshcentral_onTwoFactorCookie;
            if (allowUpdates) { meshcentral.onToolUpdate += Meshcentral_onToolUpdate; }
            if (sendEmailToken == true)
            {
                sendEmailToken = false;
                meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, "**email**", getClientAuthCertificate());
            }
            else if (sendSMSToken == true)
            {
                sendSMSToken = false;
                meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, "**sms**", getClientAuthCertificate());
            }
            else if (sendMsgToken == true)
            {
                sendMsgToken = false;
                meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, "**msg**", getClientAuthCertificate());
            }
            else
            {
                meshcentral.connect(serverurl, userNameTextBox.Text, passwordTextBox.Text, tokenTextBox.Text.Replace(" ", ""), getClientAuthCertificate());
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

        private void cancelAutoClose()
        {
            autoExitProc = null;
            cancelAutoCloseButton1.Visible = false;
            cancelAutoCloseButton2.Visible = false;
        }

        private void addRelayMapButton_Click(object sender, EventArgs e)
        {
            cancelAutoClose();
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
                map.appIdStr = form.getAppIdStr();
                map.node = form.getNode();
                if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.AbsolutePath.Replace("/control.ashx", ""); } else { map.host = serverNameComboBox.Text; }
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
            cancelAutoClose();
            new MappingHelpForm().ShowDialog(this);
        }

        private void openWebSiteButton_Click(object sender, EventArgs e)
        {
            cancelAutoClose();
            if (meshcentral.loginCookie != null)
            {
                Uri serverurl = null;
                if (authLoginUrl != null)
                {
                    string localPath = authLoginUrl.LocalPath;
                    if (localPath.EndsWith("/control.ashx")) { localPath = localPath.Substring(0, localPath.Length - 12); }
                    serverurl = new Uri("https://" + authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + localPath + "?login=" + meshcentral.loginCookie);
                }
                else
                {
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
            if (this.Visible == false)
            {
                this.WindowState = FormWindowState.Normal;
                this.Visible = true;
                SetForegroundWindow(this.Handle.ToInt32());
                this.Focus();
            } else { this.Visible = false; }
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
            SetForegroundWindow(this.Handle.ToInt32());
            this.Focus();
        }

        private void settingsPictureBox_Click(object sender, EventArgs e)
        {
            cancelAutoClose();
            SettingsForm f = new SettingsForm();
            f.BindAllInterfaces = inaddrany;
            f.ShowSystemTray = (notifyIcon.Visible == true);
            f.CheckForUpdates = Settings.GetRegValue("CheckForUpdates", true);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                inaddrany = f.BindAllInterfaces;
                Settings.SetRegValue("CheckForUpdates", f.CheckForUpdates);
                allowUpdates = f.CheckForUpdates;

                if (f.ShowSystemTray)
                {
                    notifyIcon.Visible = true;
                    this.MinimizeBox = false;
                }
                else
                {
                    notifyIcon.Visible = false;
                    this.MinimizeBox = true;
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (deviceListViewMode)
            {
                // Filter devices
                updateDeviceList(false);
            }
            else
            {
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
            cancelAutoClose();
            searchTextBox.Visible = (devicesTabControl.SelectedIndex == 0);
            if (devicesTabControl.SelectedIndex == 0)
            {
                menuLabel.ContextMenuStrip = mainContextMenuStrip;
            }
            else
            {
                menuLabel.ContextMenuStrip = mappingsContextMenuStrip;
            }
        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            cancelAutoClose();
            if (e.KeyChar == 27) { searchTextBox.Text = ""; e.Handled = true; }
        }

        public void QuickMap(int protocol, int port, int appId, NodeClass node)
        {
            NodeClass tmpNode = node;
            if (node.mesh.relayid != null)
            {
                if (!meshcentral.nodes.ContainsKey(node.mesh.relayid))
                    return;
                
                tmpNode = meshcentral.nodes[node.mesh.relayid];
            }

            // See if we already have the right port mapping
            foreach (Control c in mapPanel.Controls)
            {
                if (c.GetType() == typeof(MapUserControl))
                {
                    MapUserControl cc = (MapUserControl)c;
                    if ((cc.remoteIP == node.host) && (cc.protocol == protocol) && (cc.remotePort == port) && (cc.appId == appId) && (cc.node == tmpNode))
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
            if (node.mesh.relayid != null)
            {
                map.name = node.name;
                map.remoteIP = node.host;
            }

            map.localPort = 0; // Any
            map.remotePort = port; // HTTP
            map.appId = appId; // 0 = Custom, 1 = HTTP, 2 = HTTPS, 3 = RDP, 4 = PuTTY, 5 = WinSCP
            map.appIdStr = null;
            map.node = tmpNode;
            if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.AbsolutePath.Replace("/control.ashx", ""); } else { map.host = serverNameComboBox.Text; }
            map.certhash = meshcentral.wshash;
            map.parent = this;
            map.Dock = DockStyle.Top;
            map.Start();
            mapPanel.Controls.Add(map);
            noMapLabel.Visible = false;
            map.appButton_Click(this, null);
        }

        public void QuickMap(int protocol, int port, string appIdStr, NodeClass node)
        {
            // See if we already have the right port mapping
            foreach (Control c in mapPanel.Controls)
            {
                if (c.GetType() == typeof(MapUserControl))
                {
                    MapUserControl cc = (MapUserControl)c;
                    if ((cc.protocol == protocol) && (cc.remotePort == port) && (cc.appIdStr == appIdStr) && (cc.node == node))
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
            map.appId = 0; // 0 = Custom, 1 = HTTP, 2 = HTTPS, 3 = RDP, 4 = PuTTY, 5 = WinSCP
            map.appIdStr = appIdStr;
            map.node = node;
            if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.AbsolutePath.Replace("/control.ashx", ""); } else { map.host = serverNameComboBox.Text; }
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
            if (MessageBox.Show(this, Translate.T(Properties.Resources.SendTokenEmail), Translate.T(Properties.Resources.TwoFactorAuthentication), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sendEmailToken = true;
                sendSMSToken = false;
                sendMsgToken = false;
                nextButton2_Click(this, null);
            }
        }

        private void smsTokenButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Translate.T(Properties.Resources.SendTokenSMS), Translate.T(Properties.Resources.TwoFactorAuthentication), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sendEmailToken = false;
                sendSMSToken = true;
                sendMsgToken = false;
                nextButton2_Click(this, null);
            }
        }

        private void msgTokenButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Translate.T(Properties.Resources.SendTokenMSG), Translate.T(Properties.Resources.TwoFactorAuthentication), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sendEmailToken = false;
                sendSMSToken = false;
                sendMsgToken = true;
                nextButton2_Click(this, null);
            }
        }

        private void tokenTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            nextButton2.Enabled = (tokenTextBox.Text.Replace(" ", "") != "");
        }

        private void tokenTextBox_TextChanged(object sender, EventArgs e)
        {
            nextButton2.Enabled = (tokenTextBox.Text.Replace(" ", "") != "");
        }

        private void menuLabel_Click(object sender, EventArgs e)
        {
            cancelAutoClose();
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
            Settings.SetRegValue("Show Group Names", showGroupNamesToolStripMenuItem.Checked ? "1" : "0");
            updateDeviceList(false);
        }

        private void hideOfflineDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showOfflineDevicesToolStripMenuItem.Checked = !showOfflineDevicesToolStripMenuItem.Checked;
            Settings.SetRegValue("Show Offline Devices", showOfflineDevicesToolStripMenuItem.Checked ? "1" : "0");
            updateDeviceList(false);
        }

        private void sortByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortByNameToolStripMenuItem.Checked = true;
            sortByGroupToolStripMenuItem.Checked = false;
            Settings.SetRegValue("Device Sort", "Name");
            updateDeviceList(false);
        }

        private void sortByGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortByNameToolStripMenuItem.Checked = false;
            sortByGroupToolStripMenuItem.Checked = true;
            Settings.SetRegValue("Device Sort", "Group");
            updateDeviceList(false);
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            if (isRouterHooked())
            {
                UninstallForm form = new UninstallForm();
                if (form.ShowDialog(this) == DialogResult.OK) { unHookRouterEx(); }
            } else
            {
                InstallForm form = new InstallForm();
                if (form.ShowDialog(this) == DialogResult.OK) { hookRouterEx(); }
            }
        }

        private void ChangeLanguage(string lang)
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture == new System.Globalization.CultureInfo(lang)) return;
            if (MessageBox.Show(this, Translate.T(Properties.Resources.LanguagesChanging), Translate.T(Properties.Resources.LanguagesTitle), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            if ((jsonAction == null) || (jsonAction["hostname"].GetType() != typeof(string)) || (jsonAction["username"].GetType() != typeof(string))) return 0;
            if (mode == 1)
            {
                serverNameComboBox.Text = jsonAction["hostname"].ToString();
                userNameTextBox.Text = jsonAction["username"].ToString();
                if (jsonAction.ContainsKey("password")) { passwordTextBox.Text = jsonAction["password"].ToString(); argFlags |= 4; }
                if (jsonAction.ContainsKey("certhash")) { acceptableCertHash = jsonAction["certhash"].ToString(); }
            }
            if (jsonAction["mappings"] != null)
            {
                ArrayList mappings = (ArrayList)jsonAction["mappings"];
                if (mappings.Count > 0)
                {
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
                int appId = (int)x["appId"];

                if (appId == 6)
                {
                    // MeshCentral Router Desktop
                    if ((node.conn & 1) == 0) return; // Agent not connected on this device
                    startNewDesktopViewer(node, 0);
                }
                else if (appId == 7)
                {
                    // MeshCentral Router Files
                    if ((node.conn & 1) == 0) return; // Agent not connected on this device
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
                else
                {
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
                    if (x.ContainsKey("appIdStr")) { map.appIdStr = (string)x["appIdStr"]; }
                    if (x.ContainsKey("autoExit")) { map.autoexit = (bool)x["autoExit"]; }
                    map.node = node;
                    if (authLoginUrl != null) { map.host = authLoginUrl.Host + ":" + ((authLoginUrl.Port > 0) ? authLoginUrl.Port : 443) + authLoginUrl.AbsolutePath.Replace("/control.ashx", ""); } else { map.host = serverNameComboBox.Text; }
                    map.certhash = meshcentral.wshash;
                    map.parent = this;
                    map.Dock = DockStyle.Top;
                    map.Start();

                    mapPanel.Controls.Add(map);
                    noMapLabel.Visible = false;

                    // Launch any executable
                    if (x.ContainsKey("launch"))
                    {
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
                    if (mapCtrl.appIdStr != null) { text += "      \"appIdStr\": \"" + mapCtrl.appIdStr + "\",\r\n"; }
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
            cancelAutoClose();
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == false) return;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if ((s.Length != 1) || (s[0].ToLower().EndsWith(".mcrouter") == false)) return;
            e.Effect = DragDropEffects.All;
        }

        private void mapPanel_DragDrop(object sender, DragEventArgs e)
        {
            cancelAutoClose();
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
            wolToolStripMenuItem.Visible = false;
            if (((node.conn & 1) == 0) && (node.mtype != 3))
            { // Agent not connected on this device and not local device
                addMapToolStripMenuItem.Visible = false;
                addRelayMapToolStripMenuItem.Visible = false;
                remoteDesktopToolStripMenuItem.Visible = false;
                remoteFilesToolStripMenuItem.Visible = false;
                httpToolStripMenuItem.Visible = false;
                httpsToolStripMenuItem.Visible = false;
                rdpToolStripMenuItem.Visible = false;
                sshToolStripMenuItem.Visible = false;
                scpToolStripMenuItem.Visible = false;
                wolToolStripMenuItem.Visible = true; // Wol not allowed for local devices
            }
            else{  // Agent connected or local device
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
                addMapToolStripMenuItem.Visible = true;
                httpToolStripMenuItem.Visible = true;
                httpsToolStripMenuItem.Visible = true;
                addRelayMapToolStripMenuItem.Visible = (node.mtype != 3); // Relay mappings are not allowed for local devices
                remoteDesktopToolStripMenuItem.Visible = ((node.agentcaps & 1) != 0); // Only display remote desktop if it's supported by the agent (1 = Desktop)
                remoteFilesToolStripMenuItem.Visible = ((node.agentcaps & 4) != 0); // Only display remote desktop if it's supported by the agent (4 = Files)
            }
            
        }

        private void httpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
            QuickMap(1, 80, 1, node); // HTTP
        }

        private void httpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
            QuickMap(1, 443, 2, node); // HTTPS
        }

        private void rdpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
            int rdpport = 3389;
            if (node.rdpport != 0) { rdpport = node.rdpport; }
            QuickMap(1, rdpport, 3, node); // RDP
        }

        private void sshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
            QuickMap(1, 22, 4, node); // Putty
        }

        private void scpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
            QuickMap(1, 22, 5, node); // WinSCP
        }
        
        private void wolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) != 0) || (node.mtype == 3)) { return; } // Agent connected on this device or local device
            // List of actions : https://github.com/Ylianst/MeshCentral/blob/f5db131693386147731f2ec93b9378bf035b5861/agents/meshcore.js#L1110
            //                   https://github.com/Ylianst/MeshCentral/blob/f5db131693386147731f2ec93b9378bf035b5861/amtmanager.js#L347
            //                   https://github.com/Ylianst/MeshCentral/blob/f5db131693386147731f2ec93b9378bf035b5861/meshuser.js#L5285
            meshcentral.sendCommand("{ \"action\": \"wakedevices\", \"nodeids\": [\"" + node.nodeid + "\"]}");
        }

        private void addMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
            addButton_Click(null, null);
        }

        private void addRelayMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device
            addRelayMapButton_Click(null, null);
        }

        private void devicesListView_DoubleClick(object sender, EventArgs e)
        {
            cancelAutoClose();
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            if (((node.conn & 1) == 0) && (node.mtype != 3)) { return; } // Agent not connected on this device & not local device

            if (deviceDoubleClickAction == 0) { addMapToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 1) { addRelayMapToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 2) { remoteDesktopToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 3) { remoteFilesToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 4) { httpToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 5) { httpsToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 6) { rdpToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 7) { sshToolStripMenuItem_Click(null, null); }
            if (deviceDoubleClickAction == 8) { scpToolStripMenuItem_Click(null, null); }
        }

        private void remoteDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            startNewDesktopViewer(node, 0);
        }

        private void askConsentBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            startNewDesktopViewer(node, 0x0008 + 0x0040); // Consent Prompt + Privacy bar
        }

        private void askConsentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            startNewDesktopViewer(node, 0x0008); // Consent Prompt
        }

        private void privacyBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (devicesListView.SelectedItems.Count != 1) { return; }
            ListViewItem selecteditem = devicesListView.SelectedItems[0];
            NodeClass node = (NodeClass)selecteditem.Tag;
            startNewDesktopViewer(node, 0x0040); // Privacy bar
        }

        private void startNewDesktopViewer(NodeClass node, int consentFlags)
        {
            if ((node.agentcaps & 1) == 0) { return; } // Agent does not support remote desktop
            if ((node.conn & 1) == 0) { return; } // Agent not connected on this device
            if (node.desktopViewer == null)
            {
                node.desktopViewer = new KVMViewer(this, meshcentral, node);
                node.desktopViewer.consentFlags = consentFlags;
                node.desktopViewer.Show();
                node.desktopViewer.MenuItemConnect_Click(null, null);
            }
            else
            {
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

        private void cancelAutoCloseButton_Click(object sender, EventArgs e)
        {
            cancelAutoClose();
        }

        public delegate void SetAutoCloseHandler();
        public void SetAutoClose()
        {
            if (this.InvokeRequired) { this.Invoke(new SetAutoCloseHandler(SetAutoClose)); return; }
            cancelAutoCloseButton1.Visible = true;
            cancelAutoCloseButton2.Visible = true;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceSettingsForm f = new DeviceSettingsForm();
            f.deviceDoubleClickAction = deviceDoubleClickAction;
            f.ShowSystemTray = (notifyIcon.Visible == true);
            f.Exp_KeyboardHookPriority = Settings.GetRegValue("Exp_KeyboardHookPriority", false);
            f.Exp_KeyboardHook = Settings.GetRegValue("Exp_KeyboardHook", false);
            f.CheckForUpdates = Settings.GetRegValue("CheckForUpdates", true);
            collapseDeviceGroup = f.CollapseDeviceGroups = Settings.GetRegValue("CollapseDeviceGroups", true);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                bool updateDevices = (collapseDeviceGroup != f.CollapseDeviceGroups);
                collapseDeviceGroup = f.CollapseDeviceGroups;
                Settings.SetRegValue("CollapseDeviceGroups", f.CollapseDeviceGroups);
                Settings.SetRegValue("CheckForUpdates", f.CheckForUpdates);
                Settings.SetRegValue("NotifyIcon", f.ShowSystemTray);
                allowUpdates = f.CheckForUpdates;
                deviceDoubleClickAction = f.deviceDoubleClickAction;
                Settings.SetRegValue("DevDoubleClickClickAction", deviceDoubleClickAction.ToString());
                Settings.SetRegValue("Exp_KeyboardHook", f.Exp_KeyboardHook.ToString().ToLower());
                Settings.SetRegValue("Exp_KeyboardHookPriority", f.Exp_KeyboardHookPriority.ToString().ToLower());
                setDoubleClickDeviceAction();
                if (f.ShowSystemTray)
                {
                    notifyIcon.Visible = true;
                    this.MinimizeBox = false;
                }
                else
                {
                    notifyIcon.Visible = false;
                    this.MinimizeBox = true;
                }
                if (updateDevices) { updateDeviceList(true); }
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            cancelAutoClose();
            if ((currentPanel == 4) && (devicesTabControl.SelectedIndex == 0))
            {
                if (e.KeyChar == 27)
                {
                    searchTextBox.Text = "";
                }
                else if (e.KeyChar == 8)
                {
                    if (searchTextBox.Text.Length > 0)
                    {
                        searchTextBox.Text = searchTextBox.Text.Substring(0, searchTextBox.Text.Length - 1);
                    }
                }
                else
                {
                    searchTextBox.Text += e.KeyChar;
                }
                e.Handled = true;
            }
        }

        private void devicesListView_Click(object sender, EventArgs e)
        {
            cancelAutoClose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionSettings form = new ConnectionSettings();
            if (form.ShowDialog(this) == DialogResult.OK) { }
        }

        private void customAppsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomAppsForm f = new CustomAppsForm(Settings.GetApplications());
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                Settings.SetApplications(f.getApplications());
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Settings.SetRegValue("WindowHeight", this.Height);
        }
        
        private X509Certificate2 getClientAuthCertificate()
        {
            X509Certificate2 r = null;
            string clientCertThumbPrint = Settings.GetRegValue("ClientAuthCert", "");
            if (clientCertThumbPrint == "") return null;

            // Setup list of possible client authentication certificates
            using (X509Store CertificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                // Open the certificate stores
                CertificateStore.Open(OpenFlags.ReadOnly);

                // Load the list of trusted root certificates
                foreach (X509Certificate2 cert in CertificateStore.Certificates)
                {
                    if ((cert.HasPrivateKey) && (cert.Thumbprint == clientCertThumbPrint)) { r = cert; }
                }

                // Close the certificate stores
                CertificateStore.Close();
            }

            return r;
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
