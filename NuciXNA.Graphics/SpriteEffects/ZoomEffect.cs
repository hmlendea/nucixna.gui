using Microsoft.Xna.Framework;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Zoom sprite effect.
    /// </summary>
    public class ZoomEffect : CustomSpriteEffect
    {
        bool isIncreasing;

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the minimum zoom.
        /// </summary>
        /// <value>The minimum zoom.</value>
        public float MinimumZoom { get; set; }

        /// <summary>
        /// Gets or sets the maximum zoom.
        /// </summary>
        /// <value>The maximum zoom.</value>
        public float MaximumZoom { get; set; }

        /// <summary>
        /// Gets or sets the current zoom.
        /// </summary>
        /// <value>The current zoom.</value>
        public float CurrentZoom { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomEffect"/> class.
        /// </summary>
        public ZoomEffect()
        {
            Speed = 0.1f;
            MinimumZoom = 0.75f;
            MaximumZoom = 1.25f;
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
                CurrentZoom = MaximumZoom;
                return;
            }

            float delta = Speed * ((float)gameTime.ElapsedGameTime.TotalSeconds);

            switch (isIncreasing)
            {
                case true:
                    CurrentZoom += delta;

                    if (CurrentZoom >= MaximumZoom)
                    {
                        CurrentZoom = MaximumZoom;
                        isIncreasing = false;
                    }

                    break;

                case false:
                    CurrentZoom -= delta;

                    if (CurrentZoom <= MinimumZoom)
                    {
                        CurrentZoom = MinimumZoom;
                        isIncreasing = true;
                    }

                    break;
            }
        }
    }
}
