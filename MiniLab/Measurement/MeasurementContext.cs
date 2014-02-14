using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Measurement
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
        Volts,
        Ampere,
        Celcius,
        Farehnheit,
        Bar,
    }

    public class Scale
    {
        public double Minimum { get; protected internal set; }
        public double Maximum { get; protected internal set; }

        public Scale(double min, double max)
        {
            Minimum = min;
            Maximum = max;
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

        public MeasurementContext WithRangeFrom(double minimum)
        {
            Scale.Minimum = minimum;

            return this;
        }

        public void To(double maximum)
        {
            Scale.Maximum = maximum;
        }
    }
}
