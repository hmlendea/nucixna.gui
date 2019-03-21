using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;

namespace NuciXNA.Graphics.Drawing
{
    /// <summary>
    /// Sprite.
    /// </summary>
    public abstract class Sprite : IDisposable
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
        /// Gets or sets a value indicating whether this <see cref="Sprite"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if the content is loaded; otherwise, <c>false</c>.</value>
        public bool IsContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Sprite"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Occurs when this <see cref="Sprite"/>'s content finished loading.
        /// </summary>
        public event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="Sprite"/>'s content finished unloading.
        /// </summary>
        public event EventHandler ContentUnloaded;

        /// <summary>
        /// Occurs when this <see cref="Sprite"/> was disposed.
        /// </summary>
        public event EventHandler Disposed;

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

        ~Sprite()
        {
            Dispose();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            if (IsContentLoaded)
            {
                throw new InvalidOperationException("Content already loaded");
            }

            OpacityEffect?.LoadContent(this);
            RotationEffect?.LoadContent(this);
            ScaleEffect?.LoadContent(this);

            DoLoadContent();

            IsContentLoaded = true;
            ContentLoaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            OpacityEffect?.UnloadContent();
            RotationEffect?.UnloadContent();
            ScaleEffect?.UnloadContent();

            DoUnloadContent();

            IsContentLoaded = false;
            ContentUnloaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            OpacityEffect?.Update(gameTime);
            RotationEffect?.Update(gameTime);
            ScaleEffect?.Update(gameTime);

            DoUpdate(gameTime);
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            DoDraw(spriteBatch);
        }
        
        /// <summary>
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected abstract void DoLoadContent();

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected abstract void DoUnloadContent();

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected abstract void DoUpdate(GameTime gameTime);

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected abstract void DoDraw(SpriteBatch spriteBatch);

        /// <summary>
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (this)
            {
                if (IsContentLoaded)
                {
                    UnloadContent();
                }

                IsDisposed = true;
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fired by the <see cref="ContentLoaded"> event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnContentLoaded(object sender, EventArgs e)
        {
            IsContentLoaded = true;
        }

        /// <summary>
        /// Fired by the <see cref="ContentUnloaded"> event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnContentUnloaded(object sender, EventArgs e)
        {
            IsContentLoaded = false;
        }

        /// <summary>
        /// Fired by the <see cref="Disposed"> event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnDisposed(object sender, EventArgs e)
        {
            IsDisposed = true;
        }
    }
}
