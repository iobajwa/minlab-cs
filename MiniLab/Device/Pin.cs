using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class Pin
    {
        /// <summary>
        /// Gets the Pin ID.
        /// </summary>
        public uint PinID { get; protected set; }

        public Pin(uint pinID)
        { PinID = pinID; }

        /// <summary>
        /// Resets the Pin to default state.
        /// </summary>
        public virtual void Reset()
        { }
    }
}
