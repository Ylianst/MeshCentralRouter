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
using System.Web;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Net.Security;
using System.Windows.Forms;
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
        private xwebclient wc = null;
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
            if (debug || tlsdump) { try { File.AppendAllText("debug.log", "Connect-" + wsurl + "\r\n"); } catch (Exception) { } }
            wc.xdebug = debug;
            wc.xtlsdump = tlsdump;
            wc.xignoreCert = ignoreCert;
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
                wc.WriteStringWebSocket(cmd);
            }
        }

        public void refreshCookies()
        {
            if (wc != null) {
                if (debug) { try { File.AppendAllText("debug.log", "RefreshCookies\r\n"); } catch (Exception) { } }
                wc.WriteStringWebSocket("{\"action\":\"authcookie\"}");
                wc.WriteStringWebSocket("{\"action\":\"logincookie\"}");
            }
        }

        public void setRdpPort(NodeClass node, int port)
        {
            if (wc != null)
            {
                if (debug) { try { File.AppendAllText("debug.log", "SetRdpPort\r\n"); } catch (Exception) { } }
                wc.WriteStringWebSocket("{\"action\":\"changedevice\",\"nodeid\":\"" + node.nodeid + "\",\"rdpport\":" + port + "}");
            }
        }

        public void processServerData(string data)
        {
            if (debug) { try { File.AppendAllText("debug.log", "ServerData-" + data + "\r\n"); } catch (Exception) { } }

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
                        if (jsonAction.ContainsKey("email2fa")) { disconnectEmail2FA = (bool)jsonAction["email2fa"]; } else { disconnectEmail2FA = false; }
                        if (jsonAction.ContainsKey("email2fasent")) { disconnectEmail2FASent = (bool)jsonAction["email2fasent"]; } else { disconnectEmail2FASent = false; }
                        if (jsonAction.ContainsKey("sms2fa")) { disconnectSms2FA = (bool)jsonAction["sms2fa"]; } else { disconnectSms2FA = false; }
                        if (jsonAction.ContainsKey("sms2fasent")) { disconnectSms2FASent = (bool)jsonAction["sms2fasent"]; } else { disconnectSms2FASent = false; }
                        if (jsonAction.ContainsKey("twoFactorCookieDays") && (jsonAction["twoFactorCookieDays"].GetType() == typeof(int))) { twoFactorCookieDays = (int)jsonAction["twoFactorCookieDays"]; }
                        break;
                    }
                case "serverinfo":
                    {
                        wc.WriteStringWebSocket("{\"action\":\"usergroups\"}");
                        wc.WriteStringWebSocket("{\"action\":\"meshes\"}");
                        wc.WriteStringWebSocket("{\"action\":\"nodes\"}");
                        wc.WriteStringWebSocket("{\"action\":\"authcookie\"}");
                        wc.WriteStringWebSocket("{\"action\":\"logincookie\"}");
                        wc.WriteStringWebSocket("{\"action\":\"meshToolInfo\",\"name\":\"MeshCentralRouter\"}");
                        break;
                    }
                case "authcookie":
                    {
                        authCookie = jsonAction["cookie"].ToString();
                        rauthCookie = jsonAction["rcookie"].ToString();
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
                        Dictionary<string, object> userinfo = (Dictionary<string, object>)jsonAction["userinfo"];
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
                                    wc.WriteStringWebSocket("{\"action\":\"nodes\"}");
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
                                                n.agentcaps = (int)((Dictionary<string, object>)node["agent"])["caps"];
                                            }
                                            if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; }
                                            n.icon = (int)node["icon"];
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

                                            if (rights == 0) {
                                                // We have no rights to this device, remove it
                                                nodes.Remove(n.nodeid);
                                            } else {
                                                // Update the node
                                                nodes[n.nodeid] = n;
                                            }
                                        }
                                        if ((onNodesChanged != null) && (nodes != null)) onNodesChanged(false);
                                    }
                                    else
                                    {
                                        wc.WriteStringWebSocket("{\"action\":\"nodes\"}");
                                    }
                                    break;
                                }
                            case "nodeconnect":
                                {
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
                                    wc.WriteStringWebSocket("{\"action\":\"usergroups\"}");
                                    wc.WriteStringWebSocket("{\"action\":\"nodes\"}");
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
                                        if (node.ContainsKey("agent")) {
                                            n.agentid = (int)((Dictionary<string, object>)node["agent"])["id"];
                                            n.agentcaps = (int)((Dictionary<string, object>)node["agent"])["caps"];
                                        } else {
                                            n.agentid = -1;
                                            n.agentcaps = 0;
                                        }
                                        n.name = (string)node["name"];
                                        n.meshid = meshid;
                                        if (node.ContainsKey("users")) { n.users = (string[])((ArrayList)node["users"]).ToArray(typeof(string)); } else { n.users = null; }
                                        if (node.ContainsKey("rdpport")) { n.rdpport = (int)node["rdpport"]; } else { n.rdpport = 3389; }
                                        if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; } else { n.conn = 0; }
                                        if (node.ContainsKey("icon")) { n.icon = (int)node["icon"]; }
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
                                        if (node.ContainsKey("agent")) {
                                            n.agentid = (int)((Dictionary<string, object>)node["agent"])["id"];
                                            n.agentcaps = (int)((Dictionary<string, object>)node["agent"])["caps"];
                                        } else {
                                            n.agentid = -1;
                                            n.agentcaps = 0;
                                        }
                                        n.name = (string)node["name"];
                                        n.meshid = meshid;

                                        if (node.ContainsKey("users")) { n.users = (string[])((ArrayList)node["users"]).ToArray(typeof(string)); } else { n.users = null; }
                                        if (node.ContainsKey("rdpport")) { n.rdpport = (int)node["rdpport"]; } else { n.rdpport = 3389; }
                                        if (node.ContainsKey("conn")) { n.conn = (int)node["conn"]; } else { n.conn = 0; }
                                        if (node.ContainsKey("icon")) { n.icon = (int)node["icon"]; }
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
                            string cookie = (string)jsonAction["cookie"];
                            if (onTwoFactorCookie != null) { onTwoFactorCookie(cookie); }
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
                            string loginkey = getValueFromQueryString(wsurl.Query, "key");
                            if (loginkey != null) { url += ("&key=" + loginkey); }

                            // Server TLS certificate hash
                            string serverhash = null;
                            if (jsonAction.ContainsKey("serverhash")) { serverhash = jsonAction["serverhash"].ToString(); }

                            // If the hashes don't match, event the tool update with URL
                            if (selfExecutableHashHex != hash) { onToolUpdate((string)url, (string)jsonAction["hash"], (int)jsonAction["size"], serverhash); }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
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

        public class xwebclient : IDisposable
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
            public bool xtlsdump = false;
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
            public void TlsDump(string direction, byte[] data, int offset, int len) { if (xtlsdump) { try { File.AppendAllText("debug.log", direction + ": " + BitConverter.ToString(data, offset, len).Replace("-", string.Empty) + "\r\n"); } catch (Exception) { } } }

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
                    Debug("Connecting with proxy in use: " + proxyUri.ToString());
                    wsclient.BeginConnect(proxyUri.Host, proxyUri.Port, new AsyncCallback(OnConnectSink), this);
                }
                else
                {
                    // No proxy in use
                    proxyInUse = false;
                    wsclient = new TcpClient();
                    Debug("Connecting without proxy");
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
                    TlsDump("OutRaw", proxyRequestBuf, 0, proxyRequestBuf.Length);
                    try { wsrawstream.Write(proxyRequestBuf, 0, proxyRequestBuf.Length); } catch (Exception ex) { Debug(ex.ToString()); }
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

                TlsDump("InRaw", readBuffer, 0, readBufferLen);

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
                        wsstream.BeginAuthenticateAsClient(url.Host, null, System.Security.Authentication.SslProtocols.Default, false, new AsyncCallback(OnTlsSetupSink), this);
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
                    if (ex.InnerException != null) {
                        MessageBox.Show(ex.Message + ", Inner: " + ex.InnerException.ToString(), "MeshCentral Router");
                    } else {
                        MessageBox.Show(ex.Message, "MeshCentral Router");
                    }
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
                try { wsstream.Write(UTF8Encoding.UTF8.GetBytes(header)); } catch (Exception ex) { Debug(ex.ToString()); }

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
                TlsDump("In", readBuffer, 0, len);

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
                //try { parent.processServerData(UTF8Encoding.UTF8.GetString(data, offset, len)); } catch (Exception ex) { }
                parent.processServerData(UTF8Encoding.UTF8.GetString(data, offset, len));
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
                parent.certHash = GetMeshKeyHash(certificate);
                Debug("Verify cert: " + parent.certHash);
                if (xignoreCert) return true;
                if (chain.Build(new X509Certificate2(certificate)) == true) return true;

                // Check that the remote certificate is the expected one
                if ((parent.okCertHash != null) && (parent.okCertHash == certificate.GetCertHashString())) return true;

                // Check that the remote certificate is the expected one
                if ((parent.okCertHash2 != null) && ((parent.okCertHash2 == GetMeshKeyHash(certificate)) || (parent.okCertHash2 == GetMeshCertHash(certificate)))) { return true; }

                parent.certHash = null;
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
                    TlsDump("Out", buf, 2, len + 2);
                    try { wsstream.Write(buf, 2, len + 2); } catch (Exception ex) { Debug(ex.ToString()); }
                }
                else
                {
                    // Large fragment
                    buf[0] = 130; // Fragment op code (129 = text, 130 = binary)
                    buf[1] = 126;
                    buf[2] = (byte)((len >> 8) & 0xFF);
                    buf[3] = (byte)(len & 0xFF);
                    //try { wsstream.BeginWrite(buf, 0, len + 4, new AsyncCallback(WriteWebSocketAsyncDone), args); } catch (Exception) { Dispose(); return; }
                    TlsDump("Out", buf, 0, len + 4);
                    try { wsstream.Write(buf, 0, len + 4); } catch (Exception ex) { Debug(ex.ToString()); }
                }
            }

        }

    }
}