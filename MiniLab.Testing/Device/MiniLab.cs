using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Device.Enumeration;

namespace MiniLab.Testing.Device
{
    /// <summary>
    /// Represents the logical MiniLab Device.
    /// </summary>
    public class MiniLab
    {
        IMiniLabDevice _device;

        /// <summary>
        /// Gets the Digital Input Pins supported by the MiniLab Device.
        /// </summary>
        public PinCollection<DigitalInputPin> DigitalInputPins { get; internal set; }

        /// <summary>
        /// Gets the Digital Output Pins supported by the MiniLab Device.
        /// </summary>
        public PinCollection<DigitalOutputPin> DigitalOutputPins { get; internal set; }
        
        /// <summary>
        /// Gets the Analog Input Pins supported by the MiniLab Device.
        /// </summary>
        public PinCollection<AnalogInputPin> AnalogInputPins { get; internal set; }

        /// <summary>
        /// Gets the Analog Output Pins supported by the MiniLab Device.
        /// </summary>
        public PinCollection<AnalogOutputPin> AnalogOutputPins { get; internal set; }
        

        public MiniLab(IMiniLabDevice device)
        {
            AnalogInputPins = new PinCollection<AnalogInputPin>();
            AnalogOutputPins = new PinCollection<AnalogOutputPin>();
            DigitalInputPins = new PinCollection<DigitalInputPin>();
            DigitalOutputPins = new PinCollection<DigitalOutputPin>();

            _device = device;
            Connect();
        }


        #region this.Connect and Helpers
        private void Connect()
        {
            _device.Connect();
            EnumerateDigitalFunctions();
            EnumerateAnalogFunctions();
        }

        private void EnumerateDigitalFunctions()
        {
            List<DigitalFunctionReport> digitalReports = _device.EnumerateDigitalFunctions();
            if (digitalReports != null)
                CreateDigitalPinObjects(digitalReports);
        }

        private void CreateDigitalPinObjects(List<DigitalFunctionReport> digitalReports)
        {
            foreach (DigitalFunctionReport report in digitalReports)
            {
                if (report.IsInput)
                    DigitalInputPins.Add(new DigitalInputPin(report.ID, _device));
                else
                    DigitalOutputPins.Add(new DigitalOutputPin(report.ID, _device));
            }
        }

        private void EnumerateAnalogFunctions()
        {
            List<AnalogFunctionReport> analogReports = _device.EnumerateAnalogFunctions();
            if (analogReports != null)
                CreateAnalogPinObjects(analogReports);
        }

        private void CreateAnalogPinObjects(List<AnalogFunctionReport> analogReports)
        {
            foreach (AnalogFunctionReport report in analogReports)
            {
                if (report.IsInput)
                    AnalogInputPins.Add(new AnalogInputPin(report.ID, _device, report.BinaryMinimum, report.BinaryMaximum));
                else
                    AnalogOutputPins.Add(new AnalogOutputPin(report.ID, _device, report.BinaryMinimum, report.BinaryMaximum));
            }
        } 
        #endregion
    }
}
