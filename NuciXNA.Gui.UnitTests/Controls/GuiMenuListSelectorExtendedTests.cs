using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Controls
{
    [TestFixture]
    public class GuiMenuListSelectorExtendedTests
    {
        GuiMenuListSelector selector;

        [SetUp]
        public void SetUp()
        {
            selector = new GuiMenuListSelector();
        }

        // ── Initial state ──────────────────────────────────────────────────────

        [Test]
        public void GivenFreshSelector_WhenGettingSelectedIndex_ThenIsMinusOne()
            => Assert.That(selector.SelectedIndex, Is.EqualTo(-1));

        [Test]
        public void GivenFreshSelector_WhenGettingItemsCount_ThenIsZero()
            => Assert.That(selector.ItemsCount, Is.EqualTo(0));

        [Test]
        public void GivenFreshSelector_WhenGettingSelectedKey_ThenIsNull()
            => Assert.That(selector.SelectedKey, Is.Null);

        [Test]
        public void GivenFreshSelector_WhenGettingSelectedValue_ThenIsNull()
            => Assert.That(selector.SelectedValue, Is.Null);

        [Test]
        public void GivenFreshSelector_WhenGettingKeys_ThenIsEmpty()
            => Assert.That(selector.Keys, Is.Empty);

        [Test]
        public void GivenFreshSelector_WhenGettingValues_ThenIsEmpty()
            => Assert.That(selector.Values, Is.Empty);

        // ── SetItems – replaces previous collection ────────────────────────────

        [Test]
        public void GivenItemsAlreadySet_WhenSetItemsCalledAgain_ThenItemsCountMatchesNewCollection()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana", ["c"] = "Cherry" });
            selector.SetItems(new Dictionary<string, string> { ["x"] = "Xenon" });

            Assert.That(selector.ItemsCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenItemsAlreadySet_WhenSetItemsCalledAgain_ThenSelectedIndexResetToZero()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            selector.SelectItemByIndex(1);
            selector.SetItems(new Dictionary<string, string> { ["x"] = "Xenon" });

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenItemsAlreadySet_WhenSetItemsCalledAgain_ThenSelectedKeyMatchesNewFirst()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            selector.SetItems(new Dictionary<string, string> { ["nuc"] = "Nucilandia" });

            Assert.That(selector.SelectedKey, Is.EqualTo("nuc"));
        }

        [Test]
        public void GivenItemsAlreadySet_WhenSetItemsCalledWithEmpty_ThenSelectedIndexIsMinusOne()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });
            selector.SetItems(new Dictionary<string, string>());

            Assert.That(selector.SelectedIndex, Is.EqualTo(-1));
        }

        [Test]
        public void GivenItemsAlreadySet_WhenSetItemsCalledWithEmpty_ThenSelectedKeyIsNull()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });
            selector.SetItems(new Dictionary<string, string>());

            Assert.That(selector.SelectedKey, Is.Null);
        }

        [Test]
        public void GivenItemsAlreadySet_WhenSetItemsCalledWithEmpty_ThenSelectedValueIsNull()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });
            selector.SetItems(new Dictionary<string, string>());

            Assert.That(selector.SelectedValue, Is.Null);
        }

        [Test]
        public void GivenSetItemsCalledTwice_WhenSecondCallHasItems_ThenItemsChangedFiresTwice()
        {
            int fireCount = 0;
            selector.ItemsChanged += delegate { fireCount++; };

            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });
            selector.SetItems(new Dictionary<string, string> { ["b"] = "Banana" });

            Assert.That(fireCount, Is.EqualTo(2));
        }

        // ── SetItems – string params overload ──────────────────────────────────

        [Test]
        public void GivenStringParams_WhenSetItemsWithOneValue_ThenItemsCountIsOne()
        {
            selector.SetItems("Dark Souls III");

            Assert.That(selector.ItemsCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenStringParams_WhenSetItemsWithFiveValues_ThenItemsCountIsFive()
        {
            selector.SetItems("Minecraft", "Terraria", "RuneScape", "Narivia", "SokoGrump");

            Assert.That(selector.ItemsCount, Is.EqualTo(5));
        }

        [Test]
        public void GivenStringParams_WhenSetItems_ThenAllValuesArePresent()
        {
            selector.SetItems("Minecraft", "Terraria", "RuneScape");

            Assert.That(selector.Values, Is.EquivalentTo(new[] { "Minecraft", "Terraria", "RuneScape" }));
        }

        [Test]
        public void GivenStringEnumerable_WhenSetItems_ThenSelectedValueIsFirst()
        {
            IEnumerable<string> values = ["Dark Souls III", "Minecraft"];
            selector.SetItems(values);

            Assert.That(selector.SelectedValue, Is.EqualTo("Dark Souls III"));
        }

        // ── SetItems – KeyValuePair params overload ────────────────────────────

        [Test]
        public void GivenKeyValuePairParams_WhenSetItems_ThenAllKeysArePresent()
        {
            selector.SetItems(
                new KeyValuePair<string, string>("ds3", "Dark Souls III"),
                new KeyValuePair<string, string>("mc", "Minecraft"),
                new KeyValuePair<string, string>("rs", "RuneScape"));

            Assert.That(selector.Keys, Is.EquivalentTo(new[] { "ds3", "mc", "rs" }));
        }

        [Test]
        public void GivenKeyValuePairParams_WhenSetItems_ThenSelectedIndexIsZero()
        {
            selector.SetItems(
                new KeyValuePair<string, string>("x", "Xenon"),
                new KeyValuePair<string, string>("y", "Yellow"));

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenKeyValuePairEnumerable_WhenSetItems_ThenItemsCountMatches()
        {
            IEnumerable<KeyValuePair<string, string>> items =
            [
                new("a", "Apple"),
                new("b", "Banana"),
                new("c", "Cherry")
            ];

            selector.SetItems(items);

            Assert.That(selector.ItemsCount, Is.EqualTo(3));
        }

        // ── SelectItemByIndex – key and value cache consistency ────────────────

        [Test]
        public void GivenItems_WhenSelectItemByIndex_ThenSelectedKeyIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["solara"] = "Solara", ["nuc"] = "Nucilandia" });

            selector.SelectItemByIndex(1);

            Assert.That(selector.SelectedKey, Is.EqualTo("nuc"));
        }

        [Test]
        public void GivenItems_WhenSelectItemByIndex_ThenSelectedValueIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["solara"] = "Solara", ["nuc"] = "Nucilandia" });

            selector.SelectItemByIndex(1);

            Assert.That(selector.SelectedValue, Is.EqualTo("Nucilandia"));
        }

        [Test]
        public void GivenThreeItems_WhenSelectMiddleItemByIndex_ThenCacheIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string>
            {
                ["a"] = "Apple",
                ["b"] = "Banana",
                ["c"] = "Cherry"
            });

            selector.SelectItemByIndex(1);

            Assert.That(selector.SelectedKey, Is.EqualTo("b"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Banana"));
        }

        [Test]
        public void GivenFiveItems_WhenSelectLastItemByIndex_ThenCacheIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string>
            {
                ["a"] = "Apple",
                ["b"] = "Banana",
                ["c"] = "Cherry",
                ["d"] = "Date",
                ["e"] = "Elderberry"
            });

            selector.SelectItemByIndex(4);

            Assert.That(selector.SelectedKey, Is.EqualTo("e"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Elderberry"));
        }

        [Test]
        public void GivenItems_WhenSelectItemByIndexZero_ThenCacheMatchesFirstItem()
        {
            selector.SetItems(new Dictionary<string, string>
            {
                ["horidava"] = "Horidava",
                ["cornova"] = "Cornova"
            });

            selector.SelectItemByIndex(0);

            Assert.That(selector.SelectedKey, Is.EqualTo("horidava"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Horidava"));
        }

        // ── SelectItemByKey – cache consistency ────────────────────────────────

        [Test]
        public void GivenItems_WhenSelectItemByKey_ThenSelectedIndexIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana", ["c"] = "Cherry" });

            selector.SelectItemByKey("c");

            Assert.That(selector.SelectedIndex, Is.EqualTo(2));
        }

        [Test]
        public void GivenItems_WhenSelectItemByKey_ThenSelectedValueIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana", ["c"] = "Cherry" });

            selector.SelectItemByKey("b");

            Assert.That(selector.SelectedValue, Is.EqualTo("Banana"));
        }

        // ── SelectItemByValue – cache consistency ──────────────────────────────

        [Test]
        public void GivenItems_WhenSelectItemByValue_ThenSelectedIndexIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana", ["c"] = "Cherry" });

            selector.SelectItemByValue("Cherry");

            Assert.That(selector.SelectedIndex, Is.EqualTo(2));
        }

        [Test]
        public void GivenItems_WhenSelectItemByValue_ThenSelectedKeyIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana", ["c"] = "Cherry" });

            selector.SelectItemByValue("Cherry");

            Assert.That(selector.SelectedKey, Is.EqualTo("c"));
        }

        // ── SelectNextItem / SelectPreviousItem – cache consistency ────────────

        [Test]
        public void GivenItemsAtFirstIndex_WhenSelectNextItem_ThenCacheIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.SelectNextItem();

            Assert.That(selector.SelectedKey, Is.EqualTo("b"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Banana"));
        }

        [Test]
        public void GivenItemsAtLastIndex_WhenSelectNextItem_ThenCacheWrapsToFirst()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            selector.SelectItemByIndex(1);

            selector.SelectNextItem();

            Assert.That(selector.SelectedKey, Is.EqualTo("a"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Apple"));
        }

        [Test]
        public void GivenItemsAtSecondIndex_WhenSelectPreviousItem_ThenCacheIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            selector.SelectItemByIndex(1);

            selector.SelectPreviousItem();

            Assert.That(selector.SelectedKey, Is.EqualTo("a"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Apple"));
        }

        [Test]
        public void GivenItemsAtFirstIndex_WhenSelectPreviousItem_ThenCacheWrapsToLast()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.SelectPreviousItem();

            Assert.That(selector.SelectedKey, Is.EqualTo("b"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Banana"));
        }

        // ── Navigation – full cycle ────────────────────────────────────────────

        [Test]
        public void GivenThreeItems_WhenCyclingNextThreeTimes_ThenReturnsToFirstIndex()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "A", ["b"] = "B", ["c"] = "C" });

            selector.SelectNextItem();
            selector.SelectNextItem();
            selector.SelectNextItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenThreeItems_WhenCyclingPreviousThreeTimes_ThenReturnsToFirstIndex()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "A", ["b"] = "B", ["c"] = "C" });

            selector.SelectPreviousItem();
            selector.SelectPreviousItem();
            selector.SelectPreviousItem();

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void GivenItems_WhenAlternatingNextAndPrevious_ThenIndexOscillates()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "A", ["b"] = "B", ["c"] = "C" });

            selector.SelectNextItem();
            Assert.That(selector.SelectedIndex, Is.EqualTo(1));

            selector.SelectPreviousItem();
            Assert.That(selector.SelectedIndex, Is.EqualTo(0));

            selector.SelectNextItem();
            Assert.That(selector.SelectedIndex, Is.EqualTo(1));
        }

        // ── TrySelectItem(string) ─────────────────────────────────────────────

        [Test]
        public void GivenItems_WhenTrySelectItemByValueWithValidValue_ThenSelectedValueUpdated()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.TrySelectItem("Banana");

            Assert.That(selector.SelectedValue, Is.EqualTo("Banana"));
        }

        [Test]
        public void GivenItems_WhenTrySelectItemByValueWithUnknownValue_ThenSelectedValueUnchanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });

            selector.TrySelectItem("Cornova");

            Assert.That(selector.SelectedValue, Is.EqualTo("Apple"));
        }

        [Test]
        public void GivenItems_WhenTrySelectItemByCurrentValue_ThenDoesNotFireSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.TrySelectItem("Apple");

            Assert.That(eventFired, Is.False);
        }

        // ── TrySelectItem(int) ────────────────────────────────────────────────

        [Test]
        public void GivenItems_WhenTrySelectItemByCurrentIndex_ThenDoesNotFireSelectedItemChanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            bool eventFired = false;
            selector.SelectedItemChanged += delegate { eventFired = true; };

            selector.TrySelectItem(0);

            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void GivenItems_WhenTrySelectItemByNegativeIndex_ThenDoesNotThrow()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            Assert.DoesNotThrow(() => selector.TrySelectItem(-1));
        }

        [Test]
        public void GivenItems_WhenTrySelectItemByNegativeIndex_ThenSelectedIndexUnchanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            selector.TrySelectItem(-1);

            Assert.That(selector.SelectedIndex, Is.EqualTo(0));
        }

        // ── GetItems – isolation ───────────────────────────────────────────────

        [Test]
        public void GivenItems_WhenGetItemsReturnedAndModified_ThenOriginalIsUnchanged()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple" });

            IDictionary<string, string> copy = selector.GetItems();
            copy.Clear();

            Assert.That(selector.ItemsCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenItems_WhenGetItems_ThenReturnedDictionaryHasCorrectValues()
        {
            selector.SetItems(new Dictionary<string, string>
            {
                ["ds3"] = "Dark Souls III",
                ["mc"] = "Minecraft",
                ["rs"] = "RuneScape"
            });

            IDictionary<string, string> result = selector.GetItems();

            Assert.That(result["ds3"], Is.EqualTo("Dark Souls III"));
            Assert.That(result["mc"], Is.EqualTo("Minecraft"));
            Assert.That(result["rs"], Is.EqualTo("RuneScape"));
        }

        [Test]
        public void GivenEmptySelector_WhenGetItems_ThenReturnedDictionaryIsEmpty()
        {
            IDictionary<string, string> result = selector.GetItems();

            Assert.That(result.Count, Is.EqualTo(0));
        }

        // ── SelectedItemChanged event args ────────────────────────────────────

        [Test]
        public void GivenItems_WhenSelectNextItem_ThenSelectedItemChangedArgsAreCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            ListSelectionEventArgs receivedArgs = null;
            selector.SelectedItemChanged += (_, eventArgs) => receivedArgs = (ListSelectionEventArgs)eventArgs;

            selector.SelectNextItem();

            Assert.That(receivedArgs, Is.Not.Null);
            Assert.That(receivedArgs.SelectedIndex, Is.EqualTo(1));
            Assert.That(receivedArgs.SelectedKey, Is.EqualTo("b"));
            Assert.That(receivedArgs.SelectedValue, Is.EqualTo("Banana"));
        }

        [Test]
        public void GivenItems_WhenSelectPreviousItem_ThenSelectedItemChangedArgsAreCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            selector.SelectItemByIndex(1);
            ListSelectionEventArgs receivedArgs = null;
            selector.SelectedItemChanged += (_, eventArgs) => receivedArgs = (ListSelectionEventArgs)eventArgs;

            selector.SelectPreviousItem();

            Assert.That(receivedArgs, Is.Not.Null);
            Assert.That(receivedArgs.SelectedIndex, Is.EqualTo(0));
            Assert.That(receivedArgs.SelectedKey, Is.EqualTo("a"));
            Assert.That(receivedArgs.SelectedValue, Is.EqualTo("Apple"));
        }

        [Test]
        public void GivenItems_WhenSelectByKey_ThenSelectedItemChangedArgsHaveCorrectKey()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            ListSelectionEventArgs receivedArgs = null;
            selector.SelectedItemChanged += (_, eventArgs) => receivedArgs = (ListSelectionEventArgs)eventArgs;

            selector.SelectItemByKey("b");

            Assert.That(receivedArgs.SelectedKey, Is.EqualTo("b"));
        }

        [Test]
        public void GivenItems_WhenSelectByValue_ThenSelectedItemChangedArgsHaveCorrectValue()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "Apple", ["b"] = "Banana" });
            ListSelectionEventArgs receivedArgs = null;
            selector.SelectedItemChanged += (_, eventArgs) => receivedArgs = (ListSelectionEventArgs)eventArgs;

            selector.SelectItemByValue("Banana");

            Assert.That(receivedArgs.SelectedValue, Is.EqualTo("Banana"));
        }

        // ── Boundary – single item ─────────────────────────────────────────────

        [Test]
        public void GivenSingleItem_WhenSelectItemByIndex0_ThenCacheIsCorrect()
        {
            selector.SetItems(new Dictionary<string, string> { ["only"] = "Cornova" });
            selector.SelectItemByIndex(0);

            Assert.That(selector.SelectedKey, Is.EqualTo("only"));
            Assert.That(selector.SelectedValue, Is.EqualTo("Cornova"));
        }

        [Test]
        public void GivenSingleItem_WhenSelectItemByIndex1_ThenThrowsIndexOutOfRangeException()
        {
            selector.SetItems(new Dictionary<string, string> { ["only"] = "Cornova" });

            Assert.Throws<IndexOutOfRangeException>(() => selector.SelectItemByIndex(1));
        }

        // ── Keys and Values collections ────────────────────────────────────────

        [Test]
        public void GivenThreeItems_WhenGettingKeys_ThenCountIsThree()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "A", ["b"] = "B", ["c"] = "C" });

            Assert.That(selector.Keys.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenThreeItems_WhenGettingValues_ThenCountIsThree()
        {
            selector.SetItems(new Dictionary<string, string> { ["a"] = "A", ["b"] = "B", ["c"] = "C" });

            Assert.That(selector.Values.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenItems_WhenGettingKeys_ThenAllKeysArePresent()
        {
            selector.SetItems(new Dictionary<string, string>
            {
                ["solara"] = "Solara",
                ["horidava"] = "Horidava",
                ["cornova"] = "Cornova"
            });

            Assert.That(selector.Keys, Does.Contain("solara"));
            Assert.That(selector.Keys, Does.Contain("horidava"));
            Assert.That(selector.Keys, Does.Contain("cornova"));
        }

        [Test]
        public void GivenItems_WhenGettingValues_ThenAllValuesArePresent()
        {
            selector.SetItems(new Dictionary<string, string>
            {
                ["solara"] = "Solara",
                ["horidava"] = "Horidava",
                ["cornova"] = "Cornova"
            });

            Assert.That(selector.Values, Does.Contain("Solara"));
            Assert.That(selector.Values, Does.Contain("Horidava"));
            Assert.That(selector.Values, Does.Contain("Cornova"));
        }
    }
}
