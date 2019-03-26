using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Input;
using NuciXNA.Primitives.Mapping;

using XnaKeys = Microsoft.Xna.Framework.Input.Keys;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// Menu item GUI element that cycles through the values of a list of strings
    /// </summary>
    public class GuiMenuListSelector : GuiMenuItem
    {
        IDictionary<string, string> items { get; set; }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public IEnumerable<string> Keys => items.Keys;

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<string> Values => items.Values;

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        /// <value>The selected index.</value>
        public int SelectedIndex { get; private set; }

        /// <summary>
        /// Gets the selected key.
        /// </summary>
        /// <value>The selected key.</value>
        public string SelectedKey => Keys.ElementAt(SelectedIndex);

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        public string SelectedValue => Values.ElementAt(SelectedIndex);

        public int ItemsCount => items.Count;

        /// <summary>
        /// Occurs when the items collection was changed.
        /// </summary>
        public event EventHandler ItemsChanged;

        /// <summary>
        /// Occurs when the selected item was changed.
        /// </summary>
        public event EventHandler SelectedItemChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMenuListSelector"/> class.
        /// </summary>
        public GuiMenuListSelector()
        {
            items = new Dictionary<string, string>();
            SelectedIndex = -1;
        }

        public void SelectNextItem()
        {
            if (items.Count == 1)
            {
                return;
            }

            int newIndex = SelectedIndex + 1;

            if (newIndex >= items.Count)
            {
                newIndex = 0;
            }
            
            SelectItemByIndex(newIndex);
        }

        public void SelectPreviousItem()
        {
            if (items.Count == 1)
            {
                return;
            }

            int newIndex = SelectedIndex - 1;

            if (newIndex <= 0)
            {
                newIndex = items.Count - 1;
            }
            
            SelectItemByIndex(newIndex);
        }

        /// <summary>
        /// Selects an item by index.
        /// </summary>
        /// <param name="index">The index to select.</param>
        public void SelectItemByIndex(int index)
        {
            if (SelectedIndex == index)
            {
                return;
            }

            if (index < 0 || index >= items.Count)
            {
                throw new IndexOutOfRangeException();
            }

            SelectedIndex = index;

            ListSelectionEventArgs args = new ListSelectionEventArgs(SelectedIndex, SelectedKey, SelectedValue);
            SelectedItemChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Selects an item by key.
        /// </summary>
        /// <param name="key">The key to select.</param>
        public void SelectItemByKey(string key)
        {
            if (SelectedKey == key)
            {
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items.ElementAt(i).Key == key)
                {
                    SelectItemByIndex(i);
                    return;
                }
            }
            
            throw new ArgumentOutOfRangeException(nameof(key));
        }

        /// <summary>
        /// Selects an item by value.
        /// </summary>
        /// <param name="value">The value to select.</param>
        public void SelectItemByValue(string value)
        {
            if (SelectedValue == value)
            {
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items.ElementAt(i).Value == value)
                {
                    SelectItemByIndex(i);
                    return;
                }
            }
            
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Selects an item by index, if it exists.
        /// </summary>
        /// <param name="index">The index to select.</param>
        public void TrySelectItem(int index)
        {
            try
            {
                SelectItemByIndex(index);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Selects an item by value, if it exists.
        /// </summary>
        /// <param name="value">The value to select.</param>
        public void TrySelectItem(string value)
        {
            try
            {
                SelectItemByValue(value);
            }
            catch
            {

            }
        }

        public void SetItems(params string[] values)
        {
            IDictionary<string, string> itemsDictionary = values.ToDictionary(
                x => Guid.NewGuid().ToString(),
                x => x);

            SetItems(itemsDictionary);
        }

        public void SetItems(params KeyValuePair<string, string>[] items)
        {
            IDictionary<string, string> itemsDictionary = items.ToDictionary(
                x => x.Key,
                x => x.Value);

            SetItems(itemsDictionary);
        }

        public void SetItems(IEnumerable<string> values)
        {
            IDictionary<string, string> itemsDictionary = values.ToDictionary(
                x => Guid.NewGuid().ToString(),
                x => x);

            SetItems(itemsDictionary);
        }

        public void SetItems(IEnumerable<KeyValuePair<string, string>> items)
        {
            IDictionary<string, string> itemsDictionary = items.ToDictionary(
                x => x.Key,
                x => x.Value);

            SetItems(itemsDictionary);
        }

        public void SetItems(IDictionary<string, string> items)
        {
            this.items = items.ToDictionary(x => x.Key, x => x.Value);

            ItemsChanged?.Invoke(this, EventArgs.Empty);

            if (items.Count > 0)
            {
                SelectItemByIndex(0);
            }
        }

        public IDictionary<string, string> GetItems()
            => items.ToDictionary(x => x.Key, x => x.Value);

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            if (items.Count > 0)
            {
                SelectedIndex = 0;
            }

            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            UnregisterEvents();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);
            
            if (items.Count == 0)
            {
                return;
            }

            if (SelectedIndex >= items.Count)
            {
                SelectedIndex = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = items.Count - 1;
            }

            text.Text = Text + $" : {SelectedValue}";
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            KeyPressed += OnKeyPressed;
            MouseButtonPressed += OnMouseButtonPressed;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            KeyPressed -= OnKeyPressed;
            MouseButtonPressed -= OnMouseButtonPressed;
        }

        /// <summary>
        /// Fires when a keyboard key was pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == XnaKeys.Right || e.Key == XnaKeys.D)
            {
                SelectNextItem();
            }
            else if (e.Key == XnaKeys.Left || e.Key == XnaKeys.A)
            {
                SelectPreviousItem();
            }
        }

        void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!DisplayRectangle.Contains(e.Location))
            {
                return;
            }

            if (e.Button == MouseButton.Left)
            {
                SelectNextItem();
            }
            else if (e.Button == MouseButton.Right)
            {
                SelectPreviousItem();
            }
        }
    }
}
