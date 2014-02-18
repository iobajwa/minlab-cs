using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Testing.Device
{
    /// <summary>
    /// Represents the logical MiniLab Device.
    /// </summary>
    public class MiniLab
    {
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
        

        internal MiniLab()
        {
            AnalogInputPins = new PinCollection<AnalogInputPin>();
            AnalogOutputPins = new PinCollection<AnalogOutputPin>();
            DigitalInputPins = new PinCollection<DigitalInputPin>();
            DigitalOutputPins = new PinCollection<DigitalOutputPin>();
        }
    }
}
