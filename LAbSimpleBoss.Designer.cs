namespace RedDwarf
{
    partial class LAbSimpleBoss
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
            this.lbl_labjackOwner = new System.Windows.Forms.Label();
            this.label1_MBIVversion = new System.Windows.Forms.Label();
            this.lstCOMPorts = new System.Windows.Forms.ListBox();
            this.btn_NoManual = new System.Windows.Forms.Button();
            this.btn_YesAuto = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1_starthere = new System.Windows.Forms.Button();
            this.lbl_RX = new System.Windows.Forms.Label();
            this.lbl_DAC1 = new System.Windows.Forms.Label();
            this.tkb_DAC1 = new System.Windows.Forms.TrackBar();
            this.lbl_cnt_labRec = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_DAC1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_labjackOwner
            // 
            this.lbl_labjackOwner.AutoSize = true;
            this.lbl_labjackOwner.Location = new System.Drawing.Point(4, 49);
            this.lbl_labjackOwner.Name = "lbl_labjackOwner";
            this.lbl_labjackOwner.Size = new System.Drawing.Size(70, 25);
            this.lbl_labjackOwner.TabIndex = 0;
            this.lbl_labjackOwner.Text = "label1";
            // 
            // label1_MBIVversion
            // 
            this.label1_MBIVversion.AutoSize = true;
            this.label1_MBIVversion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1_MBIVversion.Location = new System.Drawing.Point(6, 27);
            this.label1_MBIVversion.Margin = new System.Windows.Forms.Padding(0);
            this.label1_MBIVversion.Name = "label1_MBIVversion";
            this.label1_MBIVversion.Size = new System.Drawing.Size(111, 39);
            this.label1_MBIVversion.TabIndex = 661;
            this.label1_MBIVversion.Text = "version";
            // 
            // lstCOMPorts
            // 
            this.lstCOMPorts.FormattingEnabled = true;
            this.lstCOMPorts.ItemHeight = 25;
            this.lstCOMPorts.Location = new System.Drawing.Point(102, 89);
            this.lstCOMPorts.Name = "lstCOMPorts";
            this.lstCOMPorts.Size = new System.Drawing.Size(173, 79);
            this.lstCOMPorts.TabIndex = 660;
            // 
            // btn_NoManual
            // 
            this.btn_NoManual.Location = new System.Drawing.Point(6, 129);
            this.btn_NoManual.Margin = new System.Windows.Forms.Padding(0);
            this.btn_NoManual.Name = "btn_NoManual";
            this.btn_NoManual.Size = new System.Drawing.Size(93, 39);
            this.btn_NoManual.TabIndex = 659;
            this.btn_NoManual.Text = "NO";
            this.btn_NoManual.UseVisualStyleBackColor = true;
            // 
            // btn_YesAuto
            // 
            this.btn_YesAuto.Location = new System.Drawing.Point(6, 71);
            this.btn_YesAuto.Margin = new System.Windows.Forms.Padding(0);
            this.btn_YesAuto.Name = "btn_YesAuto";
            this.btn_YesAuto.Size = new System.Drawing.Size(93, 39);
            this.btn_YesAuto.TabIndex = 658;
            this.btn_YesAuto.Text = "YES";
            this.btn_YesAuto.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1_starthere);
            this.groupBox1.Controls.Add(this.btn_YesAuto);
            this.groupBox1.Controls.Add(this.label1_MBIVversion);
            this.groupBox1.Controls.Add(this.btn_NoManual);
            this.groupBox1.Controls.Add(this.lstCOMPorts);
            this.groupBox1.Location = new System.Drawing.Point(3, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(530, 200);
            this.groupBox1.TabIndex = 662;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AutoConnect?";
            // 
            // button1_starthere
            // 
            this.button1_starthere.Location = new System.Drawing.Point(281, 104);
            this.button1_starthere.Name = "button1_starthere";
            this.button1_starthere.Size = new System.Drawing.Size(206, 64);
            this.button1_starthere.TabIndex = 703;
            this.button1_starthere.Text = "starthere";
            this.button1_starthere.UseVisualStyleBackColor = true;
            // 
            // lbl_RX
            // 
            this.lbl_RX.AutoSize = true;
            this.lbl_RX.Location = new System.Drawing.Point(11, 9);
            this.lbl_RX.Name = "lbl_RX";
            this.lbl_RX.Size = new System.Drawing.Size(36, 25);
            this.lbl_RX.TabIndex = 702;
            this.lbl_RX.Text = "rx:";
            // 
            // lbl_DAC1
            // 
            this.lbl_DAC1.AutoSize = true;
            this.lbl_DAC1.Location = new System.Drawing.Point(695, 181);
            this.lbl_DAC1.Name = "lbl_DAC1";
            this.lbl_DAC1.Size = new System.Drawing.Size(70, 25);
            this.lbl_DAC1.TabIndex = 719;
            this.lbl_DAC1.Text = "label1";
            this.lbl_DAC1.Visible = false;
            // 
            // tkb_DAC1
            // 
            this.tkb_DAC1.Location = new System.Drawing.Point(875, 178);
            this.tkb_DAC1.Maximum = 500;
            this.tkb_DAC1.Name = "tkb_DAC1";
            this.tkb_DAC1.Size = new System.Drawing.Size(553, 90);
            this.tkb_DAC1.TabIndex = 718;
            this.tkb_DAC1.Value = 1;
            this.tkb_DAC1.Visible = false;
            // 
            // lbl_cnt_labRec
            // 
            this.lbl_cnt_labRec.AutoSize = true;
            this.lbl_cnt_labRec.Location = new System.Drawing.Point(1563, 9);
            this.lbl_cnt_labRec.Name = "lbl_cnt_labRec";
            this.lbl_cnt_labRec.Size = new System.Drawing.Size(95, 25);
            this.lbl_cnt_labRec.TabIndex = 720;
            this.lbl_cnt_labRec.Text = "labreced";
            // 
            // LAbSimpleBoss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2006, 790);
            this.Controls.Add(this.lbl_cnt_labRec);
            this.Controls.Add(this.lbl_DAC1);
            this.Controls.Add(this.tkb_DAC1);
            this.Controls.Add(this.lbl_RX);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_labjackOwner);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LAbSimpleBoss";
            this.Text = "LAbSimpleBoss";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_DAC1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_labjackOwner;
        private System.Windows.Forms.Label label1_MBIVversion;
        private System.Windows.Forms.ListBox lstCOMPorts;
        private System.Windows.Forms.Button btn_NoManual;
        private System.Windows.Forms.Button btn_YesAuto;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_RX;
        private System.Windows.Forms.Button button1_starthere;
        private System.Windows.Forms.Label lbl_DAC1;
        private System.Windows.Forms.TrackBar tkb_DAC1;
        private System.Windows.Forms.Label lbl_cnt_labRec;
    }
}