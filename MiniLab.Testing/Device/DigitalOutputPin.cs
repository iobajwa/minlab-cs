using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

namespace MiniLab.Testing.Device
{
    public interface IDigitalOutputPin
    {
        bool State { set; }

        void Set();
        void Reset();
    }

    public class DigitalOutputPin : DigitalPin, IDigitalOutputPin
    {
        new public IDigitalOutputDevice ParentDevice { get; private set; }

        public DigitalOutputPin(uint pinID, IDigitalOutputDevice parent)
            : base(pinID)
        {
            ParentDevice = parent;
        }

        /// <summary>
        /// Sets or Resets the state of the pin on the underlying hardware
        /// </summary>
        public bool State { set { SetPinStateOnParent(value); } }

        /// <summary>
        /// Sets the pin state on the underlying hardware to Logic High.
        /// </summary>
        public void Set()
        {
            SetPinStateOnParent(true);
        }

        /// <summary>
        /// Sets the pin state on the underlying hardware to Logic Low.
        /// </summary>
        public override void Reset()
        {
            SetPinStateOnParent(false);
        }

        private void SetPinStateOnParent(bool state)
        {
            ParentDevice.WriteDigitalOutputPin(PinID, state);
        }
    }
}
