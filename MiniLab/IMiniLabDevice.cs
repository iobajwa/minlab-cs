using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab
{
    public interface IMiniLabDevice
    {
        bool Connected { get; }

        void Connect();
    }
}
