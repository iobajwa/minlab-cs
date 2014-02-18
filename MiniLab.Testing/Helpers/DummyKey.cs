using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

namespace MiniLab.Helpers
{
    /// <summary>
    /// Provides functionality related to simulate Hardware 'Keys' on Digital Output pins.
    /// </summary>
    public class DummyKey
    {
        public IDigitalOutputPin Pin { get; private set; }

        public bool ClickPolarity { get; private set; }
        public uint ClickDurationTime { get; private set; }
        public uint ClickReactionTime { get; private set; }
        public uint DoubleClickDurationTime { get; private set; }

        IDelay _delay;

        /// <summary>
        /// Creates a new DummyKey instance with the default values.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="delay"></param>
        public DummyKey(IDigitalOutputPin pin, IDelay delay)
        {
            Pin = pin;
            ClickPolarity = false;
            ClickReactionTime = ClickDurationTime = 100;
            DoubleClickDurationTime = ClickReactionTime / 2;
            _delay = delay;
        }

        public DummyKey(IDigitalOutputPin pin, IDelay delay, bool clickPolarity, uint clickDurationTime, uint clickReactionTime)
            :this(pin, delay)
        {
            ClickPolarity = clickPolarity;
            ClickReactionTime = clickReactionTime;
            ClickDurationTime = clickDurationTime;
            DoubleClickDurationTime = ClickReactionTime / 2;
        }
        
        public DummyKey(IDigitalOutputPin pin, IDelay delay, bool clickPolarity, uint clickDurationTime, uint clickReactionTime, uint doubleClickDurationTime)
            : this(pin, delay, clickPolarity, clickDurationTime, clickReactionTime)
        {
            DoubleClickDurationTime = doubleClickDurationTime;
        }

        /// <summary>
        /// Simulates a 'click' action on the underlying hardware pin.
        /// </summary>
        public void Click()
        {
            Pin.State = ClickPolarity;
            WaitFor(ClickDurationTime);
            Pin.State = !ClickPolarity;
            WaitFor(ClickReactionTime);
        }

        /// <summary>
        /// Simulates a 'double click' action on the underlying hardware pin.
        /// </summary>
        public void DoubleClick()
        {
            Pin.State = ClickPolarity;
            WaitFor(ClickDurationTime);
            Pin.State = !ClickPolarity;
            WaitFor(DoubleClickDurationTime);
            Pin.State = ClickPolarity;
            WaitFor(ClickDurationTime);
            Pin.State = !ClickPolarity;
            WaitFor(ClickReactionTime);
        }

        /// <summary>
        /// Simulates a 'Hold' action on the underlying hardware pin.
        /// The pin state is held to 'Click Polarity'.
        /// </summary>
        public void Hold()
        {
            Pin.State = ClickPolarity;
        }

        /// <summary>
        /// Releases the underlying hardware pin.
        /// </summary>
        public void Release()
        {
            Pin.State = !ClickPolarity;
        }

        /// <summary>
        /// Holds the key for the specified time period and releases it afterwards.
        /// </summary>
        /// <param name="timePeriod">The time duration for which to 'Hold' the key.</param>
        public void HoldFor(uint timePeriod)
        {
            Hold();
            WaitFor(timePeriod);
            Release();
        }

        private void WaitFor(uint timePeriod)
        {
            if (timePeriod > 0)
                _delay.InMilliseconds((int)timePeriod);
        }
    }
}
