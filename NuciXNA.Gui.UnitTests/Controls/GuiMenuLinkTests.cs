using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.UnitTests.Helpers;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuLinkTests
    {
        GuiMenuLink menuLink;

        [SetUp]
        public void SetUp()
        {
            menuLink = new GuiMenuLink();
        }

        // ── TargetScreen ──────────────────────────────────────────────────────

        [Test]
        public void GivenFreshMenuLink_WhenGettingTargetScreen_ThenIsNull()
            => Assert.That(menuLink.TargetScreen, Is.Null);

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreen_ThenTargetScreenIsUpdated()
        {
            menuLink.TargetScreen = typeof(DummyScreen);

            Assert.That(menuLink.TargetScreen, Is.EqualTo(typeof(DummyScreen)));
        }

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreen_ThenFiresTargetScreenChanged()
        {
            bool eventFired = false;
            menuLink.TargetScreenChanged += delegate { eventFired = true; };

            menuLink.TargetScreen = typeof(DummyScreen);

            Assert.That(eventFired);
        }

        // ── Parameters ────────────────────────────────────────────────────────

        [Test]
        public void GivenFreshMenuLink_WhenGettingParameters_ThenIsNull()
            => Assert.That(menuLink.Parameters, Is.Null);

        [Test]
        public void GivenMenuLink_WhenSettingParameters_ThenParametersIsUpdated()
        {
            object[] parameters = ["Cornova", 613];

            menuLink.Parameters = parameters;

            Assert.That(menuLink.Parameters, Is.EqualTo(parameters));
        }

        [Test]
        public void GivenMenuLink_WhenSettingParameters_ThenFiresParametersChanged()
        {
            bool eventFired = false;
            menuLink.ParametersChanged += delegate { eventFired = true; };

            menuLink.Parameters = ["Cornova", 613];

            Assert.That(eventFired);
        }
    }
}
