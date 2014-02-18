using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Device.Enumeration;
using MiniLab.Testing.Device;

namespace MiniLab.Testing
{
    public class MiniLabTest
    {
        protected IMiniLabDevice Device { get; set; }
        public Device.MiniLab MiniLab { get; protected set; }

        public MiniLabTest(IMiniLabDevice device)
        {
            Device = device;

            if (!device.Connected)
            {
                device.Connect();

                MiniLab = new Device.MiniLab();

                EnumerateAnalogFunctions();
                EnumerateDigitalFunctions();
            }
        }

        private void EnumerateDigitalFunctions()
        {
            List<DigitalFunctionReport> digitalReports = Device.EnumerateDigitalFunctions();
            if (digitalReports != null)
                CreateDigitalPinObjects(digitalReports);
        }

        private void CreateDigitalPinObjects(List<DigitalFunctionReport> digitalReports)
        {
            foreach (DigitalFunctionReport report in digitalReports)
            {
                if (report.IsInput)
                    MiniLab.DigitalInputPins.Add(new DigitalInputPin(report.ID, Device));
                else
                    MiniLab.DigitalOutputPins.Add(new DigitalOutputPin(report.ID, Device));
            }
        }

        private void EnumerateAnalogFunctions()
        {
            List<AnalogFunctionReport> analogReports = Device.EnumerateAnalogFunctions();
            if (analogReports != null)
                CreateAnalogPinObjects(analogReports);
        }

        private void CreateAnalogPinObjects(List<AnalogFunctionReport> analogReports)
        {
            foreach (AnalogFunctionReport report in analogReports)
            {
                if (report.IsInput)
                    MiniLab.AnalogInputPins.Add(new AnalogInputPin(report.ID, Device, report.BinaryMinimum, report.BinaryMaximum));
                else
                    MiniLab.AnalogOutputPins.Add(new AnalogOutputPin(report.ID, Device, report.BinaryMinimum, report.BinaryMaximum));
            }
        }
    }
}
