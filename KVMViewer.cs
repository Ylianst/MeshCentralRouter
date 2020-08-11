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
using System.Net.Sockets;
using System.Net.Security;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public partial class KVMViewer : Form
    {
        private KVMControl kvmControl = null;
        private MeshCentralServer server = null;
        private NodeClass node = null;
        private int state = 0;
        public webSocketClient wc = null;

        public KVMViewer(MeshCentralServer server, NodeClass node)
        {
            InitializeComponent();
            this.Text += " - " + node.name;
            this.node = node;
            this.server = server;
            kvmControl = resizeKvmControl.KVM;
            kvmControl.parent = this;
            resizeKvmControl.ZoomToFit = true;
            UpdateStatus();
            this.MouseWheel += MainForm_MouseWheel;
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
            this.Controls.Remove(mainMenu);
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
            Close();
        }

        public void MenuItemConnect_Click(object sender, EventArgs e)
        {
            if (wc != null) return;
            state = 1;
            string randomId = "abc"; // TODO
            Uri u = new Uri(server.wsurl.ToString().Replace("/control.ashx", "/") + "meshrelay.ashx?browser=1&p=2&nodeid=" + node.nodeid + "&id=" + randomId + "&auth=" + server.authCookie);
            wc = new webSocketClient();
            wc.Start(this, u, server.wshash);
        }

        public void onWebSocketConnected()
        {
            string randomId = "abc"; // TODO
            string u = "*/meshrelay.ashx?p=2&nodeid=" + node.nodeid + "&id=" + randomId + "&rauth=" + server.rauthCookie;
            server.sendCommand("{ \"action\": \"msg\", \"type\": \"tunnel\", \"nodeid\": \"" + node.nodeid + "\", \"value\": \"" + u.ToString() + "\", \"usage\": 2 }");
        }

        public void processServerData(string data) {
            if ((state == 1) && ((data == "c") || (data == "cr"))) {
                state = 3;
                kvmControl.Send("2");
                kvmControl.SendCompressionLevel();
                kvmControl.SendPause(false);
                kvmControl.SendRefresh();
                UpdateStatus();
                return;
            }
            if (state != 3) return;
        }

        public void processServerBinaryData(byte[] data, int offset, int len)
        {
            if (state != 3) return;
            kvmControl.ProcessData(data, offset, len);
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
            } else {
                // Connect
                MenuItemConnect_Click(null, null);
            }
        }


        public delegate void UpdateStatusHandler();

        private void UpdateStatus()
        {
            if (this.InvokeRequired) { this.Invoke(new UpdateStatusHandler(UpdateStatus));  return; }

            //if (kvmControl == null) return;
            switch (state)
            {
                case 0: // Disconnected
                    mainToolStripStatusLabel.Text = "Disconnected";
                    displaySelectComboBox.Visible = false;
                    kvmControl.Visible = false;
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
                    //string extras = ".";
                    //if (kvmControl.touchEnabled) extras = ", touch enabled.";
                    //mainToolStripStatusLabel.Text = string.Format("Connected. {0} tiles received, {1} tiles copied, {2} received, {3} sent{4}", kvmControl.tilecount, kvmControl.tilecopy, MeshUtils.GetKiloShortString((ulong)kvmControl.byterecv), MeshUtils.GetKiloShortString((ulong)kvmControl.bytesent), extras);
                    mainToolStripStatusLabel.Text = "Connected.";
                    kvmControl.Visible = true;
                    connectButton.Text = "Disconnect";
                    kvmControl.SendCompressionLevel();
                    break;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            node.desktopViewer = null;
            //if (kvmControl != null) kvmControl.Disconnect();
            //controller.OnNodeStateUpdated -= new MeshControl.NodeUpdateHandler(controller_NodeStateUpdated);
            //controller.OnPolicyUpdated -= new MeshControl.PolicyUpdateHandler(controller_OnPolicyUpdated);
        }

        private void statusToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            mainStatusStrip.Visible = statusToolStripMenuItem.Checked;
        }

        private void toolStripMenuItem2_DropDownOpening(object sender, EventArgs e)
        {
            //MenuItemConnect.Enabled = (kvmControl.State == KVMControl.ConnectState.Disconnected);
            //MenuItemDisconnect.Enabled = (kvmControl.State != KVMControl.ConnectState.Disconnected);
            //serverConnectToolStripMenuItem.Enabled = (server == null && kvmControl.State == KVMControl.ConnectState.Disconnected);
            //serviceDisconnectToolStripMenuItem.Enabled = (server != null && server.CurrentState != MeshSwarmServer.State.Disconnected);
        }

        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            debugToolStripMenuItem.Checked = kvmControl.debugmode;
            //pauseToolStripMenuItem.Checked = kvmControl.Pause;
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kvmControl != null) kvmControl.debugmode = debugToolStripMenuItem.Checked;
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
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                    kvmControl.SetCompressionParams(form.Compression, form.Scaling, form.FrameRate);
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (kvmControl != null) kvmControl.SendPause(WindowState == FormWindowState.Minimized);
        }

        private void zoomtofitToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            resizeKvmControl.ZoomToFit = zoomtofitToolStripMenuItem.Checked;
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
                //if (kvmControl != null) kvmControl.SendDisplay(displaynum);
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

        public static string GetProxyForUrlUsingPac(string DestinationUrl, string PacUri)
        {
            IntPtr WinHttpSession = Win32Api.WinHttpOpen("User", Win32Api.WINHTTP_ACCESS_TYPE_DEFAULT_PROXY, IntPtr.Zero, IntPtr.Zero, 0);

            Win32Api.WINHTTP_AUTOPROXY_OPTIONS ProxyOptions = new Win32Api.WINHTTP_AUTOPROXY_OPTIONS();
            Win32Api.WINHTTP_PROXY_INFO ProxyInfo = new Win32Api.WINHTTP_PROXY_INFO();

            ProxyOptions.dwFlags = Win32Api.WINHTTP_AUTOPROXY_CONFIG_URL;
            ProxyOptions.dwAutoDetectFlags = (Win32Api.WINHTTP_AUTO_DETECT_TYPE_DHCP | Win32Api.WINHTTP_AUTO_DETECT_TYPE_DNS_A);
            ProxyOptions.lpszAutoConfigUrl = PacUri;

            // Get Proxy 
            bool IsSuccess = Win32Api.WinHttpGetProxyForUrl(WinHttpSession, DestinationUrl, ref ProxyOptions, ref ProxyInfo);
            Win32Api.WinHttpCloseHandle(WinHttpSession);

            if (IsSuccess)
            {
                return ProxyInfo.lpszProxy;
            }
            else
            {
                Console.WriteLine("Error: {0}", Win32Api.GetLastError());
                return null;
            }
        }

        public class webSocketClient : IDisposable
        {
            private KVMViewer parent = null;
            private TcpClient wsclient = null;
            private SslStream wsstream = null;
            private NetworkStream wsrawstream = null;
            private int state = 0;
            private Uri url = null;
            private byte[] readBuffer = new Byte[500];
            private int readBufferLen = 0;
            private int accopcodes = 0;
            private bool accmask = false;
            private int acclen = 0;
            private bool proxyInUse = false;
            private string user = null;
            private string pass = null;
            private string token = null;
            public bool xdebug = false;
            public bool xignoreCert = false;

            public void Dispose()
            {
                try { wsstream.Close(); } catch (Exception) { }
                try { wsstream.Dispose(); } catch (Exception) { }
                wsstream = null;
                wsclient = null;
                state = -1;
            }

            public void Debug(string msg) { if (xdebug) { try { File.AppendAllText("debug.log", "Debug-" + msg + "\r\n"); } catch (Exception) { } } }

            public bool Start(KVMViewer parent, Uri url, string fingerprint)
            {
                if (state != 0) return false;
                state = 1;
                this.url = url;
                this.parent = parent;
                Uri proxyUri = null;

                // Check if we need to use a HTTP proxy (Auto-proxy way)
                try
                {
                    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                    Object x = registryKey.GetValue("AutoConfigURL", null);
                    if ((x != null) && (x.GetType() == typeof(string)))
                    {
                        string proxyStr = GetProxyForUrlUsingPac("http" + ((url.Port == 80) ? "" : "s") + "://" + url.Host + ":" + url.Port, x.ToString());
                        if (proxyStr != null) { proxyUri = new Uri("http://" + proxyStr); }
                    }
                }
                catch (Exception) { proxyUri = null; }

                // Check if we need to use a HTTP proxy (Normal way)
                if (proxyUri == null)
                {
                    var proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                    proxyUri = proxy.GetProxy(url);
                    if ((url.Host.ToLower() == proxyUri.Host.ToLower()) && (url.Port == proxyUri.Port)) { proxyUri = null; }
                }

                if (proxyUri != null)
                {
                    // Proxy in use
                    proxyInUse = true;
                    wsclient = new TcpClient();
                    wsclient.BeginConnect(proxyUri.Host, proxyUri.Port, new AsyncCallback(OnConnectSink), this);
                }
                else
                {
                    // No proxy in use
                    proxyInUse = false;
                    wsclient = new TcpClient();
                    wsclient.BeginConnect(url.Host, url.Port, new AsyncCallback(OnConnectSink), this);
                }
                return true;
            }

            private void OnConnectSink(IAsyncResult ar)
            {
                if (wsclient == null) return;

                // Accept the connection
                try
                {
                    wsclient.EndConnect(ar);
                }
                catch (Exception ex)
                {
                    Debug("Websocket TCP failed to connect: " + ex.ToString());
                    Dispose();
                    return;
                }

                if (proxyInUse == true)
                {
                    // Send proxy connection request
                    wsrawstream = wsclient.GetStream();
                    byte[] proxyRequestBuf = UTF8Encoding.UTF8.GetBytes("CONNECT " + url.Host + ":" + url.Port + " HTTP/1.1\r\nHost: " + url.Host + ":" + url.Port + "\r\n\r\n");
                    wsrawstream.Write(proxyRequestBuf, 0, proxyRequestBuf.Length);
                    wsrawstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnProxyResponseSink), this);
                }
                else
                {
                    // Start TLS connection
                    Debug("Websocket TCP connected, doing TLS...");
                    wsstream = new SslStream(wsclient.GetStream(), false, VerifyServerCertificate, null);
                    wsstream.BeginAuthenticateAsClient(url.Host, null, System.Security.Authentication.SslProtocols.Tls12, false, new AsyncCallback(OnTlsSetupSink), this);
                }
            }

            private void OnProxyResponseSink(IAsyncResult ar)
            {
                if (wsrawstream == null) return;

                int len = 0;
                try { len = wsrawstream.EndRead(ar); } catch (Exception) { }
                if (len == 0)
                {
                    // Disconnect
                    Debug("Websocket proxy disconnected, length = 0.");
                    Dispose();
                    return;
                }

                readBufferLen += len;
                string proxyResponse = UTF8Encoding.UTF8.GetString(readBuffer, 0, readBufferLen);
                if (proxyResponse.IndexOf("\r\n\r\n") >= 0)
                {
                    // We get a full proxy response, we should get something like "HTTP/1.1 200 Connection established\r\n\r\n"
                    if (proxyResponse.StartsWith("HTTP/1.1 200 "))
                    {
                        // All good, start TLS setup.
                        readBufferLen = 0;
                        Debug("Websocket TCP connected, doing TLS...");
                        wsstream = new SslStream(wsrawstream, false, VerifyServerCertificate, null);
                        wsstream.BeginAuthenticateAsClient(url.Host, null, System.Security.Authentication.SslProtocols.Tls12, false, new AsyncCallback(OnTlsSetupSink), this);
                    }
                    else
                    {
                        // Invalid response
                        Debug("Proxy connection failed: " + proxyResponse);
                        Dispose();
                    }
                }
                else
                {
                    if (readBufferLen == readBuffer.Length)
                    {
                        // Buffer overflow
                        Debug("Proxy connection failed");
                        Dispose();
                    }
                    else
                    {
                        // Read more proxy data
                        wsrawstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnProxyResponseSink), this);
                    }
                }
            }

            public string Base64Encode(string plainText)
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }

            public string Base64Decode(string base64EncodedData)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }

            private void OnTlsSetupSink(IAsyncResult ar)
            {
                if (wsstream == null) return;

                // Accept the connection
                try
                {
                    wsstream.EndAuthenticateAsClient(ar);
                }
                catch (Exception ex)
                {
                    // Disconnect
                    MessageBox.Show(ex.Message, "MeshRouter");
                    Debug("Websocket TLS failed: " + ex.ToString());
                    Dispose();
                    return;
                }

                // Fetch remote certificate
                //parent.wshash = wsstream.RemoteCertificate.GetCertHashString();

                // Setup extra headers if needed
                string extraHeaders = "";
                if (user != null && pass != null && token != null) { extraHeaders = "x-meshauth: " + Base64Encode(user) + "," + Base64Encode(pass) + "," + Base64Encode(token) + "\r\n"; }
                else if (user != null && pass != null) { extraHeaders = "x-meshauth: " + Base64Encode(user) + "," + Base64Encode(pass) + "\r\n"; }

                // Send the HTTP headers
                Debug("Websocket TLS setup, sending HTTP header...");
                string header = "GET " + url.PathAndQuery + " HTTP/1.1\r\nHost: " + url.Host + "\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==\r\nSec-WebSocket-Version: 13\r\n" + extraHeaders + "\r\n";
                wsstream.Write(UTF8Encoding.UTF8.GetBytes(header));

                // Start receiving data
                wsstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnTlsDataSink), this);
            }

            private void OnTlsDataSink(IAsyncResult ar)
            {
                if (wsstream == null) return;

                int len = 0;
                try { len = wsstream.EndRead(ar); } catch (Exception) { }
                if (len == 0)
                {
                    // Disconnect
                    Debug("Websocket disconnected, length = 0.");
                    Dispose();
                    return;
                }
                //parent.Debug("#" + counter + ": Websocket got new data: " + len);
                readBufferLen += len;

                // Consume all of the data
                int consumed = 0;
                int ptr = 0;
                do
                {
                    consumed = ProcessBuffer(readBuffer, ptr, readBufferLen - ptr);
                    if (consumed < 0) { Dispose(); return; } // Error, close the connection
                    ptr += consumed;
                } while ((consumed > 0) && ((readBufferLen - consumed) > 0));

                // Move the data forward
                if ((ptr > 0) && (readBufferLen - ptr) > 0)
                {
                    //Console.Write("MOVE FORWARD\r\n");
                    Array.Copy(readBuffer, ptr, readBuffer, 0, (readBufferLen - ptr));
                }
                readBufferLen = (readBufferLen - ptr);

                // If the buffer is too small, double the size here.
                if (readBuffer.Length - readBufferLen == 0)
                {
                    Debug("Increasing the read buffer size from " + readBuffer.Length + " to " + (readBuffer.Length * 2) + ".");
                    byte[] readBuffer2 = new byte[readBuffer.Length * 2];
                    Array.Copy(readBuffer, 0, readBuffer2, 0, readBuffer.Length);
                    readBuffer = readBuffer2;
                }

                // Receive more data
                try { wsstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnTlsDataSink), this); } catch (Exception) { }
            }

            private int ProcessBuffer(byte[] buffer, int offset, int len)
            {
                string ss = UTF8Encoding.UTF8.GetString(buffer, offset, len);

                if (state == 1)
                {
                    // Look for the end of the http header
                    string header = UTF8Encoding.UTF8.GetString(buffer, offset, len);
                    int i = header.IndexOf("\r\n\r\n");
                    if (i == -1) return 0;
                    Dictionary<string, string> parsedHeader = ParseHttpHeader(header.Substring(0, i));
                    if ((parsedHeader == null) || (parsedHeader["_Path"] != "101")) { Debug("Websocket bad header."); return -1; } // Bad header, close the connection
                    Debug("Websocket got setup upgrade header.");
                    state = 2;
                    this.parent.onWebSocketConnected();
                    return len; // TODO: Technically we need to return the header length before UTF8 convert.
                }
                else if (state == 2)
                {
                    // Parse a websocket fragment header
                    if (len < 2) return 0;
                    int headsize = 2;
                    accopcodes = buffer[offset];
                    accmask = ((buffer[offset + 1] & 0x80) != 0);
                    acclen = (buffer[offset + 1] & 0x7F);

                    if ((accopcodes & 0x0F) == 8)
                    {
                        // Close the websocket
                        Debug("Websocket got closed fragment.");
                        return -1;
                    }

                    if (acclen == 126)
                    {
                        if (len < 4) return 0;
                        headsize = 4;
                        acclen = (buffer[offset + 2] << 8) + (buffer[offset + 3]);
                    }
                    else if (acclen == 127)
                    {
                        if (len < 10) return 0;
                        headsize = 10;
                        acclen = (buffer[offset + 6] << 24) + (buffer[offset + 7] << 16) + (buffer[offset + 8] << 8) + (buffer[offset + 9]);
                        Debug("Websocket receive large fragment: " + acclen);
                    }
                    if (accmask == true)
                    {
                        // TODO: Do unmasking here.
                        headsize += 4;
                    }
                    //parent.Debug("#" + counter + ": Websocket frag header - FIN: " + ((accopcodes & 0x80) != 0) + ", OP: " + (accopcodes & 0x0F) + ", LEN: " + acclen + ", MASK: " + accmask);
                    state = 3;
                    return headsize;
                }
                else if (state == 3)
                {
                    // Parse a websocket fragment data
                    if (len < acclen) return 0;
                    //Console.Write("WSREAD: " + acclen + "\r\n");
                    ProcessWsBuffer(buffer, offset, acclen, accopcodes);
                    state = 2;
                    return acclen;
                }
                return 0;
            }

            private void ProcessWsBuffer(byte[] data, int offset, int len, int op)
            {
                bool binary = ((op & 1) == 0);

                Debug("Websocket got data.");
                //try { parent.processServerData(UTF8Encoding.UTF8.GetString(data, offset, len)); } catch (Exception ex) { }

                if (binary == false) {
                    parent.processServerData(UTF8Encoding.UTF8.GetString(data, offset, len));
                } else {
                    parent.processServerBinaryData(data, offset, len);
                }
            }

            private Dictionary<string, string> ParseHttpHeader(string header)
            {
                string[] lines = header.Replace("\r\n", "\r").Split('\r');
                if (lines.Length < 2) { return null; }
                string[] directive = lines[0].Split(' ');
                Dictionary<string, string> values = new Dictionary<string, string>();
                values["_Action"] = directive[0];
                values["_Path"] = directive[1];
                values["_Protocol"] = directive[2];
                for (int i = 1; i < lines.Length; i++)
                {
                    var j = lines[i].IndexOf(":");
                    values[lines[i].Substring(0, j).ToLower()] = lines[i].Substring(j + 1).Trim();
                }
                return values;
            }

            // Return a modified base64 SHA384 hash string of the certificate public key
            public static string GetMeshKeyHash(X509Certificate cert)
            {
                return ByteArrayToHexString(new SHA384Managed().ComputeHash(cert.GetPublicKey()));
            }

            // Return a modified base64 SHA384 hash string of the certificate
            public static string GetMeshCertHash(X509Certificate cert)
            {
                return ByteArrayToHexString(new SHA384Managed().ComputeHash(cert.GetRawCertData()));
            }

            public static string ByteArrayToHexString(byte[] Bytes)
            {
                StringBuilder Result = new StringBuilder(Bytes.Length * 2);
                string HexAlphabet = "0123456789ABCDEF";
                foreach (byte B in Bytes) { Result.Append(HexAlphabet[(int)(B >> 4)]); Result.Append(HexAlphabet[(int)(B & 0xF)]); }
                return Result.ToString();
            }

            private bool VerifyServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return (parent.server.certHash == GetMeshKeyHash(certificate));
            }

            public void WriteStringWebSocket(string data)
            {
                if (state < 2) return;

                // Convert the string into a buffer with 4 byte of header space.
                int len = UTF8Encoding.UTF8.GetByteCount(data);
                byte[] buf = new byte[4 + len];
                UTF8Encoding.UTF8.GetBytes(data, 0, data.Length, buf, 4);
                len = buf.Length - 4;

                // Check that everything is ok
                if ((state < 2) || (len < 1) || (len > 65535)) { Dispose(); return; }

                //Console.Write("Length: " + len + "\r\n");
                //System.Threading.Thread.Sleep(0);

                if (len < 126)
                {
                    // Small fragment
                    buf[2] = 129; // Fragment op code (129 = text, 130 = binary)
                    buf[3] = (byte)(len & 0x7F);
                    //try { wsstream.BeginWrite(buf, 2, len + 2, new AsyncCallback(WriteWebSocketAsyncDone), args); } catch (Exception) { Dispose(); return; }
                    wsstream.Write(buf, 2, len + 2);
                }
                else
                {
                    // Large fragment
                    buf[0] = 129; // Fragment op code (129 = text, 130 = binary)
                    buf[1] = 126;
                    buf[2] = (byte)((len >> 8) & 0xFF);
                    buf[3] = (byte)(len & 0xFF);
                    //try { wsstream.BeginWrite(buf, 0, len + 4, new AsyncCallback(WriteWebSocketAsyncDone), args); } catch (Exception) { Dispose(); return; }
                    wsstream.Write(buf, 0, len + 4);
                }
            }

            public void WriteBinaryWebSocket(byte[] data, int offset, int len)
            {
                if (state < 2) return;

                // Convert the string into a buffer with 4 byte of header space.
                byte[] buf = new byte[4 + len];
                Array.Copy(data, offset, buf, 4, len);
                len = buf.Length - 4;

                // Check that everything is ok
                if ((state < 2) || (len < 1) || (len > 65535)) { Dispose(); return; }

                //Console.Write("Length: " + len + "\r\n");
                //System.Threading.Thread.Sleep(0);

                if (len < 126)
                {
                    // Small fragment
                    buf[2] = 130; // Fragment op code (129 = text, 130 = binary)
                    buf[3] = (byte)(len & 0x7F);
                    //try { wsstream.BeginWrite(buf, 2, len + 2, new AsyncCallback(WriteWebSocketAsyncDone), args); } catch (Exception) { Dispose(); return; }
                    wsstream.Write(buf, 2, len + 2);
                }
                else
                {
                    // Large fragment
                    buf[0] = 130; // Fragment op code (129 = text, 130 = binary)
                    buf[1] = 126;
                    buf[2] = (byte)((len >> 8) & 0xFF);
                    buf[3] = (byte)(len & 0xFF);
                    //try { wsstream.BeginWrite(buf, 0, len + 4, new AsyncCallback(WriteWebSocketAsyncDone), args); } catch (Exception) { Dispose(); return; }
                    wsstream.Write(buf, 0, len + 4);
                }
            }

        }

    }
}
