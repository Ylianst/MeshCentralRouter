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
    public partial class CustomAppsRunForm : Form
    {
        private string command = null;

        public CustomAppsRunForm(string command)
        {
            this.command = command;
            InitializeComponent();
        }

        public string getFinalCommand()
        {
            return command.Replace("%L", addressTextBox.Text).Replace("%P", portTextBox.Text);
        }

        public void UpdateInfo()
        {
            commandTextBox.Text = getFinalCommand();
        }

        private void addressTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void portTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void CustomAppsRunForm_Load(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
