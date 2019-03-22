using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui
{
    /// <summary>
    /// GUI manager.
    /// </summary>
    public class GuiManager
    {
        static volatile GuiManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets or sets the GUI elements.
        /// </summary>
        /// <value>The GUI elements.</value>
        public List<GuiElement> GuiElements { get; set; }

        public Colour DefaultBackgroundColour { get; set; }

        public Colour DefaultForegroundColour { get; set; }

        public string DefaultFontName { get; set; }

        public TextureLayout DefaultTextureLayout { get; set; }

        public int DefaultMargins { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static GuiManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new GuiManager();
                        }
                    }
                }

                return instance;
            }
        }

        public GuiManager()
        {
            GuiElements = new List<GuiElement>();

            DefaultBackgroundColour = Colour.Transparent;
            DefaultForegroundColour = Colour.Black;
            DefaultFontName = "DefaultFont";
            DefaultTextureLayout = TextureLayout.Stretch;
            DefaultMargins = 8;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            GuiElements.ToList().ForEach(w => w.LoadContent());
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            GuiElements.ForEach(w => w.UnloadContent());
            GuiElements.Clear();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            GuiElements.RemoveAll(e => e.IsDisposed);

            foreach (GuiElement guiElement in GuiElements.Where(e => e.IsEnabled).Reverse())
            {
                if (InputManager.Instance.MouseButtonInputHandled)
                {
                    break;
                }

                guiElement.HandleInput();
            }

            InputManager.Instance.MouseButtonInputHandled = false;

            IEnumerable<GuiElement> elementsToUpdate = GuiElements.Where(x =>
                x.IsContentLoaded &&
                x.IsEnabled);

            foreach (GuiElement guiElement in elementsToUpdate)
            {
                guiElement.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            IEnumerable<GuiElement> elementsToDraw = GuiElements.Where(x =>
                x.IsContentLoaded &&
                x.IsVisible);

            foreach (GuiElement guiElement in elementsToDraw)
            {
                guiElement.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Focuses the input on the element with the specified identifier.
        /// </summary>
        /// <param name="id">Element identifier.</param>
        public void FocusElement(string id)
        {
            GuiElements.ForEach(e => e.IsFocused = false);
            GuiElements.FirstOrDefault(e => e.Id == id).IsFocused = true;
        }

        /// <summary>
        /// Focuses the input on the specified element.
        /// </summary>
        /// <param name="element">Element.</param>
        public void FocusElement(GuiElement element)
        {
            FocusElement(element.Id);
        }
    }
}
