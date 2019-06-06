using System;
using System.IO;
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
    public partial class AppLaunchForm : Form
    {
        public AppLaunchForm()
        {
            InitializeComponent();
            UpdateInfo();
        }

        public void SetAppName(string name) { appNameLabel.Text = name; }
        public void SetAppLink(string link) { appLinkLabel.Text = link; }
        public void SetAppPath(string path) { appPathTextBox.Text = path; }
        public string GetAppPath() { return appPathTextBox.Text; }

        private void folderPictureBox_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = appPathTextBox.Text;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                appPathTextBox.Text = openFileDialog.FileName;
                UpdateInfo();
            }
        }

        private void appPathTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            okButton.Enabled = File.Exists(appPathTextBox.Text);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void appLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(appLinkLabel.Text);
        }

    }
}
