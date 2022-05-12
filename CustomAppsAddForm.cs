using System;
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
    public partial class CustomAppsAddForm : Form
    {
        public string appName
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; updateInfo(); }
        }
        public string appProtocol
        {
            get { return protocolTextBox.Text; }
            set { protocolTextBox.Text = value; updateInfo(); }
        }
        public string appCommand
        {
            get { return commandTextBox.Text; }
            set { commandTextBox.Text = value; updateInfo(); }
        }

        public CustomAppsAddForm()
        {
            InitializeComponent();
        }

        public void updateInfo()
        {
            okButton.Enabled = (nameTextBox.Text.Length > 0) && (protocolTextBox.Text.Length > 0) && (commandTextBox.Text.Length > 0) && (nameTextBox.Text.IndexOf(' ') == -1) && (protocolTextBox.Text.IndexOf(' ') == -1);
        }

        private void CustomAppsAddForm_Load(object sender, EventArgs e)
        {
            updateInfo();
            nameTextBox.Focus();
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            updateInfo();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                commandTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
