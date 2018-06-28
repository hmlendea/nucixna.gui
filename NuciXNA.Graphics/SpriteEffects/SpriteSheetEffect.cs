using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Primitives;

namespace NuciXNA.Graphics.SpriteEffects
{
    public abstract class SpriteSheetEffect : CustomSpriteEffect<TextureSprite>
    {
        public int FrameCounter { get; set; }

        public int SwitchFrame { get; set; }

        public Point2D CurrentFrame { get; set; }

        public Size2D FrameAmount { get; set; }

        [XmlIgnore]
        public Size2D FrameSize { get; private set; }

        public SpriteSheetEffect()
        {
            FrameCounter = 0;
            SwitchFrame = 100;
            CurrentFrame = new Point2D(1, 0);
            FrameAmount = new Size2D(3, 4);
            FrameSize = Size2D.Empty;
        }

        public override void LoadContent(TextureSprite sprite)
        {
            base.LoadContent(sprite);

            FrameSize = new Size2D(
                Sprite.TextureSize.Width / FrameAmount.Width,
                Sprite.TextureSize.Height / FrameAmount.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Active)
            {
                return;
            }

            base.Update(gameTime);

            UpdateFrame(gameTime);

            Sprite.SourceRectangle = new Rectangle2D(
                CurrentFrame.X * FrameSize.Width,
                CurrentFrame.Y * FrameSize.Height,
                FrameSize.Width,
                FrameSize.Height);
        }

        public abstract void UpdateFrame(GameTime gameTime);
    }
}
