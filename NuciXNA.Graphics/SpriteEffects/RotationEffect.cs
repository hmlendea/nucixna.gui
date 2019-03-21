using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Rotation sprite effect.
    /// </summary>
    public abstract class RotationEffect : NuciSpriteEffect<Sprite>
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        public float CurrentMultiplier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RotationEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool IsIncreasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationEffect"/> class.
        /// </summary>
        public RotationEffect()
        {
            Speed = 1.0f;
            CurrentMultiplier = 1.0f;
            IsIncreasing = true;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Active)
            {
                UpdateMultiplier(gameTime);
            }
        }

        protected abstract void UpdateMultiplier(GameTime gameTime);
    }
}
