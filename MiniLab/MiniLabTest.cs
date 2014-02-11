using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab
{
    public class MiniLabTest
    {
        IMiniLabDevice _device;

        public MiniLabTest(IMiniLabDevice device)
        {
            _device = device;

            if (!device.Connected)
                device.Connect();
        }
    }
}
