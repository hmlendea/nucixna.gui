using System;
using System.ComponentModel;

using Microsoft.Xna.Framework;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Menu toggle GUI element.
    /// </summary>
    public class GuiMenuToggle : GuiMenuItem
    {
        string _property;
        bool _toggleState;

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public string Property
        {
            get => _property;
            set
            {
                _property = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(Property));
                PropertyChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the toggle state.
        /// </summary>
        /// <value>The type of the toggle state.</value>
        public bool ToggleState
        {
            get => _toggleState;
            set
            {
                _toggleState = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(ToggleState));
                ToggleStateChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Property"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the <see cref="ToggleState"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler ToggleStateChanged;

        string originalText;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            originalText = Text;

            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            originalText = Text;
            
            UnregisterEvents();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);

            if (ToggleState)
            {
                Text = originalText + " : On";
            }
            else
            {
                Text = originalText + " : Off";
            }
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            Activated += OnActivated;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            Activated -= OnActivated;
        }

        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnActivated(object sender, EventArgs e)
        {
            ToggleState = !ToggleState;
        }
    }
}
