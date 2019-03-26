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

        Dictionary<string, GuiElement> guiElements;

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
            guiElements = new Dictionary<string, GuiElement>();

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
            foreach (GuiElement element in guiElements.Values)
            {
                element.LoadContent();
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            foreach (GuiElement element in guiElements.Values)
            {
                element.UnloadContent();
            }

            guiElements.Clear();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            RemoveDisposedElements();

            IEnumerable<GuiElement> enabledElements = guiElements.Values.Where(e => e.IsEnabled);
            
            foreach (GuiElement guiElement in enabledElements.Reverse())
            {
                if (InputManager.Instance.MouseButtonInputHandled)
                {
                    break;
                }

                guiElement.HandleInput();
            }

            InputManager.Instance.MouseButtonInputHandled = false;

            IEnumerable<GuiElement> elementsToUpdate = guiElements.Values.Where(x =>
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
            IEnumerable<GuiElement> elementsToDraw = guiElements.Values.Where(x =>
                x.IsContentLoaded &&
                x.IsVisible);

            foreach (GuiElement guiElement in elementsToDraw)
            {
                guiElement.Draw(spriteBatch);
            }
        }

        public void RegisterElements(params GuiElement[] elements)
            => RegisterElements(elements.ToList());

        public void RegisterElements(IEnumerable<GuiElement> elements)
        {
            foreach (GuiElement element in elements)
            {
                guiElements.Add(element.Id, element);
            }
        }

        /// <summary>
        /// Focuses the input on the specified element.
        /// </summary>
        /// <param name="element">Element.</param>
        public void FocusElement(GuiElement element)
            => FocusElement(element.Id);

        /// <summary>
        /// Focuses the input on the element with the specified identifier.
        /// </summary>
        /// <param name="id">Element identifier.</param>
        public void FocusElement(string id)
        {
            foreach (GuiElement element in guiElements.Values)
            {
                if (element.IsFocused)
                {
                    element.Unfocus();
                }

                if (element.Id == id && !element.IsFocused)
                {
                    element.Focus();
                }
            }
        }

        void RemoveDisposedElements()
        {
            IEnumerable<string> disposedElementsKeys = guiElements.Keys.Where(key => guiElements[key].IsDisposed);

            foreach (string key in disposedElementsKeys)
            {
                guiElements.Remove(key);
            }
        }
    }
}
