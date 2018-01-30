using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        /// <param name="filePath">The path to the file (without extension).</param>
        public SoundEffect LoadSoundEffect(string filePath)
        {
            SoundEffect soundEffect;

            try
            {
                soundEffect = content.Load<SoundEffect>(filePath);
            }
            catch
            {
                soundEffect = SoundEffect.FromStream(File.OpenRead(Path.Combine(content.RootDirectory, $"{filePath}.wav")));
            }

            return soundEffect;
        }

        /// <summary>
        /// Loads a sprite font from the Content Pipeline.
        /// </summary>
        /// <returns>The sprite font.</returns>
        /// <param name="filePath">The path to the file (without extension).</param>
        public SpriteFont LoadSpriteFont(string filePath)
        => content.Load<SpriteFont>(filePath);

        /// <summary>
        /// Loads a 2D texture either from the Content Pipeline or from disk (PNGs only).
        /// </summary>
        /// <returns>The 2D texture.</returns>
        /// <param name="contentPath">The path to the file (without extension).</param>
        public Texture2D LoadTexture2D(string contentPath)
        {
            Texture2D texture2D = null;

            try
            {
                texture2D = content.Load<Texture2D>(contentPath);
            }
            catch
            {
                string diskFilePath = Path.Combine(content.RootDirectory, $"{contentPath}.png");

                if (File.Exists(diskFilePath))
                {
                    texture2D = Texture2D.FromStream(graphicsDevice, File.OpenRead(diskFilePath));
                }
            }

            if (texture2D == null)
            {
                if (!string.IsNullOrWhiteSpace(MissingTexturePlaceholder))
                {
                    texture2D = content.Load<Texture2D>(MissingTexturePlaceholder);

                    //string logMessage = "The repository cannot be accessed";
                    // TODO: Log the an error
                }
                else
                {
                    throw new ContentLoadException($"Could not find the desired content file: {contentPath}");
                }
            }

            return texture2D;
        }
    }
}
