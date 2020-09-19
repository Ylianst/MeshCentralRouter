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
    public partial class FilenamePromptForm : Form
    {
        public string filename
        {
            get { return mainTextBox.Text; }
            set { mainTextBox.Text = value; }
        }

        public FilenamePromptForm(string operation, string filename)
        {
            InitializeComponent();
            mainGroupBox.Text = operation;
            mainTextBox.Text = filename;
            okButton.Enabled = (filename.Length > 0);
        }

        private void mainTextBox_TextChanged(object sender, EventArgs e)
        {
            okButton.Enabled = (mainTextBox.Text.Length > 0);
        }

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
