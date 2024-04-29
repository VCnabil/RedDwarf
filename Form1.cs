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
using System.Diagnostics;
using RedDwarf.RedAwarf.UI;
using RedDwarf.RedAwarf.UI.SectionsPages;

namespace RedDwarf
{
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
            label2_JackSerial.Text = "Initial Test";
            label3_JackFirm.Text = "Initial Test";
            btn_YesAuto.Click += Btn_YesAuto_Click;
            btn_NoManual.Click += Btn_NoAuto_Click;
            lstCOMPorts.DoubleClick += lstCOMPorts_DoubleClick;
            lstCOMPorts.Visible = false;
            grouptests.Visible = false;

            button_NewTestForm.Click += (sender, e) =>
            {
                //NewTestForm newTestForm = new NewTestForm();
                //newTestForm.Show();
                testSectionForm testSectionForm = new testSectionForm();
                testSectionForm.Show();
            };

            btn_Section3ain.Click += (sender, e) =>
            {
                //NewTestForm newTestForm = new NewTestForm();
                //newTestForm.Show();
                Section3_AIN section3ainForm = new Section3_AIN();
                section3ainForm.Show();
            };

            MNGR_COMMBIV.Instance.aLabJackDataReceived += Instance_aLabJackDataReceived;
            MNGR_COMMBIV.Instance.aPortHasOpened_orCloesdEVENT += HeardPortHasOpenedOrClsed;
            MNGR_COMMBIV.Instance.FirstMEssageWasReceived += Instance_FirstMEssageWasReceived;
            this.FormClosing += new FormClosingEventHandler(FormClosing_Handler);
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
                    label3_JackFirm.Text = "LabJack Firmware ver   : " + argFirmware;
                    label0_conquestion.Text = "ALL Comunications Established";
                    grouptests.Visible = true;
                }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating UI: " + ex.Message);
            }

        }

        private void Instance_FirstMEssageWasReceived(string argVersion)
        {
            Invoke(new Action(() => {
                // Update UI controls safely
                label1_MBIVversion.Text = "MBIV Software version : "+argVersion;
                btn_YesAuto.Visible = false;
                btn_NoManual.Visible = false;
                lstCOMPorts.Visible = false;
                label0_conquestion.Text = "COM  Established";
            }));
        }

        void HeardPortHasOpenedOrClsed(string argPortName, bool argOpenedTrue_closedfalse)
        {
            if (argOpenedTrue_closedfalse)
            {
                // new testform_01;

            }
            else
            {
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

        private void Btn_YesAuto_Click(object sender, EventArgs e)
        {
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

        private void FormClosing_Handler(object sender, FormClosingEventArgs e)
        {
            btn_YesAuto.Click -= Btn_YesAuto_Click;
            btn_NoManual.Click -= Btn_NoAuto_Click;
            lstCOMPorts.DoubleClick -= lstCOMPorts_DoubleClick;  
            MNGR_COMMBIV.Instance.aPortHasOpened_orCloesdEVENT -= HeardPortHasOpenedOrClsed;
            MNGR_COMMBIV.Instance.FirstMEssageWasReceived -= Instance_FirstMEssageWasReceived;
            MNGR_COMMBIV.Instance.aLabJackDataReceived -= Instance_aLabJackDataReceived;
        }
    }
}
