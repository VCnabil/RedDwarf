using LabJack;
using RedDwarf.RedAwarf._DataObjz.DataCOMM;
using RedDwarf.RedAwarf._Globalz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedDwarf.RedAwarf.UI
{
    public partial class testSectionForm : Form
    {
        int cnt_debug = 0;
        private bool isFormActive = true; // Flag to check form activity
         private readonly object lockObject = new object(); // Synchronization object
        DATA_TX _dataTX = new DATA_TX();
        LABJAK_RX _MAINLabjackObj;
       
        public testSectionForm()
        {
            InitializeComponent();
            MNGR_COMMBIV.Instance.TimerElapsed += Instance_TimerElapsed;
            this.FormClosing += new FormClosingEventHandler(FormClosing_Handler);
        }

        private void loopTimer_Tick(object sender, EventArgs e)
        {

        }

        private void Instance_TimerElapsed(bool argPortStatus)
        {
            if (InvokeRequired)
            {
                if (isFormActive) // Check if form is still active
                {
                    try // Try to invoke the action
                    {
                        Invoke(new Action(() =>
                        {
                            if (IsDisposed || label_debug.IsDisposed) { 
                               return;
                            }
                            SafeInvokedFunction();

                        }));
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }

                }
            }
        }

        void SafeInvokedFunction() {
            cnt_debug++;
            if (cnt_debug > 10000)
            {
                cnt_debug = 0;
            }
            label_debug.Text = "ticks " + cnt_debug;
            localWriteData();

            localReadData();


            string debugstr = MNGR_COMMBIV.Instance.AINs_values[0] + " " + MNGR_COMMBIV.Instance.AINs_values[1] + " " + MNGR_COMMBIV.Instance.AINs_values[2] + " " + MNGR_COMMBIV.Instance.AINs_values[3];
            lbl_helper.Text = debugstr;
        }
        void localWriteData()
        {
            _dataTX.SetDIO(cb_cmdDiO_0_led1.Checked, cb_cmdDiO_1_led2.Checked, cb_cmdDiO_2_alarm.Checked, true);
            MNGR_COMMBIV.Instance.WriteData__MBIV(_dataTX);
        }
        void localReadData()
        {
            _MAINLabjackObj = MNGR_COMMBIV.Instance.READDATA____JACK();
            _mustUpdate_ReadingsLabjackStatus();
        }

        void _mustUpdate_ReadingsLabjackStatus()
        {
            if (_MAINLabjackObj.LED1StaeOn )
            {
                lbl_LED1_EIO0.BackColor = Color.SeaGreen;
                lbl_LED1_EIO0.Text = "LED1 Actual Output: ON";
            }
            else
            {
                lbl_LED1_EIO0.BackColor = Color.Salmon;
                lbl_LED1_EIO0.Text = "LED1 Actual Output: ff";
            }

            if (_MAINLabjackObj.LED2StaeOn)
            {
                lbl_LED2_EIO1.BackColor = Color.SeaGreen;
                lbl_LED2_EIO1.Text = "LED2 Actual Output: ON";
            }
            else
            {
                lbl_LED2_EIO1.BackColor = Color.Salmon;
                lbl_LED2_EIO1.Text = "LED2 Actual Output: ff";
            }

            if (_MAINLabjackObj.AlarmStateON)
            {
                lbl_Alarm_AIN0.BackColor = Color.SeaGreen;
                lbl_Alarm_AIN0.Text = "Alarm: ON";
            }
            else
            {

                lbl_Alarm_AIN0.BackColor = Color.Salmon;
                lbl_Alarm_AIN0.Text = "Alarm: OFF";
            }
        }
        private void FormClosing_Handler(object sender, FormClosingEventArgs e)
        {
            isFormActive = false; // Set form as inactive
            MNGR_COMMBIV.Instance.TimerElapsed -= Instance_TimerElapsed;
        }
    }
}
