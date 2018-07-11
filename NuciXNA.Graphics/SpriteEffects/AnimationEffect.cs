using Microsoft.Xna.Framework;
using NuciXNA.Primitives;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Animation sprite effect.
    /// </summary>
    public class AnimationEffect : SpriteSheetEffect
    {
        public override void UpdateFrame(GameTime gameTime)
        {
            Point2D newFrame = CurrentFrame;

            if (Sprite.Active)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;
                    newFrame.X += 1;

                    if (CurrentFrame.X >= Sprite.TextureSize.Width / FrameSize.Width - 1)
                    {
                        newFrame.X = 0;
                    }
                }
            }
            else
            {
                newFrame.X = 1;
            }

            CurrentFrame = newFrame;
        }
    }
}
