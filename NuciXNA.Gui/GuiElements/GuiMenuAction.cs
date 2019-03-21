using System;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Menu action GUI element.
    /// </summary>
    public class GuiMenuAction : GuiMenuItem
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The type of the action.</value>
        public string ActionId { get; set; }

        /// <summary>
        /// Registers the events.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            Activated += OnActivated;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            Activated -= OnActivated;
        }

        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnActivated(object sender, EventArgs e)
        {
            switch (ActionId)
            {
                case "Exit":
                    // TODO: Close the game somehow...
                    //Program.Game.Exit();
                    break;
            }
        }
    }
}
