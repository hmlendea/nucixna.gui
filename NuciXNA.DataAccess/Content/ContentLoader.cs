using System;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NuciXNA.DataAccess.Content
{
    /// <summary>
    /// Resource Manager that can load resources from plain disk files.
    /// </summary>
    public abstract class ContentLoader : IContentLoader
    {
        /// <summary>
        /// Loads a sound effect.
        /// </summary>
        /// <returns>The sound effect.</returns>
        /// <param name="resourcePath">The path to the file (without extension).</param>
        public abstract SoundEffect LoadSoundEffect(string resourcePath);
        
        /// <summary>
        /// Tries to load a sound effect.
        /// </summary>
        /// <returns>The sound effect if it can be loaded, null otherwise.</returns>
        /// <param name="resourcePath">The path to the file (without extension).</param>
        public SoundEffect TryLoadSoundEffect(string resourcePath)
        {
            try
            {
                return LoadSoundEffect(resourcePath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads a sprite font.
        /// </summary>
        /// <returns>The sprite font.</returns>
        /// <param name="resourcePath">The path to the file (without extension).</param>
        public abstract SpriteFont LoadSpriteFont(string resourcePath);

        /// <summary>
        /// Tries to load a sprite font.
        /// </summary>
        /// <returns>The sprite font if it can be loaded, null otherwise.</returns>
        /// <param name="resourcePath">The path to the file (without extension).</param>
        public SpriteFont TryLoadSpriteFont(string resourcePath)
        {
            try
            {
                return LoadSpriteFont(resourcePath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads a two-dimensional bitmap texture.
        /// </summary>
        /// <returns>The 2D texture.</returns>
        /// <param name="resourcePath">The path to the file (without extension).</param>
        public abstract Texture2D LoadTexture2D(string resourcePath);

        /// <summary>
        /// Tries to load a two-dimensional bitmap texture.
        /// </summary>
        /// <returns>The 2D texture if it can be loaded, null otherwise.</returns>
        /// <param name="resourcePath">The path to the file (without extension).</param>
        public Texture2D TryLoadTexture2D(string resourcePath)
        {
            try
            {
                return LoadTexture2D(resourcePath);
            }
            catch
            {
                return null;
            }
        }
    }
}
