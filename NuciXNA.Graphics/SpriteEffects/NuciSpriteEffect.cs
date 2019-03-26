using System;
using System.Linq;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Effect.
    /// </summary>
    public abstract class NuciSpriteEffect<TSprite> : IDisposable where TSprite : Sprite
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NuciSpriteEffect"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; private set; }

        protected TSprite Sprite { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get { return Type.ToString().Split('.').Last(); } }
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NuciSpriteEffect"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if the content is loaded; otherwise, <c>false</c>.</value>
        public bool IsContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NuciSpriteEffect"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> began loading its content.
        /// </summary>
        public event EventHandler ContentLoading;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> finished loading its content.
        /// </summary>
        public event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> began unloading its content.
        /// </summary>
        public event EventHandler ContentUnloading;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> finished unloading its content.
        /// </summary>
        public event EventHandler ContentUnloaded;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> began updating.
        /// </summary>
        public event EventHandler Updating;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> finished updating.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> began drawing.
        /// </summary>
        public event EventHandler Drawing;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> finished drawing.
        /// </summary>
        public event EventHandler Drawn;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> began disposing.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> finished disposing.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> began activating.
        /// </summary>
        public event EventHandler Activating;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> finished activating.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> began deactivating.
        /// </summary>
        public event EventHandler Deactivating;

        /// <summary>
        /// Occurs when this <see cref="NuciSpriteEffect"/> finished deactivated.
        /// </summary>
        public event EventHandler Deactivated;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuciSpriteEffect"/> class.
        /// </summary>
        public NuciSpriteEffect()
        {
            Type = GetType();
        }

        ~NuciSpriteEffect()
        {
            Dispose();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent(TSprite sprite)
        {
            if (IsContentLoaded)
            {
                throw new InvalidOperationException("Content already loaded");
            }

            ContentLoading?.Invoke(this, EventArgs.Empty);

            Sprite = sprite;

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

            ContentUnloading?.Invoke(this, EventArgs.Empty);

            IsActive = false;

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
            
            if (!IsActive)
            {
                return;
            }

            Updating?.Invoke(this, EventArgs.Empty);

            DoUpdate(gameTime);

            Updated?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>
        /// Disposes of this <see cref="NuciSpriteEffect"/>.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Activates the effect.
        /// </summary>
        public void Activate()
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            Activating?.Invoke(this, EventArgs.Empty);

            IsActive = true;

            Activated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Deactivates the effect.
        /// </summary>
        public void Deactivate()
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            Deactivating?.Invoke(this, EventArgs.Empty);

            IsActive = false;

            Deactivated?.Invoke(this, EventArgs.Empty);
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
        /// Disposes of this <see cref="NuciSpriteEffect"/>.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (this)
            {
                Disposing?.Invoke(this, EventArgs.Empty);

                if (IsContentLoaded)
                {
                    UnloadContent();
                }

                IsDisposed = true;
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
