/*
Copyright 2009-2022 Intel Corporation

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
    public partial class DeviceSettingsForm : Form
    {
        public DeviceSettingsForm()
        {
            InitializeComponent();
            Translate.TranslateControl(this);
            doubleClickComboBox.SelectedIndex = 0;
        }

        public int deviceDoubleClickAction
        {
            get { return doubleClickComboBox.SelectedIndex; }
            set { doubleClickComboBox.SelectedIndex = value; }
        }

        public bool ShowSystemTray
        {
            get { return systemTrayCheckBox.Checked; }
            set { systemTrayCheckBox.Checked = value; }
        }

        public bool CheckForUpdates
        {
            get { return checkForUpdatedCheckBox.Checked; }
            set { checkForUpdatedCheckBox.Checked = value; }
        }

        public bool CollapseDeviceGroups
        {
            get { return collapseCheckBox.Checked; }
            set { collapseCheckBox.Checked = value; }
        }

        public bool Exp_KeyboardHook
        {
            get { return exp_KeyboardHookCheckBox.Checked; }
            set
            {
                exp_KeyboardHookCheckBox.Checked = value;
                if (!value)
                {
                    exp_KeyboardHookPriorityCheckBox.Checked = false;
                    exp_KeyboardHookPriorityCheckBox.Enabled = false;
                }
            }
        }

        public bool Exp_KeyboardHookPriority
        {
            get { return exp_KeyboardHookPriorityCheckBox.Checked; }
            set { exp_KeyboardHookPriorityCheckBox.Checked = value; }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void exp_KeyboardHookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (exp_KeyboardHookCheckBox.Checked)
            {
                exp_KeyboardHookPriorityCheckBox.Enabled = true;
            }
            else
            {
                exp_KeyboardHookPriorityCheckBox.Checked = false;
                exp_KeyboardHookPriorityCheckBox.Enabled = false;
            }
        }
    }
}
