using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Testing.Measurement
{
    public enum PhysicalQuantity
    {
        ACCurrent = 0,
        DCCurrent = 0,
        ACVoltage,
        DCVoltage,
        Temperature,
        Pressure,
    }

    public enum Unit
    {
        Ampere,
        Volts,
        Celcius,
        Farehnheit,
        Bar,
    }

    public class Scale
    {
        public double Minimum { get; protected internal set; }
        public double Maximum { get; protected internal set; }

        /// <summary>
        /// Creates a new instance of Scale.
        /// </summary>
        public Scale(double min, double max)
        {
            Minimum = min;
            Maximum = max;
        }

        /// <summary>
        /// Scales a given value from the current scale to a corresponding value in new scale.
        /// </summary>
        /// <param name="value">Value to scale.</param>
        /// <param name="toNewScale">The new scale.</param>
        /// <returns>Scaled value.</returns>
        public double ScaleValue(double value, Scale toNewScale)
        {
            if (toNewScale == null)
                throw new ArgumentNullException("toNewScale");

            return ScaleValue(value, this, toNewScale);
        }

        public static double ScaleValue(double value, Scale fromScale, Scale toScale)
        {
            if (value <= fromScale.Minimum)
                return toScale.Minimum;
            else if (value >= fromScale.Maximum)
                return toScale.Maximum;

            return (toScale.Maximum - toScale.Minimum) * ((value - fromScale.Minimum) / (fromScale.Maximum - fromScale.Minimum)) + toScale.Minimum;
        }
    }

    /// <summary>
    /// Class represents the measurement context of a measurement: The physical quantity being measured
    /// and the scale and units of measurement.
    /// </summary>
    public class MeasurementContext
    {
        public Scale Scale { get; protected internal set; }
        public Unit Unit { get; protected internal set; }
        public PhysicalQuantity MeasuredPhysicalQuantity { get; protected internal set; }

        public MeasurementContext(Scale scale, Unit unit, PhysicalQuantity physicalQuantityToMeasure)
        {
            Scale = scale;
            Unit = unit;
            MeasuredPhysicalQuantity = physicalQuantityToMeasure;
        }

        /// <summary>
        /// Provide range of scale for this MeasurementContext.
        /// </summary>
        /// <param name="minimum">Provide minimum value of scale.</param>
        /// <returns></returns>
        public MeasurementContext WithRangeFrom(double minimum)
        {
            Scale.Minimum = minimum;

            return this;
        }

        /// <summary>
        /// Provide the maximum value for the scale of this MeasurementContext.
        /// </summary>
        /// <param name="maximum"></param>
        public void To(double maximum)
        {
            Scale.Maximum = maximum;
        }

        /// <summary>
        /// Used to figure out weather a given MeasurementContext instance is identical to another MeasurementContext.
        /// Two instances are said to be identical when Unit and PhysicalQuantity of each instance match with each other.
        /// </summary>
        /// <param name="toOtherContext">The context with which the comparison is to be made.</param>
        /// <returns></returns>
        public bool IsIdentical(MeasurementContext toOtherContext)
        {
            if (toOtherContext == null)
                throw new ArgumentNullException("toOtherContext");

            if (this.Unit != toOtherContext.Unit)
                return false;

            if (this.MeasuredPhysicalQuantity != toOtherContext.MeasuredPhysicalQuantity)
                return false;

            return true;
        }
    }
}
