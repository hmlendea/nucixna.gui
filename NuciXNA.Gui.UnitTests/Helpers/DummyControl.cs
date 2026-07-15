using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui.UnitTests.Helpers
{
    public sealed class DummyControl : GuiControl
    {
        public void AddChild(DummyControl child) => RegisterChild(child);

        protected override void DoLoadContent() { }

        protected override void DoUnloadContent() { }

        protected override void DoUpdate(GameTime gameTime) { }

        protected override void DoDraw(SpriteBatch spriteBatch) { }
    }
}
