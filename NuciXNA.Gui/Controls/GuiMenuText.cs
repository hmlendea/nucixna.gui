namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// Menu text GUI element
    /// </summary>
    public class GuiMenuText : GuiMenuItem
    {
        /// <summary>
        /// Gets a value indicating whether this item can receive menu navigation focus.
        /// </summary>
        /// <value>Always <c>false</c>; text entries are informational.</value>
        public override bool IsSelectable => false;
    }
}
