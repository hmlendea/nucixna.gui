using Microsoft.Xna.Framework;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Zoom sprite effect.
    /// </summary>
    public class ZoomEffect : ScaleEffect
    {
        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            float delta = Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (IsIncreasing)
            {
                CurrentHorizontalMultiplier += delta;

                if (CurrentHorizontalMultiplier >= MaximumMultiplier)
                {
                    CurrentHorizontalMultiplier = MaximumMultiplier;
                    IsIncreasing = false;
                }
            }
            else
            {
                CurrentHorizontalMultiplier -= delta;

                if (CurrentHorizontalMultiplier <= MinimumMultiplier)
                {
                    CurrentHorizontalMultiplier = MinimumMultiplier;
                    IsIncreasing = true;
                }
            }

            CurrentVerticalMultiplier = CurrentHorizontalMultiplier;
        }
    }
}
