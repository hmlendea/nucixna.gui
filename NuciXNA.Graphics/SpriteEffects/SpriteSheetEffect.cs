using Microsoft.Xna.Framework;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Primitives;

namespace NuciXNA.Graphics.SpriteEffects
{
    public abstract class SpriteSheetEffect : NuciSpriteEffect<TextureSprite>
    {
        public int FrameCounter { get; set; }

        public int SwitchFrame { get; set; }

        public Point2D CurrentFrame { get; set; }

        public Size2D FrameAmount { get; set; }
        
        public Size2D FrameSize { get; private set; }

        public SpriteSheetEffect()
        {
            FrameCounter = 0;
            SwitchFrame = 100;
            CurrentFrame = Point2D.Empty;
            FrameAmount = Size2D.Empty;
            FrameSize = Size2D.Empty;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            FrameSize = Sprite.TextureSize / FrameAmount;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {

        }
    }
}
