using System;
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
        private static int DefaultItemWidth => 512;
        private static int DefaultItemHeight => 48;
        private static int TextFadeSpeed => 2;
        private static float TextFadeMinimumMultiplier => 0.25f;
        private string textContent;
        private Colour selectedTextColour;
        private bool isSelectable;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => textContent;
            set
            {
                textContent = value;

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
            get => selectedTextColour;
            set
            {
                selectedTextColour = value;

                PropertyChangedEventArgs eventArguments = new(nameof(SelectedTextColour));
                SelectedTextColourChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item can receive menu navigation focus.
        /// </summary>
        /// <value><c>true</c> if selectable; otherwise, <c>false</c>.</value>
        public virtual bool IsSelectable
        {
            get => isSelectable;
            set
            {
                isSelectable = value;

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

        // TODO: Implement a dedicated trigger event handler and event arguments.
        /// <summary>
        /// Occurs when triggered.
        /// </summary>
        public event EventHandler Triggered;

        /// <summary>
        /// The text GUI control.
        /// </summary>
        protected GuiText TextControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMenuItem"/> class.
        /// </summary>
        public GuiMenuItem()
        {
            ForegroundColour = Colour.White;
            SelectedTextColour = Colour.Gold;

            IsSelectable = true;
            Size = new Size2D(DefaultItemWidth, DefaultItemHeight);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            TextControl = new GuiText
            {
                Id = $"{Id}_text",
                FontName = "MenuFont",
                AreEffectsActive = true,
                FadeEffect = new FadeEffect
                {
                    Speed = TextFadeSpeed,
                    MinimumMultiplier = TextFadeMinimumMultiplier
                }
            };

            RegisterChild(TextControl);

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
        private void RegisterEvents()
        {
            TextControl.ContentLoaded += OnTextContentLoaded;

            Clicked += OnClicked;
            KeyPressed += OnKeyPressed;
            GamepadButtonPressed += OnGamepadButtonPressed;
            MouseEntered += OnMouseEntered;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        private void UnregisterEvents()
        {
            TextControl.ContentLoaded -= OnTextContentLoaded;

            Clicked -= OnClicked;
            KeyPressed -= OnKeyPressed;
            GamepadButtonPressed -= OnGamepadButtonPressed;
            MouseEntered -= OnMouseEntered;
        }

        private void SetChildrenProperties()
        {
            TextControl.Text = Text;
            TextControl.Size = Size;

            if (IsFocused)
            {
                TextControl.AreEffectsActive = true;
                TextControl.ForegroundColour = SelectedTextColour;
            }
            else
            {
                TextControl.AreEffectsActive = false;
                TextControl.ForegroundColour = ForegroundColour;
            }
        }

        private void OnTextContentLoaded(object sender, EventArgs eventArguments) => TextControl.FadeEffect.Activate();

        /// <summary>
        /// Invokes the click handling path, raising the <see cref="Triggered"/> event.
        /// </summary>
        public void PerformClick()
            => OnClicked(
                this,
                new MouseButtonEventArgs(MouseButton.Left, NuciXNA.Input.ButtonState.Pressed, Point2D.Empty));

        /// <summary>
        /// Invokes the keyboard key handling path, raising the <see cref="Triggered"/> event when Enter or E is pressed.
        /// </summary>
        /// <param name="key">The pressed keyboard key.</param>
        public void PerformKeyPress(Keys key)
            => OnKeyPressed(this, new KeyboardKeyEventArgs(key, NuciXNA.Input.ButtonState.Pressed));

        /// <summary>
        /// Invokes the gamepad button handling path, raising the <see cref="Triggered"/> event when A is pressed.
        /// </summary>
        /// <param name="button">The pressed gamepad button.</param>
        public void PerformGamepadPress(Buttons button)
            => OnGamepadButtonPressed(
                this,
                new GamepadButtonEventArgs(button, NuciXNA.Input.ButtonState.Pressed, PlayerIndex.One));

        /// <summary>
        /// Invokes the mouse enter handling path.
        /// </summary>
        public void PerformMouseEnter()
            => OnMouseEntered(this, new MouseEventArgs(Point2D.Empty, Point2D.Empty));

        private void OnClicked(object sender, MouseButtonEventArgs eventArguments) => Triggered?.Invoke(this, EventArgs.Empty);

        private void OnKeyPressed(object sender, KeyboardKeyEventArgs eventArguments)
        {
            if (eventArguments.Key == Keys.Enter || eventArguments.Key == Keys.E)
            {
                Triggered?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnGamepadButtonPressed(object sender, GamepadButtonEventArgs eventArguments)
        {
            if (eventArguments.Button == Buttons.A)
            {
                Triggered?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnMouseEntered(object sender, MouseEventArgs eventArguments)
        {
            // TODO: Play the selection sound.
            GuiManager.Instance.FocusControl(this);
        }
    }
}
