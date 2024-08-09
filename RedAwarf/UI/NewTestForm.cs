using RedDwarf.RedAwarf._Actionz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedDwarf.RedAwarf.UI
{
    public partial class NewTestForm : Form
    {
      //  private ClassActionz actionManager = new ClassActionz();
        public NewTestForm()
        {
            InitializeComponent();
        }

        private async void btnStartTest_Click(object sender, EventArgs e)
        {
            //TESTTest test = new TESTTest();
            //test.AddAction(new TESTAction { ValueToWrite = "Start", WaitTimeBeforeRead = 500, ReadDuration = 5000 });
            //test.AddAction(new TESTAction { ValueToWrite = "Check", WaitTimeBeforeRead = 1000, ReadDuration = 3000 });

            //await actionManager.RunTestAsync(test);
        }
    }
}
