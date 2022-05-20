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
using System.Security;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Collections;
using System.Net.NetworkInformation;

namespace MeshCentralRouter
{

    /// <summary>
    /// This class is used to perform mesh node discovery on the local network. It is used to listen of node advertizements and to multicast a "PING" to find mesh
    /// nodes on the network. This class works with both IPv4 and IPv6 and must work regardless of the number of network interfaces on this computer.
    /// This class also monitors local adapter changes and events them. This is useful to immidiately remove nodes that are not long accessible.
    /// </summary>
    public sealed class MeshDiscovery
    {
        private static IPAddress MulticastV4Addr = IPAddress.Parse("239.255.255.235");
        private static IPAddress MulticastV6Addr = IPAddress.Parse("FF02:0:0:0:0:0:0:FE");
        private static byte[] PingPacket = UTF8Encoding.UTF8.GetBytes("MeshServerScan");

        public delegate void NotifyHandler(MeshDiscovery sender, IPEndPoint source, IPEndPoint local, string agentCertHash, string url, string name, string info);
        public event NotifyHandler OnNotify;

        public delegate void InterfaceChangedHandler(MeshDiscovery sender, bool added, IPAddress localaddress);
        public event InterfaceChangedHandler OnInterfaceChanged;

        private Hashtable sessions = new Hashtable();
        private Hashtable xsessions = new Hashtable();
        private ArrayList localinterfaces = new ArrayList();
        private IPEndPoint MulticastV4EndPoint;
        private IPEndPoint MulticastV6EndPoint;
        private UdpClient GeneralSessionV4 = null; // Used for other things like node messages, etc.
        private UdpClient GeneralSessionV6 = null; // Used for other things like node messages, etc.

        private const int SIO_UDP_CONNRESET = -1744830452;
        private byte[] inValue = new byte[] { 0, 0, 0, 0 };     // == false
        private byte[] outValue = new byte[] { 0, 0, 0, 0 };    // initialize to 0

        public MeshDiscovery()
        {
            MulticastV4EndPoint = new IPEndPoint(MulticastV4Addr, 16989);
            MulticastV6EndPoint = new IPEndPoint(MulticastV6Addr, 16989);
            SetupSessions();
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedSink);

            GeneralSessionV4 = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            GeneralSessionV4.Client.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
            GeneralSessionV4.BeginReceive(new AsyncCallback(OnReceiveSink), GeneralSessionV4);
            try
            {
                GeneralSessionV6 = new UdpClient(new IPEndPoint(IPAddress.IPv6Any, 0));
                GeneralSessionV6.Client.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
                GeneralSessionV6.BeginReceive(new AsyncCallback(OnReceiveSink), GeneralSessionV6);
            }
            catch (SocketException) { GeneralSessionV6 = null; } // This section will fail on Windows XP because it does not support IPv6
        }

        public void Dispose()
        {
            NetworkChange.NetworkAddressChanged -= new NetworkAddressChangedEventHandler(AddressChangedSink);
            CloseSessions();
            
            GeneralSessionV4.Close();
            GeneralSessionV4 = null;
            if (GeneralSessionV6 != null)
            {
                GeneralSessionV6.Close();
                GeneralSessionV6 = null;
            }
        }

        /// <summary>
        /// Called then the list of local network adapters has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressChangedSink(object sender, EventArgs e)
        {
            //DebugEventViewerForm.LogEvent(this, DebugEventViewerForm.EventType.Warning, "Network interface change detected.");
            SetupSessions();
        }

        /// <summary>
        /// Creates all of the UDP client sessions for each local IP address. This method should be called when the list of local network adapters changes on the
        /// local computer.
        /// </summary>
        private void SetupSessions()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            if (adapters == null) CloseSessions();
            ArrayList tif = new ArrayList(sessions.Keys);

            foreach (NetworkInterface adapter in adapters)
            {
                // We first filter out the local adapters that are not interesting
                if (adapter.SupportsMulticast == false) continue;
                if (adapter.OperationalStatus != OperationalStatus.Up) continue;
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                
                // Out of each remaining adapter, we look at their IP addresses and create a UDP client for each one.
                foreach (System.Net.NetworkInformation.UnicastIPAddressInformation uif in adapter.GetIPProperties().UnicastAddresses)
                {
                    IPAddress addr = uif.Address;

                    if (addr.AddressFamily == AddressFamily.InterNetwork || addr.ScopeId != 0)
                    {
                        tif.Remove(addr);
                        if (sessions.ContainsKey(addr) == false)
                        {
                            // Event this new interface
                            if (OnInterfaceChanged != null) OnInterfaceChanged(this, true, addr);

                            // If this local address is IPv4, setup an IPv4 UDP client.
                            if (addr.AddressFamily == AddressFamily.InterNetwork)
                            {
                                UdpClient session = new UdpClient(AddressFamily.InterNetwork);
                                session.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                session.ExclusiveAddressUse = false;
                                session.Client.Bind(new IPEndPoint(addr, 0));
                                session.EnableBroadcast = true;
                                session.JoinMulticastGroup(MulticastV4Addr, addr);
                                session.Client.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
                                session.BeginReceive(new AsyncCallback(OnReceiveSink), session);
                                sessions[addr] = session;
                                
                                session = new UdpClient(AddressFamily.InterNetwork);
                                session.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                session.ExclusiveAddressUse = false;
                                session.Client.Bind(new IPEndPoint(addr, 16990));
                                session.EnableBroadcast = true;
                                session.JoinMulticastGroup(MulticastV4Addr, addr);
                                session.Client.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
                                session.BeginReceive(new AsyncCallback(OnReceiveSink), session);
                                xsessions[addr] = session;
                            }

                            // If this local address is IPv6, setup an IPv6 UDP client.
                            if (addr.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                UdpClient session = new UdpClient(AddressFamily.InterNetworkV6);
                                session.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                session.ExclusiveAddressUse = false;
                                session.Client.Bind(new IPEndPoint(addr, 0));
                                session.EnableBroadcast = true;
                                session.JoinMulticastGroup((int)addr.ScopeId, MulticastV6Addr);
                                session.Client.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
                                session.BeginReceive(new AsyncCallback(OnReceiveSink), session);
                                sessions[addr] = session;

                                session = new UdpClient(AddressFamily.InterNetworkV6);
                                session.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                session.ExclusiveAddressUse = false;
                                session.Client.Bind(new IPEndPoint(addr, 16990));
                                session.EnableBroadcast = true;
                                session.JoinMulticastGroup((int)addr.ScopeId, MulticastV6Addr);
                                session.Client.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
                                session.BeginReceive(new AsyncCallback(OnReceiveSink), session);
                                xsessions[addr] = session;
                            }
                        }
                    }
                }
            }

            // Remove old sessions
            if (tif.Count > 0)
            {
                foreach (IPAddress a in tif)
                {
                    UdpClient c = (UdpClient)sessions[a];
                    c.Close();
                    sessions.Remove(a);
                    if (OnInterfaceChanged != null) OnInterfaceChanged(this, false, a);
                }
            }
        }

        /// <summary>
        /// Closes all of the UDP client sessions.
        /// </summary>
        private void CloseSessions()
        {
            ArrayList l = new ArrayList(sessions.Values);
            foreach (UdpClient session in l)
            {
                IPAddress a = ((IPEndPoint)session.Client.LocalEndPoint).Address;
                session.Close();
                sessions.Remove(a);
                if (OnInterfaceChanged != null) OnInterfaceChanged(this, false, a);
            }
            sessions.Clear();

            l = new ArrayList(xsessions.Values);
            foreach (UdpClient session in l)
            {
                IPAddress a = ((IPEndPoint)session.Client.LocalEndPoint).Address;
                session.Close();
                sessions.Remove(a);
                if (OnInterfaceChanged != null) OnInterfaceChanged(this, false, a);
            }
            xsessions.Clear();
        }

        /// <summary>
        /// Called by the UDP clients when data is received or when the client is closed.
        /// </summary>
        /// <param name="result">AsyncResult parameter</param>
        private void OnReceiveSink(IAsyncResult result)
        {
            byte[] buffer;
            IPEndPoint ep = null;
            UdpClient session = (UdpClient)result.AsyncState;

            try
            {
                buffer = session.EndReceive(result, ref ep);
                try
                {
                    // We received some data, decode it
                    string[] values = UTF8Encoding.UTF8.GetString(buffer).Split('|');
                    if ((values.Length >= 3) && (values[0] == "MeshCentral2"))
                    {
                        Uri url = new Uri(values[2].Replace("%s", ep.Address.ToString()).Replace("wss://", "https://").Replace("ws://", "http://").Replace("/agent.ashx", "/"));
                        string agentCertHash = values[1];
                        string serverName = null;
                        string serverInfo = null;
                        if (values.Length >= 4) { serverName = values[3]; }
                        if (values.Length >= 5) { serverInfo = values[4]; }
                        OnNotify(this, ep, (IPEndPoint)(session.Client.LocalEndPoint), agentCertHash, url.ToString(), serverName, serverInfo);

                        // Send a targeted scan to this server to get more information
                        if (values.Length == 3) { session.Send(PingPacket, PingPacket.Length, ep); session.Send(PingPacket, PingPacket.Length, ep); }
                    }
                }
                catch (Exception) { }
                session.BeginReceive(new AsyncCallback(OnReceiveSink), session);
            }
            catch (Exception)
            {
                // If the client is disposed, it will throw another exception.
                try
                {
                    if (session != null && session.Client != null)
                    {
                        IPAddress a = ((IPEndPoint)session.Client.LocalEndPoint).Address;
                        sessions.Remove(a);
                        if (OnInterfaceChanged != null) OnInterfaceChanged(this, false, a);
                    }
                }
                catch (Exception) { }
            }
        }

        /*
        internal void SendBlockRequest(TargetTrak tt, string[] requests)
        {
            // PB_SYNCREQUEST
            MemoryStream mem = new MemoryStream();
            mem.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)12)), 0, 2);  // Write PB_SYNCREQUEST
            mem.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(4 + (32 * requests.Length)))), 0, 2);  // Write length of header + (32 * requests)
            foreach (string s in requests)
            {
                byte[] buf = MeshUtils.HexToBytes(s);
                mem.Write(buf, 0, buf.Length);
            }
            byte[] packet = MeshDiscovery.util_cipher(tt.UdpSessionKey, 0, MeshUtils.HexToBytes(tt.Node.NodeIdHex), mem.ToArray(), true, tt.ConsoleId);
            GeneralUnicast(packet, new IPEndPoint(tt.Target, 16990));
        }
        */

        /// <summary>
        /// This method sends out a multicast "PING" on all local adapters in both IPv4 and IPv6.
        /// </summary>
        public void MulticastPing()
        {
            //DebugEventViewerForm.LogEvent(this, DebugEventViewerForm.EventType.Information, "Performing full multicast ping.");
            foreach (UdpClient session in sessions.Values) MulticastPing(session);
        }

        /// <summary>
        /// Performs a multicast ping on a single UDP session.
        /// </summary>
        /// <param name="session">The UDP client on which to perform the multicast</param>
        private void MulticastPing(UdpClient session)
        {
            try
            {
                IPAddress address = ((IPEndPoint)session.Client.LocalEndPoint).Address;
                if (session.Client.AddressFamily == AddressFamily.InterNetwork)
                {
                    session.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, address.GetAddressBytes());
                    session.Send(PingPacket, PingPacket.Length, MulticastV4EndPoint);
                }
                else if (session.Client.AddressFamily == AddressFamily.InterNetworkV6 && address.ScopeId != 0)
                {
                    session.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, BitConverter.GetBytes((int)address.ScopeId));
                    session.Send(PingPacket, PingPacket.Length, MulticastV6EndPoint);
                }
            }
            catch (SocketException) { }
        }

        /// <summary>
        /// This method is used to send out unicast UDP packets.
        /// </summary>
        /// <param name="buffer">The buffer to send out as unicast</param>
        /// <param name="local">The local IP address to use</param>
        /// <param name="remote">The remote IP address that this packet will be sent to</param>
        public void UnicastData(byte[] buffer, IPAddress local, IPEndPoint remote)
        {
            UdpClient session = (UdpClient)sessions[local];
            if (session == null) return;
            try
            {
                session.Send(buffer, buffer.Length, remote);
            }
            catch (SocketException) { }
        }

        public void GeneralUnicast(byte[] buffer, IPEndPoint remote)
        {
            try
            {
                if (remote.AddressFamily == AddressFamily.InterNetwork && GeneralSessionV4 != null) GeneralSessionV4.Send(buffer, buffer.Length, remote);
                if (remote.AddressFamily == AddressFamily.InterNetworkV6 && GeneralSessionV6 != null) GeneralSessionV6.Send(buffer, buffer.Length, remote);
            }
            catch (SocketException) { }
        }

    }

}
