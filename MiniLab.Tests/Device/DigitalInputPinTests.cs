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

        [Test]
        public void _01_IsSet_SHOULD_return_true_WHEN_the_pin_is_set_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(true);

            Assert.That(_pin.IsSet, Is.True);
        }

        [Test]
        public void _02_IsSet_SHOULD_return_false_WHEN_the_pin_is_reset_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(false);

            Assert.That(_pin.IsSet, Is.False);
        }

        [Test]
        public void _03_IsReset_SHOULD_return_true_WHEN_the_pin_is_reset_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(false);

            Assert.That(_pin.IsReset, Is.True);
        }

        [Test]
        public void _04_IsReset_SHOULD_return_false_WHEN_the_pin_is_set_on_parent_device()
        {
            _mockParent.Setup(parent => parent.ReadDigitalInputPin(3)).Returns(true);

            Assert.That(_pin.IsReset, Is.False);
        }
    }   
}
