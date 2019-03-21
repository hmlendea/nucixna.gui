using System;

using NuciXNA.Gui.Screens;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Menu link GUI element
    /// </summary>
    public class GuiMenuLink : GuiMenuItem
    {
        /// <summary>
        /// Gets or sets the targeted screen.
        /// </summary>
        /// <value>The targeted screen.</value>
        public Type TargetScreen { get; set; }

        /// <summary>
        /// Gets or sets the link arguments.
        /// </summary>
        /// <value>The link arguments.</value>
        public object[] Parameters { get; set; }

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
            ScreenManager.Instance.ChangeScreens(TargetScreen, Parameters);
        }
    }
}
