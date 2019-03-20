using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
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
        public List<GuiMenuItem> Items { get; set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        /// <value>The item number.</value>
        public int SelectedItemIndex { get; private set; }

        bool lastDirectionBack;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuScreen"/> class.
        /// </summary>
        public MenuScreen()
        {
            Id = string.Empty;
            SelectedItemIndex = 0;
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
            int newSelectedItemIndex = GetNormalisedItemNumber(SelectedItemIndex);

            if (newSelectedItemIndex != SelectedItemIndex)
            {
                GuiManager.Instance.FocusElement(Items[newSelectedItemIndex]);

                SelectedItemIndex = newSelectedItemIndex;
            }

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
            Size2D halfSpacingSize = new Size2D(Spacing);
            Size2D dimensions = Size2D.Empty;

            foreach (GuiMenuItem menuItem in Items)
            {
                dimensions += menuItem.Size + halfSpacingSize;
            }

            dimensions = (ScreenManager.Instance.Size - dimensions) / 2;

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

            int index = Items.FindIndex(x =>
                x.Selectable &&
                x.DisplayRectangle.Contains(e.Location));

            if (index >= 0)
            {
                SelectedItemIndex = index;
            }
        }

        protected override void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            base.OnKeyPressed(sender, e);

            List<Keys> backKeys;
            List<Keys> forwardKeys;

            if (Axis == MenuScreenAxis.Vertical)
            {
                backKeys = new List<Keys> { Keys.W, Keys.Up };
                forwardKeys = new List<Keys> { Keys.S, Keys.Down };
            }
            else
            {
                backKeys = new List<Keys> { Keys.A, Keys.Left};
                forwardKeys = new List<Keys> { Keys.D, Keys.Right };
            }

            if (backKeys.Contains(e.Key))
            {
                SelectedItemIndex -= 1;
                lastDirectionBack = true;
            }
            else if (forwardKeys.Contains(e.Key))
            {
                SelectedItemIndex += 1;
                lastDirectionBack = false;
            }

            SelectedItemIndex = GetNormalisedItemNumber(SelectedItemIndex);
        }

        int GetNormalisedItemNumber(int itemNumber)
        {
            int normalisedItemNumber = GetSafeItemNumber(itemNumber);

            if (!Items[normalisedItemNumber].Selectable && Items.Any(x => x.Selectable))
            {
                if (lastDirectionBack)
                {
                    normalisedItemNumber -= 1;
                }
                else
                {
                    normalisedItemNumber += 1;
                }
            }

            return GetSafeItemNumber(normalisedItemNumber);
        }

        int GetSafeItemNumber(int itemNumber)
        {
            int normalisedItemNumber = itemNumber;

            if (normalisedItemNumber < 0)
            {
                normalisedItemNumber = Items.Count - 1;
            }
            else if (normalisedItemNumber >= Items.Count)
            {
                normalisedItemNumber = 0;
            }

            return normalisedItemNumber;
        }
    }
}
