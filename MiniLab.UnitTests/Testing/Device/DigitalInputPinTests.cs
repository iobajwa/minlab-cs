using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Testing.Device;

using NUnit.Framework;
using Moq;

namespace MiniLab.UnitTests.Testing.Device.Digital
{
    [TestFixture]
    class when_interacting_with_DigitalInputPin
    {
        DigitalInputPin _pin;
        Mock<IDigitalInputDevice> _mockParent;

        [SetUp]
        public void Setup()
        {
            _mockParent = new Mock<IDigitalInputDevice>(MockBehavior.Strict);
            _pin = new DigitalInputPin(3, _mockParent.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockParent.VerifyAll();
        }

        [Test]
        public void _01_Parent_SHOULD_return_the_same_parent_reference_which_was_passed_to_it_during_construction()
        {
            Assert.That(_pin.ParentDevice, Is.EqualTo(_mockParent.Object));
        }

        [Test]
        public void _02_PinID_SHOULD_return_the_same_value_which_was_passed_to_it_during_construction()
        {
            Assert.That(_pin.PinID, Is.EqualTo(3));
        }

        [Test]
        public void _03_IsSet_SHOULD_return_true_WHEN_the_pin_is_set_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(true);

            Assert.That(_pin.IsSet, Is.True);
        }

        [Test]
        public void _04_IsSet_SHOULD_return_false_WHEN_the_pin_is_reset_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(false);

            Assert.That(_pin.IsSet, Is.False);
        }

        [Test]
        public void _05_IsReset_SHOULD_return_true_WHEN_the_pin_is_reset_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(false);

            Assert.That(_pin.IsReset, Is.True);
        }

        [Test]
        public void _06_IsReset_SHOULD_return_false_WHEN_the_pin_is_set_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(true);

            Assert.That(_pin.IsReset, Is.False);
        }

        [Test]
        public void _07_State_SHOULD_return_false_WHEN_pin_is_set_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(false);

            Assert.That(_pin.State, Is.False);
        }

        [Test]
        public void _08_State_SHOULD_return_true_WHEN_pin_is_set_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(true);

            Assert.That(_pin.State, Is.True);
        }
    }   
}
