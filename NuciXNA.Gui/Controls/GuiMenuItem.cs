﻿using System;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// Menu item GUI Control.
    /// </summary>
    public class GuiMenuItem : GuiControl, IGuiControl
    {
        private string _text;
        private Colour _selectedTextColour;
        private bool _isSelectable;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;

                PropertyChangedEventArgs eventArguments = new(nameof(Text));
                TextChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the selected text colour.
        /// </summary>
        /// <value>The selected text colour.</value>
        public Colour SelectedTextColour
        {
            get => _selectedTextColour;
            set
            {
                _selectedTextColour = value;

                PropertyChangedEventArgs eventArguments = new(nameof(SelectedTextColour));
                SelectedTextColourChanged?.Invoke(this, eventArguments);
            }
        }

        public virtual bool IsSelectable
        {
            get => _isSelectable;
            set
            {
                _isSelectable = value;

                PropertyChangedEventArgs eventArguments = new(nameof(IsSelectable));
                IsSelectableChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Text"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler TextChanged;

        /// <summary>
        /// Occurs when the <see cref="SelectedTextColour"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler SelectedTextColourChanged;

        /// <summary>
        /// Occurs when the <see cref="IsSelectable"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler IsSelectableChanged;

        // TODO: Maybe implement my own handler and args
        /// <summary>
        /// Occurs when triggered.
        /// </summary>
        public event EventHandler Triggered;

        /// <summary>
        /// The text GUI control.
        /// </summary>
        protected GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMenuItem"/> class.
        /// </summary>
        public GuiMenuItem()
        {
            ForegroundColour = Colour.White;
            SelectedTextColour = Colour.Gold;

            IsSelectable = true;
            Size = new Size2D(512, 48);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            text = new GuiText
            {
                Id = $"{Id}_{nameof(text)}",
                FontName = "MenuFont",
                AreEffectsActive = true,
                FadeEffect = new FadeEffect
                {
                    Speed = 2,
                    MinimumMultiplier = 0.25f
                }
            };

            RegisterChild(text);

            SetChildrenProperties();
            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent() => UnregisterEvents();

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime) => SetChildrenProperties();

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch) { }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            text.ContentLoaded += OnTextContentLoaded;

            Clicked += OnClicked;
            KeyPressed += OnKeyPressed;
            MouseEntered += OnMouseEntered;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            text.ContentLoaded -= OnTextContentLoaded;

            Clicked -= OnClicked;
            KeyPressed -= OnKeyPressed;
            MouseEntered -= OnMouseEntered;
        }

        void SetChildrenProperties()
        {
            text.Text = Text;
            text.Size = Size;

            if (IsFocused)
            {
                text.AreEffectsActive = true;
                text.ForegroundColour = SelectedTextColour;
            }
            else
            {
                text.AreEffectsActive = false;
                text.ForegroundColour = ForegroundColour;
            }
        }

        void OnTextContentLoaded(object sender, EventArgs e) => text.FadeEffect.Activate();

        void OnClicked(object sender, MouseButtonEventArgs e) => Triggered?.Invoke(this, EventArgs.Empty);

        void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Enter || e.Key == Keys.E)
            {
                Triggered?.Invoke(this, EventArgs.Empty);
            }
        }

        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            // TODO: Play selection sound
            GuiManager.Instance.FocusControl(this);
        }
    }
}
