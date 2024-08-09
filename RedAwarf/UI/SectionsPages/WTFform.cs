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
    public partial class WTFform : Form
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
        double _RAW_DACTOSEND = 0.0;
        int collectedSamples = 0;
        int targetSamples = 100;

        //private bool canReadCleanData = false;
        private bool measuringChannel16 = false;
        private List<int>[] dataFloatingReadingsArray;
        AINtestSTATE aINtestSTATE = AINtestSTATE.SHOULDNOTREAD;
        public WTFform()
        {
            InitializeComponent();
            dataFloatingReadingsArray = new List<int>[17]; // Initialize the array for 17 lists
            for (int i = 0; i < dataFloatingReadingsArray.Length; i++)
            {
                dataFloatingReadingsArray[i] = new List<int>(); // Initialize each list in the array
            }

            TimerForLoop.Interval = 100;
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
            _ints_ADOS = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            btn_startTest.Click += btn_Start_Click;
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(16, 0);
            btn_testFloat.Click += btn_testFloat_Click;
        }

        private async void btn_testFloat_Click(object sender, EventArgs e)
        {
            aINtestSTATE = AINtestSTATE.SHOULDNOTREAD;
            _ACTIVEAIN = 16;
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, 0);
            await Task.Delay(300);
            aINtestSTATE = AINtestSTATE.MEASURING_BULK;
            targetSamples = 40;
            await RunMeasurementProcess();
        }

        private async Task RunMeasurementProcess()
        {
            // First, handle bulk measurements
            await CollectDataForBulk();
            // Then handle single measurement
            await CollectDataForSingle();
            // Finally, handle full array measurement
            await StartFullArrayMeasurement();
        }
        private async Task CollectDataForBulk()
        {
            while (dataFloatingReadingsArray[15].Count < targetSamples)
            {
                await Task.Delay(100); // Check every 100 ms
            }
            for (int i = 1; i <= 15; i++)
            {
                ProcessData(i);
                dataFloatingReadingsArray[i].Clear();
            }
            _ACTIVEAIN = 1;
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, 0);
            await Task.Delay(300);
            aINtestSTATE = AINtestSTATE.MEASURING_Single;
        }
        private async Task CollectDataForSingle()
        {
            while (dataFloatingReadingsArray[1].Count < targetSamples)
            {
                await Task.Delay(100); // Check every 100 ms
            }
            ProcessData(1);
            dataFloatingReadingsArray[1].Clear();
            aINtestSTATE = AINtestSTATE.MEASURING_FULLARRAY;
        }

        private async Task StartFullArrayMeasurement()
        {
            for (int powerLevel = 0; powerLevel <= 2; powerLevel++) // Assuming power levels are 0-2
            {
                for (int curAIN = 1; curAIN <= 16; curAIN++)
                {
                    _ACTIVEAIN = curAIN;
                    _ACTIVELEVEL = powerLevel;
                    MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, _ACTIVELEVEL);
                    await Task.Delay(300); // Wait for settings to take effect

                    await CollectDataForFullArray(curAIN, powerLevel, 200); // Collect 200 samples
                }
            }
            aINtestSTATE = AINtestSTATE.SHOULDNOTREAD; // Reset state after completing all measurements
        }
        private void timer_100_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < 17; i++)
            {
                _lbls_ADOs[i].Text = "o" + i + ": " + _ints_ADOS[i].ToString();
            }

            switch (aINtestSTATE)
            {
                case AINtestSTATE.SHOULDNOTREAD:
                    return;

                case AINtestSTATE.MEASURING_BULK:
                    CollectDataForBulk();
                    break;

                case AINtestSTATE.MEASURING_Single:
                    CollectDataForSingle();
                    break;
                case AINtestSTATE.MEASURING_FULLARRAY:
                    // Simplified for clarity; actual logic depends on how data is received
                    if (_ints_ADOS.Length > _ACTIVEAIN)
                        dataFloatingReadingsArray[_ACTIVEAIN].Add(_ints_ADOS[_ACTIVEAIN]);
                    break;
            }



        }
        private async Task CollectDataForFullArray(int curAIN, int powerLevel, int numSamples)
        {
            dataFloatingReadingsArray[curAIN].Clear(); // Clear existing data
            while (dataFloatingReadingsArray[curAIN].Count < numSamples)
            {
                await Task.Delay(TimerForLoop.Interval); // Assume data is collected each timer tick
                                                         // Simulation of data collection, actual implementation may vary
            }
            ProcessData(curAIN, powerLevel); // Process data after collection is done
        }


        private void ProcessData(int index, int powerLevel)
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
                //_labels2D_minmax[index - 1, powerLevel].Text = $"Min: {min}, Max: {max}, Avg: {average:N2}";
                Invoke(new Action(() =>
                {
                    _labels2D_minmax[index, powerLevel].Text = $"Min: {min}, Max: {max}, Avg: {average:N2}";
                }));
            }
            dataFloatingReadingsArray[index].Clear(); // Clear existing data after processing
        }


        //private async void CollectDataForBulk()
        //{
        //    for (int i = 1; i <= 15; i++)
        //    {
        //        dataFloatingReadingsArray[i].Add(_ints_ADOS[i]);
        //        if (dataFloatingReadingsArray[i].Count == targetSamples)
        //        {
        //            ProcessData(i);
        //            dataFloatingReadingsArray[i].Clear();
        //        }
        //    }

        //    if (dataFloatingReadingsArray[15].Count == 0) // Check if the last list has been cleared
        //    {
        //        _ACTIVEAIN = 1;
        //        MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, 0);
        //        await Task.Delay(300);
        //        aINtestSTATE = AINtestSTATE.MEASURING_Single;
        //    }
        //}

        //private void CollectDataForSingle()
        //{
        //    dataFloatingReadingsArray[16].Add(_ints_ADOS[16]);
        //    if (dataFloatingReadingsArray[16].Count == targetSamples)
        //    {
        //        ProcessData(16);
        //        dataFloatingReadingsArray[16].Clear();
        //        aINtestSTATE = AINtestSTATE.SHOULDNOTREAD;
        //    }
        //}

        //private async void StartFullArrayMeasurement()
        //{
        //    for (int powerLevel = 0; powerLevel <= 3; powerLevel++)
        //    {
        //        for (int curAIN = 1; curAIN <= 16; curAIN++)
        //        {
        //            _ACTIVEAIN = curAIN;
        //            _ACTIVELEVEL = powerLevel;
        //            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, _ACTIVELEVEL);
        //            await Task.Delay(300); // Wait for settings to take effect

        //            aINtestSTATE = AINtestSTATE.MEASURING_FULLARRAY;
        //            await CollectDataForFullArray(curAIN, powerLevel, 200); // Collect 200 samples

        //            // Optionally, you can process data here or after all collections are done
        //        }
        //    }

        //    aINtestSTATE = AINtestSTATE.SHOULDNOTREAD; // Reset state after completing all measurements
        //}



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
            else
            {

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

        private void ProcessData(int index)
        {
            var readings = dataFloatingReadingsArray[index];
            readings.Sort();
            readings.RemoveAt(0); // Remove first
            readings.RemoveAt(readings.Count - 1); // Remove last

            int min = readings.Min();
            int max = readings.Max();
            double average = readings.Average();

            floatingColumn[index].Text = $"Min: {min}, Max: {max}, Avg: {average:N2}";
        }



        private async void btn_Start_Click(object sender, EventArgs e)
        {

        }


    }
}


/*


        async void  MeasureChannel16()
        {
            if (!canReadCleanData)
            {
                await Task.Delay(200); // Wait for additional 200ms to stabilize the channel
                canReadCleanData = true; // Allow reading data
            }
            else
            {
                // Assuming you have a method to get data for channel 16
                _ints_ADOS[16] = _ints_ADOS[16]; // Replace GetChannel16Data with actual method
                _lbls_ADOs[16].Text = "o16: " + _ints_ADOS[16].ToString();
                // After processing, you may reset the conditions to measure again or stop
                measuringChannel16 = false; // Reset flag
                TimerForLoop.Stop(); // Optionally stop the timer if done
            }
        }
 */