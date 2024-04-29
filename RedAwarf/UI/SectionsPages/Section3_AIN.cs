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
using static RedDwarf.RedAwarf._Globalz.Helpers;

namespace RedDwarf.RedAwarf.UI.SectionsPages
{
    public partial class Section3_AIN : Form
    {
        private System.Windows.Forms.Timer TimerForLoop = new System.Windows.Forms.Timer();
        private List<int> measurementValues = new List<int>();  // List to store measurement values for each turn

   
        Label[] _lbls_ADOs;
        int[] _ints_ADOS;
        Label[,] _labels2D_minmax;
        Label[] floatingColumn;
        private bool allowedToFilterMinMax = true;
        int minValue, maxValue;
        int _activeFloatingIndex = 16;
        int _ACTIVEAIN = 1;
        private int _ACTIVELEVEL = 0;
        int indexFloating = 8;
        int _minFloating, _maxFloating;

        MBIV_RX _myCopyofDataMBIV;
        double _RAW_DACTOSEND=0.0;
        int collectedSamples = 0;
        int targetSamples = 100;

        //private bool canReadCleanData = false;
        private bool measuringChannel16 = false;
        private List<int>[] dataFloatingReadingsArray;
        AINtestSTATE aINtestSTATE = AINtestSTATE.SHOULDNOTREAD;
        public Section3_AIN()
        {
            InitializeComponent();
            dataFloatingReadingsArray = new List<int>[17]; // Initialize the array for 17 lists
            for (int i = 0; i < dataFloatingReadingsArray.Length; i++)
            {
                dataFloatingReadingsArray[i] = new List<int>(); // Initialize each list in the array
            }

            TimerForLoop.Interval = 280;
            TimerForLoop.Tick += timer_100_Tick;
            TimerForLoop.Start();
            MNGR_COMMBIV.Instance.MessageReceived += Instance_MessageReceived;
           _lbls_ADOs = new Label[] { LBL_AD0, LBL_AD1, LBL_AD2, LBL_AD3, LBL_AD4, LBL_AD5, LBL_AD6, LBL_AD7, LBL_AD8, LBL_AD9, lbl_AD10, lbl_AD11, lbl_AD12, lbl_AD13, lbl_AD14, lbl_AD15, lbl_AD16 };
            _labels2D_minmax = new Label[,]
             {
                { label_0_0, label_0_1, label_0_2 },
                { label_1_0, label_1_1, label_1_2 },
                { label_2_0, label_2_1, label_2_2 },
                { label_3_0, label_3_1, label_3_2 },
                { label_4_0, label_4_1, label_4_2 },
                { label_5_0, label_5_1, label_5_2 },
                { label_6_0, label_6_1, label_6_2 },
                { label_7_0, label_7_1, label_7_2 },
                { label_8_0, label_8_1, label_8_2 },
                { label_9_0, label_9_1, label_9_2 },
                { label_10_0, label_10_1, label_10_2 },
                { label_11_0, label_11_1, label_11_2 },
                { label_12_0, label_12_1, label_12_2 },
                { label_13_0, label_13_1, label_13_2 },
                { label_14_0, label_14_1, label_14_2 },
                { label_15_0, label_15_1, label_15_2 },
                { label_16_0, label_16_1, label_16_2 }
             };
            floatingColumn = new Label[] { label_0_3, label_1_3, label_2_3, label_3_3, label_4_3, label_5_3, label_6_3, label_7_3, label_8_3, label_9_3, label_10_3, label_11_3, label_12_3, label_13_3, label_14_3, label_15_3, label_16_3 };
            _ints_ADOS = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
            btn_startTest.Click += btn_Start_Click;
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(16, 0);
            btn_testFloat.Click += btn_testFloat_Click;
        }

        private void btn_testFloat_Click(object sender, EventArgs e)
        {
            
        }

        private void timer_100_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
                    MessageBox.Show("Error updating UI: " + ex.Message);
                }
            }
            else {

                try // Try to invoke the action
                {
                    SafeInvokedFunctionOnRX(message);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    MessageBox.Show("Error updating UI: " + ex.Message);
                }
            }
        }
        void SafeInvokedFunctionOnRX(MBIV_RX message)
        {
            //display cur _ACTIVEAIN value and _ACTIVELEVEL in lbl_curAIN and lbl_curLVL
            lbl_curAIN.Text = "curAIN: " + _ACTIVEAIN;
            lbl_curLVL.Text = "curLVL: " + _ACTIVELEVEL;
            lbl_RX.Text = "RX: " + message.ToString();
        }
 

        private async void btn_Start_Click(object sender, EventArgs e)
        {
        
        }


    }
}

 