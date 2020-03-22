using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class DeviceUserControl : UserControl
    {
        public MeshClass mesh;
        public NodeClass node;
        public MainForm parent;
        public bool present;

        public DeviceUserControl()
        {
            InitializeComponent();
        }

        public void UpdateInfo()
        {
            deviceNameLabel.Text = node.name;
            if (node.conn == 0) {
                devicePictureBox.Image = disabledDeviceImageList.Images[node.icon - 1];
            } else {
                devicePictureBox.Image = deviceImageList.Images[node.icon - 1];
            }

            string status = "";
            if ((node.conn & 1) != 0) { if (status.Length > 0) { status += ", "; }  status += "Agent"; }
            if ((node.conn & 2) != 0) { if (status.Length > 0) { status += ", "; } status += "AMT"; }
            if ((node.conn & 4) != 0) { if (status.Length > 0) { status += ", "; } status += "CIRA"; }
            if ((node.conn & 8) != 0) { if (status.Length > 0) { status += ", "; } status += "MQTT"; }
            if (status == "") { status = "Offline"; }
            deviceStatusLabel.Text = status;

            if (node.agentid < 6) {
                // Windows OS
                sshButton.Visible = false;
                scpButton.Visible = false;
                rdpButton.Visible = true;
            } else {
                // Other OS
                sshButton.Visible = true;
                scpButton.Visible = true;
                rdpButton.Visible = false;
            }

            // Must have remote control rights
            if ((mesh.rights & 8) != 0) {
                sshButton.Enabled = scpButton.Enabled = rdpButton.Enabled = httpsButton.Enabled = httpButton.Enabled = ((node.conn & 1) != 0);
            } else {
                sshButton.Enabled = scpButton.Enabled = rdpButton.Enabled = httpsButton.Enabled = httpButton.Enabled = false;
            }
        }

        private void httpButton_Click(object sender, System.EventArgs e)
        {
            parent.QuickMap(1, 80, 1, node); // HTTP
        }

        private void httpsButton_Click(object sender, System.EventArgs e)
        {
            parent.QuickMap(1, 443, 2, node); // HTTPS
        }

        private void sshButton_Click(object sender, System.EventArgs e)
        {
            parent.QuickMap(1, 22, 4, node); // Putty
        }

        private void scpButton_Click(object sender, System.EventArgs e)
        {
            parent.QuickMap(1, 22, 5, node); // WinSCP
        }

        private void rdpButton_Click(object sender, System.EventArgs e)
        {
            int rdpport = 3389;
            if (node.rdpport != 0) { rdpport = node.rdpport; }
            parent.QuickMap(1, rdpport, 3, node); // RDP
        }

        private void setRDPPortToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            int rdpport = 3389;
            if (node.rdpport != 0) { rdpport = node.rdpport; }
            rdpPortForm form = new rdpPortForm();
            form.rdpPort = node.rdpport;
            if ((form.ShowDialog(this) == DialogResult.OK) && (rdpport != form.rdpPort))
            {
                parent.meshcentral.setRdpPort(node, form.rdpPort);
            }
        }
    }
}
