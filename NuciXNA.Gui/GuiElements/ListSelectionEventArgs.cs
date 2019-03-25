using System;

namespace NuciXNA.Gui.GuiElements
{
    public class ListSelectionEventArgs : EventArgs
    {
        public int SelectedIndex { get; set; }

        public string SelectedValue { get; set; }

        public ListSelectionEventArgs(int index, string value)
        {
            SelectedIndex = index;
            SelectedValue = value;
        }
    }
}
