using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.DataAccess.Resources;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;


namespace NuciXNA.Graphics.Drawing
{
    public class TextureSprite : Sprite
    {
        Texture2D alphaMask;

        string loadedContentFile;
        string loadedAlphaMaskFile;

        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        /// <value>The content file.</value>
        public string ContentFile { get; set; }

        /// <summary>
        /// Gets or sets the alpha mask path.
        /// </summary>
        /// <value>The alpha mask path.</value>
        public string AlphaMaskFile { get; set; }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <value>The texture.</value>
        [XmlIgnore]
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the texture size.
        /// </summary>
        /// <value>The texture size.</value>
        [XmlIgnore]
        public Size2D TextureSize => new Size2D(Texture.Width, Texture.Height);

        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        [XmlIgnore]
        public Rectangle2D SourceRectangle { get; set; }

        /// <summary>
        /// Gets the covered screen area.
        /// </summary>
        /// <value>The covered screen area.</value>
        public override Rectangle2D ClientRectangle
        {
            get
            {
                return new Rectangle2D(
                    Location.X,
                    Location.Y,
                    (int)(SourceRectangle.Width * Scale.Horizontal),
                    (int)(SourceRectangle.Height * Scale.Vertical));
            }
        }

        /// <summary>
        /// Gets or sets the fill mode.
        /// </summary>
        /// <value>The fill mode.</value>
        public TextureLayout TextureLayout { get; set; }

        /// <summary>
        /// Gets or sets the animation effect.
        /// </summary>
        /// <value>The animation effect.</value>
        public AnimationEffect AnimationEffect { get; set; }

        /// <summary>
        /// Gets or sets the sprite sheet effect.
        /// </summary>
        /// <value>The sprite sheet effect.</value>
        public SpriteSheetEffect SpriteSheetEffect { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public TextureSprite() : base()
        {
            ContentFile = string.Empty;
            AlphaMaskFile = string.Empty;

            SourceRectangle = Rectangle2D.Empty;
            TextureLayout = TextureLayout.Stretch;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            AnimationEffect?.LoadContent(this);
            SpriteSheetEffect?.LoadContent(this);

            LoadContentFile();
            LoadAlphaMaskFile();

            if (SpriteSize == Size2D.Empty)
            {
                Size2D size = Size2D.Empty;

                if (Texture != null)
                {
                    size.Width = Texture.Width;
                    size.Height = Texture.Height;
                }
                else
                {
                    size.Width = 1;
                    size.Height = 1;
                }

                SpriteSize = size;
            }

            if (SourceRectangle == Rectangle2D.Empty)
            {
                SourceRectangle = new Rectangle2D(Point2D.Empty, SpriteSize);
            }

            RenderTarget2D renderTarget = new RenderTarget2D(
                GraphicsManager.Instance.Graphics.GraphicsDevice,
                SpriteSize.Width, SpriteSize.Height);

            GraphicsManager.Instance.Graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(Color.Transparent);

            if (Texture != null)
            {
                GraphicsManager.Instance.SpriteBatch.Begin();
                GraphicsManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
                GraphicsManager.Instance.SpriteBatch.End();
            }

            Texture = renderTarget;

            GraphicsManager.Instance.Graphics.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            AnimationEffect?.UnloadContent();
            SpriteSheetEffect?.UnloadContent();

            Texture = null;
            alphaMask = null;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AnimationEffect?.Update(gameTime);
            SpriteSheetEffect?.Update(gameTime);

            LoadContentFile();
            LoadAlphaMaskFile();
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Point2D origin = new Point2D(SourceRectangle.Width / 2, SourceRectangle.Height / 2);

            // TODO: Do not do this for every Draw call
            Texture2D textureToDraw = Texture;

            // TODO: Find a better way to do this, because this one doesn't keep the mipmaps
            if (alphaMask != null)
            {
                textureToDraw = TextureBlend(Texture, alphaMask);
            }
            
            TextureDrawer.Draw(
                spriteBatch,
                textureToDraw,
                Location,
                SourceRectangle,
                Tint,
                ClientOpacity,
                ClientRotation,
                origin,
                ClientScale,
                TextureLayout);
        }

        Texture2D TextureBlend(Texture2D source, Texture2D mask)
        {
            Color[] textureBits = new Color[source.Width * source.Height];
            Color[] maskBits = new Color[mask.Width * mask.Height];

            source.GetData(textureBits);
            mask.GetData(maskBits);

            int startX, startY, endX, endY;

            if (mask.Width > source.Width)
            {
                startX = mask.Width - source.Width;
                endX = startX + source.Width;
            }
            else
            {
                startX = source.Width - mask.Width;
                endX = startX + mask.Width;
            }

            if (mask.Height > source.Height)
            {
                startY = mask.Height - source.Height;
                endY = startY + source.Height;
            }
            else
            {
                startY = source.Height - mask.Height;
                endY = startY + mask.Height;
            }

            Parallel.For(startY, endY, y => Parallel.For(startX, endX, x =>
            {
                int indexTexture = x - startX + (y - startY) * source.Width;
                int indexMask = x - startX + (y - startY) * mask.Width;

                textureBits[indexTexture] = Color.FromNonPremultiplied(
                    textureBits[indexTexture].R,
                    textureBits[indexTexture].G,
                    textureBits[indexTexture].B,
                    textureBits[indexTexture].A - 255 + maskBits[indexMask].R);
            }));

            Texture2D blendedTexture = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            blendedTexture.SetData(textureBits);

            return blendedTexture;
        }

        void LoadContentFile()
        {
            if (loadedContentFile == ContentFile || string.IsNullOrEmpty(ContentFile))
            {
                return;
            }

            Texture = ResourceManager.Instance.LoadTexture2D(ContentFile);

            loadedContentFile = ContentFile;
        }

        void LoadAlphaMaskFile()
        {
            if (loadedAlphaMaskFile == AlphaMaskFile || string.IsNullOrEmpty(AlphaMaskFile))
            {
                return;
            }

            alphaMask = ResourceManager.Instance.LoadTexture2D(AlphaMaskFile);

            loadedAlphaMaskFile = AlphaMaskFile;
        }
    }
}
