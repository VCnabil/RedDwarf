using RedDwarf.RedAwarf._DataObjz.DataTestReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedDwarf.RedAwarf.UI.APPforms
{
    public partial class AppFormALLinone : Form
    {
        public DATA_TESTREPORT TPReport;

        public AppFormALLinone(DATA_TESTREPORT arg_tp)
        {
            TPReport = arg_tp;
            InitializeComponent();
            TPReport.MBIV_RedDwarf_GUI_Version = "v1.0.0";
        }
    }
}
