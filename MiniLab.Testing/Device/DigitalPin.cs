using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

namespace MiniLab.Testing.Device
{
    public class DigitalPin : Pin
    {
        public IDigitalDevice ParentDevice { get; private set; }

        public DigitalPin(uint pinID, IDigitalDevice parentDevice) : base(pinID)
        {
            ParentDevice = parentDevice;
        }
    }
}
