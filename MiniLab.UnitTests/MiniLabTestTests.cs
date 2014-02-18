using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;

using MiniLab.Testing;
using MiniLab.Testing.Device;

namespace MiniLab.UnitTests.Testing
{
    [TestFixture]
    public class when_creating_a_new_MiniLabTest_instance
    {
        Mock<IMiniLabDevice> _mockUSBDevice;
        MiniLabTest _miniLabTestBase;

        [SetUp]
        public void Setup()
        {
            _mockUSBDevice = new Mock<IMiniLabDevice>(MockBehavior.Strict);
        }

        [TearDown]
        public void VerifyMocks()
        {
            _mockUSBDevice.VerifyAll();
        }

        [Test]
        public void _01_SHOULD_attempt_to_connect_with_the_usb_device_WHEN_the_connection_has_not_been_already_made()
        {
            _mockUSBDevice.Setup(device => device.Connected).Returns(false);
            _mockUSBDevice.Setup(device => device.Connect());

            _miniLabTestBase = new MiniLabTest(_mockUSBDevice.Object);
        }

        [Test]
        public void _02_SHOULD_not_attempt_to_connect_with_the_usb_device_WHEN_the_connection_has_already_been_made()
        {
            _mockUSBDevice.Setup(device => device.Connected).Returns(true);

            _miniLabTestBase = new MiniLabTest(_mockUSBDevice.Object);
        }

        [Test]
        public void _03_MiniLab_public_property_SHOULD_return_same_reference_that_was_passed_while_construction()
        {
            _mockUSBDevice.Setup(device => device.Connected).Returns(true);
            _miniLabTestBase = new MiniLabTest(_mockUSBDevice.Object);

            Assert.That(_miniLabTestBase.MiniLab, Is.EqualTo(_mockUSBDevice.Object));
        }
    }
}
