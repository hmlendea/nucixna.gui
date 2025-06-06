using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Helpers
{
    public class DummyControl : GuiControl, IGuiControl
    {
        protected override void DoLoadContent() { }

        protected override void DoUnloadContent() { }

        protected override void DoUpdate(GameTime gameTime) { }

        protected override void DoDraw(SpriteBatch spriteBatch) { }
    }
}
