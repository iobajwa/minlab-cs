using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class AnalogOutputPin : AnalogPin
    {
        public AnalogOutputPin(uint pinID, IAnalogOutputDevice parent)
            : base(pinID, 0, 0)
        { _parent = parent; }

        public AnalogOutputPin(uint pinID, IAnalogOutputDevice parent, uint binaryMinimum, uint binaryMaximum)
            : base(pinID, binaryMinimum, binaryMaximum)
        { _parent = parent; }

        IAnalogOutputDevice _parent;
        /// <summary>
        /// Gets the reference of the underlying Parent Device.
        /// </summary>
        public IAnalogOutputDevice ParentDevice { get { return _parent; } }

        /// <summary>
        /// Writes the passed binary value onto the underlying analog pin on the parent device.
        /// Note: Value is automatically trunacted to fit between BinaryMinimum and BinaryMaximum in case the
        /// passed value is beyond the range.
        /// </summary>
        public void Set(uint binaryValue)
        {
            if (binaryValue > BinaryMaximum)
                binaryValue = BinaryMaximum;
            else if (binaryValue < BinaryMinimum)
                binaryValue = BinaryMinimum;

            _parent.WriteAnalogOutputPin(PinID, binaryValue);
        }

        
    }
}
