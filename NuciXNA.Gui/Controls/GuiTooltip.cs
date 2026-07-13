using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Primitives;

using NuciXNA.Gui.Screens;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// Tool tip GUI Control.
    /// </summary>
    public class GuiTooltip : GuiControl, IGuiControl
    {
        private string textContent;
        private int borderSize;

        private GuiImage border;
        private GuiImage background;
        private GuiText text;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => textContent;
            set
            {
                textContent = value;

                PropertyChangedEventArgs eventArguments = new(nameof(Text));
                TextChanged?.Invoke(this, eventArguments);
            }
        }

        public int BorderSize
        {
            get => borderSize;
            set
            {
                borderSize = value;

                PropertyChangedEventArgs eventArguments = new(nameof(BorderSize));
                BorderSizeChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Text"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler TextChanged;

        /// <summary>
        /// Occurs when the <see cref="BorderSize"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler BorderSizeChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiTooltip"/> class.
        /// </summary>
        public GuiTooltip()
        {
            FontName = "MenuFont";
            IsVisible = false;

            BorderSize = 1;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            border = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile
            };
            background = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile
            };
            text = new GuiText
            {
                Margins = 2,
                BackgroundColour = Colour.Transparent
            };

            RegisterChild(border);
            RegisterChild(background);
            RegisterChild(text);

            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent() { }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime) => SetChildrenProperties();

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch) { }

        private void SetChildrenProperties()
        {
            if (ScreenLocation.X + Size.Width > ScreenManager.Instance.Size.Width)
            {
                Location = new Point2D(Location.X - (ScreenLocation.X + Size.Width - ScreenManager.Instance.Size.Width), Location.Y);
            }
            else if (ScreenLocation.X < 0)
            {
                Location = new Point2D(Location.X - ScreenLocation.X, Location.Y);
            }

            if (ScreenLocation.Y + Size.Height > ScreenManager.Instance.Size.Height)
            {
                Location = new Point2D(Location.X, Location.Y - (ScreenLocation.Y + Size.Height - ScreenManager.Instance.Size.Height));
            }
            else if (ScreenLocation.Y < 0)
            {
                Location = new Point2D(Location.X, Location.Y - ScreenLocation.Y);
            }

            border.TintColour = ForegroundColour;
            background.TintColour = BackgroundColour;
            background.Size = new Size2D(Size.Width - BorderSize * 2, Size.Height - BorderSize * 2);
            background.Location = new Point2D(border.Location.X + BorderSize, border.Location.Y + BorderSize);
            text.Text = Text;
        }
    }
}
