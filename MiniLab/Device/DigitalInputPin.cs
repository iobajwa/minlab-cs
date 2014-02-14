using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class DigitalInputPin
    {
        public uint PinID { get; private set; }
        IDigitalInputDevice _parent;
        public IDigitalInputDevice ParentDevice { get { return _parent; } }

        public DigitalInputPin(uint pinID, IDigitalInputDevice parent)
        { PinID = pinID; _parent = parent; }

        public bool IsSet 
        {
            get { return _parent.ReadDigitalInputPin(PinID) == true; }
        }

        public bool IsReset
        {
            get { return _parent.ReadDigitalInputPin(PinID) == false; }
        }
    }
}
