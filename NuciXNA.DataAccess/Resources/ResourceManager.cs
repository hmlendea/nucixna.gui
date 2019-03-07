using System;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.DataAccess.Extensions;

namespace NuciXNA.DataAccess.Resources
{
    /// <summary>
    /// Resource Manager that can load content either from the Content Pipeline or from disk.
    /// </summary>
    public class ResourceManager
    {
        static volatile ResourceManager instance;
        static object syncRoot = new object();

        ContentManager content;
        GraphicsDevice graphicsDevice { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ResourceManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets placeholder used when a texture is missing.
        /// If a value is set, this texture will be displayed when the desired one is missing, instead of throwing an exception.
        /// </summary>
        /// <value>The path to the content file.</value>
        public static string MissingTexturePlaceholder { get; set; }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="content">Content manager.</param>
        /// <param name="graphicsDevice">Graphics device.</param>
        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Loads a sound effect either from the Content Pipeline or from disk (WAVs only).
        /// </summary>
        /// <returns>The sound effect.</returns>
        /// <param name="contentPath">The path to the file (without extension).</param>
        public SoundEffect LoadSoundEffect(string contentPath)
        {
            SoundEffect soundEffect = content.TryLoad<SoundEffect>(contentPath);

            if (soundEffect is null)
            {
                soundEffect = LoadSoundEffectFromFile(contentPath);
            }

            return soundEffect;
        }

        /// <summary>
        /// Loads a sprite font from the Content Pipeline.
        /// </summary>
        /// <returns>The sprite font.</returns>
        /// <param name="contentPath">The path to the file (without extension).</param>
        public SpriteFont LoadSpriteFont(string contentPath)
            => content.Load<SpriteFont>(contentPath);

        /// <summary>
        /// Loads a 2D texture either from the Content Pipeline or from disk (PNGs only).
        /// </summary>
        /// <returns>The 2D texture.</returns>
        /// <param name="contentPath">The path to the file (without extension).</param>
        public Texture2D LoadTexture2D(string contentPath)
        {
            Texture2D texture2D = content.TryLoad<Texture2D>(contentPath);

            if (!(texture2D is null))
            {
                return texture2D;
            }

            try
            {
                texture2D = LoadTexture2DFromFile(contentPath);
            }
            catch
            {
                if (string.IsNullOrWhiteSpace(MissingTexturePlaceholder))
                {
                    throw;
                }

                texture2D = content.Load<Texture2D>(MissingTexturePlaceholder);
            }
            
            return texture2D;
        }

        private SoundEffect LoadSoundEffectFromFile(string filePath)
        {
            FileStream fileStream = GetContentFileStream($"{filePath}.wav");
            SoundEffect soundEffect = SoundEffect.FromStream(fileStream);

            return soundEffect;
        }

        private Texture2D LoadTexture2DFromFile(string filePath)
        {
            FileStream fileStream = GetContentFileStream($"{filePath}.png");
            Texture2D texture2D = Texture2D.FromStream(graphicsDevice, fileStream);

            return texture2D;
        }

        private FileStream GetContentFileStream(string filePath)
        {
            string fullFilePath = Path.Combine(content.RootDirectory, filePath);

            FileStream fileStream = File.OpenRead(fullFilePath);

            return fileStream;
        }
    }
}
