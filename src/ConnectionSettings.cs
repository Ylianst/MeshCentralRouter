using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MeshCentralRouter
{
    public partial class ConnectionSettings : Form
    {
        public ConnectionSettings()
        {
            InitializeComponent();
        }

        private void ConnectionSettings_Load(object sender, EventArgs e)
        {
            // Setup connection settings values from the registry
            manualProxyCheckBox.Checked = Settings.GetRegValue("ManualProxy", false);
            string host = Settings.GetRegValue("ProxyHost", "");
            if (host != "") { host += ":" + Settings.GetRegValue("ProxyPort", 443); }
            hostTextBox.Text = host;
            authComboBox.SelectedIndex = Settings.GetRegValue("ProxyAuth", 0);
            usernameTextBox.Text = Settings.GetRegValue("ProxyUsername", "");
            passwordTextBox.Text = Settings.GetRegValue("ProxyPassword", "");

            // Setup list of possible client authentication certificates
            using (X509Store CertificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                // Open the certificate stores
                CertificateStore.Open(OpenFlags.ReadOnly);

                // Load the list client authentication certificates
                clientCertificateComboBox.Items.Add(new DropDownItem(Properties.Resources.None, 0, null));
                foreach (X509Certificate2 cert in CertificateStore.Certificates)
                {
                    if (cert.HasPrivateKey)
                    {
                        bool clientAuthCert = false;
                        foreach (X509Extension ex in cert.Extensions)
                        {
                            if (ex.Oid.Value == "2.5.29.37")
                            {
                                X509EnhancedKeyUsageExtension exx = (X509EnhancedKeyUsageExtension)ex;
                                foreach (var usage in exx.EnhancedKeyUsages)
                                {
                                    if (usage.Value == "1.3.6.1.5.5.7.3.2") { clientAuthCert = true; }
                                }
                            }
                        }
                        if (clientAuthCert) { clientCertificateComboBox.Items.Add(new DropDownItem(GetCertificateString(cert.Subject), 0, cert)); }
                    }
                }
                clientCertificateComboBox.SelectedIndex = 0;
                clientCertificateComboBox.Enabled = (clientCertificateComboBox.Items.Count > 1);

                // Close the certificate stores
                CertificateStore.Close();
            }

            // Select the client authentication cert
            int selected = 0;
            string clientCertThumbPrint = Settings.GetRegValue("ClientAuthCert", "");
            for (int i = 0; i < clientCertificateComboBox.Items.Count; i++)
            {
                DropDownItem item = (DropDownItem)clientCertificateComboBox.Items[i];
                X509Certificate2 cert = (X509Certificate2)item.Tag;
                if ((cert != null) && (cert.Thumbprint == clientCertThumbPrint)) { selected = i; }
            }
            clientCertificateComboBox.SelectedIndex = selected;

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
            // Save proxy settings
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

            // Save client authentication certificate setting
            DropDownItem item = (DropDownItem)clientCertificateComboBox.SelectedItem;
            X509Certificate2 cert = (X509Certificate2)item.Tag;
            Settings.SetRegValue("ClientAuthCert", (cert == null) ? "" : cert.Thumbprint);

            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void viewCertButton_Click(object sender, EventArgs e)
        {
            DropDownItem selectedItem = (DropDownItem)clientCertificateComboBox.SelectedItem;
            if ((selectedItem == null) || (selectedItem.Tag == null)) return;
            X509Certificate2UI.DisplayCertificate((X509Certificate2)selectedItem.Tag);
        }

        /// <summary>
        /// This class is used to add items to DropDown combo boxes and still be able to reference the item quickly.
        /// </summary>
        public class DropDownItem
        {
            public string Text = null;
            public object Tag = null;
            public int Handle = 0;
            public override string ToString() { return Text; }

            public DropDownItem()
            {
            }

            public DropDownItem(string Text, int Handle, object Tag)
            {
                this.Text = Text;
                this.Handle = Handle;
                this.Tag = Tag;
            }
        }

        public static Dictionary<string, string> ParseCertificateSubject(string str)
        {
            string t = "";
            bool quotes = false;
            Dictionary<string, string> r = new Dictionary<string, string>();
            foreach (char c in str)
            {
                if ((c == ',') && (quotes == false))
                {
                    if (t.Trim() != "") { int i = t.IndexOf('='); if (i > 0) { r[t.Substring(0, i).Trim()] = t.Substring(i + 1).Trim(); } }
                    t = "";
                }
                else if (c == '\"')
                {
                    if (quotes == false) { quotes = true; } else { quotes = false; }
                    t += c;
                }
                else
                {
                    t += c;
                }
            }
            if (t.Trim() != "") { int i = t.IndexOf('='); if (i > 0) { r[t.Substring(0, i).Trim()] = t.Substring(i + 1).Trim(); } }
            return r;
        }

        public static string GetCertificateString(string certDataStr)
        {
            Dictionary<string, string> names = ParseCertificateSubject(certDataStr);
            if (names.ContainsKey("CN")) return names["CN"];
            if (names.ContainsKey("O")) return names["O"];
            return "Unknown";
        }

        private void clientCertificateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownItem selectedItem = (DropDownItem)clientCertificateComboBox.SelectedItem;
            viewCertButton.Enabled = (selectedItem.Tag != null);
        }
    }
}
