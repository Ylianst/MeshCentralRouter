using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class InstallForm : Form
    {
        public InstallForm()
        {
            InitializeComponent();

            // Load customizations
            FileInfo selfExe = new FileInfo(Assembly.GetExecutingAssembly().Location);
            try { pictureBox1.Image = (Bitmap)Image.FromFile(Path.Combine(selfExe.Directory.FullName, @"customization\install.png")); } catch (Exception) { }
            try {
                string[] lines = File.ReadAllLines(Path.Combine(selfExe.Directory.FullName, @"customization\customize.txt"));
                if (lines[2] != "") { groupBox1.Text = lines[2]; }
                if (lines[3] != "") { label1.Text = lines[3]; }
            } catch (Exception) { }
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
