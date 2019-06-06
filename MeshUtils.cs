/*
Copyright 2009-2017 Intel Corporation

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
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MeshMiniRouterTool
{
    class MeshUtils
    {
        public static RNGCryptoServiceProvider CryptoRandom = new RNGCryptoServiceProvider();

        public static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1 == null && a2 == null) return true;
            if (a1 != null && a2 == null) return false;
            if (a1 == null && a2 != null) return false;
            if (a1.Length != a2.Length) return false;
            for (int i = 0; i < a1.Length; i++) if (a1[i] != a2[i]) return false;
            return true;
        }

        private static char[] hexlookup = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public static string BytesToHex(byte[] data)
        {
            if (data == null) return "";
            int i = 0, p = 0, l = data.Length;
            char[] c = new char[l * 2];
            byte d;
            while (i < l)
            {
                d = data[i++];
                c[p++] = hexlookup[d / 0x10];
                c[p++] = hexlookup[d % 0x10];
            }
            return new string(c, 0, c.Length);
        }

        public static string BytesToHex(byte[] data, int spacing)
        {
            int i = 0, p = 0, l = data.Length;
            char[] c = new char[(l * 2) + (l / spacing)];
            byte d;
            while (i < l)
            {
                d = data[i++];
                c[p++] = hexlookup[d / 0x10];
                c[p++] = hexlookup[d % 0x10];
                if ((i % spacing) == 0) c[p++] = ' ';
            }
            return new string(c, 0, c.Length);
        }

        public static byte[] HexToBytes(string hex)
        {
            if (hex == null) return null;
            hex = hex.ToUpper();
            bool firstbyte = true;
            byte v = 0;
            ArrayList output = new ArrayList();
            foreach (char c in hex)
            {
                if (c >= '0' && c <= '9') v += (byte)(c - '0');
                else if (c >= 'A' && c <= 'F') v += (byte)((c - 'A') + 10);
                else continue;

                if (firstbyte)
                {
                    v = (byte)(v * 16);
                    firstbyte = false;
                }
                else
                {
                    output.Add(v);
                    v = 0;
                    firstbyte = true;
                }
            }
            return (byte[])output.ToArray(typeof(byte));
        }

        public class HttpRoutingCookie
        {
            public byte version;
            public string username;
            public byte[] nodeid;
            public IPAddress address;
            public ushort port;
            public bool tls;
            public bool oob;
            public string hostname;
            public long ticks;

            public HttpRoutingCookie() { version = 1; }
            public override string ToString() { return string.Format("{0}:{1}:{2}:{3}", version, username, hostname, port.ToString()); }
            public string ToLongString()
            {
                DateTime t1 = new DateTime(ticks);
                DateTime t2 = DateTime.UtcNow;

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version: {0}\r\n", version);
                sb.AppendFormat("Username: {0}\r\n", username);
                sb.AppendFormat("NodeId: {0}\r\n", MeshUtils.BytesToHex(nodeid));
                if (address != null) sb.AppendFormat("Address: {0}\r\n", address.ToString()); else sb.AppendFormat("Address: NULL\r\n");
                sb.AppendFormat("Port: {0}\r\n", port);
                sb.AppendFormat("TLS: {0}\r\n", tls);
                sb.AppendFormat("OOB: {0}\r\n", oob);
                sb.AppendFormat("Hostname: {0}\r\n", hostname);
                sb.AppendFormat("Time: {0}\r\n", t1.ToString());
                sb.AppendFormat("Now: {0}\r\n", t2.ToString());
                return sb.ToString();
            }
        }

        public static string EncodeHttpRoutingCookie(byte[] key, HttpRoutingCookie cookie)
        {
            if (key == null || key.Length != 50 || cookie.username == null || cookie.nodeid == null || cookie.nodeid.Length != 32 || cookie.hostname == null) return null;
            byte[] user = UTF8Encoding.UTF8.GetBytes(cookie.username);
            byte[] host = UTF8Encoding.UTF8.GetBytes(cookie.hostname);
            if (user.Length > 250 || host.Length > 250) return null;
            byte[] ticks = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

            byte flags = 0;
            if (cookie.tls) flags += 1;
            if (cookie.oob) flags += 2;

            BinaryWriter bw = MeshUtils.GetBinaryWriter();

            if (cookie.version == 1) // Automatic routing cookie
            {
                bw.Write((byte)1);                                                                                // Cookie version
                bw.Write(cookie.nodeid, 0, 32);                                                             // Target nodeid
                bw.Write(IPAddress.HostToNetworkOrder((short)(cookie.port)));                               // Target port
                bw.Write(flags);                                                                            // Flags (Use TLS/OOB)
                bw.Write(ticks, 0, 8);                                                                      // Current time
                bw.Write((byte)user.Length);                                                                // Username length
                bw.Write(user, 0, user.Length);                                                             // Username
                bw.Write((byte)host.Length);                                                                // Hostname length
                bw.Write(host, 0, host.Length);                                                             // Hostname
            }
            else if (cookie.version == 2 && cookie.address != null && cookie.address.AddressFamily == AddressFamily.InterNetwork) // IPv4 Direct routing cookie
            {
                bw.Write((byte)2);                                                                                // Cookie version
                bw.Write(cookie.nodeid, 0, 32);                                                             // Relay nodeid
                bw.Write(cookie.address.GetAddressBytes(), 0, 4);                                           // Target IPv4 address
                bw.Write(IPAddress.HostToNetworkOrder((short)(cookie.port)));                               // Target port
                bw.Write(flags);                                                                            // Flags (Use TLS/OOB)
                bw.Write(ticks, 0, 8);                                                                      // Current time
                bw.Write((byte)user.Length);                                                                // Username length
                bw.Write(user, 0, user.Length);                                                             // Username
                bw.Write((byte)host.Length);                                                                // Hostname length
                bw.Write(host, 0, host.Length);                                                             // Hostname
            }

            string r = EncodeHttpRoutingCookieEx(key, ((MemoryStream)bw.BaseStream).ToArray());
            MeshUtils.RecycleBinaryWriter(bw);
            return r;
        }

        public static HttpRoutingCookie DecodeHttpRoutingCookie(byte[] key, string msg)
        {
            byte[] data = DecodeHttpRoutingCookieEx(key, msg);
            if (data == null) return null;
            HttpRoutingCookie cookie = new HttpRoutingCookie();
            try
            {

                if (data[0] == 1) // Automatic routing cookie
                {
                    cookie.version = 1;
                    cookie.nodeid = new byte[32];
                    Array.Copy(data, 1, cookie.nodeid, 0, 32);                                                       // Target nodeid
                    cookie.port = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, 33));      // Target port
                    cookie.tls = ((data[35] & 0x01) != 0);                                                           // Use TLS
                    cookie.oob = ((data[35] & 0x02) != 0);                                                           // Use OOB
                    cookie.ticks = BitConverter.ToInt64(data, 36);                                                   // Time Ticks
                    byte[] user = new byte[data[44]];                                                                // Username length
                    Array.Copy(data, 45, user, 0, user.Length);                                                      // Username
                    cookie.username = UTF8Encoding.UTF8.GetString(user);
                    byte[] host = new byte[data[45 + user.Length]];                                                  // Hostname length
                    Array.Copy(data, 46 + user.Length, host, 0, host.Length);                                        // Hostname
                    cookie.hostname = UTF8Encoding.UTF8.GetString(host);
                }
                else if (data[0] == 2) // IPv4 Direct routing cookie
                {
                    cookie.version = 2;
                    cookie.nodeid = new byte[32];
                    Array.Copy(data, 1, cookie.nodeid, 0, 32);                                                       // Relay nodeid
                    byte[] addr = new byte[4];
                    Array.Copy(data, 33, addr, 0, 4);                                                                // Target address
                    cookie.address = new IPAddress(addr);
                    cookie.port = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, 37));      // Target port
                    cookie.tls = ((data[39] & 0x01) != 0);                                                           // Use TLS
                    cookie.oob = ((data[39] & 0x02) != 0);                                                           // Use OOB
                    cookie.ticks = BitConverter.ToInt64(data, 40);                                                   // Time Ticks
                    byte[] user = new byte[data[48]];                                                                // Username length
                    Array.Copy(data, 49, user, 0, user.Length);                                                      // Username
                    cookie.username = UTF8Encoding.UTF8.GetString(user);
                    byte[] host = new byte[data[49 + user.Length]];                                                  // Hostname length
                    Array.Copy(data, 50 + user.Length, host, 0, host.Length);                                        // Hostname
                    cookie.hostname = UTF8Encoding.UTF8.GetString(host);
                }

                // Check clock
                DateTime t1 = new DateTime(cookie.ticks);
                DateTime t2 = DateTime.UtcNow;
                if (t1 > t2.AddMinutes(3) || t1.AddMinutes(60) < t2) return null;
            }
            catch (Exception) { return null; }

            return cookie;
        }

        private static string EncodeHttpRoutingCookieEx(byte[] key, byte[] msg)
        {
            if (key == null || key.Length != 50 || msg == null) return null;
            MemoryStream mem2 = MeshUtils.GetMemoryStream();

            // Split the key
            //ushort KeyId = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(key, 0));
            byte[] key1 = new byte[32]; // HMAC
            byte[] key2 = new byte[16]; // AES
            Array.Copy(key, 2, key1, 0, 32);
            Array.Copy(key, 34, key2, 0, 16);

            // Select an IV
            byte[] iv = new byte[16];
            CryptoRandom.GetBytes(iv);

            // Setup and perform HMAC-SHA256
            using (HMACSHA256 hmac = new HMACSHA256(key1))
            {
                byte[] hmacresult = hmac.ComputeHash(msg);
                mem2.Write(hmacresult, 0, hmacresult.Length);
                mem2.Write(msg, 0, msg.Length);
                msg = mem2.ToArray();
                mem2.SetLength(0);
            }

            // Setup AES128-CBC
            byte[] buf;
            using (Rijndael rij = new RijndaelManaged())
            {
                rij.KeySize = 128;
                rij.Mode = CipherMode.CBC;
                rij.Padding = PaddingMode.PKCS7;
                rij.Key = key2;
                rij.IV = iv;

                // Perform AES128 encrypt
                using (MemoryStream mem = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(mem, rij.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(msg, 0, msg.Length);
                        cs.FlushFinalBlock();
                        buf = mem.ToArray();
                    }
                }
            }

            // Perform formatting
            mem2.Write(key, 0, 2); // Write the key identifier
            mem2.Write(iv, 0, 16);
            mem2.Write(buf, 0, buf.Length);
            string r = "MRC" + UrlEscapeBase64(Convert.ToBase64String(mem2.ToArray()));
            MeshUtils.RecycleMemoryStream(mem2);
            return r;
        }

        private static byte[] DecodeHttpRoutingCookieEx(byte[] key, string msg)
        {
            byte[] resulting_msg = null;
            try
            {
                if (key == null || key.Length != 50 || msg == null || msg.Length < 5 || msg.StartsWith("MRC") == false) return null;
                msg = UrlUnEscapeBase64(msg);

                // Split the key
                ushort KeyId = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(key, 0));
                byte[] key1 = new byte[32]; // HMAC
                byte[] key2 = new byte[16]; // AES
                Array.Copy(key, 2, key1, 0, 32);
                Array.Copy(key, 34, key2, 0, 16);

                // Decode IV
                byte[] buf2 = null;
                try { buf2 = Convert.FromBase64String(msg.Substring(3)); }
                catch (Exception) { }
                if (buf2 == null || buf2.Length < 20) return null;
                ushort KeyId2 = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buf2, 0));
                if (KeyId != KeyId2) return null;
                byte[] iv = new byte[16];
                Array.Copy(buf2, 2, iv, 0, 16);
                byte[] buf = new byte[buf2.Length - 18];
                Array.Copy(buf2, 18, buf, 0, buf.Length);

                // Setup and perform AES128-CBC
                byte[] decoded_msg = null;
                using (Rijndael rij = new RijndaelManaged())
                {
                    rij.KeySize = 128;
                    rij.Mode = CipherMode.CBC;
                    rij.Padding = PaddingMode.PKCS7;
                    rij.Key = key2;
                    rij.IV = iv;

                    // Perform decrypt
                    using (CryptoStream cs = new CryptoStream(new MemoryStream(buf), rij.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] cleanmem = new byte[msg.Length]; // Worst case possible
                        int clearlen = cs.Read(cleanmem, 0, msg.Length);
                        if (clearlen < 16) return null;
                        decoded_msg = new byte[clearlen];
                        Array.Copy(cleanmem, 0, decoded_msg, 0, clearlen);
                    }
                }

                // Setup and perform HMAC-SHA256
                using (HMACSHA256 hmac = new HMACSHA256(key1))
                {
                    byte[] hmacresult = hmac.ComputeHash(decoded_msg, 32, decoded_msg.Length - 32);
                    byte[] hmacexpected = new byte[32];
                    Array.Copy(decoded_msg, 0, hmacexpected, 0, 32);
                    if (MeshUtils.ByteArrayCompare(hmacresult, hmacexpected) == false) return null;
                    resulting_msg = new byte[decoded_msg.Length - 32];
                    Array.Copy(decoded_msg, 32, resulting_msg, 0, decoded_msg.Length - 32);
                }
            }
            catch (Exception) { return null; }

            return resulting_msg;
        }

        public static string UrlEscapeBase64(string base64str)
        {
            char[] t = base64str.ToCharArray();
            for (int i = 0; i < t.Length; i++)
            {
                switch (t[i])
                {
                    case '+': t[i] = '@'; break;
                    case '/': t[i] = '$'; break;
                }
            }
            return new string(t);
        }

        public static string UrlUnEscapeBase64(string base64str)
        {
            char[] t = base64str.ToCharArray();
            for (int i = 0; i < t.Length; i++)
            {
                switch (t[i])
                {
                    case '@': t[i] = '+'; break;
                    case '$': t[i] = '/'; break;
                }
            }
            return new string(t);
        }

        // BinaryWriter Object Recycling System
        private static Stack<BinaryWriter> BinaryWriteRecycleList = new Stack<BinaryWriter>();
        public static BinaryWriter GetBinaryWriter() { lock (BinaryWriteRecycleList) { return (BinaryWriteRecycleList.Count == 0) ? new BinaryWriter(new MemoryStream(32000), Encoding.UTF8) : BinaryWriteRecycleList.Pop(); } }
        public static void RecycleBinaryWriter(BinaryWriter obj) { lock (BinaryWriteRecycleList) { ((MemoryStream)obj.BaseStream).SetLength(0); BinaryWriteRecycleList.Push(obj); } }

        // MemoryStream Object Recycling System
        private static Stack<MemoryStream> MemoryStreamRecycleList = new Stack<MemoryStream>();
        public static MemoryStream GetMemoryStream() { lock (MemoryStreamRecycleList) { return (MemoryStreamRecycleList.Count == 0) ? new MemoryStream(32000) : MemoryStreamRecycleList.Pop(); } }
        public static void RecycleMemoryStream(MemoryStream obj) { lock (MemoryStreamRecycleList) { obj.SetLength(0); MemoryStreamRecycleList.Push(obj); } }

        // StringBuilder Object Recycling System
        private static Stack<StringBuilder> StringBuilderRecycleList = new Stack<StringBuilder>();
        public static StringBuilder GetStringBuilder() { lock (StringBuilderRecycleList) { return (StringBuilderRecycleList.Count == 0) ? new StringBuilder(16000) : StringBuilderRecycleList.Pop(); } }
        public static void RecycleStringBuilder(StringBuilder obj) { lock (StringBuilderRecycleList) { obj.Length = 0; StringBuilderRecycleList.Push(obj); } }

    }
}
