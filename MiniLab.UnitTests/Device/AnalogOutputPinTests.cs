using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

using NUnit.Framework;
using Moq;

namespace MiniLab.Tests.Device
{
    #region base test class
    public class analog_output_pin_test_base
    {
        protected AnalogOutputPin _pin;
        protected Mock<IAnalogOutputDevice> _mockParent;

        [SetUp]
        public void Setup()
        {
            _mockParent = new Mock<IAnalogOutputDevice>(MockBehavior.Strict);
            _pin = new AnalogOutputPin(1, _mockParent.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockParent.VerifyAll();
        }
    }
    #endregion

    [TestFixture]
    public class when_constructing_a_new_AnalogOutputPin : analog_output_pin_test_base
    {
        [Test]
        public void _01_Parent_SHOULD_return_the_same_parent_reference_which_was_passed_to_it_during_construction()
        {
            Assert.That(_pin.ParentDevice, Is.EqualTo(_mockParent.Object));
        }

        [Test]
        public void _02_PinID_SHOULD_return_the_same_value_which_was_passed_to_it_during_construction()
        {
            Assert.That(_pin.PinID, Is.EqualTo(1));
        }

        [Test]
        public void _03_BinaryMinimum_and_BinaryMaximum_SHOULD_return_corrects_value_which_were_passed_to_them_during_construction()
        {
            _pin = new AnalogOutputPin(0, null, 5, 10);

            Assert.That(_pin.BinaryMinimum, Is.EqualTo(5));
            Assert.That(_pin.BinaryMaximum, Is.EqualTo(10));
        }
    }

    [TestFixture]
    public class when_writing_a_binary_value_onto_an_analog_pin : analog_output_pin_test_base
    {
        [SetUp]
        new public void Setup()
        {
            _pin = new AnalogOutputPin(1, _mockParent.Object, 10, 100);
        }

        [Test]
        public void _01_the_value_SHOULD_pass_on_to_the_parent()
        {
            _mockParent.Setup(parent => parent.WriteAnalogOutputPin(1, 75));

            _pin.Set(75);
        }

        [Test]
        public void _02_SHOULD_truncate_the_value_to_BinaryMaximum_WHEN_passed_value_is_greater_than_BinaryMaximum()
        {
            _mockParent.Setup(parent => parent.WriteAnalogOutputPin(1, 100));

            _pin.Set(150);
        }

        [Test]
        public void _03_SHOULD_truncate_the_value_to_BinaryMinimum_WHEN_passed_value_is_less_than_BinaryManimum()
        {
            _mockParent.Setup(parent => parent.WriteAnalogOutputPin(1, 10));

            _pin.Set(5);
        }
    }
}
