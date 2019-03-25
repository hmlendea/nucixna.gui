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
        /// <summary>
        /// Gets or sets the toggle state.
        /// </summary>
        /// <value>The type of the toggle state.</value>
        public bool IsOn { get; private set; }

        /// <summary>
        /// Occurs when the state was changed.
        /// </summary>
        public event PropertyChangedEventHandler StateChanged;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

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

            if (IsOn)
            {
                text.Text = Text + " : On";
            }
            else
            {
                text.Text = Text + " : Off";
            }
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            Triggered += OnTriggered;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            Triggered -= OnTriggered;
        }

        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnTriggered(object sender, EventArgs e)
        {
            IsOn = !IsOn;
        }
    }
}
