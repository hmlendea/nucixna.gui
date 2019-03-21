using System;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NuciXNA.DataAccess.Content
{
    public interface IContentLoader
    {
        /// <summary>
        /// Loads a sound effect either from the Content Pipeline or from disk (WAVs only).
        /// </summary>
        /// <returns>The sound effect.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        SoundEffect LoadSoundEffect(string contentPath);
        
        /// <summary>
        /// Tries to load a sound effect either from the Content Pipeline or from disk (WAVs only).
        /// </summary>
        /// <returns>The sound effect if it can be loaded, null otherwise.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        SoundEffect TryLoadSoundEffect(string contentPath);

        /// <summary>
        /// Loads a sprite font.
        /// </summary>
        /// <returns>The sprite font.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        SpriteFont LoadSpriteFont(string contentPath);

        /// <summary>
        /// Tries to load a sprite font.
        /// </summary>
        /// <returns>The sprite font if it can be loaded, null otherwise.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        SpriteFont TryLoadSpriteFont(string contentPath);

        /// <summary>
        /// Loads a two-dimensional bitmap texture.
        /// </summary>
        /// <returns>The 2D texture.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        Texture2D LoadTexture2D(string contentPath);

        /// <summary>
        /// Tries to load a two-dimensional bitmap texture.
        /// </summary>
        /// <returns>The 2D texture if it can be loaded, null otherwise.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        Texture2D TryLoadTexture2D(string contentPath);
    }
}
