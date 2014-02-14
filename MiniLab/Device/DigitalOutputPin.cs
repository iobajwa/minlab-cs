using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class DigitalOutputPin : Pin
    {
        IDigitalOutputDevice _parent;
        public IDigitalOutputDevice ParentDevice { get { return _parent; } }

        public DigitalOutputPin(uint pinID, IDigitalOutputDevice parent) : base(pinID)
        { _parent = parent; }

        public bool State { set { _parent.WriteDigitalOutputPin(PinID, value); } }
    }
}
