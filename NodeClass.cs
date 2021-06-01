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

using System.Windows.Forms;
using System.Collections.Generic;

namespace MeshCentralRouter
{
    public class NodeClass
    {
        public string name;
        public int icon;
        public string nodeid;
        public string meshid;
        public int agentid;
        public int agentcaps;
        public int conn;
        public int rdpport;
        public ulong rights;
        public int mtype;
        public MeshClass mesh;
        public ListViewItem listitem;
        public DeviceUserControl control;
        public KVMViewer desktopViewer;
        public FileViewer fileViewer;
        public Dictionary<string, ulong> links;
        public string[] users;

        public override string ToString() { return name; }

        public string getStateString()
        {
            string status = "";
            if (mtype == 3) return Properties.Resources.Local;
            if ((conn & 1) != 0) { if (status.Length > 0) { status += ", "; } status += Properties.Resources.Agent; }
            if ((conn & 2) != 0) { if (status.Length > 0) { status += ", "; } status += Properties.Resources.CIRA; }
            if ((conn & 4) != 0) { if (status.Length > 0) { status += ", "; } status += Properties.Resources.AMT; }
            if ((conn & 8) != 0) { if (status.Length > 0) { status += ", "; } status += Properties.Resources.Relay; }
            if ((conn & 16) != 0) { if (status.Length > 0) { status += ", "; } status += Properties.Resources.MQTT; }
            if (status == "") { status = Properties.Resources.Offline; }
            return status;
        }
    }

    public class MeshClass
    {
        public string name;
        public string meshid;
        public string desc;
        public int type;
        public ulong rights;
        public Dictionary<string, ulong> links;

        public override string ToString() { return name; }
    }
}
