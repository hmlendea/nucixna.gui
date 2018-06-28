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
        public float MinimumRotation { get; set; }

        /// <summary>
        /// Gets or sets the maximum rotation.
        /// </summary>
        /// <value>The maximum rotation.</value>
        public float MaximumRotation { get; set; }

        /// <summary>
        /// Gets or sets the current rotation.
        /// </summary>
        /// <value>The current rotation.</value>
        public float CurrentRotation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationEffect"/> class.
        /// </summary>
        public RotationEffect()
        {
            Speed = 1.0f;
            MinimumRotation = 0.5f;
            MaximumRotation = 1.5f;
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
                CurrentRotation = MaximumRotation;
                return;
            }

            float delta = Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);

            switch (isIncreasing)
            {
                case true:
                    CurrentRotation += delta;

                    if (CurrentRotation >= MaximumRotation)
                    {
                        CurrentRotation = MaximumRotation;
                        isIncreasing = false;
                    }

                    break;

                case false:
                    CurrentRotation -= delta;

                    if (CurrentRotation <= MinimumRotation)
                    {
                        CurrentRotation = MinimumRotation;
                        isIncreasing = true;
                    }

                    break;
            }
        }
    }
}
