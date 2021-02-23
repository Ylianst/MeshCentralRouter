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
    public partial class MappingHelpForm : Form
    {
        public MappingHelpForm()
        {
            InitializeComponent();

            // Load customizations
            try { helpPictureBox.Image = (Bitmap)Image.FromFile(@"customization\help.png"); } catch (Exception) { }
        }
    }
}
