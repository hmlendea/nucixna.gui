using System.Collections.Generic;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Input;
using NuciXNA.Primitives;

using NuciXNA.Gui.Controls;

namespace NuciXNA.Gui
{
    /// <summary>
    /// GUI manager.
    /// </summary>
    public class GuiManager
    {
        private static readonly Lock syncRoot = new();
        private static volatile GuiManager instance;

        private readonly Dictionary<string, GuiControl> guiControls;

        /// <summary>
        /// Gets or sets the default background colour applied to controls without an explicit value.
        /// </summary>
        /// <value>The default background colour.</value>
        public Colour DefaultBackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the default foreground colour applied to controls without an explicit value.
        /// </summary>
        /// <value>The default foreground colour.</value>
        public Colour DefaultForegroundColour { get; set; }

        /// <summary>
        /// Gets or sets the default font name applied to controls without an explicit value.
        /// </summary>
        /// <value>The default font name.</value>
        public string DefaultFontName { get; set; }

        /// <summary>
        /// Gets or sets the default texture layout applied to controls without an explicit value.
        /// </summary>
        /// <value>The default texture layout.</value>
        public TextureLayout DefaultTextureLayout { get; set; }

        /// <summary>
        /// Gets or sets the default text margins applied to controls without an explicit value.
        /// </summary>
        /// <value>The default margins.</value>
        public int DefaultMargins { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static GuiManager Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (syncRoot)
                    {
                        instance ??= new GuiManager();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiManager"/> class.
        /// </summary>
        public GuiManager()
        {
            guiControls = [];

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

            List<GuiControl> controlValues = [.. guiControls.Values];

            for (int controlIndex = controlValues.Count - 1; controlIndex >= 0; controlIndex -= 1)
            {
                if (InputManager.Instance.MouseButtonInputHandled)
                {
                    break;
                }

                if (controlValues[controlIndex].IsEnabled)
                {
                    controlValues[controlIndex].HandleInput();
                }
            }

            InputManager.Instance.MouseButtonInputHandled = false;

            foreach (GuiControl control in controlValues)
            {
                if (control.IsContentLoaded && control.IsEnabled)
                {
                    control.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GuiControl control in guiControls.Values)
            {
                if (control.IsContentLoaded && control.IsVisible)
                {
                    control.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Registers a root control by its identifier.
        /// </summary>
        /// <param name="control">The root control.</param>
        public void RegisterControl(GuiControl control)
            => guiControls.Add(control.Id, control);

        /// <summary>
        /// Registers multiple root controls by their identifiers.
        /// </summary>
        /// <param name="controls">The root controls.</param>
        public void RegisterControls(params GuiControl[] controls)
            => RegisterControls((IEnumerable<GuiControl>)controls);

        /// <summary>
        /// Registers multiple root controls by their identifiers.
        /// </summary>
        /// <param name="controls">The root controls.</param>
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

                if (string.Equals(control.Id, id) && !control.IsFocused)
                {
                    control.Focus();
                }
            }
        }

        private void RemoveDisposedControls()
        {
            List<string> disposedControlIds = [];

            foreach (KeyValuePair<string, GuiControl> entry in guiControls)
            {
                if (entry.Value.IsDisposed)
                {
                    disposedControlIds.Add(entry.Key);
                }
            }

            foreach (string controlId in disposedControlIds)
            {
                guiControls.Remove(controlId);
            }
        }
    }
}
