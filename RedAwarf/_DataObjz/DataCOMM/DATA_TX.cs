using RedDwarf.RedAwarf._Globalz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._DataObjz.DataCOMM
{
    public class DATA_TX
    {

        int _dio; //digtal io

        int _pb; //port bucker
        int _pn; //port nozzle
        int _pi; //port interceptor
        int _sb; //starboard bucker
        int _sn; //starboard nozzle
        int _si; //starboard interceptor

        int _pe; //port engine
        int _se; //starboard engine

        int _sa; //safety

        //make syre the value is smaller than 8 when setting DIO  and greater than 0


        public int DIO_0
        {
            get { return _dio; }
            private set
            {
                if (value < 0)
                {
                    _dio = 0;
                }
                else if (value > 8)
                {
                    _dio = 8;
                }
                else
                {
                    _dio = value;
                }
            }
        }

        public void SetDIO(bool arg_bit0, bool arg_bit1, bool arg_bit2, bool argsafe)
        {
            _dio = 0;
            if (arg_bit0)
            {
                _dio += 1;
            }
            if (arg_bit1)
            {
                _dio += 2;
            }
            if (arg_bit2)
            {
                _dio += 4;
            }

            _sa = argsafe ? 1 : 0;
        }
        public int PB_1
        {
            get { return _pb; }
            set
            {
                if (value < 0)
                {
                    _pb = 0;
                }
                else if (value > 200)
                {
                    _pb = 200;
                }
                else
                {
                    _pb = value;
                }
            }
        }
        public int PN_2
        {
            get { return _pn; }
            set
            {
                if (value < 0)
                {
                    _pn = 0;
                }
                else if (value > 200)
                {
                    _pn = 200;
                }
                else
                {
                    _pn = value;
                }
            }
        }
        public int PI_3
        {
            get { return _pi; }
            set
            {
                if (value < 0)
                {
                    _pi = 0;
                }
                else if (value > 200)
                {
                    _pi = 200;
                }
                else
                {
                    _pi = value;
                }
            }
        }
        public int SB_4
        {
            get { return _sb; }
            set
            {
                if (value < 0)
                {
                    _sb = 0;
                }
                else if (value > 200)
                {
                    _sb = 200;
                }
                else
                {
                    _sb = value;
                }
            }
        }
        public int SN_5
        {
            get { return _sn; }
            set
            {
                if (value < 0)
                {
                    _sn = 0;
                }
                else if (value > 200)
                {
                    _sn = 200;
                }
                else
                {
                    _sn = value;
                }
            }
        }
        public int SI_6
        {
            get { return _si; }
            set
            {
                if (value < 0)
                {
                    _si = 0;
                }
                else if (value > 200)
                {
                    _si = 200;
                }
                else
                {
                    _si = value;
                }
            }
        }
        public int PE_7
        {
            get { return _pe; }
            set
            {
                if (value < 0)
                {
                    _pe = 0;
                }
                else if (value > 2000)
                {
                    _pe = 2000;
                }
                else
                {
                    _pe = value;
                }
            }
        }
        public int SE_8
        {
            get { return _se; }
            set
            {
                if (value < 0)
                {
                    _se = 0;
                }
                else if (value > 2000)
                {
                    _se = 2000;
                }
                else
                {
                    _se = value;
                }
            }
        }
        public int SA_9
        {
            get { return _sa; }
            private set
            {
                if (value < 0)
                {
                    _sa = 0;
                }
                else if (value > 1)
                {
                    _sa = 1;
                }
                else
                {
                    _sa = value;
                }
            }
        }

        public DATA_TX()
        {

            _dio = 0;
            _pb = 100;
            _pn = 100;
            _pi = 100;
            _sb = 100;
            _sn = 100;
            _si = 100;
            _pe = 2000;
            _se = 2000;
            _sa = 1;

        }
        ~DATA_TX() { }

        public string CREATE_FullString_for_TX()
        {
            string formattedStringBODY = Helpers_FormatData(_dio, _pb, _pn, _pi, _sb, _sn, _si, _pe, _se, _sa == 1);
            return formattedStringBODY;
        }
        string Helpers_FormatData(int argDio, int argPb, int argPN, int argPI, int argSB, int argSN, int argSI, int argP, int argS, bool argb)
        {
            // Ensure values do not exceed their maximum allowed values
            argPb = Math.Min(argPb, 255);
            argPN = Math.Min(argPN, 255);
            argPI = Math.Min(argPI, 255);
            argSB = Math.Min(argSB, 255);
            argSN = Math.Min(argSN, 255);
            argSI = Math.Min(argSI, 255);

            argP = Math.Min(argP, 2000);
            argS = Math.Min(argS, 2000);

            // Convert bool to int for string formatting (assuming 1 for true and 0 for false)
            int boolAsInt = argb ? 1 : 0;

            // Create the formatted string
            string formattedStringBODY = $"$AAAA,{argDio},{argPb},{argPN},{argPI},{argSB},{argSN},{argSI},{argP},{argS},{boolAsInt}";

            string checksum = Helpers_Generate_Checksum_fromBody(formattedStringBODY);
            string formattedString = $"{formattedStringBODY}*{checksum}";
            return formattedString;
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
    }

}
