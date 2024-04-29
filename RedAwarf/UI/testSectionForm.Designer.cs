namespace RedDwarf.RedAwarf.UI
{
    partial class testSectionForm
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
            this.lbl_LED2_TestResult = new System.Windows.Forms.Label();
            this.lbl_LED1_TestResult = new System.Windows.Forms.Label();
            this.lbl_Alarm_TestResult = new System.Windows.Forms.Label();
            this.GroupBox7 = new System.Windows.Forms.GroupBox();
            this.cb_cmdDiO_2_alarm = new System.Windows.Forms.CheckBox();
            this.cb_cmdDiO_1_led2 = new System.Windows.Forms.CheckBox();
            this.cb_cmdDiO_0_led1 = new System.Windows.Forms.CheckBox();
            this.lbl_Alarm_AIN0 = new System.Windows.Forms.Label();
            this.lbl_LED2_EIO1 = new System.Windows.Forms.Label();
            this.lbl_LED1_EIO0 = new System.Windows.Forms.Label();
            this.label_debug = new System.Windows.Forms.Label();
            this.lbl_helper = new System.Windows.Forms.Label();
            this.btn_startTest = new System.Windows.Forms.Button();
            this.GroupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_LED2_TestResult
            // 
            this.lbl_LED2_TestResult.AutoSize = true;
            this.lbl_LED2_TestResult.Location = new System.Drawing.Point(480, 217);
            this.lbl_LED2_TestResult.Name = "lbl_LED2_TestResult";
            this.lbl_LED2_TestResult.Size = new System.Drawing.Size(101, 25);
            this.lbl_LED2_TestResult.TabIndex = 670;
            this.lbl_LED2_TestResult.Text = "Pass/Fail";
            // 
            // lbl_LED1_TestResult
            // 
            this.lbl_LED1_TestResult.AutoSize = true;
            this.lbl_LED1_TestResult.Location = new System.Drawing.Point(480, 170);
            this.lbl_LED1_TestResult.Name = "lbl_LED1_TestResult";
            this.lbl_LED1_TestResult.Size = new System.Drawing.Size(101, 25);
            this.lbl_LED1_TestResult.TabIndex = 669;
            this.lbl_LED1_TestResult.Text = "Pass/Fail";
            // 
            // lbl_Alarm_TestResult
            // 
            this.lbl_Alarm_TestResult.AutoSize = true;
            this.lbl_Alarm_TestResult.Location = new System.Drawing.Point(480, 117);
            this.lbl_Alarm_TestResult.Name = "lbl_Alarm_TestResult";
            this.lbl_Alarm_TestResult.Size = new System.Drawing.Size(101, 25);
            this.lbl_Alarm_TestResult.TabIndex = 668;
            this.lbl_Alarm_TestResult.Text = "Pass/Fail";
            // 
            // GroupBox7
            // 
            this.GroupBox7.Controls.Add(this.cb_cmdDiO_2_alarm);
            this.GroupBox7.Controls.Add(this.cb_cmdDiO_1_led2);
            this.GroupBox7.Controls.Add(this.cb_cmdDiO_0_led1);
            this.GroupBox7.Location = new System.Drawing.Point(44, 87);
            this.GroupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox7.Name = "GroupBox7";
            this.GroupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox7.Size = new System.Drawing.Size(153, 176);
            this.GroupBox7.TabIndex = 664;
            this.GroupBox7.TabStop = false;
            this.GroupBox7.Text = "Digital Out";
            // 
            // cb_cmdDiO_2_alarm
            // 
            this.cb_cmdDiO_2_alarm.AutoSize = true;
            this.cb_cmdDiO_2_alarm.Location = new System.Drawing.Point(8, 41);
            this.cb_cmdDiO_2_alarm.Margin = new System.Windows.Forms.Padding(4);
            this.cb_cmdDiO_2_alarm.Name = "cb_cmdDiO_2_alarm";
            this.cb_cmdDiO_2_alarm.Size = new System.Drawing.Size(99, 29);
            this.cb_cmdDiO_2_alarm.TabIndex = 2;
            this.cb_cmdDiO_2_alarm.Text = "Alarm";
            this.cb_cmdDiO_2_alarm.UseVisualStyleBackColor = true;
            // 
            // cb_cmdDiO_1_led2
            // 
            this.cb_cmdDiO_1_led2.AutoSize = true;
            this.cb_cmdDiO_1_led2.Location = new System.Drawing.Point(8, 125);
            this.cb_cmdDiO_1_led2.Margin = new System.Windows.Forms.Padding(4);
            this.cb_cmdDiO_1_led2.Name = "cb_cmdDiO_1_led2";
            this.cb_cmdDiO_1_led2.Size = new System.Drawing.Size(103, 29);
            this.cb_cmdDiO_1_led2.TabIndex = 1;
            this.cb_cmdDiO_1_led2.Text = "LED 2";
            this.cb_cmdDiO_1_led2.UseVisualStyleBackColor = true;
            // 
            // cb_cmdDiO_0_led1
            // 
            this.cb_cmdDiO_0_led1.AutoSize = true;
            this.cb_cmdDiO_0_led1.Location = new System.Drawing.Point(8, 83);
            this.cb_cmdDiO_0_led1.Margin = new System.Windows.Forms.Padding(4);
            this.cb_cmdDiO_0_led1.Name = "cb_cmdDiO_0_led1";
            this.cb_cmdDiO_0_led1.Size = new System.Drawing.Size(103, 29);
            this.cb_cmdDiO_0_led1.TabIndex = 0;
            this.cb_cmdDiO_0_led1.Text = "LED 1";
            this.cb_cmdDiO_0_led1.UseVisualStyleBackColor = true;
            // 
            // lbl_Alarm_AIN0
            // 
            this.lbl_Alarm_AIN0.AutoSize = true;
            this.lbl_Alarm_AIN0.Location = new System.Drawing.Point(238, 117);
            this.lbl_Alarm_AIN0.Name = "lbl_Alarm_AIN0";
            this.lbl_Alarm_AIN0.Size = new System.Drawing.Size(123, 25);
            this.lbl_Alarm_AIN0.TabIndex = 667;
            this.lbl_Alarm_AIN0.Text = "Alarm is Off";
            // 
            // lbl_LED2_EIO1
            // 
            this.lbl_LED2_EIO1.AutoSize = true;
            this.lbl_LED2_EIO1.Location = new System.Drawing.Point(238, 217);
            this.lbl_LED2_EIO1.Name = "lbl_LED2_EIO1";
            this.lbl_LED2_EIO1.Size = new System.Drawing.Size(121, 25);
            this.lbl_LED2_EIO1.TabIndex = 666;
            this.lbl_LED2_EIO1.Text = "LED2 is Off";
            // 
            // lbl_LED1_EIO0
            // 
            this.lbl_LED1_EIO0.AutoSize = true;
            this.lbl_LED1_EIO0.Location = new System.Drawing.Point(238, 170);
            this.lbl_LED1_EIO0.Name = "lbl_LED1_EIO0";
            this.lbl_LED1_EIO0.Size = new System.Drawing.Size(121, 25);
            this.lbl_LED1_EIO0.TabIndex = 665;
            this.lbl_LED1_EIO0.Text = "LED1 is Off";
            // 
            // label_debug
            // 
            this.label_debug.AutoSize = true;
            this.label_debug.Location = new System.Drawing.Point(798, 24);
            this.label_debug.Name = "label_debug";
            this.label_debug.Size = new System.Drawing.Size(101, 25);
            this.label_debug.TabIndex = 671;
            this.label_debug.Text = "Pass/Fail";
            // 
            // lbl_helper
            // 
            this.lbl_helper.AutoSize = true;
            this.lbl_helper.Location = new System.Drawing.Point(47, 342);
            this.lbl_helper.Name = "lbl_helper";
            this.lbl_helper.Size = new System.Drawing.Size(449, 25);
            this.lbl_helper.TabIndex = 672;
            this.lbl_helper.Text = "read the AIN0 EIO0 EIO1 from labjack register";
            // 
            // button1
            // 
            this.btn_startTest.Location = new System.Drawing.Point(853, 118);
            this.btn_startTest.Name = "button1";
            this.btn_startTest.Size = new System.Drawing.Size(162, 87);
            this.btn_startTest.TabIndex = 673;
            this.btn_startTest.Text = "button1";
            this.btn_startTest.UseVisualStyleBackColor = true;
            // 
            // testSectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1832, 1160);
            this.Controls.Add(this.btn_startTest);
            this.Controls.Add(this.lbl_helper);
            this.Controls.Add(this.label_debug);
            this.Controls.Add(this.lbl_LED2_TestResult);
            this.Controls.Add(this.lbl_LED1_TestResult);
            this.Controls.Add(this.lbl_Alarm_TestResult);
            this.Controls.Add(this.GroupBox7);
            this.Controls.Add(this.lbl_Alarm_AIN0);
            this.Controls.Add(this.lbl_LED2_EIO1);
            this.Controls.Add(this.lbl_LED1_EIO0);
            this.Name = "testSectionForm";
            this.Text = "testSectionForm";
            this.GroupBox7.ResumeLayout(false);
            this.GroupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_LED2_TestResult;
        private System.Windows.Forms.Label lbl_LED1_TestResult;
        private System.Windows.Forms.Label lbl_Alarm_TestResult;
        internal System.Windows.Forms.GroupBox GroupBox7;
        internal System.Windows.Forms.CheckBox cb_cmdDiO_2_alarm;
        internal System.Windows.Forms.CheckBox cb_cmdDiO_1_led2;
        internal System.Windows.Forms.CheckBox cb_cmdDiO_0_led1;
        private System.Windows.Forms.Label lbl_Alarm_AIN0;
        private System.Windows.Forms.Label lbl_LED2_EIO1;
        private System.Windows.Forms.Label lbl_LED1_EIO0;
        private System.Windows.Forms.Label label_debug;
        private System.Windows.Forms.Label lbl_helper;
        private System.Windows.Forms.Button btn_startTest;
    }
}