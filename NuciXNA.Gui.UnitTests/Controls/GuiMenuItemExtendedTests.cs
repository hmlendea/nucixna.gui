using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuItemExtendedTests
    {
        GuiMenuItem menuItem;

        [SetUp]
        public void SetUp()
        {
            menuItem = new GuiMenuItem();
        }

        // ── Default property values ────────────────────────────────────────────

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

        // ── Text ───────────────────────────────────────────────────────────────

        [Test]
        public void GivenMenuItem_WhenSettingTextToEmptyString_ThenTextIsEmptyString()
        {
            menuItem.Text = string.Empty;

            Assert.That(menuItem.Text, Is.EqualTo(string.Empty));
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

        // ── SelectedTextColour ─────────────────────────────────────────────────

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

        // ── IsSelectable ───────────────────────────────────────────────────────

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

        // ── Size ───────────────────────────────────────────────────────────────

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

        // ── ForegroundColour ───────────────────────────────────────────────────

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

        // ── GuiMenuText specifics ──────────────────────────────────────────────

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
