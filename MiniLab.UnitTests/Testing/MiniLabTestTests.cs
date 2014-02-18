using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;

using MiniLab.Device;
using MiniLab.Device.Enumeration;
using MiniLab.Testing;
using MiniLab.Testing.Device;

namespace MiniLab.UnitTests.Testing.when_creating_a_new_MiniLabTest_instance
{
    public class MiniLabTest_test_fixture_base
    {
        protected Mock<IMiniLabDevice> _mockUSBDevice;
        protected MiniLabTest _miniLabTestBase;

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
    }

    [TestFixture]
    public class and_no_connection_is_in_place : MiniLabTest_test_fixture_base
    {
        [SetUp]
        new public void Setup()
        {
            _mockUSBDevice = new Mock<IMiniLabDevice>();
            _mockUSBDevice.Setup(device => device.Connected).Returns(false);
        }

        [Test]
        public void _01_SHOULD_attempt_to_connect_with_the_usb_device_WHEN_the_connection_has_not_been_already_made()
        {
            _mockUSBDevice.Setup(device => device.Connect()).Verifiable();

            _miniLabTestBase = new MiniLabTest(_mockUSBDevice.Object);
        }

        [Test]
        public void _02_SHOULD_enumerate_the_device_for_analog_functions_and_construct_analog_pins_accordingly()
        {
            List<AnalogFunctionReport> dummyEnumerationReport = new List<AnalogFunctionReport>()
            {
                new AnalogFunctionReport(1, false, 10, 20),
                new AnalogFunctionReport(2, true, 30, 40),
                new AnalogFunctionReport(3, true, 50, 60),
            };
            _mockUSBDevice.Setup(device => device.EnumerateAnalogFunctions()).Returns(dummyEnumerationReport).Verifiable();
            PinCollection expectedAIPins = new PinCollection()
            {
                new AnalogInputPin(2, _mockUSBDevice.Object, 30, 40),
                new AnalogInputPin(3, _mockUSBDevice.Object, 50, 60),
            };
            PinCollection expectedAOPins = new PinCollection()
            {
                new AnalogOutputPin(1, _mockUSBDevice.Object, 10, 20),
            };



            _miniLabTestBase = new MiniLabTest(_mockUSBDevice.Object);


            Assert.That(_miniLabTestBase.MiniLab.AnalogInputPins.Count, Is.EqualTo(expectedAIPins.Count));
            Assert.That(_miniLabTestBase.MiniLab.AnalogOutputPins.Count, Is.EqualTo(expectedAOPins.Count));
            for (int i = 0; i < expectedAIPins.Count; i++)
                Assert.That(AreAnalogPinsIdentical((AnalogInputPin)_miniLabTestBase.MiniLab.AnalogInputPins[0], (AnalogInputPin)expectedAIPins[0]), Is.True);
            for (int i = 0; i < expectedAIPins.Count; i++)
                Assert.That(AreAnalogPinsIdentical((AnalogOutputPin)_miniLabTestBase.MiniLab.AnalogOutputPins[0], (AnalogOutputPin)expectedAOPins[0]), Is.True);
        }

        [Test]
        public void _03_SHOULD_enumerate_the_device_for_digital_functions_and_construct_digital_pins_accordingly()
        {
            List<DigitalFunctionReport> dummyEnumerationReport = new List<DigitalFunctionReport>()
            {
                new DigitalFunctionReport(1, false),
                new DigitalFunctionReport(2, true),
                new DigitalFunctionReport(3, true),
            };
            _mockUSBDevice.Setup(device => device.EnumerateDigitalFunctions()).Returns(dummyEnumerationReport).Verifiable();
            PinCollection expectedDIPins = new PinCollection()
            {
                new DigitalInputPin(2, _mockUSBDevice.Object),
                new DigitalInputPin(3, _mockUSBDevice.Object),
            };
            PinCollection expectedDOPins = new PinCollection()
            {
                new DigitalOutputPin(1, _mockUSBDevice.Object),
            };



            _miniLabTestBase = new MiniLabTest(_mockUSBDevice.Object);


            Assert.That(_miniLabTestBase.MiniLab.DigitalInputPins.Count, Is.EqualTo(expectedDIPins.Count));
            Assert.That(_miniLabTestBase.MiniLab.DigitalOutputPins.Count, Is.EqualTo(expectedDOPins.Count));
            for (int i = 0; i < expectedDIPins.Count; i++)
                Assert.That(AreDigitalPinsIdentical((DigitalInputPin)_miniLabTestBase.MiniLab.DigitalInputPins[0], (DigitalInputPin)expectedDIPins[0]), Is.True);
            for (int i = 0; i < expectedDIPins.Count; i++)
                Assert.That(AreDigitalPinsIdentical((DigitalOutputPin)_miniLabTestBase.MiniLab.DigitalOutputPins[0], (DigitalOutputPin)expectedDOPins[0]), Is.True);
        }

        bool AreAnalogPinsIdentical(AnalogPin pin1, AnalogPin pin2)
        {
            if (
                pin1.ParentDevice == pin2.ParentDevice &&
                pin1.PinID == pin2.PinID &&
                pin1.BinaryMinimum == pin2.BinaryMinimum &&
                pin1.BinaryMaximum == pin2.BinaryMaximum
               )
                return true;
            else
                return false;
        }

        bool AreDigitalPinsIdentical(DigitalPin pin1, DigitalPin pin2)
        {
            if (
                pin1.PinID == pin2.PinID &&
                pin1.ParentDevice == pin2.ParentDevice
               )
                return true;
            else
                return false;
        }
    }

    [TestFixture]
    public class and_connection_is_already_in_place : MiniLabTest_test_fixture_base
    {
        [Test]
        public void _01_SHOULD_not_attempt_to_connect_and_perform_enumeration_with_the_usb_device()
        {
            _mockUSBDevice.Setup(device => device.Connected).Returns(true);

            _miniLabTestBase = new MiniLabTest(_mockUSBDevice.Object);
        }
    }
}
