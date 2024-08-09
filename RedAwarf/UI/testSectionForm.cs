using LabJack;
using RedDwarf.RedAwarf._Actionz;
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
            //  btn_startTest.Click += async (sender, e) => await StartTestSequence();
            btn_startTest.Click += btn_startTest_Click;
        }
        private void btn_startTest_Click(object sender, EventArgs e)
        {
            StartTestSequence().ConfigureAwait(false);
        }
        private async Task StartTestSequence()
        {
            var test = new TESTTest();
            // Define your actions
            test.TESTActions.Add(new TESTAction { DeviceName = "LED1", ValueToWrite = "LED1_ON", WaitTimeBeforeRead = 4000, ExpectedState = true });
            test.TESTActions.Add(new TESTAction { DeviceName = "LED1", ValueToWrite = "LED1_OFF", WaitTimeBeforeRead = 1000, ExpectedState = false });
            test.TESTActions.Add(new TESTAction { DeviceName = "LED2", ValueToWrite = "LED2_ON", WaitTimeBeforeRead = 4000, ExpectedState = true });
            test.TESTActions.Add(new TESTAction { DeviceName = "LED2", ValueToWrite = "LED2_OFF", WaitTimeBeforeRead = 1000, ExpectedState = false });
            test.TESTActions.Add(new TESTAction { DeviceName = "Alarm", ValueToWrite = "ALARM_ON", WaitTimeBeforeRead = 4000, ExpectedState = true });
            test.TESTActions.Add(new TESTAction { DeviceName = "Alarm", ValueToWrite = "ALARM_OFF", WaitTimeBeforeRead = 1000, ExpectedState = false });

            ClassActionz actionz = new ClassActionz();
            await actionz.RunTestAsync(test, WriteToDevice, ReadDeviceState);

            foreach (var action in test.TESTActions)
            {
                UpdateUIForResult(action.DeviceName, action.Result);
            }
        }


        private void WriteToDevice(string command)
        {
            // Implement actual device write logic here
            switch (command)
            {
                case "LED1_ON":
                    _dataTX.SetDIO(true, false, false, true); // Example: Turn LED1 ON
                    break;
                case "LED1_OFF":
                    _dataTX.SetDIO(false, false, false, true); // Example: Turn LED1 OFF
                    break;
                 case "LED2_ON":
                    _dataTX.SetDIO(false, true, false, true); // Example: Turn LED2 ON
                    break;
                    case "LED2_OFF":
                    _dataTX.SetDIO(false, false, false, true); // Example: Turn LED2 OFF
                    break;
                    case "ALARM_ON":
                    _dataTX.SetDIO(false, false, true, true); // Example: Turn ALARM ON
                    break;
                    case "ALARM_OFF":
                    _dataTX.SetDIO(false, false, false, true); // Example: Turn ALARM OFF
                    break;

            }
            MNGR_COMMBIV.Instance.WriteData__MBIV(_dataTX);
        }


        private bool ReadDeviceState(string deviceName)
        {
            // Handle device-specific read logic
            _MAINLabjackObj = MNGR_COMMBIV.Instance.READDATA____JACK();
           
            switch (deviceName)
            {
                case "LED1":
                    return _MAINLabjackObj.LED1StaeOn;
                case "LED2":
                    return _MAINLabjackObj.LED2StaeOn;
                case "Alarm":
                    return _MAINLabjackObj.AlarmStateON;
                default:
                    return false;
            }
        }


        private void UpdateUIForResult(string deviceName, bool result)
        {
            Label lbl = null;
            switch (deviceName)
            {
                case "LED1":
                    lbl = lbl_LED1_TestResult;
                    break;
                case "LED2":
                    lbl = lbl_LED2_TestResult;
                    break;
                case "Alarm":
                    lbl = lbl_Alarm_TestResult;
                    break;
            }

            if (lbl != null)
            {
                lbl.Text = result ? "Pass" : "Fail";
                lbl.BackColor = result ? Color.SeaGreen : Color.Salmon;
            }
        }

        private void UpdateUIForResultold(string deviceName, bool result)
        {
          switch (deviceName)
            {
                case "LED1":
                    lbl_LED1_EIO0.BackColor = result ? Color.SeaGreen : Color.Salmon;
                    lbl_LED1_EIO0.Text = result ? "LED1 Actual Output: ON" : "LED1 Actual Output: OFF";
                    break;
                case "LED2":
                    lbl_LED2_EIO1.BackColor = result ? Color.SeaGreen : Color.Salmon;
                    lbl_LED2_EIO1.Text = result ? "LED2 Actual Output: ON" : "LED2 Actual Output: OFF";
                    break;
                case "Alarm":
                    lbl_Alarm_AIN0.BackColor = result ? Color.SeaGreen : Color.Salmon;
                    lbl_Alarm_AIN0.Text = result ? "Alarm: ON" : "Alarm: OFF";
                    break;
            }

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
            //  localWriteData();

            // localReadData();

            showmeLabjackvals();

        }
        void showmeLabjackvals()
        {
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
            btn_startTest.Click -= btn_startTest_Click;

        }
    }
}
