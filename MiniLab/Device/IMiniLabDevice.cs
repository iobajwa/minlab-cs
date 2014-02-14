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

    public interface IAnalogOutputDevice
    {
        void WriteAnalogOutputPin(uint pinID, uint value);
    }

    public interface IMiniLabDevice : IDigitalInputDevice, IDigitalOutputDevice, IAnalogOutputDevice
    {
        bool Connected { get; }

        void Connect();
        void Reset();

        uint ReadAnalogInputPin(uint pinID);
    }
}
