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
    public partial class ProxySettings : Form
    {
        public ProxySettings()
        {
            InitializeComponent();
            useManualProxySettings.Checked = Settings.GetRegValue("Use_Manual_Http_proxy", false);
            manualHttpProxyHost.Text = Settings.GetRegValue("Manual_Http_proxy_host", "");
            manualHttpProxyPort.Text = Settings.GetRegValue("Manual_Http_proxy_port", "");
            manualHttpProxyUsername.Text = Settings.GetRegValue("Manual_Http_proxy_username", "");
            manualHttpProxyPassword.Text = Settings.GetRegValue("Manual_Http_proxy_password", "");
            checkbox_refresh_form();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SaveProxyConfig_Click(object sender, EventArgs e)
        {
            Settings.SetRegValue("Use_Manual_Http_proxy", useManualProxySettings.Checked);
            Settings.SetRegValue("Manual_Http_proxy_host", manualHttpProxyHost.Text);
            Settings.SetRegValue("Manual_Http_proxy_port", manualHttpProxyPort.Text);
            Settings.SetRegValue("Manual_Http_proxy_username", manualHttpProxyUsername.Text);
            Settings.SetRegValue("Manual_Http_proxy_password", manualHttpProxyPassword.Text);
            DialogResult = DialogResult.OK;
        }

        private void checkbox_refresh_form()
        {
            if (useManualProxySettings.Checked)
            {
                manualHttpProxyHost.ReadOnly = false;
                manualHttpProxyPort.ReadOnly = false;
                manualHttpProxyUsername.ReadOnly = false;
                manualHttpProxyPassword.ReadOnly = false;
            }
            else
            {
                manualHttpProxyHost.ReadOnly = true;
                manualHttpProxyPort.ReadOnly = true;
                manualHttpProxyUsername.ReadOnly = true;
                manualHttpProxyPassword.ReadOnly = true;
            }
        }

        private void useManualProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            checkbox_refresh_form();    
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
