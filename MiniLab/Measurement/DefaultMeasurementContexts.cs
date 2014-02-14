using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Measurement
{
    public class TemperatureInDegreeCelcius : MeasurementContext
    {
        public TemperatureInDegreeCelcius()
            : base(new Scale(0, 100), Unit.Celcius, PhysicalQuantity.Temperature)
        { }

        public TemperatureInDegreeCelcius(Scale scale)
            : base(scale, Unit.Celcius, PhysicalQuantity.Temperature)
        { }
    }

    public class PT100 : MeasurementContext
    {
        public PT100()
            : base(new Scale(-50, 200), Unit.Celcius, PhysicalQuantity.Temperature)
        { }

        public PT100(Scale scale)
            : base(scale, Unit.Celcius, PhysicalQuantity.Temperature)
        { }
    }
}
