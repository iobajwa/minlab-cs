using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Testing.Measurement;

namespace MiniLab.Testing.Device
{
    public class AnalogInputPin : AnalogPin
    {
        public AnalogInputPin(uint pinID, IAnalogInputDevice parent)
            : base(pinID, 0, 0)
        { ParentDevice = parent; }

        public AnalogInputPin(uint pinID, IAnalogInputDevice parent, uint binaryMinimum, uint binaryMaximum)
            : base(pinID, binaryMinimum, binaryMaximum)
        { ParentDevice = parent; }

        /// <summary>
        /// Gets the reference to the underlying Parent Device.
        /// </summary>
        new public IAnalogInputDevice ParentDevice { get; protected set; }

        /// <summary>
        /// Reads the current analog value on the pin.
        /// </summary>
        /// <returns>Raw value.</returns>
        public uint Read()
        {
            return ParentDevice.ReadAnalogInputPin(PinID);
        }

        /// <summary>
        /// Reads the current analog value on the pin, scales it using the ConfiguredMeasurementContext scale.
        /// </summary>
        /// <typeparam name="T">Provide a MeasurementContext</typeparam>
        /// <returns>Scaled result.</returns>
        public double Read<T>() where T : MeasurementContext, new()
        {
            T measurementContextOfValue = new T();

            if (!ConfiguredMeasurementContext.IsIdentical(measurementContextOfValue))
                throw new InvalidOperationException("Configured units for the pin and units of passed value do no match.");

            uint rawValue = ParentDevice.ReadAnalogInputPin(PinID);

            return BinaryScale.ScaleValue(rawValue, ConfiguredMeasurementContext.Scale);
        }
    }
}
