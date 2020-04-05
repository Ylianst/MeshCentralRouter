using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DeviceUserControl control;

        public override string ToString() { return name; }
    }

    public class MeshClass
    {
        public string name;
        public string meshid;
        public string desc;
        public int type;
        public ulong rights;

        public override string ToString() { return name; }
    }
}
