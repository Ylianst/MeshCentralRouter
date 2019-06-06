using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Net.Sockets;
using System.Net.Security;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Deployment.Application;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public class MeshMapper
    {
        public int state = 0;
        Uri wsurl = null;
        public int protocol = 1; // 1 = TCP, 2 = UDP
        public int localport = 0;
        public int remoteport = 0;
        public string remoteip = null;
        public string certhash = null;
        public int connectCounter = 0;
        public int totalConnectCounter = 0;
        public bool exit = false;
        public bool xdebug = false;
        public bool inaddrany = false;
        TcpListener listener = null;

        public delegate void onStateMsgChangedHandler(string statemsg);
        public event onStateMsgChangedHandler onStateMsgChanged;

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

        // Starts the routing server, called when the start button is pressed
        public void start(int protocol, int localPort, string url, int remotePort, string remoteIP)
        {
            this.protocol = protocol;
            this.remoteport = remotePort;
            this.remoteip = remoteIP;
            wsurl = new Uri(url);
            //wshash = serverHashTextBox.Text;

            if (protocol == 1)
            {
                // Start the TCP listener
                try
                {
                    if (listener != null) return;
                    listener = new TcpListener(inaddrany ? IPAddress.Any : IPAddress.Loopback, localPort);
                    try { listener.Start(); } catch (Exception) { listener = null; }
                    if (listener != null)
                    {
                        listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientSink), null);
                        localport = ((IPEndPoint)listener.LocalEndpoint).Port;
                        state = 1;
                        UpdateInfo();
                    }
                    else
                    {
                        state = -1;
                        UpdateInfo();
                    }
                }
                catch (Exception ex) {
                    Debug(ex.ToString());
                    stop();
                }
            } else if (protocol == 2) {
                // Start websocket connection
                try
                {
                    IPEndPoint udpEndPoint = new IPEndPoint(inaddrany ? IPAddress.Any : IPAddress.Loopback, localPort);
                    UdpClient client = new UdpClient(udpEndPoint);
                    localport = ((IPEndPoint)client.Client.LocalEndPoint).Port;
                    state = 1;
                    UpdateInfo();
                    ConnectWS(client, ++connectCounter);
                }
                catch (Exception ex) {
                    Debug(ex.ToString());
                    stop();
                }
            }

        }

        public void Debug(string msg) { if (xdebug) { try { File.AppendAllText("debug.log", "Debug-MeshMapper-" + msg + "\r\n"); } catch (Exception) { } } }

        private void UpdateInfo() {
            string msg = "";
            if (state == -1) { msg = "Unable to bind to local port"; }
            else if (state == 0) { msg = "Stopped"; }
            else if (state == 1) {
                if (remoteip == null)
                {
                    msg = "Port " + localport + " to port " + remoteport;
                } else {
                    msg = "Port " + localport + " to " + remoteip + ":" + remoteport;
                }
                if (totalConnectCounter == 1) { msg += ", 1 connection."; }
                if (totalConnectCounter > 1) { msg += ", " + totalConnectCounter + " connections."; }
            }
            if (onStateMsgChanged != null) { onStateMsgChanged(msg); }
        }

        public void stop()
        {
            Debug("stopButton_Click()");
            exit = true;
            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }
            localport = 0;
            state = 0;
            UpdateInfo();
        }

        private void ShutdownClients(TcpClient c1, UdpClient c2, xwebclient c3, int counter)
        {
            Debug("#" + counter + ": ShutdownWebClients()");

            if (c1 != null)
            {
                try
                {
                    if (c1.Client.Connected)
                    {
                        try { c1.Client.Disconnect(false); } catch (Exception) { }
                        try { c1.Client.Close(); } catch (Exception) { }
                        try { c1.Close(); } catch (Exception) { }
                    }
                }
                catch (Exception) { }
            }
            if (c2 != null)
            {
                try { c2.Close(); } catch (Exception) { }
            }
            try { c3.Dispose(); } catch (Exception) { }
        }

        private void AcceptTcpClientSink(IAsyncResult ar)
        {
            TcpClient client = null;
            try
            {
                client = listener.EndAcceptTcpClient(ar);
            }
            catch (Exception) { exit = true; }

            if (client != null)
            {
                // Connect using websocket
                ConnectWS(client, ++connectCounter);
            }

            try
            {
                listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientSink), null);
            }
            catch (Exception) { exit = true; }
        }

        private void ConnectWS(TcpClient client, int counter)
        {
            xwebclient wc = new xwebclient();
            Debug("#" + counter + ": Connecting web socket to: " + wsurl.ToString());
            wc.Start(this, wsurl, certhash, client, counter);
        }
        private void ConnectWS(UdpClient client, int counter)
        {
            xwebclient wc = new xwebclient();
            Debug("#" + counter + ": Connecting web socket to: " + wsurl.ToString());
            wc.Start(this, wsurl, certhash, client, counter);
        }

        private class xwebclient : IDisposable
        {
            private TcpClient client = null;
            private UdpClient uclient = null;
            private NetworkStream stream = null;
            private TcpClient wsclient = null;
            private SslStream wsstream = null;
            private NetworkStream wsrawstream = null;
            private int state = 0;
            private Uri url = null;
            private byte[] readBuffer = new Byte[65000];
            private int readBufferLen = 0;
            private int accopcodes = 0;
            private bool accmask = false;
            private int acclen = 0;
            private bool tunneling = false;
            private MeshMapper parent = null;
            private bool proxyInUse = false;
            public int counter = 0;
            private string certhash;
            private IPEndPoint uendpoint = null;

            public void Dispose()
            {
                if (state == 0) return;
                state = 0;
                try { wsstream.Close(); } catch (Exception) { }
                try { wsstream.Dispose(); } catch (Exception) { }
                try { client.Close(); } catch (Exception) { }
                try { stream.Close(); } catch (Exception) { }
                stream = null;
                client = null;
                wsstream = null;
                wsclient = null;
                --parent.totalConnectCounter;
                parent.UpdateInfo();
            }

            public bool Start(MeshMapper parent, Uri url, string certhash, TcpClient client, int counter)
            {
                this.client = client;
                return StartEx(parent, url, certhash, counter);
            }

            public bool Start(MeshMapper parent, Uri url, string certhash, UdpClient client, int counter)
            {
                this.uclient = client;
                return StartEx(parent, url, certhash, counter);
            }

            public bool StartEx(MeshMapper parent, Uri url, string certhash, int counter)
            {
                if (state != 0) return false;
                state = 1;
                this.parent = parent;
                this.url = url;
                this.counter = counter;
                this.certhash = certhash;
                if (client != null) { this.stream = client.GetStream(); }
                Uri proxyUri = null;
                ++parent.totalConnectCounter;
                parent.UpdateInfo();

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
                    parent.Debug("#" + counter + ": Websocket TCP failed to connect: " + ex.ToString());
                    parent.ShutdownClients(client, uclient, this, this.counter);
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
                    parent.Debug("#" + counter + ": Websocket TCP connected, doing TLS...");
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
                    parent.Debug("#" + counter + ": Websocket proxy disconnected, length = 0.");
                    parent.ShutdownClients(client, uclient, this, this.counter);
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
                        parent.Debug("#" + counter + ": Websocket TCP connected, doing TLS...");
                        wsstream = new SslStream(wsrawstream, false, VerifyServerCertificate, null);
                        wsstream.BeginAuthenticateAsClient(url.Host, null, System.Security.Authentication.SslProtocols.Tls12, false, new AsyncCallback(OnTlsSetupSink), this);
                    }
                    else
                    {
                        // Invalid response
                        parent.Debug("#" + counter + ": Proxy connection failed: " + proxyResponse);
                        parent.ShutdownClients(client, uclient, this, this.counter);
                    }
                }
                else
                {
                    if (readBufferLen == readBuffer.Length)
                    {
                        // Buffer overflow
                        parent.Debug("#" + counter + ": Proxy connection failed");
                        parent.ShutdownClients(client, uclient, this, this.counter);
                    }
                    else
                    {
                        // Read more proxy data
                        wsrawstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnProxyResponseSink), this);
                    }
                }
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
                    parent.Debug("#" + counter + ": Websocket TLS failed: " + ex.ToString());
                    parent.ShutdownClients(client, uclient, this, this.counter);
                    return;
                }

                // Send the HTTP header
                parent.Debug("#" + counter + ": Websocket TLS setup, sending HTTP header...");
                string header = "GET " + url.PathAndQuery + " HTTP/1.1\r\nHost: " + url.Host + "\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==\r\nSec-WebSocket-Version: 13\r\n\r\n";
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
                    parent.Debug("#" + counter + ": Websocket disconnected, length = 0.");
                    parent.ShutdownClients(client, uclient, this, this.counter);
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
                    if (consumed < 0) { parent.ShutdownClients(client, uclient, this, this.counter); return; } // Error, close the connection
                    ptr += consumed;
                } while ((consumed > 0) && ((readBufferLen - consumed) > 0));

                // Move the data forward
                if ((ptr > 0) && (readBufferLen - ptr) > 0)
                {
                    //Console.Write("MOVE FORWARD\r\n");
                    Array.Copy(readBuffer, ptr, readBuffer, 0, (readBufferLen - ptr));
                }
                readBufferLen = (readBufferLen - ptr);

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
                    if ((parsedHeader == null) || (parsedHeader["_Path"] != "101")) { parent.Debug("#" + counter + ": Websocket bad header."); return -1; } // Bad header, close the connection
                    parent.Debug("#" + counter + ": Websocket got setup upgrade header.");
                    state = 2;
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
                        parent.Debug("#" + counter + ": Websocket got closed fragment.");
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
                        parent.Debug("#" + counter + ": Websocket receive large fragment: " + acclen);
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
                //parent.Debug("#" + counter + ": Websocket frag data: " + acclen);
                if ((tunneling == false) && (data[offset] == 'c'))
                {
                    parent.Debug("#" + counter + ": Websocket get server 'c' confirmation.");

                    // Server confirmed connection, start reading the TCP stream
                    //Console.Write("WS-Relay Connect\r\n");

                    if (client != null)
                    {
                        byte[] buf1 = new byte[65000];
                        try { client.GetStream().BeginRead(buf1, 4, buf1.Length - 4, new AsyncCallback(ClientEndReadWS), new object[] { this, client, buf1, counter }); } catch (Exception) { }
                        tunneling = true;
                    }
                    else if (uclient != null)
                    {
                        try { uclient.BeginReceive(new AsyncCallback(UClientEndReadWS), new object[] { this, uclient, counter }); } catch (Exception) { }
                        tunneling = true;
                    }
                }
                else
                {
                    if (client != null)
                    {
                        // Write: WS --> TCP
                        // TODO: Async write?
                        if (stream != null) { try { stream.Write(data, offset, len); } catch (Exception) { } }
                    }
                    else if (uclient != null)
                    {
                        // Write: WS --> UDP
                        if (uendpoint != null) {
                            if (offset == 0) {
                                try { uclient.Send(data, len, uendpoint); } catch (Exception) { }
                            } else {
                                byte[] data2 = new byte[len];
                                Array.Copy(data, offset, data2, 0, len);
                                try { uclient.Send(data2, len, uendpoint); } catch (Exception) { }
                            }
                        }
                    }
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

            private bool VerifyServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                // Check that the remote certificate is the expected one
                return (certificate.GetCertHashString() == certhash);
            }

            public void WriteWebSocketAsync(byte[] buf, int offset, int len, object[] args)
            {
                // Fetch the state
                xwebclient wc = (xwebclient)args[0];

                // Check that everything is ok
                if ((state < 2) || (len < 1) || (len > 65535)) { parent.ShutdownClients(client, uclient, wc, counter); return; }

                //Console.Write("Length: " + len + "\r\n");
                //System.Threading.Thread.Sleep(0);

                if (len < 126)
                {
                    // Small fragment
                    buf[2] = 130; // Fragment op code (129 = text, 130 = binary)
                    buf[3] = (byte)(len & 0x7F);
                    try { wsstream.BeginWrite(buf, 2, len + 2, new AsyncCallback(WriteWebSocketAsyncDone), args); } catch (Exception) {
                        parent.ShutdownClients(client, uclient, wc, counter); return; }
                }
                else
                {
                    // Large fragment
                    buf[0] = 130; // Fragment op code (129 = text, 130 = binary)
                    buf[1] = 126;
                    buf[2] = (byte)((len >> 8) & 0xFF);
                    buf[3] = (byte)(len & 0xFF);
                    try { wsstream.BeginWrite(buf, 0, len + 4, new AsyncCallback(WriteWebSocketAsyncDone), args); } catch (Exception) { parent.ShutdownClients(client, uclient, wc, counter); return; }
                }
            }

            private void WriteWebSocketAsyncDone(IAsyncResult ar)
            {
                // Fetch the state
                object[] args = (object[])ar.AsyncState;
                xwebclient wc = (xwebclient)args[0];
                object oclient;
                TcpClient client = null;
                UdpClient uclient = null;
                byte[] buf = null;
                int counter;

                if (args.Length == 4) {
                    oclient = client = (TcpClient)args[1];
                    buf = (byte[])args[2];
                    counter = (int)args[3];
                } else {
                    oclient = uclient = (UdpClient)args[1];
                    counter = (int)args[2];
                }

                try { wsstream.EndWrite(ar); } catch (Exception) { parent.ShutdownClients(client, uclient, wc, counter); return; }

                if (client != null) {
                    // Receive more TCP data
                    try { client.GetStream().BeginRead(buf, 4, buf.Length - 4, new AsyncCallback(ClientEndReadWS), args); } catch (Exception) { parent.ShutdownClients(client, uclient, wc, counter); return; }
                } else if (uclient != null) {
                    // Receive more UDP data
                    try { uclient.BeginReceive(new AsyncCallback(UClientEndReadWS), args); } catch (Exception) { parent.ShutdownClients(client, uclient, wc, counter); return; }
                }
            }

            // Read from the local client
            private void ClientEndReadWS(IAsyncResult ar)
            {
                // Fetch the state
                object[] args = (object[])ar.AsyncState;
                xwebclient wc = (xwebclient)args[0];
                TcpClient client = (TcpClient)args[1];
                byte[] buf = (byte[])args[2];
                int counter = (int)args[3];

                int len = 0;
                try
                {
                    // Read the data
                    if (client != null && client.Connected == true) { len = client.GetStream().EndRead(ar); }
                }
                catch (Exception) { parent.ShutdownClients(client, uclient, wc, counter); return; }

                //Debug("#" + counter + ": ClientEndRead(" + len + ") - Local Read");
                if (len > 0)
                {
                    // Forward the data & read again
                    try
                    {
                        wc.WriteWebSocketAsync(buf, 4, len, args);
                    }
                    catch (Exception)
                    {
                        parent.ShutdownClients(client, uclient, wc, counter);
                        return;
                    }
                }
                else
                {
                    parent.ShutdownClients(client, uclient, wc, counter);
                    return;
                }
            }

            private void UClientEndReadWS(IAsyncResult ar)
            {
                // Fetch the state
                object[] args = (object[])ar.AsyncState;
                xwebclient wc = (xwebclient)args[0];
                UdpClient uclient = (UdpClient)args[1];
                int counter = (int)args[2];

                byte[] buf = null;
                try
                {
                    // Read the data
                    if (uclient != null) { buf = uclient.EndReceive(ar, ref uendpoint); }
                }
                catch (Exception) { parent.ShutdownClients(client, uclient, wc, counter); return; }

                byte[] buf2 = new byte[4 + buf.Length];
                Array.Copy(buf, 0, buf2, 4, buf.Length);

                if (buf != null) { wc.WriteWebSocketAsync(buf2, 4, buf.Length, args); }
            }

        }

    }
}
