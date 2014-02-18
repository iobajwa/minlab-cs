using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLab.Testing.Helpers
{
    /// <summary>
    /// Provides functionality to enlist a set of actions and 'play' (act upon each) when directed.
    /// </summary>
    public class Gesture
    {
        /// <summary>
        /// Gets the list of Actions that is contained within the Gesture instance.
        /// </summary>
        public List<Action> Actions { get; set; }

        /// <summary>
        /// Creates a new Gesture instance.
        /// </summary>
        public Gesture()
        {
            Actions = new List<Action>();
        }

        /// <summary>
        /// Plays the set of actions in exactly the same sequence in which they were added.
        /// </summary>
        public void Play()
        {
            foreach (Action action in Actions)
                action();
        }

        /// <summary>
        /// Adds actions from another gesture instance.
        /// </summary>
        /// <param name="gesture">The gesture instance from which to borrow actions from.</param>
        public void AddActionsFromGesture(Gesture gesture)
        {
            if (gesture == null)
                throw new ArgumentNullException("gesture");

            Actions.AddRange(gesture.Actions);
        }
    }
}
