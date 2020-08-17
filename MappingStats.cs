using System;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class MappingStats : Form
    {
        public MapUserControl mapControl;

        public MappingStats(MapUserControl mapControl)
        {
            this.mapControl = mapControl;
            InitializeComponent();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            kvmInBytesLabel.Text = string.Format(((mapControl.mapper.bytesToClient == 1)?"{0} Byte":"{0} Bytes"), mapControl.mapper.bytesToClient);
            kvmOutBytesLabel.Text = string.Format(((mapControl.mapper.bytesToServer == 1) ? "{0} Byte" : "{0} Bytes"), mapControl.mapper.bytesToServer);
            kvmCompInBytesLabel.Text = string.Format(((mapControl.mapper.bytesToClientCompressed == 1) ? "{0} Byte" : "{0} Bytes"), mapControl.mapper.bytesToClientCompressed);
            kvmCompOutBytesLabel.Text = string.Format(((mapControl.mapper.bytesToServerCompressed == 1) ? "{0} Byte" : "{0} Bytes"), mapControl.mapper.bytesToServerCompressed);
            if (mapControl.mapper.bytesToClient == 0) {
                inRatioLabel.Text = "0%";
            } else {
                inRatioLabel.Text = (100 - ((mapControl.mapper.bytesToClientCompressed * 100) / mapControl.mapper.bytesToClient)) + "%";
            }
            if (mapControl.mapper.bytesToServer == 0) {
                outRatioLabel.Text = "0%";
            } else {
                outRatioLabel.Text = (100 - ((mapControl.mapper.bytesToServerCompressed * 100) / mapControl.mapper.bytesToServer)) + "%";
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
            Text += " - " + mapControl.node.name;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            mapControl.closeStats();
        }
    }
}
