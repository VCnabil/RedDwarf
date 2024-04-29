using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._DataObjz.DataTestReport
{
    public class DATA_ADOMeasures
    {
        

  

        public DATA_ADOMeasures()
        {
 
        }
    }

    public class DATA_EXPECTED_MEASURES
    {



    

        public DATA_EXPECTED_MEASURES()
        {
             
        }

    }


    public class DATA_CELL_MEASURES {
        int _samplesTaken;
        double _minValue;
        double _maxValue;
        double _averageValue;

        public double MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        public double MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }
        public double AverageValue
        {
            get { return _averageValue; }
            set { _averageValue = value; }
        }

        public int SamplesTaken
        {
            get { return _samplesTaken; }
            set { _samplesTaken = value; }
        }

        double _expectedMin;
        double _expectedMax;
        public DATA_CELL_MEASURES(double argExpectedMin, double argExpetedMAX)
        {
            int _smallestIntegerPossible = int.MinValue;
            int _largestIntegerPossible = int.MaxValue;
            _minValue = _largestIntegerPossible;
            _maxValue = _smallestIntegerPossible;
            _averageValue = -1;
            _samplesTaken = 0;
            _expectedMin = argExpectedMin;
            _expectedMax = argExpetedMAX;
        }

    }
}
