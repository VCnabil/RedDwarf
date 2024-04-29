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


        int _MAX_AINS = 3;
        int _MAX_LVLS = 1;
        int PAuseDelays = 100;
        int WaitToTakeEffect = 100;
        int longWaiteffect = 500;
        int targetSamples = 6;
        private System.Windows.Forms.Timer TimerForLoop = new System.Windows.Forms.Timer();
        private List<int> measurementValues = new List<int>();  // List to store measurement values for each turn

   
        Label[] _lbls_ADOs;
        int[] _ints_ADOS;
        Label[,] _labels2D_minmax;
        Label[] floatingColumn;
        private bool allowedToFilterMinMax = true;
        int minValue, maxValue;
        int _FloaterIndexTOAVOID = 16;
        int _ACTIVEAIN = 1;
        private int _ACTIVELEVEL = 0;
        int indexFloating = 8;
        int _minFloating, _maxFloating;

        MBIV_RX _myCopyofDataMBIV;
        double _RAW_DACTOSEND=0.0;
        int collectedSamples = 0;
 

        //private bool canReadCleanData = false;
        private bool measuringChannel16 = false;
        private List<int>[] dataFloatingReadingsArray;
       // AINtestSTATE aINtestSTATE = AINtestSTATE.SHOULDNOTREAD;
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

        private async  void btn_testFloat_Click(object sender, EventArgs e)
        {
       //     aINtestSTATE = AINtestSTATE.SHOULDNOTREAD;
            _ACTIVEAIN = _MAX_AINS;
            _ACTIVELEVEL = _MAX_LVLS;
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, _ACTIVELEVEL);
            btn_testFloat.BackColor = Color.Green;
            await Task.Delay(PAuseDelays);
          //  aINtestSTATE = AINtestSTATE.MEASURING_BULK;
            
            await RunMeasurementProcess();
            btn_testFloat.BackColor = Color.Red;
        }

        private async Task RunMeasurementProcess()
        {
            // Finally, handle full array measurement
          //  await Start_FULLARRAY_Measurement();
            btn_testFloat.BackColor = Color.CadetBlue;
         await StartMeasuring__AllFloatersLessLAst();
            btn_testFloat.BackColor = Color.Yellow;
          await StartMeasuring__lastFloater();
        }
        void ClearAll_Floaters()
        {
            for (int i = 1; i < _FloaterIndexTOAVOID; i++)
            {
                dataFloatingReadingsArray[i].Clear(); // Clear existing data
            }
        }


        #region Full_ARRAY_MEASUREMENT
        private async Task Start_FULLARRAY_Measurement()
        {
            ClearAll_Floaters();
            for (int powerLevel = 0; powerLevel <= _MAX_LVLS; powerLevel++)
            {
                await Task.Delay(WaitToTakeEffect);
                for (int curAIN = 1; curAIN <= _MAX_AINS; curAIN++)
                {
                    _ACTIVEAIN = curAIN;
                    _ACTIVELEVEL = powerLevel;
                    MNGR_COMMBIV.Instance.WRITEDATA_CHAN_LVL(_ACTIVEAIN, _ACTIVELEVEL, true);
                    await Task.Delay(WaitToTakeEffect); // Wait for settings to take effect

                    await CollectData_FULLARRAY(curAIN, powerLevel, targetSamples); 
                }
            }
        }
        private async Task CollectData_FULLARRAY(int curAIN, int powerLevel, int numSamples)
        {
            dataFloatingReadingsArray[curAIN].Clear(); // Clear existing data
            while (dataFloatingReadingsArray[curAIN].Count < numSamples)
            {
                await Task.Delay(TimerForLoop.Interval); // Assume data is collected each timer tick
                                                         // Simulation of data collection, actual implementation may vary
                int reading = _ints_ADOS[curAIN];
                dataFloatingReadingsArray[curAIN].Add(reading);

            }
            ProcessData_FULLARRAY(curAIN, powerLevel); // Process data after collection is done
        }
        private void ProcessData_FULLARRAY(int index, int powerLevel)
        {
            var readings = dataFloatingReadingsArray[index];
            if (readings.Count > 2)
            {
                readings.Sort();
                readings.RemoveAt(0); // Remove first
                readings.RemoveAt(readings.Count - 1); // Remove last

                int min = readings.Min();
                int max = readings.Max();
                double average = readings.Average();

                // Update the corresponding label in the 2D array
                _labels2D_minmax[index, powerLevel].Text = $"i:{min} a: {max} v: {average:N2}";
            }
            dataFloatingReadingsArray[index].Clear(); // Clear existing data after processing
        }
        #endregion

        #region FLoaters


        private async Task StartMeasuring__AllFloatersLessLAst()
        {
            ClearAll_Floaters();

            _FloaterIndexTOAVOID = _MAX_AINS;
            MNGR_COMMBIV.Instance.WRITEDATA_CHAN_LVL(_FloaterIndexTOAVOID, 0, false);
            await Task.Delay(longWaiteffect);

            await CollectDataFor_Floaters(targetSamples);

        }
        private async Task CollectDataFor_Floaters(int numSamples)
        {

            ClearAll_Floaters();


            while (dataFloatingReadingsArray[_FloaterIndexTOAVOID - 1].Count < numSamples)
            {
                await Task.Delay(TimerForLoop.Interval);
                for (int i = 1; i < _FloaterIndexTOAVOID; i++)
                {

                    int reading = _ints_ADOS[i];
                    dataFloatingReadingsArray[i].Add(reading);

                }
            }

            ProcessDataFloaters_UPTo(_FloaterIndexTOAVOID); // Process data after collection is done
        }
        private void ProcessDataFloaters_UPTo(int index)
        {
            for (int i = 1; i < index; i++)
            {
                var readings = dataFloatingReadingsArray[i];
                if (readings.Count > 2)
                {
                    readings.Sort();
                    readings.RemoveAt(0); // Remove first
                    readings.RemoveAt(readings.Count - 1); // Remove last

                    int min = readings.Min();
                    int max = readings.Max();
                    double average = readings.Average();

                    // Update the corresponding label in the 2D array
                    floatingColumn[i].Text = $"i:{min} a: {max} v: {average:N2}";
                }
                dataFloatingReadingsArray[i].Clear(); // Clear existing data after processing
            }
        }
        #endregion

        #region LAstFloater
        private async Task StartMeasuring__lastFloater()
        {
            ClearAll_Floaters();
          _FloaterIndexTOAVOID = _MAX_AINS;
            MNGR_COMMBIV.Instance.WRITEDATA_CHAN_LVL(1, 0, false);
            await Task.Delay(WaitToTakeEffect);
            await CollectDataFor_LastFloater(targetSamples);
        }
        private async Task CollectDataFor_LastFloater(int numSamples)
        {
            dataFloatingReadingsArray[_FloaterIndexTOAVOID].Clear(); // Clear existing data
            while (dataFloatingReadingsArray[_FloaterIndexTOAVOID].Count < numSamples)
            {
                await Task.Delay(TimerForLoop.Interval);
                int reading = _ints_ADOS[_FloaterIndexTOAVOID];
                dataFloatingReadingsArray[_FloaterIndexTOAVOID].Add(reading);
            }
            ProcessData_lastFloater(_FloaterIndexTOAVOID); // Process data after collection is done
        }



        private void ProcessData_lastFloater(int index)
        {
            var readings = dataFloatingReadingsArray[index];
            if (readings.Count > 2)
            {
                readings.Sort();
                readings.RemoveAt(0); // Remove first
                readings.RemoveAt(readings.Count - 1); // Remove last

                int min = readings.Min();
                int max = readings.Max();
                double average = readings.Average();

                // Update the corresponding label in the 2D array
                floatingColumn[index].Text = $"i:{min} a: {max} v: {average:N2}";
            }
            dataFloatingReadingsArray[index].Clear(); // Clear existing data after processing
        }
        #endregion




        private void timer_100_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < 17; i++)
            {
                _lbls_ADOs[i].Text = "o" + i + ": " + _ints_ADOS[i].ToString();
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
            lbl_cufloaterIndx.Text = "curFloater: " + _FloaterIndexTOAVOID;
        }
 

        private async void btn_Start_Click(object sender, EventArgs e)
        {
        
        }


    }
}

 