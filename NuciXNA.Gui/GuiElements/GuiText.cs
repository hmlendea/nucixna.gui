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
                    OnTextChanged(this, null);
                }
            }
        }

        public FontOutline FontOutline { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        /// <value>The vertical alignment.</value>
        public VerticalAlignment VerticalAlignment { get; set; }

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
        public bool EffectsActive { get; set; }

        public event EventHandler TextChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiText"/> class.
        /// </summary>
        public GuiText()
        {
            HorizontalAlignment = HorizontalAlignment.Centre;
            VerticalAlignment = VerticalAlignment.Centre;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            backgroundImage = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile,
                Size = new Size2D(
                    Size.Width + Margins * 2,
                    Size.Height + Margins * 2)
            };

            textSprite = new TextSprite();
            
            base.LoadContent();
            textSprite.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            textSprite.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            textSprite.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            textSprite.Draw(spriteBatch);
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();
            
            AddChild(backgroundImage);
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            backgroundImage.TintColour = BackgroundColour;
            backgroundImage.Size = new Size2D(
                Size.Width + Margins * 2,
                Size.Height + Margins * 2);

            textSprite.Text = Text;
            textSprite.FontName = FontName;
            textSprite.FontOutline = FontOutline;
            textSprite.Tint = ForegroundColour;
            textSprite.TextVerticalAlignment = VerticalAlignment;
            textSprite.TextHorizontalAlignment = HorizontalAlignment;
            textSprite.Location = new Point2D(
                ScreenLocation.X + Margins,
                ScreenLocation.Y + Margins);
            textSprite.SpriteSize = new Size2D(
                Size.Width - Margins * 2,
                Size.Height - Margins * 2);
            textSprite.FadeEffect = FadeEffect;
            textSprite.Active = EffectsActive;
        }

        protected virtual void OnTextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }
    }
}
