using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

namespace NuciXNA.Graphics.Drawing
{
    /// <summary>
    /// Sprite.
    /// </summary>
    public abstract class Sprite
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Sprite"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the tint.
        /// </summary>
        /// <value>The tint.</value>
        public Colour Tint { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>The zoom.</value>
        public float Zoom { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point2D Location { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size2D SpriteSize { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public Scale2D Scale { get; set; }

        /// <summary>
        /// Gets the covered screen area.
        /// </summary>
        /// <value>The covered screen area.</value>
        public abstract Rectangle2D ClientRectangle { get; }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        /// <value>The fade effect.</value>
        public FadeEffect FadeEffect { get; set; }

        /// <summary>
        /// Gets or sets the rotation effect.
        /// </summary>
        /// <value>The rotation effect.</value>
        public RotationEffect RotationEffect { get; set; }

        /// <summary>
        /// Gets or sets the zoom effect.
        /// </summary>
        /// <value>The zoom effect.</value>
        public ZoomEffect ZoomEffect { get; set; }

        protected float FinalRotation
        {
            get
            {
                float value = Rotation;

                if (RotationEffect != null && RotationEffect.Active)
                {
                    value += RotationEffect.CurrentRotation;
                }

                return value;
            }
        }

        protected float FinalZoom
        {
            get
            {
                float value = Zoom;

                if (ZoomEffect != null && ZoomEffect.Active)
                {
                    value += ZoomEffect.CurrentZoom;
                }

                return value;
            }
        }

        protected Scale2D FinalScale => new Scale2D(Scale.Horizontal * FinalZoom, Scale.Vertical * FinalZoom);

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public Sprite()
        {
            Active = true;

            Location = Point2D.Empty;

            Opacity = 1.0f;
            Zoom = 1.0f;
            Scale = Scale2D.One;

            Tint = Colour.White;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            FadeEffect?.LoadContent(this);
            RotationEffect?.LoadContent(this);
            ZoomEffect?.LoadContent(this);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            FadeEffect?.UnloadContent();
            RotationEffect?.UnloadContent();
            ZoomEffect?.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            FadeEffect?.Update(gameTime);
            RotationEffect?.Update(gameTime);
            ZoomEffect?.Update(gameTime);
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
