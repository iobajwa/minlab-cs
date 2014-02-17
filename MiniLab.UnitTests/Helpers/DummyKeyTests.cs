using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniLab.Device;
using MiniLab.Helpers;

using Moq;
using NUnit.Framework;

namespace MiniLab.Tests.Helpers.Keys
{
    public class dummy_key_test_fixture_base
    {
        protected Mock<IDigitalOutputPin> _dummyPin;
        protected Mock<IDelay> _mockDelay;
        protected DummyKey _Key;

        [SetUp]
        public void Setup()
        {
            _dummyPin = new Mock<IDigitalOutputPin>();
            _mockDelay = new Mock<IDelay>();
            _Key = new DummyKey(_dummyPin.Object, _mockDelay.Object, true, 200, 600);
        }

        [TearDown]
        public void VerifyMocks()
        {
            _dummyPin.VerifyAll();
            _mockDelay.VerifyAll();
        }
    }

    [TestFixture]
    public class when_creating_a_new_dummy_key_instance : dummy_key_test_fixture_base
    {
        [SetUp]
        new public void Setup()
        {
            _dummyPin = new Mock<IDigitalOutputPin>(MockBehavior.Strict);
            _mockDelay = new Mock<IDelay>(MockBehavior.Strict);
        }

        [Test]
        public void _01_SHOULD_create_a_valid_key_instance_with_default_values_WHEN_only_DigitalOutputPin_reference_is_provided()
        {
            DummyKey key = new DummyKey(_dummyPin.Object, _mockDelay.Object);

            Assert.That(key.Pin, Is.EqualTo(_dummyPin.Object));
            Assert.That(key.ClickPolarity, Is.False);
            Assert.That(key.ClickDurationTime, Is.EqualTo(100));
            Assert.That(key.ClickReactionTime, Is.EqualTo(100));
            Assert.That(key.DoubleClickDurationTime, Is.EqualTo(50));
        }

        [Test]
        public void _02_SHOULD_create_a_valid_key_instance_with_provided_values_WHEN_all_settings_except_DoubleClickDurationTime_are_provided()
        {
            DummyKey key = new DummyKey(_dummyPin.Object, _mockDelay.Object, true, 200, 600);

            Assert.That(key.Pin, Is.EqualTo(_dummyPin.Object));
            Assert.That(key.ClickPolarity, Is.True);
            Assert.That(key.ClickDurationTime, Is.EqualTo(200));
            Assert.That(key.ClickReactionTime, Is.EqualTo(600));
            Assert.That(key.DoubleClickDurationTime, Is.EqualTo(300));
        }

        [Test]
        public void _03_SHOULD_create_a_valid_key_instance_with_provided_values_WHEN_all_settings_are_provided()
        {
            DummyKey key = new DummyKey(_dummyPin.Object, _mockDelay.Object, true, 200, 600, 800);

            Assert.That(key.Pin, Is.EqualTo(_dummyPin.Object));
            Assert.That(key.ClickPolarity, Is.True);
            Assert.That(key.ClickDurationTime, Is.EqualTo(200));
            Assert.That(key.ClickReactionTime, Is.EqualTo(600));
            Assert.That(key.DoubleClickDurationTime, Is.EqualTo(800));
        }
    }

    [TestFixture]
    public class when_invoking_Click_action_on_a_dummy_key_instance : dummy_key_test_fixture_base
    {
        [Test]
        public void _01_SHOULD_set_the_State_of_the_pin_to_ClickPolarity()
        {
            _dummyPin.SetupSet(pin => pin.State = true).Verifiable();

            _Key.Click();
        }

        [Test]
        public void _02_SHOULD_delay_for_ClickDuration_time_period()
        {
            _mockDelay.Setup(delay => delay.InMilliseconds(200)).Verifiable();

            _Key.Click();
        }

        [Test]
        public void _03_SHOULD_toggle_the_State_back_to_the_original_polarity_after_waiting_For_ClickDuration_period()
        {
            _dummyPin.SetupSet(pin => pin.State = false).Verifiable();

            _Key.Click();
        }

        [Test]
        public void _04_SHOULD_wait_for_ClickReactionTime_period_after_toggling_the_state_back_to_unclicked_state()
        {
            _mockDelay.Setup(delay => delay.InMilliseconds(600)).Verifiable();

            _Key.Click();
        }
    }

    [TestFixture]
    public class when_invokding_DoubleClick_action_on_a_dummy_key_instance : dummy_key_test_fixture_base
    {
        [SetUp]
        public void Setup2()
        {
            _Key = new DummyKey(_dummyPin.Object, _mockDelay.Object, true, 100, 200, 150);
        }

        [Test]
        public void _01_SHOULD_invoke_two_clicks_with_DoubleClickDuration_period()
        {
            _dummyPin.SetupSet(pin => pin.State = true);
            _mockDelay.Setup(delay => delay.InMilliseconds(100)).Verifiable();
            _dummyPin.SetupSet(pin => pin.State = false).Verifiable();
            _mockDelay.Setup(delay => delay.InMilliseconds(150)).Verifiable();
            _dummyPin.SetupSet(pin => pin.State = true);
            _mockDelay.Setup(delay => delay.InMilliseconds(100)).Verifiable();
            _dummyPin.SetupSet(pin => pin.State = false).Verifiable();
            _mockDelay.Setup(delay => delay.InMilliseconds(200)).Verifiable();

            _Key.DoubleClick();
        }
    }

    [TestFixture]
    public class when_invoking_Hold_on_a_dummy_key_instance : dummy_key_test_fixture_base
    {
        [Test]
        public void _01_SHOULD_set_the_pin_state_to_click_polarity_AND_return_immediately()
        {
            _dummyPin.SetupSet(pin => pin.State = true);

            _Key.Hold();
        }
    }

    [TestFixture]
    public class when_invoking_Release_on_a_dummy_key_instance : dummy_key_test_fixture_base
    {
        [Test]
        public void _01_SHOULD_release_the_pin_state_AND_return_immediately()
        {
            _dummyPin.SetupSet(pin => pin.State = false);

            _Key.Release();
        }
    }

    [TestFixture]
    public class when_invoking_HoldFor_on_a_dummy_key_instance : dummy_key_test_fixture_base
    {
        [Test]
        public void _01_SHOULD_Hold_for_specified_time_period_and_release_the_pin_afterwards()
        {
            _dummyPin.SetupSet(pin => pin.State = true);
            _mockDelay.Setup(delay => delay.InMilliseconds(1234));
            _dummyPin.SetupSet(pin => pin.State = false);

            _Key.HoldFor(1234);
        }
    }
}
