using System;

namespace NuciXNA.Gui.Controls
{
    public class ListSelectionEventArgs(int index, string key, string value) : EventArgs
    {
        public int SelectedIndex { get; set; } = index;

        public string SelectedKey { get; set; } = key;

        public string SelectedValue { get; set; } = value;
    }
}
