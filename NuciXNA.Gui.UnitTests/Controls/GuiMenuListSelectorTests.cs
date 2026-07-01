using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    public class GuiMenuListSelectorTests
    {
        GuiMenuListSelector selector;

        [SetUp]
        public void SetUp()
        {
            selector = new GuiMenuListSelector();
        }

        // --- SetItems ---

        [Test]
        public void GivenDictionaryItems_WhenSetItems_ThenItemsCountMatches()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            Assert.That(selector.ItemsCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenDictionaryItems_WhenSetItems_ThenSelectedIndexIsZero()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenDictionaryItems_WhenSetItems_ThenSelectedKeyIsFirstKey()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            Assert.That(selector.SelectedKey, Is.EqualTo("a"));
        }

        [Test]
        public void GivenDictionaryItems_WhenSetItems_ThenSelectedValueIsFirstValue()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            Assert.That(selector.SelectedValue, Is.EqualTo("Apple"));
        }

        [Test]
        public void GivenDictionaryItems_WhenSetItems_ThenFiresItemsChanged()
        {
            bool eventFired = false;
            selector.ItemsChanged += delegate { eventFired = true; };

            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.That(eventFired);
        }

        [Test]
        public void GivenDictionaryItems_WhenSetItems_ThenFiresSelectedItemChanged()
        {
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.That(eventFired);
        }

        [Test]
        public void GivenEmptyDictionary_WhenSetItems_ThenSelectedIndexIsMinusOne()
        {
            selector.SetItems(new Dictionary<string, string>());

            Assert.That(selector.SelectedIndex, Is.EqualTo(-1));
        }

        [Test]
        public void GivenEmptyDictionary_WhenSetItems_ThenFiresItemsChanged()
        {
            bool eventFired = false;
            selector.ItemsChanged += delegate { eventFired = true; };

            selector.SetItems(new Dictionary<string, string>());

            Assert.That(eventFired);
        }

        [Test]
        public void GivenEmptyDictionary_WhenSetItems_ThenDoesNotFireSelectedItemChanged()
        {
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SetItems(new Dictionary<string, string>());

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenStringParams_WhenSetItems_ThenItemsCountMatches()
        {
            selector.SetItems("Apple", "Banana", "Cherry");

            Assert.That(selector.ItemsCount, Is.EqualTo(3));
        }

        [Test]
        public void GivenStringParams_WhenSetItems_ThenSelectedValueIsFirst()
        {
            selector.SetItems("Apple", "Banana", "Cherry");

            Assert.That(selector.SelectedValue, Is.EqualTo("Apple"));
        }

        [Test]
        public void GivenEnumerableStrings_WhenSetItems_ThenItemsCountMatches()
        {
            IEnumerable<string> values = ["Apple", "Banana"];
            selector.SetItems(values);

            Assert.That(selector.ItemsCount, Is.EqualTo(2));
        }

        [Test]
        public void GivenKeyValuePairParams_WhenSetItems_ThenSelectedKeyIsFirst()
        {
            selector.SetItems(
                new KeyValuePair<string, string>("x", "Xenon"),
                new KeyValuePair<string, string>("y", "Yellow"));

            Assert.That(selector.SelectedKey, Is.EqualTo("x"));
        }

        // --- SelectItemByIndex ---

        [Test]
        public void GivenItems_WhenSelectItemByIndex_ThenSelectedIndexUpdated()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.SelectItemByIndex(1);

            Assert.That(selector.SelectedIndex, Is.EqualTo(1));
        }

        [Test]
        public void GivenItems_WhenSelectItemByIndex_ThenFiresSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectItemByIndex(1);

            Assert.That(eventFired);
        }

        [Test]
        public void GivenItems_WhenSelectItemByCurrentIndex_ThenDoesNotFireSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectItemByIndex(0);

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenItems_WhenSelectItemByIndexOutOfRange_ThenThrowsIndexOutOfRangeException()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.Throws<IndexOutOfRangeException>(() => selector.SelectItemByIndex(5));
        }

        [Test]
        public void GivenItems_WhenSelectItemByNegativeIndex_ThenThrowsIndexOutOfRangeException()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.Throws<IndexOutOfRangeException>(() => selector.SelectItemByIndex(-1));
        }

        // --- SelectItemByKey ---

        [Test]
        public void GivenItems_WhenSelectItemByKey_ThenSelectedKeyUpdated()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.SelectItemByKey("b");

            Assert.That(selector.SelectedKey, Is.EqualTo("b"));
        }

        [Test]
        public void GivenItems_WhenSelectItemByKey_ThenFiresSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectItemByKey("b");

            Assert.That(eventFired);
        }

        [Test]
        public void GivenItems_WhenSelectItemByCurrentKey_ThenDoesNotFireSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectItemByKey("a");

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenItems_WhenSelectItemByUnknownKey_ThenThrowsArgumentOutOfRangeException()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.Throws<ArgumentOutOfRangeException>(() => selector.SelectItemByKey("z"));
        }

        // --- SelectItemByValue ---

        [Test]
        public void GivenItems_WhenSelectItemByValue_ThenSelectedValueUpdated()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.SelectItemByValue("Banana");

            Assert.That(selector.SelectedValue, Is.EqualTo("Banana"));
        }

        [Test]
        public void GivenItems_WhenSelectItemByValue_ThenFiresSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectItemByValue("Banana");

            Assert.That(eventFired);
        }

        [Test]
        public void GivenItems_WhenSelectItemByCurrentValue_ThenDoesNotFireSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectItemByValue("Apple");

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenItems_WhenSelectItemByUnknownValue_ThenThrowsArgumentOutOfRangeException()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.Throws<ArgumentOutOfRangeException>(() => selector.SelectItemByValue("Zucchini"));
        }

        // --- SelectNextItem ---

        [Test]
        public void GivenItemsAtFirstIndex_WhenSelectNextItem_ThenSelectedIndexIsOne()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana", ["c"] = "Cherry" });

            selector.SelectNextItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(1));
        }

        [Test]
        public void GivenItemsAtLastIndex_WhenSelectNextItem_ThenWrapsToZero()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            selector.SelectItemByIndex(1);

            selector.SelectNextItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenSingleItem_WhenSelectNextItem_ThenIndexStaysZero()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            selector.SelectNextItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenSingleItem_WhenSelectNextItem_ThenDoesNotFireSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectNextItem();

            Assert.That(eventFired, Is.False);
        }

        // --- SelectPreviousItem ---

        [Test]
        public void GivenItemsAtSecondIndex_WhenSelectPreviousItem_ThenSelectedIndexIsZero()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            selector.SelectItemByIndex(1);

            selector.SelectPreviousItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenItemsAtFirstIndex_WhenSelectPreviousItem_ThenWrapsToLast()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.SelectPreviousItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(1));
        }

        [Test]
        public void GivenSingleItem_WhenSelectPreviousItem_ThenIndexStaysZero()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            selector.SelectPreviousItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenSingleItem_WhenSelectPreviousItem_ThenDoesNotFireSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.SelectPreviousItem();

            Assert.That(eventFired, Is.False);
        }

        // --- TrySelectItem(int) ---

        [Test]
        public void GivenItems_WhenTrySelectItemWithValidIndex_ThenSelectedIndexUpdated()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.TrySelectItem(1);

            Assert.That(selector.SelectedIndex, Is.EqualTo(1));
        }

        [Test]
        public void GivenItems_WhenTrySelectItemWithInvalidIndex_ThenDoesNotThrow()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.DoesNotThrow(() => selector.TrySelectItem(99));
        }

        [Test]
        public void GivenItems_WhenTrySelectItemWithInvalidIndex_ThenSelectedIndexUnchanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            selector.TrySelectItem(99);

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        // --- TrySelectItem(string) ---

        [Test]
        public void GivenItems_WhenTrySelectItemWithValidValue_ThenSelectedValueUpdated()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.TrySelectItem("Banana");

            Assert.That(selector.SelectedValue, Is.EqualTo("Banana"));
        }

        [Test]
        public void GivenItems_WhenTrySelectItemWithUnknownValue_ThenDoesNotThrow()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.DoesNotThrow(() => selector.TrySelectItem("Zucchini"));
        }

        [Test]
        public void GivenItems_WhenTrySelectItemWithUnknownValue_ThenSelectedIndexUnchanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            selector.TrySelectItem("Zucchini");

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        // --- GetItems ---

        [Test]
        public void GivenItems_WhenGetItems_ThenReturnsAllItems()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            IDictionary<string, string> result = selector.GetItems();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result["a"], Is.EqualTo("Apple"));
            Assert.That(result["b"], Is.EqualTo("Banana"));
        }

        [Test]
        public void GivenItems_WhenGetItems_ThenReturnsCopy()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            IDictionary<string, string> result = selector.GetItems();
            result["a"] = "Modified";

            Assert.That(selector.SelectedValue, Is.EqualTo("Apple"));
        }

        // --- Keys / Values ---

        [Test]
        public void GivenItems_WhenGettingKeys_ThenContainsAllKeys()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            Assert.That(selector.Keys, Is.EquivalentTo(new[] { "a", "b" }));
        }

        [Test]
        public void GivenItems_WhenGettingValues_ThenContainsAllValues()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            Assert.That(selector.Values, Is.EquivalentTo(new[] { "Apple", "Banana" }));
        }

        // --- SelectedItemChanged args ---

        [Test]
        public void GivenItems_WhenSelectItemByIndex_ThenSelectedItemChangedArgsHaveCorrectIndex()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            ListSelectionEventArgs receivedArgs = null;
            selector.SelectedItemChanged += (_, e) => receivedArgs = (ListSelectionEventArgs)e;

            selector.SelectItemByIndex(1);

            Assert.That(receivedArgs, Is.Not.Null);
            Assert.That(receivedArgs.SelectedIndex, Is.EqualTo(1));
            Assert.That(receivedArgs.SelectedKey, Is.EqualTo("b"));
            Assert.That(receivedArgs.SelectedValue, Is.EqualTo("Banana"));
        }
    }
}
