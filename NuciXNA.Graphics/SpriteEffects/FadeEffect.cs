using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Fade sprite effect.
    /// </summary>
    public class FadeEffect : CustomSpriteEffect<Sprite>
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        public float Value { get; private set; }

        /// <summary>
        /// Gets or sets the minimum opacity.
        /// </summary>
        /// <value>The minimum opacity.</value>
        public float MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum opacity.
        /// </summary>
        /// <value>The maximum opacity.</value>
        public float MaximumValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FadeEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool Increasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FadeEffect"/> class.
        /// </summary>
        public FadeEffect()
        {
            Speed = 1;
            MinimumValue = 0.0f;
            MaximumValue = 1.0f;
            Increasing = false;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Sprite.Active)
            {
                if (Increasing == false)
                {
                    Value -= Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    Value += Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if (Value < MinimumValue)
                {
                    Increasing = true;
                    Value = MinimumValue;
                }
                else if (Value > MaximumValue)
                {
                    Increasing = false;
                    Value = MaximumValue;
                }
            }
            else
            {
                Value = MaximumValue;
            }
        }
    }
}
