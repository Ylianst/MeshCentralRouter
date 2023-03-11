/*
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
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net.Security;
using System.Net.WebSockets;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public class webSocketClient : IDisposable
    {
        private ClientWebSocket ws = null; // Native Windows WebSocket
        private CancellationTokenSource CTS;

        public bool AllowCompression = true;
        private TcpClient wsclient = null;
        private SslStream wsstream = null;
        private NetworkStream wsrawstream = null;
        private ConnectionStates state = 0;
        private int fragmentParsingState = 0;
        private Uri url = null;
        private byte[] readBuffer = new Byte[1024];
        private int readBufferLen = 0;
        private int accopcodes = 0;
        private bool accmask = false;
        private int acclen = 0;
        private bool proxyInUse = false;
        private Uri proxyUri = null;
        private string tlsCertFingerprint = null;
        private string tlsCertFingerprint2 = null;
        //private ConnectionErrors lastError = ConnectionErrors.NoError;
        public bool debug = false;
        public bool tlsdump = false;
        public Dictionary<string, string> extraHeaders = null;
        private MemoryStream inflateMemory;
        private DeflateStream inflate;
        private MemoryStream deflateMemory;
        private static byte[] inflateEnd = { 0x00, 0x00, 0xff, 0xff };
        private static byte[] inflateStart = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public int pingTimeSeconds = 0;
        public int pongTimeSeconds = 0;
        private System.Threading.Timer pingTimer = null;
        private System.Threading.Timer pongTimer = null;
        private System.Threading.Timer connectTimer = null;
        private bool pendingSendCall = false;
        private MemoryStream pendingSendBuffer = null;
        private bool readPaused = false;
        private bool shouldRead = false;
        private RNGCryptoServiceProvider CryptoRandom = new RNGCryptoServiceProvider();
        private object mainLock = new object();
        public TLSCertificateCheck TLSCertCheck = TLSCertificateCheck.Verify;
        public X509Certificate2 tlsCert = null;
        public X509Certificate2 failedTlsCert = null;
        public X509Certificate2 clientAuthCert = null;
        static public bool nativeWebSocketFirst = false;
        private SemaphoreSlim receiveLock = new SemaphoreSlim(1, 1);

        // Outside variables
        public object tag = null;
        public object[] tag2 = null;
        public int id = 0;
        public bool tunneling = false;
        public IPEndPoint endpoint;

        public enum ConnectionStates
        {
            Disconnected = 0,
            Connecting = 1,
            Connected = 2
        }

        public enum TLSCertificateCheck
        {
            Ignore = 0,
            Fingerprint = 1,
            Verify = 2
        }

        public enum ConnectionErrors
        {
            NoError = 0
        }

        public long PendingSendLength { get { if (ws != null) { lock (pendingSends) { return pendingSends.Count; } } else { return (pendingSendBuffer == null) ? 0 : pendingSendBuffer.Length; } } }

        private void TlsDump(string direction, byte[] data, int offset, int len) { if (tlsdump) { try { File.AppendAllText("debug.log", direction + ": " + BitConverter.ToString(data, offset, len).Replace("-", string.Empty) + "\r\n"); } catch (Exception) { } } }

        public delegate void onBinaryDataHandler(webSocketClient sender, byte[] data, int offset, int length, int orglen);
        public event onBinaryDataHandler onBinaryData;
        public delegate void onStringDataHandler(webSocketClient sender, string data, int orglen);
        public event onStringDataHandler onStringData;
        public delegate void onDebugMessageHandler(webSocketClient sender, string msg);
        public event onDebugMessageHandler onDebugMessage;
        public delegate void onStateChangedHandler(webSocketClient sender, ConnectionStates state);
        public event onStateChangedHandler onStateChanged;
        public delegate void onSendOkHandler(webSocketClient sender);
        public event onSendOkHandler onSendOk;

        public ConnectionStates State { get { return state; } }

        public X509Certificate RemoteCertificate { get { if (tlsCert != null) return tlsCert; try { return wsstream.RemoteCertificate; } catch (Exception) { return null; } } }

        private void SetState(ConnectionStates newstate)
        {
            if (state == newstate) return;
            state = newstate;
            if (onStateChanged != null) { onStateChanged(this, state); }
        }

        public void Dispose()
        {
            if (ws != null)
            {
                if (ws.State == WebSocketState.Open)
                {
                    CTS.CancelAfter(TimeSpan.FromSeconds(2));
                    ws.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                    ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                try { if (ws != null) { ws.Dispose(); ws = null; } } catch (Exception) { }
                try { if (CTS != null) { CTS.Dispose(); CTS = null; } } catch (Exception) { }
            }
            if (pingTimer != null) { pingTimer.Dispose(); pingTimer = null; }
            if (pongTimer != null) { pongTimer.Dispose(); pongTimer = null; }
            if (connectTimer != null) { try { connectTimer.Dispose(); } catch (Exception) { } connectTimer = null; }
            if (wsstream != null) { try { wsstream.Close(); } catch (Exception) { } try { wsstream.Dispose(); } catch (Exception) { } wsstream = null; }
            if (wsclient != null) { wsclient = null; }
            if (pendingSendBuffer != null) { pendingSendBuffer.Dispose(); pendingSendBuffer = null; }
            pendingSendCall = false;
            SetState(ConnectionStates.Disconnected);
        }

        public void Log(string msg)
        {
            if (onDebugMessage != null) { onDebugMessage(this, msg); }
            if (debug) { try { File.AppendAllText("debug.log", DateTime.Now.ToString("HH:mm:tt.ffff") + ": WebSocket: " + msg + "\r\n"); } catch (Exception) { } }
        }

        private async Task ConnectAsync(Uri url)
        {
            if (state != ConnectionStates.Disconnected) return;
            SetState(ConnectionStates.Connecting);
            this.url = url;
            if (tlsCertFingerprint != null) { this.tlsCertFingerprint = tlsCertFingerprint.ToUpper(); }
            Log("Websocket Start, URL=" + ((url == null) ? "(NULL)" : url.ToString()));

            proxyUri = Win32Api.GetProxy(url);
            if (CTS != null) CTS.Dispose();
            CTS = new CancellationTokenSource();
            try { await ws.ConnectAsync(url, CTS.Token); } catch (Exception) { SetState(0); return; }
            await Task.Factory.StartNew(ReceiveLoop, CTS.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task DisconnectAsync()
        {
            if (ws == null) return;
            if (ws.State == WebSocketState.Open)
            {
                CTS.CancelAfter(TimeSpan.FromSeconds(2));
                await ws.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            ws.Dispose();
            ws = null;
            CTS.Dispose();
            CTS = null;
        }

        public bool Start(Uri url, string tlsCertFingerprint, string tlsCertFingerprint2, bool force = false)
        {
            if ((force == false) && (state != ConnectionStates.Disconnected)) return false;
            SetState(ConnectionStates.Connecting);
            this.url = url;
            if (tlsCertFingerprint != null) { this.tlsCertFingerprint = tlsCertFingerprint.ToUpper(); }
            if (tlsCertFingerprint2 != null) { this.tlsCertFingerprint2 = tlsCertFingerprint2.ToUpper(); }
            if (nativeWebSocketFirst) { try { ws = new ClientWebSocket(); } catch (Exception) { } }
            if (ws != null)
            {
                // Use Windows native websocket
                if (clientAuthCert != null) { ws.Options.ClientCertificates.Add(clientAuthCert); }
                Log("Websocket (native) Start, URL=" + ((url == null) ? "(NULL)" : url.ToString()));
                if (extraHeaders != null) { foreach (var key in extraHeaders.Keys) { ws.Options.SetRequestHeader(key, extraHeaders[key]); } }
                Task t = ConnectAsync(url);
            }
            else
            {
                // Use C# coded websockets
                Uri proxyUri = null;
                Log("Websocket Start, URL=" + ((url == null) ? "(NULL)" : url.ToString()));

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
                    Log("Websocket proxyUri: " + proxyUri.ToString());
                    proxyInUse = true;
                    wsclient = new TcpClient();
                    wsclient.BeginConnect(proxyUri.Host, proxyUri.Port, new AsyncCallback(OnConnectSink), this);
                }
                else
                {
                    // No proxy in use
                    Log("Websocket noProxy");
                    proxyInUse = false;
                    wsclient = new TcpClient();
                    string h = url.Host;
                    if (h.StartsWith("[") && h.EndsWith("]")) { h = h.Substring(1, h.Length - 2); }
                    wsclient.BeginConnect(h, url.Port, new AsyncCallback(OnConnectSink), this);
                }

                // Start a timer that will fallback to native sockets automatically.
                // For some proxy types, native websockets are the only way to connect.
                if (connectTimer != null) { try { connectTimer.Dispose(); } catch (Exception) { } connectTimer = null; }
                connectTimer = new System.Threading.Timer(new System.Threading.TimerCallback(ConnectTimerCallback), null, 3000, 3000);
            }
            return true;
        }

        private void OnConnectSink(IAsyncResult ar)
        {
            if (connectTimer != null) { try { connectTimer.Dispose(); } catch (Exception) { } connectTimer = null; }
            if (wsclient == null) return;

            // Accept the connection
            try
            {
                wsclient.EndConnect(ar);
            }
            catch (Exception ex)
            {
                Log("Websocket TCP failed to connect: " + ex.ToString());
                if (nativeWebSocketFirst == false)
                {
                    ConnectTimerCallback(null);
                }
                else
                {
                    Dispose();
                }
                return;
            }

            if (proxyInUse == true)
            {
                // Send proxy connection request
                wsrawstream = wsclient.GetStream();

                string basicAuth = "";
                if (proxyUri?.UserInfo != null)
                {
                    // Base64 encode for basic authentication
                    string userCreds = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(proxyUri.UserInfo));
                    basicAuth = "\r\nProxy-Authorization: Basic " + userCreds;
                }

                byte[] proxyRequestBuf = UTF8Encoding.UTF8.GetBytes("CONNECT " + url.Host + ":" + url.Port + " HTTP/1.1\r\nHost: " + url.Host + ":" + url.Port + basicAuth + "\r\n\r\n");
                wsrawstream.Write(proxyRequestBuf, 0, proxyRequestBuf.Length);
                wsrawstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnProxyResponseSink), this);
            }
            else
            {
                // Start TLS connection
                Log("Websocket TCP connected, doing TLS...");
                wsstream = new SslStream(wsclient.GetStream(), false, VerifyServerCertificate, LocalCertificateSelectionCallback);
                try { wsstream.BeginAuthenticateAsClient(url.Host, null, System.Security.Authentication.SslProtocols.Tls12, false, new AsyncCallback(OnTlsSetupSink), this); } catch (Exception) { Dispose(); }
            }
        }

        private X509Certificate LocalCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return clientAuthCert;
        }

        private void OnProxyResponseSink(IAsyncResult ar)
        {
            if (wsrawstream == null) return;

            int len = 0;
            try { len = wsrawstream.EndRead(ar); } catch (Exception) { }
            if (len == 0)
            {
                // Disconnect
                Log("Websocket proxy disconnected, length = 0.");
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
                    Log("Websocket TCP connected, doing TLS...");
                    wsstream = new SslStream(wsrawstream, false, VerifyServerCertificate, LocalCertificateSelectionCallback);
                    try { wsstream.BeginAuthenticateAsClient(url.Host, null, System.Security.Authentication.SslProtocols.Tls12, false, new AsyncCallback(OnTlsSetupSink), this); } catch (Exception) { Dispose(); }
                }
                else
                {
                    // Invalid response
                    Log("Proxy connection failed: " + proxyResponse);
                    Dispose();
                }
            }
            else
            {
                if (readBufferLen == readBuffer.Length)
                {
                    // Buffer overflow
                    Log("Proxy connection failed");
                    Dispose();
                }
                else
                {
                    // Read more proxy data
                    try { wsrawstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnProxyResponseSink), this); } catch (Exception) { Dispose(); }
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
                Log("Websocket TLS failed: " + ex.ToString());
                Dispose();
                return;
            }

            pendingSendBuffer = new MemoryStream();
            pendingSendCall = false;

            // Build extra headers
            string extraHeadersStr = "";
            if (extraHeaders != null)
            {
                foreach (string key in extraHeaders.Keys) { extraHeadersStr += key + ": " + extraHeaders[key] + "\r\n"; }
            }

            // Send the HTTP headers
            Log("Websocket TLS setup, sending HTTP header...");
            string header;
            if (AllowCompression)
            {
                header = "GET " + url.PathAndQuery + " HTTP/1.1\r\nHost: " + url.Host + "\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==\r\nSec-WebSocket-Version: 13\r\nSec-WebSocket-Extensions: permessage-deflate; client_no_context_takeover\r\n" + extraHeadersStr + "\r\n";
            }
            else
            {
                header = "GET " + url.PathAndQuery + " HTTP/1.1\r\nHost: " + url.Host + "\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ==\r\nSec-WebSocket-Version: 13\r\n" + extraHeadersStr + "\r\n";
            }
            SendData(UTF8Encoding.UTF8.GetBytes(header));

            // Start receiving data
            try { wsstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnTlsDataSink), this); } catch (Exception) { Dispose(); }
        }

        private void OnTlsDataSink(IAsyncResult ar)
        {
            if (wsstream == null) return;

            int len = 0;
            try { len = wsstream.EndRead(ar); } catch (Exception) { }
            if (len == 0)
            {
                // Disconnect
                Log("Websocket disconnected, length = 0.");
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
                Log("Increasing the read buffer size from " + readBuffer.Length + " to " + (readBuffer.Length * 2) + ".");
                byte[] readBuffer2 = new byte[readBuffer.Length * 2];
                Array.Copy(readBuffer, 0, readBuffer2, 0, readBuffer.Length);
                readBuffer = readBuffer2;
            }

            lock (mainLock)
            {
                // Receive more data
                if (readPaused == false)
                {
                    if (wsstream != null)
                    {
                        try { wsstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnTlsDataSink), this); } catch (Exception) { }
                    }
                }
                else
                {
                    shouldRead = true;
                }
            }
        }
        private void WriteWebSocketAsyncDone(IAsyncResult ar)
        {
            if ((wsstream == null) || (pendingSendBuffer == null)) return;
            try { wsstream.EndWrite(ar); } catch (Exception) { }
            if (pendingSendBuffer == null) return;
            lock (pendingSendBuffer)
            {
                if (pendingSendBuffer == null) return;
                if (pendingSendBuffer.Length > 0)
                {
                    byte[] buf = pendingSendBuffer.ToArray();
                    try { wsstream.BeginWrite(buf, 0, buf.Length, new AsyncCallback(WriteWebSocketAsyncDone), null); } catch (Exception) { Dispose(); return; }
                    pendingSendBuffer.SetLength(0);
                }
                else
                {
                    pendingSendCall = false;
                    if (onSendOk != null) { onSendOk(this); }
                }
            }
        }

        private void PingTimerCallback(object state) { SendPing(null, 0, 0); }

        private void PongTimerCallback(object state) { SendPong(null, 0, 0); }

        private void ConnectTimerCallback(object state)
        {
            // Switch from C# sockets to native sockets
            if ((nativeWebSocketFirst == false) && (this.state == ConnectionStates.Connecting))
            {
                Log("Switching to native Websocket");
                if (pingTimer != null) { try { pingTimer.Dispose(); } catch (Exception) { } pingTimer = null; }
                if (pongTimer != null) { try { pongTimer.Dispose(); } catch (Exception) { } pongTimer = null; }
                if (connectTimer != null) { try { connectTimer.Dispose(); } catch (Exception) { } connectTimer = null; }
                if (wsstream != null) { try { wsstream.Close(); } catch (Exception) { } try { wsstream.Dispose(); } catch (Exception) { } wsstream = null; }
                if (wsclient != null) { wsclient = null; }
                if (pendingSendBuffer != null) { pendingSendBuffer.Dispose(); pendingSendBuffer = null; }
                nativeWebSocketFirst = true;
                Start(this.url, this.tlsCertFingerprint, this.tlsCertFingerprint2, true);
            }
        }

        private int ProcessBuffer(byte[] buffer, int offset, int len)
        {
            TlsDump("InRaw", buffer, offset, len);
            string ss = UTF8Encoding.UTF8.GetString(buffer, offset, len);

            if (state == ConnectionStates.Connecting)
            {
                // Look for the end of the http header
                string header = UTF8Encoding.UTF8.GetString(buffer, offset, len);
                int i = header.IndexOf("\r\n\r\n");
                if (i == -1) return 0;
                Dictionary<string, string> parsedHeader = ParseHttpHeader(header.Substring(0, i));
                if ((parsedHeader == null) || (parsedHeader["_Path"] != "101")) { Log("Websocket bad header."); return -1; } // Bad header, close the connection
                Log("Websocket got setup upgrade header.");
                SetState(ConnectionStates.Connected);

                if (parsedHeader.ContainsKey("sec-websocket-extensions") && (parsedHeader["sec-websocket-extensions"].IndexOf("permessage-deflate") >= 0))
                {
                    inflateMemory = new MemoryStream();
                    inflate = new DeflateStream(inflateMemory, CompressionMode.Decompress);
                    deflateMemory = new MemoryStream();
                }

                // Start ping/pong timers if needed
                if (pingTimeSeconds > 0) { pingTimer = new System.Threading.Timer(new System.Threading.TimerCallback(PingTimerCallback), null, pingTimeSeconds * 1000, pingTimeSeconds * 1000); }
                if (pongTimeSeconds > 0) { pongTimer = new System.Threading.Timer(new System.Threading.TimerCallback(PongTimerCallback), null, pongTimeSeconds * 1000, pongTimeSeconds * 1000); }

                fragmentParsingState = 1;
                return len; // TODO: Technically we need to return the header length before UTF8 convert.
            }
            else if (state == ConnectionStates.Connected)
            {
                if (fragmentParsingState == 1)
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
                        Log("Websocket got closed fragment.");
                        return -1;
                    }

                    // For control commands with no playloads like ping and pong, handle this here.
                    if (acclen == 0) { ProcessWsBuffer(null, 0, 0, accopcodes); return headsize; }

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
                        Log("Websocket receive large fragment: " + acclen);
                    }
                    if (accmask == true)
                    {
                        // TODO: Do unmasking here.
                        headsize += 4;
                    }
                    //parent.Debug("#" + counter + ": Websocket frag header - FIN: " + ((accopcodes & 0x80) != 0) + ", OP: " + (accopcodes & 0x0F) + ", LEN: " + acclen + ", MASK: " + accmask);
                    fragmentParsingState = 2;
                    return headsize;
                }
                if (fragmentParsingState == 2)
                {
                    // Parse a websocket fragment data
                    if (len < acclen) return 0;
                    //Console.Write("WSREAD: " + acclen + "\r\n");
                    ProcessWsBuffer(buffer, offset, acclen, accopcodes);
                    fragmentParsingState = 1;
                    return acclen;
                }
            }
            return 0;
        }

        private void ProcessWsBuffer(byte[] data, int offset, int len, int op)
        {
            int orglen = len;
            MemoryStream mem = null;
            if (((op & 0x40) != 0) && (inflateMemory != null))
            {
                // This is a deflate compressed frame
                lock (inflateMemory)
                {
                    inflateMemory.SetLength(0);
                    inflateMemory.Write(data, offset, len);
                    inflateMemory.Write(inflateEnd, 0, 4);
                    inflateMemory.Seek(0, SeekOrigin.Begin);
                    MemoryStream memoryStream = new MemoryStream();
                    inflate.CopyTo(memoryStream);
                    data = memoryStream.GetBuffer();
                    offset = 0;
                    len = (int)memoryStream.Length;
                }
            }

            switch (op & 0x0F)
            {
                case 0x01: // This is a text frame
                    {
                        Log("Websocket got string data, len = " + len);
                        TlsDump("InStr", data, offset, len);
                        if (onStringData != null) { onStringData(this, UTF8Encoding.UTF8.GetString(data, offset, len), orglen); }
                        break;
                    }
                case 0x02: // This is a birnay frame
                    {
                        Log("Websocket got binary data, len = " + len);
                        TlsDump("InBin", data, offset, len);
                        if (onBinaryData != null) { onBinaryData(this, data, offset, len, orglen); }
                        break;
                    }
                case 0x09: // Ping
                    {
                        SendPong(data, offset, len);
                        break;
                    }
                case 0x0A: // Pong
                    {
                        break;
                    }
            }
            if (mem != null) { mem.Dispose(); mem = null; }
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
            string hash1 = GetMeshCertHash(certificate);
            string hash2 = certificate.GetCertHashString();
            //Debug("Verify cert: " + hash1);

            if (TLSCertCheck == TLSCertificateCheck.Ignore)
            {
                // Ignore certificate check
                return true;
            }
            else if (TLSCertCheck == TLSCertificateCheck.Fingerprint)
            {
                // Fingerprint certificate check
                if (tlsCertFingerprint == null) return true;
                if ((tlsCertFingerprint.Length == 32) && (certificate.GetCertHashString().Equals(tlsCertFingerprint))) { return true; }
                if (tlsCertFingerprint.Length == 96)
                {
                    if (GetMeshCertHash(certificate).Equals(tlsCertFingerprint)) { return true; }
                    if (GetMeshKeyHash(certificate).Equals(tlsCertFingerprint)) { return true; }
                }

                Log("VerifyServerCertificate: tlsCertFingerprint = " + tlsCertFingerprint);
                Log("VerifyServerCertificate: Hash1 = " + hash1);
                Log("VerifyServerCertificate: Hash2 = " + hash2);
                return ((tlsCertFingerprint == GetMeshKeyHash(certificate)) || (tlsCertFingerprint == certificate.GetCertHashString()));
            }
            else
            {
                // Normal certificate check
                if (chain.Build(new X509Certificate2(certificate)) == true) return true;

                // Check that the remote certificate is the expected one
                if ((tlsCertFingerprint != null) && ((tlsCertFingerprint == certificate.GetCertHashString()) || (tlsCertFingerprint == GetMeshKeyHash(certificate)) || (tlsCertFingerprint == GetMeshCertHash(certificate)))) { return true; }
                if ((tlsCertFingerprint2 != null) && ((tlsCertFingerprint2 == certificate.GetCertHashString()) || (tlsCertFingerprint2 == GetMeshKeyHash(certificate)) || (tlsCertFingerprint2 == GetMeshCertHash(certificate)))) { return true; }
                failedTlsCert = new X509Certificate2(certificate);
                return false;
            }
        }

        public int SendString(string data)
        {
            if (state != ConnectionStates.Connected) return 0;
            Log("WebSocketClient-SEND-String: " + data);
            byte[] buf = UTF8Encoding.UTF8.GetBytes(data);
            return SendFragment(buf, 0, buf.Length, 129, true);
        }

        public int SendBinary(byte[] data)
        {
            Log("WebSocketClient-SEND-Binary-Len:" + data.Length);
            return SendFragment(data, 0, data.Length, 130, false);
        }

        public int SendBinary(byte[] data, int offset, int len)
        {
            Log("WebSocketClient-SEND-Binary-Len:" + len);
            return SendFragment(data, offset, len, 130, false);
        }

        public int SendPing(byte[] data, int offset, int len)
        {
            Log("WebSocketClient-SEND-Ping");
            return SendFragment(null, 0, 0, 137, true);
        }

        public int SendPong(byte[] data, int offset, int len)
        {
            Log("WebSocketClient-SEND-Pong");
            return SendFragment(null, 0, 0, 138, true);
        }

        // This controls the flow of fragments being sent, queuing send operations if needed
        private Task pendingSend = null;
        private List<pendingSendClass> pendingSends = new List<pendingSendClass>();
        private class pendingSendClass
        {
            public pendingSendClass(byte[] data, int offset, int len, byte op) { this.data = data; this.offset = offset; this.len = len; this.op = op; }
            public byte[] data;
            public int offset;
            public int len;
            public byte op;
        }

        // Fragment op code (129 = text, 130 = binary)
        // ownershipReleased = True is the memory in data is not owned anymore.
        private int SendFragment(byte[] data, int offset, int len, byte op, bool ownershipReleased)
        {
            TlsDump("Out(" + op + ")", data, offset, len);
            if (ws != null)
            {
                if ((data == null) || (len == 0)) return 0;

                if (ownershipReleased == false)
                {
                    // Since this is going into a aynsc send or in a queue, copy the outgoing data into a new buffer.
                    byte[] buf = new byte[len];
                    Array.Copy(data, offset, buf, 0, len);
                    data = buf;
                    offset = 0;
                }

                // Using native websocket
                lock (pendingSends)
                {
                    if (pendingSend != null)
                    {
                        // A send operating is already being processes, queue this send.
                        pendingSends.Add(new pendingSendClass(data, 0, len, op));
                    }
                    else
                    {
                        // No send operations being performed now, send this fragment now.
                        ArraySegment<byte> arr = new ArraySegment<byte>(data, offset, len);
                        WebSocketMessageType msgType = ((op == 129) ? WebSocketMessageType.Text : WebSocketMessageType.Binary);
                        pendingSend = ws.SendAsync(arr, msgType, true, CTS.Token);
                        pendingSend.ContinueWith(antecedent => SendFragmentDone());
                    }
                }
                return len;
            }
            else
            {
                // Using C# websocket
                lock (mainLock)
                {
                    if (state != ConnectionStates.Connected) return 0;
                    byte[] buf;

                    // If deflate is active, attempt to compress the data here.
                    if ((deflateMemory != null) && (len > 32) && (AllowCompression))
                    {
                        deflateMemory.SetLength(0);
                        DeflateStream deflate = new DeflateStream(deflateMemory, CompressionMode.Compress, true);
                        deflate.Write(data, offset, len);
                        deflate.Dispose();
                        deflate = null;
                        if (deflateMemory.Length < len)
                        {
                            // Copy to a new buffer, this is needed because we do async send operation
                            int newlen = (int)deflateMemory.Length;
                            buf = new byte[14 + newlen];
                            Array.Copy(deflateMemory.GetBuffer(), 0, buf, 14, newlen);
                            len = newlen;
                            op |= 0x40; // Add compression op
                        }
                        else
                        {
                            // Don't use the compress data
                            // Convert the string into a buffer with 4 byte of header space.
                            buf = new byte[14 + len];
                            Array.Copy(data, offset, buf, 14, len);
                        }
                    }
                    else
                    {
                        // Convert the string into a buffer with 14 bytea of header space.
                        buf = new byte[14 + len];
                        if (len > 0) { Array.Copy(data, offset, buf, 14, len); }
                    }

                    // Check that everything is ok
                    if (len < 0) { Dispose(); return 0; }

                    // Set the mask to a cryptographic random value and XOR the data
                    byte[] rand = new byte[4];
                    CryptoRandom.GetBytes(rand);
                    Array.Copy(rand, 0, buf, 10, 4);
                    for (int x = 0; x < len; x++) { buf[x + 14] ^= rand[x % 4]; }

                    if (len < 126)
                    {
                        // Small fragment
                        buf[8] = op;
                        buf[9] = (byte)((len & 0x7F) + 128); // Add 128 to indicate the mask is present
                        SendData(buf, 8, len + 6);
                    }
                    else if (len < 65535)
                    {
                        // Medium fragment
                        buf[6] = op;
                        buf[7] = 126 + 128; // Add 128 to indicate the mask is present
                        buf[8] = (byte)((len >> 8) & 0xFF);
                        buf[9] = (byte)(len & 0xFF);
                        SendData(buf, 6, len + 8);
                    }
                    else
                    {
                        // Large fragment
                        buf[0] = op;
                        buf[1] = 127 + 128; // Add 128 to indicate the mask is present
                        buf[6] = (byte)((len >> 24) & 0xFF);
                        buf[7] = (byte)((len >> 16) & 0xFF);
                        buf[8] = (byte)((len >> 8) & 0xFF);
                        buf[9] = (byte)(len & 0xFF);
                        SendData(buf, 0, len + 14);
                    }

                    return len;
                }
            }
        }


        // Called when a fragment is done sending. We look to send the next one or signal that we can accept more data
        private void SendFragmentDone()
        {
            bool q = false;
            lock (pendingSends)
            {
                pendingSend = null;
                if (pendingSends.Count > 0)
                {
                    // There is more send operation pending, send the next one now.
                    pendingSendClass p = pendingSends[0];
                    pendingSends.RemoveAt(0);
                    ArraySegment<byte> arr = new ArraySegment<byte>(p.data, p.offset, p.len);
                    WebSocketMessageType msgType = ((p.op == 129) ? WebSocketMessageType.Text : WebSocketMessageType.Binary);
                    pendingSend = ws.SendAsync(arr, msgType, true, CTS.Token);
                    pendingSend.ContinueWith(antecedent => SendFragmentDone());
                }
                else { q = true; } // No pending send operations, signal ok to send more.
            }
            if ((q == true) && (onSendOk != null)) { onSendOk(this); }
        }

        private void SendData(byte[] buf) { SendData(buf, 0, buf.Length); }

        private void SendData(byte[] buf, int off, int len)
        {
            TlsDump("OutRaw", buf, off, len);
            if (pendingSendCall)
            {
                lock (pendingSendBuffer) { pendingSendBuffer.Write(buf, off, len); }
            }
            else
            {
                pendingSendCall = true;
                try { wsstream.BeginWrite(buf, off, len, new AsyncCallback(WriteWebSocketAsyncDone), null); } catch (Exception) { Dispose(); return; }
            }
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

        public void Pause()
        {
            lock (mainLock)
            {
                if (readPaused == true) return;
                readPaused = true;
                if (ws != null) { receiveLock.Wait(); }
            }
        }

        public void Resume()
        {
            lock (mainLock)
            {
                if (readPaused == false) return;
                readPaused = false;
                if (ws != null)
                {
                    receiveLock.Release();
                }
                else
                {
                    if (shouldRead == true)
                    {
                        shouldRead = false;
                        try { wsstream.BeginRead(readBuffer, readBufferLen, readBuffer.Length - readBufferLen, new AsyncCallback(OnTlsDataSink), this); } catch (Exception) { }
                    }
                }
            }
        }

        private void ReceiveLoop()
        {
            SetState(ConnectionStates.Connected);
            var loopToken = CTS.Token;
            MemoryStream outputStream = null;
            WebSocketReceiveResult receiveResult = null;
            var buffer = new byte[8192];
            ArraySegment<byte> bufferEx = new ArraySegment<byte>(buffer);
            try
            {
                while (!loopToken.IsCancellationRequested)
                {
                    outputStream = new MemoryStream(8192);
                    do
                    {
                        try
                        {
                            Task<WebSocketReceiveResult> t = ws.ReceiveAsync(bufferEx, CTS.Token);
                            t.Wait();
                            receiveResult = t.Result;
                            if (receiveResult.MessageType != WebSocketMessageType.Close) { outputStream.Write(buffer, 0, receiveResult.Count); }
                        }
                        catch (Exception)
                        {
                            outputStream?.Dispose();
                            SetState(0);
                            return;
                        }
                    }
                    while (!receiveResult.EndOfMessage);
                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    outputStream.Position = 0;

                    receiveLock.Wait(); // Pause reading if needed
                    receiveLock.Release();

                    if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        Log("Websocket got string data, len = " + (int)outputStream.Length);
                        TlsDump("InStr", outputStream.GetBuffer(), 0, (int)outputStream.Length);
                        if (onStringData != null) { onStringData(this, UTF8Encoding.UTF8.GetString(outputStream.GetBuffer(), 0, (int)outputStream.Length), (int)outputStream.Length); }
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Binary)
                    {
                        Log("Websocket got binary data, len = " + (int)outputStream.Length);
                        TlsDump("InBin", outputStream.GetBuffer(), 0, (int)outputStream.Length);
                        if (onBinaryData != null) { onBinaryData(this, outputStream.GetBuffer(), 0, (int)outputStream.Length, (int)outputStream.Length); }
                    }
                }
            }
            catch (TaskCanceledException) { }
            finally
            {
                outputStream?.Dispose();
                SetState(0);
            }
        }

    }

}
