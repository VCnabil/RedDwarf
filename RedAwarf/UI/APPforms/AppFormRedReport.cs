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
    public partial class AppFormRedReport : Form
    {
        DATA_TESTREPORT report;
        public AppFormRedReport(DATA_TESTREPORT arg_tp)
        {
            report = arg_tp;
            InitializeComponent();

            tb_labjackFirmwareVersion.Text = report.LabjackFirmwareVersion;
            tb_labjackSerialNumber.Text = report.LabjackSerialNumber;

            btn_Print.Hide();
            group_RESULTS_AIN3.Hide();
            group_RESULTS_DO4.Hide();
            group_RESULTS_DI7.Hide();

            btn_ToTestForm.Click += OpenTestForm;
        }

        private void OpenTestForm(object sender, EventArgs e)
        {
            AppFormALLinone testForm = new AppFormALLinone(report);
            testForm.FormClosed += new FormClosedEventHandler(TestForm_FormClosed);
            this.Hide();
            testForm.Show();
        }
        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            AppFormALLinone testForm = sender as AppFormALLinone;
            if (testForm != null && testForm.TPReport != null)
            {
                tb__mbivRedDwarf_GUI_Version.Text = testForm.TPReport.MBIV_RedDwarf_GUI_Version; // Update your label
            }
            this.Show();
        }
    }
}
