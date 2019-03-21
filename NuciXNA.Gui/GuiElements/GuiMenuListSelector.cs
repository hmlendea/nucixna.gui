using System;
using System.Collections.Generic;

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
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public List<string> Values { get; set; }

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        /// <value>The selected index.</value>
        public int SelectedIndex { get; set; }

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <value>The selected value.</value>
        public string SelectedValue
        {
            get
            {
                if (Values.Count > 0 && Values.Count > SelectedIndex)
                {
                    return Values[SelectedIndex];
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Occurs when the selected index was changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs when the selected value was changed.
        /// </summary>
        public event EventHandler SelectedValueChanged;

        int lastSelectedIndex;
        string lastSelectedValue;
        string originalText;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMenuListSelector"/> class.
        /// </summary>
        public GuiMenuListSelector()
        {
            Values = new List<string>();
            SelectedIndex = -1;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (Values.Count > 0)
            {
                SelectedIndex = 0;
            }

            lastSelectedIndex = SelectedIndex;
            lastSelectedValue = SelectedValue;

            originalText = Text;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
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

            if (SelectedIndex != lastSelectedIndex)
            {
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }

            if (SelectedValue != lastSelectedValue)
            {
                SelectedValueChanged?.Invoke(this, EventArgs.Empty);
            }

            lastSelectedIndex = SelectedIndex;
            lastSelectedValue = SelectedValue;

            base.Update(gameTime);
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            KeyPressed += OnKeyPressed;
            MouseButtonPressed += OnMouseButtonPressed;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

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
