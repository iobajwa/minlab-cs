using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

using NUnit.Framework;
using Moq;

namespace MiniLab.Tests.Device.Analog
{
    public class when_constructing_an_analog_pin
    {
        AnalogPin _pin;

        [SetUp]
        public void Setup()
        {
            _pin = new AnalogPin(1, 5, 10);
        }

        [Test]
        public void _01_PinID_SHOULD_return_the_same_value_which_was_passed_to_it_during_construction()
        {
            Assert.That(_pin.PinID, Is.EqualTo(1));
        }

        [Test]
        public void _02_BinaryMinimum_and_BinaryMaximum_SHOULD_return_corrects_value_which_were_passed_to_them_during_construction()
        {
            Assert.That(_pin.BinaryMinimum, Is.EqualTo(5));
            Assert.That(_pin.BinaryMaximum, Is.EqualTo(10));
        }
    }
}
