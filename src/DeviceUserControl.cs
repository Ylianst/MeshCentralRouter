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

using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class DeviceUserControl : UserControl
    {
        public MeshClass mesh;
        public NodeClass node;
        public MainForm parent;
        public bool present;
        private bool xShowDeviceGroupName = true;

        public bool showDeviceGroupName
        {
            get { return xShowDeviceGroupName; }
            set { xShowDeviceGroupName = value; }
        }

        public DeviceUserControl()
        {
            InitializeComponent();
        }

        public void UpdateInfo()
        {
            if (parent.getShowGroupNames() && (mesh != null)) { deviceNameLabel.Text = mesh.name + ", " + node.name; } else { deviceNameLabel.Text = node.name; }
            if ((node.icon > 0) && (node.icon <= disabledDeviceImageList.Images.Count))
            {
                if (node.conn == 0)
                {
                    devicePictureBox.Image = disabledDeviceImageList.Images[node.icon - 1];
                }
                else
                {
                    devicePictureBox.Image = deviceImageList.Images[node.icon - 1];
                }
            }

            string status = "";
            if ((node.conn & 1) != 0) { if (status.Length > 0) { status += ", "; }  status += Translate.T(Properties.Resources.Agent); }
            if ((node.conn & 2) != 0) { if (status.Length > 0) { status += ", "; } status += Translate.T(Properties.Resources.CIRA); }
            if ((node.conn & 4) != 0) { if (status.Length > 0) { status += ", "; } status += Translate.T(Properties.Resources.AMT); }
            if ((node.conn & 8) != 0) { if (status.Length > 0) { status += ", "; } status += Translate.T(Properties.Resources.Relay); }
            if ((node.conn & 16) != 0) { if (status.Length > 0) { status += ", "; } status += Translate.T(Properties.Resources.MQTT); }
            if (status == "") { status = Translate.T(Properties.Resources.Offline); }
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

            // Compute rights on this device
            ulong rights = node.rights; // Direct device rights
            if (mesh != null) { rights |= mesh.rights; } // Device group rights
            foreach (string i in node.links.Keys) { if (parent.meshcentral.userGroups.ContainsKey(i)) { rights |= node.links[i]; } } // Take a look at group rights
            foreach (string i in parent.meshcentral.userRights.Keys) {
                if ((i.StartsWith("ugrp/")) && (mesh.links.ContainsKey(i))) {
                    rights |= (ulong)mesh.links[i];
                }
            }

            // Must have remote control rights
            if ((rights & 8) != 0)
            {
                sshButton.Enabled = scpButton.Enabled = rdpButton.Enabled = httpsButton.Enabled = httpButton.Enabled = ((node.conn & 1) != 0);
            }
            else
            {
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

        private void toolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            int port = 0, appid = 0;
            if (altPortContextMenuStrip.SourceControl == httpButton) { appid = 1; port = 80; } // HTTP
            if (altPortContextMenuStrip.SourceControl == httpsButton) { appid = 2; port = 443; } // HTTPS
            if (altPortContextMenuStrip.SourceControl == scpButton) { appid = 5; port = 22; } // SCP
            if (altPortContextMenuStrip.SourceControl == sshButton) { appid = 4; port = 22; } // SSH
            if (appid == 0) return;

            altPortForm form = new altPortForm();
            form.altPort = port;
            if ((form.ShowDialog(this) == DialogResult.OK)) { parent.QuickMap(1, form.altPort, appid, node); }
        }
    }
}
