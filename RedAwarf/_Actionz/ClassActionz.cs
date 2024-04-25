using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._Actionz
{
    public class ClassActionz
    {
        public async Task PerformTestActionAsync(TESTAction arg_Testaction)
        {
            WriteToDevice(arg_Testaction.ValueToWrite);
            await Task.Delay(arg_Testaction.WaitTimeBeforeRead);

            var startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < arg_Testaction.ReadDuration)
            {
                double readValue = ReadFromDevice();
                arg_Testaction.ReadValues.Add(readValue);
                await Task.Delay(100); // Adjust based on expected frequency of reading values
            }

            arg_Testaction.Result = EvaluateAction(arg_Testaction);
        }

        private void WriteToDevice(string value)
        {
            // Example: Log writing to device for debugging
            Console.WriteLine($"Writing '{value}' to device");
            // Add actual device communication logic here
        }

        private double ReadFromDevice()
        {
            // Simulate reading from device
            return new Random().NextDouble() * 100; // Random value between 0 and 100
        }

        private bool EvaluateAction(TESTAction srg_TESTAction)
        {
            // Example evaluation criteria
            var (min, max) = srg_TESTAction.MinMax;
            srg_TESTAction.Result = srg_TESTAction.AverageValue >= 10 && srg_TESTAction.AverageValue <= 20 && min >= 0 && max <= 100;
            return srg_TESTAction.Result;
        }

        public async Task RunTestAsync(TESTTest argTESTtest)
        {
            foreach (var testaction in argTESTtest.Actions)
            {
                await PerformTestActionAsync(testaction);
            }

            bool isTestPassed = argTESTtest.IsTestPassed;
            Console.WriteLine(isTestPassed ? "Test passed." : "Test failed.");
        }
    }

    public class TESTAction
    {
        public string ValueToWrite { get; set; }
        public int WaitTimeBeforeRead { get; set; }
        public int ReadDuration { get; set; }
        public List<double> ReadValues { get; set; } = new List<double>();
        public double AverageValue => ReadValues.Count > 0 ? ReadValues.Average() : 0;
        public (double Min, double Max) MinMax => (ReadValues.Min(), ReadValues.Max());
        public bool Result { get; set; }
    }

    public class TESTTest
    {
        public List<TESTAction> Actions { get; set; }
        public bool IsTestPassed => Actions.All(a => a.Result);

        public TESTTest()
        {
            Actions = new List<TESTAction>();
        }

        public void AddAction(TESTAction action)
        {
            Actions.Add(action);
        }
    }

}
