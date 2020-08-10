using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public partial class MapUserControl : UserControl
    {
        public string ruleName;
        public int protocol;
        public int localPort;
        public string remoteIP = null;
        public int remotePort;
        public int appId;
        public NodeClass node;
        public MainForm parent;
        public MeshMapper mapper;
        public string host;
        public string authCookie;
        public string certhash;
        public bool xdebug = false;
        public bool inaddrany = false;

        public static void saveToRegistry(string name, string value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenSource\MeshRouter", name, value); } catch (Exception) { }
        }
        public static string loadFromRegistry(string name)
        {
            try { return Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\OpenSource\MeshRouter", name, "").ToString(); } catch (Exception) { return ""; }
        }

        public MapUserControl()
        {
            InitializeComponent();
        }

        public void UpdateInfo()
        {
            if(this.ruleName != null)
            {
                deviceNameLabel.Text = node.name + ": " + this.ruleName;
            }
            else
            {
                deviceNameLabel.Text = node.name;
            }
            devicePictureBox.Image = deviceImageList.Images[node.icon - 1];
        }

        public void Start()
        {
            routingStatusLabel.Text = Properties.Resources.Starting;
            appButton.Enabled = (appId != 0);
            mapper = new MeshMapper();
            mapper.xdebug = xdebug;
            mapper.inaddrany = inaddrany;
            mapper.certhash = certhash;
            mapper.onStateMsgChanged += Mapper_onStateMsgChanged;
            string serverurl = "wss://" + host + "/meshrelay.ashx?auth=" + Uri.EscapeDataString(authCookie) + "&nodeid=" + node.nodeid;
            if (protocol == 1) {
                serverurl += ("&tcpport=" + remotePort);
                if (remoteIP != null) { serverurl += "&tcpaddr=" + remoteIP; }
            } else if (protocol == 2) {
                serverurl += ("&udpport=" + remotePort);
                if (remoteIP != null) { serverurl += "&udpaddr=" + remoteIP; }
            }
            mapper.start(protocol, localPort, serverurl, remotePort, remoteIP);
            UpdateInfo();
        }

        public void Stop()
        {
            routingStatusLabel.Text = Properties.Resources.Stopped;
            appButton.Enabled = false;
            mapper.onStateMsgChanged -= Mapper_onStateMsgChanged;
            mapper.stop();
            mapper = null;
        }

        private void Mapper_onStateMsgChanged(string statemsg)
        {
            if (this.InvokeRequired) { this.Invoke(new MeshMapper.onStateMsgChangedHandler(Mapper_onStateMsgChanged), statemsg); return; }
            if (protocol == 2) {
                statemsg = "UDP: " + statemsg;
            } else {
                if (appId == 1) { statemsg = "HTTP: " + statemsg; }
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
            if (appId == 1) { System.Diagnostics.Process.Start("http://localhost:" + mapper.localport); }
            if (appId == 2) { System.Diagnostics.Process.Start("https://localhost:" + mapper.localport); }
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
            }
            if (appId == 4)
            {
                using (AppLaunchForm f = new AppLaunchForm())
                {
                    System.Diagnostics.Process proc = null;
                    f.SetAppName(Properties.Resources.PuttyAppName);
                    f.SetAppLink("http://www.chiark.greenend.org.uk/~sgtatham/putty/");
                    f.SetAppPath(loadFromRegistry("PuttyPath"));
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        saveToRegistry("PuttyPath", f.GetAppPath());
                        string args = "-ssh 127.0.0.1 -P " + mapper.localport;
                        // Launch the process
                        try { proc = System.Diagnostics.Process.Start(f.GetAppPath(), args); }
                        catch (System.ComponentModel.Win32Exception) { }
                    }
                }
            }
            if (appId == 5)
            {
                using (AppLaunchForm f = new AppLaunchForm())
                {
                    System.Diagnostics.Process proc = null;
                    f.SetAppName(Properties.Resources.WinscpAppName);
                    f.SetAppLink("http://winscp.net/");
                    f.SetAppPath(loadFromRegistry("WinSCPPath"));
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        saveToRegistry("WinSCPPath", f.GetAppPath());
                        string args = "scp://127.0.0.1:" + mapper.localport;
                        // Launch the process
                        try { proc = System.Diagnostics.Process.Start(f.GetAppPath(), args); }
                        catch (System.ComponentModel.Win32Exception) { }
                    }
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (mapper != null)
            {
                mapper.stop();
                mapper = null;
            }
            parent.removeMap(this);
        }
    }
}
