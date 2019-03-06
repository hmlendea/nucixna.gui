using System;
using System.Linq;

using Microsoft.Xna.Framework;
using NuciXNA.Graphics.Drawing;

namespace NuciXNA.Graphics.SpriteEffects
{
    /// <summary>
    /// Effect.
    /// </summary>
    public class CustomSpriteEffect<TSprite> where TSprite : Sprite
    {
        bool isContentLoaded;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CustomSpriteEffect"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; private set; }

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
        /// Initializes a new instance of the <see cref="CustomSpriteEffect"/> class.
        /// </summary>
        public CustomSpriteEffect()
        {
            Type = GetType();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent(TSprite sprite)
        {
            Sprite = sprite;
            isContentLoaded = true;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            Active = false;
            isContentLoaded = false;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
    }
}
