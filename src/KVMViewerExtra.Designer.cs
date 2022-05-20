using System.Windows.Forms;

namespace MeshCentralRouter
{
    partial class KVMViewerExtra
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KVMViewerExtra));
            this.resizeKvmControl = new MeshCentralRouter.KVMResizeControl();
            this.SuspendLayout();
            // 
            // resizeKvmControl
            // 
            this.resizeKvmControl.BackColor = System.Drawing.Color.Gray;
            resources.ApplyResources(this.resizeKvmControl, "resizeKvmControl");
            this.resizeKvmControl.Name = "resizeKvmControl";
            this.resizeKvmControl.ZoomToFit = false;
            this.resizeKvmControl.Enter += new System.EventHandler(this.resizeKvmControl_Enter);
            this.resizeKvmControl.Leave += new System.EventHandler(this.resizeKvmControl_Leave);
            // 
            // KVMViewerExtra
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.resizeKvmControl);
            this.Name = "KVMViewerExtra";
            this.Activated += new System.EventHandler(this.KVMViewer_Activated);
            this.Deactivate += new System.EventHandler(this.KVMViewer_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.KVMViewerExtra_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private KVMResizeControl resizeKvmControl;
    }
}

