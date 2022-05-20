using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class ProxySettings : Form
    {
        public ProxySettings()
        {
            InitializeComponent();
            manualProxyCheckBox.Checked = Settings.GetRegValue("ManualProxy", false);
            string host = Settings.GetRegValue("ProxyHost", "");
            if (host != "") { host += ":" + Settings.GetRegValue("ProxyPort", 443); }
            hostTextBox.Text = host;
            authComboBox.SelectedIndex = Settings.GetRegValue("ProxyAuth", 0);
            usernameTextBox.Text = Settings.GetRegValue("ProxyUsername", "");
            passwordTextBox.Text = Settings.GetRegValue("ProxyPassword", "");
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            hostTextBox.Enabled = manualProxyCheckBox.Checked;
            usernameTextBox.Enabled = passwordTextBox.Enabled = (manualProxyCheckBox.Checked && (authComboBox.SelectedIndex == 1));

            bool ok = true;
            if (manualProxyCheckBox.Checked == true)
            {
                string portStr = "";
                string hostStr = hostTextBox.Text;
                int i = hostStr.IndexOf(':');
                if (i >= 0) { portStr = hostStr.Substring(i + 1); hostStr = hostStr.Substring(0, i); }
                int port = 0;
                int.TryParse(portStr, out port);
                if ((hostStr.Length == 0) || (port < 1) || (port > 65535)) { ok = false; }
            }
            okButton.Enabled = ok;
        }

        private void manualProxyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Settings.SetRegValue("ManualProxy", manualProxyCheckBox.Checked);
            if (manualProxyCheckBox.Checked == true) {
                string hostStr = hostTextBox.Text;
                string portStr = "";
                int i = hostStr.IndexOf(':');
                if (i >= 0) { portStr = hostStr.Substring(i + 1); hostStr = hostStr.Substring(0, i); }
                int port = 0;
                int.TryParse(portStr, out port);
                Settings.SetRegValue("ProxyHost", hostStr);
                Settings.SetRegValue("ProxyPort", port);
                Settings.SetRegValue("ProxyAuth", authComboBox.SelectedIndex);
                Settings.SetRegValue("ProxyUsername", (authComboBox.SelectedIndex == 1) ? usernameTextBox.Text : "");
                Settings.SetRegValue("ProxyPassword", (authComboBox.SelectedIndex == 1) ? passwordTextBox.Text : "");
            } else {
                Settings.SetRegValue("ProxyHost", "");
                Settings.SetRegValue("ProxyPort", "");
                Settings.SetRegValue("ProxyAuth", 0);
                Settings.SetRegValue("ProxyUsername", "");
                Settings.SetRegValue("ProxyPassword", "");
            }
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
