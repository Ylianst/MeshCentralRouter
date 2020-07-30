using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class AddRelayMapForm : Form
    {
        private MeshCentralServer meshcentral;
        private NodeClass selectedNode = null;

        public AddRelayMapForm(MeshCentralServer meshcentral)
        {
            this.meshcentral = meshcentral;
            InitializeComponent();
            updateInfo();
        }

        public int getProtocol() { return (int)(tcpRadioButton.Checked ? 1 : 2); }
        public int getLocalPort() { return (int)localNumericUpDown.Value; }
        public int getRemotePort() { return (int)remoteNumericUpDown.Value; }
        public string getRemoteIP() { return remoteIpTextBox.Text; }
        public int getAppId() { return (int)appComboBox.SelectedIndex; }
        public NodeClass getNode() { return (NodeClass)nodeComboBox.SelectedItem; }
        public void setNode(NodeClass node) { selectedNode = node; }

        private void AddRelayMapForm_Load(object sender, EventArgs e)
        {
            if (selectedNode == null)
            {
                // Fill the groups
                groupComboBox.Items.Clear();
                foreach (string meshid in meshcentral.meshes.Keys)
                {
                    MeshClass mesh = meshcentral.meshes[meshid];
                    if (mesh.type == 2)
                    {
                        int nodeCount = 0;
                        foreach (string nodeid in meshcentral.nodes.Keys)
                        {
                            NodeClass node = meshcentral.nodes[nodeid];
                            if ((node.meshid == mesh.meshid) && ((node.conn & 1) != 0)) { nodeCount++; }
                        }
                        if (nodeCount > 0) { groupComboBox.Items.Add(mesh); }
                    }
                }

                // Set default selection
                if (groupComboBox.Items.Count > 0) { groupComboBox.SelectedIndex = 0; }
                appComboBox.SelectedIndex = 1;
                fillNodesInDropDown();
            } else {
                groupComboBox.Items.Add(selectedNode.mesh);
                groupComboBox.SelectedIndex = 0;
                groupComboBox.Enabled = false;
                nodeComboBox.Items.Add(selectedNode);
                nodeComboBox.SelectedIndex = 0;
                nodeComboBox.Enabled = false;
                appComboBox.SelectedIndex = 1;
            }
        }

        private void fillNodesInDropDown()
        {
            if (selectedNode != null) return;
            MeshClass mesh = (MeshClass)groupComboBox.SelectedItem;

            // Fill the nodes dropdown
            nodeComboBox.Items.Clear();
            foreach (string nodeid in meshcentral.nodes.Keys)
            {
                NodeClass node = meshcentral.nodes[nodeid];
                if ((node.meshid == mesh.meshid) && ((node.conn & 1) != 0)) { nodeComboBox.Items.Add(node); }
            }

            if (nodeComboBox.Items.Count > 0) { nodeComboBox.SelectedIndex = 0; }
        }

        private void appComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (appComboBox.SelectedIndex == 1) { remoteNumericUpDown.Value = 80; }
            if (appComboBox.SelectedIndex == 2) { remoteNumericUpDown.Value = 443; }
            if (appComboBox.SelectedIndex == 3) { remoteNumericUpDown.Value = 3389; }
            if (appComboBox.SelectedIndex == 4) { remoteNumericUpDown.Value = 22; }
            if (appComboBox.SelectedIndex == 5) { remoteNumericUpDown.Value = 22; }
        }

        private void updateInfo()
        {
            IPAddress ipaddress = null;
            try { ipaddress = IPAddress.Parse(remoteIpTextBox.Text); } catch (Exception) { }
            okButton.Enabled = (ipaddress != null);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void remoteIpTextBox_TextChanged(object sender, EventArgs e)
        {
            updateInfo();
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
