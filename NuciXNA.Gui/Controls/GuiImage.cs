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
    /// Image GUI Control.
    /// </summary>
    public class GuiImage : GuiControl, IGuiControl
    {
        TextureSprite sprite;

        string _contentFile;
        string _maskFile;
        Colour _tintColour;
        Rectangle2D? _sourceRectangle;
        TextureLayout? _textureLayout;

        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        /// <value>The content file.</value>
        public string ContentFile
        {
            get
            {
                if (_contentFile != null)
                {
                    return _contentFile;
                }

                return string.Empty;
            }
            set
            {
                _contentFile = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(ContentFile));
                ContentFileChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the mask file.
        /// </summary>
        /// <value>The mask file.</value>
        public string MaskFile
        {
            get
            {
                if (_maskFile != null)
                {
                    return _maskFile;
                }

                return string.Empty;
            }
            set
            {
                _maskFile = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(MaskFile));
                MaskFileChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the tint colour.
        /// </summary>
        /// <value>The tint colour.</value>
        public Colour TintColour
        {
            get
            {
                if (_tintColour != null)
                {
                    return _tintColour;
                }
                
                return Colour.White;
            }
            set
            {
                _tintColour = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(TintColour));
                TintColourChanged?.Invoke(this, eventArguments);
            }
        }
        
        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>The source rectangle.</value>
        public Rectangle2D SourceRectangle
        {
            get
            {
                if (_sourceRectangle != null)
                {
                    return (Rectangle2D)_sourceRectangle;
                }

                return Rectangle2D.Empty;
            }
            set
            {
                _sourceRectangle = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(SourceRectangle));
                SourceRectangleChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the texture fill mode.
        /// </summary>
        /// <value>The fill mode.</value>
        public TextureLayout TextureLayout
        {
            get
            {
                if (_textureLayout != null)
                {
                    return (TextureLayout)_textureLayout;
                }

                return GuiManager.Instance.DefaultTextureLayout;
            }
            set
            {
                _textureLayout = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(TextureLayout));
                TextureLayoutChanged?.Invoke(this, eventArguments);
            }
        }
        
        /// <summary>
        /// Gets or sets the opacity effect.
        /// </summary>
        /// <value>The opacity effect.</value>
        public OpacityEffect OpacityEffect { get; set; }

        /// <summary>
        /// Gets or sets the rotation effect.
        /// </summary>
        /// <value>The rotation effect.</value>
        public RotationEffect RotationEffect { get; set; }

        /// <summary>
        /// Gets or sets the sprite sheet effect.
        /// </summary>
        /// <value>The sprite sheet effect.</value>
        public SpriteSheetEffect SpriteSheetEffect { get; set; }

        /// <summary>
        /// Gets or sets the scale effect.
        /// </summary>
        /// <value>The scale effect.</value>
        public ScaleEffect ScaleEffect { get; set; }

        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the effects are active.
        /// </summary>
        /// <value><c>true</c> if the effects are active; otherwise, <c>false</c>.</value>
        public bool AreEffectsActive { get; set; }

        /// <summary>
        /// Occurs when the <see cref="ContentFile"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler ContentFileChanged;

        /// <summary>
        /// Occurs when the <see cref="MaskFile"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler MaskFileChanged;

        /// <summary>
        /// Occurs when the <see cref="TintColour"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler TintColourChanged;

        /// <summary>
        /// Occurs when the <see cref="SourceRectangle"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler SourceRectangleChanged;

        /// <summary>
        /// Occurs when the <see cref="TextureLayout"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler TextureLayoutChanged;
        
        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            this.sprite = new TextureSprite();

            SetChildrenProperties();

            sprite.LoadContent();

            if (SourceRectangle == Rectangle2D.Empty)
            {
                SourceRectangle = new Rectangle2D(Point2D.Empty, sprite.SpriteSize);
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            sprite.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            sprite.Update(gameTime);
            SetChildrenProperties();
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        void SetChildrenProperties()
        {
            if (Size == Size2D.Empty)
            {
                Size = sprite.SourceRectangle.Size;
            }

            sprite.IsActive = AreEffectsActive;
            sprite.AlphaMaskFile = MaskFile;
            sprite.ContentFile = ContentFile;
            sprite.Location = ScreenLocation;
            sprite.SourceRectangle = SourceRectangle;
            sprite.Rotation = Rotation;
            sprite.TextureLayout = TextureLayout;
            sprite.Tint = TintColour;
            
            sprite.OpacityEffect = OpacityEffect;
            sprite.RotationEffect = RotationEffect;
            sprite.SpriteSheetEffect = SpriteSheetEffect;
            sprite.ScaleEffect = ScaleEffect;

            if (!sprite.SourceRectangle.IsEmpty)
            {
                sprite.Scale = new Scale2D(
                    (float)Size.Width / sprite.SourceRectangle.Width,
                    (float)Size.Height / sprite.SourceRectangle.Height);
            }
        }
    }
}
