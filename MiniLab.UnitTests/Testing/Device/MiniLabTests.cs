using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Device.Enumeration;
using MiniLab.Testing.Device;

using NUnit.Framework;
using Moq;

namespace MiniLab.UnitTests.Testing.Device.MiniLab_
{
    [TestFixture]
    class when_creating_a_new_MiniLab_instance
    {
        Mock<IMiniLabDevice> _mockUSBDevice;
        MiniLab.Testing.Device.MiniLab _lab;

        [SetUp]
        public void Setup()
        {
            _mockUSBDevice = new Mock<IMiniLabDevice>();
        }

        [Test]
        public void _01_SHOULD_attempt_to_connect_with_the_usb_device_WHEN_the_connection_has_not_been_already_made()
        {
            _mockUSBDevice.Setup(device => device.Connect()).Verifiable();

            _lab = new MiniLab.Testing.Device.MiniLab(_mockUSBDevice.Object);
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
            PinCollection<AnalogInputPin> expectedAIPins = new PinCollection<AnalogInputPin>()
            {
                new AnalogInputPin(2, _mockUSBDevice.Object, 30, 40),
                new AnalogInputPin(3, _mockUSBDevice.Object, 50, 60),
            };
            PinCollection<AnalogOutputPin> expectedAOPins = new PinCollection<AnalogOutputPin>()
            {
                new AnalogOutputPin(1, _mockUSBDevice.Object, 10, 20),
            };



            _lab = new MiniLab.Testing.Device.MiniLab(_mockUSBDevice.Object);



            Assert.That(_lab.AnalogInputPins.Count, Is.EqualTo(expectedAIPins.Count));
            Assert.That(_lab.AnalogOutputPins.Count, Is.EqualTo(expectedAOPins.Count));
            for (int i = 0; i < expectedAIPins.Count; i++)
                Assert.That(AreAnalogPinsIdentical(_lab.AnalogInputPins[0], expectedAIPins[0]), Is.True);
            for (int i = 0; i < expectedAIPins.Count; i++)
                Assert.That(AreAnalogPinsIdentical(_lab.AnalogOutputPins[0], expectedAOPins[0]), Is.True);
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
            PinCollection<DigitalInputPin> expectedDIPins = new PinCollection<DigitalInputPin>()
            {
                new DigitalInputPin(2, _mockUSBDevice.Object),
                new DigitalInputPin(3, _mockUSBDevice.Object),
            };
            PinCollection<DigitalOutputPin> expectedDOPins = new PinCollection<DigitalOutputPin>()
            {
                new DigitalOutputPin(1, _mockUSBDevice.Object),
            };



            _lab = new MiniLab.Testing.Device.MiniLab(_mockUSBDevice.Object);



            Assert.That(_lab.DigitalInputPins.Count, Is.EqualTo(expectedDIPins.Count));
            Assert.That(_lab.DigitalOutputPins.Count, Is.EqualTo(expectedDOPins.Count));
            for (int i = 0; i < expectedDIPins.Count; i++)
                Assert.That(AreDigitalPinsIdentical(_lab.DigitalInputPins[0], expectedDIPins[0]), Is.True);
            for (int i = 0; i < expectedDIPins.Count; i++)
                Assert.That(AreDigitalPinsIdentical(_lab.DigitalOutputPins[0], expectedDOPins[0]), Is.True);
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
}
