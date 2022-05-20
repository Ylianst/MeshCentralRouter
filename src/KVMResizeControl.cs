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
using System.ComponentModel;

namespace MeshCentralRouter
{
    public partial class KVMResizeControl : UserControl
    {
        private bool zoomtofit = false;

        public KVMControl KVM { get { return kvmControl; } }
        public bool ZoomToFit { get { return zoomtofit; } set { zoomtofit = value; CenterKvmControl(false); } }
        
        [Category("Action")]
        [Description("Fires when the connection state changes.")]
        public event EventHandler StateChanged;

        [Category("Action")]
        [Description("Fires when the display list is received.")]
        public event EventHandler DisplaysReceived;

        public KVMResizeControl()
        {
            InitializeComponent();
            vScrollBar.LargeChange = 100;
            vScrollBar.SmallChange = 10;
            hScrollBar.LargeChange = 100;
            hScrollBar.SmallChange = 10;
            //KVM.StateChanged += new EventHandler(KVM_StateChanged);
            KVM.DisplaysReceived += new EventHandler(KVM_DisplaysReceived);
            CenterKvmControl(false);
        }

        /*
        void KVM_StateChanged(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new EventHandler(KVM_StateChanged), sender, e); return; }
            CenterKvmControl(false);
            kvmControl.Visible = false;
            //kvmControl.Visible = (kvmControl.State == KVMControl.ConnectState.Connected);
            if (StateChanged != null) StateChanged(this, e);
        }
        */

        void KVM_DisplaysReceived(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new EventHandler(KVM_DisplaysReceived), sender, e); return; }
            if (DisplaysReceived != null) DisplaysReceived(this, e);
        }

        public void CenterKvmControl(bool forceRefresh)
        {
            int w = clientPanel.Width;
            int h = clientPanel.Height;
            int l = 0;
            int t = 0;
            bool invalidate = true;

            if (zoomtofit == false)
            {
                if (kvmControl.Width != kvmControl.DesktopWidth || kvmControl.Height != kvmControl.DesktopHeight || kvmControl.ScaleFactor != 1)
                {
                    kvmControl.Size = new System.Drawing.Size(kvmControl.DesktopWidth, kvmControl.DesktopHeight);
                    kvmControl.ScaleFactor = 1;
                    invalidate = true;
                }
                hScrollBar.Visible = (w < kvmControl.DesktopWidth);
                rightScrollPanel.Visible = vScrollBar.Visible = (h < kvmControl.DesktopHeight);
                cornerBlockPanel.Visible = (hScrollBar.Visible & vScrollBar.Visible);
            }
            else
            {
                hScrollBar.Visible = false;
                rightScrollPanel.Visible = false;
                double sfx = (double)kvmControl.DesktopWidth / (double)Width;
                double sfy = (double)kvmControl.DesktopHeight / (double)Height;
                double sf = Math.Max(sfx, sfy);
                sf = Math.Max(1, sf);
                if ((kvmControl.ScaleFactor != sf) || (forceRefresh))
                {
                    kvmControl.Size = new System.Drawing.Size((int)((double)kvmControl.DesktopWidth / sf), (int)((double)kvmControl.DesktopHeight / sf));
                    kvmControl.ScaleFactor = sf;
                    invalidate = true;
                }
            }

            if (w < kvmControl.Width) { hScrollBar.Maximum = (kvmControl.Width - w) + 100; } else { l = (w - kvmControl.Width) / 2; }
            if (h < kvmControl.Height) { vScrollBar.Maximum = (kvmControl.Height - h) + 100; } else { t = (h - kvmControl.Height) / 2; }
            if (vScrollBar.Visible) { kvmControl.Top = 0 - vScrollBar.Value; } else { kvmControl.Top = t; }
            if (hScrollBar.Visible) { kvmControl.Left = 0 - hScrollBar.Value; } else { kvmControl.Left = l; }
            if (invalidate) kvmControl.Invalidate();
        }

        private void kvmControl_Resize(object sender, EventArgs e)
        {
            CenterKvmControl(false);
        }

        private void ResizeKVMControl_Resize(object sender, EventArgs e)
        {
            CenterKvmControl(false);
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            kvmControl.Top = -vScrollBar.Value;
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            kvmControl.Left = -hScrollBar.Value;
        }

        private void kvmControl_DesktopSizeChanged(object sender, EventArgs e)
        {
            CenterKvmControl(true);
        }

        public void MouseWheelEx(object sender, MouseEventArgs e)
        {
            //if (KVMControl.WPF) kvmControl.SendMouseWheel(e, 0);
        }
    }
}
