using Microsoft.Xna.Framework;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Zoom sprite effect.
    /// </summary>
    public class ZoomEffect : ScaleEffect
    {
        protected override void UpdateMultiplier(GameTime gameTime)
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
