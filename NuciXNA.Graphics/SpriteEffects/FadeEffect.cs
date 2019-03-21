using Microsoft.Xna.Framework;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Fade sprite effect.
    /// </summary>
    public class FadeEffect : OpacityEffect
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
                CurrentMultiplier += delta;

                if (CurrentMultiplier >= MaximumMultiplier)
                {
                    CurrentMultiplier = MaximumMultiplier;
                    IsIncreasing = false;
                }
            }
            else
            {
                CurrentMultiplier -= delta;

                if (CurrentMultiplier <= MinimumMultiplier)
                {
                    CurrentMultiplier = MinimumMultiplier;
                    IsIncreasing = true;
                }
            }
        }
    }
}
