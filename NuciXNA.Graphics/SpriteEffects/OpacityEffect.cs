using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Fade sprite effect.
    /// </summary>
    public abstract class OpacityEffect : NuciSpriteEffect<Sprite>
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        public float CurrentMultiplier { get; set; }
        
        public float MinimumMultiplier { get; set; }
        
        public float MaximumMultiplier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FadeEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool IsIncreasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpacityEffect"/> class.
        /// </summary>
        public OpacityEffect()
        {
            Speed = 1;
            CurrentMultiplier = 0.0f;
            MinimumMultiplier = 0.0f;
            MaximumMultiplier = 1.0f;
            IsIncreasing = true;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {

        }
    }
}
