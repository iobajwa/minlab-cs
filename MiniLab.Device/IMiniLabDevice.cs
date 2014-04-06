using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device.Enumeration;

namespace MiniLab.Device
{
    public interface IDigitalDevice
    { }

    public interface IDigitalInputDevice : IDigitalDevice
    {
        bool ReadDigitalInputPin(uint pinID);
    }

    public interface IDigitalOutputDevice : IDigitalDevice
    {
        void WriteDigitalOutputPin(uint pinID, bool state);
    }

    public interface IAnalogDevice
    { }

    public interface IAnalogOutputDevice : IAnalogDevice
    {
        void WriteAnalogOutputPin(uint pinID, uint value);
    }

    public interface IAnalogInputDevice : IAnalogDevice
    {
        uint ReadAnalogInputPin(uint pinID);
    }

    public interface IMiniLabDevice : IDigitalInputDevice, IDigitalOutputDevice, IAnalogInputDevice, IAnalogOutputDevice
    {
        bool Connected { get; }
        //string SerialNumber { get; set; }

        void Connect();
        //void Reset();

        List<AnalogFunctionReport> EnumerateAnalogFunctions();
        List<DigitalFunctionReport> EnumerateDigitalFunctions();
    }
}
