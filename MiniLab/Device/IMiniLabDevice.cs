using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public interface IMiniLabDevice
    {
        bool Connected { get; }

        void Connect();
        void Reset();

        void WriteDigitalOutputPin(uint pinID, bool status);
        bool ReadDigitalInputPin(uint pinID);

        void WriteAnalogOutputPin(uint pinID, uint value);
        uint ReadAnalogInputPin(uint pinID);
    }
}
