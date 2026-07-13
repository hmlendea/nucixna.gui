using System;
using System.ComponentModel;

using NuciXNA.Gui.Screens;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// Menu link GUI element
    /// </summary>
    public class GuiMenuLink : GuiMenuItem
    {
        private Type targetScreen;
        private object[] parameters;

        /// <summary>
        /// Gets or sets the targeted screen.
        /// </summary>
        /// <value>The targeted screen.</value>
        public Type TargetScreen
        {
            get => targetScreen;
            set
            {
                targetScreen = value;

                PropertyChangedEventArgs eventArguments = new(nameof(TargetScreen));
                TargetScreenChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the link arguments.
        /// </summary>
        /// <value>The link arguments.</value>
        public object[] Parameters
        {
            get => parameters;
            set
            {
                parameters = value;

                PropertyChangedEventArgs eventArguments = new(nameof(Parameters));
                ParametersChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="TargetScreen"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler TargetScreenChanged;

        /// <summary>
        /// Occurs when the <see cref="Parameters"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler ParametersChanged;

        /// <summary>
        /// Registers the events.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            Triggered += OnTriggered;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            Triggered -= OnTriggered;
        }

        /// <summary>
        /// Fired by the <see cref="Triggered"> event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnTriggered(object sender, EventArgs e) => ScreenManager.Instance.ChangeScreens(TargetScreen, Parameters);
    }
}
