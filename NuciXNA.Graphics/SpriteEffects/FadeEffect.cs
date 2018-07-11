using Microsoft.Xna.Framework;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Fade sprite effect.
    /// </summary>
    public class FadeEffect : OpacityEffect
    {
        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void UpdateValue(GameTime gameTime)
        {
            if (IsIncreasing == false)
            {
                CurrentMultiplier -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                CurrentMultiplier += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (CurrentMultiplier < MinimumMultiplier)
            {
                IsIncreasing = true;
                CurrentMultiplier = MinimumMultiplier;
            }
            else if (CurrentMultiplier > MaximumMultiplier)
            {
                IsIncreasing = false;
                CurrentMultiplier = MaximumMultiplier;
            }
        }
    }
}
