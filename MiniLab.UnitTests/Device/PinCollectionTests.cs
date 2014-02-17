using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;

using Moq;
using NUnit.Framework;

namespace MiniLab.Tests.Device.Pins
{
    public class pin_collection_test_fixture_base
    {
        protected PinCollection _pins;

        [SetUp]
        public void Setup()
        {
            _pins = new PinCollection();
        }
    }

    [TestFixture]
    public class when_creating_a_new_PinCollection_instance : pin_collection_test_fixture_base
    {
        [Test]
        public void _01_Pins_SHOULD_be_initially_be_empty()
        {
            int count = 0;
            foreach(Pin pin in _pins)
                count++;

            Assert.That(count, Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class when_interacting_with_the_pin_collection : pin_collection_test_fixture_base
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Pins cannot be removed.")]
        public void _01_Remove_pin_SHOULD_throw_InvalidOperationException()
        {
            _pins.Remove(new Pin(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Pins cannot be removed.")]
        public void _02_RemoveAll_SHOULD_throw_InvalidOperationException()
        {
            _pins.RemoveAll((Pin p) => p.PinID == 1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Pins cannot be removed.")]
        public void _03_RemoveAt_SHOULD_throw_InvalidOperationException()
        {
            _pins.RemoveAt(0);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Pins cannot be removed.")]
        public void _04_RemoveRange_SHOULD_throw_InvalidOperationException()
        {
            _pins.RemoveRange(0, 0);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Pins cannot be reversed.")]
        public void _05_Reverse_SHOULD_throw_InvalidOperationException()
        {
            _pins.Reverse();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Pins cannot be reversed.")]
        public void _06_Reverse_index_count_SHOULD_throw_InvalidOperationException()
        {
            _pins.Reverse(0, 1);
        }

        [Test]
        public void _07_pins_added_SHOULD_be_retreivable_via_int_indexer()
        {
            Pin dummyPin = new Pin(0);
            _pins.Add(dummyPin);

            Assert.That(_pins[0], Is.EqualTo(dummyPin));
        }

        internal class DummyPin : Pin
        {
            public bool ResetCalled { get; set; }
            
            public DummyPin()
                : base(0)
            { }

            public override void Reset()
            {
                ResetCalled = true;
            }
        }

        [Test]
        public void _08_ResetAll_SHOULD_reset_every_pin_in_the_collection()
        {
            _pins.Add(new DummyPin());
            _pins.Add(new DummyPin());
            _pins.Add(new DummyPin());

            _pins.ResetAll();

            foreach (DummyPin pin in _pins)
                Assert.That(pin.ResetCalled, Is.True);
        }
    }

    [TestFixture]
    public class when_retreiving_pins_using_string_indexer : pin_collection_test_fixture_base
    {
        [Test]
        public void _01_result_SHOULD_be_retreivable_using__pin_ID__format()
        {
            Pin dummyPin = new Pin(0);
            _pins.Add(dummyPin);

            Assert.That(_pins["Pin0"], Is.EqualTo(dummyPin));
        }

        [Test]
        public void _02_result_SHOULD_be_retreivable_via__p_ID__format()
        {
            Pin dummyPin = new Pin(0);
            _pins.Add(dummyPin);

            Assert.That(_pins["p0"], Is.EqualTo(dummyPin));
        }

        [Test]
        [ExpectedException(typeof(FormatException), ExpectedMessage = "Tag should be in 'Pin<id>' or 'P<id>' (case-insensitive) format.")]
        public void _03_SHOULD_throw_FormatException_WHEN_tag_is_passed_in_an_unexpected_format()
        {
            Pin result = _pins["unexpected_format_0"];
        }
    }
}
