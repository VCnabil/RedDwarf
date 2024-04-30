using RedDwarf.RedAwarf._DataObjz.DataCOMM;
using RedDwarf.RedAwarf._DataObjz.DataTestReport;
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


        int _MAX_AINS = 16; //full =16 , 2

        int _MAX_LVLS = 2;
 
        int WaitToTakeEffect = 150;
        int WAIT_DIO_EFFECT = 4000;
        int targetSamples = 7;
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

        DATA_CELL_MEASURES[,] _dataCellMeasures;
        DATA_CELL_MEASURES[] _floatingCellsMEasures;

        MBIV_RX _myCopyofDataMBIV;
        LABJAK_RX _MAINLabjackObj;

        int _FULL_MAX_AINS = 17;
        int _FULL_MAX_LVLS = 2;

        private List<int>[] dataFloatingReadingsArray;

        bool is_cycling = false;

        DATA_TX _DATA_TX = new DATA_TX();
        public Section3_AIN()
        {
            InitializeComponent();

            _FULL_MAX_AINS = _MAX_AINS + 1;
            _FULL_MAX_LVLS = _MAX_LVLS + 1;

            dataFloatingReadingsArray = new List<int>[17]; // Initialize the array for 17 lists
            for (int i = 0; i < dataFloatingReadingsArray.Length; i++)
            {
                dataFloatingReadingsArray[i] = new List<int>(); // Initialize each list in the array
            }

            _floatingCellsMEasures = new DATA_CELL_MEASURES[_FULL_MAX_AINS];
            for (int i = 0; i < _FULL_MAX_AINS; i++)
            {
                _floatingCellsMEasures[i] = new DATA_CELL_MEASURES(Helpers.GetExpectedMinValue(3), Helpers.GetExpectedMaxValue(3));
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



            _dataCellMeasures = new DATA_CELL_MEASURES[_FULL_MAX_AINS, _FULL_MAX_LVLS];
            for (int i = 0; i < _FULL_MAX_AINS; i++)
            {
                for (int j = 0; j < _FULL_MAX_LVLS; j++)
                {
                    _dataCellMeasures[i, j] = new DATA_CELL_MEASURES(Helpers.GetExpectedMinValue(j), Helpers.GetExpectedMaxValue(j));
                }
            }
            floatingColumn = new Label[] { label_0_3, label_1_3, label_2_3, label_3_3, label_4_3, label_5_3, label_6_3, label_7_3, label_8_3, label_9_3, label_10_3, label_11_3, label_12_3, label_13_3, label_14_3, label_15_3, label_16_3 };
            _ints_ADOS = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
           
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(16, 0);
            btn_testFloat.Click += btn_testFloat_Click;
            btn_startTest7DI.Click += btn_startTest7DI_Click;

            label_0_0.Text = Helpers.GetExpectedMinValue(0) + " - " + Helpers.GetExpectedMaxValue(0);
            label_0_1.Text = Helpers.GetExpectedMinValue(1) + " - " + Helpers.GetExpectedMaxValue(1);
            label_0_2.Text = Helpers.GetExpectedMinValue(2) + " - " + Helpers.GetExpectedMaxValue(2);
            label_0_3.Text = Helpers.GetExpectedMinValue(3) + " - " + Helpers.GetExpectedMaxValue(3);

            btn_startTest4DO.Click += btn_startTest4DO_Click;




        }
        private async Task startTest4D (){
            MNGR_COMMBIV.Instance.WRITEDATA_FIO(cb_xfer1CMD.Checked, cb_xfer2CMD.Checked, cb_DK1CMD.Checked, cb_DK2CMD.Checked, cb_CL1CMD.Checked, cb_CL2CMD.Checked);

            await Task.Delay(WAIT_DIO_EFFECT);

            cb_DKtr1.Checked = _myCopyofDataMBIV.GP3_Dktr1;
            cb_DKtr2.Checked = _myCopyofDataMBIV.GP4_DKtr2;
            cb_Xfer1.Checked = _myCopyofDataMBIV.GP5_Xfer1;
            cb_Xfer2.Checked = _myCopyofDataMBIV.GP6_Xfer2;
            cb_Clutch1.Checked = _myCopyofDataMBIV.GP0_sClutch;
            cb_Clutch2.Checked = _myCopyofDataMBIV.GP7_pClutch;
        }
        private async Task startTest7DI() {

            //set led1 led2 alarm to on 
            cb_cmdDiO_0_led1.Checked = true;
            cb_cmdDiO_1_led2.Checked = true;
            cb_cmdDiO_2_alarm.Checked = true;

            _DATA_TX.SetDIO(cb_cmdDiO_2_alarm.Checked, cb_cmdDiO_1_led2.Checked, cb_cmdDiO_0_led1.Checked, true);
            MNGR_COMMBIV.Instance.WriteData__MBIV(_DATA_TX);
            await Task.Delay(WAIT_DIO_EFFECT);
            _MAINLabjackObj = MNGR_COMMBIV.Instance.READDATA____JACK();
            if (!_MAINLabjackObj.LED1StaeOn)
            {
                lbl_LED1_EIO0.BackColor = Color.SeaGreen;
            }
            else
            {
                lbl_LED1_EIO0.BackColor = Color.Salmon;
            }

            if (!_MAINLabjackObj.LED2StaeOn)
            {
                lbl_LED2_EIO1.BackColor = Color.SeaGreen;
            }
            else
            {
                lbl_LED2_EIO1.BackColor = Color.Salmon;
            }

            if (!_MAINLabjackObj.AlarmStateON)
            {
                lbl_Alarm_AIN0.BackColor = Color.SeaGreen;
            }
            else
            {
                lbl_Alarm_AIN0.BackColor = Color.Salmon;
            }
        }
        private async void btn_startTest4DO_Click(object sender, EventArgs e)
        {


            MNGR_COMMBIV.Instance.WRITEDATA_FIO(cb_xfer1CMD.Checked, cb_xfer2CMD.Checked, cb_DK1CMD.Checked, cb_DK2CMD.Checked, cb_CL1CMD.Checked, cb_CL2CMD.Checked);

            await Task.Delay(WAIT_DIO_EFFECT);

            cb_DKtr1.Checked = _myCopyofDataMBIV.GP3_Dktr1;
            cb_DKtr2.Checked = _myCopyofDataMBIV.GP4_DKtr2;
            cb_Xfer1.Checked = _myCopyofDataMBIV.GP5_Xfer1;
            cb_Xfer2.Checked = _myCopyofDataMBIV.GP6_Xfer2;
            cb_Clutch1.Checked = _myCopyofDataMBIV.GP0_sClutch;
            cb_Clutch2.Checked = _myCopyofDataMBIV.GP7_pClutch;






        }

        private async void btn_startTest7DI_Click(object sender, EventArgs e)
        {



             

   
            //set led1 led2 alarm to on 
            cb_cmdDiO_0_led1.Checked = true;
            cb_cmdDiO_1_led2.Checked = true;
            cb_cmdDiO_2_alarm.Checked = true;

            _DATA_TX.SetDIO(cb_cmdDiO_2_alarm.Checked, cb_cmdDiO_1_led2.Checked, cb_cmdDiO_0_led1.Checked, true);
            MNGR_COMMBIV.Instance.WriteData__MBIV(_DATA_TX);
            await Task.Delay(WAIT_DIO_EFFECT);
            _MAINLabjackObj = MNGR_COMMBIV.Instance.READDATA____JACK();
            if (!_MAINLabjackObj.LED1StaeOn)
            {
                lbl_LED1_EIO0.BackColor = Color.SeaGreen;
            }
            else
            {
                lbl_LED1_EIO0.BackColor = Color.Salmon;
            }

            if (!_MAINLabjackObj.LED2StaeOn)
            {
                lbl_LED2_EIO1.BackColor = Color.SeaGreen;
            }
            else
            {
                lbl_LED2_EIO1.BackColor = Color.Salmon;
            }

            if (!_MAINLabjackObj.AlarmStateON)
            {
                lbl_Alarm_AIN0.BackColor = Color.SeaGreen;
            }
            else
            {
                lbl_Alarm_AIN0.BackColor = Color.Salmon;
            }

        }

        private async  void btn_testFloat_Click(object sender, EventArgs e)
        {
       //     aINtestSTATE = AINtestSTATE.SHOULDNOTREAD;
            _ACTIVEAIN = _MAX_AINS;
            _ACTIVELEVEL = _MAX_LVLS;
            MNGR_COMMBIV.Instance.WRITEDATA_MUXDAC(_ACTIVEAIN, _ACTIVELEVEL);
            btn_testFloat.BackColor = Color.Green;
            await Task.Delay(WaitToTakeEffect);
          //  aINtestSTATE = AINtestSTATE.MEASURING_BULK;
            
            await RunMeasurementProcess();
            btn_testFloat.BackColor = Color.Red;
        }

        private async Task RunMeasurementProcess()
        {

            await Start_FULLARRAY_Measurement();
            btn_testFloat.BackColor = Color.CadetBlue;
            await StartMeasuring__AllFloatersLessLAst();
            btn_testFloat.BackColor = Color.Yellow;
            await StartMeasuring__lastFloater();
            btn_testFloat.BackColor = Color.Purple;
            await Start_Validate_Measurement();
            btn_testFloat.BackColor = Color.Blue;

            await startTest4D();
            btn_testFloat.BackColor = Color.Green;
            await startTest7DI();
        }


        private async Task Start_Validate_Measurement()
        {
            is_cycling = false;

            await Task.Delay(WaitToTakeEffect);

            for (int i = 1; i <= _MAX_AINS; i++)
            {
                for (int j = 0; j <= _MAX_LVLS; j++)
                {
                    if (_dataCellMeasures[i, j].MinValue < Helpers.GetExpectedMinValue(j) || _dataCellMeasures[i, j].MaxValue > Helpers.GetExpectedMaxValue(j))
                    {
                        _labels2D_minmax[i, j].BackColor = Color.Red;
                    }
                    else
                    {
                        _labels2D_minmax[i, j].BackColor = Color.Green;
                    }
                }
            }   

            for (int i = 1; i <= _MAX_AINS; i++)
            {
                if (_floatingCellsMEasures[i].MinValue < Helpers.GetExpectedMinValue(3) || _floatingCellsMEasures[i].MaxValue > Helpers.GetExpectedMaxValue(3))
                {
                    floatingColumn[i].BackColor = Color.Red;
                }
                else
                {
                    floatingColumn[i].BackColor = Color.Green;
                }
            }
           
        }
        void ClearAll_Floaters()
        {
            for (int i = 1; i < _FloaterIndexTOAVOID; i++)
            {
                dataFloatingReadingsArray[i].Clear();  
            }
        }


        #region Full_ARRAY_MEASUREMENT
        private async Task Start_FULLARRAY_Measurement()
        {
            is_cycling = true;
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
            dataFloatingReadingsArray[curAIN].Clear(); // Clear existing exact data
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
                //_labels2D_minmax[index, powerLevel].Text = $"i:{min} -: {max} v: {average:N2}";
                _labels2D_minmax[index, powerLevel].Text = $"{min} - {max}";
                _dataCellMeasures[index, powerLevel].MinValue = min;
                _dataCellMeasures[index, powerLevel].MaxValue = max;
                _dataCellMeasures[index, powerLevel].AverageValue = average;
                _dataCellMeasures[index, powerLevel].SamplesTaken = readings.Count;
            }
            dataFloatingReadingsArray[index].Clear(); // Clear existing exact data after processing
        }
        #endregion

        #region FLoaters


        private async Task StartMeasuring__AllFloatersLessLAst()
        {
            is_cycling = false;
            ClearAll_Floaters();

            _FloaterIndexTOAVOID = _MAX_AINS;
            MNGR_COMMBIV.Instance.WRITEDATA_CHAN_LVL(_FloaterIndexTOAVOID, 0, false);
            await Task.Delay(WaitToTakeEffect);

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
                    // floatingColumn[i].Text = $"i:{min} a: {max} v: {average:N2}";
                    floatingColumn[i].Text = $"{min} - {max}";
                    _floatingCellsMEasures[i].MinValue = min;
                    _floatingCellsMEasures[i].MaxValue = max;
                    _floatingCellsMEasures[i].AverageValue = average;
                    _floatingCellsMEasures[i].SamplesTaken = readings.Count;
                }
                dataFloatingReadingsArray[i].Clear(); // Clear existing data after processing
            }
        }
        #endregion

        #region LAstFloater
        private async Task StartMeasuring__lastFloater()
        {
            is_cycling = false;
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
                //  floatingColumn[index].Text = $"i:{min} a: {max} v: {average:N2}";
                floatingColumn[index].Text = $"{min} - {max}";
                _floatingCellsMEasures[index].MinValue = min;
                _floatingCellsMEasures[index].MaxValue = max;
                _floatingCellsMEasures[index].AverageValue = average;
                _floatingCellsMEasures[index].SamplesTaken = readings.Count;
            }
            dataFloatingReadingsArray[index].Clear(); // Clear existing data after processing
        }
        #endregion




        private void timer_100_Tick(object sender, EventArgs e)
        {

            if (is_cycling)
            {
                for (int i = 1; i < 17; i++)
                {


                    if (i == _ACTIVEAIN)
                    {
                        _lbls_ADOs[i].Text = "o" + i + ": " + _ints_ADOS[i].ToString();
                        _lbls_ADOs[i].Font = new Font(_lbls_ADOs[i].Font, FontStyle.Bold);
                    }
                    else
                    {
                        _lbls_ADOs[i].Text = "";
                        _lbls_ADOs[i].Font = new Font(_lbls_ADOs[i].Font, FontStyle.Regular);
                    }
                }
            }
            else {

                for (int i = 1; i < 17; i++)
                {
                    _lbls_ADOs[i].Text = "o" + i + ": " + _ints_ADOS[i].ToString();
                    _lbls_ADOs[i].Font = new Font(_lbls_ADOs[i].Font, FontStyle.Regular);
                  
                }
            }
      

            //make curent text bold
         

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

 