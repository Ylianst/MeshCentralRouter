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
    public partial class FileDialogMsgForm : Form
    {
        private FileViewer xparent = null;

        public FileDialogMsgForm(FileViewer xparent)
        {
            this.xparent = xparent;
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        public void UpdateStatus(string msg, string file, int progress)
        {
            if (msg != null) { mainLabel1.Text = msg; } else { mainLabel1.Text = ""; }
            if (file != null) { mainLabel2.Text = file; } else { mainLabel2.Text = ""; }
            if ((progress >= 0) && (progress <= 100)) { progressBar1.Value = progress; }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            xparent.requestCancel();
        }
    }
}
