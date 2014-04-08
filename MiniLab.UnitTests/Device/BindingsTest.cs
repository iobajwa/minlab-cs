using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject;

using USBHostLib;
using MiniLab.Device;

using NUnit.Framework;

namespace MiniLab.UnitTests.Device
{
    class when_verifying_bindings__a_call_to_get
    {
        IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel(new MiniLab.Device.Bindings(), new USBHostLib.Bindings());
        }

        [Test]
        public void _01_IMiniLabDevice_SHOULD_return_the_same_MiniLabDevice_instance_each_time()
        {
            var instance1 = _kernel.Get<IMiniLabDevice>();
            var instance2 = _kernel.Get<IMiniLabDevice>();

            Assert.That(instance1, Is.EqualTo(instance2));
        }
    }
}
