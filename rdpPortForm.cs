using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class rdpPortForm : Form
    {
        public rdpPortForm()
        {
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        public int originalRdpPort;

        public int rdpPort
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = originalRdpPort = value; updateInfo(); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            updateInfo();
        }

        public void updateInfo()
        {
            okButton.Enabled = (numericUpDown1.Value > 0) && (numericUpDown1.Value < 65536) && (numericUpDown1.Value != originalRdpPort);
        }

        private void numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            updateInfo();
        }
    }
}
