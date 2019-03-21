using System;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NuciXNA.DataAccess.Content
{
    /// <summary>
    /// A <see cref="ContentManager"> alternative that can load content either from the Content Pipeline or from plain disk files.
    /// </summary>
    public class NuciContentManager
    {
        static volatile NuciContentManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static NuciContentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new NuciContentManager();
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
        
        private IContentLoader pipelineContentLoader;

        private IContentLoader plainFileContentLoader;

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="pipelineContentLoader">The XNA Pipeline content loader.</param>
        /// <param name="plainFileContentLoader">The plain disk files content loader.</param>
        public void LoadContent(IContentLoader pipelineContentLoader, IContentLoader plainFileContentLoader)
        {
            this.pipelineContentLoader = pipelineContentLoader;
            this.plainFileContentLoader = plainFileContentLoader;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="content">Content manager.</param>
        /// <param name="graphicsDevice">Graphics device.</param>
        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.pipelineContentLoader = new PipelineContentLoader(content);
            this.plainFileContentLoader = new PlainFileContentLoader(graphicsDevice);
        }

        /// <summary>
        /// Loads a sound effect either from the Content Pipeline or from plain files (WAVs only).
        /// </summary>
        /// <returns>The sound effect.</returns>
        /// <param name="contentFile">The path to the content (without extension).</param>
        public SoundEffect LoadSoundEffect(string contentFile)
        {
            SoundEffect soundEffect = pipelineContentLoader.TryLoadSoundEffect(contentFile);

            if (soundEffect is null)
            {
                soundEffect = plainFileContentLoader.LoadSoundEffect(contentFile);
            }

            return soundEffect;
        }

        /// <summary>
        /// Loads a sprite font from the Content Pipeline.
        /// </summary>
        /// <returns>The sprite font.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        public SpriteFont LoadSpriteFont(string contentPath)
        {
            SpriteFont spriteFont = pipelineContentLoader.LoadSpriteFont(contentPath);

            return spriteFont;
        }

        /// <summary>
        /// Loads a 2D texture either from the Content Pipeline or from disk (PNGs only).
        /// </summary>
        /// <returns>The 2D texture.</returns>
        /// <param name="contentPath">The path to the content (without extension).</param>
        public Texture2D LoadTexture2D(string contentPath)
        {
            Texture2D texture2d = pipelineContentLoader.TryLoadTexture2D(contentPath);

            if (texture2d is null)
            {
                if (string.IsNullOrWhiteSpace(MissingTexturePlaceholder))
                {
                    texture2d = plainFileContentLoader.LoadTexture2D(contentPath);
                }
                else
                {
                    texture2d = plainFileContentLoader.TryLoadTexture2D(contentPath);
                }
            }

            if (texture2d is null &&
                !string.IsNullOrWhiteSpace(MissingTexturePlaceholder))
            {
                texture2d = pipelineContentLoader.LoadTexture2D(MissingTexturePlaceholder);
            }
            
            return texture2d;
        }
    }
}
