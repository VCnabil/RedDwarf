using LabJack;
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

namespace RedDwarf
{
    public partial class Form3 : Form
    {
        private System.Windows.Forms.Timer TimerForLoop = new System.Windows.Forms.Timer();
        byte dataRED;
        byte dataGREEN;
        byte[] dataArrayRED;
        byte[] dataArrayGREEN;
      //  int i, j;
        enum EnumPins
        {

            None = 0,
            LatchPin = 1,
            DataPin = 2,
            ClockPin = 3
        }
        byte _lowByte = 0x00;
        byte _highByte = 0x00;

        int handle = 0;
        int devType = 0;
        int conType = 0;
        int serNum = 0;
        int ipAddr = 0;
        int port = 0;
        int maxBytesPerMB = 0;
        string ipAddrStr = "";
        string _labSerial = "";
        string _labFirmware = "";
        bool isOnBus = false;

        #region EIO params

        #endregion

        void digitalWrite(EnumPins argpin, int state)
        {

            string lanJackPin = "";
            switch (argpin)
            {
                case EnumPins.LatchPin:
                    lanJackPin = "CIO1";
                    break;
                case EnumPins.DataPin:
                    lanJackPin = "CIO0";
                    break;
                case EnumPins.ClockPin:
                    lanJackPin = "CIO2";
                    break;
                default:
                    break;
            }

            LJM.eWriteName(handle, lanJackPin, state);

        }
        public Form3()
        {
            if (isOnBus) return;

            LJM.OpenS("ANY", "ANY", "ANY", ref handle);
            LJM.GetHandleInfo(handle, ref devType, ref conType, ref serNum, ref ipAddr, ref port, ref maxBytesPerMB);
            int numFrames = 3;
            string[] names = new string[3] { "SERIAL_NUMBER", "PRODUCT_ID", "FIRMWARE_VERSION" };
            double[] aValues = new double[3] { 0, 0, 0 };
            int errorAddress = 0;
            LJM.eReadNames(handle, numFrames, names, aValues, ref errorAddress);
            _labSerial = aValues[0].ToString();
            _labFirmware = aValues[2].ToString();


            if (maxBytesPerMB > 1)
            {
                isOnBus = true;
                Debug.WriteLine(_labSerial);
                Debug.WriteLine(_labFirmware);



            }
            else
            {
                isOnBus = false;
            }



            dataArrayRED = new byte[10];
            dataArrayGREEN = new byte[10];

            dataArrayRED[0] = 0xFF; //11111111
            dataArrayRED[1] = 0xFE; //11111110
            dataArrayRED[2] = 0xFC; //11111100
            dataArrayRED[3] = 0xF8; //11111000
            dataArrayRED[4] = 0xF0; //11110000
            dataArrayRED[5] = 0xE0; //11100000
            dataArrayRED[6] = 0xC0; //11000000
            dataArrayRED[7] = 0x80; //10000000
            dataArrayRED[8] = 0x00; //00000000
            dataArrayRED[9] = 0xE0; //11100000
                                    //Arduino doesn't seem to have a way to write binary straight into the code
                                    //so these values are in HEX.  Decimal would have been fine, too.
            //dataArrayGREEN[0] = 0xFF; //11111111
            //dataArrayGREEN[1] = 0x7F; //01111111
            //dataArrayGREEN[2] = 0x3F; //00111111
            //dataArrayGREEN[3] = 0x1F; //00011111
            //dataArrayGREEN[4] = 0x0F; //00001111
            //dataArrayGREEN[5] = 0x07; //00000111
            //dataArrayGREEN[6] = 0x03; //00000011
            //dataArrayGREEN[7] = 0x01; //00000001
            //dataArrayGREEN[8] = 0x00; //00000000
            //dataArrayGREEN[9] = 0x07; //00000000

            for (int i = 0; i < 10; i++)
            {
                dataArrayGREEN[i] = (byte)(0xFF);
            }
            InitializeComponent();

            label1.Text = _labSerial;


            TimerForLoop.Interval = 200;
            TimerForLoop.Tick += Loop;
            TimerForLoop.Start();

            //  blinkAll_2Bytes(2, 500);

            //button1.Click += Button1_Click;
            //button2.Click += Button2_Click;
            btn_B0_ConvertBitsToHEX.Click += Btn_B0_ConvertBitsToHEX_Click;
            btn_B1_ConvertBitsToHEX.Click += Btn_B1_ConvertBitsToHEX_Click;
        }

        private void Btn_B0_ConvertBitsToHEX_Click(object sender, EventArgs e)
        {
            byte _result_B0 = 0x00 ;

            if (cb_B0_b0.Checked) _result_B0 += 0x01;
            if (cb_B0_b1.Checked) _result_B0 += 0x02;
            if (cb_B0_b2.Checked) _result_B0 += 0x04;
            if (cb_B0_b3.Checked) _result_B0 += 0x08;
            if (cb_B0_b4.Checked) _result_B0 += 0x10;
            if (cb_B0_b5.Checked) _result_B0 += 0x20;
            if (cb_B0_b6.Checked) _result_B0 += 0x40;
            if (cb_B0_b7.Checked) _result_B0 += 0x80;

            label_Byte0.Text = _result_B0.ToString("X");
            _lowByte = _result_B0;
            UpdateDisplay();
        }

        private void Btn_B1_ConvertBitsToHEX_Click(object sender, EventArgs e)
        {
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
            _highByte = _result_B1;
            UpdateDisplay();
        }

        //private void Button1_Click(object sender, EventArgs e)
        //{
        //    i++;
        //    if (i > 9) i = 0;
        //    UpdateOutputs();
        //}

        //private void Button2_Click(object sender, EventArgs e)
        //{
        //  //  j++;
        // //   if (j > 9) j = 0;
        //   UpdateOutputs();
        //}

        //private void UpdateOutputs()
        //{
        //    dataRED = dataArrayRED[i];
        //    dataGREEN = dataArrayGREEN[j];
        //    UpdateDisplay();
        //}

        private void UpdateDisplay()
        {
            // Assume EnumPins.DataPin and EnumPins.ClockPin are defined elsewhere correctly
            digitalWrite(EnumPins.LatchPin, 0);  // Prepare latch
            shiftOut(_lowByte);  // Output green data
            shiftOut(_highByte);    // Output red data
            digitalWrite(EnumPins.LatchPin, 1);  // Latch data
        }

        void shiftOut(byte myDataOut)
        {
            for (int bitIndex = 7; bitIndex >= 0; bitIndex--)
            {
                int pinState = ((myDataOut & (1 << bitIndex)) != 0) ? 1 : 0;
                digitalWrite(EnumPins.DataPin, pinState);
                digitalWrite(EnumPins.ClockPin, 1);  // Clock high: shift in the bit
                digitalWrite(EnumPins.DataPin, 0);   // Reset data pin
                digitalWrite(EnumPins.ClockPin, 0);  // Clock low: prepare for next bit
            }
        }
        private void Loop(object sender, EventArgs e)
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
            _lowByte = _result_B0;

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
            _highByte = _result_B1;


            UpdateDisplay();
        }
    }
}
