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
        public IDigitalDevice ParentDevice { get; protected set; }

        public DigitalPin(uint pinID)
            : base(pinID)
        { }
    }
}
