using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Testing.Helpers;

using SystemInterface.Threading;

using NUnit.Framework;
using Moq;

namespace MiniLab.UnitTests.Testing.Helpers.Delays
{
    class basic_delay_test_fixture
    {
        protected IDelay _delay;
        protected Mock<IThread> _mockThread;

        [SetUp]
        public void Setup()
        {
            _mockThread = new Mock<IThread>(MockBehavior.Strict);
            _delay = new Delay(_mockThread.Object);
        }

        [TearDown]
        public void VerifyMocks()
        {
            _mockThread.VerifyAll();
        }
    }

    [TestFixture]
    class when_requesting_delay_in_milliseconds : basic_delay_test_fixture
    {
        [Test]
        public void _01_SHOULD_put_the_thread_to_sleep_for_10_ms_WHEN_10_ms_delay_has_been_requested()
        {
            _mockThread.Setup(thread => thread.Sleep(10));

            _delay.InMilliseconds(10);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "milliSeconds cannot be zero or negative value.")]
        public void _02_SHOULD_throw_InvalidArgumentException_WHEN_passed_milliSeconds_is_less_than_or_equal_to_zero()
        {
            _delay.InMilliseconds(0);
        }
    }

    [TestFixture]
    class when_requesting_delay_in_seconds : basic_delay_test_fixture
    {
        [Test]
        public void _01_SHOULD_put_the_thread_to_sleep_for_10_seconds_WHEN_10_second_delay_has_been_requested()
        {
            _mockThread.Setup(thread => thread.Sleep(10 * 1000));

            _delay.InSeconds(10);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "seconds cannot be zero or negative value.")]
        public void _02_SHOULD_throw_InvalidArgumentException_WHEN_passed_seconds_is_less_than_or_equal_to_zero()
        {
            _delay.InSeconds(0);
        }
    }
}
