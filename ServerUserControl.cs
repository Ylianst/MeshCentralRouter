using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class ServerUserControl : UserControl
    {
        private MainForm parent;
        public string key;
        public string name;
        public string info;
        public string url;
        public DateTime lastUpdate;
        private Color xbackColor;

        public ServerUserControl(MainForm parent, string key, string name, string info, string url)
        {
            this.parent = parent;
            this.key = key;
            this.name = name;
            this.info = info;
            this.url = url;
            this.lastUpdate = DateTime.Now;
            InitializeComponent();
        }

        private void ServerUserControl_Load(object sender, EventArgs e)
        {
            serverNameLabel.Text = this.name;
            serverInfoLabel.Text = this.info;
            xbackColor = BackColor;
            BackColor = Color.Wheat;
            backTimer.Enabled = true;
        }

        public bool Update(string name, string info, string url)
        {
            this.lastUpdate = DateTime.Now;

            // Fix the name
            if (name == null) { name = this.name; }
            if (name == null) { name = "MeshCentral"; }

            // Don't replace a IPv4 address with a IPv6 one, or remove information
            if ((this.url.IndexOf("://[") == -1) && (url.IndexOf("://[") >= 0)) return false;
            if ((this.info != null) && (info == null)) return false;

            // If any changes need to be made, make them now
            if ((this.name != name) || (this.info != info) || (this.url != url))
            {
                this.name = name;
                this.info = info;
                this.url = url;
                serverNameLabel.Text = this.name;
                if (this.info != null) { serverInfoLabel.Text = this.info; } else { serverInfoLabel.Text = url; }
                BackColor = Color.Wheat;
                backTimer.Enabled = true;
                return true;
            }
            return false;
        }

        private void serverButton_Click(object sender, EventArgs e)
        {
            //MouseEventArgs me = (MouseEventArgs)e;
            //if (me.Button == MouseButtons.Left) { parent.serverClick(this); }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ServerInfoForm form = new ServerInfoForm(name, url, info, key);
            //form.ShowDialog(this);
        }

        private void backTime_Tick(object sender, EventArgs e)
        {
            BackColor = xbackColor;
            backTimer.Enabled = false;
        }
    }
}
