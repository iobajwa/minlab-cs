using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Testing.Helpers;

using Moq;
using NUnit.Framework;

namespace MiniLab.UnitTests.Testing.Helpers.Gestures
{
    public class gesture_test_fixture_base
    {
        protected Gesture _gesture;

        [SetUp]
        public void Setup()
        {
            _gesture = new Gesture();
        }
    }

    [TestFixture]
    public class when_creating_a_new_gesture : gesture_test_fixture_base
    {
        [Test]
        public void _01_SHOULD_create_an_instance_with_no_actions()
        {
            Assert.That(_gesture.Actions, Is.Empty);
        }
    }

    [TestFixture]
    public class when_adding_actions_into_a_gesture : gesture_test_fixture_base
    {
        private void DummyAction1() { }
        private void DummyAction2() { }
        
        [Test]
        public void _01_SHOULD_add_the_passed_item_into_gesture()
        {
            _gesture.Actions.Add(DummyAction1);
            _gesture.Actions.Add(DummyAction2);

            Assert.That(_gesture.Actions.Count, Is.EqualTo(2));
            Assert.That(_gesture.Actions[0], Is.EqualTo((Action)DummyAction1));
            Assert.That(_gesture.Actions[1], Is.EqualTo((Action)DummyAction2));
        }

        [Test]
        public void _02_AddActionsFromGesture_SHOULD_add_all_actions_from_the_passed_gesture()
        {
            Gesture dummyGesture = new Gesture();
            dummyGesture.Actions.Add(DummyAction1);
            dummyGesture.Actions.Add(DummyAction2);

            _gesture.AddActionsFromGesture(dummyGesture);

            Assert.That(_gesture.Actions, Is.EqualTo(dummyGesture.Actions));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void _03_AddActionsFromGesture_SHOULD_throw_ArgumentNullException_WHEN_gesture_is_passed_null()
        {
            _gesture.AddActionsFromGesture(null);
        }
    }

    [TestFixture]
    public class when_playing_a_gesture : gesture_test_fixture_base
    {
        [Test]
        public void _01_SHOULD_play_each_action_in_the_right_sequence()
        {
            Stack<int> recordedSequence = new Stack<int>();
            Stack<int> expectedSequence = new Stack<int>();
            expectedSequence.Push(1);
            expectedSequence.Push(2);
            expectedSequence.Push(3);
            _gesture.Actions.Add(() => recordedSequence.Push(1));
            _gesture.Actions.Add(() => recordedSequence.Push(2));
            _gesture.Actions.Add(() => recordedSequence.Push(3));


            _gesture.Play();


            Assert.That(recordedSequence, Is.EqualTo(expectedSequence));
        }
    }
}
