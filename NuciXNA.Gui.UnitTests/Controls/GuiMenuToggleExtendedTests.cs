using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuToggleExtendedTests
    {
        GuiMenuToggle toggle;

        [SetUp]
        public void SetUp()
        {
            toggle = new GuiMenuToggle();
        }

        // ── Initial state ──────────────────────────────────────────────────────

        [Test]
        public void GivenFreshToggle_WhenGettingIsOn_ThenIsFalse()
            => Assert.That(toggle.IsOn, Is.False);

        // ── SetState – idempotency ─────────────────────────────────────────────

        [Test]
        public void GivenToggleOff_WhenSetStateFalse_ThenIsOnRemainsOff()
        {
            toggle.SetState(false);

            Assert.That(toggle.IsOn, Is.False);
        }

        [Test]
        public void GivenToggleOn_WhenSetStateTrue_ThenIsOnRemainsOn()
        {
            toggle.SwitchOn();
            toggle.SetState(true);

            Assert.That(toggle.IsOn);
        }

        [Test]
        public void GivenToggleOff_WhenSetStateFalseRepeatedly_ThenStateChangedNeverFires()
        {
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SetState(false);
            toggle.SetState(false);
            toggle.SetState(false);

            Assert.That(fireCount, Is.EqualTo(0));
        }

        [Test]
        public void GivenToggleOn_WhenSetStateTrueRepeatedly_ThenStateChangedNeverFires()
        {
            toggle.SwitchOn();
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SetState(true);
            toggle.SetState(true);
            toggle.SetState(true);

            Assert.That(fireCount, Is.EqualTo(0));
        }

        // ── SwitchOn ───────────────────────────────────────────────────────────

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

        // ── SwitchOff ──────────────────────────────────────────────────────────

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

        // ── SwitchState ────────────────────────────────────────────────────────

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

        [Test]
        public void GivenToggleOff_WhenSwitchStateFourTimes_ThenIsOnIsFalse()
        {
            toggle.SwitchState();
            toggle.SwitchState();
            toggle.SwitchState();
            toggle.SwitchState();

            Assert.That(toggle.IsOn, Is.False);
        }

        [Test]
        public void GivenToggleOff_WhenSwitchStateFiveTimes_ThenIsOnIsTrue()
        {
            toggle.SwitchState();
            toggle.SwitchState();
            toggle.SwitchState();
            toggle.SwitchState();
            toggle.SwitchState();

            Assert.That(toggle.IsOn);
        }

        [Test]
        public void GivenToggleOff_WhenSwitchStateFourTimes_ThenStateChangedFiresFourTimes()
        {
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SwitchState();
            toggle.SwitchState();
            toggle.SwitchState();
            toggle.SwitchState();

            Assert.That(fireCount, Is.EqualTo(4));
        }

        // ── SetState – transitions ─────────────────────────────────────────────

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

        // ── Complex sequences ──────────────────────────────────────────────────

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
