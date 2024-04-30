using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._Globalz
{
    public class Helpers
    {

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
    }
}
