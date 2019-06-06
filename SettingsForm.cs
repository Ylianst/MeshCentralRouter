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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public bool BindAllInterfaces
        {
            get { return allInterfacesCheckBox.Checked; }
            set { allInterfacesCheckBox.Checked = value; }
        }
        public bool ShowSystemTray
        {
            get { return systemTrayCheckBox.Checked; }
            set { systemTrayCheckBox.Checked = value; }
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
