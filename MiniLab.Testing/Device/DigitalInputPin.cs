using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

namespace MiniLab.Testing.Device
{
    public class DigitalInputPin : DigitalPin
    {
        new public IDigitalInputDevice ParentDevice { get; private set; }

        public DigitalInputPin(uint pinID, IDigitalInputDevice parent)
            : base(pinID, parent)
        { }

        public bool IsSet 
        {
            get { return ParentDevice.ReadDigitalInputPin(PinID) == true; }
        }

        public bool IsReset
        {
            get { return ParentDevice.ReadDigitalInputPin(PinID) == false; }
        }

        public bool State
        { get { return ParentDevice.ReadDigitalInputPin(PinID); } }
    }
}
