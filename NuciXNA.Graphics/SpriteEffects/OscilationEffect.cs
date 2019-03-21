using Microsoft.Xna.Framework;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Oscilation sprite effect.
    /// </summary>
    public class OscilationEffect : RotationEffect
    {
        public float MinimumMultiplier { get; set; }

        public float MaximumMultiplier { get; set; }

        /// <summaryOscilationEffect
        /// Initializes a new instance of the <see cref="RotationEffect"/> class.
        /// </summary>
        public OscilationEffect()
            : base()
        {
            MinimumMultiplier = 0.5f;
            MaximumMultiplier = 1.5f;
        }

        /// <summary>
        /// Updates the rotation multiplier.
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
