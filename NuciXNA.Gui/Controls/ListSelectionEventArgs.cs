using System;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// Provides data for a menu list selection change.
    /// </summary>
    public class ListSelectionEventArgs(int index, string key, string value) : EventArgs
    {
        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        /// <value>The selected index.</value>
        public int SelectedIndex { get; set; } = index;

        /// <summary>
        /// Gets or sets the key of the selected item.
        /// </summary>
        /// <value>The selected key.</value>
        public string SelectedKey { get; set; } = key;

        /// <summary>
        /// Gets or sets the value of the selected item.
        /// </summary>
        /// <value>The selected value.</value>
        public string SelectedValue { get; set; } = value;
    }
}
