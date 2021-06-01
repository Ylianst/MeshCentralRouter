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
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Net.Security;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace MeshCentralRouter
{
    public partial class UpdateForm : Form
    {
        private string url = null;
        private string hash = null;
        private int size = 0;
        private string[] args = null;
        private string serverTlsCertHash = null;

        public UpdateForm(string url, string hash, int size, string[] args, string serverhash)
        {
            InitializeComponent();
            Translate.TranslateControl(this);
            this.url = url;
            this.hash = hash;
            this.size = size;
            this.args = args;
            this.serverTlsCertHash = serverhash;

            try
            {
                FileInfo selfExe = new FileInfo(Assembly.GetExecutingAssembly().Location);
                string[] lines = File.ReadAllLines(Path.Combine(selfExe.Directory.FullName, @"customization\customize.txt"));
                if (lines[4] != "") { this.Text = lines[4]; }
                if (lines[5] != "") { mainLabel.Text = lines[5]; }
            }
            catch (Exception) { }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DownloadUpdate();
        }

        private void Client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            updateProgressBar.Value = e.ProgressPercentage;
        }

        private void DownloadUpdate()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            Uri x = webRequest.RequestUri;
            webRequest.Method = "GET";
            webRequest.Timeout = 10000;
            webRequest.BeginGetResponse(new AsyncCallback(DownloadUpdateRespone), webRequest);
            webRequest.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
            updateProgressBar.Visible = true;
        }

        public bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Check MeshCentral server's TLS certificate. This is our first security layer.
            if ((serverTlsCertHash != null) && (serverTlsCertHash != certificate.GetCertHashString().ToLower()) && (serverTlsCertHash != GetMeshKeyHash(certificate).ToLower()) && (serverTlsCertHash != GetMeshCertHash(certificate).ToLower())) return false;
            return true;
        }

        public delegate void updateProgressHandler(int ptr, int total);
        public void updateProgress(int ptr, int total)
        {
            if (this.InvokeRequired) { this.Invoke(new updateProgressHandler(updateProgress), ptr, total); return; }
            updateProgressBar.Visible = true;
            updateProgressBar.Maximum = total;
            if (ptr <= total) { updateProgressBar.Value = ptr; } else { updateProgressBar.Value = total; }
        }

        public delegate void updateMessageHandler(string msg, int buttons);
        public void updateMessage(string msg, int buttons)
        {
            if (this.InvokeRequired) { this.Invoke(new updateMessageHandler(updateMessage), msg, buttons); return; }
            mainLabel.Text = msg;
            okButton.Enabled = ((buttons & 1) != 0);
            cancelButton.Enabled = ((buttons & 2) != 0);
            updateProgressBar.Visible = ((buttons & 4) != 0);
        }

        private void DownloadUpdateRespone(IAsyncResult asyncResult)
        {
            long received = 0;
            HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;
            try
            {
                // Hash our own executable
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult))
                {
                    byte[] buffer = new byte[4096];
                    FileStream fileStream = File.OpenWrite(System.Reflection.Assembly.GetEntryAssembly().Location + ".update.exe");
                    using (Stream input = webResponse.GetResponseStream())
                    {
                        int size = input.Read(buffer, 0, buffer.Length);
                        while (size > 0)
                        {
                            fileStream.Write(buffer, 0, size);
                            received += size;
                            updateProgress((int)received, (int)size);
                            size = input.Read(buffer, 0, buffer.Length);
                        }
                    }
                    fileStream.Flush();
                    fileStream.Close();

                    // Hash the resulting file
                    byte[] downloadHash;
                    using (var sha384 = SHA384Managed.Create()) { using (var stream = File.OpenRead(System.Reflection.Assembly.GetEntryAssembly().Location + ".update.exe")) { downloadHash = sha384.ComputeHash(stream); } }
                    string downloadHashHex = BitConverter.ToString(downloadHash).Replace("-", string.Empty).ToLower();
                    if (downloadHashHex != hash) {
                        updateMessage(Translate.T(Properties.Resources.InvalidDownload), 2);
                        File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location + ".update.exe");
                    } else {
                        updateMessage(Translate.T(Properties.Resources.Updating), 0);
                        Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location + ".update.exe", "-update:" + System.Reflection.Assembly.GetEntryAssembly().Location + " " + string.Join(" ", args));
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex) { updateMessage("Error: " + ex.ToString(), 2); }
        }


        // Return a modified base64 SHA384 hash string of the certificate public key
        public static string GetMeshKeyHash(X509Certificate cert)
        {
            return ByteArrayToHexString(new SHA384Managed().ComputeHash(cert.GetPublicKey()));
        }

        // Return a modified base64 SHA384 hash string of the certificate
        public static string GetMeshCertHash(X509Certificate cert)
        {
            return ByteArrayToHexString(new SHA384Managed().ComputeHash(cert.GetRawCertData()));
        }

        public static string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);
            string HexAlphabet = "0123456789ABCDEF";
            foreach (byte B in Bytes) { Result.Append(HexAlphabet[(int)(B >> 4)]); Result.Append(HexAlphabet[(int)(B & 0xF)]); }
            return Result.ToString();
        }

    }
}
