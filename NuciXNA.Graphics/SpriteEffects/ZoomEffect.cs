using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Zoom sprite effect.
    /// </summary>
    public class ZoomEffect : CustomSpriteEffect<Sprite>
    {
        bool isIncreasing;

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the current zoom.
        /// </summary>
        /// <value>The current zoom.</value>
        public float Value { get; private set; }

        /// <summary>
        /// Gets or sets the minimum zoom.
        /// </summary>
        /// <value>The minimum zoom.</value>
        public float MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum zoom.
        /// </summary>
        /// <value>The maximum zoom.</value>
        public float MaximumValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomEffect"/> class.
        /// </summary>
        public ZoomEffect()
        {
            Speed = 0.1f;
            MinimumValue = 0.75f;
            MaximumValue = 1.25f;
            isIncreasing = true;
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
