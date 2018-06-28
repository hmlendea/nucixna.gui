using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// Tool tip GUI element.
    /// </summary>
    public class GuiTooltip : GuiElement
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public int BorderSize { get; set; }

        GuiImage border;
        GuiImage background;
        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiTooltip"/> class.
        /// </summary>
        public GuiTooltip()
        {
            FontName = "MenuFont";
            Visible = false;

            BorderSize = 1;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
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

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(border);
            AddChild(background);
            AddChild(text);
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

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
