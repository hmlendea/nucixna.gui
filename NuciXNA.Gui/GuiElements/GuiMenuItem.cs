using System;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Menu item GUI element.
    /// </summary>
    public abstract class GuiMenuItem : GuiElement
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
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(Text));
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
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(SelectedTextColour));
                SelectedTextColourChanged?.Invoke(this, eventArguments);
            }
        }

        public virtual bool IsSelectable
        {
            get => _isSelectable;
            set
            {
                _isSelectable = value;
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(IsSelectable));
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
        /// Occurs when activated.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// The text GUI element.
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

            AddChild(text);

            SetChildrenProperties();
            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

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

        void OnTextContentLoaded(object sender, EventArgs e)
        {
            text.FadeEffect.Activate();
        }

        void OnClicked(object sender, MouseButtonEventArgs e)
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Enter || e.Key == Keys.E)
            {
                Activated?.Invoke(this, EventArgs.Empty);
            }
        }

        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            // TODO: Play selection sound
            GuiManager.Instance.FocusElement(this);
        }
    }
}
