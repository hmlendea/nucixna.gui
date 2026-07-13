using System;

using NuciXNA.Primitives;

using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuItemTests
    {
        GuiMenuItem menuItem;

        [SetUp]
        public void SetUp()
        {
            menuItem = new GuiMenuItem();
        }

        [Test]
        public void GivenFreshMenuItem_WhenGettingIsSelectable_ThenIsTrue()
            => Assert.That(menuItem.IsSelectable, Is.True);

        [Test]
        public void GivenFreshMenuItem_WhenGettingSelectedTextColour_ThenIsGold()
            => Assert.That(menuItem.SelectedTextColour, Is.EqualTo(Colour.Gold));

        [Test]
        public void GivenFreshMenuItem_WhenGettingForegroundColour_ThenIsWhite()
            => Assert.That(menuItem.ForegroundColour, Is.EqualTo(Colour.White));

        [Test]
        public void GivenMenuItem_WhenSettingText_ThenTextIsUpdated()
        {
            menuItem.Text = "Options";

            Assert.That(menuItem.Text, Is.EqualTo("Options"));
        }

        [Test]
        public void GivenMenuItem_WhenSettingText_ThenFiresTextChanged()
        {
            bool eventFired = false;
            menuItem.TextChanged += delegate { eventFired = true; };

            menuItem.Text = "Options";

            Assert.That(eventFired);
        }

        [Test]
        public void GivenMenuItem_WhenSettingSelectedTextColour_ThenColourIsUpdated()
        {
            menuItem.SelectedTextColour = Colour.Red;

            Assert.That(menuItem.SelectedTextColour, Is.EqualTo(Colour.Red));
        }

        [Test]
        public void GivenMenuItem_WhenSettingSelectedTextColour_ThenFiresSelectedTextColourChanged()
        {
            bool eventFired = false;
            menuItem.SelectedTextColourChanged += delegate { eventFired = true; };

            menuItem.SelectedTextColour = Colour.Red;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenSelectableMenuItem_WhenSetIsSelectableFalse_ThenIsSelectableIsFalse()
        {
            menuItem.IsSelectable = false;

            Assert.That(menuItem.IsSelectable, Is.False);
        }

        [Test]
        public void GivenNonSelectableMenuItem_WhenSetIsSelectableTrue_ThenIsSelectableIsTrue()
        {
            menuItem.IsSelectable = false;
            menuItem.IsSelectable = true;

            Assert.That(menuItem.IsSelectable, Is.True);
        }

        [Test]
        public void GivenMenuItem_WhenSettingIsSelectable_ThenFiresIsSelectableChanged()
        {
            bool eventFired = false;
            menuItem.IsSelectableChanged += delegate { eventFired = true; };

            menuItem.IsSelectable = false;

            Assert.That(eventFired);
        }

        [Test]
        public void GivenMenuItem_WhenSettingIsSelectableToSameValue_ThenFiresIsSelectableChanged()
        {
            bool eventFired = false;
            menuItem.IsSelectableChanged += delegate { eventFired = true; };

            menuItem.IsSelectable = true;

            Assert.That(eventFired);
        }
    }
}
