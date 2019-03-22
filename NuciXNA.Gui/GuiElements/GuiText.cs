using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Primitives;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Graphics.SpriteEffects;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Text GUI element.
    /// </summary>
    public class GuiText : GuiElement
    {
        GuiImage backgroundImage;
        TextSprite textSprite;

        string _text;
        
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                if (_text != null)
                {
                    return _text;
                }
                
                return string.Empty;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    TextChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Colour OutlineColour { get; set; }

        public FontOutline FontOutline { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public Alignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        /// <value>The vertical alignment.</value>
        public Alignment VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the margins.
        /// </summary>
        /// <value>The margins.</value>
        public int Margins { get; set; }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        /// <value>The fade effect.</value>
        public FadeEffect FadeEffect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the effects are active.
        /// </summary>
        /// <value><c>true</c> if the effects are active; otherwise, <c>false</c>.</value>
        public bool AreEffectsActive { get; set; }

        public event EventHandler TextChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiText"/> class.
        /// </summary>
        public GuiText()
        {
            HorizontalAlignment = Alignment.Middle;
            VerticalAlignment = Alignment.Middle;
            OutlineColour = Colour.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            backgroundImage = new GuiImage
            {
                Id = $"{Id}_{nameof(backgroundImage)}",
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile,
                Size = new Size2D(
                    Size.Width + Margins * 2,
                    Size.Height + Margins * 2)
            };

            textSprite = new TextSprite
            {
                OpacityEffect = FadeEffect
            };
            
            AddChild(backgroundImage);

            SetChildrenProperties();

            textSprite.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            textSprite.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
            textSprite.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            textSprite.Draw(spriteBatch);
        }

        void SetChildrenProperties()
        {
            backgroundImage.TintColour = BackgroundColour;
            backgroundImage.Size = new Size2D(
                Size.Width + Margins * 2,
                Size.Height + Margins * 2);
            backgroundImage.AreEffectsActive = AreEffectsActive;

            textSprite.Text = Text;
            textSprite.FontName = FontName;
            textSprite.OutlineColour = OutlineColour;
            textSprite.FontOutline = FontOutline;
            textSprite.Tint = ForegroundColour;
            textSprite.Opacity = Opacity;
            textSprite.VerticalAlignment = VerticalAlignment;
            textSprite.HorizontalAlignment = HorizontalAlignment;
            textSprite.Location = new Point2D(ScreenLocation.X + Margins, ScreenLocation.Y + Margins);
            textSprite.SpriteSize = new Size2D(Size.Width - Margins * 2, Size.Height - Margins * 2);
            textSprite.IsActive = AreEffectsActive;
        }
    }
}
