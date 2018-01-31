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
        public string LinkArgs { get; set; }

        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnActivated(object sender, EventArgs e)
        {
            base.OnActivated(sender, e);

            string[] args = new string[0];

            if (!string.IsNullOrWhiteSpace(LinkArgs))
            {
                args = LinkArgs.Split(' ');
            }

            ScreenManager.Instance.ChangeScreens(TargetScreen, args);
        }
    }
}
