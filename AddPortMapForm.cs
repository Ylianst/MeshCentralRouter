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
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class AddPortMapForm : Form
    {
        private MeshCentralServer meshcentral;
        private NodeClass selectedNode = null;

        public AddPortMapForm(MeshCentralServer meshcentral)
        {
            this.meshcentral = meshcentral;
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        public string getName() { return nameTextBox.Text; }
        public int getProtocol() { return (int)(tcpRadioButton.Checked?1:2); }
        public int getLocalPort() { return (int)localNumericUpDown.Value; }
        public int getRemotePort() { return (int)remoteNumericUpDown.Value; }
        public int getAppId() {
            int x = (int)appComboBox.SelectedIndex;
            if (x == 0) { return 0; } // Custom
            if (x == 1) { return 1; } // HTTP
            if (x == 2) { return 2; } // HTTPS
            if (x == 3) { return 4; } // PuTTY
            if (x == 4) { return 3; } // RDP
            if (x == 5) { return 5; } // WinSCP
            return x;
        }
        public NodeClass getNode() { return (NodeClass)nodeComboBox.SelectedItem; }
        public void setNode(NodeClass node) { selectedNode = node; }

        private void AddPortMapForm_Load(object sender, EventArgs e)
        {
            if (selectedNode == null)
            {
                // Fill the groups
                groupComboBox.Items.Clear();
                foreach (string meshid in meshcentral.meshes.Keys)
                {
                    MeshClass mesh = meshcentral.meshes[meshid];
                    if ((mesh.type == 2) || (mesh.type == 3))
                    {
                        int nodeCount = 0;
                        foreach (string nodeid in meshcentral.nodes.Keys)
                        {
                            NodeClass node = meshcentral.nodes[nodeid];
                            if ((node.meshid == mesh.meshid) && (((node.conn & 1) != 0) || (node.mtype == 3))) { nodeCount++; }
                        }
                        if (nodeCount > 0) { groupComboBox.Items.Add(mesh); }
                    }
                }

                // If the user has indivitual device rights, add an extra device group
                if (meshcentral.userRights != null)
                {
                    bool indivitualDevices = false;
                    foreach (string id in meshcentral.userRights.Keys) { if (id.StartsWith("node/")) { indivitualDevices = true; } }
                    if (indivitualDevices)
                    {
                        MeshClass m = new MeshClass();
                        m.name = Properties.Resources.IndividualDevices;
                        groupComboBox.Items.Add(m);
                    }
                }

                // Set default selection
                if (groupComboBox.Items.Count > 0) { groupComboBox.SelectedIndex = 0; }
                appComboBox.SelectedIndex = 1;
                fillNodesInDropDown();
            } else {
                if (selectedNode.mesh == null)
                {
                    MeshClass m = new MeshClass();
                    m.name = Properties.Resources.IndividualDevices;
                    groupComboBox.Items.Add(m);
                }
                else
                {
                    groupComboBox.Items.Add(selectedNode.mesh);
                }
                groupComboBox.SelectedIndex = 0;
                groupComboBox.Enabled = false;
                nodeComboBox.Items.Add(selectedNode);
                nodeComboBox.SelectedIndex = 0;
                nodeComboBox.Enabled = false;
                appComboBox.SelectedIndex = 1;
            }
            nameTextBox.Focus();
        }

        private void fillNodesInDropDown()
        {
            if (selectedNode != null) return;

            MeshClass mesh = (MeshClass)groupComboBox.SelectedItem;

            // Fill the nodes dropdown
            nodeComboBox.Items.Clear();
            if (meshcentral.nodes != null)
            {
                foreach (string nodeid in meshcentral.nodes.Keys)
                {
                    NodeClass node = meshcentral.nodes[nodeid];
                    if (((node.meshid == mesh.meshid) || ((mesh.meshid == null) && (meshcentral.userRights.ContainsKey(node.nodeid)))) && (((node.conn & 1) != 0) || (node.mtype == 3))) { nodeComboBox.Items.Add(node); }
                }
            }

            if (nodeComboBox.Items.Count > 0) { nodeComboBox.SelectedIndex = 0; }
        }

        private void appComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (appComboBox.SelectedIndex == 1) { remoteNumericUpDown.Value = 80; } // HTTP
            if (appComboBox.SelectedIndex == 2) { remoteNumericUpDown.Value = 443; } // HTTPS
            if (appComboBox.SelectedIndex == 3) { remoteNumericUpDown.Value = 22; } // SSH
            if (appComboBox.SelectedIndex == 4) { remoteNumericUpDown.Value = 3389; } // RDP
            if (appComboBox.SelectedIndex == 5) { remoteNumericUpDown.Value = 22; } // SCP
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void groupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillNodesInDropDown();
        }

        private void tcpRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            appComboBox.Enabled = tcpRadioButton.Checked;
            if (udpRadioButton.Checked) { appComboBox.SelectedIndex = 0; }
        }
    }
}
