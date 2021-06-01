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
            Translate.TranslateControl(this);

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
