/*
Copyright 2009-2018 Intel Corporation

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
using System.Web;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Net.Security;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;

namespace MeshCentralRouter
{

    public class MeshCentralServer
    {
        private Uri wsurl = null;
        private string user = null;
        private string pass = null;
        private string token = null;
        private xwebclient wc = null;
        //private System.Timers.Timer procTimer = new System.Timers.Timer(5000);
        private int constate = 0;
        public Dictionary<string, NodeClass> nodes = null;
        public Dictionary<string, MeshClass> meshes = null;
        public string disconnectCause = null;
        public string disconnectMsg = null;
        public X509Certificate2 disconnectCert;
        public string authCookie = null;
        public string loginCookie = null;
        public string wshash = null;
        public string okCertHash = null;
        public bool debug = false;
        public bool ignoreCert = false;

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

        // Parse the URL query parameters and returns a collection
        public static NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                nameValueTable = HttpUtility.ParseQueryString(queryString);
            }
            return (nameValueTable);
        }

        // Starts the routing server, called when the start button is pressed
        public void connect(Uri wsurl, string user, string pass, string token)
        {
            this.user = user;
            this.pass = pass;
            this.token = token;
            this.wsurl = wsurl;

            wc = new xwebclient();
            //Debug("#" + counter + ": Connecting web socket to: " + wsurl.ToString());
            wc.Start(this, wsurl, user, pass, token, wshash);
            if (debug) { File.AppendAllText("debug.log", "Connect-" + wsurl + "\r\n"); }
            wc.xdebug = debug;
            wc.xignoreCert = ignoreCert;
        }

        public void disconnect()
        {
            if (wc != null)
            {
                wc.Dispose();
                wc = null;
                if (debug) { File.AppendAllText("debug.log", "Disconnect\r\n"); }
            }
        }

        public void refreshCookies()
        {
            if (wc != null) {
                if (debug) { File.AppendAllText("debug.log", "RefreshCookies\r\n"); }
                wc.WriteStringWebSocket("{\"action\":\"authcookie\"}");
                wc.WriteStringWebSocket("{\"action\":\"logincookie\"}");
            }
        }

        public void processServerData(string data)
        {
            if (debug) { File.AppendAllText("debug.log", "ServerData-" + data + "\r\n"); }

            // Parse the received JSON
            Dictionary<string, object> jsonAction = new Dictionary<string, object>();
            jsonAction = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(data);
            if (jsonAction == null || jsonAction["action"].GetType() != typeof(string)) return;

            string action = jsonAction["action"].ToString();
            switch (action)
            {
                case "close":
                    {
                        disconnectCause = jsonAction["cause"].ToString();
                        disconnectMsg = jsonAction["msg"].ToString();
                        break;
                    }
                case "serverinfo":
                    {
                        wc.WriteStringWebSocket("{\"action\":\"meshes\"}");
                        wc.WriteStringWebSocket("{\"action\":\"nodes\"}");
                        wc.WriteStringWebSocket("{\"action\":\"authcookie\"}");
                        wc.WriteStringWebSocket("{\"action\":\"logincookie\"}");
                        break;
                    }
                case "authcookie":
                    {
                        authCookie = jsonAction["cookie"].ToString();
                        changeState(2);
                        break;
                    }
                case "logincookie":
                    {
                        loginCookie = jsonAction["cookie"].ToString();
                        if (onLoginTokenChanged != null) { onLoginTokenChanged(); }
                        break;
                    }
                case "userinfo":
                    {
                        break;
                    }
                case "event":
                    {
                        Dictionary<string, object> ev = (Dictionary<string, object>)jsonAction["event"];
                        string action2 = ev["action"].ToString();
                        switch (action2)
                        {
                            case "changenode":
                                {
                                    Dictionary<string, object> node = (Dictionary<string, object>)ev["node"];
                                    string nodeid = (string)node["_id"];
                                    if (nodes.ContainsKey(nodeid))
                                    {
                                        NodeClass n = (NodeClass)nodes[nodeid];
                                        n.nodeid = (string)node["_id"];
                                        n.name = (string)node["name"];
                                        if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; }
                                        n.icon = (int)node["icon"];
                                        nodes[n.nodeid] = n;
                                        if (onNodesChanged != null) onNodesChanged();
                                    }
                                    break;
                                }
                            case "nodeconnect":
                                {
                                    string nodeid = (string)ev["nodeid"];
                                    if (nodes.ContainsKey(nodeid))
                                    {
                                        NodeClass n = (NodeClass)nodes[nodeid];
                                        if (ev.ContainsKey("conn")) { n.conn = (int)ev["conn"]; }
                                        nodes[n.nodeid] = n;
                                        if (onNodesChanged != null) onNodesChanged();
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "meshes":
                    {
                        meshes = new Dictionary<string, MeshClass>();

                        ArrayList meshList = (ArrayList)jsonAction["meshes"];
                        for (int i = 0; i < meshList.Count; i++)
                        {
                            Dictionary<string, object> mesh = (Dictionary<string, object>)meshList[i];
                            MeshClass m = new MeshClass();
                            m.meshid = (string)mesh["_id"];
                            m.name = (string)mesh["name"];
                            m.desc = (string)mesh["desc"];
                            if (mesh["mtype"].GetType() == typeof(string)) { m.type = int.Parse((string)mesh["mtype"]); }
                            if (mesh["mtype"].GetType() == typeof(int)) { m.type = (int)mesh["mtype"]; }
                            meshes[m.meshid] = m;
                        }

                        break;
                    }
                case "nodes":
                    {
                        nodes = new Dictionary<string, NodeClass>();

                        Dictionary<string, object> groups = (Dictionary<string, object>)jsonAction["nodes"];
                        foreach (string meshid in groups.Keys)
                        {
                            ArrayList nodesinMesh = (ArrayList)groups[meshid];
                            for (int i = 0; i < nodesinMesh.Count; i++)
                            {
                                Dictionary<string, object> node = (Dictionary<string, object>)nodesinMesh[i];
                                NodeClass n = new NodeClass();
                                n.nodeid = (string)node["_id"];
                                n.name = (string)node["name"];
                                n.meshid = meshid;
                                if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; } else { n.conn = 0; }
                                n.icon = (int)node["icon"];
                                nodes[n.nodeid] = n;
                            }
                        }
                        if (onNodesChanged != null) onNodesChanged();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public delegate void onStateChangedHandler(int state);
        public event onStateChangedHandler onStateChanged;
        public void changeState(int newState) { if (constate != newState) { constate = newState; if (onStateChanged != null) { onStateChanged(constate); } } }

        public delegate void onNodeListChangedHandler();
        public event onNodeListChangedHandler onNodesChanged;
        public delegate void onLoginTokenChangedHandler();
        public event onLoginTokenChangedHandler onLoginTokenChanged;

        private class xwebclient : IDisposable
        {
            private MeshCentralServer parent = null;
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

            public void Dispose() {
                try { wsstream.Close(); } catch (Exception) { }
                try { wsstream.Dispose(); } catch (Exception) { }
                wsstream = null;
                wsclient = null;
                state = -1;
                parent.changeState(0);
                parent.wshash = null;
            }

            public void Debug(string msg) { if (xdebug) { try { File.AppendAllText("debug.log", "Debug-" + msg + "\r\n"); } catch (Exception) { } } }

            public bool Start(MeshCentralServer parent, Uri url, string user, string pass, string token, string fingerprint)
            {
                if (state != 0) return false;
                parent.changeState(1);
                state = 1;
                this.parent = parent;
                this.url = url;
                this.user = user;
                this.pass = pass;
                this.token = token;
                Uri proxyUri = null;

                // Check if we need to use a HTTP proxy (Auto-proxy way)
                try {
                    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                    Object x = registryKey.GetValue("AutoConfigURL", null);
                    if ((x != null) && (x.GetType() == typeof(string))) {
                        string proxyStr = GetProxyForUrlUsingPac("http" + ((url.Port == 80) ? "" : "s") + "://" + url.Host + ":" + url.Port, x.ToString());
                        if (proxyStr != null) { proxyUri = new Uri("http://" + proxyStr); }
                    }
                } catch (Exception) { proxyUri = null; }

                // Check if we need to use a HTTP proxy (Normal way)
                if (proxyUri == null) {
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
                } catch (Exception ex) {
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
                } else {
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
                parent.wshash = wsstream.RemoteCertificate.GetCertHashString();

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
                if ((ptr > 0) && (readBufferLen - ptr) > 0) {
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
                    return len; // TODO: Technically we need to return the header length before UTF8 convert.
                } else if (state == 2) {
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
                Debug("Websocket got data.");
                try { parent.processServerData(UTF8Encoding.UTF8.GetString(data, offset, len)); } catch (Exception) { }
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

            private bool VerifyServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (xignoreCert) return true;
                if (chain.Build(new X509Certificate2(certificate)) == true) return true;

                // Check that the remote certificate is the expected one
                if ((parent.okCertHash != null) && (parent.okCertHash == certificate.GetCertHashString())) return true;

                parent.disconnectMsg = "cert";
                parent.disconnectCert = new X509Certificate2(certificate);
                return false;
            }

            public void WriteStringWebSocket(string data)
            {
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