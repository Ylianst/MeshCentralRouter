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
            Translate.TranslateControl(this);
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
                int x = (int)fileViewer.uploadFilePtr;
                if (x < 0) { x = 0; }
                if (x > (int)fileViewer.uploadFileSize) { x = (int)fileViewer.uploadFileSize; }
                progressBar1.Value = x;

                progressBar2.Maximum = fileViewer.uploadFileArray.Count;
                x = (int)(int)fileViewer.uploadFileSize;
                if (x < 0) { x = 0; }
                if (x > (int)fileViewer.uploadFileArray.Count) { x = fileViewer.uploadFileArray.Count; }
                progressBar2.Value = fileViewer.uploadFileArrayPtr;
            }
            else if (fileViewer.downloadActive)
            {
                mainLabel1.Text = (string)fileViewer.downloadFileArray[fileViewer.downloadFileArrayPtr];
                progressBar1.Maximum = (int)fileViewer.downloadFileSize;
                int x = (int)fileViewer.downloadFilePtr;
                if (x < 0) { x = 0; }
                if (x > (int)fileViewer.downloadFileSize) { x = (int)fileViewer.downloadFileSize; }
                progressBar1.Value = x;

                progressBar2.Maximum = fileViewer.downloadFileArray.Count;
                x = (int)(int)fileViewer.downloadFileSize;
                if (x < 0) { x = 0; }
                if (x > (int)fileViewer.downloadFileArray.Count) { x = fileViewer.downloadFileArray.Count; }
                progressBar2.Value = fileViewer.downloadFileArrayPtr;
            }
            else { Close(); }
        }

        private void FileTransferStatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileViewer.uploadActive) { fileViewer.uploadStop = true; }
            if (fileViewer.downloadActive) { fileViewer.downloadStop = true; }
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
