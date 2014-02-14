using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Measurement;

using NUnit.Framework;

namespace MiniLab.Tests.Measurement
{
    [TestFixture]
    public class when_configuring_a_measurement_context
    {
        MeasurementContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new MeasurementContext(new Scale(1, 10), Unit.Celcius, PhysicalQuantity.Temperature);
        }

        [Test]
        public void _01_WithRangeFrom_SHOULD_return_the_same_instance_reference_and_configure_minimum_maximum_with_provided_values()
        {
            MeasurementContext expectedReference = _context, receivedReference;

            receivedReference = _context.WithRangeFrom(-100);

            Assert.That(receivedReference, Is.EqualTo(expectedReference));
            Assert.That(receivedReference.Scale.Minimum, Is.EqualTo(-100));
            Assert.That(receivedReference.Scale.Maximum, Is.EqualTo(10));
        }

        [Test]
        public void _02_To_SHOULD_set_the_maximum_value_of_scale()
        {
            _context.To(100);

            Assert.That(_context.Scale.Maximum, Is.EqualTo(100));
        }
    }
}
