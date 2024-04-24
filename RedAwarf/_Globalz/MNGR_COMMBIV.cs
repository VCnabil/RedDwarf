using RedDwarf.RedAwarf._DataObjz.DataCOMM;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

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

        private Timer commTimer;
        public delegate void TimerElapsedHandler(bool argPortStatus);
        public event TimerElapsedHandler TimerElapsed;
        private MNGR_COMMBIV() {
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
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);

            commTimer = new Timer(100); // Set interval to 100 milliseconds
            commTimer.Elapsed += OnTimerElapsed;
            commTimer.AutoReset = true;
            commTimer.Enabled = true;
            StartTimer();
        }
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            
            if (serialPort == null) {
                TimerElapsed?.Invoke(false);
            }
            else if (serialPort.IsOpen)
            {
                TimerElapsed?.Invoke(true);
            }
            else
            {
                TimerElapsed?.Invoke(false);
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
        protected virtual void OnMessageReceived(MBIV_RX message)
        {
            MessageReceived?.Invoke(message);
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            incomingDataBuffer.Append(sp.ReadExisting());
            ProcessBuffer();
        }
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
                        //     EventsManagerLib.Call_LogConsole("4. Last complete message: " + mostRecentMessage + " has a valid checksum");
                        _myMBIV_RX.Update_INTarra_FromCommaDelimitedString(latestComplete_Validated_MessageBody);
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
        }
        public void ClosePort()
        {
           if (serialPort.IsOpen){
                serialPort.Close();
            }
        }

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

        public void WriteData(DATA_TX argDatatxObj)
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
    }
}
