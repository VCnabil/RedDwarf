using RedDwarf.RedAwarf._DataObjz.DataCOMM;
using RedDwarf.RedAwarf._DataObjz.DataTestReport;
using RedDwarf.RedAwarf._Globalz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RedDwarf.RedAwarf._Globalz.Helpers;
namespace RedDwarf.RedAwarf
{
    public partial class NoJack : Form
    {
        private System.Windows.Forms.Timer KeepMBIValiveTimer = new System.Windows.Forms.Timer();
        DATA_TX DATA_TX;
        DATA_TESTREPORT _dataPAPAreport;
        MBIV_RX _myCopyofDataMBIV;
        int[] _ints_ADOS;
        public NoJack()
        {
            InitializeComponent();

            DATA_TX = new DATA_TX();
            _dataPAPAreport = new DATA_TESTREPORT(Get_MAX_AINs() + 1, Get_MAX_LVLS() + 1);
            MNGR_COMMBIV.Instance.FirstMEssageWasReceived += Instance_FirstMEssageWasReceived;
            btn_YesAuto.Click += Btn_YesAuto_Click;
            btn_NoManual.Click += Btn_NoAuto_Click;
            MNGR_COMMBIV.Instance.MessageReceived += Instance_MessageReceived;
            MNGR_COMMBIV.Instance.aLabJackDataReceived += Instance_aLabJackDataReceived;
            KeepMBIValiveTimer.Interval = 100;
            KeepMBIValiveTimer.Tick += new EventHandler(KeepMBIValiveTimer_Tick);
            KeepMBIValiveTimer.Start();
        }

        private void KeepMBIValiveTimer_Tick(object sender, EventArgs e)
        {
            DATA_TX.SetDIO(cb_cmdDiO_0_led1.Checked, cb_cmdDiO_1_led2.Checked, cb_cmdDiO_2_alarm.Checked, true);
            MNGR_COMMBIV.Instance.WriteData__MBIV(DATA_TX);
            lbl_TX.Text = "tx:" + DATA_TX.CREATE_FullString_for_TX();

      
            if(_ints_ADOS!=null)
            label_ain6.Text = "AIN6 : " + _ints_ADOS[6];
 

        }
        private void Instance_aLabJackDataReceived(string argSerial, string argFirmware)
        {
            // Directly use Invoke with a lambda expression to ensure the UI thread handles the updates.
            try
            {
                this.Invoke(new Action(() => {
                    label2_JackSerial.Text = "LabJack Serial Number : " + argSerial;
                    //only giveme the first 5 characters of the firmware
                    argFirmware = argFirmware.Substring(0, 5);
                    //  label3_JackFirm.Text = "LabJack Firmware ver   : " + argFirmware;
                    _dataPAPAreport.LabjackFirmwareVersion = argFirmware;
                    _dataPAPAreport.LabjackSerialNumber = argSerial;
                    // label0_conquestion.Text = "ALL Comunications Established";
                    // grouptests.Visible = true;
                }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating UI: " + ex.Message);
            }
        }
        private void Instance_MessageReceived(MBIV_RX message)
        {
            if (message == null) return;
            _myCopyofDataMBIV = message;
            // get latest data FYI DATA AT 0 IS NOT USED TO MAKE IT EASIER TO USE 1-16
            _ints_ADOS = _myCopyofDataMBIV.GET_allAINS();
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new Action(() =>
                    {
                        SafeInvokedFunctionOnRX(message);

                    }));
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error updating UI: " + ex.Message);
                }
            }
            else
            {

                try // Try to invoke the action
                {
                    SafeInvokedFunctionOnRX(message);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    //MessageBox.Show("Error updating UI: " + ex.Message);
                }
            }
        }
        void SafeInvokedFunctionOnRX(MBIV_RX message)
        {
            lbl_RX.Text = "RX: " + message.ToString();

        }

        private void Btn_NoAuto_Click(object sender, EventArgs e)
        {
            btn_NoManual.Visible = false;
            btn_YesAuto.Visible = false;
            lstCOMPorts.Visible = true;
            lstCOMPorts.Items.Clear();
            populatListbox();
            MNGR_COMMBIV.Instance.INIT_CON_LABJACK();
        }
        void populatListbox()
        {
            string[] ports = MNGR_COMMBIV.Instance.GetAvailablePortNames();
            foreach (string port in ports)
            {
                lstCOMPorts.Items.Add(port);
            }
        }
        private async void Btn_YesAuto_Click(object sender, EventArgs e)
        {
            MNGR_COMMBIV.Instance.INIT_CON_LABJACK();
            await Task.Delay(1000);
            lstCOMPorts.Visible = false;
            btn_NoManual.Visible = false;
            btn_YesAuto.Visible = false;
            MNGR_COMMBIV.Instance.OpenPort("COM7");


        }

        private void Instance_FirstMEssageWasReceived(string argVersion)
        {
            Invoke(new Action(() => {
                // Update UI controls safely
                label1_MBIVversion.Text = "first : " + argVersion;
                btn_YesAuto.Visible = false;
                btn_NoManual.Visible = false;
                lstCOMPorts.Visible = false;
                _dataPAPAreport.MBIV_SW_Version = argVersion;

            }));
        }
    }
}
