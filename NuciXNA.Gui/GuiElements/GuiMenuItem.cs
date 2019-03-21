using System;

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
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the selected text colour.
        /// </summary>
        /// <value>The selected text colour.</value>
        public Colour SelectedTextColour { get; set; }

        public virtual bool Selectable { get; }

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

            Selectable = true;
            Size = new Size2D(512, 48);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            text = new GuiText
            {
                FontName = "MenuFont",
                AreEffectsActive = true,
                FadeEffect = new FadeEffect
                {
                    Speed = 2,
                    MinimumMultiplier = 0.25f
                }
            };

            base.LoadContent();
            text.FadeEffect.Activate();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(text);
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            Clicked += OnClicked;
            KeyPressed += OnKeyPressed;
            MouseEntered += OnMouseEntered;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            Clicked -= OnClicked;
            KeyPressed -= OnKeyPressed;
            MouseEntered -= OnMouseEntered;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

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
