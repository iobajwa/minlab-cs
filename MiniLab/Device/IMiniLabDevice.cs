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

    public interface IMiniLabDevice : IDigitalInputDevice
    {
        bool Connected { get; }

        void Connect();
        void Reset();

        void WriteDigitalOutputPin(uint pinID, bool status);

        void WriteAnalogOutputPin(uint pinID, uint value);
        uint ReadAnalogInputPin(uint pinID);
    }
}
