using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Measurement;

using NUnit.Framework;
using Moq;

namespace MiniLab.Tests.Device.Analog.AnalogInput
{
    #region base test class
    public class analog_input_pin_test_base
    {
        protected AnalogInputPin _pin;
        protected Mock<IAnalogInputDevice> _mockParent;

        [SetUp]
        public void Setup()
        {
            _mockParent = new Mock<IAnalogInputDevice>(MockBehavior.Strict);
            _pin = new AnalogInputPin(1, _mockParent.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockParent.VerifyAll();
        }
    }
    #endregion

    [TestFixture]
    public class when_constructing_a_new_AnalogInputPin : analog_input_pin_test_base
    {
        [Test]
        public void _01_SHOULD_derive_from_AnalogPin_base_class()
        {
            AnalogPin baseObject = _pin as AnalogPin;

            Assert.That(baseObject, Is.Not.Null);
        }

        [Test]
        public void _02_Parent_SHOULD_return_the_same_parent_reference_which_was_passed_to_it_during_construction()
        {
            Assert.That(_pin.ParentDevice, Is.EqualTo(_mockParent.Object));
        }
    }

    [TestFixture]
    public class when_reading_binary_value_from_an_analog_pin : analog_input_pin_test_base
    {
        [Test]
        public void _01_SHOULD_return_raw_binary_value_read_from_the_underlying_parent()
        {
            _mockParent.Setup(parent => parent.ReadAnalogInputPin(1)).Returns(45);
            uint expectedValue = 45, readValue;

            readValue = _pin.Read();

            Assert.That(readValue, Is.EqualTo(expectedValue));
        }
    }

    [TestFixture]
    public class when_reading_scaled_value_from_an_analog_pin : analog_input_pin_test_base
    {
        [SetUp]
        new public void Setup()
        {
            _pin = new AnalogInputPin(1, _mockParent.Object, 0, 200);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Configured units for the pin and units of passed value do no match.")]
        public void _01_SHOULD_throw_InvalidOperationException_WHEN_passed_MeasurementContext_is_in_voilation_with_the_configured_MeasurementContext()
        {
            _pin.ConfigureScaleFor<TemperatureInDegreeCelcius>().WithRangeFrom(45).To(60);

            _pin.Read<CurrentInAmperes>();
        }

        [Test]
        public void _02_SHOULD_return_the_scaled_value()
        {
            _mockParent.Setup(parent => parent.ReadAnalogInputPin(1)).Returns(101);
            _pin.ConfigureScaleFor<TemperatureInDegreeCelcius>().WithRangeFrom(0).To(100);
            double readValue;

            readValue = _pin.Read<TemperatureInDegreeCelcius>();

            Assert.That(readValue, Is.EqualTo(50.5));
        }
    }
}
