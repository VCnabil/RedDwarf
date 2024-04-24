using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._DataObjz.DataCOMM
{
    public class LABJAK_RX
    {
        bool _AlarmStateON = false;
        bool _LED1StaeOn = false;
        bool _LED2StaeOn = false;

        public void Set_all_DigitalsAtOnce(double argAlarm, double argLED1, double argLED2)
        {
            _AlarmStateON = argAlarm > 0 ? true:false;
            _LED1StaeOn = argLED1 > 0 ? true : false;
            _LED2StaeOn = argLED2 > 0 ? true : false;
        }
    
        public bool AlarmStateON
        {
            get { return _AlarmStateON; }
        }
        public bool LED1StaeOn
        {
            get { return _LED1StaeOn; }
        }
        public bool LED2StaeOn
        {
            get { return _LED2StaeOn; }
        }
        public LABJAK_RX() {
           
        }
    }
}
