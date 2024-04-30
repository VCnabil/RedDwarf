using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._DataObjz.DataCOMM
{
    public class MBIV_RX
    {
        #region old Params
        string _version;
        int _ain1;
        int _ain2;
        int _ain3;
        int _ain4;
        int _ain5;
        int _ain6;
        int _ain7;
        int _ain8;
        int _ain9;
        int _ain10;
        int _ain11;
        int _ain12;
        int _ain13;
        int _ain14;
        int _ain15;
        int _ain16;

        bool _gp0_sclutch;
        bool _gp1_portAP;
        bool _gp2_stbdAP;
        bool _gp3_Dktr1;
        bool _gp4_Dktr2;
        bool _gp5_Xfer1;
        bool _gp6_Xfer2;
        bool _gp7_pclutch;
        bool _apdi8;

        bool _mbiv_Serial_Fault;
        string _apInput;

        int _pnoz_FDBK;
        int _snoz_FDBK;
        int _pint_FDBK;
        int _sbuck_FDBK;
        int _pbuck_FDBK;
        int _sint_FDBK;

        bool _pb_fault;
        bool _si_fault;
        bool _pi_fault;
        bool _sb_fault;
        bool _pn_fault;
        bool _sn_fault;


        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        public int AIN1
        {
            get { return _ain1; }
            set
            {

                if (value < 0)
                {
                    _ain1 = 0;
                }
                else if (value > 4095)
                {
                    _ain1 = 4095;
                }
                else
                {
                    _ain1 = value;
                }

            }
        }
        public int AIN2
        {
            get { return _ain2; }
            set
            {

                if (value < 0)
                {
                    _ain2 = 0;
                }
                else if (value > 4095)
                {
                    _ain2 = 4095;
                }
                else
                {
                    _ain2 = value;
                }

            }
        }
        public int AIN3
        {
            get { return _ain3; }
            set
            {

                if (value < 0)
                {
                    _ain3 = 0;
                }
                else if (value > 4095)
                {
                    _ain3 = 4095;
                }
                else
                {
                    _ain3 = value;
                }

            }
        }

        public int AIN4
        {
            get { return _ain4; }
            set
            {

                if (value < 0)
                {
                    _ain4 = 0;
                }
                else if (value > 4095)
                {
                    _ain4 = 4095;
                }
                else
                {
                    _ain4 = value;
                }

            }
        }
        public int AIN5
        {
            get { return _ain5; }
            set
            {

                if (value < 0)
                {
                    _ain5 = 0;
                }
                else if (value > 4095)
                {
                    _ain5 = 4095;
                }
                else
                {
                    _ain5 = value;
                }

            }
        }
        public int AIN6
        {
            get { return _ain6; }
            set
            {

                if (value < 0)
                {
                    _ain6 = 0;
                }
                else if (value > 4095)
                {
                    _ain6 = 4095;
                }
                else
                {
                    _ain6 = value;
                }

            }
        }
        public int AIN7
        {
            get { return _ain7; }
            set
            {

                if (value < 0)
                {
                    _ain7 = 0;
                }
                else if (value > 4095)
                {
                    _ain7 = 4095;
                }
                else
                {
                    _ain7 = value;
                }

            }
        }
        public int AIN8
        {
            get { return _ain8; }
            set
            {

                if (value < 0)
                {
                    _ain8 = 0;
                }
                else if (value > 4095)
                {
                    _ain8 = 4095;
                }
                else
                {
                    _ain8 = value;
                }

            }
        }
        public int AIN9
        {
            get { return _ain9; }
            set
            {

                if (value < 0)
                {
                    _ain9 = 0;
                }
                else if (value > 4095)
                {
                    _ain9 = 4095;
                }
                else
                {
                    _ain9 = value;
                }

            }
        }
        public int AIN10
        {
            get { return _ain10; }
            set
            {

                if (value < 0)
                {
                    _ain10 = 0;
                }
                else if (value > 4095)
                {
                    _ain10 = 4095;
                }
                else
                {
                    _ain10 = value;
                }

            }
        }
        public int AIN11
        {
            get { return _ain11; }
            set
            {

                if (value < 0)
                {
                    _ain11 = 0;
                }
                else if (value > 4095)
                {
                    _ain11 = 4095;
                }
                else
                {
                    _ain11 = value;
                }

            }
        }
        public int AIN12
        {
            get { return _ain12; }
            set
            {

                if (value < 0)
                {
                    _ain12 = 0;
                }
                else if (value > 4095)
                {
                    _ain12 = 4095;
                }
                else
                {
                    _ain12 = value;
                }

            }
        }
        public int AIN13
        {
            get { return _ain13; }
            set
            {

                if (value < 0)
                {
                    _ain13 = 0;
                }
                else if (value > 4095)
                {
                    _ain13 = 4095;
                }
                else
                {
                    _ain13 = value;
                }

            }
        }
        public int AIN14
        {
            get { return _ain14; }
            set
            {

                if (value < 0)
                {
                    _ain14 = 0;
                }
                else if (value > 4095)
                {
                    _ain14 = 4095;
                }
                else
                {
                    _ain14 = value;
                }

            }
        }
        public int AIN15
        {
            get { return _ain15; }
            set
            {

                if (value < 0)
                {
                    _ain15 = 0;
                }
                else if (value > 4095)
                {
                    _ain15 = 4095;
                }
                else
                {
                    _ain15 = value;
                }

            }
        }
        public int AIN16
        {
            get { return _ain16; }
            set
            {

                if (value < 0)
                {
                    _ain16 = 0;
                }
                else if (value > 4095)
                {
                    _ain16 = 4095;
                }
                else
                {
                    _ain16 = value;
                }

            }
        }

        public bool GP0_sClutch
        {
            get { return _gp0_sclutch; }
            private set { _gp0_sclutch = value; }
        }
        public bool GP1_AP1
        {
            get { return _gp1_portAP; }
            private set { _gp1_portAP = value; }
        }

        public bool GP2_AP2
        {
            get { return _gp2_stbdAP; }
            private set { _gp2_stbdAP = value; }
        }
        public bool GP3_Dktr1
        {
            get { return _gp3_Dktr1; }
            private set { _gp3_Dktr1 = value; }
        }
        public bool GP4_DKtr2
        {
            get { return _gp4_Dktr2; }
            private set { _gp4_Dktr2 = value; }
        }
        public bool GP5_Xfer1
        {
            get { return _gp5_Xfer1; }
            private set { _gp5_Xfer1 = value; }
        }
        public bool GP6_Xfer2
        {
            get { return _gp6_Xfer2; }
            private set { _gp6_Xfer2 = value; }
        }
        public bool GP7_pClutch
        {
            get { return _gp7_pclutch; }
            private set { _gp7_pclutch = value; }
        }
        public bool APDI_bit8
        {
            get { return _apdi8; }
            private set { _apdi8 = value; }
        }
        public bool MBIV_Serial_Fault_19
        {
            get { return _mbiv_Serial_Fault; }
            set { _mbiv_Serial_Fault = value; }
        }
        public string APInput_20
        {
            get { return _apInput; }
            set { _apInput = value; }
        }

        public int PNOZ_FDBK_21
        {
            get { return _pnoz_FDBK; }
            set
            {
                if (value < 0)
                {
                    _pnoz_FDBK = 0;
                }
                else if (value > 255)
                {
                    _pnoz_FDBK = 255;
                }
                else
                {
                    _pnoz_FDBK = value;
                }
            }
        }

        public int SNOZ_FDBK_22
        {
            get { return _snoz_FDBK; }
            set
            {
                if (value < 0)
                {
                    _snoz_FDBK = 0;
                }
                else if (value > 255)
                {
                    _snoz_FDBK = 255;
                }
                else
                {
                    _snoz_FDBK = value;
                }
            }
        }

        public int PINT_FDBK_23
        {
            get { return _pint_FDBK; }
            set
            {
                if (value < 0)
                {
                    _pint_FDBK = 0;
                }
                else if (value > 255)
                {
                    _pint_FDBK = 255;
                }
                else
                {
                    _pint_FDBK = value;
                }
            }
        }

        public int SBKT_FDBK_24
        {
            get { return _sbuck_FDBK; }
            set
            {
                if (value < 0)
                {
                    _sbuck_FDBK = 0;
                }
                else if (value > 255)
                {
                    _sbuck_FDBK = 255;
                }
                else
                {
                    _sbuck_FDBK = value;
                }
            }
        }
        public int PBKT_FDBK_25
        {
            get { return _pbuck_FDBK; }
            set
            {
                if (value < 0)
                {
                    _pbuck_FDBK = 0;
                }
                else if (value > 255)
                {
                    _pbuck_FDBK = 255;
                }
                else
                {
                    _pbuck_FDBK = value;
                }
            }
        }
        public int SINT_FDBK_26
        {
            get { return _sint_FDBK; }
            set
            {
                if (value < 0)
                {
                    _sint_FDBK = 0;
                }
                else if (value > 255)
                {
                    _sint_FDBK = 255;
                }
                else
                {
                    _sint_FDBK = value;
                }
            }
        }

        public bool PB_Fault
        {
            get { return _pb_fault; }
            private set { _pb_fault = value; }
        }
        public bool SI_Fault
        {
            get { return _si_fault; }
            private set { _si_fault = value; }
        }
        public bool PI_Fault
        {
            get { return _pi_fault; }
            private set { _pi_fault = value; }
        }
        public bool SB_Fault
        {
            get { return _sb_fault; }
            private set { _sb_fault = value; }
        }
        public bool PN_Fault
        {
            get { return _pn_fault; }
            private set { _pn_fault = value; }
        }
        public bool SN_Fault
        {
            get { return _sn_fault; }
            private set { _sn_fault = value; }
        }

        #endregion
        DateTime _dateTimeReceived;
        TimeSpan argintervalSinceLastRX;

        int[] __all_ains;
        int[] __allDIs_ordered_xfer_dk_clutch_ap_adpi;
        int[] __all_FBKs_ordered_Noz_Buk_Ints;
        int[] __all_Faults_ordered_Noz_Buk_Ints;


        public void Update_IntArray_withTimeDate(string argBody, DateTime argDateTimeReceived, TimeSpan arg_timeSinceLast) { 
        
            _dateTimeReceived = argDateTimeReceived;
            argintervalSinceLastRX = arg_timeSinceLast;
            Update_INTarra_FromCommaDelimitedString(argBody);
        }
        private void Update_INTarra_FromCommaDelimitedString(string argBody)
        {
            //the argBody  "$VCIA,1.11_Rev5712,4049,4062,4062,4063,4038,4037,4058,4054,4053,4056,4050,4041,4043,4056,4055,4063 ,511,1,6,26,23,46,32,25,23,63"
            //split the string into an array of strings using the comma as the delimiter
            string[] __split = argBody.Split(',');
            //assign the values to the properties
            _version = __split[1];
            _ain1 = Convert.ToInt32(__split[2]);
            _ain2 = Convert.ToInt32(__split[3]);
            _ain3 = Convert.ToInt32(__split[4]);
            _ain4 = Convert.ToInt32(__split[5]);
            _ain5 = Convert.ToInt32(__split[6]);
            _ain6 = Convert.ToInt32(__split[7]);
            _ain7 = Convert.ToInt32(__split[8]);
            _ain8 = Convert.ToInt32(__split[9]);
            _ain9 = Convert.ToInt32(__split[10]);
            _ain10 = Convert.ToInt32(__split[11]);
            _ain11 = Convert.ToInt32(__split[12]);
            _ain12 = Convert.ToInt32(__split[13]);
            _ain13 = Convert.ToInt32(__split[14]);
            _ain14 = Convert.ToInt32(__split[15]);
            _ain15 = Convert.ToInt32(__split[16]);
            _ain16 = Convert.ToInt32(__split[17]);

            // SetGP_bools_18(Convert.ToInt32(__split[18]));
            int arg_18 = Convert.ToInt32(__split[18]);
            _gp0_sclutch = (arg_18 & 0x01) == 0x01;
            _gp1_portAP = (arg_18 & 0x02) == 0x02;
            _gp2_stbdAP = (arg_18 & 0x04) == 0x04;
            _gp3_Dktr1 = (arg_18 & 0x08) == 0x08;
            _gp4_Dktr2 = (arg_18 & 0x10) == 0x10;
            _gp5_Xfer1 = (arg_18 & 0x20) == 0x20;
            _gp6_Xfer2 = (arg_18 & 0x40) == 0x40;
            _gp7_pclutch = (arg_18 & 0x80) == 0x80;
            _apdi8 = (arg_18 & 0x100) == 0x100;



            _mbiv_Serial_Fault = Convert.ToInt32(__split[19]) == 1;
            _apInput = __split[20];

            _pnoz_FDBK = Convert.ToInt32(__split[21]);
            _snoz_FDBK = Convert.ToInt32(__split[22]);
            _pint_FDBK = Convert.ToInt32(__split[23]);
            _sbuck_FDBK = Convert.ToInt32(__split[24]);
            _pbuck_FDBK = Convert.ToInt32(__split[25]);
            _sint_FDBK = Convert.ToInt32(__split[26]);

            //Set_boolFaults_27(Convert.ToInt32(__split[27]));
            int arg27 = Convert.ToInt32(__split[27]);
            _pb_fault = (arg27 & 0x01) == 0x01;
            _si_fault = (arg27 & 0x02) == 0x02;
            _pi_fault = (arg27 & 0x04) == 0x04;
            _sb_fault = (arg27 & 0x08) == 0x08;
            _pn_fault = (arg27 & 0x10) == 0x10;
            _sn_fault = (arg27 & 0x20) == 0x20;

            __all_ains[0] = -1;
            __all_ains[1] = AIN1;
            __all_ains[2] = AIN2;
            __all_ains[3] = AIN3;
            __all_ains[4] = AIN4;
            __all_ains[5] = AIN5;
            __all_ains[6] = AIN6;
            __all_ains[7] = AIN7;
            __all_ains[8] = AIN8;
            __all_ains[9] = AIN9;
            __all_ains[10] = AIN10;
            __all_ains[11] = AIN11;
            __all_ains[12] = AIN12;
            __all_ains[13] = AIN13;         
            __all_ains[14] = AIN14;
            __all_ains[15] = AIN15;
            __all_ains[16] = AIN16;



            /*
             //alt
             
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[0] = _gp5_Xfer1? 0:1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[1] = _gp6_Xfer2? 0:1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[2] = _gp3_Dktr1? 0 : 1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[3] = _gp4_Dktr2? 0 : 1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[4] = _gp0_sclutch? 0 : 1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[5] = _gp7_pclutch? 0 : 1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[6] = _gp1_portAP? 0 : 1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[7] = _gp2_stbdAP? 0 : 1;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[8] = _apdi8? 0 : 1;
             */
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[0] = _gp5_Xfer1? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[1] = _gp6_Xfer2? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[2] = _gp3_Dktr1? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[3] = _gp4_Dktr2? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[4] = _gp0_sclutch? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[5] = _gp7_pclutch? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[6] = _gp1_portAP? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[7] = _gp2_stbdAP? 1:0;
            __allDIs_ordered_xfer_dk_clutch_ap_adpi[8] = _apdi8? 1:0;

            __all_FBKs_ordered_Noz_Buk_Ints[0] = _pnoz_FDBK;
            __all_FBKs_ordered_Noz_Buk_Ints[1] = _snoz_FDBK;
            __all_FBKs_ordered_Noz_Buk_Ints[2] = _pbuck_FDBK;
            __all_FBKs_ordered_Noz_Buk_Ints[3] = _sbuck_FDBK;
            __all_FBKs_ordered_Noz_Buk_Ints[4] = _pint_FDBK; 
            __all_FBKs_ordered_Noz_Buk_Ints[5] = _sint_FDBK;

            __all_Faults_ordered_Noz_Buk_Ints[0] = _pn_fault? 1:0;
            __all_Faults_ordered_Noz_Buk_Ints[1] = _sn_fault? 1:0;
            __all_Faults_ordered_Noz_Buk_Ints[2] = _pb_fault? 1:0;
            __all_Faults_ordered_Noz_Buk_Ints[3] = _sb_fault? 1:0;
            __all_Faults_ordered_Noz_Buk_Ints[4] = _pi_fault? 1:0;
            __all_Faults_ordered_Noz_Buk_Ints[5] = _si_fault? 1:0;


        }
  
        public int Get_Stored_AINVal(int cur_auto_channelIndex)
        {
            switch (cur_auto_channelIndex)
            {
                case 0:
                    return -1;
                case 1:
                    return AIN1;
                case 2:
                    return AIN2;
                case 3:
                    return AIN3;
                case 4:
                    return AIN4;
                case 5:
                    return AIN5;
                case 6:
                    return AIN6;
                case 7:
                    return AIN7;
                case 8:
                    return AIN8;
                case 9:
                    return AIN9;
                case 10:
                    return AIN10;
                case 11:
                    return AIN11;
                case 12:
                    return AIN12;
                case 13:
                    return AIN13;
                case 14:
                    return AIN14;
                case 15:
                    return AIN15;
                case 16:
                    return AIN16;
                default:
                    return AIN1;
            }
        }

        public int[] GET_allAINS()
        {

            return __all_ains;
        }
        public int[] GET_allDIs()
        {
            return __allDIs_ordered_xfer_dk_clutch_ap_adpi;
        }


        public MBIV_RX()
        {
            _version = "";
            _ain1 = 0;
            _ain2 = 0;
            _ain3 = 0;
            _ain4 = 0;
            _ain5 = 0;
            _ain6 = 0;
            _ain7 = 0;
            _ain8 = 0;
            _ain9 = 0;
            _ain10 = 0;
            _ain11 = 0;
            _ain12 = 0;
            _ain13 = 0;
            _ain14 = 0;
            _ain15 = 0;
            _ain16 = 0;

            _gp0_sclutch = false;
            _gp1_portAP = false;
            _gp2_stbdAP = false;
            _gp3_Dktr1 = false;
            _gp4_Dktr2 = false;
            _gp5_Xfer1 = false;
            _gp6_Xfer2 = false;
            _gp7_pclutch = false;
            _apdi8 = false;

            _mbiv_Serial_Fault = false;
            _apInput = "6";

            _pnoz_FDBK = 0;
            _snoz_FDBK = 0;
            _pint_FDBK = 0;
            _sbuck_FDBK = 0;
            _pbuck_FDBK = 0;
            _sint_FDBK = 0;

            _pb_fault = false;
            _si_fault = false;
            _pi_fault = false;
            _sb_fault = false;
            _pn_fault = false;
            _sn_fault = false;
            __all_ains = new int[17];
            __allDIs_ordered_xfer_dk_clutch_ap_adpi = new int[9];

            __all_FBKs_ordered_Noz_Buk_Ints= new int[6];

            __all_Faults_ordered_Noz_Buk_Ints = new int[6];
        }

        //override ToString method
        public override string ToString()
        {
            return string.Format("MBIV_RX: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}", _version, _ain1, _ain2, _ain3, _ain4, _ain5, _ain6, _ain7, _ain8, _ain9, _ain10, _ain11, _ain12, _ain13, _ain14, _ain15, _ain16, _gp0_sclutch, _gp1_portAP, _gp2_stbdAP, _gp3_Dktr1, _gp4_Dktr2, _gp5_Xfer1, _gp6_Xfer2, _gp7_pclutch, _apdi8, _mbiv_Serial_Fault, _apInput, _pnoz_FDBK, _snoz_FDBK, _pint_FDBK, _sbuck_FDBK, _pbuck_FDBK, _sint_FDBK, _pb_fault, _si_fault, _pi_fault, _sb_fault, _pn_fault, _sn_fault);
        }


    }
}
