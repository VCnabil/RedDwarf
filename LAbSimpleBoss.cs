using RedDwarf.RedAwarf._DataObjz.DataTestReport;
using RedDwarf.RedAwarf._Globalz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabJack;
using static RedDwarf.RedAwarf._Globalz.Helpers;
using RedDwarf.RedAwarf._DataObjz.DataCOMM;
using RedDwarf.RedAwarf.UI.APPforms;


namespace RedDwarf
{
    public partial class LAbSimpleBoss : Form
    {
        DATA_TESTREPORT _dataPAPAreport;
        bool labjackIsOpen = false;
        bool MBIV_isConnected = false;
        MBIV_RX _myCopyofDataMBIV;
        int[] _ints_ADOS;


        public LAbSimpleBoss()
        {
            _ints_ADOS = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

            InitializeComponent();
            _dataPAPAreport = new DATA_TESTREPORT(Get_MAX_AINs() + 1, Get_MAX_LVLS() + 1);
            MNGR_COMMBIV.Instance.aLabJackDataReceived += Instance_aLabJackDataReceived;
            MNGR_COMMBIV.Instance.FirstMEssageWasReceived += Instance_FirstMEssageWasReceived;
            MNGR_COMMBIV.Instance.aPortHasOpened_orCloesdEVENT += HeardPortHasOpenedOrClsed;

            Load += LAbSimpleBoss_Load;
            FormClosing += LAbSimpleBoss_FormClosing;

            btn_YesAuto.Click += Btn_YesAuto_Click;
            btn_NoManual.Click += Btn_NoAuto_Click;
            lstCOMPorts.DoubleClick += lstCOMPorts_DoubleClick;
            lstCOMPorts.Visible = false;
            groupBox1.Hide();

            MNGR_COMMBIV.Instance.MessageReceived += Instance_MessageReceived;

            button1_starthere.Click += (sender, e) =>
            {
                AppFormRedReport testForm = new AppFormRedReport(_dataPAPAreport);
                testForm.Show();
            };

            button1_starthere.Hide();
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

            double val = tkb_DAC1.Value;
            double Converted_forDac1 = val / 100;
            lbl_DAC1.Text = "DAC1: " + Converted_forDac1.ToString();
            if (labjackIsOpen)
            {
                lbl_DAC1.BackColor = Color.Green;

            }
            else
            {
                lbl_DAC1.BackColor = Color.Red;
            }
            




        }
        void HeardPortHasOpenedOrClsed(string argPortName, bool argOpenedTrue_closedfalse)
        {
            if (argOpenedTrue_closedfalse)
            {
                // new testform_01;
                MBIV_isConnected    = true;
                button1_starthere.Show();
              //  MessageBox.Show("port " + argPortName + " has been opened.");
            }
            else
            {

                MBIV_isConnected = false;
                MessageBox.Show("Port " + argPortName + " has been closed.");
            }
        }
        private void Btn_NoAuto_Click(object sender, EventArgs e)
        {
            btn_NoManual.Visible = false;
            btn_YesAuto.Visible = false;
            lstCOMPorts.Visible = true;
            lstCOMPorts.Items.Clear();
            populatListbox();
        }

        private  async void Btn_YesAuto_Click(object sender, EventArgs e)
        {
          
            MNGR_COMMBIV.Instance.turnoffAllRelays();
            MNGR_COMMBIV.Instance.RelayBlock_powerOff();
            MNGR_COMMBIV.Instance.CloseMainPowerSource();
            await Task.Delay(500);
            MNGR_COMMBIV.Instance.OpenMainPowerSource();
            MNGR_COMMBIV.Instance.RelayBlock_powerOn();
            await Task.Delay(5000);
            lstCOMPorts.Visible = false;
            btn_NoManual.Visible = false;
            btn_YesAuto.Visible = false;
            MNGR_COMMBIV.Instance.OpenPort("COM7");
      

        }
        private void lstCOMPorts_DoubleClick(object sender, EventArgs e)
        {
            if (lstCOMPorts.SelectedItem != null)
            {
                string selectedPort = lstCOMPorts.SelectedItem.ToString();
                MNGR_COMMBIV.Instance.OpenPort(selectedPort);
            }
            else
            {
                MessageBox.Show("Please select a COM port from the list.");
            }
        }
        void populatListbox()
        {
            string[] ports = MNGR_COMMBIV.Instance.GetAvailablePortNames();
            foreach (string port in ports)
            {
                lstCOMPorts.Items.Add(port);
            }
        }


        int _labjackReceivedMEssages = 0;
        private void Instance_aLabJackDataReceived(string argSerial, string argFirmware)
        {
            try
            {
                this.Invoke(new Action(() => {
                    argFirmware = argFirmware.Substring(0, 5);
                    _dataPAPAreport.LabjackFirmwareVersion = argFirmware;
                    _dataPAPAreport.LabjackSerialNumber = argSerial;
                    if (argSerial.Contains("470033436"))
                    {
                        lbl_labjackOwner.Text = "VCinc";
                    }
                    else {
                        lbl_labjackOwner.Text = "NAbil";
                    }
                    labjackIsOpen = true;
                      _valueFromAIN0 = 0;
                      _samplesGatheed = 0;

                    get_AIN0_reading();
                    
                    // grouptests.Visible = true;
                }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating UI: " + ex.Message);
            }
        }

        double _valueFromAIN0 = 0;
        int _samplesGatheed = 0;
        List<double> SAMPLES= new List<double>();
        async void get_AIN0_reading() {


            MNGR_COMMBIV.Instance.SetDAC1_toHIgh();
            await Task.Delay(1000);

            while (_samplesGatheed<10)
            {

                _samplesGatheed++;

                MNGR_COMMBIV.Instance.SetDAC1_toHIgh();
               await Task.Delay(200);
                _valueFromAIN0 = MNGR_COMMBIV.Instance.Get_Value_AIN0();
                SAMPLES.Add(_valueFromAIN0);
            }
          
            double _average = SAMPLES.Average();
            lbl_cnt_labRec.Text = "voltage out : " + _average.ToString();
            if (SAMPLES.Count > 0)
            {
                SAMPLES.Clear();
            }

            if (_average > 4.87)
            {
                lbl_cnt_labRec.BackColor = Color.Green;
                groupBox1.Show();
            }
            else { 
                lbl_cnt_labRec.BackColor = Color.Red;
                MessageBox.Show("PLEASE  USE SS USB PORT");
            }


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

        private async void LAbSimpleBoss_Load(object sender, EventArgs e) 
        {
            await Task.Delay(1000);  
            MNGR_COMMBIV.Instance.INIT_CON_LABJACK();
         //   await Task.Delay(1000);
          
        }


        private async void LAbSimpleBoss_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (labjackIsOpen)
            {
                MNGR_COMMBIV.Instance.SetDAC1_toLow();
                MNGR_COMMBIV.Instance.turnoffAllRelays();
                MNGR_COMMBIV.Instance.RelayBlock_powerOff();
                MNGR_COMMBIV.Instance.CloseMainPowerSource();

                MNGR_COMMBIV.Instance.Close_LABJACK();
                await Task.Delay(1000);

               // MessageBox.Show("Form cloased abjack");
                labjackIsOpen = false;
            }
            else { 
                MessageBox.Show("Form Closing without closing labkjack");
            }

            btn_YesAuto.Click -= Btn_YesAuto_Click;
            btn_NoManual.Click -= Btn_NoAuto_Click;
            lstCOMPorts.DoubleClick -= lstCOMPorts_DoubleClick;
            MNGR_COMMBIV.Instance.aPortHasOpened_orCloesdEVENT -= HeardPortHasOpenedOrClsed;
            MNGR_COMMBIV.Instance.FirstMEssageWasReceived -= Instance_FirstMEssageWasReceived;
            MNGR_COMMBIV.Instance.aLabJackDataReceived -= Instance_aLabJackDataReceived;
        }
        
    }
}
