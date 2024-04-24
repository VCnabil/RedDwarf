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
        string _mbivControlUnitKiyoonSWVersion = "";
        string _labjackFirmwareVersion = "";
        string _labjackSerialNumber = "";
        string _mbivKiyoonTestGUISWVersion = "";
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
        public string LabjackFirmwareRevision
        {
            get { return _labjackSerialNumber; }
            set { _labjackSerialNumber = value; }
        }
        public string XMegaSWVersion
        {
            get { return _xMegaSWVersion; }
            set { _xMegaSWVersion = value; }
        }
        public string MBIVControlUnitKiyoonSWVersion
        {
            get { return _mbivControlUnitKiyoonSWVersion; }
            set { _mbivControlUnitKiyoonSWVersion = value; }
        }
        public string MBIVKiyoonTestGUISWVersion
        {
            get { return _mbivKiyoonTestGUISWVersion; }
            set { _mbivKiyoonTestGUISWVersion = value; }
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


        public DATA_TESTREPORT()
        {
            _docID = "";
            _project = "";
            _assemblyPN = "";
            _assemblySN = "";
            _pcbRevisionPN = "";
            _pcbSN = "";
            _xMegaSWVersion = "";
            _mbivControlUnitKiyoonSWVersion = "";
            _labjackFirmwareVersion = "";
            _labjackSerialNumber = "";
            _mbivKiyoonTestGUISWVersion = "";
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
        }
    }
}
