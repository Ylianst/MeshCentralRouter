/*
Copyright 2009-2021 Intel Corporation

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class FileTransferStatusForm : Form
    {
        private FileViewer fileViewer;
        private bool loaded = false;
        public bool showingError = false;
        private string errors = "";
        private bool done = false;
        private Point mainBoxLocation;
        private Point errorBoxLocation;

        public FileTransferStatusForm(FileViewer fileViewer)
        {
            this.fileViewer = fileViewer;
            InitializeComponent();
            Translate.TranslateControl(this);
            updateInfo();
            updateTimer.Enabled = true;
            mainBoxLocation = mainGroupBox.Location;
            errorBoxLocation = errorsGroupBox.Location;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            updateInfo();
        }

        public delegate void updateErrorMessageshandler();

        public void addErrorMessage(string msg)
        {
            lock (this)
            {
                errors += (msg + "\r\n");
                showingError = true;
            }
            if (loaded == false) return;
            if (this.InvokeRequired) { this.Invoke(new updateErrorMessageshandler(updateErrorMessages)); } else { updateErrorMessages(); }
        }

        public void updateErrorMessages()
        {
            showDialogParts(!done, showingError);
            lock (this) { errorsTextBox.Text = errors; }
        }

        public void transferCompleted()
        {
            done = true;
            showDialogParts(!done, showingError);
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

                // Compute bytes per second & estimated time left
                double elapseTimeSeconds = DateTime.Now.Subtract(fileViewer.uploadFileStartTime).TotalMilliseconds / 1000;
                if (elapseTimeSeconds < 5) { mainLabel2.Text = Translate.T(Properties.Resources.EstimatingDotDotDot); } else
                {
                    double bytePerSecond = x / elapseTimeSeconds;
                    double secondsLeft = Math.Round((fileViewer.uploadFileSize - x) / bytePerSecond);
                    mainLabel2.Text = bytePerSecondToString(bytePerSecond) + ", " + secondsLeftToString(secondsLeft);
                }

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

                // Compute bytes per second & estimated time left
                double elapseTimeSeconds = DateTime.Now.Subtract(fileViewer.downloadFileStartTime).TotalMilliseconds / 1000;
                if (elapseTimeSeconds < 5) { mainLabel2.Text = Translate.T(Properties.Resources.EstimatingDotDotDot); } else
                {
                    double bytePerSecond = x / elapseTimeSeconds;
                    double secondsLeft = Math.Round((fileViewer.downloadFileSize - x) / bytePerSecond);
                    mainLabel2.Text = bytePerSecondToString(bytePerSecond) + ", " + secondsLeftToString(secondsLeft);
                }

                progressBar2.Maximum = fileViewer.downloadFileArray.Count;
                x = (int)(int)fileViewer.downloadFileSize;
                if (x < 0) { x = 0; }
                if (x > (int)fileViewer.downloadFileArray.Count) { x = fileViewer.downloadFileArray.Count; }
                progressBar2.Value = fileViewer.downloadFileArrayPtr;
            }
            else
            {
                done = true;
                if (showingError == false) { Close(); } else { cancelButton.Text = Translate.T(Properties.Resources.Close); }
            }
        }

        private string secondsLeftToString(double x)
        {
            if (x > 5400) return String.Format(Translate.T(Properties.Resources.xhoursleft), Math.Round(x / 60 / 60));
            if (x > 90) return String.Format(Translate.T(Properties.Resources.xminutesleft), Math.Round(x / 60));
            if (x > 1) return String.Format(Translate.T(Properties.Resources.xsecondsleft), Math.Round(x));
            return Translate.T(Properties.Resources.Almostdone);
        }

        private string bytePerSecondToString(double x)
        {
            if (x > 1200000000) return String.Format(Translate.T(Properties.Resources.XGbytesPersec), Math.Round((x / 1024 / 1024 / 1024) * 10) / 10);
            if (x > 1200000) return String.Format(Translate.T(Properties.Resources.XMbytesPersec), Math.Round((x / 1024 / 1024) * 10) / 10);
            if (x > 1200) return String.Format(Translate.T(Properties.Resources.XKbytesPersec), Math.Round((x / 1024) * 10) / 10);
            return String.Format(Translate.T(Properties.Resources.XbytesPersec), x);
        }

        private void FileTransferStatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileViewer.uploadActive) { fileViewer.uploadStop = true; }
            if (fileViewer.downloadActive) { fileViewer.downloadStop = true; }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (done)
            {
                fileViewer.transferStatusForm = null;
                Close();
            }
            else
            {
                // Cancel file transfer
                if (fileViewer.uploadActive) { fileViewer.uploadCancel(); }
                if (fileViewer.downloadActive) { fileViewer.downloadCancel(); }
            }
        }

        private void FileTransferStatusForm_Load(object sender, EventArgs e)
        {
            CenterToParent();
            lock (this) { errorsTextBox.Text = errors; }
            loaded = true;
            showDialogParts(!done, showingError);
        }

        private void showDialogParts(bool status, bool errors)
        {
            mainGroupBox.Visible = status;
            errorsGroupBox.Visible = errors;
            if (errors == false)
            {
                if (status == false)
                {
                    // Show nothing
                    this.Height = 109;
                }
                else
                {
                    // Show only status box
                    this.Height = mainGroupBox.Height + 99;
                }
            }
            else
            {
                if (status == false)
                {
                    // Show only error box
                    this.Height = errorsGroupBox.Height + 99;
                    errorsGroupBox.Location = mainBoxLocation;
                }
                else
                {
                    // Show both status and error box
                    this.Height = mainGroupBox.Height + errorsGroupBox.Height + 109;
                    errorsGroupBox.Location = errorBoxLocation;
                }
            }
        }

    }
}
