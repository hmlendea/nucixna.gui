using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    public class ListSelectionEventArgsTests
    {
        [Test]
        public void GivenIndexKeyValue_WhenCreated_ThenSelectedIndexMatches()
        {
            ListSelectionEventArgs args = new(3, "key", "value");

            Assert.That(args.SelectedIndex, Is.EqualTo(3));
        }

        [Test]
        public void GivenIndexKeyValue_WhenCreated_ThenSelectedKeyMatches()
        {
            ListSelectionEventArgs args = new(3, "myKey", "value");

            Assert.That(args.SelectedKey, Is.EqualTo("myKey"));
        }

        [Test]
        public void GivenIndexKeyValue_WhenCreated_ThenSelectedValueMatches()
        {
            ListSelectionEventArgs args = new(3, "key", "myValue");

            Assert.That(args.SelectedValue, Is.EqualTo("myValue"));
        }

        [Test]
        public void GivenCreatedArgs_WhenSettingSelectedIndex_ThenSelectedIndexUpdated()
        {
            ListSelectionEventArgs args = new(0, "key", "value");

            args.SelectedIndex = 7;

            Assert.That(args.SelectedIndex, Is.EqualTo(7));
        }

        [Test]
        public void GivenCreatedArgs_WhenSettingSelectedKey_ThenSelectedKeyUpdated()
        {
            ListSelectionEventArgs args = new(0, "key", "value");

            args.SelectedKey = "newKey";

            Assert.That(args.SelectedKey, Is.EqualTo("newKey"));
        }

        [Test]
        public void GivenCreatedArgs_WhenSettingSelectedValue_ThenSelectedValueUpdated()
        {
            ListSelectionEventArgs args = new(0, "key", "value");

            args.SelectedValue = "newValue";

            Assert.That(args.SelectedValue, Is.EqualTo("newValue"));
        }
    }
}
