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
            // 5.02 is on 7.5 is off
            _AlarmStateON = argAlarm < 6 ? true:false;
            _LED1StaeOn = argLED1 > 0 ? false : true;
            _LED2StaeOn = argLED2 > 0 ? false : true;
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
