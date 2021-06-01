using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class MappingHelpForm : Form
    {
        public MappingHelpForm()
        {
            InitializeComponent();
            Translate.TranslateControl(this);

            // Load customizations
            FileInfo selfExe = new FileInfo(Assembly.GetExecutingAssembly().Location);
            try { helpPictureBox.Image = (Bitmap)Image.FromFile(Path.Combine(selfExe.Directory.FullName, @"customization\help.png")); } catch (Exception) { }
        }
    }
}
