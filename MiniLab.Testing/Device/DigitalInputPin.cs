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
        new public IDigitalInputDevice ParentDevice { get; protected set; }

        public DigitalInputPin(uint pinID, IDigitalInputDevice parent)
            : base(pinID)
        {
            ParentDevice = parent;
        }

        /// <summary>
        /// Gets weather underlying pin hardware state is set to Logic High.
        /// </summary>
        public bool IsSet 
        {
            get { return ParentDevice.ReadDigitalInputPin(PinID) == true; }
        }

        /// <summary>
        /// Gets weather underlying pin hardware state is set to Logic Low.
        /// </summary>
        public bool IsReset
        {
            get { return ParentDevice.ReadDigitalInputPin(PinID) == false; }
        }

        /// <summary>
        /// Gets the underlying pin hardware state.
        /// </summary>
        public bool State
        { get { return ParentDevice.ReadDigitalInputPin(PinID); } }
    }
}
