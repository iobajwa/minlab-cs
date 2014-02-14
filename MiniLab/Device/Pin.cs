using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class Pin
    {
        public uint PinID { get; protected set; }

        public Pin(uint pinID)
        { PinID = pinID; }
    }
}
