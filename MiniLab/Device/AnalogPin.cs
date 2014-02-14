using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class AnalogPin : Pin
    {
        /// <summary>
        /// Gets the Minimum value (in binary) which can be written onto the Analog Pin.
        /// </summary>
        public uint BinaryMinimum { get; protected internal set; }

        /// <summary>
        /// Gets the Maximum value (in binary) which can be written onto the Analog Pin.
        /// </summary>
        public uint BinaryMaximum { get; protected internal set; }


        public AnalogPin(uint pinID, uint binaryMinimum, uint binaryMaximum) : base(pinID)
        {
            BinaryMinimum = binaryMinimum;
            BinaryMaximum = binaryMaximum;
        }

        //public Scale CurrentScale { get; set; }

        //public void ConfigureFor<T>()
        //{
        //    throw new NotImplementedException();
        //}

        //public void ConfigureFor<T>(float min, float max)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ConfigureScaleFor<T>()
        //{
        //    throw new NotImplementedException();
        //}

        //public void ConfigureScaleFor<T>(float min, float max)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Set<T>(float value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
