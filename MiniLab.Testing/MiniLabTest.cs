using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Device.Enumeration;
using MiniLab.Testing.Device;

using Ninject;

namespace MiniLab.Testing
{
    public class MiniLabTest
    {
        protected static IKernel Kernel { get; private set; }

        protected static Device.MiniLab MiniLab_ { get; private set; }

        static MiniLabTest()
        {
            Kernel = new StandardKernel(new MiniLab.Device.Bindings(), new MiniLab.Testing.Bindings(), new USBHostLib.Bindings());
            MiniLab_ = Kernel.Get<MiniLab.Testing.Device.MiniLab>();
        }
    }
}
