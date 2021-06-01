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
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class rdpPortForm : Form
    {
        public rdpPortForm()
        {
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        public int originalRdpPort;

        public int rdpPort
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = originalRdpPort = value; updateInfo(); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            updateInfo();
        }

        public void updateInfo()
        {
            okButton.Enabled = (numericUpDown1.Value > 0) && (numericUpDown1.Value < 65536) && (numericUpDown1.Value != originalRdpPort);
        }

        private void numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            updateInfo();
        }
    }
}
