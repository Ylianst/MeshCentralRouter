/*
Copyright 2009-2011 Intel Corporation

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
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace MeshCentralRouter
{
    public partial class KVMSettingsForm : Form
    {
        public KVMSettingsForm()
        {
            InitializeComponent();
            qualityComboBox.Items.Add(new DropListItem(90, "90%"));
            qualityComboBox.Items.Add(new DropListItem(80, "80%"));
            qualityComboBox.Items.Add(new DropListItem(70, "70%"));
            qualityComboBox.Items.Add(new DropListItem(60, "60%"));
            qualityComboBox.Items.Add(new DropListItem(50, "50%"));
            qualityComboBox.Items.Add(new DropListItem(40, "40%"));
            qualityComboBox.Items.Add(new DropListItem(30, "30%"));
            qualityComboBox.Items.Add(new DropListItem(20, "20%"));
            qualityComboBox.Items.Add(new DropListItem(10, "10%"));
            qualityComboBox.Items.Add(new DropListItem(5, "5%"));
            qualityComboBox.Items.Add(new DropListItem(1, "1%"));
            scalingComboBox.Items.Add(new DropListItem(1024, "100%"));
            scalingComboBox.Items.Add(new DropListItem(896, "87.5%"));
            scalingComboBox.Items.Add(new DropListItem(768, "75%"));
            scalingComboBox.Items.Add(new DropListItem(640, "62.5%"));
            scalingComboBox.Items.Add(new DropListItem(512, "50%"));
            scalingComboBox.Items.Add(new DropListItem(384, "37.5%"));
            scalingComboBox.Items.Add(new DropListItem(256, "25%"));
            scalingComboBox.Items.Add(new DropListItem(128, "12.5%"));
            frameRateComboBox.Items.Add(new DropListItem(50, "Fast"));
            frameRateComboBox.Items.Add(new DropListItem(100, "Medium"));
            frameRateComboBox.Items.Add(new DropListItem(400, "Slow"));
            frameRateComboBox.Items.Add(new DropListItem(1000, "Very slow"));
            qualityComboBox.SelectedIndex = 4;
            scalingComboBox.SelectedIndex = 0;
            frameRateComboBox.SelectedIndex = 1;
        }

        private class DropListItem
        {
            public int value;
            public string str;

            public DropListItem(int value, string str) { this.value = value; this.str = str; }

            public override string ToString() { return str; }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
        }

        public int Compression
        {
            get { return ((DropListItem)qualityComboBox.SelectedItem).value; }
            set
            {
                if (value >= 90) { qualityComboBox.SelectedIndex = 0; return; }
                if (value >= 80) { qualityComboBox.SelectedIndex = 1; return; }
                if (value >= 70) { qualityComboBox.SelectedIndex = 2; return; }
                if (value >= 60) { qualityComboBox.SelectedIndex = 3; return; }
                if (value >= 50) { qualityComboBox.SelectedIndex = 4; return; }
                if (value >= 40) { qualityComboBox.SelectedIndex = 5; return; }
                if (value >= 30) { qualityComboBox.SelectedIndex = 6; return; }
                if (value >= 20) { qualityComboBox.SelectedIndex = 7; return; }
                if (value >= 10) { qualityComboBox.SelectedIndex = 8; return; }
                if (value >= 5) { qualityComboBox.SelectedIndex = 9; return; }
                qualityComboBox.SelectedIndex = 10;
            }
        }

        public int Scaling
        {
            get { return ((DropListItem)scalingComboBox.SelectedItem).value; }
            set
            {
                if (value >= 1024) { scalingComboBox.SelectedIndex = 0; return; }
                if (value >= 896) { scalingComboBox.SelectedIndex = 1; return; }
                if (value >= 768) { scalingComboBox.SelectedIndex = 2; return; }
                if (value >= 640) { scalingComboBox.SelectedIndex = 3; return; }
                if (value >= 512) { scalingComboBox.SelectedIndex = 4; return; }
                if (value >= 384) { scalingComboBox.SelectedIndex = 5; return; }
                if (value >= 256) { scalingComboBox.SelectedIndex = 6; return; }
                scalingComboBox.SelectedIndex = 7;
            }
        }

        public int FrameRate
        {
            get { return ((DropListItem)frameRateComboBox.SelectedItem).value; }
            set
            {
                if (value <= 50) { frameRateComboBox.SelectedIndex = 0; return; }
                if (value <= 100) { frameRateComboBox.SelectedIndex = 1; return; }
                if (value <= 400) { frameRateComboBox.SelectedIndex = 2; return; }
                frameRateComboBox.SelectedIndex = 3;
            }
        }

        public bool SwamMouseButtons
        {
            get { return swapMouseButtonsCheckBox.Checked; }
            set { swapMouseButtonsCheckBox.Checked = value; }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
