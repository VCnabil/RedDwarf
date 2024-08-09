using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._Globalz
{
    public class Helpers
    {



        public enum TESTsteps { 
            APPstarted = 0,
            TP_inited = 1,
            TESTFORM_0_opened = 2,
            TESTFORM_1_look = 3,
            TESTFORM_2_verify_3AIN = 4,
            TESTFORM_3_verify_4DO = 5,
            TESTFORM_4_verify_7DI = 6,
            TESTFORM_5_ISTESTING = 7,
            TESTFORM_6_TESTED_COMPLETE = 8,
        }

        public enum DACsetting
        {
            LOW = 0,
            MEDIUM = 1,
            HIGH = 2,
            FLOATING = 3
        }

        public enum AINtestSTATE
        {
            SHOULDNOTREAD = 0,
            MEASURING_BULK = 1,
            MEASURING_Single = 2,
            MEASURING_FULLARRAY= 3,

        }






        //static double[] Expected_LOW_MED_HIGH_FLOAT_MinValues = new double[] { 0, 2010, 4050 , 4050 };
        //static double[] Expected_LOW_MED_HIGH_FLOAT_MaxValues = new double[] { 50, 2090, 4095 , 4095 };
        static double[] Expected_LOW_MED_HIGH_FLOAT_MinValues = new double[] { 0, 2000, 4010, 4010 };
        static double[] Expected_LOW_MED_HIGH_FLOAT_MaxValues = new double[] { 80, 2200, 4095, 4095 };
        public static double[] Expected_average()
        {
            //   return new double[] { 0, 2100, 4050, 4050 };
            double[] temp = new double[4];
            temp[0] = (Expected_LOW_MED_HIGH_FLOAT_MinValues[0] + Expected_LOW_MED_HIGH_FLOAT_MaxValues[0]) / 2;
            temp[1] = (Expected_LOW_MED_HIGH_FLOAT_MinValues[1] + Expected_LOW_MED_HIGH_FLOAT_MaxValues[1]) / 2;
            temp[2] = (Expected_LOW_MED_HIGH_FLOAT_MinValues[2] + Expected_LOW_MED_HIGH_FLOAT_MaxValues[2]) / 2;
            temp[3] = (Expected_LOW_MED_HIGH_FLOAT_MinValues[3] + Expected_LOW_MED_HIGH_FLOAT_MaxValues[3]) / 2;

         return temp;
        }
        public static double GetExpectedMinValue(int arg_LVL) { 

            if(arg_LVL < 0)
            {
                arg_LVL = 0;
            }
            if(arg_LVL > 3)
            {
                arg_LVL = 3;
            }
            return Expected_LOW_MED_HIGH_FLOAT_MinValues[arg_LVL];
        }

        public static double GetExpectedMaxValue(int arg_LVL)
        {

            if (arg_LVL < 0)
            {
                arg_LVL = 0;
            }
            if (arg_LVL > 3)
            {
                arg_LVL = 3;
            }
            return Expected_LOW_MED_HIGH_FLOAT_MaxValues[arg_LVL];
        }

        static int _maxains = 16;
        static int _maxlvls = 2;
        public static int Get_MAX_AINs()
        {
            return _maxains;
        }
        public static int Get_MAX_LVLS()
        {
            return _maxlvls;
        }

        public static int intervalTicks = 240;
        public static int WaitToTakeEffect = 310;
        public static int WAIT_DIO_EFFECT = 4000;
        public static int targetSamples = 5;

        static bool _doMaual = true;
        public static bool DOManual { 
            get => _doMaual;
            private set => _doMaual = value;
        }

    }
}
