using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace MeshCentralRouter
{
    public partial class CustomAppsForm : Form
    {
        List<string[]> apps;

        public CustomAppsForm(List<string[]> apps)
        {
            this.apps = apps;
            InitializeComponent();
        }

        private void CustomAppsForm_Load(object sender, EventArgs e)
        {
            if (apps != null) {
                foreach (string[] app in apps) {
                    string[] x = new string[5];
                    x[0] = app[0];
                    x[1] = app[1];
                    x[2] = "\"" + app[2] + "\" " + app[3];
                    x[3] = app[2];
                    x[4] = app[3];
                    mainListView.Items.Add(new ListViewItem(x));
                }
            }
            UpdateInfo();
        }

        public List<string[]> getApplications()
        {
            List<string[]> r = new List<string[]>();
            foreach (ListViewItem l in mainListView.Items)
            {
                string[] x = new string[4];
                x[0] = l.SubItems[0].Text;
                x[1] = l.SubItems[1].Text.ToLower();
                x[2] = l.SubItems[3].Text;
                x[3] = l.SubItems[4].Text;
                r.Add(x);
            }
            return r;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            CustomAppsAddForm f = new CustomAppsAddForm();
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                // Remove any matching protocol
                List<ListViewItem> list = new List<ListViewItem>();
                foreach (ListViewItem l in mainListView.Items) { if (l.SubItems[1].Text.ToLower() == f.appProtocol.ToLower()) { list.Add(l); } }
                foreach (ListViewItem l in list) { mainListView.Items.Remove(l); }

                // Add the new protocol
                string[] x = new string[5];
                x[0] = f.appName;
                x[1] = f.appProtocol.ToLower();
                x[2] = "\"" + f.appCommand + "\" " + f.appArgs;
                x[3] = f.appCommand;
                x[4] = f.appArgs;
                mainListView.Items.Add(new ListViewItem(x));
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void mainListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            removeButton.Enabled = (mainListView.SelectedItems.Count > 0);
            runButton.Enabled = editButton.Enabled = (mainListView.SelectedItems.Count == 1);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (mainListView.SelectedItems.Count == 0) return;
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (ListViewItem l in mainListView.SelectedItems) { list.Add(l); }
            foreach (ListViewItem l in list) { mainListView.Items.Remove(l); }
        }

        private void appContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            runToolStripMenuItem.Visible = (mainListView.SelectedItems.Count == 1);
            removeToolStripMenuItem.Visible = toolStripMenuItem1.Visible = (mainListView.SelectedItems.Count > 0);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainListView.SelectedItems.Count != 1) return;
            ListViewItem i = mainListView.SelectedItems[0];
            CustomAppsRunForm f = new CustomAppsRunForm(i.SubItems[3].Text, i.SubItems[4].Text);
            if (f.ShowDialog(this) == DialogResult.OK) { Process.Start(i.SubItems[3].Text, f.getFinalArgs()); }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainListView.SelectedItems.Count != 1) return;
            ListViewItem i = mainListView.SelectedItems[0];
            CustomAppsAddForm f = new CustomAppsAddForm();
            f.appName = i.SubItems[0].Text;
            f.appProtocol = i.SubItems[1].Text;
            f.appCommand = i.SubItems[3].Text;
            f.appArgs = i.SubItems[4].Text;
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                // Remove any matching protocol
                List<ListViewItem> list = new List<ListViewItem>();
                foreach (ListViewItem l in mainListView.Items) { if (l.SubItems[1].Text.ToLower() == f.appProtocol.ToLower()) { list.Add(l); } }
                foreach (ListViewItem l in list) { mainListView.Items.Remove(l); }

                // Add the new protocol
                string[] x = new string[5];
                x[0] = f.appName;
                x[1] = f.appProtocol.ToLower();
                x[2] = "\"" + f.appCommand + "\" " + f.appArgs;
                x[3] = f.appCommand;
                x[4] = f.appArgs;
                mainListView.Items.Add(new ListViewItem(x));
            }
        }
    }
}
