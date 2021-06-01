using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class SshUsernameForm : Form
    {
        public SshUsernameForm()
        {
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        public string Username
        {
            get { return usernameTextBox.Text; }
            set { usernameTextBox.Text = value; updateInfo(); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public void updateInfo()
        {
            okButton.Enabled = (usernameTextBox.Text.Length > 0) && (usernameTextBox.Text.IndexOf(" ") == -1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateInfo();
        }

        private void SshUsernameForm_Load(object sender, EventArgs e)
        {
            updateInfo();
            usernameTextBox.Focus();
        }
    }
}
