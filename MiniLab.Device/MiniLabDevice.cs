using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device.Enumeration;

using USBHostLib;

namespace MiniLab.Device
{
    public class MiniLabDevice : IMiniLabDevice
    {
        private IHIDDevice _hidDevice;
        public bool Connected { get; private set; }

        public MiniLabDevice(IHIDDevice hidDevice)
        {
            _hidDevice = hidDevice;
        }


        /// <summary>
        /// Finds and connects with the MiniLab device hardware attached on the USB bus.
        /// </summary>
        public void Connect()
        {
            //_hidDevice.FindDevice();
            throw new NotImplementedException();
        }


        /// <summary>
        /// Enumerates Digital Functions present on the underlying MiniLab Device.
        /// </summary>
        /// <returns>List of DigitalFunctionReports for each digital function discovered.</returns>
        public List<DigitalFunctionReport> EnumerateDigitalFunctions()
        {
            byte commandCode = (byte)MiniLabCommands.EnumerateDigitalInputOutput;
            byte[] receivedResponse = SendCommand(commandCode);

            SanityCheckCommonPacketProperties(commandCode, receivedResponse, MiniLabPacketSizes.EnumerateDigitalInputOutputResponseSize);

            List<DigitalFunctionReport> digitalFunctions;

            digitalFunctions = ConstructDigitalInputFunctionReports(receivedResponse[1]);
            digitalFunctions.AddRange(ConstructDigitalOutputFunctionReports(receivedResponse[2]));

            return digitalFunctions;
        }


        /// <summary>
        /// Enumerates Analog Functions present on the underlying MiniLab Device.
        /// </summary>
        /// <returns>List of AnalogFunctionReports for each digital function discovered.</returns>
        public List<AnalogFunctionReport> EnumerateAnalogFunctions()
        {
            List<AnalogFunctionReport> analogFunctions = new List<AnalogFunctionReport>();

            byte commandCode = (byte)MiniLabCommands.EnumerateAnalogInput;
            byte[] receivedResponse = SendCommand(commandCode);
            VerifyEnumAnalogResponseSanity(commandCode, receivedResponse, MiniLabCommands.EnumerateAnalogInput);
            CreateAnalogFunctionReports(receivedResponse, true, 0, analogFunctions);

            commandCode = (byte)MiniLabCommands.EnumerateAnalogOutput;
            receivedResponse = SendCommand(commandCode);
            VerifyEnumAnalogResponseSanity(commandCode, receivedResponse, MiniLabCommands.EnumerateAnalogInput);
            CreateAnalogFunctionReports(receivedResponse, false, analogFunctions.Count, analogFunctions);

            return analogFunctions;
        }


        /// <summary>
        /// Reads the status of a digital input pin on the underlying MiniLab Device.
        /// </summary>
        /// <param name="pinID">The 0 based index of the pin whose status is to be read.</param>
        /// <returns>True when pin is at Logical High, False otherwise.</returns>
        public bool ReadDigitalInputPin(uint pinID)
        {
            byte commandCode = (byte)MiniLabCommands.ReadDigitalPin;
            byte[] response = SendCommand(commandCode, new byte[] { (byte)pinID });

            SanityCheckCommonPacketProperties(commandCode, response, MiniLabPacketSizes.ReadDigitalPinResponseSize);

            return response[2] == 0x01;
        }


        /// <summary>
        /// Writes the status of a digital output pin on the underlying MiniLab Device.
        /// </summary>
        /// <param name="pinID">The 0 based index of the pin whose state is to be written onto.</param>
        /// <param name="state">The actual state.</param>
        public void WriteDigitalOutputPin(uint pinID, bool state)
        {
            byte commandCode = (byte)MiniLabCommands.WriteDigitalPin;
            byte expectedState = (byte)((state == true) ? 0x01 : 0x00);
            byte[] response = SendCommand(commandCode, new byte[] { (byte)pinID, expectedState });

            SanityCheckCommonPacketProperties(commandCode, response, MiniLabPacketSizes.WriteDigitalPinResponseSize);

            byte actualState = response[2];
            if (actualState != expectedState)
                ThrowInvalidStateException(expectedState, actualState);
        }


        /// <summary>
        /// Reads the status of an analog input pin on the underlying MiniLab Device.
        /// </summary>
        /// <param name="pinID">The 0 based index of the pin whose status is to be read.</param>
        /// <returns>The binary value read from the underlying device.</returns>
        public uint ReadAnalogInputPin(uint pinID)
        {
            byte commandCode = (byte)MiniLabCommands.ReadAnalogPin;
            byte[] response = SendCommand(commandCode, new byte[] { (byte)pinID });

            SanityCheckCommonPacketProperties(commandCode, response, MiniLabPacketSizes.ReadAnalogPinResponseSize);

            uint readValue = response[2];
            readValue |= (uint)(response[3] << 8);
            readValue |= (uint)(response[4] << 16);

            return readValue;
        }


        /// <summary>
        /// Writes the status of an analog output pin on the underlying MiniLab Device.
        /// </summary>
        /// <param name="pinID">The 0 based index of the pin whose state is to be written onto.</param>
        /// <param name="state">The binary value to write.</param>
        public void WriteAnalogOutputPin(uint pinID, uint value)
        {
            byte commandCode = (byte)MiniLabCommands.WriteAnalogPin;
            byte[] response = SendCommand(commandCode, new byte[] { (byte)pinID, (byte)value, (byte)(value >> 8), (byte)(value >> 16) });

            SanityCheckCommonPacketProperties(commandCode, response, MiniLabPacketSizes.WriteAnalogPinResponseSize);

            uint actualState = response[2];
            actualState |= (uint)(response[3] << 8);
            actualState |= (uint)(response[4] << 16);
            if (actualState != value)
                ThrowInvalidStateException(value, actualState);
        }


        #region Common Helpers
        private byte[] SendCommand(byte commandCode)
        {
            byte[] commandPacket = new byte[] { commandCode };
            byte[] receivedResponse = ExecuteCommandOnDevice(commandPacket);
            return receivedResponse;
        }

        private byte[] SendCommand(byte commandCode, byte[] parameters)
        {
            byte[] commandPacket = new byte[] { commandCode };
            commandPacket = commandPacket.Concat(parameters).ToArray();
            byte[] receivedResponse = ExecuteCommandOnDevice(commandPacket);
            return receivedResponse;
        }

        protected byte[] ExecuteCommandOnDevice(byte[] commandPacket)
        {
            _hidDevice.WriteReportViaInterruptTransfer(commandPacket);
            return _hidDevice.ReadReportViaInterruptTransfer();
        }

        private static void SanityCheckCommonPacketProperties(byte commandCode, byte[] receivedResponse, int expectedSize)
        {
            if (receivedResponse.Length != expectedSize)
                ThrowInvalidPacketSizeException(expectedSize, receivedResponse.Length);

            if (receivedResponse[0] != commandCode)
                ThrowInvalidResponseCodeException(commandCode, receivedResponse[0]);
        }

        private static void ThrowInvalidPacketSizeException(int expectedSize, int actualSize)
        {
            throw new MiniLabCommunicationException("Protocol Error: Device responded with an invalid response packet. Expected size was " + expectedSize + " actual was " + actualSize + ".");
        }

        private static void ThrowInvalidResponseCodeException(byte expectedCode, byte receivedCode)
        {
            throw new MiniLabCommunicationException("Protocol Error: Device responded with an invalid response code. Expected " + expectedCode + " was " + receivedCode + ".");
        }

        private static void ThrowInvalidStateException(uint expectedState, uint actualState)
        {
            throw new MiniLabCommunicationException("Protocol Error: Device responded with an invalid response packet. Expected status was " + expectedState + " actual was " + actualState + ".");
        }
        #endregion


        #region Enumerate Digital Function Helpers
        private List<DigitalFunctionReport> ConstructDigitalInputFunctionReports(byte numberOfReportsToConstruct)
        {
            return ConstructDigitalFunctionReport(numberOfReportsToConstruct, true);
        }

        private List<DigitalFunctionReport> ConstructDigitalOutputFunctionReports(byte numberOfReportsToConstruct)
        {
            return ConstructDigitalFunctionReport(numberOfReportsToConstruct, false);
        }

        private List<DigitalFunctionReport> ConstructDigitalFunctionReport(byte numberOfReportsToConstruct, bool isInput)
        {
            List<DigitalFunctionReport> functionReports = new List<DigitalFunctionReport>(numberOfReportsToConstruct);

            for (uint i = 0; i < numberOfReportsToConstruct; i++)
                functionReports.Add(new DigitalFunctionReport(i, isInput));

            return functionReports;
        }
        #endregion


        #region Enumerate Analog Function Helpers
        private static void CreateAnalogFunctionReports(byte[] receivedResponse, bool isInput, int startID, List<AnalogFunctionReport> analogFunctions)
        {
            for (uint i = 0; i < receivedResponse[1]; i++)
            {
                uint logicalMin = receivedResponse[2 + i * 4];
                uint logicalMax = 0;
                logicalMax |= (uint)receivedResponse[3 + i * 4];
                logicalMax |= (uint)receivedResponse[4 + i * 4] << 8;
                logicalMax |= (uint)receivedResponse[5 + i * 4] << 16;
                AnalogFunctionReport functionReport = new AnalogFunctionReport((uint)startID + i, isInput, logicalMin, logicalMax);
                analogFunctions.Add(functionReport);
            }
        }

        private static void VerifyEnumAnalogResponseSanity(byte commandCode, byte[] receivedResponse, MiniLabCommands command)
        {
            if (receivedResponse[0] != commandCode)
                ThrowInvalidResponseCodeException(commandCode, receivedResponse[0]);

            if (receivedResponse.Length < 2)
                throw new MiniLabCommunicationException("Protocol Error: Device responded with a malformed packet for " + command + " command.");

            int expectedSize = receivedResponse[1] * 4 + 2;
            if (expectedSize != receivedResponse.Length)
                ThrowInvalidPacketSizeException(expectedSize, receivedResponse.Length);
        }
        #endregion
    }
}
