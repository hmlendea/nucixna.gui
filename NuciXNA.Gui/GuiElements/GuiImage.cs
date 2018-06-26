using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Primitives;

using NuciXNA.Graphics;
using NuciXNA.Graphics.SpriteEffects;
using NuciXNA.Graphics.Enumerations;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Image GUI element.
    /// </summary>
    public class GuiImage : GuiElement
    {
        Sprite sprite;

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
                if (_contentFile == null || _contentFile != value)
                {
                    OnContentFileChanged(this, null);
                }

                _contentFile = value;
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
                if (_maskFile == null || _maskFile != value)
                {
                    OnMaskFileChanged(this, null);
                }

                _maskFile = value;
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
                if (_tintColour == null || _tintColour != value)
                {
                    OnTintColourChanged(this, null);
                }

                _tintColour = value;
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
                if (_sourceRectangle == null || _sourceRectangle != value)
                {
                    OnSourceRectangleChanged(this, null);
                }

                _sourceRectangle = value;
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
                if (_textureLayout == null || _textureLayout != value)
                {
                    OnTextureLayoutChanged(this, null);
                }

                _textureLayout = value;
            }
        }

        /// <summary>
        /// Gets or sets the animation effect.
        /// </summary>
        /// <value>The animation effect.</value>
        public AnimationEffect AnimationEffect { get; set; }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        /// <value>The fade effect.</value>
        public FadeEffect FadeEffect { get; set; }

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
        /// Gets or sets the zoom effect.
        /// </summary>
        /// <value>The zoom effect.</value>
        public ZoomEffect ZoomEffect { get; set; }

        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the effects are active.
        /// </summary>
        /// <value><c>true</c> if the effects are active; otherwise, <c>false</c>.</value>
        public bool EffectsActive { get; set; }

        public event EventHandler ContentFileChanged;

        public event EventHandler MaskFileChanged;

        public event EventHandler TintColourChanged;

        public event EventHandler SourceRectangleChanged;

        public event EventHandler TextureLayoutChanged;
        
        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            this.sprite = new Sprite();

            SetChildrenProperties();

            sprite.LoadContent();

            base.LoadContent();

            if (SourceRectangle == Rectangle2D.Empty)
            {
                SourceRectangle = new Rectangle2D(Point2D.Empty, sprite.SpriteSize);
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            sprite.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            if (Size == Size2D.Empty)
            {
                Size = sprite.SourceRectangle.Size;
            }

            sprite.Active = EffectsActive;
            sprite.AlphaMaskFile = MaskFile;
            sprite.ContentFile = ContentFile;
            sprite.Location = ScreenLocation;
            sprite.SourceRectangle = SourceRectangle;
            sprite.Rotation = Rotation;
            sprite.TextureLayout = TextureLayout;
            sprite.Tint = TintColour;

            sprite.AnimationEffect = AnimationEffect;
            sprite.FadeEffect = FadeEffect;
            sprite.RotationEffect = RotationEffect;
            sprite.SpriteSheetEffect = SpriteSheetEffect;
            sprite.ZoomEffect = ZoomEffect;

            if (!sprite.SourceRectangle.IsEmpty)
            {
                sprite.Scale = new Scale2D(
                    (float)Size.Width / sprite.SourceRectangle.Width,
                    (float)Size.Height / sprite.SourceRectangle.Height);
            }
        }


        protected virtual void OnContentFileChanged(object sender, EventArgs e)
        {
            ContentFileChanged?.Invoke(sender, e);
        }

        protected virtual void OnMaskFileChanged(object sender, EventArgs e)
        {
            MaskFileChanged?.Invoke(sender, e);
        }

        protected virtual void OnTintColourChanged(object sender, EventArgs e)
        {
            TintColourChanged?.Invoke(sender, e);
        }

        protected virtual void OnSourceRectangleChanged(object sender, EventArgs e)
        {
            SourceRectangleChanged?.Invoke(sender, e);
        }

        protected virtual void OnTextureLayoutChanged(object sender, EventArgs e)
        {
            TextureLayoutChanged?.Invoke(sender, e);
        }
    }
}
