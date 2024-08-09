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
using RedDwarf.RedAwarf._DataObjz.DataTestReport;
using RedDwarf.RedAwarf._Globalz;
using static RedDwarf.RedAwarf._Globalz.Helpers;

namespace RedDwarf
{
    public partial class FormLabBoss : Form
    {
        DATA_TESTREPORT _dataPAPAreport;
       
        public FormLabBoss()
        {
            InitializeComponent();
            cb_B0_b0.Checked = true;
            cb_B0_b1.Checked = true;
            cb_B0_b2.Checked = true;
            cb_B0_b3.Checked = true;
            cb_B0_b4.Checked = true;
            cb_B0_b5.Checked = true;
            cb_B0_b6.Checked = true;
            cb_B0_b7.Checked = true;
            cb_B1_b0.Checked = true;
            cb_B1_b1.Checked = true;
            cb_B1_b2.Checked = true;
            cb_B1_b3.Checked = true;
            cb_B1_b4.Checked = true;
            cb_B1_b5.Checked = true;
            cb_B1_b6.Checked = true;
            cb_B1_b7.Checked = true;
                


           _dataPAPAreport = new DATA_TESTREPORT(Get_MAX_AINs() + 1, Get_MAX_LVLS() + 1);
            MNGR_COMMBIV.Instance.aLabJackDataReceived += Instance_aLabJackDataReceived;
            btn_openLabjak.Click += Btn_openLabjak_Click;
            cb_B0_b0.CheckedChanged += aCheckBoxHasChanged;
            cb_B0_b1.CheckedChanged += aCheckBoxHasChanged;
            cb_B0_b2.CheckedChanged += aCheckBoxHasChanged;
            cb_B0_b3.CheckedChanged += aCheckBoxHasChanged;
            cb_B0_b4.CheckedChanged += aCheckBoxHasChanged;
            cb_B0_b5.CheckedChanged += aCheckBoxHasChanged;
            cb_B0_b6.CheckedChanged += aCheckBoxHasChanged;
            cb_B0_b7.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b0.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b1.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b2.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b3.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b4.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b5.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b6.CheckedChanged += aCheckBoxHasChanged;
            cb_B1_b7.CheckedChanged += aCheckBoxHasChanged;

            btn_RellaysON.Click += (sender, e) =>
            {
                MNGR_COMMBIV.Instance.turnonAllRelays();
            };
            btn_RellaysOFF.Click += (sender, e) =>
            {
                MNGR_COMMBIV.Instance.turnoffAllRelays();
            };

            btn_MainPowerOn.Click += OnPowerToggled_ON;
            btn_MainPowerOff.Click += OnPowerToggled_OFF;
            btn_RelayBlockVIN_on.Click += OnRelayBlockVIN_ON;
            btn_RelayBlockVIN_off.Click += OnRelayBlockVIN_OFF;
        }

        private void OnRelayBlockVIN_OFF(object sender, EventArgs e)
        {
            MNGR_COMMBIV.Instance.RelayBlock_powerOff();
        }

        private void OnRelayBlockVIN_ON(object sender, EventArgs e)
        {
           MNGR_COMMBIV.Instance.RelayBlock_powerOn();
        }

        private void OnPowerToggled_OFF(object sender, EventArgs e)
        {
            MNGR_COMMBIV.Instance.CloseMainPowerSource();
        }

        private void OnPowerToggled_ON(object sender, EventArgs e)
        {
            MNGR_COMMBIV.Instance.OpenMainPowerSource();
        }

        private void aCheckBoxHasChanged(object sender, EventArgs e)
        {

            byte _result_B0 = 0x00;

            if (cb_B0_b0.Checked) _result_B0 += 0x01;
            if (cb_B0_b1.Checked) _result_B0 += 0x02;
            if (cb_B0_b2.Checked) _result_B0 += 0x04;
            if (cb_B0_b3.Checked) _result_B0 += 0x08;
            if (cb_B0_b4.Checked) _result_B0 += 0x10;
            if (cb_B0_b5.Checked) _result_B0 += 0x20;
            if (cb_B0_b6.Checked) _result_B0 += 0x40;
            if (cb_B0_b7.Checked) _result_B0 += 0x80;

            label_Byte0.Text = _result_B0.ToString("X");
           // _lowByte = _result_B0;

            byte _result_B1 = 0x00;

            if (cb_B1_b0.Checked) _result_B1 += 0x01;
            if (cb_B1_b1.Checked) _result_B1 += 0x02;
            if (cb_B1_b2.Checked) _result_B1 += 0x04;
            if (cb_B1_b3.Checked) _result_B1 += 0x08;
            if (cb_B1_b4.Checked) _result_B1 += 0x10;
            if (cb_B1_b5.Checked) _result_B1 += 0x20;
            if (cb_B1_b6.Checked) _result_B1 += 0x40;
            if (cb_B1_b7.Checked) _result_B1 += 0x80;
            label_Byte1.Text = _result_B1.ToString("X");
           // _highByte = _result_B1;
           MNGR_COMMBIV.Instance.RELAYARRAY_SET(_result_B0, _result_B1);
        }

        private void Btn_openLabjak_Click(object sender, EventArgs e)
        {
            MNGR_COMMBIV.Instance.INIT_CON_LABJACK();
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
    }
}
