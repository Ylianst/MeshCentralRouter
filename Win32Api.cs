/*
Copyright 2009-2021 Intel Corporation

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
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace MeshCentralRouter
{
    public class Win32Api
    {
        public const int WINHTTP_ACCESS_TYPE_DEFAULT_PROXY = 0;
        public const int WINHTTP_ACCESS_TYPE_NO_PROXY = 1;
        public const int WINHTTP_ACCESS_TYPE_NAMED_PROXY = 3;
        public const int WINHTTP_AUTOPROXY_AUTO_DETECT = 0x00000001;
        public const int WINHTTP_AUTOPROXY_CONFIG_URL = 0x00000002;
        public const int WINHTTP_AUTOPROXY_RUN_INPROCESS = 0x00010000;
        public const int WINHTTP_AUTOPROXY_RUN_OUTPROCESS_ONLY = 0x00020000;
        public const int WINHTTP_AUTO_DETECT_TYPE_DHCP = 0x00000001;
        public const int WINHTTP_AUTO_DETECT_TYPE_DNS_A = 0x00000002;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WINHTTP_AUTOPROXY_OPTIONS
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwFlags;
            [MarshalAs(UnmanagedType.U4)]
            public int dwAutoDetectFlags;
            public string lpszAutoConfigUrl;
            public IntPtr lpvReserved;
            [MarshalAs(UnmanagedType.U4)]
            public int dwReserved;
            public bool fAutoLoginIfChallenged;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WINHTTP_PROXY_INFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwAccessType;
            public string lpszProxy;
            public string lpszProxyBypass;
        }

        [DllImport("winhttp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool WinHttpGetProxyForUrl(IntPtr hSession, string lpcwszUrl, ref WINHTTP_AUTOPROXY_OPTIONS pAutoProxyOptions, ref WINHTTP_PROXY_INFO pProxyInfo);

        [DllImport("winhttp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr WinHttpOpen(string pwszUserAgent, int dwAccessType, IntPtr pwszProxyName, IntPtr pwszProxyBypass, int dwFlags);

        [DllImport("winhttp.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool WinHttpCloseHandle(IntPtr hInternet);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        public static Uri GetProxy(Uri url)
        {
            // Check if we need to use a HTTP proxy (Auto-proxy way)
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
                Object x = registryKey.GetValue("AutoConfigURL", null);
                if ((x != null) && (x.GetType() == typeof(string)))
                {
                    string proxyStr = GetProxyForUrlUsingPac("http" + ((url.Port == 80) ? "" : "s") + "://" + url.Host + ":" + url.Port, x.ToString());
                    return new Uri("http://" + proxyStr);
                }
            }
            catch (Exception) { }

            Uri proxyUri = null;
            try
            {
                // Check if we need to use a HTTP proxy (Normal way)
                proxyUri = System.Net.HttpWebRequest.GetSystemWebProxy().GetProxy(url);
                if ((url.Host.ToLower() == proxyUri.Host.ToLower()) && (url.Port == proxyUri.Port)) { return null; }
            }
            catch (Exception) { }

            return proxyUri;
        }

        private static string GetProxyForUrlUsingPac(string DestinationUrl, string PacUri)
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
    }
    
}