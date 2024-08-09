using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.RedAwarf._Actionz
{
    public class ClassActionz
    {
        public delegate void WriteAction(string value);
        public delegate bool ReadAction(string parameter);

        public async Task<bool> PerformTestActionAsync(TESTAction action, WriteAction write, ReadAction read)
        {
            write(action.ValueToWrite);
            await Task.Delay(action.WaitTimeBeforeRead);
            bool readValue = read(action.DeviceName);
            return readValue == action.ExpectedState;
        }

        public async Task RunTestAsync(TESTTest test, WriteAction write, ReadAction read)
        {
            foreach (var action in test.TESTActions)
            {
                bool result = await PerformTestActionAsync(action, write, read);
                action.Result = result;
            }
            bool isTestPassed = test.IsTestPassed;
            Console.WriteLine(isTestPassed ? "Test passed." : "Test failed.");
        }

        public async Task RunLabJackTests()
        {
            double[] dacValues = { 0.0, 2.5, 5.0 };
            int[] channels = Enumerable.Range(1, 17).ToArray();  // Channels 1 to 17

            foreach (var dac in dacValues)
            {
                foreach (var channel in channels)
                {
                    var action = new LabJackTestAction
                    {
                        Channel = channel,
                        DacValue = dac,
                        ExpectedValues = GetExpectedValues(channel, dac)
                    };

                    // Send command to LabJack
                    WriteCommand(action.Channel, action.DacValue);
                    await Task.Delay(4000); // Wait for the system to stabilize

                    // Start measuring
                    var startTime = DateTime.Now;
                    while ((DateTime.Now - startTime).TotalMilliseconds < 1200)
                    {
                        action.Measurements.Add(Read_getChanMeasurement(action.Channel));
                        await Task.Delay(100); // Delay between measurements
                    }

                    // Evaluate the results
                    var (measuredMin, measuredMax) = (action.Measurements.Min(), action.Measurements.Max());
                    action.Result = measuredMin >= action.ExpectedValues.ExpectedMin &&
                                    measuredMax <= action.ExpectedValues.ExpectedMax;

                    // Log or update UI with the result
                    LogResult(action);
                }
            }
        }

        private (double ExpectedMin, double ExpectedMax) GetExpectedValues(int channel, double dacValue)
        {
            // Define logic to determine expected min/max based on channel and DAC value
            // Example:
            return (0.1, 4.9);  // Placeholder values
        }

        private void WriteCommand(int channel, double dacValue)
        {
            // Implement the method to send a command to the LabJack device
        }

        private double Read_getChanMeasurement(int channel)
        {
            // Implement the method to read a measurement from the LabJack device
            return new Random().NextDouble() * 5;  // Simulated reading, replace with actual device reading
        }

        private void LogResult(LabJackTestAction action)
        {
            // Implement a method to log or display the results
            Console.WriteLine($"Channel: {action.Channel}, DAC: {action.DacValue}, Result: {action.Result}");
        }






    }

    public class TESTAction
    {
        public string DeviceName { get; set; }
        public bool ExpectedState { get; set; }
        public string ValueToWrite { get; set; }
        public int WaitTimeBeforeRead { get; set; }
        public int ReadDuration { get; set; }
        public List<double> ReadValues { get; set; } = new List<double>();
        public double AverageValue => ReadValues.Count > 0 ? ReadValues.Average() : 0;
        public (double Min, double Max) MinMax => (ReadValues.Min(), ReadValues.Max());
        public bool Result { get; set; }
    }


    public class LabJackTestAction
    {
        public int Channel { get; set; }
        public double DacValue { get; set; }
        public List<double> Measurements { get; set; } = new List<double>();
        public (double ExpectedMin, double ExpectedMax) ExpectedValues { get; set; }
        public bool Result { get; set; }
    }


    public class TESTTest
    {
        public List<TESTAction> TESTActions { get; set; }
        public bool IsTestPassed => TESTActions.All(a => a.Result);

        public TESTTest()
        {
            TESTActions = new List<TESTAction>();
        }

        public void AddAction(TESTAction action)
        {
            TESTActions.Add(action);
        }
    }

}
