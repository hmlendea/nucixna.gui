using System;

namespace NuciXNA.Gui.Controls
{
    public class ListSelectionEventArgs : EventArgs
    {
        public int SelectedIndex { get; set; }

        public string SelectedKey { get; set; }

        public string SelectedValue { get; set; }

        public ListSelectionEventArgs(int index, string key, string value)
        {
            SelectedIndex = index;
            SelectedKey = key;
            SelectedValue = value;
        }
    }
}
