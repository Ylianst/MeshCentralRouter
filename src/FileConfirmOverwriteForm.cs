using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class FileConfirmOverwriteForm : Form
    {
        public FileConfirmOverwriteForm()
        {
            InitializeComponent();
            skipCheckBox.Checked = (Settings.GetRegValue("skipExistingFiles", "0") == "1");
        }

        public string mainTextLabel { get { return mainLabel.Text; } set { mainLabel.Text = value; } }
        public bool skipExistingFiles { get { return skipCheckBox.Checked; } set { skipCheckBox.Checked = value; } }

        private void okButton_Click(object sender, EventArgs e)
        {
            Settings.SetRegValue("skipExistingFiles", skipCheckBox.Checked ? "1" : "0");
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
