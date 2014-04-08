using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Device.Enumeration;

using USBHostLib;

using Moq;
using NUnit.Framework;

namespace MiniLab.UnitTests.Device.MiniLab
{
    internal class MiniLabDeviceTestsBase
    {
        protected MiniLabDevice _device;
        protected Mock<IHIDDevice> _mockUSB;
    
        [SetUp]
        public void __Setup()
        {
            _mockUSB = new Mock<IHIDDevice>(MockBehavior.Strict);
            _device = new MiniLabDevice(_mockUSB.Object);
        }

        [TearDown]
        public void __VerifyMocks()
        {
            _mockUSB.VerifyAll();
        }
    }

    [TestFixture]
    class when_enumerating_digital_functions : MiniLabDeviceTestsBase
    {
        [SetUp]
        public void SetupEnumCommand()
        {
            byte[] commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateDigitalInputOutput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateDigitalInputOutput, 4, 6 });
        }

        [Test]
        public void _01_SHOULD_relay_report_via_interrupt_transfer_to_the_underlying_HID_device()
        {
            _device.EnumerateDigitalFunctions();
        }

        [Test]
        public void _02_SHOULD_return_appropriatly_formed_Digital_Function_Reports_WHEN_a_valid_response_from_the_device_is_received()
        {
            List<DigitalFunctionReport> receivedReports;

            
            receivedReports = _device.EnumerateDigitalFunctions();


            Assert.That(receivedReports.Count, Is.EqualTo(10));
            for (int i = 0; i < 4; i++)
            {
                Assert.That(receivedReports[i].ID, Is.EqualTo(i));
                Assert.That(receivedReports[i].IsInput, Is.True);
            }
            for (int i = 0; i < 6; i++)
            {
                Assert.That(receivedReports[i + 4].ID, Is.EqualTo(i));
                Assert.That(receivedReports[i + 4].IsInput, Is.False);
            }
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response code. Expected 1 was 2.")]
        public void _03_SHOULD_throw_MiniLabCommunicationException_WHEN_received_response_from_underlying_device_contains_unexpected_command_code()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateDigitalInputOutput + 1, 4, 6 });

            _device.EnumerateDigitalFunctions();
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected size was 3 actual was 2.")]
        public void _04_SHOULD_throw_MiniLabCommunicationException_WHEN_received_response_packet_is_of_wrong_size()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateDigitalInputOutput, 4 });

            _device.EnumerateDigitalFunctions();
        }
    }

    [TestFixture]
    class when_enumerating_analog_functions : MiniLabDeviceTestsBase
    {
        public void SetupEnumAnalogIPCommand()
        {
            byte[] commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 1, 1, 2, 3, 4 });
        }

        public void SetupEnumAnalogIPOPCommand()
        {
            byte[] commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);

            _mockUSB
                .SetupSequence(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 1, 1, 2, 3, 4 })
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput, 1, 5, 6, 7, 8 });
        }

        [Test]
        public void _01_SHOULD_relay_Read_Analog_Input_and_Read_Analog_Output_report_via_interrupt_transfer_to_the_underlying_HID_device()
        {
            SetupEnumAnalogIPOPCommand();

            _device.EnumerateAnalogFunctions();
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response code. Expected 2 was 3.")]
        public void _02_SHOULD_throw_MiniLabCommunicationException_WHEN_Read_Analog_Input_response_received_from_underlying_device_contains_unexpected_command_code()
        {
            SetupEnumAnalogIPCommand();
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput + 1, 1, 1, 1, 1, 1 });


            _device.EnumerateAnalogFunctions();
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected size was 10 actual was 6.")]
        public void _03_SHOULD_throw_MiniLabCommunicationException_WHEN_Read_Analog_Input_response_packet_is_of_wrong_size()
        {
            SetupEnumAnalogIPCommand();
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 2, 1, 2, 3, 4 });


            _device.EnumerateAnalogFunctions();
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with a malformed packet for EnumerateAnalogInput command.")]
        public void _04_SHOULD_throw_MiniLabCommunicationException_WHEN_Read_Analog_input_response_packet_size_is_less_than_even_the_required_minimum()
        {
            SetupEnumAnalogIPCommand();
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput });


            _device.EnumerateAnalogFunctions();
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response code. Expected 3 was 4.")]
        public void _05_SHOULD_throw_MiniLabCommunicationException_WHEN_Read_Analog_Output_response_received_from_underlying_device_contains_unexpected_command_code()
        {
            byte[] commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);

            _mockUSB
                .SetupSequence(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 1, 1, 2, 3, 4 })
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput + 1, 1, 1, 1, 1, 1 });


            _device.EnumerateAnalogFunctions();
        }
        
        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected size was 2 actual was 6.")]
        public void _06_SHOULD_throw_MiniLabCommunicationException_WHEN_Read_Analog_Input_response_packet_is_of_wrong_size()
        {
            byte[] commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);

            _mockUSB
                .SetupSequence(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 1, 1, 2, 3, 4 })
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput, 0, 1, 1, 1, 1 });


            _device.EnumerateAnalogFunctions();
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with a malformed packet for EnumerateAnalogInput command.")]
        public void _07_SHOULD_throw_MiniLabCommunicationException_WHEN_Read_Analog_input_response_packet_size_is_less_than_even_the_required_minimum()
        {
            byte[] commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);

            _mockUSB
                .SetupSequence(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 1, 1, 2, 3, 4 })
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput });


            _device.EnumerateAnalogFunctions();
        }

        [Test]
        public void _08_SHOULD_return_appropriate_AnalogFunctionReports_formed_by_enumerating_the_underlying_HID_device()
        {
            byte[] commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            commandPacket = new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandPacket)).Returns(true);
            _mockUSB
                .SetupSequence(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 1, 1, 2, 3, 4 })
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput, 2, 5, 6, 7, 8, 8, 7, 6, 5 });


            List<AnalogFunctionReport> reports = _device.EnumerateAnalogFunctions();


            Assert.That(reports.Count, Is.EqualTo(3));
            Assert.That(reports[0].ID, Is.EqualTo(0));
            Assert.That(reports[0].IsInput, Is.True);
            Assert.That(reports[0].BinaryMinimum, Is.EqualTo(1));
            Assert.That(reports[0].BinaryMaximum, Is.EqualTo(0x00040302));

            Assert.That(reports[1].ID, Is.EqualTo(1));
            Assert.That(reports[1].IsInput, Is.False);
            Assert.That(reports[1].BinaryMinimum, Is.EqualTo(5));
            Assert.That(reports[1].BinaryMaximum, Is.EqualTo(0x00080706));

            Assert.That(reports[2].ID, Is.EqualTo(2));
            Assert.That(reports[1].IsInput, Is.False);
            Assert.That(reports[2].BinaryMinimum, Is.EqualTo(8));
            Assert.That(reports[2].BinaryMaximum, Is.EqualTo(0x00050607));
        }
    }

    [TestFixture]
    class when_reading_status_of_a_digital_pin : MiniLabDeviceTestsBase
    {
        [SetUp]
        public void Setup()
        {
            byte[] commandCode = new byte[] { (byte)MiniLabCommands.ReadDigitalPin, 3 };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandCode)).Returns(true);
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.ReadDigitalPin, 3, 0x01 });
        }

        [Test]
        public void _01_SHOULD_relay_Read_Digital_Pin_report_via_interrupt_transfer_to_the_underlying_HID_device()
        {
            _device.ReadDigitalInputPin(3);
        }

        [Test]
        public void _02_SHOULD_return_true_WHEN_the_device_returns_true_as_status_of_the_pin()
        {
            bool receivedStatus = _device.ReadDigitalInputPin(3);

            Assert.That(receivedStatus, Is.True);
        }

        [Test]
        public void _03_SHOULD_return_false_WHEN_the_device_returns_false_as_status_of_the_pin()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.ReadDigitalPin, 3, 0x00 });

            bool receivedStatus = _device.ReadDigitalInputPin(3);

            Assert.That(receivedStatus, Is.False);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response code. Expected 16 was 17.")]
        public void _04_SHOULD_throw_MiniLabCommunicationException_WHEN_response_received_from_underlying_device_contains_unexpected_command_code()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.ReadDigitalPin + 1, 3, 0x00 });
            
            
            _device.ReadDigitalInputPin(3);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected size was 3 actual was 2.")]
        public void _05_SHOULD_throw_MiniLabCommunicationException_WHEN_response_packet_is_of_wrong_size()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.ReadDigitalPin, 3 });


            _device.ReadDigitalInputPin(3);
        }
    }

    [TestFixture]
    class when_writing_onto_a_digital_pin : MiniLabDeviceTestsBase
    {
        [SetUp]
        public void Setup()
        {
            byte[] commandCode = new byte[] { (byte)MiniLabCommands.WriteDigitalPin, 6, 0x01 };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandCode)).Returns(true);
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(commandCode);
        }

        [Test]
        public void _01_SHOULD_relay_Write_Digital_Pin_report_via_interrupt_transfer_with_status_set_to_true_to_the_underlying_HID_device_WHEN_true_is_passed_as_status()
        {
            _device.WriteDigitalOutputPin(6, true);
        }

        [Test]
        public void _02_SHOULD_relay_Write_Digital_Pin_report_via_interrupt_transfer_with_status_set_to_false_to_the_underlying_HID_device_WHEN_false_is_passed_as_status()
        {
            _mockUSB = new Mock<IHIDDevice>(MockBehavior.Strict);
            _device = new MiniLabDevice(_mockUSB.Object);
            byte[] commandCode = new byte[] { (byte)MiniLabCommands.WriteDigitalPin, 6, 0x00 };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandCode)).Returns(true);
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(commandCode);


            _device.WriteDigitalOutputPin(6, false);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response code. Expected 17 was 18.")]
        public void _03_SHOULD_throw_MiniLabCommunicationException_WHEN_response_received_from_underlying_device_contains_unexpected_command_code()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.WriteDigitalPin + 1, 6, 0x01 });


            _device.WriteDigitalOutputPin(6, true);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected size was 3 actual was 4.")]
        public void _04_SHOULD_throw_MiniLabCommunicationException_WHEN_response_packet_is_of_wrong_size()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.WriteDigitalPin, 6, 0x01, 0x02 });


            _device.WriteDigitalOutputPin(6, true);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected status was 1 actual was 0.")]
        public void _05_SHOULD_throw_MiniLabCommunicationException_WHEN_response_echos_an_invalid_status()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.WriteDigitalPin, 6, 0x00 });


            _device.WriteDigitalOutputPin(6, true);
        }
    }

    [TestFixture]
    class when_reading_status_of_an_analog_pin : MiniLabDeviceTestsBase
    {
        [SetUp]
        public void Setup()
        {
            byte[] commandCode = new byte[] { (byte)MiniLabCommands.ReadAnalogPin, 4 };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandCode)).Returns(true);
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.ReadAnalogPin, 4, 0x01, 0x02, 0x03 });
        }

        [Test]
        public void _01_SHOULD_relay_Read_Analog_Pin_report_via_interrupt_transfer_to_the_underlying_HID_device()
        {
            _device.ReadAnalogInputPin(4);
        }

        [Test]
        public void _02_SHOULD_return_the_packed_value_returned_by_the_underlying_device()
        {
            uint receivedValue = _device.ReadAnalogInputPin(4);

            Assert.That(receivedValue, Is.EqualTo(0x00030201));
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response code. Expected 18 was 19.")]
        public void _03_SHOULD_throw_MiniLabCommunicationException_WHEN_response_received_from_underlying_device_contains_unexpected_command_code()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.ReadAnalogPin + 1, 4, 0x01, 0x02, 0x03 });


            _device.ReadAnalogInputPin(4);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected size was 5 actual was 6.")]
        public void _04_SHOULD_throw_MiniLabCommunicationException_WHEN_response_packet_is_of_wrong_size()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.ReadAnalogPin + 1, 4, 0x01, 0x02, 0x03, 0x04 });


            _device.ReadAnalogInputPin(4);
        }
    }

    [TestFixture]
    class when_writing_onto_an_analog_pin : MiniLabDeviceTestsBase
    {
        [SetUp]
        public void Setup()
        {
            byte[] commandCode = new byte[] { (byte)MiniLabCommands.WriteAnalogPin, 7, 0x01, 0x02, 0x03 };
            _mockUSB
                .Setup(usb => usb.WriteReportViaInterruptTransfer(commandCode)).Returns(true);
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(commandCode);
        }

        [Test]
        public void _01_SHOULD_relay_Write_Analog_Pin_report_via_interrupt_transfer_with_passed_value_to_the_underlying_HID_device()
        {
            _device.WriteAnalogOutputPin(7, 0x030201);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response code. Expected 19 was 20.")]
        public void _02_SHOULD_throw_MiniLabCommunicationException_WHEN_response_received_from_underlying_device_contains_unexpected_command_code()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.WriteAnalogPin + 1, 7, 0x01, 0x02, 0x03 });


            _device.WriteAnalogOutputPin(7, 0x030201);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected size was 5 actual was 2.")]
        public void _03_SHOULD_throw_MiniLabCommunicationException_WHEN_response_packet_is_of_wrong_size()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.WriteAnalogPin + 1, 7 });


            _device.WriteAnalogOutputPin(7, 0x030201);
        }

        [Test]
        [ExpectedException(typeof(MiniLabCommunicationException), ExpectedMessage = "Protocol Error: Device responded with an invalid response packet. Expected status was 197121 actual was 3.")]
        public void _04_SHOULD_throw_MiniLabCommunicationException_WHEN_response_echos_an_invalid_state()
        {
            _mockUSB
                .Setup(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.WriteAnalogPin, 7, 0x03, 0x00, 0x00 });


            _device.WriteAnalogOutputPin(7, 0x030201);
        }
    }
}
