using System;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Primitives;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Graphics.SpriteEffects;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// Text GUI Control.
    /// </summary>
    public class GuiText : GuiControl, IGuiControl
    {
        GuiImage backgroundImage;
        TextSprite textSprite;

        string _text;
        Colour _outlineColour;
        FontOutline _fontOutline;
        Alignment _horizontalAlignment;
        Alignment _verticalAlignment;
        int _margins;
        
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(Text));
                TextChanged?.Invoke(this, eventArguments);
            }
        }

        public Colour OutlineColour
        {
            get => _outlineColour;
            set
            {
                _outlineColour = value;
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(OutlineColour));
                OutlineColourChanged?.Invoke(this, eventArguments);
            }
        }

        public FontOutline FontOutline
        {
            get => _fontOutline;
            set
            {
                _fontOutline = value;
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(FontOutline));
                FontOutlineChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text.
        /// </summary>
        /// <value>The horizontal alignment.</value>
        public Alignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(HorizontalAlignment));
                HorizontalAlignmentChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        /// <value>The vertical alignment.</value>
        public Alignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(VerticalAlignment));
                VerticalAlignmentChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the margins.
        /// </summary>
        /// <value>The margins.</value>
        public int Margins
        {
            get => _margins;
            set
            {
                _margins = value;
                
                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(Margins));
                MarginsChanged?.Invoke(this, eventArguments);
            }
        }

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

        /// <summary>
        /// Occurs when the <see cref="Text"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler TextChanged;

        /// <summary>
        /// Occurs when the <see cref="OutlineColour"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler OutlineColourChanged;

        /// <summary>
        /// Occurs when the <see cref="FontOutline"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler FontOutlineChanged;

        /// <summary>
        /// Occurs when the <see cref="HorizontalAlignment"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler HorizontalAlignmentChanged;

        /// <summary>
        /// Occurs when the <see cref="VerticalAlignment"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler VerticalAlignmentChanged;

        /// <summary>
        /// Occurs when the <see cref="Margins"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler MarginsChanged;

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
            
            RegisterChild(backgroundImage);

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
