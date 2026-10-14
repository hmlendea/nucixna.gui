using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuItemExtendedTests
    {
        private GuiMenuItem menuItem;

        [SetUp]
        public void SetUp()
        {
            menuItem = new GuiMenuItem();
        }

        [Test]
        public void GivenFreshMenuItem_WhenGettingText_ThenIsNull()
            => Assert.That(menuItem.Text, Is.Null);

        [Test]
        public void GivenFreshMenuItem_WhenGettingForegroundColour_ThenIsWhite()
            => Assert.That(menuItem.ForegroundColour, Is.EqualTo(Colour.White));

        [Test]
        public void GivenFreshMenuItem_WhenGettingSelectedTextColour_ThenIsGold()
            => Assert.That(menuItem.SelectedTextColour, Is.EqualTo(Colour.Gold));

        [Test]
        public void GivenFreshMenuItem_WhenGettingIsSelectable_ThenIsTrue()
            => Assert.That(menuItem.IsSelectable);

        [Test]
        public void GivenFreshMenuItem_WhenGettingWidth_ThenIs512()
            => Assert.That(menuItem.Size.Width, Is.EqualTo(512));

        [Test]
        public void GivenFreshMenuItem_WhenGettingHeight_ThenIs48()
            => Assert.That(menuItem.Size.Height, Is.EqualTo(48));

        [Test]
        public void GivenMenuItem_WhenSettingTextToEmptyString_ThenTextIsEmptyString()
        {
            menuItem.Text = string.Empty;

            Assert.That(menuItem.Text, Is.Empty);
        }

        [Test]
        public void GivenMenuItem_WhenSettingTextTwice_ThenLastValueWins()
        {
            menuItem.Text = "Dark Souls III";
            menuItem.Text = "Minecraft";

            Assert.That(menuItem.Text, Is.EqualTo("Minecraft"));
        }

        [Test]
        public void GivenMenuItem_WhenSettingTextTwice_ThenTextChangedFiresTwice()
        {
            int fireCount = 0;
            menuItem.TextChanged += delegate { fireCount++; };

            menuItem.Text = "Dark Souls III";
            menuItem.Text = "Minecraft";

            Assert.That(fireCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenMenuItem_WhenSettingTextToNull_ThenTextIsNull()
        {
            menuItem.Text = "Dark Souls III";
            menuItem.Text = null;

            Assert.That(menuItem.Text, Is.Null);
        }

        [Test]
        public void GivenMenuItem_WhenSettingTextToNull_ThenFiresTextChanged()
        {
            menuItem.Text = "Dark Souls III";
            bool eventFired = false;
            menuItem.TextChanged += delegate { eventFired = true; };

            menuItem.Text = null;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenMenuItem_WhenSettingSelectedTextColour_ThenColourIsUpdated()
        {
            menuItem.SelectedTextColour = Colour.Red;

            Assert.That(menuItem.SelectedTextColour, Is.EqualTo(Colour.Red));
        }

        [Test]
        public void GivenMenuItem_WhenSettingSelectedTextColourTwice_ThenLastValueWins()
        {
            menuItem.SelectedTextColour = Colour.Red;
            menuItem.SelectedTextColour = Colour.ChromeYellow;

            Assert.That(menuItem.SelectedTextColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenMenuItem_WhenSettingSelectedTextColourTwice_ThenSelectedTextColourChangedFiresTwice()
        {
            int fireCount = 0;
            menuItem.SelectedTextColourChanged += delegate { fireCount++; };

            menuItem.SelectedTextColour = Colour.Red;
            menuItem.SelectedTextColour = Colour.ChromeYellow;

            Assert.That(fireCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenMenuItem_WhenSettingIsSelectableToFalse_ThenIsSelectableIsFalse()
        {
            menuItem.IsSelectable = false;

            Assert.That(menuItem.IsSelectable, Is.False);
        }

        [Test]
        public void GivenMenuItem_WhenSettingIsSelectableToTrueAfterFalse_ThenIsSelectableIsTrue()
        {
            menuItem.IsSelectable = false;
            menuItem.IsSelectable = true;

            Assert.That(menuItem.IsSelectable);
        }

        [Test]
        public void GivenMenuItem_WhenSettingIsSelectableToSameValueTwice_ThenFiresTwice()
        {
            int fireCount = 0;
            menuItem.IsSelectableChanged += delegate { fireCount++; };

            menuItem.IsSelectable = true;
            menuItem.IsSelectable = true;

            Assert.That(fireCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenMenuItem_WhenTogglingIsSelectableMultipleTimes_ThenEventFiresEachTime()
        {
            int fireCount = 0;
            menuItem.IsSelectableChanged += delegate { fireCount++; };

            menuItem.IsSelectable = false;
            menuItem.IsSelectable = true;
            menuItem.IsSelectable = false;

            Assert.That(fireCount, Is.EqualTo(3));
        }

        [Test]
        public void GivenMenuItem_WhenSettingSize_ThenSizeIsUpdated()
        {
            menuItem.Size = new Size2D(800, 60);

            Assert.That(menuItem.Size.Width, Is.EqualTo(800));
            Assert.That(menuItem.Size.Height, Is.EqualTo(60));
        }

        [Test]
        public void GivenMenuItem_WhenResettingToDefaultSize_ThenSizeMatchesDefault()
        {
            menuItem.Size = new Size2D(800, 60);
            menuItem.Size = new Size2D(512, 48);

            Assert.That(menuItem.Size.Width, Is.EqualTo(512));
            Assert.That(menuItem.Size.Height, Is.EqualTo(48));
        }

        [Test]
        public void GivenMenuItem_WhenSettingForegroundColour_ThenForegroundColourIsUpdated()
        {
            menuItem.ForegroundColour = Colour.ChromeYellow;

            Assert.That(menuItem.ForegroundColour, Is.EqualTo(Colour.ChromeYellow));
        }

        [Test]
        public void GivenMenuItem_WhenSettingForegroundColourTwice_ThenLastValueWins()
        {
            menuItem.ForegroundColour = Colour.Red;
            menuItem.ForegroundColour = Colour.Gold;

            Assert.That(menuItem.ForegroundColour, Is.EqualTo(Colour.Gold));
        }

        [Test]
        public void GivenMenuItem_WhenSettingForegroundColourTwice_ThenForegroundColourChangedFiresTwice()
        {
            int fireCount = 0;
            menuItem.ForegroundColourChanged += delegate { fireCount++; };

            menuItem.ForegroundColour = Colour.Red;
            menuItem.ForegroundColour = Colour.Gold;

            Assert.That(fireCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenMenuItem_WhenSettingForegroundColourToSameValue_ThenFiresEvent()
        {
            int fireCount = 0;
            menuItem.ForegroundColourChanged += delegate { fireCount++; };

            menuItem.ForegroundColour = Colour.White;
            menuItem.ForegroundColour = Colour.White;

            Assert.That(fireCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenMenuItem_WhenTriggeredEventSubscribed_ThenCanBeInvoked()
        {
            bool eventFired = false;
            menuItem.Triggered += delegate { eventFired = true; };

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenMenuItem_WhenClickedEventFires_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnClicked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.MouseButtonEventArgs(NuciXNA.Input.MouseButton.Left, NuciXNA.Input.ButtonState.Pressed, NuciXNA.Primitives.Point2D.Empty)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenMenuItem_WhenKeyPressedEnter_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnKeyPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.KeyboardKeyEventArgs(Microsoft.Xna.Framework.Input.Keys.Enter, NuciXNA.Input.ButtonState.Pressed)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenMenuItem_WhenKeyPressedE_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnKeyPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.KeyboardKeyEventArgs(Microsoft.Xna.Framework.Input.Keys.E, NuciXNA.Input.ButtonState.Pressed)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenMenuItem_WhenKeyPressedOther_ThenTriggeredDoesNotFire()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnKeyPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.KeyboardKeyEventArgs(Microsoft.Xna.Framework.Input.Keys.Space, NuciXNA.Input.ButtonState.Pressed)]);

            Assert.That(triggeredFired, Is.False);
        }

        [Test]
        public void GivenMenuItem_WhenGamepadButtonPressedA_ThenTriggeredFires()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnGamepadButtonPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.GamepadButtonEventArgs(Microsoft.Xna.Framework.Input.Buttons.A, NuciXNA.Input.ButtonState.Pressed, Microsoft.Xna.Framework.PlayerIndex.One)]);

            Assert.That(triggeredFired);
        }

        [Test]
        public void GivenMenuItem_WhenGamepadButtonPressedStart_ThenTriggeredDoesNotFire()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnGamepadButtonPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.GamepadButtonEventArgs(Microsoft.Xna.Framework.Input.Buttons.Start, NuciXNA.Input.ButtonState.Pressed, Microsoft.Xna.Framework.PlayerIndex.One)]);

            Assert.That(triggeredFired, Is.False);
        }

        [Test]
        public void GivenMenuItem_WhenGamepadButtonPressedOther_ThenTriggeredDoesNotFire()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnGamepadButtonPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.GamepadButtonEventArgs(Microsoft.Xna.Framework.Input.Buttons.B, NuciXNA.Input.ButtonState.Pressed, Microsoft.Xna.Framework.PlayerIndex.One)]);

            Assert.That(triggeredFired, Is.False);
        }

        [Test]
        public void GivenMenuItem_WhenMouseEntered_ThenTriggeredDoesNotFire()
        {
            bool triggeredFired = false;
            menuItem.Triggered += delegate { triggeredFired = true; };

            menuItem.GetType()
                .GetMethod("OnMouseEntered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(menuItem, [this, new NuciXNA.Input.MouseEventArgs(NuciXNA.Primitives.Point2D.Empty, NuciXNA.Primitives.Point2D.Empty)]);

            Assert.That(triggeredFired, Is.False);
        }

        [Test]
        public void GivenFreshMenuText_WhenGettingIsSelectable_ThenIsFalse()
        {
            GuiMenuText menuText = new();

            Assert.That(menuText.IsSelectable, Is.False);
        }

        [Test]
        public void GivenMenuText_WhenSettingIsSelectableViaBase_ThenIsSelectableRemainsAlwaysFalse()
        {
            GuiMenuText menuText = new();

            ((GuiMenuItem)menuText).IsSelectable = true;

            Assert.That(menuText.IsSelectable, Is.False);
        }

        [Test]
        public void GivenMenuText_WhenSettingIsSelectableViaBaseMultipleTimes_ThenIsSelectableRemainsAlwaysFalse()
        {
            GuiMenuText menuText = new();

            ((GuiMenuItem)menuText).IsSelectable = true;
            ((GuiMenuItem)menuText).IsSelectable = false;
            ((GuiMenuItem)menuText).IsSelectable = true;

            Assert.That(menuText.IsSelectable, Is.False);
        }
    }
}
