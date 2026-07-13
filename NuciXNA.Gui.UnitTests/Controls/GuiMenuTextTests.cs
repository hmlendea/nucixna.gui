using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuTextTests
    {
        GuiMenuText menuText;

        [SetUp]
        public void SetUp()
        {
            menuText = new GuiMenuText();
        }

        // ── IsSelectable ──────────────────────────────────────────────────────

        [Test]
        public void GivenFreshMenuText_WhenGettingIsSelectable_ThenIsFalse()
            => Assert.That(menuText.IsSelectable, Is.False);

        [Test]
        public void GivenMenuText_WhenBaseIsSelectableSetToTrue_ThenIsSelectableRemainsAlwaysFalse()
        {
            ((GuiMenuItem)menuText).IsSelectable = true;

            Assert.That(menuText.IsSelectable, Is.False);
        }
    }
}
