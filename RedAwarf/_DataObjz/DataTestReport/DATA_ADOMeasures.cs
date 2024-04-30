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
        int _validationResult;

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
        double _expectedAverage;
        double _averagePAssThreshol=5;
        public DATA_CELL_MEASURES(double argExpectedMin, double argExpetedMAX, double expectedAverage)
        {
            int _smallestIntegerPossible = int.MinValue;
            int _largestIntegerPossible = int.MaxValue;
            _minValue = _largestIntegerPossible;
            _maxValue = _smallestIntegerPossible;
            _averageValue = -1;
            _samplesTaken = 0;
            _expectedMin = argExpectedMin;
            _expectedMax = argExpetedMAX;
            _validationResult = -1;
            _expectedAverage = expectedAverage;
        }

        // int result 0 = validation passed,
        // 1 = validation failed min value out of range
        // 2 = validation failed max value out of range
        // 3 = validation failed both min and max out of range
        // 4 = validation failed average out of range

        public int Validate()
        {

            //using _validationResult 

            if (_minValue < _expectedMin && _maxValue > _expectedMax)
            {
                _validationResult = 0;
            }
            else if (_minValue < _expectedMin)
            {
                _validationResult = 1;
            }
            else if (_maxValue > _expectedMax)
            {
                _validationResult = 2;
            }
            else if (_minValue < _expectedMin && _maxValue > _expectedMax)
            {
                _validationResult = 3;
            }
            else if (_averageValue < (_expectedAverage-_averagePAssThreshol) || _averageValue > (_expectedAverage +_averagePAssThreshol) )
            {
                _validationResult = 4;
            }
            else
            {
                _validationResult = 0;
            }
            return _validationResult;
        }

    }
}
