using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

using NUnit.Framework;
using Moq;

namespace MiniLab.Tests.Device.Digital
{
    [TestFixture]
    public class when_interacting_with_DigitalOutputPin
    {
        DigitalOutputPin _pin;
        Mock<IDigitalOutputDevice> _mockParent;

        [SetUp]
        public void Setup()
        {
            _mockParent = new Mock<IDigitalOutputDevice>(MockBehavior.Strict);
            _pin = new DigitalOutputPin(5, _mockParent.Object);
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
            Assert.That(_pin.PinID, Is.EqualTo(5));
        }

        [Test]
        public void _03_value_written_onto_State_SHOULD_pass_down_to_the_parent()
        {
            _mockParent.Setup(parent => parent.WriteDigitalOutputPin(5, true));

            _pin.State = true;
        }
    }
}
