using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Measurement;

namespace MiniLab.Device
{
    public class AnalogOutputPin : AnalogPin
    {
        public AnalogOutputPin(uint pinID, IAnalogOutputDevice parent)
            : base(pinID, 0, 0)
        { ParentDevice = parent; }

        public AnalogOutputPin(uint pinID, IAnalogOutputDevice parent, uint binaryMinimum, uint binaryMaximum)
            : base(pinID, binaryMinimum, binaryMaximum)
        { ParentDevice = parent; }

        /// <summary>
        /// Gets the reference to the underlying Parent Device.
        /// </summary>
        public IAnalogOutputDevice ParentDevice { get; protected set; }

        /// <summary>
        /// Writes the passed binary value onto the underlying analog pin of the parent device.
        /// Note: Value is automatically trunacted to fit inside the BinaryScale.
        /// <param name="binaryValue">The raw value to write onto the pin.</param>
        /// </summary>
        public void Set(uint binaryValue)
        {
            if (binaryValue > BinaryMaximum)
                binaryValue = BinaryMaximum;
            else if (binaryValue < BinaryMinimum)
                binaryValue = BinaryMinimum;

            ParentDevice.WriteAnalogOutputPin(PinID, binaryValue);
        }

        /// <summary>
        /// Writes the passed scaled value onto the underlying analog pin of the parent device.
        /// Note: The value is automatically trunacted to fit inside the BinaryScale.
        /// </summary>
        /// <typeparam name="T">Provide a MeasurementContext.</typeparam>
        /// <param name="value">The value to write onto the pin.</param>
        public void Set<T>(double value) where T : MeasurementContext, new()
        {
            T measurementContextOfValue = new T();

            if (!ConfiguredMeasurementContext.IsIdentical(measurementContextOfValue))
                throw new InvalidOperationException("Configured units for the pin and units of passed value do no match.");

            uint rawValue = (uint)ConfiguredMeasurementContext.Scale.ScaleValue(value, BinaryScale);

            ParentDevice.WriteAnalogOutputPin(PinID, rawValue);
        }

        /// <summary>
        /// Resets the Analog Pin on the underlying Hardware by writing BinaryMinimum as output.
        /// </summary>
        public override void Reset()
        {
            ParentDevice.WriteAnalogOutputPin(PinID, BinaryMinimum);
        }
    }
}
