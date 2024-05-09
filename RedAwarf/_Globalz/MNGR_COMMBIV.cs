using RedDwarf.RedAwarf._DataObjz.DataCOMM;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using LabJack;
using System.Diagnostics;
namespace RedDwarf.RedAwarf._Globalz
{
    public class MNGR_COMMBIV : IDisposable
    {
        private static readonly Lazy<MNGR_COMMBIV> _instance = new Lazy<MNGR_COMMBIV>(() => new MNGR_COMMBIV());
        private SerialPort serialPort;
        private StringBuilder incomingDataBuffer;
        private string lastCompleteMessage = string.Empty;
        private string latestComplete_Validated_MessageBody = string.Empty;
        private string latestCompleteMessageExtratedCHecksum = string.Empty;
        public static MNGR_COMMBIV Instance { get { return _instance.Value; } }
        public delegate void MessageReceivedHandler(MBIV_RX  message);
        public event MessageReceivedHandler MessageReceived;
        private bool disposed = false;
        MBIV_RX _myMBIV_RX;
        DATA_TX _myDATA_TX;
        private Timer commTimer;
        public delegate void TimerElapsedHandler(bool argPortStatus);
        public event TimerElapsedHandler TimerElapsed;
        string[] AvailablePortNAmes;

        public delegate void aPortHasOpenedHandler(string argPortName, bool argOpenedTrue_closedfalse);
        public event aPortHasOpenedHandler aPortHasOpened_orCloesdEVENT;

        public delegate void FirstMEssageWasReceivedHandler(string argVersion);
        public event FirstMEssageWasReceivedHandler FirstMEssageWasReceived;

        public delegate void aLabJackDataReceivedHandler(string argSerial, string argFirmware);
        public event aLabJackDataReceivedHandler aLabJackDataReceived;

        string _foundMBIV_softwareVersion = "";
        bool _FirstMessgeWasRead = false;


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
        #region MUXDAC vars
        const int Const_MUX_Entries = 5;
        int _num_MUXDAC_enries = 5;
        public int Num_MUXDAC_enries
        {
            get { return _num_MUXDAC_enries; }
            private set { _num_MUXDAC_enries = value; }
        }
        public string[] MUXDAC_names_withFIOS;
        public double[] MUXDAC_values_WithFIOS;
        #endregion
        bool muxbit0 = true;
        bool muxbit1 = true;
        bool muxbit2 = true;
        bool muxbit3 = true;
        double _RAW_DACTOSEND = 0.0;
        #region EIO for digital inputs xfer dk clutch
        const int ConstEIOs_forXferDkCLu = 6;
        int _num_EIOForXferDKCLU = 6;
        public int Num_EIOs_ForXferDKCLU
        {
            get { return _num_EIOForXferDKCLU; }
            private set { _num_EIOForXferDKCLU = value; }
        }
        public string[] EIOs_names_forXferDKCLU;
        public double[] EIOs_values_forXferDKCLU;
        #endregion
        double xfer1Val = 0;
        double xfer2Val = 0;
        double dktr1Val = 0;
        double dktr2Val = 0;
        double clutch1Va = 0;
        double clutch2Val = 0;
        #region AINs vars
        const int ConstAINs = 4;
        int _num_AINs = 4;
        public int Num_AINs
        {
            get { return _num_AINs; }
            private set { _num_AINs = value; }
        }
        public string[] AINs_names;
        public double[] AINs_values;
        #endregion
        LABJAK_RX _myLABJK_RX;


        private DateTime lastReceivedTime = DateTime.MinValue;
        private MNGR_COMMBIV() {
           INIT_CONSTRUCTOR_COMM();
           // INIT_CON_LABJACK();
        }

        void INIT_CONSTRUCTOR_COMM() {
            incomingDataBuffer = new StringBuilder();
            serialPort = new SerialPort
            {
                //PortName = "COM7", // "COM7 is default here on my pc for serial to MBIV serial connection. but it can be over written later with OpenPort(portname)"
                BaudRate = 19200,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };
            _myMBIV_RX = new MBIV_RX();
            _myDATA_TX = new DATA_TX();
            _myLABJK_RX = new LABJAK_RX();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);

            AvailablePortNAmes = SerialPort.GetPortNames();

            commTimer = new Timer(100); // Set interval to 100 milliseconds
            commTimer.Elapsed += OnTimerElapsed;
            commTimer.AutoReset = true;
            commTimer.Enabled = true;
            StartTimer();
        }

        public string[] GetAvailablePortNames()
        {
            AvailablePortNAmes = SerialPort.GetPortNames();
            return AvailablePortNAmes;
        }
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            
            if (serialPort == null) {
                TimerElapsed?.Invoke(false);
                return;
            }
            else if (!serialPort.IsOpen)
            {
                TimerElapsed?.Invoke(false);
                return;
            }
            else
            {
                TimerElapsed?.Invoke(true); //and continue to write to the port
            }
          
        }

         void StartTimer()
        {
            commTimer.Start();
        }

         void StopTimer()
        {
            commTimer.Stop();
        }

        ~MNGR_COMMBIV()
        {
            Dispose(false);
        }

        void OnLabJack_FirstInfo_Received(string argSerial, string argFirmware)
        {
            aLabJackDataReceived?.Invoke(argSerial, argFirmware);
        }
        protected void OnFirstMEssageWasReceived(string argVersion)
        {
            FirstMEssageWasReceived?.Invoke(argVersion);
        }
        protected virtual void OnPortHasHopened_or_closed(string argPortName, bool argstate)
        {
            aPortHasOpened_orCloesdEVENT?.Invoke(argPortName, argstate);
        }
        protected virtual void OnMessageReceived(MBIV_RX message)
        {
            MessageReceived?.Invoke(message);
        }
        //*****************************************************************************************************************tSerial is received and processed here
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            incomingDataBuffer.Append(sp.ReadExisting());
            ProcessBuffer();
        }
        // *****************************************************************************************************************t then it fires an event with a MBIV object to be used by the UI
        private void ProcessBuffer() {
            // This regex matches the pattern of a complete message ending with *XX where XX are hex digits
            var regex = new Regex(@"\*[\dA-Fa-f]{2}");
            string buffer = incomingDataBuffer.ToString();
            Match match = regex.Match(buffer);
            // Initialize the last complete message to an empty string at the start of processing
            string mostRecentMessage = string.Empty;


            while (match.Success)
            {
                // Extract the complete message up to the end of the checksum
                int endIndex = match.Index + match.Length;
                string completeData = buffer.Substring(0, endIndex);
                // Update the most recent message
                mostRecentMessage = completeData;
                // Log or process the complete message

                //OnMessageReceived(mostRecentMessage);  // Fire the event
                // Prepare for the next message
                buffer = buffer.Substring(endIndex);
                match = regex.Match(buffer);
            }

            // Keep the last complete message
            if (!string.IsNullOrEmpty(mostRecentMessage))
            {
                lastCompleteMessage = mostRecentMessage;

                // Extract the message body and checksum
                int indexDollar = mostRecentMessage.IndexOf("$");//at index 1 
                int index = mostRecentMessage.IndexOf("*");//at indesx 128
                                                           //EventsManagerLib.Call_LogConsole("2. Last complete message: " + mostRecentMessage + "has a $ at index: " + indexDollar + " and * at index: " + index);

                if (indexDollar > 0 && index > indexDollar + 1)
                {
                    latestComplete_Validated_MessageBody = mostRecentMessage.Substring(indexDollar, index - indexDollar);
                    latestCompleteMessageExtratedCHecksum = mostRecentMessage.Substring(index + 1);
                    //  EventsManagerLib.Call_LogConsole("3. Last complete message body: " + latestComplete_Validated_MessageBody + " and checksum: " + latestCompleteMessageExtratedCHecksum);
                    bool isChecksumValid = Helpers_Vealidate_Checksum(latestComplete_Validated_MessageBody, latestCompleteMessageExtratedCHecksum);
                    if (isChecksumValid)
                    {
                        DateTime currentTime = DateTime.Now;
                        TimeSpan interval = TimeSpan.Zero;

                        if (lastReceivedTime != DateTime.MinValue)
                        {
                            interval = currentTime - lastReceivedTime;
                        }
                        lastReceivedTime = currentTime;

                        //     EventsManagerLib.Call_LogConsole("4. Last complete message: " + mostRecentMessage + " has a valid checksum");
                        _myMBIV_RX.Update_IntArray_withTimeDate(latestComplete_Validated_MessageBody, currentTime, interval );
                        _foundMBIV_softwareVersion = _myMBIV_RX.Version;
                        if (!_FirstMessgeWasRead) {
                            _FirstMessgeWasRead= true;
                            OnFirstMEssageWasReceived(_foundMBIV_softwareVersion);
                           // INIT_CON_LABJACK();
                        }

                      

                        OnMessageReceived(_myMBIV_RX);
                    }
                    else
                    {
                        EventsManagerLib_Call_LogConsole("4. Last complete message: " + mostRecentMessage + " has an invalid checksum");
                    }

                }
                else
                {
                    EventsManagerLib_Call_LogConsole("4. Last complete message: " + mostRecentMessage + " has no $ or *");
                }
            }
            // Keep the unprocessed part in the buffer
            incomingDataBuffer.Clear();
            incomingDataBuffer.Append(buffer);
        }
        bool Helpers_Vealidate_Checksum(string argMessageBody, string arg_receivedChecksum)
        {

            string __calculatedChecksum = Helpers_Generate_Checksum_fromBody(argMessageBody);
            if (__calculatedChecksum != arg_receivedChecksum)
            {
                return false;
            }
            return true;
        }
        string Helpers_Generate_Checksum_fromBody(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            short csum = 0;
            byte[] strg;
            int idx = 1;  // Start after the '$' character to compute checksum correctly

            strg = Encoding.ASCII.GetBytes(input);

            //while not end of string only 
            while (idx < strg.Length)
            {
                csum ^= strg[idx];  // XOR each byte with the checksum
                idx++;
            }
            return csum.ToString("X2");  // Convert the checksum to a hex string
        }

        void EventsManagerLib_Call_LogConsole(string argstr) { 
            Console.WriteLine(argstr);
        }
        public void OpenPort(string portName)
        {
            if (!serialPort.IsOpen)
            {
                serialPort.PortName = portName;
                serialPort.Open();
            }
            if (serialPort.IsOpen)
            {
                OnPortHasHopened_or_closed(portName, true);
            }
        }
        public void ClosePort()
        {
           if (serialPort.IsOpen){
                serialPort.Close();
            }
           if (!serialPort.IsOpen)
            {
                _FirstMessgeWasRead = false;
                OnPortHasHopened_or_closed(serialPort.PortName, false);
            }
        }




        //*****************************************************************************************************************this will need to be sent constantly to the MBIV. we will use the onmessagereceived to prompt a new write 
        public void WriteData__MBIV(DATA_TX argDatatxObj)
        {

            string data = argDatatxObj.CREATE_FullString_for_TX();
            if (serialPort.IsOpen)
            {
                serialPort.Write(data);
            }
            else
            {
                throw new InvalidOperationException("Attempt to write to closed serial port.");
            }
        }


        bool[] _virtualRellayArray = new bool[16];

        byte[] dataArrayRED;
        byte[] dataArrayGREEN;
        enum EnumPins
        {

            None = 0,
            LatchPin = 1,
            DataPin = 2,
            ClockPin = 3
        }
        byte _lowByte = 0x00;
        byte _highByte = 0x00;
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
        private void UpdateDisplay()
        {
            // Assume EnumPins.DataPin and EnumPins.ClockPin are defined elsewhere correctly
            digitalWrite(EnumPins.LatchPin, 0);  // Prepare latch
            shiftOut(_lowByte);  // Output green data
            shiftOut(_highByte);    // Output red data
            digitalWrite(EnumPins.LatchPin, 1);  // Latch data
        }

        void Update_virtualRellaystate(byte argLwbute, byte argHbyte)
        {
            // Update the first 8 bits using the low byte
            for (int i = 0; i < 8; i++)
            {
                _virtualRellayArray[i] = (argLwbute & (1 << i)) != 0;
            }

            // Update the second 8 bits using the high byte
            for (int i = 0; i < 8; i++)
            {
                _virtualRellayArray[8 + i] = (argHbyte & (1 << i)) != 0;
            }

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
        public void RELAYARRAY_SET(byte arg_lowb, byte arg_highb) {
            Update_virtualRellaystate(arg_lowb, arg_highb);
            digitalWrite(EnumPins.LatchPin, 0);
            shiftOut(arg_lowb);
            shiftOut(arg_highb);
            digitalWrite(EnumPins.LatchPin, 1);
        }

        public void turnonAllRelays()
        {
            RELAYARRAY_SET(0x00, 0x00);
        }   
        public void turnoffAllRelays()
        {
            RELAYARRAY_SET(0xFF, 0xFF);
        }

        public void OpenMainPowerSource()
        {
            LJM.eWriteName(handle, "CIO3", 1);
        }
        public void CloseMainPowerSource()
        {
            LJM.eWriteName(handle, "CIO3", 0);
        }

        public void RelayBlock_powerOn()
        {
            LJM.eWriteName(handle, "FIO6", 1);
        }
        public void RelayBlock_powerOff()
        {
            LJM.eWriteName(handle, "FIO6", 0);
        }
        public void SetDAC1_toHIgh() {
            
            LJM.eWriteName(handle, "DAC1", 5.0);
        }

        public void SetDAC1_toLow()
        {
            LJM.eWriteName(handle, "DAC1", 0.0);
        }

        public double Get_Value_AIN0() {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN0", ref _temVal);
            return _temVal;
        }

        public double Get_Value_AIN1()
        {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN1", ref _temVal);
            return _temVal;
        }

        //holds PI value
        public double Get_Value_AIN2()
        {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN2", ref _temVal);
            return _temVal;
        }
        //holds SI value
        public double Get_Value_AIN3()
        {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN3", ref _temVal);
            return _temVal;
        }
        //holds PB value
        public double Get_Value_AIN4()
        {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN4", ref _temVal);
            return _temVal;
        }
        //holds PN value
        public double Get_Value_AIN5()
        {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN5", ref _temVal);
            return _temVal;
        }
        //holds SB value
        public double Get_Value_AIN6()
        {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN6", ref _temVal);
            return _temVal;
        }
        //holds SN value
        public double Get_Value_AIN7()
        {

            double _temVal = 0;
            LJM.eReadName(handle, "AIN7", ref _temVal);
            return _temVal;
        }





        //---------------------------------------------------------LABJACK
        public void INIT_CON_LABJACK() {
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
               OnLabJack_FirstInfo_Received(_labSerial, _labFirmware);

                aLabJackDataReceived?.Invoke(_labSerial, _labFirmware);
            }
            else
            {
                isOnBus = false;
            }


            #region MUXDAC init
            _num_MUXDAC_enries = Const_MUX_Entries;
            MUXDAC_names_withFIOS = new string[Const_MUX_Entries];
            MUXDAC_names_withFIOS[0] = "FIO2";
            MUXDAC_names_withFIOS[1] = "FIO3";
            MUXDAC_names_withFIOS[2] = "FIO4";
            MUXDAC_names_withFIOS[3] = "FIO5";
            MUXDAC_names_withFIOS[4] = "DAC0";
            MUXDAC_values_WithFIOS = new double[Const_MUX_Entries];
            MUXDAC_values_WithFIOS[0] = 0;
            MUXDAC_values_WithFIOS[1] = 0;
            MUXDAC_values_WithFIOS[2] = 0;
            MUXDAC_values_WithFIOS[3] = 0;
            MUXDAC_values_WithFIOS[4] = 0;
            #endregion


            #region FIOs init
            _num_EIOForXferDKCLU = ConstEIOs_forXferDkCLu;
            EIOs_names_forXferDKCLU = new string[ConstEIOs_forXferDkCLu];
            EIOs_names_forXferDKCLU[0] = "EIO2";
            EIOs_names_forXferDKCLU[1] = "EIO3";
            EIOs_names_forXferDKCLU[2] = "EIO4";
            EIOs_names_forXferDKCLU[3] = "EIO5";
            EIOs_names_forXferDKCLU[4] = "EIO6";
            EIOs_names_forXferDKCLU[5] = "EIO7";
            EIOs_values_forXferDKCLU = new double[ConstEIOs_forXferDkCLu];
            EIOs_values_forXferDKCLU[0] = 0;
            EIOs_values_forXferDKCLU[1] = 0;
            EIOs_values_forXferDKCLU[2] = 0;
            EIOs_values_forXferDKCLU[3] = 0;
            EIOs_values_forXferDKCLU[4] = 0;
            EIOs_values_forXferDKCLU[5] = 0;
            #endregion

            #region AINs init
            _num_AINs = ConstAINs;
            AINs_names = new string[ConstAINs];
            AINs_names[0] = "AIN13"; //alarm 
            AINs_names[1] = "AIN1";
            AINs_names[2] = "EIO0"; //led1
            AINs_names[3] = "EIO1"; //led2

            AINs_values = new double[ConstAINs];
            AINs_values[0] = 0;
            AINs_values[1] = 0;
            AINs_values[2] = 0;
            AINs_values[3] = 0;

            #endregion

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

            //DIO_INHIBIT, DIO_DIRECTION, DIO_STATE
            // DIO_INHIBIT =   111111000011 0x3FC
            // DIO_DIRECTION = 111111111100 0x3FF
            // DIO_STATE =     111111111100 0x3FF
            string[] Configstrings = new string[3] { " DIO_INHIBIT", "DIO_DIRECTION", "DIO_STATE" };
            //double[] stringvaluesValues = new double[3] { 0, 0, 0 };
            //LJM.eWriteName(handle, "DIO_INHIBIT", 0x3FC);
            //LJM.eWriteName(handle, "DIO_DIRECTION", 0x3FF);
            //double[] statesFIOS = new double[4] { 0, 0, 0, 0 };
            //LJM.eReadNames(handle, 4, new string[4] { "FIO2", "FIO3", "FIO4", "FIO5" }, statesFIOS, ref errorAddress);

            double tempstes = 0;
            //LJM.eReadName(handle, "FIO2", ref tempstes);
            //LJM.eReadName(handle, "FIO3", ref tempstes);
            //LJM.eReadName(handle, "FIO4", ref tempstes);
            //LJM.eReadName(handle, "FIO5", ref tempstes);
            //LJM.eWriteNames(handle, 3, Configstrings, new double[3] { 0x3FC, 0x3FF, 0x3FF }, ref errorAddress);
        }

        //*****************************************************************************************************************tWriting MuxDAC values to the LabJack will be read later by the MBIV and found in the MessageReceived 
        public void WRITEDATA_MUXDAC(int argCHannelNumber, double argDACValue)
        {
            if(argDACValue<0)
            {
                argDACValue = 0;
            }
            if (argDACValue > 5)
            {
                argDACValue = 5;
            }
            switch (argCHannelNumber)
            {
                case 1:

                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
                case 2:

                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
                case 3:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
                case 4:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;

                case 5:
                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;

                case 6:
                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;
                case 7:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;
                case 8:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;
                case 9:
                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 10:
                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 11:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 12:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 13:

                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;

                case 14:
                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;
                case 15:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;
                case 16:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;
                default:
                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
            }

            _RAW_DACTOSEND = argDACValue;

            MUXDAC_values_WithFIOS[0] = muxbit0 ? 1 : 0;
            MUXDAC_values_WithFIOS[1] = muxbit1 ? 1 : 0;
            MUXDAC_values_WithFIOS[2] = muxbit2 ? 1 : 0;
            MUXDAC_values_WithFIOS[3] = muxbit3 ? 1 : 0;
            MUXDAC_values_WithFIOS[4] = _RAW_DACTOSEND;

            if (!isOnBus )
            {
                return;
            }

            int errorAddress1 = 0;
            LJM.eWriteNames(handle, Num_MUXDAC_enries, MUXDAC_names_withFIOS, MUXDAC_values_WithFIOS, ref errorAddress1);

        }

        public void WRITEDATA_CHAN_LVL(int argCHannelNumber, double arg_lvl, bool argIsNOtFloat)
        {
            if (arg_lvl < 0)
            {
                arg_lvl = 0;
            }
            if (arg_lvl > 5)
            {
                arg_lvl = 5;
            }
            

            switch (argCHannelNumber)
            {
                case 1:

                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
                case 2:

                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
                case 3:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
                case 4:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;

                case 5:
                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;

                case 6:
                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;
                case 7:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;
                case 8:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = true;
                    break;
                case 9:
                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 10:
                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 11:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 12:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = true;
                    muxbit3 = false;
                    break;
                case 13:

                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;

                case 14:
                    muxbit0 = false;
                    muxbit1 = true;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;
                case 15:
                    muxbit0 = false;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;
                case 16:
                    muxbit0 = true;
                    muxbit1 = false;
                    muxbit2 = false;
                    muxbit3 = false;
                    break;
                default:
                    muxbit0 = true;
                    muxbit1 = true;
                    muxbit2 = true;
                    muxbit3 = true;
                    break;
            }

          switch(arg_lvl)
            {
                case 0:
                    _RAW_DACTOSEND = 0.0;
                    break;
                case 1:
                    _RAW_DACTOSEND = 2.5;
                    break;
                case 2:
                    _RAW_DACTOSEND = 5.0;
                    break;
                default:
                    _RAW_DACTOSEND = 0.0;
                    break;
            }

            MUXDAC_values_WithFIOS[0] = muxbit0 ? 1 : 0;
            MUXDAC_values_WithFIOS[1] = muxbit1 ? 1 : 0;
            MUXDAC_values_WithFIOS[2] = muxbit2 ? 1 : 0;
            MUXDAC_values_WithFIOS[3] = muxbit3 ? 1 : 0;
            MUXDAC_values_WithFIOS[4] = _RAW_DACTOSEND;

            if (!isOnBus)
            {
                return;
            }

            int errorAddress1 = 0;
            LJM.eWriteNames(handle, Num_MUXDAC_enries, MUXDAC_names_withFIOS, MUXDAC_values_WithFIOS, ref errorAddress1);

        }
        //*****************************************************************************************************************tWrting FIO values to the LabJack will be read later by the MBIV and found in the MessageReceived
        public void WRITEDATA_EIOsXferDKclu(bool argXfer1, bool argXfer2, bool argDK1, bool argDk2, bool argClu1, bool argClu2)
        {
            xfer1Val = argXfer1 ? 1 : 0;
            xfer2Val = argXfer2 ? 1 : 0;
            dktr1Val = argDK1 ? 1 : 0;
            dktr2Val = argDk2 ? 1 : 0;
            clutch1Va = argClu1 ? 1 : 0;
            clutch2Val = argClu2 ? 1 : 0;

            EIOs_values_forXferDKCLU[0] = xfer1Val;
            EIOs_values_forXferDKCLU[1] = xfer2Val;
            EIOs_values_forXferDKCLU[2] = dktr1Val;
            EIOs_values_forXferDKCLU[3] = dktr2Val;
            EIOs_values_forXferDKCLU[4] = clutch1Va;
            EIOs_values_forXferDKCLU[5] = clutch2Val;
                

            
            if (!isOnBus)
            {
                return;
            }

            int errorAddress1 = 0;
            LJM.eWriteNames(handle, Num_EIOs_ForXferDKCLU, EIOs_names_forXferDKCLU, EIOs_values_forXferDKCLU, ref errorAddress1);

        }

        //*****************************************************************************************************************this will need to be read after the write to the MBIV
        public LABJAK_RX READDATA____JACK()
        {
            if (!isOnBus)
            {
                return _myLABJK_RX;
            }

            int errorAddress1 = 0;
            LJM.eReadNames(handle, Num_AINs, AINs_names, AINs_values, ref errorAddress1);

            _myLABJK_RX.Set_all_DigitalsAtOnce(AINs_values[0], AINs_values[2], AINs_values[3]);
            return _myLABJK_RX;
        }

        public void Close_LABJACK()
        {
            LJM.CloseAll();
            isOnBus = false;
        }


        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (serialPort != null)
                    {
                        serialPort.Close();
                        serialPort.Dispose();
                    }
                    if (commTimer != null)
                    {
                        StopTimer();
                        commTimer.Dispose();
                    }
                }
                disposed = true;
            }
        }

        #endregion
    }
}
