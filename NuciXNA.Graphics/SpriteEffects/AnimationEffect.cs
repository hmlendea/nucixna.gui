using Microsoft.Xna.Framework;
using NuciXNA.Primitives;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Animation sprite effect.
    /// </summary>
    public class AnimationEffect : SpriteSheetEffect
    {
        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            Point2D newFrame = CurrentFrame;

            if (Sprite.IsActive)
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
