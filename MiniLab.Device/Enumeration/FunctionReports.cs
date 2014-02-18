using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device.Enumeration
{
    /// <summary>
    /// Used to describe the Analog Function(s) of the MiniLab Device.
    /// </summary>
    public class AnalogFunctionReport
    {
        public uint ID { get; private set; }
        public bool IsInput {get; private set; }
        public uint BinaryMaximum { get; private set; }
        public uint BinaryMinimum { get; private set; }

        public AnalogFunctionReport(uint id, bool isInput, uint binaryMin, uint binaryMax)
        {
            ID = id;
            IsInput = isInput;
            BinaryMinimum = binaryMin;
            BinaryMaximum = binaryMax;
        }
    }

    /// <summary>
    /// Used to describe the Digital Function(s) of the MiniLab Device.
    /// </summary>
    public class DigitalFunctionReport
    {
        public uint ID { get; private set; }
        public bool IsInput { get; private set; }

        public DigitalFunctionReport(uint id, bool isInput)
        {
            ID = id;
            IsInput = isInput;
        }
    }
}
