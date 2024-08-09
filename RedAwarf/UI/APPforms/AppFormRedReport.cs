using RedDwarf.RedAwarf._DataObjz.DataCOMM;
using RedDwarf.RedAwarf._DataObjz.DataTestReport;
using RedDwarf.RedAwarf._Globalz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RedDwarf.RedAwarf._Globalz.Helpers;

namespace RedDwarf.RedAwarf.UI.APPforms
{
    public partial class AppFormRedReport : Form
    {
        DATA_TESTREPORT report;
        Label[,] _labels2D_minmax;
        Label[] floatingColumn;

        public AppFormRedReport(DATA_TESTREPORT arg_tp)
        {
            report = arg_tp;
            InitializeComponent();

            tb_labjackFirmwareVersion.Text = report.LabjackFirmwareVersion;
            tb_labjackSerialNumber.Text = report.LabjackSerialNumber;
            tb_mbivControlUnitKiyoonSWVersion.Text = report.MBIV_SW_Version;
            string _today_Date_MM_DD_YYYY = DateTime.Now.ToString("MM/dd/yyyy");
            tb_firstTestersDate.Text = _today_Date_MM_DD_YYYY;
            tb_secondTestersDate.Text = _today_Date_MM_DD_YYYY;
            _labels2D_minmax = new Label[,]
              {
                { label_0_0, label_0_1, label_0_2 },
                { label_1_0, label_1_1, label_1_2 },
                { label_2_0, label_2_1, label_2_2 },
                { label_3_0, label_3_1, label_3_2 },
                { label_4_0, label_4_1, label_4_2 },
                { label_5_0, label_5_1, label_5_2 },
                { label_6_0, label_6_1, label_6_2 },
                { label_7_0, label_7_1, label_7_2 },
                { label_8_0, label_8_1, label_8_2 },
                { label_9_0, label_9_1, label_9_2 },
                { label_10_0, label_10_1, label_10_2 },
                { label_11_0, label_11_1, label_11_2 },
                { label_12_0, label_12_1, label_12_2 },
                { label_13_0, label_13_1, label_13_2 },
                { label_14_0, label_14_1, label_14_2 },
                { label_15_0, label_15_1, label_15_2 },
                { label_16_0, label_16_1, label_16_2 }
              };
            floatingColumn = new Label[] { label_0_3, label_1_3, label_2_3, label_3_3, label_4_3, label_5_3, label_6_3, label_7_3, label_8_3, label_9_3, label_10_3, label_11_3, label_12_3, label_13_3, label_14_3, label_15_3, label_16_3 };

            btn_Print.Hide();
            group_RESULTS_AIN3.Hide();
            group_RESULTS_DO4.Hide();
            group_RESULTS_DI7.Hide();

            btn_ToTestForm.Click += OpenTestForm;
            btn_ToTestForm3.Click += Btn_ToTestForm3_Click;
            btn_ToTempEnginOutForm.Click += Btn_ToTempEnginOutForm_Click;

        
        }

        private void Btn_ToTempEnginOutForm_Click(object sender, EventArgs e)
        {
            AppFormTempEngins appFormTempEngins = new AppFormTempEngins();
            appFormTempEngins.Show();
        }

        private void Btn_ToTestForm3_Click(object sender, EventArgs e)
        {
             Form3 form3 = new Form3();
            form3.Show();

        }

        private void OpenTestForm(object sender, EventArgs e)
        {
            report.DocID= tb_docID.Text;
            report.Project = tb_project.Text;
            report.AssemblyPN = tb_assemblyPN.Text;
            report.AssemblySN = tb_assemblySN.Text;
            report.PCBRevisionPN = tb_pcbRevisionPN.Text;
            report.PCBSN = tb_pcbSN.Text;
            report.XMegaSWVersion = tb_xMegaSWVersion.Text;
            report.MBIV_SW_Version = tb_mbivControlUnitKiyoonSWVersion.Text;
            report.MBIV_RedDwarf_GUI_Version = tb__mbivRedDwarf_GUI_Version.Text;
            report.Multimeter1Model ="na";
            report.Multimeter2Model = "na";
            report.FirstTestersName =tb_firstTestersName.Text;
            report.SecondTestersName = tb_secondTestersName.Text;
            report.FirstTestersSignature = tb_firstTestersSignature.Text;
            report.SecondTestersSignature = tb_secondTestersSignature.Text;


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


                for (int i = 1; i <= Get_MAX_AINs(); i++)
                {
                    for (int j = 0; j <= Get_MAX_LVLS(); j++)
                    {


                        int result = testForm.TPReport.CellMeasures2D_AIN3[i, j].Validate();
                        switch (result)
                        {
                            case 0:
                                _labels2D_minmax[i, j].BackColor = Color.LimeGreen;
                                _labels2D_minmax[i, j].Text = "PASSED";
                                break;
                            case 1:
                                _labels2D_minmax[i, j].BackColor = Color.Yellow;
                                _labels2D_minmax[i, j].Text = "FAILED min";
                                break;
                            case 2:
                                _labels2D_minmax[i, j].BackColor = Color.Orange;
                                _labels2D_minmax[i, j].Text = "FAILED max";
                                break;
                            case 3:
                                _labels2D_minmax[i, j].BackColor = Color.Red;
                                _labels2D_minmax[i, j].Text = "FAILED BOTH";
                                break;
                            case 4:
                                //_labels2D_minmax[i, j].BackColor = Color.Purple;
                                //_labels2D_minmax[i, j].Text = "FAILED AVG";
                                _labels2D_minmax[i, j].BackColor = Color.LimeGreen;
                                _labels2D_minmax[i, j].Text = "PASSED";
                                break;
                            default:
                                _labels2D_minmax[i, j].BackColor = Color.Red;
                                _labels2D_minmax[i, j].Text = "FAILED";
                                break;
                        }
                    }
                }

                for (int i = 1; i <= Get_MAX_AINs(); i++)
                {
             
                    int result = testForm.TPReport.CellMeasuresFLoats[i].Validate();

                    switch (result)
                    {
                        case 0:
                            floatingColumn[i].BackColor = Color.LimeGreen;
                            floatingColumn[i].Text = "PASSED";
                            break;
                        case 1:
                            floatingColumn[i].BackColor = Color.Yellow;
                            floatingColumn[i].Text = "FAILED min";
                            break;
                        case 2:
                            floatingColumn[i].BackColor = Color.Orange;
                            floatingColumn[i].Text = "FAILED max";
                            break;
                        case 3:
                            floatingColumn[i].BackColor = Color.Red;
                            floatingColumn[i].Text = "FAILED BOTH";
                            break;
                        case 4:
                            //floatingColumn[i].BackColor = Color.Purple;
                            //floatingColumn[i].Text = "FAILED AVG";
                            floatingColumn[i].BackColor = Color.LimeGreen;
                            floatingColumn[i].Text = "PASSED";
                            break;
                        default:
                            floatingColumn[i].BackColor = Color.Red;
                            floatingColumn[i].Text = "FAILED";
                            break;
                    }
                }

                if (testForm.TPReport.Alarm_Passed)
                {
                    lbl_passfail_ALARM.BackColor = Color.LimeGreen;
                    lbl_passfail_ALARM.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_ALARM.BackColor = Color.Red;
                    lbl_passfail_ALARM.Text = "FAILED";
                }

                if (testForm.TPReport.Led1_Passed)
                {
                    lbl_passfail_LED1.BackColor = Color.LimeGreen;
                    lbl_passfail_LED1.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_LED1.BackColor = Color.Red;
                    lbl_passfail_LED1.Text = "FAILED";
                }

                if (testForm.TPReport.Led2_Passed)
                {
                    lbl_passfail_LED2.BackColor = Color.LimeGreen;
                    lbl_passfail_LED2.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_LED2.BackColor = Color.Red;
                    lbl_passfail_LED2.Text = "FAILED";
                }

                if (testForm.TPReport.Xfer1_Passed)
                {
                    lbl_passfail_xfer1.BackColor = Color.LimeGreen;
                    lbl_passfail_xfer1.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_xfer1.BackColor = Color.Red;
                    lbl_passfail_xfer1.Text = "FAILED";
                }

                if (testForm.TPReport.Xfer2_Passed)
                {
                    lbl_passfail_xfer2.BackColor = Color.LimeGreen;
                    lbl_passfail_xfer2.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_xfer2.BackColor = Color.Red;
                    lbl_passfail_xfer2.Text = "FAILED";
                }

                if (testForm.TPReport.Dktr1_Passed)
                {
                    lbl_passfail_dk1.BackColor = Color.LimeGreen;
                    lbl_passfail_dk1.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_dk1.BackColor = Color.Red;
                    lbl_passfail_dk1.Text = "FAILED";
                }

                if (testForm.TPReport.Dktr2_Passed)
                {
                    lbl_passfail_dk2.BackColor = Color.LimeGreen;
                    lbl_passfail_dk2.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_dk2.BackColor = Color.Red;
                    lbl_passfail_dk2.Text = "FAILED";
                }

                if (testForm.TPReport.Clu1_Passed)
                {
                    lbl_passfail_clu1.BackColor = Color.LimeGreen;
                    lbl_passfail_clu1.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_clu1.BackColor = Color.Red;
                    lbl_passfail_clu1.Text = "FAILED";
                }

                if (testForm.TPReport.Clu2_Passed)
                {
                    lbl_passfail_clu2.BackColor = Color.LimeGreen;
                    lbl_passfail_clu2.Text = "PASSED";
                }
                else
                {
                    lbl_passfail_clu2.BackColor = Color.Red;
                    lbl_passfail_clu2.Text = "FAILED";
                }


                tb_labjackFirmwareVersion.Text = report.LabjackFirmwareVersion;
                tb_labjackSerialNumber.Text = report.LabjackSerialNumber;
                tb_mbivControlUnitKiyoonSWVersion.Text = report.MBIV_SW_Version;

                group_RESULTS_AIN3.Show();
                group_RESULTS_DO4.Show();
                group_RESULTS_DI7.Show();


            }
            this.Show();
        }
    }
}
