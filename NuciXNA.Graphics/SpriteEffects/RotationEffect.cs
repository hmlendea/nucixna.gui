using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Rotation sprite effect.
    /// </summary>
    public class RotationEffect : CustomSpriteEffect<Sprite>
    {
        bool isIncreasing;

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the minimum rotation.
        /// </summary>
        /// <value>The minimum rotation.</value>
        public float MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum rotation.
        /// </summary>
        /// <value>The maximum rotation.</value>
        public float MaximumValue { get; set; }

        /// <summary>
        /// Gets or sets the current rotation.
        /// </summary>
        /// <value>The current rotation.</value>
        public float Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationEffect"/> class.
        /// </summary>
        public RotationEffect()
        {
            Speed = 1.0f;
            MinimumValue = 0.5f;
            MaximumValue = 1.5f;
            isIncreasing = false;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Active)
            {
                Value = MaximumValue;
                return;
            }

            float delta = Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (isIncreasing)
            {
                Value += delta;

                if (Value >= MaximumValue)
                {
                    Value = MaximumValue;
                    isIncreasing = false;
                }
            }
            else
            {
                Value -= delta;

                if (Value <= MinimumValue)
                {
                    Value = MinimumValue;
                    isIncreasing = true;
                }
            }
        }
    }
}
