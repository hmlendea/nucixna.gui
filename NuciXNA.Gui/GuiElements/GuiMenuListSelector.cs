using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Input;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Menu item GUI element that cycles through the values of a list of strings
    /// </summary>
    public class GuiMenuListSelector : GuiMenuItem
    {
        string originalText;

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IList<string> Values { get; set; }

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        /// <value>The selected index.</value>
        public int SelectedIndex { get; private set; }

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        public string SelectedValue => Values[SelectedIndex];

        /// <summary>
        /// Occurs when the selected item was changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMenuListSelector"/> class.
        /// </summary>
        public GuiMenuListSelector()
        {
            Values = new List<string>();
            SelectedIndex = -1;
        }

        /// <summary>
        /// Selects an item by index.
        /// </summary>
        /// <param name="index">The index to select.</param>
        public void SelectItem(int index)
        {
            if (index < 0 || index > Values.Count)
            {
                throw new IndexOutOfRangeException();
            }

            SelectedIndex = index;
            ListSelectionEventArgs args = new ListSelectionEventArgs(index, Values[index]);

            SelectionChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Selects an item by value.
        /// </summary>
        /// <param name="value">The value to select.</param>
        public void SelectItem(string value)
        {
            int index = Values.IndexOf(value);

            SelectItem(index);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            if (Values.Count > 0)
            {
                SelectedIndex = 0;
            }

            originalText = Text;

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
            
            Text = originalText;

            if (Values.Count == 0)
            {
                return;
            }

            if (SelectedIndex >= Values.Count)
            {
                SelectedIndex = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = Values.Count - 1;
            }

            Text += $" : {SelectedValue}";
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
            if (e.Key == Keys.Right || e.Key == Keys.D)
            {
                SelectedIndex += 1;
            }
            else if (e.Key == Keys.Left || e.Key == Keys.A)
            {
                SelectedIndex -= 1;
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
                SelectedIndex += 1;
            }
            else if (e.Button == MouseButton.Right)
            {
                SelectedIndex -= 1;
            }
        }
    }
}
