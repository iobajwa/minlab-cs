using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemInterface.Threading;

namespace MiniLab.Testing.Helpers
{
    public interface IDelay
    {
        void InSeconds(int seconds);
        void InMilliseconds(int milliSeconds);
    }

    /// <summary>
    /// Provides Delay related services.
    /// <remarks>
    /// Delay is provided by putting the current executing thread to sleep. 
    /// Thereby actual delay provided is equal to "atleast" the requested amount of delay.
    /// Under heavy CPU load, actual delay might be more than requested amount.
    /// </remarks>
    /// </summary>
    public class Delay : IDelay
    {
        IThread _thread;

        public Delay(IThread thread)
        {
            _thread = thread;
        }

        /// <summary>
        /// Causes the current thread to sleep for provided milliseconds.
        /// </summary>
        /// <param name="milliseconds"></param>
        public void InMilliseconds(int milliseconds)
        {
            if (milliseconds <= 0)
                throw new ArgumentException("milliSeconds cannot be zero or negative value.");

            _thread.Sleep(milliseconds);
        }

        /// <summary>
        /// Causes the current thread to sleep for provided seconds.
        /// </summary>
        /// <param name="seconds"></param>
        public void InSeconds(int seconds)
        {
            if (seconds <= 0)
                throw new ArgumentException("seconds cannot be zero or negative value.");

            _thread.Sleep(seconds * 1000);
        }
    }
}
