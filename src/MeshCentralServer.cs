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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Deployment.Application;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;

namespace MeshCentralRouter
{

    public class MeshCentralServer
    {
        public Uri wsurl = null;
        private string user = null;
        private string pass = null;
        private string token = null;
        private webSocketClient wc = null;
        //private System.Timers.Timer procTimer = new System.Timers.Timer(5000);
        private int constate = 0;
        public Dictionary<string, NodeClass> nodes = null;
        public Dictionary<string, MeshClass> meshes = null;
        public string disconnectCause = null;
        public string disconnectMsg = null;
        public bool disconnectEmail2FA = false;
        public bool disconnectEmail2FASent = false;
        public bool disconnectSms2FA = false;
        public bool disconnectSms2FASent = false;
        public bool disconnectMsg2FA = false;
        public bool disconnectMsg2FASent = false;
        public X509Certificate2 disconnectCert;
        public string authCookie = null;
        public string rauthCookie = null;
        public string loginCookie = null;
        public string wshash = null;
        public string certHash = null;
        public string okCertHash = null;
        public string okCertHash2 = null;
        public bool debug = false;
        public bool tlsdump = false;
        public bool ignoreCert = false;
        public string userid = null;
        public string username = null;
        public int twoFactorCookieDays = 0;
        public Dictionary<string, ulong> userRights = null;
        public Dictionary<string, string> userGroups = null;
        private JavaScriptSerializer JSON = new JavaScriptSerializer();
        public int features = 0; // Bit flags of server features
        public int features2 = 0; // Bit flags of server features
        public Dictionary<string, object> serverinfo = null;
        public Dictionary<string, object> userinfo = null;

        public int connectionState { get { return constate; } }

        // Mesh Rights
        /*
        const MESHRIGHT_EDITMESH = 1;
        const MESHRIGHT_MANAGEUSERS = 2;
        const MESHRIGHT_MANAGECOMPUTERS = 4;
        const MESHRIGHT_REMOTECONTROL = 8;
        const MESHRIGHT_AGENTCONSOLE = 16;
        const MESHRIGHT_SERVERFILES = 32;
        const MESHRIGHT_WAKEDEVICE = 64;
        const MESHRIGHT_SETNOTES = 128;
        const MESHRIGHT_REMOTEVIEWONLY = 256;
        const MESHRIGHT_NOTERMINAL = 512;
        const MESHRIGHT_NOFILES = 1024;
        const MESHRIGHT_NOAMT = 2048;
        const MESHRIGHT_DESKLIMITEDINPUT = 4096;
        const MESHRIGHT_LIMITEVENTS = 8192;
        const MESHRIGHT_CHATNOTIFY = 16384;
        const MESHRIGHT_UNINSTALL = 32768;
        */

        public static void saveToRegistry(string name, string value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value); } catch (Exception) { }
        }
        public static string loadFromRegistry(string name)
        {
            try { return Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, "").ToString(); } catch (Exception) { return ""; }
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
        public void connect(Uri wsurl, string user, string pass, string token, X509Certificate2 clientAuthCert)
        {
            JSON.MaxJsonLength = 217483647;
            this.user = user;
            this.pass = pass;
            this.token = token;
            this.wsurl = wsurl;

            // Setup extra headers if needed
            Dictionary<string, string> extraHeaders = new Dictionary<string, string>();
            if (user != null && pass != null && token != null) {
                extraHeaders.Add("x-meshauth", Base64Encode(user) + "," + Base64Encode(pass) + "," + Base64Encode(token));
            } else if (user != null && pass != null) {
                extraHeaders.Add("x-meshauth", Base64Encode(user) + "," + Base64Encode(pass));
            }

            wc = new webSocketClient();
            wc.clientAuthCert = clientAuthCert;
            wc.extraHeaders = extraHeaders;
            wc.onStateChanged += new webSocketClient.onStateChangedHandler(changeStateEx);
            wc.onStringData += new webSocketClient.onStringDataHandler(processServerData);
            //Debug("#" + counter + ": Connecting web socket to: " + wsurl.ToString());
            wc.TLSCertCheck = webSocketClient.TLSCertificateCheck.Verify;
            wc.Start(wsurl, okCertHash, okCertHash2);
            if (debug || tlsdump) { try { File.AppendAllText("debug.log", "Connect-" + wsurl + "\r\n"); } catch (Exception) { } }
            wc.debug = debug;
            wc.tlsdump = tlsdump;
            wc.TLSCertCheck = (ignoreCert) ? webSocketClient.TLSCertificateCheck.Ignore : webSocketClient.TLSCertificateCheck.Verify;
        }

        public void disconnect()
        {
            if (wc != null)
            {
                wc.Dispose();
                wc = null;
                if (debug || tlsdump) { try { File.AppendAllText("debug.log", "Disconnect\r\n"); } catch (Exception) { } }
            }
        }

        public void sendCommand(string cmd)
        {
            if (wc != null)
            {
                if (debug) { try { File.AppendAllText("debug.log", "sendCommand: " + cmd + "\r\n"); } catch (Exception) { } }
                wc.SendString(cmd);
            }
        }

        public void refreshCookies()
        {
            if (wc != null) {
                if (debug) { try { File.AppendAllText("debug.log", "RefreshCookies\r\n"); } catch (Exception) { } }
                wc.SendString("{\"action\":\"authcookie\"}");
                wc.SendString("{\"action\":\"logincookie\"}");
            }
        }

        public void setRdpPort(NodeClass node, int port)
        {
            if (wc != null)
            {
                if (debug) { try { File.AppendAllText("debug.log", "SetRdpPort\r\n"); } catch (Exception) { } }
                wc.SendString("{\"action\":\"changedevice\",\"nodeid\":\"" + node.nodeid + "\",\"rdpport\":" + port + "}");
            }
        }

        // Return the number of 2nd factor for this account
        public int CountTwoFactorAuths()
        {
            if (userinfo == null) return -1;
            int authFactorCount = 0;
            object val;
            if (userinfo.TryGetValue("otpsecret", out val) && Convert.ToInt32(val) == 1) authFactorCount++;
            if (userinfo.TryGetValue("otpduo", out val) && Convert.ToInt32(val) == 1) authFactorCount++;
            if (userinfo.TryGetValue("otpdev", out val) && Convert.ToInt32(val) == 1) authFactorCount++;
            if (userinfo.TryGetValue("otphkeys", out val) && Convert.ToInt32(val) > 0) authFactorCount += Convert.ToInt32(val);
            if ((features & 0x00800000) != 0 && userinfo.TryGetValue("otpekey", out val) && Convert.ToInt32(val) == 1) authFactorCount++;
            if ((features & 0x02000000) != 0 && (features & 0x04000000) != 0 && userinfo.TryGetValue("phone", out val) && val != null) authFactorCount++;
            if ((features2 & 0x02000000) != 0 && (features2 & 0x04000000) != 0 && userinfo.TryGetValue("msghandle", out val) && val != null) authFactorCount++;
            if (authFactorCount > 0 && userinfo.TryGetValue("otpkeys", out val) && Convert.ToInt32(val) > 0 && (features2 & 0x40000) == 0) authFactorCount++;
            return authFactorCount;
        }

        public void processServerData(webSocketClient sender, string data, int orglen)
        {
            if (debug) { try { File.AppendAllText("debug.log", "ServerData-" + data + "\r\n"); } catch (Exception) { } }

            // Parse the received JSON
            Dictionary<string, object> jsonAction = new Dictionary<string, object>();
            try
            {
                jsonAction = JSON.Deserialize<Dictionary<string, object>>(data);
            } catch (Exception ex) {
                if (debug) {
                    try {
                        File.AppendAllText("debug.log", "processServerData JSON Deserialize error: \r\n" + ex.ToString());
                        File.AppendAllText("debug.log", "Invalid data (" + data.Length + "): \r\n" + data);
                    } catch (Exception) { }
                }
                return;
            }
            if (jsonAction == null || jsonAction["action"].GetType() != typeof(string)) return;

            try
            {
                string action = jsonAction["action"].ToString();
                switch (action)
                {
                    case "pong":
                        {
                            // NOP
                            break;
                        }
                    case "ping":
                        {
                            // Send pong back
                            if (wc != null) { wc.SendString("{\"action\":\"pong\"}"); }
                            break;
                        }
                    case "close":
                        {
                            disconnectCause = jsonAction["cause"].ToString();
                            disconnectMsg = jsonAction["msg"].ToString();
                            if (jsonAction.ContainsKey("email2fa")) { disconnectEmail2FA = (bool)jsonAction["email2fa"]; } else { disconnectEmail2FA = false; }
                            if (jsonAction.ContainsKey("email2fasent")) { disconnectEmail2FASent = (bool)jsonAction["email2fasent"]; } else { disconnectEmail2FASent = false; }
                            if (jsonAction.ContainsKey("sms2fa")) { disconnectSms2FA = (bool)jsonAction["sms2fa"]; } else { disconnectSms2FA = false; }
                            if (jsonAction.ContainsKey("sms2fasent")) { disconnectSms2FASent = (bool)jsonAction["sms2fasent"]; } else { disconnectSms2FASent = false; }
                            if (jsonAction.ContainsKey("msg2fa")) { disconnectMsg2FA = (bool)jsonAction["msg2fa"]; } else { disconnectMsg2FA = false; }
                            if (jsonAction.ContainsKey("msg2fasent")) { disconnectMsg2FASent = (bool)jsonAction["msg2fasent"]; } else { disconnectMsg2FASent = false; }
                            if (jsonAction.ContainsKey("twoFactorCookieDays") && (jsonAction["twoFactorCookieDays"].GetType() == typeof(int))) { twoFactorCookieDays = (int)jsonAction["twoFactorCookieDays"]; }
                            break;
                        }
                    case "serverinfo":
                        {
                            // Get the bit flags of server features
                            serverinfo = (Dictionary<string, object>)jsonAction["serverinfo"];
                            if (serverinfo.ContainsKey("features") && (serverinfo["features"].GetType() == typeof(int))) { features = (int)serverinfo["features"]; }
                            if (serverinfo.ContainsKey("features2") && (serverinfo["features2"].GetType() == typeof(int))) { features2 = (int)serverinfo["features2"]; }

                            // Ask for a lot of things from the server
                            wc.SendString("{\"action\":\"usergroups\"}");
                            wc.SendString("{\"action\":\"meshes\"}");
                            wc.SendString("{\"action\":\"nodes\"}");
                            wc.SendString("{\"action\":\"authcookie\"}");
                            wc.SendString("{\"action\":\"logincookie\"}");
                            if (onToolUpdate != null) { wc.SendString("{\"action\":\"meshToolInfo\",\"name\":\"MeshCentralRouter\"}"); }
                            break;
                        }
                    case "authcookie":
                        {
                            authCookie = jsonAction["cookie"].ToString();
                            rauthCookie = jsonAction["rcookie"].ToString();
                            changeState(2);

                            if (sender.RemoteCertificate != null)
                            {
                                certHash = webSocketClient.GetMeshCertHash(new X509Certificate2(sender.RemoteCertificate));
                            }

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
                            userinfo = (Dictionary<string, object>)jsonAction["userinfo"];
                            userid = (string)userinfo["_id"];
                            if (userinfo.ContainsKey("name")) { username = (string)userinfo["name"]; }
                            userRights = new Dictionary<string, ulong>();
                            if (userinfo.ContainsKey("links"))
                            {
                                Dictionary<string, object> userLinks = (Dictionary<string, object>)userinfo["links"];
                                foreach (string i in userLinks.Keys)
                                {
                                    Dictionary<string, object> userLinksEx = (Dictionary<string, object>)userLinks[i];
                                    if (userLinksEx.ContainsKey("rights"))
                                    {
                                        userRights[i] = ulong.Parse(userLinksEx["rights"].ToString());
                                    }
                                }
                            }

                            int twoFactorCount = -1;
                            try { twoFactorCount = CountTwoFactorAuths(); } catch (Exception) { }
                            if (debug) {
                                try { File.AppendAllText("debug.log", "CountTwoFactorAuths-" + twoFactorCount + "\r\n"); } catch (Exception) { }
                            }
                            // If no 2FA is setup and feature flag set, set message and change state
                            if (twoFactorCount == 0 && (features & 0x00040000) != 0)
                            {
                                disconnectMsg = "2fasetuprequired";
                                changeState(0);
                            }

                            break;
                        }
                    case "usergroups":
                        {
                            userGroups = new Dictionary<string, string>();
                            if (jsonAction.ContainsKey("ugroups"))
                            {
                                Dictionary<string, object> usergroups = (Dictionary<string, object>)jsonAction["ugroups"];
                                if (usergroups != null)
                                {
                                    foreach (string i in usergroups.Keys)
                                    {
                                        Dictionary<string, object> usergroupsEx = (Dictionary<string, object>)usergroups[i];
                                        if (usergroupsEx.ContainsKey("name"))
                                        {
                                            userGroups.Add(i, usergroupsEx["name"].ToString());
                                        }
                                    }
                                    if ((onNodesChanged != null) && (nodes != null)) onNodesChanged(false);
                                }
                            }
                            break;
                        }
                    case "event":
                        {
                            Dictionary<string, object> ev = (Dictionary<string, object>)jsonAction["event"];
                            string action2 = ev["action"].ToString();
                            switch (action2)
                            {
                                case "meshchange":
                                    {
                                        // Get the new values
                                        string meshid = ev["meshid"].ToString();
                                        string meshname = (string)ev["name"];
                                        string meshdesc = (string)ev["desc"];
                                        int meshtype = (int)ev["mtype"];
                                        ulong meshrights = 0;
                                        Dictionary<string, object> links = ((Dictionary<string, object>)ev["links"]);
                                        if (links.ContainsKey(userid))
                                        {
                                            Dictionary<string, object> urights = ((Dictionary<string, object>)links[userid]);
                                            if (urights != null)
                                            {
                                                if (urights["rights"].GetType() == typeof(int)) { meshrights = (ulong)((int)urights["rights"]); }
                                                if (urights["rights"].GetType() == typeof(Int64)) { meshrights = (ulong)((Int64)urights["rights"]); }
                                            }
                                        }

                                        Dictionary<string, ulong> newlinks = new Dictionary<string, ulong>();
                                        foreach (string j in links.Keys)
                                        {
                                            Dictionary<string, object> urights = ((Dictionary<string, object>)links[j]);
                                            if (urights != null)
                                            {
                                                if (urights["rights"].GetType() == typeof(int)) { newlinks[j] = (ulong)((int)urights["rights"]); }
                                                if (urights["rights"].GetType() == typeof(Int64)) { newlinks[j] = (ulong)((Int64)urights["rights"]); }
                                            }
                                        }

                                        // Update the mesh
                                        if (meshes.ContainsKey(meshid))
                                        {
                                            MeshClass mesh = (MeshClass)meshes[meshid];
                                            mesh.name = meshname;
                                            mesh.desc = meshdesc;
                                            mesh.rights = meshrights;
                                            mesh.links = newlinks;
                                        }
                                        else
                                        {
                                            MeshClass mesh = new MeshClass();
                                            mesh.name = meshname;
                                            mesh.desc = meshdesc;
                                            mesh.rights = meshrights;
                                            mesh.type = meshtype;
                                            mesh.links = newlinks;
                                            meshes[meshid] = mesh;
                                        }
                                        wc.SendString("{\"action\":\"nodes\"}");
                                        if ((onNodesChanged != null) && (nodes != null)) onNodesChanged(false);
                                        break;
                                    }
                                case "changenode":
                                    {
                                        if (nodes == null) return;
                                        Dictionary<string, object> node = (Dictionary<string, object>)ev["node"];
                                        string nodeid = (string)node["_id"];
                                        if (nodes.ContainsKey(nodeid))
                                        {
                                            // Change existing node
                                            lock (nodes)
                                            {
                                                NodeClass n = (NodeClass)nodes[nodeid];
                                                n.name = (string)node["name"];
                                                if (node.ContainsKey("agent"))
                                                {
                                                    n.agentid = (int)((Dictionary<string, object>)node["agent"])["id"];
                                                    if (((Dictionary<string, object>)node["agent"]).ContainsKey("caps")) { n.agentcaps = (int)((Dictionary<string, object>)node["agent"])["caps"]; } else { n.agentcaps = 0; }
                                                }
                                                if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; }
                                                n.icon = 1;
                                                if (node.ContainsKey("icon")) { n.icon = (int)node["icon"]; }
                                                if (node.ContainsKey("users")) { n.users = (string[])((ArrayList)node["users"]).ToArray(typeof(string)); } else { n.users = null; }
                                                if (node.ContainsKey("rdpport")) { n.rdpport = (int)node["rdpport"]; }
                                                ulong nodeRights = 0;
                                                if (node.ContainsKey("links"))
                                                {
                                                    Dictionary<string, object> links = ((Dictionary<string, object>)node["links"]);
                                                    if (links.ContainsKey(userid))
                                                    {
                                                        Dictionary<string, object> linksEx = ((Dictionary<string, object>)links[userid]);
                                                        if (linksEx.ContainsKey("rights")) { nodeRights = (ulong)(int)linksEx["rights"]; }
                                                    }

                                                    n.links = new Dictionary<string, ulong>();
                                                    foreach (string j in links.Keys)
                                                    {
                                                        Dictionary<string, object> linksEx = ((Dictionary<string, object>)links[j]);
                                                        if (linksEx.ContainsKey("rights"))
                                                        {
                                                            n.links.Add(j, ulong.Parse(linksEx["rights"].ToString()));
                                                        }
                                                    }
                                                }
                                                n.rights = nodeRights;

                                                // Compute rights on this device
                                                ulong rights = n.rights; // Direct device rights
                                                if (meshes.ContainsKey(n.meshid)) { rights |= meshes[n.meshid].rights; } // Device group rights
                                                foreach (string i in n.links.Keys) { if (userGroups.ContainsKey(i)) { rights |= n.links[i]; } } // Take a look at group rights

                                                if (rights == 0)
                                                {
                                                    // We have no rights to this device, remove it
                                                    nodes.Remove(n.nodeid);
                                                }
                                                else
                                                {
                                                    // Update the node
                                                    nodes[n.nodeid] = n;
                                                }
                                            }
                                            if ((onNodesChanged != null) && (nodes != null)) onNodesChanged(false);
                                        }
                                        else
                                        {
                                            wc.SendString("{\"action\":\"nodes\"}");
                                        }
                                        break;
                                    }
                                case "nodeconnect":
                                    {
                                        if (nodes == null) return;
                                        string nodeid = (string)ev["nodeid"];
                                        if (nodes.ContainsKey(nodeid))
                                        {
                                            lock (nodes)
                                            {
                                                NodeClass n = (NodeClass)nodes[nodeid];
                                                if (ev.ContainsKey("conn")) { n.conn = (int)ev["conn"]; }
                                                nodes[n.nodeid] = n;
                                            }
                                            if ((onNodesChanged != null) && (nodes != null)) onNodesChanged(false);
                                        }
                                        break;
                                    }
                                case "usergroupchange":
                                    {
                                        wc.SendString("{\"action\":\"usergroups\"}");
                                        wc.SendString("{\"action\":\"nodes\"}");
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
                                if (mesh.ContainsKey("desc")) { m.desc = (string)mesh["desc"]; }
                                if (mesh.ContainsKey("relayid")) { m.relayid = (string)mesh["relayid"]; }
                                m.rights = 0;
                                m.links = new Dictionary<string, ulong>();

                                Dictionary<string, object> links = ((Dictionary<string, object>)mesh["links"]);
                                if (links.ContainsKey(userid))
                                {
                                    Dictionary<string, object> urights = ((Dictionary<string, object>)links[userid]);
                                    if (urights != null)
                                    {
                                        if (urights["rights"].GetType() == typeof(int)) { m.rights = (ulong)((int)urights["rights"]); }
                                        if (urights["rights"].GetType() == typeof(Int64)) { m.rights = (ulong)((Int64)urights["rights"]); }
                                    }
                                }

                                foreach (string j in links.Keys)
                                {
                                    Dictionary<string, object> urights = ((Dictionary<string, object>)links[j]);
                                    if (urights != null)
                                    {
                                        if (urights["rights"].GetType() == typeof(int)) { m.links[j] = (ulong)((int)urights["rights"]); }
                                        if (urights["rights"].GetType() == typeof(Int64)) { m.links[j] = (ulong)((Int64)urights["rights"]); }
                                    }
                                }

                                if (mesh["mtype"].GetType() == typeof(string)) { m.type = int.Parse((string)mesh["mtype"]); }
                                if (mesh["mtype"].GetType() == typeof(int)) { m.type = (int)mesh["mtype"]; }
                                meshes[m.meshid] = m;
                            }

                            break;
                        }
                    case "nodes":
                        {
                            if (nodes == null) { nodes = new Dictionary<string, NodeClass>(); }
                            Dictionary<string, NodeClass> nodes2 = new Dictionary<string, NodeClass>();
                            lock (nodes)
                            {
                                Dictionary<string, object> groups = (Dictionary<string, object>)jsonAction["nodes"];
                                foreach (string meshid in groups.Keys)
                                {
                                    ArrayList nodesinMesh = (ArrayList)groups[meshid];
                                    for (int i = 0; i < nodesinMesh.Count; i++)
                                    {
                                        Dictionary<string, object> node = (Dictionary<string, object>)nodesinMesh[i];
                                        string nodeid = (string)node["_id"];
                                        if (nodes.ContainsKey(nodeid))
                                        {
                                            NodeClass n = (NodeClass)nodes[nodeid];
                                            n.nodeid = nodeid;
                                            if (node.ContainsKey("agent"))
                                            {
                                                n.agentid = (int)((Dictionary<string, object>)node["agent"])["id"];
                                                if (((Dictionary<string, object>)node["agent"]).ContainsKey("caps")) { n.agentcaps = (int)((Dictionary<string, object>)node["agent"])["caps"]; } else { n.agentcaps = 0; }
                                            }
                                            else
                                            {
                                                n.agentid = -1;
                                                n.agentcaps = 0;
                                            }
                                            n.name = (string)node["name"];
                                            n.meshid = meshid;
                                            if (node.ContainsKey("mtype"))
                                            {
                                                if (node["mtype"].GetType() == typeof(string)) { n.mtype = int.Parse((string)node["mtype"]); }
                                                if (node["mtype"].GetType() == typeof(int)) { n.mtype = (int)node["mtype"]; }
                                            }
                                            if (node.ContainsKey("users")) { n.users = (string[])((ArrayList)node["users"]).ToArray(typeof(string)); } else { n.users = null; }
                                            if (node.ContainsKey("rdpport")) { n.rdpport = (int)node["rdpport"]; } else { n.rdpport = 3389; }
                                            if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; } else { n.conn = 0; }
                                            if (node.ContainsKey("icon")) { n.icon = (int)node["icon"]; }
                                            if (n.icon == 0) { n.icon = 1; }
                                            n.rights = 0;
                                            n.links = new Dictionary<string, ulong>();
                                            if (node.ContainsKey("links"))
                                            {
                                                Dictionary<string, object> links = ((Dictionary<string, object>)node["links"]);
                                                if (links.ContainsKey(userid))
                                                {
                                                    Dictionary<string, object> linksEx = ((Dictionary<string, object>)links[userid]);
                                                    if (linksEx.ContainsKey("rights")) { n.rights = (ulong)(int)linksEx["rights"]; }
                                                }
                                                foreach (string j in links.Keys)
                                                {
                                                    Dictionary<string, object> linksEx = ((Dictionary<string, object>)links[j]);
                                                    if (linksEx.ContainsKey("rights"))
                                                    {
                                                        n.links.Add(j, ulong.Parse(linksEx["rights"].ToString()));
                                                    }
                                                }
                                            }
                                            nodes2[n.nodeid] = n;
                                        }
                                        else
                                        {
                                            NodeClass n = new NodeClass();
                                            n.nodeid = nodeid;
                                            if (node.ContainsKey("agent"))
                                            {
                                                n.agentid = (int)((Dictionary<string, object>)node["agent"])["id"];
                                                if (((Dictionary<string, object>)node["agent"]).ContainsKey("caps")) { n.agentcaps = (int)((Dictionary<string, object>)node["agent"])["caps"]; } else { n.agentcaps = 0; }
                                            }
                                            else
                                            {
                                                n.agentid = -1;
                                                n.agentcaps = 0;
                                            }
                                            n.name = (string)node["name"];
                                            n.meshid = meshid;
                                            if (node.ContainsKey("host")) { n.host = (string)node["host"]; }

                                            if (node.ContainsKey("mtype"))
                                            {
                                                if (node["mtype"].GetType() == typeof(string)) { n.mtype = int.Parse((string)node["mtype"]); }
                                                if (node["mtype"].GetType() == typeof(int)) { n.mtype = (int)node["mtype"]; }
                                            }
                                            if (node.ContainsKey("users"))
                                            {
                                                if (node["users"].GetType() == typeof(ArrayList))
                                                {
                                                    n.users = (string[])((ArrayList)node["users"]).ToArray(typeof(string));
                                                }
                                                else if (node["users"].GetType() == typeof(Dictionary<string, object>))
                                                {
                                                    Dictionary<string, object> users = (Dictionary<string, object>)node["users"];
                                                    ArrayList users2 = new ArrayList();
                                                    foreach (string u in users.Keys) { if (users[u].GetType() == typeof(string)) { users2.Add((string)users[u]); } }
                                                    n.users = (string[])users2.ToArray(typeof(string));
                                                }
                                                else
                                                {
                                                    n.users = null;
                                                }
                                            }
                                            else
                                            {
                                                n.users = null;
                                            }
                                            if (node.ContainsKey("rdpport")) { n.rdpport = (int)node["rdpport"]; } else { n.rdpport = 3389; }
                                            if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; } else { n.conn = 0; }
                                            if (node.ContainsKey("icon")) { n.icon = (int)node["icon"]; }
                                            if (n.icon == 0) { n.icon = 1; }
                                            n.rights = 0;
                                            n.links = new Dictionary<string, ulong>();
                                            if (node.ContainsKey("links"))
                                            {
                                                Dictionary<string, object> links = ((Dictionary<string, object>)node["links"]);
                                                if (links.ContainsKey(userid))
                                                {
                                                    Dictionary<string, object> linksEx = ((Dictionary<string, object>)links[userid]);
                                                    if (linksEx.ContainsKey("rights")) { n.rights = (ulong)(int)linksEx["rights"]; }
                                                }
                                                foreach (string j in links.Keys)
                                                {
                                                    Dictionary<string, object> linksEx = ((Dictionary<string, object>)links[j]);
                                                    if (linksEx.ContainsKey("rights"))
                                                    {
                                                        n.links.Add(j, ulong.Parse(linksEx["rights"].ToString()));
                                                    }
                                                }
                                            }
                                            nodes2[n.nodeid] = n;
                                        }
                                    }
                                }
                            }
                            nodes = nodes2;
                            if ((onNodesChanged != null) && (nodes != null)) onNodesChanged(true);
                            break;
                        }
                    case "msg":
                        {
                            if (jsonAction.ContainsKey("type"))
                            {
                                string type = (string)jsonAction["type"];
                                if ((type == "getclip") && (jsonAction.ContainsKey("data")) && (jsonAction.ContainsKey("nodeid")))
                                {
                                    // We requested the remote clipboard
                                    string nodeid = (string)jsonAction["nodeid"];
                                    string clipData = (string)jsonAction["data"];
                                    if (onClipboardData != null) { onClipboardData(nodeid, clipData); }
                                }
                            }
                            break;
                        }
                    case "twoFactorCookie":
                        {
                            if (jsonAction.ContainsKey("cookie"))
                            {
                                if (jsonAction["cookie"] == null) return;
                                if (jsonAction["cookie"].GetType() != typeof(string)) return;
                                string cookie = null;
                                try { cookie = (string)jsonAction["cookie"]; } catch (Exception) { }
                                if ((cookie != null) && (onTwoFactorCookie != null)) { onTwoFactorCookie(cookie); }
                            }
                            break;
                        }
                    case "meshToolInfo":
                        {
                            if (onToolUpdate == null) return;
                            if (jsonAction.ContainsKey("hash") && jsonAction.ContainsKey("url"))
                            {
                                // MeshCentral Router hash on the server
                                string hash = (string)jsonAction["hash"];

                                // Hash our own executable
                                byte[] selfHash;
                                using (var sha384 = SHA384Managed.Create()) { using (var stream = File.OpenRead(System.Reflection.Assembly.GetEntryAssembly().Location)) { selfHash = sha384.ComputeHash(stream); } }
                                string selfExecutableHashHex = BitConverter.ToString(selfHash).Replace("-", string.Empty).ToLower();

                                // Get login key
                                string url = jsonAction["url"] + "&auth=" + authCookie;
                                if (url.StartsWith("*/")) { url = "https://" + wsurl.Authority + url.Substring(1); }
                                string loginkey = getValueFromQueryString(wsurl.Query, "key");
                                if (loginkey != null) { url += ("&key=" + loginkey); }

                                // Server TLS certificate hash
                                string serverhash = null;
                                if (jsonAction.ContainsKey("serverhash")) { serverhash = jsonAction["serverhash"].ToString(); }

                                // If the hashes don't match, event the tool update with URL
                                if (selfExecutableHashHex != hash) {
                                    if (debug) { try { File.AppendAllText("debug.log", "Self-executable hash mismatch, update available.\r\n"); } catch (Exception) { } }
                                    onToolUpdate((string)url, (string)jsonAction["hash"], (int)jsonAction["size"], serverhash);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            } catch (Exception ex)
            {
                if (debug) { try { File.AppendAllText("debug.log", ex.ToString() + "\r\n"); } catch (Exception) { } }
            }
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

        public delegate void onStateChangedHandler(int state);
        public event onStateChangedHandler onStateChanged;
        public void changeState(int newState) { if (constate != newState) { constate = newState; if (onStateChanged != null) { onStateChanged(constate); } } }

        private void changeStateEx(webSocketClient sender, webSocketClient.ConnectionStates newState)
        {
            if (newState == webSocketClient.ConnectionStates.Disconnected) {
                if (sender.failedTlsCert != null) { certHash = null; disconnectMsg = "cert"; disconnectCert = sender.failedTlsCert; }
                changeState(0);
            }
            if (newState == webSocketClient.ConnectionStates.Connecting) { changeState(1); }
            if (newState == webSocketClient.ConnectionStates.Connected) { }
        }

        public delegate void onNodeListChangedHandler(bool fullRefresh);
        public event onNodeListChangedHandler onNodesChanged;
        public delegate void onLoginTokenChangedHandler();
        public event onLoginTokenChangedHandler onLoginTokenChanged;
        public delegate void onClipboardDataHandler(string nodeid, string data);
        public event onClipboardDataHandler onClipboardData;
        public delegate void twoFactorCookieHandler(string cookie);
        public event twoFactorCookieHandler onTwoFactorCookie;
        public delegate void toolUpdateHandler(string url, string hash, int size, string serverhash);
        public event toolUpdateHandler onToolUpdate;

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

    }
}