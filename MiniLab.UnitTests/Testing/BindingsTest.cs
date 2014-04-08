using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MiniLab.Device;

using USBHostLib;

using Ninject;
using Ninject.Modules;

using Moq;
using NUnit.Framework;

namespace MiniLab.UnitTests.Testing
{
    internal class DummyUSBHostBindings : NinjectModule
    {
        private static Mock<IHIDDevice> _mockDevice = new Mock<IHIDDevice>();
        private static Mock<IHIDFinder> _mockFinder = new Mock<IHIDFinder>();

        public override void Load()
        {
            _mockFinder
                .Setup(finder => finder.FindDevice(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(_mockDevice.Object);

            _mockDevice
                .SetupSequence(usb => usb.ReadReportViaInterruptTransfer())
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateDigitalInputOutput, 0, 0 })
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogInput, 0 })
                .Returns(new byte[] { (byte)MiniLabCommands.EnumerateAnalogOutput, 0 });

            Bind<IHIDFinder>().ToConstant(_mockFinder.Object).InSingletonScope();
        }
    }

    class when_verifying_bindings__a_call_to_get
    {
        IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel(new MiniLab.Testing.Bindings(), new MiniLab.Device.Bindings(), new DummyUSBHostBindings());
        }

        [Test]
        public void _01_MiniLab_SHOULD_return_the_same_MiniLab_instance_each_time()
        {
            var instance1 = _kernel.Get<MiniLab.Testing.Device.MiniLab>();
            var instance2 = _kernel.Get<MiniLab.Testing.Device.MiniLab>();

            Assert.That(instance1, Is.EqualTo(instance2));
        }
    }
}
