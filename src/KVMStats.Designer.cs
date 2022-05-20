namespace MeshCentralRouter
{
    partial class KVMStats
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KVMStats));
            this.okButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.kvmCompInBytesLabel = new System.Windows.Forms.Label();
            this.kvmCompOutBytesLabel = new System.Windows.Forms.Label();
            this.kvmOutBytesLabel = new System.Windows.Forms.Label();
            this.kvmInBytesLabel = new System.Windows.Forms.Label();
            this.outRatioLabel = new System.Windows.Forms.Label();
            this.inRatioLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.outRatioLabel);
            this.groupBox1.Controls.Add(this.inRatioLabel);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.kvmCompOutBytesLabel);
            this.groupBox1.Controls.Add(this.kvmCompInBytesLabel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.kvmOutBytesLabel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.kvmInBytesLabel);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // kvmCompInBytesLabel
            // 
            resources.ApplyResources(this.kvmCompInBytesLabel, "kvmCompInBytesLabel");
            this.kvmCompInBytesLabel.Name = "kvmCompInBytesLabel";
            // 
            // kvmCompOutBytesLabel
            // 
            resources.ApplyResources(this.kvmCompOutBytesLabel, "kvmCompOutBytesLabel");
            this.kvmCompOutBytesLabel.Name = "kvmCompOutBytesLabel";
            // 
            // kvmOutBytesLabel
            // 
            resources.ApplyResources(this.kvmOutBytesLabel, "kvmOutBytesLabel");
            this.kvmOutBytesLabel.Name = "kvmOutBytesLabel";
            // 
            // kvmInBytesLabel
            // 
            resources.ApplyResources(this.kvmInBytesLabel, "kvmInBytesLabel");
            this.kvmInBytesLabel.Name = "kvmInBytesLabel";
            // 
            // outRatioLabel
            // 
            resources.ApplyResources(this.outRatioLabel, "outRatioLabel");
            this.outRatioLabel.Name = "outRatioLabel";
            // 
            // inRatioLabel
            // 
            resources.ApplyResources(this.inRatioLabel, "inRatioLabel");
            this.inRatioLabel.Name = "inRatioLabel";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 500;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // KVMStats
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KVMStats";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KVMStats_FormClosing);
            this.Load += new System.EventHandler(this.KVMStats_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label outRatioLabel;
        private System.Windows.Forms.Label inRatioLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label kvmCompOutBytesLabel;
        private System.Windows.Forms.Label kvmCompInBytesLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label kvmOutBytesLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label kvmInBytesLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer refreshTimer;
    }
}