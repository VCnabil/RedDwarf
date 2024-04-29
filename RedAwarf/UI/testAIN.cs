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
    public partial class testAIN : Form
    {
        private List<int> measurementValues = new List<int>();  // List to store measurement values for each turn
        private System.Windows.Forms.Timer timer_100Second = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer timer_powerLevelAdjustment = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer TimerTICKER_readJackslow = new System.Windows.Forms.Timer();
        Label[] _lbls_ADOs;
        int[] _ints_ADOS;
        Label[,] _labels2D_minmax;
        Label[] floatingColumn;
        private bool allowedToFilterMinMax = true;
        int minValue, maxValue;
        int _ACTIVEAIN = 1;
        private int _ACTIVELEVEL = 0;
        int indexFloating = 8;
        int _minFloating, _maxFloating;
        bool muxbit0 = true;
        bool muxbit1 = true;
        bool muxbit2 = true;
        bool muxbit3 = true;
        double _RAW_DACTOSEND = 0.0;
        LABJAK_RX _MAINLabjackObj;
        MBIV_RX _myCopyofDataMBIV;
        private int targetSamples = 20; // Set this to the number of samples you want
        private int collectedSamples = 0;
        private bool isFirstSample = true;  // Flag to track if the first sample has been received
        public testAIN()
        {

            InitializeComponent();
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
            timer_100Second.Interval = 100;
            timer_100Second.Tick += new EventHandler(timer_100_Tick);
            timer_powerLevelAdjustment.Interval = 520; // 500 ms for power level adjustment
            timer_powerLevelAdjustment.Tick += new EventHandler(timer_powerLevelAdjustment_Tick);
            MNGR_COMMBIV.Instance.MessageReceived += Instance_MessageReceived;
            btn_startTest.Click += btn_Start_Click;
        }
        private void timer_100_Tick(object sender, EventArgs e)
        {
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, _RAW_DACTOSEND);
        }
        private void Instance_MessageReceived(MBIV_RX message)
        {
            if (InvokeRequired)
            {
                try // Try to invoke the action
                {
                    Invoke(new Action(() =>
                    {
                        SafeInvokedFunction(message);
                    }));
                }
                catch (Exception ex)
                {
                    // Handle exception
                }
            }
        }
        void SafeInvokedFunction(MBIV_RX message)
        {
            if (message == null) return;
            // Check if the first sample has been received
            if (isFirstSample)
            {
                isFirstSample = false;  // Update the flag to indicate the first message has been handled
                return;  // Skip processing for this sample
            }
            _myCopyofDataMBIV = message;
            // Process the data
            _ints_ADOS = _myCopyofDataMBIV.GET_allAINS();
            for (int i = 0; i < 17; i++)
            {
                _lbls_ADOs[i].Text = "AD" + i + ": " + _ints_ADOS[i].ToString();
            }
            if (allowedToFilterMinMax)
            {
                int value = _myCopyofDataMBIV.Get_Stored_AINVal(_ACTIVEAIN);
                measurementValues.Add(value); // Add the new value to the list
                minValue = Math.Min(minValue, value);
                maxValue = Math.Max(maxValue, value);
                indexFloating = (_ACTIVEAIN + 6) % 16 + 1;
                int simulated_floatingValue = _myCopyofDataMBIV.Get_Stored_AINVal(indexFloating);
                _minFloating = Math.Min(_minFloating, simulated_floatingValue);
                _maxFloating = Math.Max(_maxFloating, simulated_floatingValue);
            }
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, _RAW_DACTOSEND);
            // Increment sample count and check if enough samples have been collected
            collectedSamples++;
            if (collectedSamples >= targetSamples)
            {
                CompleteDataCollection();
            }
        }
        private void CompleteDataCollection()
        {
            // Stop any ongoing processes related to data collection
            TimerTICKER_readJackslow.Stop();  // Assuming this needs to be stopped as well
            allowedToFilterMinMax = false;
            collectedSamples = 0; // Reset for the next data collection cycle
            isFirstSample = true;  // Reset the flag to ensure the first sample is skipped in the next cycle
            // Add any additional wrap-up operations here
        }
        private void timer_powerLevelAdjustment_Tick(object sender, EventArgs e)
        {
            timer_powerLevelAdjustment.Stop(); // Stop the power level adjustment timer
        }
        private async void StartProcess()
        {
            ResetMinMax(); // Reset min and max values before starting the data collection process
            collectedSamples = 0; // Ensure the sample count is reset at the start of the process
            allowedToFilterMinMax = true;
            for (int power = 0; power < 3; power++)
            {
                _ACTIVELEVEL = power;
                SetPowerValue(power);
                for (int i = 1; i < 17; i++)
                {
                    _ACTIVEAIN = i;
                    isFirstSample = true;  // Reset first sample flag at the start of each channel measurement
                    measurementValues.Clear();  // Clear the list at the start of each new measurement turn
                    await SetPowerLevel();  // Start power level adjustment and wait
                    await WaitForDataCollection();  // Wait before starting data collection, collect data and process
                    // Remove the first two and last two elements from the list, then find min and max
                    if (measurementValues.Count >= targetSamples)
                    {
                        measurementValues.Sort(); // Sort the list to make removing easier
                        measurementValues.RemoveRange(0, 5); // Remove first two elements
                        measurementValues.RemoveRange(measurementValues.Count - 5, 5); // Remove last two elements
                        minValue = measurementValues.Min();
                        maxValue = measurementValues.Max();
                    }
                    _labels2D_minmax[i, _ACTIVELEVEL].Text = $"{minValue}|{maxValue}";
                    if (power == 0)
                    {
                        floatingColumn[indexFloating].Text = $"{_minFloating}|{_maxFloating}"; // Display the floating column values for the first power level
                    }
                }
            }
        }
        private void ResetMinMax()
        {
            minValue = int.MaxValue; // Reset minimum value
            maxValue = int.MinValue; // Reset maximum value
            _maxFloating = int.MinValue;
            _minFloating = int.MaxValue;
            allowedToFilterMinMax = true; // Enable data filtering for new data collection cycle
        }
        private void SetPowerValue(int powerLevel)
        {
            switch (powerLevel)
            {
                case 0:
                    _RAW_DACTOSEND = 0;
                    break;
                case 1:
                    _RAW_DACTOSEND = 2.5;
                    break;
                case 2:
                    _RAW_DACTOSEND = 5.0;
                    break;
            }
        }
        private async Task SetPowerLevel()
        {
            timer_powerLevelAdjustment.Start();
            await Task.Delay(timer_powerLevelAdjustment.Interval); // Initial delay after setting power level
            timer_powerLevelAdjustment.Stop();
        }
        private async Task WaitForDataCollection()
        {
            // Optional: Add an additional delay here if needed before starting to collect data
            await Task.Delay(500); // Wait for an additional 200 ms before starting data collection to ensure system stability
            TimerTICKER_readJackslow.Start();
            //  timer_oneSecond.Start();
            // await Task.Delay(timer_oneSecond.Interval); // Delay for the timer interval during which data is collected
            // This loop will ensure you keep collecting data until you reach the target number of samples
            //while (collectedSamples < targetSamples)
            //{
            //    await Task.Delay(timer_oneSecond.Interval); // Wait for the timer interval
            //}
            TimerTICKER_readJackslow.Stop();
            // timer_oneSecond.Stop();
            collectedSamples = 0; // Reset for the next data collection cycle
            allowedToFilterMinMax = false;
        }
        private void btn_Start_Click(object sender, EventArgs e)
        {
            StartProcess();
        }

    }
}
