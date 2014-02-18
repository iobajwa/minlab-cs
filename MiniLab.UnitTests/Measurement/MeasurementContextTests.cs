using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Measurement;

using NUnit.Framework;

namespace MiniLab.UnitTests.Measurement
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

    [TestFixture]
    public class when_comparing_two_measurement_contexts
    {
        MeasurementContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new MeasurementContext(new Scale(1, 10), Unit.Celcius, PhysicalQuantity.Temperature);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void _01_SHOULD_throw_ArgumentNullException_WHEN_toOtherContext_is_passed_null()
        {
            _context.IsIdentical(null);
        }

        [Test]
        public void _02_IsIdentical_SHOULD_return_false_WHEN_Unit_of_new_context_does_not_matches()
        {
            MeasurementContext newContext = new MeasurementContext(new Scale(0, 10), Unit.Ampere, PhysicalQuantity.Temperature);
            bool result;

            result = _context.IsIdentical(newContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void _03_IsIdentical_SHOULD_return_false_WHEN_PhysicalQuantity_of_new_context_does_not_matches()
        {
            MeasurementContext newContext = new MeasurementContext(new Scale(0, 10), Unit.Celcius, PhysicalQuantity.Pressure);
            bool result;

            result = _context.IsIdentical(newContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void _04_IsIdentical_SHOULD_return_true_WHEN_both_Unit_and_PhysicalQuantity_of_new_context_match()
        {
            MeasurementContext newContext = new MeasurementContext(new Scale(0, 10), Unit.Celcius, PhysicalQuantity.Temperature);
            bool result;

            result = _context.IsIdentical(newContext);

            Assert.That(result, Is.True);
        }
    }

    [TestFixture]
    public class when_scaling_values
    {
        Scale _referenceScale;

        [SetUp]
        public void Setup()
        {
            _referenceScale = new Scale(0, 100);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void _01_SHOULD_throw_NullArgumentException_WHEN_newScale_is_passed_null()
        {
            _referenceScale.ScaleValue(45, null);
        }

        [Test]
        public void _02_SHOULD_convert_passed_value_to_new_scale()
        {
            Scale newScale = new Scale(-100, 100);
            double scaledValue;

            scaledValue = _referenceScale.ScaleValue(50, newScale);

            Assert.That(scaledValue, Is.EqualTo(0));
        }
    }
}
