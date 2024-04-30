using RedDwarf.RedAwarf._Globalz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._DataObjz.DataTestReport
{
    public class DATA_TESTREPORT
    {
        string _docID = "";
        string _project = "";
        string _assemblyPN = "";
        string _assemblySN = "";
        string _pcbRevisionPN = "";
        string _pcbSN = "";
        string _xMegaSWVersion = "";
        string _mbiv_SW_Version = "";
        string _labjackFirmwareVersion = "";
        string _labjackSerialNumber = "";
        string _mbiv_RedDwarf_GUI_Version = "";
        string _multimeter1Model = "";
        string _multimeter1SN = "";
        string _multimeter2Model = "";
        string _multimeter2SN = "";
        string _condition = ""; //New, Field Return
        string _diagnosis = ""; //Fit for Installation, Can be Repaired, Can NOT be Repaired
        string _firstTestersName = "";
        string _firstTestersSignature = "";
        string _firstTestersDate = "";
        string _secondTestersName = "";
        string _secondTestersSignature = "";
        string _secondTestersDate = "";
        string _revisionNotes = ""; //Rev., Date, Change, Author, Approved
        #region frontpage
        public string DocID
        {
            get { return _docID; }
            set { _docID = value; }
        }
        public string Project
        {
            get { return _project; }
            set { _project = value; }
        }
        public string AssemblyPN
        {
            get { return _assemblyPN; }
            set { _assemblyPN = value; }
        }
        public string AssemblySN
        {
            get { return _assemblySN; }
            set { _assemblySN = value; }
        }
        public string PCBRevisionPN
        {
            get { return _pcbRevisionPN; }
            set { _pcbRevisionPN = value; }
        }
        public string PCBSN
        {
            get { return _pcbSN; }
            set { _pcbSN = value; }
        }
        public string LabjackFirmwareVersion
        {
            get { return _labjackFirmwareVersion; }
            set { _labjackFirmwareVersion = value; }
        }
        public string LabjackSerialNumber
        {
            get { return _labjackSerialNumber; }
            set { _labjackSerialNumber = value; }
        }
        public string XMegaSWVersion
        {
            get { return _xMegaSWVersion; }
            set { _xMegaSWVersion = value; }
        }
        public string MBIV_SW_Version
        {
            get { return _mbiv_SW_Version; }
            set { _mbiv_SW_Version = value; }
        }
        public string MBIV_RedDwarf_GUI_Version
        {
            get { return _mbiv_RedDwarf_GUI_Version; }
            set { _mbiv_RedDwarf_GUI_Version = value; }
        }
        public string Multimeter1Model
        {
            get { return _multimeter1Model; }
            set { _multimeter1Model = value; }
        }
        public string Multimeter1SN
        {
            get { return _multimeter1SN; }
            set { _multimeter1SN = value; }
        }
        public string Multimeter2Model
        {
            get { return _multimeter2Model; }
            set { _multimeter2Model = value; }
        }
        public string Multimeter2SN
        {
            get { return _multimeter2SN; }
            set { _multimeter2SN = value; }
        }
        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        public string Diagnosis
        {
            get { return _diagnosis; }
            set { _diagnosis = value; }
        }
        public string FirstTestersName
        {
            get { return _firstTestersName; }
            set { _firstTestersName = value; }
        }
        public string FirstTestersSignature
        {
            get { return _firstTestersSignature; }
            set { _firstTestersSignature = value; }
        }
        public string FirstTestersDate
        {
            get { return _firstTestersDate; }
            set { _firstTestersDate = value; }
        }
        public string SecondTestersName
        {
            get { return _secondTestersName; }
            set { _secondTestersName = value; }
        }
        public string SecondTestersSignature
        {
            get { return _secondTestersSignature; }
            set { _secondTestersSignature = value; }
        }
        public string SecondTestersDate
        {
            get { return _secondTestersDate; }
            set { _secondTestersDate = value; }
        }
        public string RevisionNotes
        {
            get { return _revisionNotes; }
            set { _revisionNotes = value; }
        }
        #endregion

        DATA_CELL_MEASURES[,] _cellMeasures2D_AIN3 ;

        bool _alarm_passed;
        public bool Alarm_Passed
        {
            get { return _alarm_passed; }
            set { _alarm_passed = value; }
        }

        bool _Led1_passed;
        public bool Led1_Passed
        {
            get { return _Led1_passed; }
            set { _Led1_passed = value; }
        }

        bool _Led2_passed;
        public bool Led2_Passed
        {
            get { return _Led2_passed; }
            set { _Led2_passed = value; }
        }

        bool _xfer1_passed;
        public bool Xfer1_Passed
        {
            get { return _xfer1_passed; }
            set { _xfer1_passed = value; }
        }
        
        bool _xfer2_passed;
        public bool Xfer2_Passed
        {
            get { return _xfer2_passed; }
            set { _xfer2_passed = value; }
        }

        bool _dktr1_passed;
        public bool Dktr1_Passed
        {
            get { return _dktr1_passed; }
            set { _dktr1_passed = value; }
        }
        bool _dktr2_passed;
        public bool Dktr2_Passed
        {
            get { return _dktr2_passed; }
            set { _dktr2_passed = value; }
        }

        bool _clu1_passed;
        public bool Clu1_Passed
        {
            get { return _clu1_passed; }
            set { _clu1_passed = value; }
        }
        bool _clu2_passed;
        public bool Clu2_Passed
        {
            get { return _clu2_passed; }
            set { _clu2_passed = value; }
        }


        public DATA_CELL_MEASURES[,] CellMeasures2D_AIN3
        {
            get { return _cellMeasures2D_AIN3; }
            set { _cellMeasures2D_AIN3 = value; }
        }
        public DATA_TESTREPORT()
        {
            _docID = "";
            _project = "";
            _assemblyPN = "";
            _assemblySN = "";
            _pcbRevisionPN = "";
            _pcbSN = "";
            _xMegaSWVersion = "";
            _mbiv_SW_Version = "";
            _labjackFirmwareVersion = "";
            _labjackSerialNumber = "";
            _mbiv_RedDwarf_GUI_Version = "";
            _multimeter1Model = "";
            _multimeter1SN = "";
            _multimeter2Model = "";
            _multimeter2SN = "";
            _condition = "";
            _diagnosis = "";
            _firstTestersName = "";
            _firstTestersSignature = "";
            _firstTestersDate = "";
            _secondTestersName = "";
            _secondTestersSignature = "";
            _secondTestersDate = "";
            _revisionNotes = "";

            _cellMeasures2D_AIN3 = new DATA_CELL_MEASURES[17, 4];
            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _cellMeasures2D_AIN3[i, j] = new DATA_CELL_MEASURES(Helpers.GetExpectedMinValue(j), Helpers.GetExpectedMaxValue(j), Helpers.Expected_average()[j]);
                }
            }


            _alarm_passed = false;
            _Led1_passed = false;
            _Led2_passed = false;
            _xfer1_passed = false;
            _xfer2_passed = false;
            _dktr1_passed = false;
            _dktr2_passed = false;
            _clu1_passed = false;
            _clu2_passed = false;
                

        }

    }
}
