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
    public partial class KVMStats : Form
    {
        public KVMViewer viewer;

        public KVMStats(KVMViewer viewer)
        {
            this.viewer = viewer;
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            kvmInBytesLabel.Text = string.Format(((viewer.bytesIn == 1)?"{0} Byte":"{0} Bytes"), viewer.bytesIn);
            kvmOutBytesLabel.Text = string.Format(((viewer.bytesOut == 1) ? "{0} Byte" : "{0} Bytes"), viewer.bytesOut);
            kvmCompInBytesLabel.Text = string.Format(((viewer.bytesInCompressed == 1) ? "{0} Byte" : "{0} Bytes"), viewer.bytesInCompressed);
            kvmCompOutBytesLabel.Text = string.Format(((viewer.bytesOutCompressed == 1) ? "{0} Byte" : "{0} Bytes"), viewer.bytesOutCompressed);
            if (viewer.bytesIn == 0) {
                inRatioLabel.Text = "0%";
            } else {
                inRatioLabel.Text = (100 - ((viewer.bytesInCompressed * 100) / viewer.bytesIn)) + "%";
            }
            if (viewer.bytesOut == 0) {
                outRatioLabel.Text = "0%";
            } else {
                outRatioLabel.Text = (100 - ((viewer.bytesOutCompressed * 100) / viewer.bytesOut)) + "%";
            }
        }

        private void KVMStats_FormClosing(object sender, FormClosingEventArgs e)
        {
            refreshTimer.Enabled = false;
        }

        private void KVMStats_Load(object sender, EventArgs e)
        {
            refreshTimer.Enabled = true;
            refreshTimer_Tick(this, null);
            Text = viewer.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            viewer.closeKvmStats();
        }
    }
}
