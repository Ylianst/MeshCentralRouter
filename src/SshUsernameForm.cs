﻿/*
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
    public partial class SshUsernameForm : Form
    {
        public SshUsernameForm()
        {
            InitializeComponent();
            Translate.TranslateControl(this);
        }

        public string Username
        {
            get { return usernameTextBox.Text; }
            set { usernameTextBox.Text = value; updateInfo(); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public void updateInfo()
        {
            okButton.Enabled = (usernameTextBox.Text.Length > 0) && (usernameTextBox.Text.IndexOf(" ") == -1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateInfo();
        }

        private void SshUsernameForm_Load(object sender, EventArgs e)
        {
            updateInfo();
            usernameTextBox.Focus();
        }
    }
}
