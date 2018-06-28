using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.DataAccess.Resources;
using NuciXNA.Graphics.Enumerations;
using NuciXNA.Graphics.Helpers;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Primitives;


namespace NuciXNA.Graphics.Drawing
{
    public class TextSprite : Sprite
    {
        SpriteFont font;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName { get; set; }

        // TODO: Make this a number (Outline size)
        /// <summary>
        /// Gets or sets a value indicating whether the text of the <see cref="Sprite"/> will be outlined.
        /// </summary>
        /// <value><c>true</c> if the text is outlined; otherwise, <c>false</c>.</value>
        public FontOutline FontOutline { get; set; }

        /// <summary>
        /// Gets or sets the text horizontal alignment.
        /// </summary>
        /// <value>The text horizontal alignment.</value>
        public HorizontalAlignment TextHorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the text vertical alignment.
        /// </summary>
        /// <value>The text vertical alignment.</value>
        public VerticalAlignment TextVerticalAlignment { get; set; }

        /// <summary>
        /// Gets the covered screen area.
        /// </summary>
        /// <value>The covered screen area.</value>
        public override Rectangle2D ClientRectangle
            => new Rectangle2D(Location, SpriteSize);

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public TextSprite() : base()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(FontName))
            {
                font = ResourceManager.Instance.LoadSpriteFont("Fonts/" + FontName);
            }

            if (SpriteSize == Size2D.Empty)
            {
                Size2D size = Size2D.Empty;
                
                if (!string.IsNullOrEmpty(Text))
                {
                    size.Width = (int)font.MeasureString(Text).X;
                    size.Height = (int)font.MeasureString(Text).Y;
                }
                else
                {
                    size.Width = 1;
                    size.Height = 1;
                }

                SpriteSize = size;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Text = string.Empty;
            font = null;
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            Colour colour = Tint;
            colour.A = (byte)(colour.A * Opacity);

            StringDrawer.Draw(
                spriteBatch,
                font,
                StringUtils.WrapText(font, Text, SpriteSize.Width),
                ClientRectangle,
                colour,
                TextHorizontalAlignment,
                TextVerticalAlignment,
                FontOutline);
        }
    }
}
