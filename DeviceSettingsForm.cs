using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class DeviceSettingsForm : Form
    {
        public DeviceSettingsForm()
        {
            InitializeComponent();
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
