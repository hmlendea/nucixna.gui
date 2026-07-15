using System;

using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.UnitTests.Helpers;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuLinkExtendedTests
    {
        GuiMenuLink menuLink;

        [SetUp]
        public void SetUp()
        {
            menuLink = new GuiMenuLink();
        }

        // ── Default state ──────────────────────────────────────────────────────

        [Test]
        public void GivenFreshMenuLink_WhenGettingTargetScreen_ThenIsNull()
            => Assert.That(menuLink.TargetScreen, Is.Null);

        [Test]
        public void GivenFreshMenuLink_WhenGettingParameters_ThenIsNull()
            => Assert.That(menuLink.Parameters, Is.Null);

        [Test]
        public void GivenFreshMenuLink_WhenGettingIsSelectable_ThenIsTrue()
            => Assert.That(menuLink.IsSelectable);

        // ── TargetScreen ───────────────────────────────────────────────────────

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

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreenTwice_ThenLastValueWins()
        {
            menuLink.TargetScreen = typeof(DummyScreen);
            menuLink.TargetScreen = typeof(Exception);

            Assert.That(menuLink.TargetScreen, Is.EqualTo(typeof(Exception)));
        }

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreenTwice_ThenTargetScreenChangedFiresTwice()
        {
            int fireCount = 0;
            menuLink.TargetScreenChanged += delegate { fireCount++; };

            menuLink.TargetScreen = typeof(DummyScreen);
            menuLink.TargetScreen = typeof(Exception);

            Assert.That(fireCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreenToNull_ThenTargetScreenIsNull()
        {
            menuLink.TargetScreen = typeof(DummyScreen);
            menuLink.TargetScreen = null;

            Assert.That(menuLink.TargetScreen, Is.Null);
        }

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreenToNull_ThenFiresTargetScreenChanged()
        {
            menuLink.TargetScreen = typeof(DummyScreen);
            bool eventFired = false;
            menuLink.TargetScreenChanged += delegate { eventFired = true; };

            menuLink.TargetScreen = null;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreenToSameTypeTwice_ThenFiresTwice()
        {
            int fireCount = 0;
            menuLink.TargetScreenChanged += delegate { fireCount++; };

            menuLink.TargetScreen = typeof(DummyScreen);
            menuLink.TargetScreen = typeof(DummyScreen);

            Assert.That(fireCount, Is.EqualTo(2));
        }

        // ── Parameters ────────────────────────────────────────────────────────

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

        [Test]
        public void GivenMenuLink_WhenSettingParametersTwice_ThenLastValueWins()
        {
            menuLink.Parameters = ["Cornova", 613];
            menuLink.Parameters = ["Horidava", 873];

            Assert.That(menuLink.Parameters[0], Is.EqualTo("Horidava"));
            Assert.That(menuLink.Parameters[1], Is.EqualTo(873));
        }

        [Test]
        public void GivenMenuLink_WhenSettingParametersTwice_ThenParametersChangedFiresTwice()
        {
            int fireCount = 0;
            menuLink.ParametersChanged += delegate { fireCount++; };

            menuLink.Parameters = ["Cornova", 613];
            menuLink.Parameters = ["Horidava", 873];

            Assert.That(fireCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenMenuLink_WhenSettingParametersToNull_ThenParametersIsNull()
        {
            menuLink.Parameters = ["Cornova", 613];
            menuLink.Parameters = null;

            Assert.That(menuLink.Parameters, Is.Null);
        }

        [Test]
        public void GivenMenuLink_WhenSettingParametersToNull_ThenFiresParametersChanged()
        {
            menuLink.Parameters = ["Cornova", 613];
            bool eventFired = false;
            menuLink.ParametersChanged += delegate { eventFired = true; };

            menuLink.Parameters = null;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenMenuLink_WhenSettingParametersToEmptyArray_ThenParametersIsEmpty()
        {
            menuLink.Parameters = [];

            Assert.That(menuLink.Parameters, Is.Empty);
        }

        [Test]
        public void GivenMenuLink_WhenSettingParametersToEmptyArray_ThenFiresParametersChanged()
        {
            bool eventFired = false;
            menuLink.ParametersChanged += delegate { eventFired = true; };

            menuLink.Parameters = [];

            Assert.That(eventFired);
        }

        [Test]
        public void GivenMenuLink_WhenSettingParametersWithMixedTypes_ThenParametersAreStoredCorrectly()
        {
            object[] parameters = ["Solaire of Astora", 613, true, 3.14];
            menuLink.Parameters = parameters;

            Assert.That(menuLink.Parameters[0], Is.EqualTo("Solaire of Astora"));
            Assert.That(menuLink.Parameters[1], Is.EqualTo(613));
            Assert.That(menuLink.Parameters[2], Is.EqualTo(true));
            Assert.That(menuLink.Parameters[3], Is.EqualTo(3.14));
        }

        // ── Inherited MenuItem properties ──────────────────────────────────────

        [Test]
        public void GivenFreshMenuLink_WhenGettingForegroundColour_ThenIsWhite()
            => Assert.That(menuLink.ForegroundColour, Is.EqualTo(Colour.White));

        [Test]
        public void GivenFreshMenuLink_WhenGettingSelectedTextColour_ThenIsGold()
            => Assert.That(menuLink.SelectedTextColour, Is.EqualTo(Colour.Gold));

        [Test]
        public void GivenFreshMenuLink_WhenGettingText_ThenIsNull()
            => Assert.That(menuLink.Text, Is.Null);

        [Test]
        public void GivenMenuLink_WhenSettingText_ThenFiresTextChanged()
        {
            bool eventFired = false;
            menuLink.TextChanged += delegate { eventFired = true; };

            menuLink.Text = "Nucilandia";

            Assert.That(eventFired);
        }

        [Test]
        public void GivenMenuLink_WhenSettingText_ThenTextIsUpdated()
        {
            menuLink.Text = "Nucilandia";

            Assert.That(menuLink.Text, Is.EqualTo("Nucilandia"));
        }
    }
}
