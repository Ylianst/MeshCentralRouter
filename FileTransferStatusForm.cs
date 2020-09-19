using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class FileTransferStatusForm : Form
    {
        private FileViewer fileViewer;

        public FileTransferStatusForm(FileViewer fileViewer)
        {
            this.fileViewer = fileViewer;
            InitializeComponent();
            updateInfo();
            updateTimer.Enabled = true;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            updateInfo();
        }

        private void updateInfo()
        {
            if (fileViewer.uploadActive)
            {
                mainLabel1.Text = (string)fileViewer.uploadFileArray[fileViewer.uploadFileArrayPtr];
                progressBar1.Maximum = (int)fileViewer.uploadFileSize;
                progressBar1.Value = (int)fileViewer.uploadFilePtr;
                progressBar2.Maximum = fileViewer.uploadFileArray.Count;
                progressBar2.Value = fileViewer.uploadFileArrayPtr;
            }
            else { Close(); }
        }

        private void FileTransferStatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileViewer.uploadActive) { fileViewer.uploadStop = true; }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FileTransferStatusForm_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
