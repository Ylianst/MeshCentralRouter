namespace MeshCentralRouter
{
    partial class KVMControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // KVMControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "KVMControl";
            this.Size = new System.Drawing.Size(585, 364);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.KVMControl_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KVMControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.KVMControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KVMControl_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.Integration.ElementHost mainElementHost;

    }
}
