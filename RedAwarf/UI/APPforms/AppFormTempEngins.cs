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

namespace RedDwarf.RedAwarf.UI.APPforms
{
    public partial class AppFormTempEngins : Form
    {

        DATA_TX DATA_TX;
        private System.Windows.Forms.Timer KeepMBIValiveTimer = new System.Windows.Forms.Timer();
        public AppFormTempEngins()
        {
            DATA_TX = new DATA_TX();
            InitializeComponent();
            groupBox_EnginTX.DoubleClick += new EventHandler(MyGroupBox_DoubleClick);
        

            tkb_PB.ValueChanged += new EventHandler(tkb_PB_ValueChanged);
            tkb_PN.ValueChanged += new EventHandler(tkb_PN_ValueChanged);
            tkb_PI.ValueChanged += new EventHandler(tkb_PI_ValueChanged);
            tkb_SB.ValueChanged += new EventHandler(tkb_SB_ValueChanged);
            tkb_SN.ValueChanged += new EventHandler(tkb_SN_ValueChanged);
            tkb_SI.ValueChanged += new EventHandler(tkb_SI_ValueChanged);
            tkb_E1.ValueChanged += new EventHandler(tkb_E1_ValueChanged);
            tkb_E2.ValueChanged += new EventHandler(tkb_E2_ValueChanged);

            KeepMBIValiveTimer.Interval = 100;
            KeepMBIValiveTimer.Tick += new EventHandler(KeepMBIValiveTimer_Tick);
            KeepMBIValiveTimer.Start();

        }

        private void KeepMBIValiveTimer_Tick(object sender, EventArgs e)
        {
            
            MNGR_COMMBIV.Instance.WriteData__MBIV(DATA_TX);
            lbl_TX.Text = "tx:" + DATA_TX.CREATE_FullString_for_TX();

            double AIN2_PI = MNGR_COMMBIV.Instance.Get_Value_AIN2();
            double AIN3_SI = MNGR_COMMBIV.Instance.Get_Value_AIN3();
            double AIN4_PE = MNGR_COMMBIV.Instance.Get_Value_AIN4();
            double AIN5_SE = MNGR_COMMBIV.Instance.Get_Value_AIN5();
            double AiN6_PB = MNGR_COMMBIV.Instance.Get_Value_AIN6();
            double AiN7_PN = MNGR_COMMBIV.Instance.Get_Value_AIN7();

            label1.Text = "AIN2_PI: " + AIN2_PI.ToString();
            label2.Text = "AIN3_SI: " + AIN3_SI.ToString();
            label3.Text = "AIN4_PE: " + AIN4_PE.ToString();
            label4.Text = "AIN5_SE: " + AIN5_SE.ToString();
            label5.Text = "AIN6_PB: " + AiN6_PB.ToString();
            label6.Text = "AIN7_PN: " + AiN7_PN.ToString();

        }

        // Event handler for the DoubleClick event
        private void MyGroupBox_DoubleClick(object sender, EventArgs e)
        {
            tkb_PB.Value = 100;
            tkb_PN.Value = 100;
            tkb_PI.Value = 100;
            tkb_SB.Value = 100;
            tkb_SN.Value = 100;
            tkb_SI.Value = 100;
            tkb_E1.Value = 0;
            tkb_E2.Value = 0;
            DATA_TX.PB_1 = tkb_PB.Value;
            DATA_TX.PN_2 = tkb_PN.Value;
            DATA_TX.PI_3 = tkb_PI.Value;
            DATA_TX.SB_4 = tkb_SB.Value;
            DATA_TX.SN_5 = tkb_SN.Value;
            DATA_TX.SI_6 = tkb_SI.Value;
            DATA_TX.PE_7 = tkb_E1.Value;
            DATA_TX.SE_8 = tkb_E2.Value;
            lbl_PB.Text = "PB: " + tkb_PB.Value.ToString() + "";
            lbl_PN.Text = "PN: " + tkb_PN.Value.ToString() + "";
            lbl_PI.Text = "PI: " + tkb_PI.Value.ToString() + "";
            lbl_SB.Text = "SB: " + tkb_SB.Value.ToString() + "";
            lbl_SN.Text = "SN: " + tkb_SN.Value.ToString() + "";
            lbl_SI.Text = "SI: " + tkb_SI.Value.ToString() + "";
            lbl_E1.Text = "PE: " + tkb_E1.Value.ToString() + "";
            lbl_E2.Text = "SE: " + tkb_E2.Value.ToString() + "";

        }
        private void tkb_PB_ValueChanged(object sender, EventArgs e)
        {
            lbl_PB.Text = "PB: " + tkb_PB.Value.ToString() + "";
            DATA_TX.PB_1 = tkb_PB.Value;
        }
        private void tkb_PN_ValueChanged(object sender, EventArgs e)
        {
            lbl_PN.Text = "PN: " + tkb_PN.Value.ToString() + "";
            DATA_TX.PN_2 = tkb_PN.Value;
        }
        private void tkb_PI_ValueChanged(object sender, EventArgs e)
        {
            lbl_PI.Text = "PI: " + tkb_PI.Value.ToString() + "";
            DATA_TX.PI_3 = tkb_PI.Value;
        }
        private void tkb_SB_ValueChanged(object sender, EventArgs e)
        {
            lbl_SB.Text = "SB: " + tkb_SB.Value.ToString() + "";
            DATA_TX.SB_4 = tkb_SB.Value;
        }
        private void tkb_SN_ValueChanged(object sender, EventArgs e)
        {
            lbl_SN.Text = "SN: " + tkb_SN.Value.ToString() + "";
            DATA_TX.SN_5 = tkb_SN.Value;
        }
        private void tkb_SI_ValueChanged(object sender, EventArgs e)
        {
            lbl_SI.Text = "SI: " + tkb_SI.Value.ToString() + "";
            DATA_TX.SI_6 = tkb_SI.Value;
        }
        private void tkb_E1_ValueChanged(object sender, EventArgs e)
        {
            lbl_E1.Text = "PE: " + tkb_E1.Value.ToString() + "";
            DATA_TX.PE_7 = tkb_E1.Value;
        }
        private void tkb_E2_ValueChanged(object sender, EventArgs e)
        {
            lbl_E2.Text = "SE: " + tkb_E2.Value.ToString() + "";
            DATA_TX.SE_8 = tkb_E2.Value;
        }

    }
}
