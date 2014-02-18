using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Testing.Device
{
    /// <summary>
    /// Provides functionality to hold a collection of Pins.
    /// </summary>
    public class PinCollection<T> : List<T> where T : Pin
    {
        public void RemoveError()
        {
            throw new InvalidOperationException("Pins cannot be removed.");
        }

        new public void Remove(T item) { RemoveError(); }

        new public void RemoveAll(Predicate<T> criteria) { RemoveError(); }

        new public void RemoveAt(int index) { RemoveError(); }

        new public void RemoveRange(int index, int count) { RemoveError(); }

        new public void Reverse()
        {
            throw new InvalidOperationException("Pins cannot be reversed.");
        }

        new public void Reverse(int index, int count) { Reverse(); }

        /// <summary>
        /// Gets the pin based upon the Pin ID specified in the tag.
        /// </summary>
        /// <param name="tag">Tag should follow following format: Pin#ID# or P#ID# (case insensitive)</param>
        /// <returns></returns>
        public T this[string tag] 
        {
            get 
            {
                tag = tag.ToLower().Replace("pin", "").Replace("p", "");
                
                int index = 0;
                if (int.TryParse(tag, out index))
                    return this[index];

                throw new FormatException("Tag should be in 'Pin<id>' or 'P<id>' (case-insensitive) format.");
            }
        }

        /// <summary>
        /// Gets the pin at the specified ID.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        new public T this[int index]
        {
            get { return base[index]; }
        }

        /// <summary>
        /// Resets every pin contained inside the collection.
        /// </summary>
        public void ResetAll()
        {
            ForEach((T p) => p.Reset());
        }
    }
}
