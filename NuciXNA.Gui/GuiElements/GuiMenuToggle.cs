using System;

using Microsoft.Xna.Framework;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Menu toggle GUI element.
    /// </summary>
    public class GuiMenuToggle : GuiMenuItem
    {
        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the toggle state.
        /// </summary>
        /// <value>The type of the toggle state.</value>
        public bool ToggleState { get; set; }

        string originalText;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            originalText = Text;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (ToggleState)
            {
                Text = originalText + " : On";
            }
            else
            {
                Text = originalText + " : Off";
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            Activated += OnActivated;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

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
