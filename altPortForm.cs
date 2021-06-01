using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class altPortForm : Form
    {
        public altPortForm()
        {
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        public int altPort
        {
            get { return (int)portNumericUpDown.Value; }
            set { portNumericUpDown.Value = value; updateInfo(); }
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
            okButton.Enabled = (portNumericUpDown.Value > 0) && (portNumericUpDown.Value < 65536);
        }

        private void numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            updateInfo();
        }
    }
}
