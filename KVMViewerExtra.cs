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
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace MeshCentralRouter
{
    public partial class KVMViewerExtra : Form
    {
        private MainForm parent = null;
        private KVMViewer mainViewer = null;
        private KVMControl kvmControl = null;
        public KVMControl mainKvmControl = null;
        private int displayId;
        public string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        public KVMViewerExtra(MainForm parent, KVMViewer mainViewer, NodeClass node, KVMControl mainKvmControl, int displayId)
        {
            this.parent = parent;
            this.mainViewer = mainViewer;
            this.mainKvmControl = mainKvmControl;
            this.displayId = displayId;
            InitializeComponent();
            Translate.TranslateControl(this);
            this.Text += " - " + node.name + " (" + displayId + ")";
            kvmControl = resizeKvmControl.KVM;
            kvmControl.desktop = mainKvmControl.desktop;
            kvmControl.parentEx = this;
            //kvmControl.Visible = true;
            kvmControl.DesktopSizeChanged += KvmControl_DesktopSizeChanged;
            kvmControl.cropDisplay(mainKvmControl.displayOrigin, mainKvmControl.displayInfo[displayId]);
            resizeKvmControl.ZoomToFit = true;
            this.MouseWheel += MainForm_MouseWheel;
        }

        public void UpdateScreenArea(Bitmap desktop, Rectangle r)
        {
            if (kvmControl.displayCrop.IntersectsWith(r) == false) return;
            Rectangle r2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
            r2.Intersect(kvmControl.displayCrop);
            //kvmControl.Repaint(new Rectangle(r2.X - kvmControl.displayCrop.X, r2.Y - kvmControl.displayCrop.Y, r2.Width, r2.Height));
            kvmControl.Repaint(null);
        }

        private void KvmControl_DesktopSizeChanged(object sender, EventArgs e)
        {
            //kvmControl.Visible = true;
        }

        void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0) return;
            Control c = this.GetChildAtPoint(e.Location);
            if (c != null && c == resizeKvmControl) resizeKvmControl.MouseWheelEx(sender, e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(820, 480);
            resizeKvmControl.CenterKvmControl(false);
            kvmControl.Repaint(null);
        }

        public void OnScreenChanged()
        {
            resizeKvmControl.CenterKvmControl(true);
        }

        private void resizeKvmControl_Enter(object sender, EventArgs e)
        {
            kvmControl.AttachKeyboard();
        }

        private void resizeKvmControl_Leave(object sender, EventArgs e)
        {
            kvmControl.DetacheKeyboard();
        }

        private void KVMViewer_Deactivate(object sender, EventArgs e)
        {
            kvmControl.DetacheKeyboard();
        }

        private void KVMViewer_Activated(object sender, EventArgs e)
        {
            kvmControl.AttachKeyboard();
        }

        bool isPointVisibleOnAScreen(Point p)
        {
            foreach (Screen s in Screen.AllScreens) { if ((p.X < s.Bounds.Right) && (p.X > s.Bounds.Left) && (p.Y > s.Bounds.Top) && (p.Y < s.Bounds.Bottom)) return true; }
            return false;
        }

        private void KVMViewerExtra_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainViewer.extraScreenClosed();
        }
    }
}
