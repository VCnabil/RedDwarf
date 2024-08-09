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
using LabJack;
using System.Runtime.InteropServices.ComTypes;

namespace RedDwarf
{
    public partial class Form2 : Form
    {
        private System.Windows.Forms.Timer TimerForLoop = new System.Windows.Forms.Timer();
        byte dataRED;
        byte dataGREEN;
        byte[] dataArrayRED;
        byte[] dataArrayGREEN;

        // int latchPin = 8; //GREEN WIRE      EIO5
        // int dataPin = 11;  //BLUE WIRE      EIO7
        // int clockPin = 12; //YELLOW WIRE    EIO6

        enum EnumPins { 
         
            None = 0,
            LatchPin = 1,
            DataPin = 2,
            ClockPin = 3
        }

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
        public Form2()
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
            dataArrayGREEN[0] = 0xFF; //11111111
            dataArrayGREEN[1] = 0x7F; //01111111
            dataArrayGREEN[2] = 0x3F; //00111111
            dataArrayGREEN[3] = 0x1F; //00011111
            dataArrayGREEN[4] = 0x0F; //00001111
            dataArrayGREEN[5] = 0x07; //00000111
            dataArrayGREEN[6] = 0x03; //00000011
            dataArrayGREEN[7] = 0x01; //00000001
            dataArrayGREEN[8] = 0x00; //00000000
            dataArrayGREEN[9] = 0x07; //00000111

            InitializeComponent();

            label1.Text = _labSerial;


            TimerForLoop.Interval = 300;
            TimerForLoop.Tick += Loop;
            TimerForLoop.Start();

          //  blinkAll_2Bytes(2, 500);

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            j++;
            if (j > 9)
            {
                j = 0;
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            i++;
            if (i > 9)
            {
                i = 0;
            }
        }
        int j = 0;
        int i = 0;

        private void Loop(object sender, EventArgs e)
        {
            //load the light sequence you want from array
            dataRED = dataArrayRED[j];
            dataGREEN = dataArrayGREEN[j];
            //ground latchPin and hold low for as long as you are transmitting
            digitalWrite(EnumPins.LatchPin, 0);
            //move 'em out
            //shiftOut(dataPin, clockPin, dataGREEN);
            //shiftOut(dataPin, clockPin, dataRED);
            shiftOut(dataGREEN);
            shiftOut(dataRED);
            //return the latch pin high to signal chip that it
            //no longer needs to listen for information
            digitalWrite(EnumPins.LatchPin, 1);

            //for (int j = 0; j < 10; j++)
            //{

            //}

        }

        void shiftOut(byte myDataOut) {
            int i = 0;
            int pinState;
            //for each bit in the byte myDataOut&#xFFFD;
            //NOTICE THAT WE ARE COUNTING DOWN in our for loop
            //This means that %00000001 or "1" will go through such
            //that it will be pin Q0 that lights.
            for (i = 7; i >= 0; i--)
            {
                //if the value passed to myDataOut and a bitmask result
                // true then... so if we are at i=6 and our value is
                // %11010100 it would the code compares it to %01000000
                // and proceeds to set pinState to 1.
                if ((myDataOut & (1 << i)) != 0)
                {
                    pinState = 1;
                }
                else
                {
                    pinState = 0;
                }

                digitalWrite(EnumPins.DataPin, pinState);

                //register shifts bits on upstroke of clock pin
                digitalWrite(EnumPins.ClockPin, 1);
                //zero the data pin after shift to prevent bleed through
                digitalWrite(EnumPins.DataPin, 0);

            }
            //stop shifting
            digitalWrite(EnumPins.ClockPin, 0);
        }
        //blinks the whole register based on the number of times you want to
        //blink "n" and the pause between them "d"
        //starts with a moment of darkness to make sure the first blink
        //has its full visual effect.


        void blinkAll_2Bytes(int n, int d)
        {

            digitalWrite(EnumPins.LatchPin, 0);
            shiftOut(0);
            shiftOut(0);
            digitalWrite(EnumPins.LatchPin, 1);
            System.Threading.Thread.Sleep(200);
            for (int x = 0; x < n; x++)
            {
                digitalWrite(EnumPins.LatchPin, 0);
                shiftOut(0);
                shiftOut(16);
                digitalWrite(EnumPins.LatchPin, 1);
                System.Threading.Thread.Sleep(d);
                digitalWrite(EnumPins.LatchPin, 0);
                shiftOut(0);
                shiftOut(0);
                digitalWrite(EnumPins.LatchPin, 1);
                System.Threading.Thread.Sleep(d);
            }
        }
    }
}
