using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public interface IDigitalInputDevice
    {
        bool ReadDigitalInputPin(uint pinID);
    }

    public interface IDigitalOutputDevice
    {
        void WriteDigitalOutputPin(uint pinID, bool state);
    }

    public interface IMiniLabDevice : IDigitalInputDevice, IDigitalOutputDevice
    {
        bool Connected { get; }

        void Connect();
        void Reset();

        void WriteAnalogOutputPin(uint pinID, uint value);
        uint ReadAnalogInputPin(uint pinID);
    }
}
