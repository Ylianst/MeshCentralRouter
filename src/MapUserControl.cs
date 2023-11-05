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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public partial class MapUserControl : UserControl
    {
        public string name;
        public int protocol;
        public int localPort;
        public string remoteIP = null;
        public int remotePort;
        public int appId;
        public string appIdStr;
        public NodeClass node;
        public MainForm parent;
        public MeshMapper mapper;
        public string host;
        //public string authCookie;
        public string certhash;
        public bool xdebug = false;
        public bool inaddrany = false;
        public MappingStats stats = null;
        public bool autoexit = false;

        public static void saveToRegistry(string name, string value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value); } catch (Exception) { }
        }
        public static string loadFromRegistry(string name)
        {
            try { return Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, "").ToString(); } catch (Exception) { return ""; }
        }

        public MapUserControl()
        {
            InitializeComponent();
        }

        public void UpdateInfo()
        {
            if ((name != null) && (name != ""))
            {
                deviceNameLabel.Text = node.name + ": " + this.name;
            }
            else
            {
                deviceNameLabel.Text = node.name;
            }
            devicePictureBox.Image = deviceImageList.Images[node.icon - 1];
        }

        public void Start()
        {
            routingStatusLabel.Text = Translate.T(Properties.Resources.Starting);
            appButton.Enabled = (appId != 0) || (appIdStr != null);
            mapper = new MeshMapper();
            mapper.xdebug = xdebug;
            mapper.inaddrany = inaddrany;
            mapper.certhash = certhash;
            mapper.onStateMsgChanged += Mapper_onStateMsgChanged;
            string serverurl;
            int keyIndex = host.IndexOf("?key=");
            if (keyIndex >= 0) {
                serverurl = "wss://" + host.Substring(0, keyIndex) + "/" + ((node.mtype == 3)?"local":"mesh") + "relay.ashx?nodeid=" + node.nodeid + "&key=" + host.Substring(keyIndex + 5);
            } else {
                serverurl = "wss://" + host + "/" + ((node.mtype == 3) ? "local" : "mesh") + "relay.ashx?nodeid=" + node.nodeid;
            }
            if (protocol == 1) {
                serverurl += ("&tcpport=" + remotePort);
                if (remoteIP != null) { serverurl += "&tcpaddr=" + remoteIP; }
            } else if (protocol == 2) {
                serverurl += ("&udpport=" + remotePort);
                if (remoteIP != null) { serverurl += "&udpaddr=" + remoteIP; }
            }
            mapper.start(parent.meshcentral, protocol, localPort, serverurl, remotePort, remoteIP);
            UpdateInfo();
        }

        public void Stop()
        {
            routingStatusLabel.Text = Translate.T(Properties.Resources.Stopped);
            appButton.Enabled = false;
            if (mapper != null) { mapper.onStateMsgChanged -= Mapper_onStateMsgChanged; mapper.stop(); mapper = null; }
            if (stats != null) { stats.Close(); stats = null; }
        }

        private void Mapper_onStateMsgChanged(string statemsg)
        {
            if (this.InvokeRequired) { this.Invoke(new MeshMapper.onStateMsgChangedHandler(Mapper_onStateMsgChanged), statemsg); return; }
            if (protocol == 2) {
                statemsg = "UDP: " + statemsg;
            } else {
                if ((appId == 0) && (appIdStr != null)) { statemsg = appIdStr + ": " + statemsg; }
                else if (appId == 1) { statemsg = "HTTP: " + statemsg; }
                else if (appId == 2) { statemsg = "HTTPS: " + statemsg; }
                else if (appId == 3) { statemsg = "RDP: " + statemsg; }
                else if (appId == 4) { statemsg = "PuTTY: " + statemsg; }
                else if (appId == 5) { statemsg = "WinSCP: " + statemsg; }
                else { statemsg = "TCP: " + statemsg; }
            }
            routingStatusLabel.Text = statemsg;
        }

        public void appButton_Click(object sender, EventArgs e)
        {
            bool shift = false;
            if (Control.ModifierKeys == Keys.Shift) { shift = true; }

            if (appId == 0)
            {
                if (appIdStr == null) return;
                string appCmd = null;
                string appArgs = null;
                List<String[]> apps = Settings.GetApplications();
                foreach (String[] app in apps) { if (app[1].ToLower() == appIdStr.ToLower()) { appCmd = app[2]; if (app[3] != null) { appArgs = app[3].Replace("%P", mapper.localport.ToString()).Replace("%L", "127.0.0.1").Replace("%N", "xxx"); } } }
                if (appCmd != null) {
                    // Launch the process
                    System.Diagnostics.Process proc = null;
                    try { proc = System.Diagnostics.Process.Start(appCmd, appArgs); } catch (System.ComponentModel.Win32Exception) { }
                    // Setup auto-exit
                    if ((autoexit == true) && (parent.autoExitProc == null)) { parent.autoExitProc = proc; parent.SetAutoClose(); autoExitTimer.Enabled = true; }
                    autoexit = false;
                }
                return;
            }
            if (appId == 1) { Shell.Start("http://localhost:" + mapper.localport); }
            if (appId == 2) { Shell.Start("https://localhost:" + mapper.localport); }
            if (appId == 3)
            {
                System.Diagnostics.Process proc = null;
                string cmd = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System) + "\\mstsc.exe";
                string tfile = Path.Combine(Path.GetTempPath(), "MeshRdpFile.rdp");
                string[] f = null;
                try { if (File.Exists(tfile)) f = File.ReadAllLines(tfile); } catch (Exception) { }
                if (f != null)
                {
                    List<string> f2 = new List<string>();
                    foreach (string fx in f) { if (!fx.StartsWith("full address")) f2.Add(fx); }
                    f2.Add(string.Format("full address:s:127.0.0.1:{0}", mapper.localport));
                    File.WriteAllLines(tfile, f2.ToArray());
                }
                else
                {
                    File.WriteAllText(tfile, string.Format("full address:s:127.0.0.1:{0}", mapper.localport));
                }
                string args = "/edit:\"" + tfile + "\"";

                // Launch the process
                try { proc = System.Diagnostics.Process.Start(cmd, args); }
                catch (System.ComponentModel.Win32Exception) { }

                // Setup auto-exit
                if ((autoexit == true) && (parent.autoExitProc == null)) { parent.autoExitProc = proc; parent.SetAutoClose(); autoExitTimer.Enabled = true; }
                autoexit = false;
            }
            if (appId == 4)
            {
                string appTag = null;
                string appPath = null;
                string puttyPath = loadFromRegistry("PuttyPath");
                string openSshPath = loadFromRegistry("OpenSshPath");
                if (File.Exists(puttyPath)) { appTag = "PuttyPath"; appPath = puttyPath; }
                if (File.Exists(openSshPath)) { appTag = "OpenSshPath"; appPath = openSshPath; }

                if ((shift == false) && (appPath != null))
                {
                    // Launch the process
                    System.Diagnostics.Process proc = null;
                    string args = null;
                    if (appTag == "OpenSshPath") {
                        SshUsernameForm f2 = new SshUsernameForm();
                        if (f2.ShowDialog(this) != DialogResult.OK) return;
                        args = "127.0.0.1 -l " + f2.Username + " -p " + mapper.localport;
                    }
                    if (appTag == "PuttyPath") { args = "-ssh 127.0.0.1 -P " + mapper.localport; }
                    try { proc = System.Diagnostics.Process.Start(appPath, args); }
                    catch (System.ComponentModel.Win32Exception) { }

                    // Setup auto-exit
                    if ((autoexit == true) && (parent.autoExitProc == null)) { parent.autoExitProc = proc; parent.SetAutoClose(); autoExitTimer.Enabled = true; }
                    autoexit = false;
                }
                else
                {
                    using (AppLaunchForm f = new AppLaunchForm())
                    {
                        System.Diagnostics.Process proc = null;
                        AppLaunchForm.AppInfo[] apps = new AppLaunchForm.AppInfo[2];
                        apps[0] = new AppLaunchForm.AppInfo(Translate.T(Properties.Resources.OpenSSHAppName), "https://www.openssh.com/", (parent.nativeSshPath != null) ? parent.nativeSshPath.FullName : "", "OpenSshPath");
                        apps[1] = new AppLaunchForm.AppInfo(Translate.T(Properties.Resources.PuttyAppName), "http://www.chiark.greenend.org.uk/~sgtatham/putty/", puttyPath, "PuttyPath");
                        f.SetApps(apps);
                        if (f.ShowDialog(this) == DialogResult.OK)
                        {
                            appTag = f.GetAppTag();
                            appPath = f.GetAppPath();
                            saveToRegistry(appTag, f.GetAppPath());

                            string args = null;
                            if (appTag == "OpenSshPath") {
                                SshUsernameForm f2 = new SshUsernameForm();
                                if (f2.ShowDialog(this) != DialogResult.OK) return;
                                args = "127.0.0.1 -l " + f2.Username + " -p " + mapper.localport; saveToRegistry("PuttyPath", "");
                            }
                            if (appTag == "PuttyPath") { args = "-ssh 127.0.0.1 -P " + mapper.localport; saveToRegistry("OpenSshPath", ""); }
                            if (args == null) return;

                            // Launch the process
                            try { proc = System.Diagnostics.Process.Start(appPath, args); } catch (Win32Exception) { }

                            // Setup auto-exit
                            if ((autoexit == true) && (parent.autoExitProc == null)) { parent.autoExitProc = proc; parent.SetAutoClose(); autoExitTimer.Enabled = true; }
                            autoexit = false;
                        }
                    }
                }
            }
            if (appId == 5)
            {
                string winScpPath = loadFromRegistry("WinSCPPath");
                if ((shift == false) && (File.Exists(winScpPath)))
                {
                    // Launch the process
                    System.Diagnostics.Process proc = null;
                    string args = "scp://127.0.0.1:" + mapper.localport;
                    try { proc = System.Diagnostics.Process.Start(winScpPath, args); }
                    catch (System.ComponentModel.Win32Exception) { }

                    // Setup auto-exit
                    if ((autoexit == true) && (parent.autoExitProc == null)) { parent.autoExitProc = proc; parent.SetAutoClose(); autoExitTimer.Enabled = true; }
                    autoexit = false;
                }
                else
                {
                    using (AppLaunchForm f = new AppLaunchForm())
                    {
                        System.Diagnostics.Process proc = null;
                        f.SetAppName(Translate.T(Properties.Resources.WinscpAppName));
                        f.SetAppLink("http://winscp.net/");
                        f.SetAppPath(winScpPath);
                        if (f.ShowDialog(this) == DialogResult.OK)
                        {
                            saveToRegistry("WinSCPPath", f.GetAppPath());
                            string args = "scp://127.0.0.1:" + mapper.localport;
                            
                            // Launch the process
                            try { proc = System.Diagnostics.Process.Start(f.GetAppPath(), args); }
                            catch (System.ComponentModel.Win32Exception) { }

                            // Setup auto-exit
                            if ((autoexit == true) && (parent.autoExitProc == null)) { parent.autoExitProc = proc; parent.SetAutoClose(); autoExitTimer.Enabled = true; }
                            autoexit = false;
                        }
                    }
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (stats != null) { stats.Close(); stats = null; }
            if (mapper != null) { mapper.stop(); mapper = null; }
            parent.removeMap(this);
        }

        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stats == null) {
                stats = new MappingStats(this);
                stats.Show(this);
            } else {
                stats.Focus();
            }
        }

        public void closeStats()
        {
            if (stats == null) return;
            stats.Close();
            stats = null;
        }

        private void autoExitTimer_Tick(object sender, EventArgs e)
        {
            if (parent.autoExitProc == null) return;
            if (parent.autoExitProc.HasExited == true) { Application.Exit(); }
        }
    }
}
