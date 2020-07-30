using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
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
        public int conn;
        public int rdpport;
        public ulong rights;
        public MeshClass mesh;
        public ListViewItem listitem;
        public DeviceUserControl control;
        public Dictionary<string, ulong> links;

        public override string ToString() { return name; }

        public string getStateString()
        {
            string status = "";
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
