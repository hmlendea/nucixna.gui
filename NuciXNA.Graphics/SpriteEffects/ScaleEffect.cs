using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Zoom sprite effect.
    /// </summary>
    public abstract class ScaleEffect : NuciSpriteEffect<Sprite>
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        public float CurrentHorizontalMultiplier { get; set; }

        public float CurrentVerticalMultiplier { get; set; }

        public float MinimumMultiplier { get; set; }
        
        public float MaximumMultiplier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ScaleEffect"/> is increasing.
        /// </summary>
        /// <value><c>true</c> if increasing; otherwise, <c>false</c>.</value>
        public bool IsIncreasing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleEffect"/> class.
        /// </summary>
        public ScaleEffect()
        {
            Speed = 0.1f;
            CurrentHorizontalMultiplier = 1.0f;
            CurrentVerticalMultiplier = 1.0f;
            MinimumMultiplier = 0.0f;
            MaximumMultiplier = 2.0f;
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
