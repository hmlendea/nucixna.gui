using System;

using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuToggleTests
    {
        GuiMenuToggle toggle;

        [SetUp]
        public void SetUp()
        {
            toggle = new GuiMenuToggle();
        }

        [Test]
        public void GivenFreshToggle_WhenGettingIsOn_ThenIsFalse()
            => Assert.That(toggle.IsOn, Is.False);

        [Test]
        public void GivenToggleOff_WhenSwitchOn_ThenIsOnIsTrue()
        {
            toggle.SwitchOn();

            Assert.That(toggle.IsOn);
        }

        [Test]
        public void GivenToggleOn_WhenSwitchOff_ThenIsOnIsFalse()
        {
            toggle.SwitchOn();
            toggle.SwitchOff();

            Assert.That(toggle.IsOn, Is.False);
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
        public void GivenToggleOff_WhenSetStateFalse_ThenDoesNotFireStateChanged()
        {
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SetState(false);

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenToggleOn_WhenSetStateTrue_ThenDoesNotFireStateChanged()
        {
            toggle.SwitchOn();
            bool eventFired = false;
            toggle.StateChanged += delegate { eventFired = true; };

            toggle.SetState(true);

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenToggleOff_WhenSwitchOnTwice_ThenStateChangedFiresOnce()
        {
            int fireCount = 0;
            toggle.StateChanged += delegate { fireCount++; };

            toggle.SwitchOn();
            toggle.SwitchOn();

            Assert.That(fireCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenToggle_WhenSwitchStateMultipleTimes_ThenIsOnAlternates()
        {
            toggle.SwitchState();
            Assert.That(toggle.IsOn, Is.True);

            toggle.SwitchState();
            Assert.That(toggle.IsOn, Is.False);

            toggle.SwitchState();
            Assert.That(toggle.IsOn, Is.True);
        }
    }
}
