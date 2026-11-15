using System;

using Microsoft.Xna.Framework;

namespace NuciXNA.Gui.Controls
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
        public event EventHandler StateChanged;

        /// <summary>
        /// Turns the toggle on.
        /// </summary>
        public void SwitchOn() => SetState(true);

        /// <summary>
        /// Turns the toggle off.
        /// </summary>
        public void SwitchOff() => SetState(false);

        /// <summary>
        /// Inverts the current toggle state.
        /// </summary>
        public void SwitchState() => SetState(!IsOn);

        /// <summary>
        /// Sets the toggle state.
        /// </summary>
        /// <param name="state">The new state.</param>
        public void SetState(bool state)
        {
            if (IsOn == state)
            {
                return;
            }

            IsOn = state;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

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

            string stateLabel = " : Off";

            if (IsOn)
            {
                stateLabel = " : On";
            }

            TextControl.Text = Text + stateLabel;
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        private void RegisterEvents() => Triggered += OnTriggered;

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        private void UnregisterEvents() => Triggered -= OnTriggered;

        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnTriggered(object sender, EventArgs e) => SwitchState();
    }
}
