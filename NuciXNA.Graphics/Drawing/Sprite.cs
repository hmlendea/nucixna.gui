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
        /// Gets or sets the opacity effect.
        /// </summary>
        /// <value>The opacity effect.</value>
        public OpacityEffect OpacityEffect { get; set; }

        /// <summary>
        /// Gets or sets the rotation effect.
        /// </summary>
        /// <value>The rotation effect.</value>
        public RotationEffect RotationEffect { get; set; }

        /// <summary>
        /// Gets or sets the scale effect.
        /// </summary>
        /// <value>The scale effect.</value>
        public ScaleEffect ScaleEffect { get; set; }

        public float ClientOpacity
        {
            get
            {
                float value = Opacity;

                if (OpacityEffect != null && OpacityEffect.Active)
                {
                    value *= OpacityEffect.CurrentMultiplier;
                }
                
                return value;
            }
        }

        public float ClientRotation
        {
            get
            {
                float value = Rotation;

                if (RotationEffect != null && RotationEffect.Active)
                {
                    value += RotationEffect.CurrentMultiplier;
                }

                return value;
            }
        }

        public Scale2D ClientScale
        {
            get
            {
                Scale2D value = Scale;

                if (ScaleEffect != null && ScaleEffect.Active)
                {
                    value = new Scale2D(
                        Scale.Horizontal * ScaleEffect.CurrentHorizontalMultiplier,
                        Scale.Vertical * ScaleEffect.CurrentVerticalMultiplier);
                }
                
                return value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public Sprite()
        {
            Active = true;

            Location = Point2D.Empty;

            Opacity = 1.0f;
            Scale = Scale2D.One;

            Tint = Colour.White;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            OpacityEffect?.LoadContent(this);
            RotationEffect?.LoadContent(this);
            ScaleEffect?.LoadContent(this);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            OpacityEffect?.UnloadContent();
            RotationEffect?.UnloadContent();
            ScaleEffect?.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            OpacityEffect?.Update(gameTime);
            RotationEffect?.Update(gameTime);
            ScaleEffect?.Update(gameTime);
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
