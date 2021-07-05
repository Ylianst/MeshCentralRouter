using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class FileConfirmOverwriteForm : Form
    {
        public FileConfirmOverwriteForm()
        {
            InitializeComponent();
        }

        public string mainTextLabel { get { return mainLabel.Text; } set { mainLabel.Text = value; } }

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
