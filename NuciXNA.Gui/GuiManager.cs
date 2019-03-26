using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
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

        Dictionary<string, GuiControl> guiControls;

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
            guiControls = new Dictionary<string, GuiControl>();

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
            foreach (GuiControl control in guiControls.Values)
            {
                control.LoadContent();
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            foreach (GuiControl control in guiControls.Values)
            {
                control.UnloadContent();
            }

            guiControls.Clear();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            RemoveDisposedControls();

            IEnumerable<GuiControl> enabledControls = guiControls.Values.Where(e => e.IsEnabled);
            
            foreach (GuiControl control in enabledControls.Reverse())
            {
                if (InputManager.Instance.MouseButtonInputHandled)
                {
                    break;
                }

                control.HandleInput();
            }

            InputManager.Instance.MouseButtonInputHandled = false;

            IEnumerable<GuiControl> controlsToUpdate = guiControls.Values.Where(x =>
                x.IsContentLoaded &&
                x.IsEnabled);

            foreach (GuiControl control in controlsToUpdate)
            {
                control.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            IEnumerable<GuiControl> controlsToDraw = guiControls.Values.Where(x =>
                x.IsContentLoaded &&
                x.IsVisible);

            foreach (GuiControl control in controlsToDraw)
            {
                control.Draw(spriteBatch);
            }
        }

        public void RegisterControls(params GuiControl[] controls)
            => RegisterControls(controls.ToList());

        public void RegisterControls(IEnumerable<GuiControl> controls)
        {
            foreach (GuiControl control in controls)
            {
                guiControls.Add(control.Id, control);
            }
        }

        /// <summary>
        /// Focuses the input on the specified control.
        /// </summary>
        /// <param name="control">The <see cref="GuiControl"> to focus.</param>
        public void FocusControl(GuiControl control)
            => FocusControl(control.Id);

        /// <summary>
        /// Focuses the input on the control with the specified identifier.
        /// </summary>
        /// <param name="id"><see cref="GuiControl"> identifier.</param>
        public void FocusControl(string id)
        {
            foreach (GuiControl control in guiControls.Values)
            {
                if (control.IsFocused)
                {
                    control.Unfocus();
                }

                if (control.Id == id && !control.IsFocused)
                {
                    control.Focus();
                }
            }
        }

        void RemoveDisposedControls()
        {
            IEnumerable<string> disposedControlsKeys = guiControls.Keys.Where(key => guiControls[key].IsDisposed);

            foreach (string key in disposedControlsKeys)
            {
                guiControls.Remove(key);
            }
        }
    }
}
