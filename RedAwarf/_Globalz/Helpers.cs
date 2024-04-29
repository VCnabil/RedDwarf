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
    }
}
