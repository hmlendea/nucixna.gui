using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuToggleExtendedTests
    {
        private GuiMenuToggle toggle;

        [SetUp]
        public void SetUp()
        {
            toggle = new GuiMenuToggle();
        }


        [Test]
        public void GivenFreshToggle_WhenGettingIsOn_ThenIsFalse()
            => Assert.That(toggle.IsOn, Is.False);


        [TestCase(false, false, false)]
        [TestCase(true, true, true)]
        [TestCase(false, true, true)]
        [TestCase(true, false, false)]
        public void GivenToggleWithInitialState_WhenSetState_ThenIsOnMatchesExpectedState(
            bool initialState,
            bool state,
            bool expectedState)
        {
            toggle.SetState(initialState);
            toggle.SetState(state);

            Assert.That(toggle.IsOn, Is.EqualTo(expectedState));
        }

        [TestCase(false, false, 0)]
        [TestCase(true, true, 0)]
        [TestCase(false, true, 1)]
        [TestCase(true, false, 1)]
        public void GivenToggleWithInitialState_WhenSetState_ThenStateChangedFiresExpectedNumberOfTimes(
            bool initialState,
            bool state,
            int expectedFireCount)
        {
            toggle.SetState(initialState);

            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SetState(state);

            Assert.That(fireCount, Is.EqualTo(expectedFireCount));
        }


        [Test]
        public void GivenToggleOff_WhenSwitchOn_ThenIsOnIsTrue()
        {
            toggle.SwitchOn();

            Assert.That(toggle.IsOn);
        }

        [Test]
        public void GivenToggleOn_WhenSwitchOn_ThenIsOnRemainsTrue()
        {
            toggle.SwitchOn();
            toggle.SwitchOn();

            Assert.That(toggle.IsOn);
        }

        [Test]
        public void GivenToggleOff_WhenSwitchOn_ThenFiresStateChanged()
        {
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SwitchOn();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenToggleOn_WhenSwitchOnAgain_ThenDoesNotFireStateChanged()
        {
            toggle.SwitchOn();
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SwitchOn();

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenToggleOff_WhenSwitchOnThreeTimes_ThenStateChangedFiresOnce()
        {
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SwitchOn();
            toggle.SwitchOn();
            toggle.SwitchOn();

            Assert.That(fireCount, Is.EqualTo(1));
        }


        [Test]
        public void GivenToggleOn_WhenSwitchOff_ThenIsOnIsFalse()
        {
            toggle.SwitchOn();
            toggle.SwitchOff();

            Assert.That(toggle.IsOn, Is.False);
        }

        [Test]
        public void GivenToggleOff_WhenSwitchOff_ThenIsOnRemainsFalse()
        {
            toggle.SwitchOff();

            Assert.That(toggle.IsOn, Is.False);
        }

        [Test]
        public void GivenToggleOn_WhenSwitchOff_ThenFiresStateChanged()
        {
            toggle.SwitchOn();
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SwitchOff();

            Assert.That(eventFired);
        }

        [Test]
        public void GivenToggleOff_WhenSwitchOff_ThenDoesNotFireStateChanged()
        {
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SwitchOff();

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenToggleOn_WhenSwitchOffThreeTimes_ThenStateChangedFiresOnce()
        {
            toggle.SwitchOn();
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SwitchOff();
            toggle.SwitchOff();
            toggle.SwitchOff();

            Assert.That(fireCount, Is.EqualTo(1));
        }


        [Test]
        public void GivenToggleOff_WhenSwitchState_ThenIsOnIsTrue()
        {
            toggle.SwitchState();

            Assert.That(toggle.IsOn);
        }

        [Test]
        public void GivenToggleOn_WhenSwitchState_ThenIsOnIsFalse()
        {
            toggle.SwitchOn();
            toggle.SwitchState();

            Assert.That(toggle.IsOn, Is.False);
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public void GivenToggleOff_WhenSwitchStateRepeatedly_ThenIsOnMatchesParity(
            int switchCount,
            bool expectedState)
        {
            for (int iteration = 0; iteration < switchCount; iteration++)
            {
                toggle.SwitchState();
            }

            Assert.That(toggle.IsOn, Is.EqualTo(expectedState));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(5)]
        public void GivenToggleOff_WhenSwitchStateRepeatedly_ThenStateChangedFiresOncePerSwitch(int switchCount)
        {
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            for (int iteration = 0; iteration < switchCount; iteration++)
            {
                toggle.SwitchState();
            }

            Assert.That(fireCount, Is.EqualTo(switchCount));
        }


        [Test]
        public void GivenToggleOff_WhenSetStateTrue_ThenIsOnIsTrue()
        {
            toggle.SetState(true);

            Assert.That(toggle.IsOn);
        }

        [Test]
        public void GivenToggleOn_WhenSetStateFalse_ThenIsOnIsFalse()
        {
            toggle.SwitchOn();
            toggle.SetState(false);

            Assert.That(toggle.IsOn, Is.False);
        }

        [Test]
        public void GivenToggleOff_WhenSetStateTrue_ThenFiresStateChanged()
        {
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SetState(true);

            Assert.That(eventFired);
        }

        [Test]
        public void GivenToggleOn_WhenSetStateFalse_ThenFiresStateChanged()
        {
            toggle.SwitchOn();
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SetState(false);

            Assert.That(eventFired);
        }


        [Test]
        public void GivenToggle_WhenOnOffOnOff_ThenStateChangedFiresFourTimes()
        {
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SwitchOn();
            toggle.SwitchOff();
            toggle.SwitchOn();
            toggle.SwitchOff();

            Assert.That(fireCount, Is.EqualTo(4));
        }

        [Test]
        public void GivenToggle_WhenMixOfSwitchAndSetState_ThenFinalStateIsCorrect()
        {
            toggle.SwitchOn();
            toggle.SetState(false);
            toggle.SwitchState();
            toggle.SwitchOff();

            Assert.That(toggle.IsOn, Is.False);
        }

        [Test]
        public void GivenToggle_WhenIsSelectableAlwaysInherited_ThenIsSelectableIsTrue()
        {
            Assert.That(toggle.IsSelectable);
        }
    }
}
