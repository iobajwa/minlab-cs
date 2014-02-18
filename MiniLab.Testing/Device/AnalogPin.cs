using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Testing.Measurement;

namespace MiniLab.Testing.Device
{
    public class AnalogPin : Pin
    {
        protected Scale BinaryScale { get; set; }

        /// <summary>
        /// Gets the Minimum value (in binary) which can be written onto the Analog Pin.
        /// </summary>
        public uint BinaryMinimum { get { return (uint)BinaryScale.Minimum; } }

        /// <summary>
        /// Gets the Maximum value (in binary) which can be written onto the Analog Pin.
        /// </summary>
        public uint BinaryMaximum { get { return (uint)BinaryScale.Maximum; } }

        /// <summary>
        /// Gets the Measurement Context (Physical Quantity being measured, Scale and Unit of measurement) that
        /// the pin is configured for.
        /// </summary>
        public MeasurementContext ConfiguredMeasurementContext { get; private set; }


        /// <summary>
        /// Creates a new AnalogPin instance.
        /// </summary>
        /// <param name="pinID">The pinID to identify the pin on the underlying hardware.</param>
        /// <param name="binaryMinimum">The minimum value that can be measured.</param>
        /// <param name="binaryMaximum">The maximum value that can be measured.</param>
        public AnalogPin(uint pinID, uint binaryMinimum, uint binaryMaximum) : base(pinID)
        {
            BinaryScale = new Measurement.Scale(binaryMinimum, binaryMaximum);
        }


        /// <summary>
        /// Configures the Measurement Context (Physical Quantity being measured and it's Scale, Units) for
        /// measurements taken for the current pin.
        /// </summary>
        /// <typeparam name="T">The Measurement Context to configure from.</typeparam>
        public MeasurementContext ConfigureScaleFor<T>() where T : MeasurementContext, new()
        {
            ConfiguredMeasurementContext = new T();
            return ConfiguredMeasurementContext;
        }
    }
}
