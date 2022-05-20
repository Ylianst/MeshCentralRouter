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

using Microsoft.Win32;
using System.Collections.Generic;
using System;

namespace MeshCentralRouter
{
    public static class Settings
    {
        public static void SetRegValue(string name, string value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value); } catch (Exception) { }
        }
        public static void SetRegValue(string name, bool value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value.ToString()); } catch (Exception) { }
        }
        public static void SetRegValue(string name, int value)
        {
            try { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value.ToString()); } catch (Exception) { }
        }
        /// <summary>
        /// This function querys the registry. If the key is found it returns the value as a string
        /// </summary>
        /// <param name="name">Keyname</param>
        /// <param name="value">Return on fail</param>
        /// <returns></returns>
        public static string GetRegValue(string name, string value)
        {
            try {
                String v = (String)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", name, value);
                if (v == null) return value;
                return v.ToString();
            } catch (Exception) { return value; }
        }
        /// <summary>
        /// This function querys the registry. If the key is found it returns the value as a boolean
        /// </summary>
        /// <param name="name">Keyname</param>
        /// <param name="value">Return on fail</param>
        /// <returns></returns>
        public static bool GetRegValue(string name, bool value)
        {
            try { return bool.Parse(GetRegValue(name, value.ToString())); } catch (Exception) { return value; }
        }

        public static int GetRegValue(string name, int value)
        {
            try { return int.Parse(GetRegValue(name, value.ToString())); } catch (Exception) { return value; }
        }

        public static void SetApplications(List<string[]> apps)
        {
            ClearApplications();
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router\Applications", true))
            {
                foreach (string[] app in apps)
                {
                    using (RegistryKey skey = key.CreateSubKey(app[0]))
                    {
                        skey.SetValue("Protocol", app[1]);
                        skey.SetValue("Command", app[2]);
                        skey.SetValue("Arguments", app[3]);
                    }
                }
            }
        }

        public static List<string[]> GetApplications()
        {
            List<string[]> apps = new List<string[]>();
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router\Applications\", false))
            {
                string[] keys = key.GetSubKeyNames();
                foreach (string k in keys)
                {
                    using (RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router\Applications\" + k, false))
                    {
                        string protocol = (string)key2.GetValue("Protocol");
                        string command = (string)key2.GetValue("Command");
                        string args = (string)key2.GetValue("Arguments");
                        String[] a = new string[4];
                        a[0] = k;
                        a[1] = protocol;
                        a[2] = command;
                        a[3] = args;
                        apps.Add(a);
                    }
                }
            }
            return apps;
        }

        public static void ClearApplications()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"HKEY_CURRENT_USER\SOFTWARE\Open Source\MeshCentral Router", true);
            key.DeleteSubKeyTree("Applications");
            key.Close();
        }
    }
}
