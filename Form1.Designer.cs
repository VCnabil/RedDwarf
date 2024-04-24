namespace RedDwarf
{
    partial class Form1
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
            this.label0 = new System.Windows.Forms.Label();
            this.btn_YesAuto = new System.Windows.Forms.Button();
            this.btn_NoManual = new System.Windows.Forms.Button();
            this.lstCOMPorts = new System.Windows.Forms.ListBox();
            this.label1_MBIVversion = new System.Windows.Forms.Label();
            this.label2_JackSerial = new System.Windows.Forms.Label();
            this.label3_JackFirm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label0
            // 
            this.label0.AutoSize = true;
            this.label0.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label0.Location = new System.Drawing.Point(9, 9);
            this.label0.Margin = new System.Windows.Forms.Padding(0);
            this.label0.Name = "label0";
            this.label0.Size = new System.Drawing.Size(212, 39);
            this.label0.TabIndex = 641;
            this.label0.Text = "Auto Connect?";
            // 
            // btn_YesAuto
            // 
            this.btn_YesAuto.Location = new System.Drawing.Point(221, 9);
            this.btn_YesAuto.Margin = new System.Windows.Forms.Padding(0);
            this.btn_YesAuto.Name = "btn_YesAuto";
            this.btn_YesAuto.Size = new System.Drawing.Size(93, 39);
            this.btn_YesAuto.TabIndex = 650;
            this.btn_YesAuto.Text = "YES";
            this.btn_YesAuto.UseVisualStyleBackColor = true;
            // 
            // btn_NoManual
            // 
            this.btn_NoManual.Location = new System.Drawing.Point(314, 9);
            this.btn_NoManual.Margin = new System.Windows.Forms.Padding(0);
            this.btn_NoManual.Name = "btn_NoManual";
            this.btn_NoManual.Size = new System.Drawing.Size(93, 39);
            this.btn_NoManual.TabIndex = 651;
            this.btn_NoManual.Text = "NO";
            this.btn_NoManual.UseVisualStyleBackColor = true;
            // 
            // lstCOMPorts
            // 
            this.lstCOMPorts.FormattingEnabled = true;
            this.lstCOMPorts.ItemHeight = 25;
            this.lstCOMPorts.Location = new System.Drawing.Point(410, 12);
            this.lstCOMPorts.Name = "lstCOMPorts";
            this.lstCOMPorts.Size = new System.Drawing.Size(147, 79);
            this.lstCOMPorts.TabIndex = 652;
            // 
            // label1_MBIVversion
            // 
            this.label1_MBIVversion.AutoSize = true;
            this.label1_MBIVversion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1_MBIVversion.Location = new System.Drawing.Point(9, 52);
            this.label1_MBIVversion.Margin = new System.Windows.Forms.Padding(0);
            this.label1_MBIVversion.Name = "label1_MBIVversion";
            this.label1_MBIVversion.Size = new System.Drawing.Size(111, 39);
            this.label1_MBIVversion.TabIndex = 653;
            this.label1_MBIVversion.Text = "version";
            // 
            // label2_JackSerial
            // 
            this.label2_JackSerial.AutoSize = true;
            this.label2_JackSerial.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2_JackSerial.Location = new System.Drawing.Point(9, 91);
            this.label2_JackSerial.Margin = new System.Windows.Forms.Padding(0);
            this.label2_JackSerial.Name = "label2_JackSerial";
            this.label2_JackSerial.Size = new System.Drawing.Size(88, 39);
            this.label2_JackSerial.TabIndex = 656;
            this.label2_JackSerial.Text = "Serial";
            // 
            // label3_JackFirm
            // 
            this.label3_JackFirm.AutoSize = true;
            this.label3_JackFirm.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3_JackFirm.Location = new System.Drawing.Point(9, 130);
            this.label3_JackFirm.Margin = new System.Windows.Forms.Padding(0);
            this.label3_JackFirm.Name = "label3_JackFirm";
            this.label3_JackFirm.Size = new System.Drawing.Size(136, 39);
            this.label3_JackFirm.TabIndex = 657;
            this.label3_JackFirm.Text = "firmware";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3_JackFirm);
            this.Controls.Add(this.label2_JackSerial);
            this.Controls.Add(this.label1_MBIVversion);
            this.Controls.Add(this.lstCOMPorts);
            this.Controls.Add(this.btn_NoManual);
            this.Controls.Add(this.btn_YesAuto);
            this.Controls.Add(this.label0);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label0;
        private System.Windows.Forms.Button btn_YesAuto;
        private System.Windows.Forms.Button btn_NoManual;
        private System.Windows.Forms.ListBox lstCOMPorts;
        private System.Windows.Forms.Label label1_MBIVversion;
        private System.Windows.Forms.Label label2_JackSerial;
        private System.Windows.Forms.Label label3_JackFirm;
    }
}

