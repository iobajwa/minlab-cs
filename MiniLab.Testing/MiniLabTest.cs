using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

namespace MiniLab
{
    public class MiniLabTest
    {
        IMiniLabDevice _device;

        public IMiniLabDevice MiniLab { get { return _device; } }

        public MiniLabTest() { }

        public MiniLabTest(IMiniLabDevice device)
        {
            _device = device;

            if (!device.Connected)
                device.Connect();
        }
    }
}
