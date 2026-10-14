using System.Reflection;

using Microsoft.Xna.Framework.Input;

using NUnit.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.UnitTests.Helpers;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuLinkTests
    {
        private GuiMenuLink menuLink;

        [SetUp]
        public void SetUp()
        {
            menuLink = new GuiMenuLink();
        }

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

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreenTwice_ThenLastValueWins()
        {
            menuLink.TargetScreen = typeof(DummyScreen);
            menuLink.TargetScreen = typeof(DummyScreen);

            Assert.That(menuLink.TargetScreen, Is.EqualTo(typeof(DummyScreen)));
        }

        [Test]
        public void GivenMenuLink_WhenSettingTargetScreenToNull_ThenTargetScreenIsNull()
        {
            menuLink.TargetScreen = typeof(DummyScreen);
            menuLink.TargetScreen = null;

            Assert.That(menuLink.TargetScreen, Is.Null);
        }

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

        [Test]
        public void GivenMenuLink_WhenSettingParametersTwice_ThenLastValueWins()
        {
            menuLink.Parameters = ["Cornova", 613];
            menuLink.Parameters = ["New", "Values"];

            Assert.That(menuLink.Parameters, Is.EqualTo(new object[] { "New", "Values" }));
        }

        [Test]
        public void GivenMenuLink_WhenSettingParametersToNull_ThenParametersIsNull()
        {
            menuLink.Parameters = ["Cornova", 613];
            menuLink.Parameters = null;

            Assert.That(menuLink.Parameters, Is.Null);
        }

        [Test]
        public void GivenMenuLink_WhenTriggeredEventSubscribed_ThenCanBeInvoked()
        {
            bool eventFired = false;
            menuLink.Triggered += delegate { eventFired = true; };

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenMenuLink_WhenClickedEventFires_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuLink.Triggered += delegate { triggeredFired = true; };

            typeof(GuiMenuItem)
                .GetMethod("OnClicked", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(menuLink, [this, new MouseButtonEventArgs(MouseButton.Left, Input.ButtonState.Pressed, Point2D.Empty)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenMenuLink_WhenKeyPressedEnter_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuLink.Triggered += delegate { triggeredFired = true; };

            typeof(GuiMenuItem)
                .GetMethod("OnKeyPressed", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(menuLink, [this, new KeyboardKeyEventArgs(Keys.Enter, Input.ButtonState.Pressed)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenMenuLink_WhenKeyPressedE_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuLink.Triggered += delegate { triggeredFired = true; };

            typeof(GuiMenuItem)
                .GetMethod("OnKeyPressed", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(menuLink, [this, new KeyboardKeyEventArgs(Keys.E, Input.ButtonState.Pressed)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenMenuLink_WhenGamepadButtonPressedA_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuLink.Triggered += delegate { triggeredFired = true; };

            typeof(GuiMenuItem)
                .GetMethod("OnGamepadButtonPressed", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(menuLink, [this, new GamepadButtonEventArgs(Buttons.A, Input.ButtonState.Pressed, Microsoft.Xna.Framework.PlayerIndex.One)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenFreshMenuLink_WhenGettingIsSelectable_ThenIsTrue()
            => Assert.That(menuLink.IsSelectable, Is.True);

        [Test]
        public void GivenFreshMenuLink_WhenGettingSelectedTextColour_ThenIsGold()
            => Assert.That(menuLink.SelectedTextColour, Is.EqualTo(Colour.Gold));

        [Test]
        public void GivenFreshMenuLink_WhenGettingForegroundColour_ThenIsWhite()
            => Assert.That(menuLink.ForegroundColour, Is.EqualTo(Colour.White));

        [Test]
        public void GivenFreshMenuLink_WhenGettingSize_ThenIs512By48()
        {
            Assert.That(menuLink.Size.Width, Is.EqualTo(512));
            Assert.That(menuLink.Size.Height, Is.EqualTo(48));
        }

        [Test]
        public void GivenFreshMenuLink_WhenGettingText_ThenIsNull()
            => Assert.That(menuLink.Text, Is.Null);
    }
}
