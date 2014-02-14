using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Device
{
    public class AnalogOutputPin : Pin
    {
        public AnalogOutputPin(uint pinID, IAnalogOutputDevice parent)
            : base(pinID)
        { _parent = parent; }

        public AnalogOutputPin(uint pinID, IAnalogOutputDevice parent, uint binaryMinimum, uint binaryMaximum)
            : this(pinID, parent)
        { BinaryMinimum = binaryMinimum; BinaryMaximum = binaryMaximum; }

        IAnalogOutputDevice _parent;
        /// <summary>
        /// Gets the reference of the underlying Parent Device.
        /// </summary>
        public IAnalogOutputDevice ParentDevice { get { return _parent; } }

        //public Scale CurrentScale { get; set; }
        /// <summary>
        /// Gets the Minimum value (in binary) which can be written onto the Analog Pin.
        /// </summary>
        public uint BinaryMinimum { get; protected internal set; }

        /// <summary>
        /// Gets the Maximum value (in binary) which can be written onto the Analog Pin.
        /// </summary>
        public uint BinaryMaximum { get; protected internal set; }

        /// <summary>
        /// Writes the passed binary value onto the underlying analog pin on the parent device.
        /// Note: Value is automatically trunacted to fit between BinaryMinimum and BinaryMaximum in case the
        /// passed value is beyond the range.
        /// </summary>
        public void Set(uint binaryValue)
        {
            if (binaryValue > BinaryMaximum)
                binaryValue = BinaryMaximum;
            else if (binaryValue < BinaryMinimum)
                binaryValue = BinaryMinimum;

            _parent.WriteAnalogOutputPin(PinID, binaryValue);
        }

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
