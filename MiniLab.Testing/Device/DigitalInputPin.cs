using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

namespace MiniLab.Testing.Device
{
    public class DigitalInputPin : Pin
    {
        IDigitalInputDevice _parent;
        public IDigitalInputDevice ParentDevice { get { return _parent; } }

        public DigitalInputPin(uint pinID, IDigitalInputDevice parent) : base(pinID)
        { _parent = parent; }

        public bool IsSet 
        {
            get { return _parent.ReadDigitalInputPin(PinID) == true; }
        }

        public bool IsReset
        {
            get { return _parent.ReadDigitalInputPin(PinID) == false; }
        }

        public bool State
        { get { return _parent.ReadDigitalInputPin(PinID); } }
    }
}
