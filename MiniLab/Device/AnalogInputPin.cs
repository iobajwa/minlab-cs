using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class AnalogInputPin : AnalogPin
    {
        public AnalogInputPin(uint pinID, IAnalogInputDevice parent)
            : base(pinID, 0, 0)
        { _parent = parent; }

        public AnalogInputPin(uint pinID, IAnalogInputDevice parent, uint binaryMinimum, uint binaryMaximum)
            : base(pinID, binaryMinimum, binaryMaximum)
        { _parent = parent; }

        IAnalogInputDevice _parent;
        /// <summary>
        /// Gets the reference of the underlying Parent Device.
        /// </summary>
        public IAnalogInputDevice ParentDevice { get { return _parent; } }

        public uint Read()
        {
            return _parent.ReadAnalogInputPin(PinID);
        }
    }
}
