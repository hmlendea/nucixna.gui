using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Gui.Screens
{
    /// <summary>
    /// Menu screen.
    /// </summary>
    public class MenuScreen : Screen
    {
        /// <summary>
        /// Gets or sets the axis.
        /// </summary>
        /// <value>The axis.</value>
        public MenuScreenAxis Axis { get; set; }

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public int Spacing { get; set; }
        
        /// <summary>
        /// Gets all the items.
        /// </summary>
        /// <value>The items.</value>
        [XmlIgnore]
        public List<GuiMenuItem> Items { get; set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        /// <value>The item number.</value>
        [XmlIgnore]
        public int ItemNumber { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        public MenuScreen()
        {
            Id = string.Empty;
            ItemNumber = 0;
            Axis = MenuScreenAxis.Vertical;
            Spacing = 32;

            Items = new List<GuiMenuItem>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            GuiManager.Instance.GuiElements.AddRange(Items);

            base.LoadContent();

            AlignMenuItems();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            int newSelectedItemIndex = ItemNumber;

            if (newSelectedItemIndex < 0)
            {
                newSelectedItemIndex = 0;
            }
            else if (newSelectedItemIndex > Items.Count - 1 && Items.Count > 0)
            {
                newSelectedItemIndex = Items.Count - 1;
            }

            GuiManager.Instance.FocusElement(Items[newSelectedItemIndex]);

            ItemNumber = newSelectedItemIndex;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        void AlignMenuItems()
        {
            Size2D dimensions = Size2D.Empty;

            Items.ForEach(item => dimensions += new Size2D(
                item.Size.Width + Spacing / 2,
                item.Size.Height + Spacing / 2));

            dimensions = new Size2D(
                (ScreenManager.Instance.Size.Width - dimensions.Width) / 2,
                (ScreenManager.Instance.Size.Height - dimensions.Height) / 2);

            foreach (GuiMenuItem item in Items)
            {
                if (Axis == MenuScreenAxis.Vertical)
                {
                    item.Location = new Point2D(
                        (ScreenManager.Instance.Size.Width - item.Size.Width) / 2,
                        dimensions.Height);
                }
                else if (Axis == MenuScreenAxis.Horizontal)
                {
                    item.Location = new Point2D(
                        dimensions.Width,
                        (ScreenManager.Instance.Size.Height - item.Size.Height) / 2);
                }

                dimensions += new Size2D(
                    item.Size.Width + Spacing / 2,
                    item.Size.Height + Spacing / 2);
            }
        }

        protected override void OnMouseMoved(object sender, MouseEventArgs e)
        {
            base.OnMouseMoved(sender, e);

            // TODO: Optimise this
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].DisplayRectangle.Contains(e.Location.ToPoint2D()))
                {
                    ItemNumber = i;
                }
            }
        }

        protected override void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            base.OnKeyPressed(sender, e);

            if (Axis == MenuScreenAxis.Vertical)
            {
                if (e.Key == Keys.W || e.Key == Keys.Up)
                {
                    ItemNumber -= 1;
                }
                else if (e.Key == Keys.S || e.Key == Keys.Down)
                {
                    ItemNumber += 1;
                }
            }
            else if (Axis == MenuScreenAxis.Horizontal)
            {
                if (e.Key == Keys.D || e.Key == Keys.Right)
                {
                    ItemNumber -= 1;
                }
                else if (e.Key == Keys.A || e.Key == Keys.Left)
                {
                    ItemNumber += 1;
                }
            }
        }
    }
}
