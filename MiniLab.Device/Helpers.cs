using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public enum MiniLabCommands
    {
        EnumerateDigitalInputOutput = 0x01,
        EnumerateAnalogInput = 0x02,
        EnumerateAnalogOutput = 0x03,

        ReadDigitalPin = 0x10,
        WriteDigitalPin = 0x11,
        ReadAnalogPin = 0x12,
        WriteAnalogPin = 0x13,
    }

    internal static class MiniLabPacketSizes
    {
        internal const int EnumerateDigitalInputOutputResponseSize = 3;
        internal const int ReadDigitalPinResponseSize = 3;
        internal const int WriteDigitalPinResponseSize = 3;
        internal const int ReadAnalogPinResponseSize = 5;
        internal const int WriteAnalogPinResponseSize = 5;
    }

    public class MiniLabCommunicationException : Exception
    {
        public MiniLabCommunicationException(string message)
            : base(message)
        { }
    }
}
