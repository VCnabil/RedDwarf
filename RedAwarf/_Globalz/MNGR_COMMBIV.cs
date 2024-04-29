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
        public string[] MUXDAC_names;
        public double[] MUXDAC_values;
        #endregion
        bool muxbit0 = true;
        bool muxbit1 = true;
        bool muxbit2 = true;
        bool muxbit3 = true;
        double _RAW_DACTOSEND = 0.0;
        #region FIO for digital inputs xfer dk clutch
        const int ConstFIOs = 6;
        int _num_FIOs = 6;
        public int Num_FIOs
        {
            get { return _num_FIOs; }
            private set { _num_FIOs = value; }
        }
        public string[] FIOs_names;
        public double[] FIOs_values;
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
                            INIT_CON_LABJACK();
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
            MUXDAC_names = new string[Const_MUX_Entries];
            MUXDAC_names[0] = "EIO2";
            MUXDAC_names[1] = "EIO3";
            MUXDAC_names[2] = "EIO4";
            MUXDAC_names[3] = "EIO5";
            MUXDAC_names[4] = "DAC0";
            MUXDAC_values = new double[Const_MUX_Entries];
            MUXDAC_values[0] = 0;
            MUXDAC_values[1] = 0;
            MUXDAC_values[2] = 0;
            MUXDAC_values[3] = 0;
            MUXDAC_values[4] = 0;
            #endregion


            #region FIOs init
            _num_FIOs = ConstFIOs;
            FIOs_names = new string[ConstFIOs];
            FIOs_names[0] = "FIO0";
            FIOs_names[1] = "FIO1";
            FIOs_names[2] = "FIO2";
            FIOs_names[3] = "FIO3";
            FIOs_names[4] = "FIO4";
            FIOs_names[5] = "FIO5";
            FIOs_values = new double[ConstFIOs];
            FIOs_values[0] = 0;
            FIOs_values[1] = 0;
            FIOs_values[2] = 0;
            FIOs_values[3] = 0;
            FIOs_values[4] = 0;
            FIOs_values[5] = 0;
            #endregion

            #region AINs init
            _num_AINs = ConstAINs;
            AINs_names = new string[ConstAINs];
            AINs_names[0] = "AIN0"; //alarm 
            AINs_names[1] = "AIN1";
            AINs_names[2] = "EIO0"; //led1
            AINs_names[3] = "EIO1"; //led2

            AINs_values = new double[ConstAINs];
            AINs_values[0] = 0;
            AINs_values[1] = 0;
            AINs_values[2] = 0;
            AINs_values[3] = 0;

            #endregion
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

            MUXDAC_values[0] = muxbit0 ? 0 : 1;
            MUXDAC_values[1] = muxbit1 ? 0 : 1;
            MUXDAC_values[2] = muxbit2 ? 0 : 1;
            MUXDAC_values[3] = muxbit3 ? 0 : 1;
            MUXDAC_values[4] = _RAW_DACTOSEND;

            if (!isOnBus )
            {
                return;
            }

            int errorAddress1 = 0;
            LJM.eWriteNames(handle, Num_MUXDAC_enries, MUXDAC_names, MUXDAC_values, ref errorAddress1);

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

            MUXDAC_values[0] = muxbit0 ? 0 : 1;
            MUXDAC_values[1] = muxbit1 ? 0 : 1;
            MUXDAC_values[2] = muxbit2 ? 0 : 1;
            MUXDAC_values[3] = muxbit3 ? 0 : 1;
            MUXDAC_values[4] = _RAW_DACTOSEND;

            if (!isOnBus)
            {
                return;
            }

            int errorAddress1 = 0;
            LJM.eWriteNames(handle, Num_MUXDAC_enries, MUXDAC_names, MUXDAC_values, ref errorAddress1);

        }
        //*****************************************************************************************************************tWrting FIO values to the LabJack will be read later by the MBIV and found in the MessageReceived
        public void WRITEDATA_FIO(bool argXfer1, bool argXfer2, bool argDK1, bool argDk2, bool argClu1, bool argClu2)
        {
            xfer1Val = argXfer1 ? 1 : 0;
            xfer2Val = argXfer2 ? 1 : 0;
            dktr1Val = argDK1 ? 1 : 0;
            dktr2Val = argDk2 ? 1 : 0;
            clutch1Va = argClu1 ? 1 : 0;
            clutch2Val = argClu2 ? 1 : 0;

            FIOs_values[0] = xfer1Val;
            FIOs_values[1] = xfer2Val;
            FIOs_values[2] = dktr1Val;
            FIOs_values[3] = dktr2Val;
            FIOs_values[4] = clutch1Va;
            FIOs_values[5] = clutch2Val;
                

            
            if (!isOnBus)
            {
                return;
            }

            int errorAddress1 = 0;
            LJM.eWriteNames(handle, Num_FIOs, FIOs_names, FIOs_values, ref errorAddress1);

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
