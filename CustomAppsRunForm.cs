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
        private string args = null;

        public CustomAppsRunForm(string command, string args)
        {
            this.command = command;
            this.args = args;
            InitializeComponent();
        }

        public string getFinalArgs()
        {
            return args.Replace("%L", addressTextBox.Text).Replace("%P", portTextBox.Text).Replace("%N", nameTextBox.Text);
        }

        public void UpdateInfo()
        {
            commandTextBox.Text = command;
            argsTextBox.Text = getFinalArgs();
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
