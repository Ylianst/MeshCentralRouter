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
