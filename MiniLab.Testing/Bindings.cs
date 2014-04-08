using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

namespace MiniLab.Testing
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<MiniLab.Testing.Device.MiniLab>().ToSelf().InSingletonScope();
        }
    }
}
