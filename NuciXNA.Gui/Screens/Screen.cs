using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Input;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using NuciXNA.Graphics;

namespace NuciXNA.Gui.Screens
{
    /// <summary>
    /// Screen.
    /// </summary>
    public class Screen : IDisposable
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Colour BackgroundColour
        {
            get
            {
                if (_backgroundColour != null)
                {
                    return _backgroundColour;
                }

                return GuiManager.Instance.DefaultBackgroundColour;
            }
            set
            {
                if (_backgroundColour == null || _backgroundColour != value)
                {
                    // TODO: Pass event args
                    OnBackgroundColourChanged(this, null);
                }

                _backgroundColour = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground colour.
        /// </summary>
        /// <value>The foreground colour.</value>
        public Colour ForegroundColour
        {
            get
            {
                if (_foregroundColour != null)
                {
                    return _foregroundColour;
                }

                return GuiManager.Instance.DefaultForegroundColour;
            }
            set
            {
                if (_foregroundColour == null || _foregroundColour != value)
                {
                    OnForegroundColourChanged(this, null);
                }

                _foregroundColour = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Screen"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Occurs when the <see cref="BackgroundColour"> was changed.
        /// </summary>
        public event EventHandler BackgroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="ForegroundColour"> was changed.
        /// </summary>
        public event EventHandler ForegroundColourChanged;

        /// <summary>
        /// Occurs when a key is pressed while this <see cref="Screen"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a mouse button is pressed.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when the mouse moved.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> was created.
        /// </summary>
        public event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="Screen"/>'s content finished loading.
        /// </summary>
        public event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> was disposed.
        /// </summary>
        public event EventHandler Disposed;
        
        Colour _backgroundColour;
        Colour _foregroundColour;

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        public Screen()
        {
            Id = Guid.NewGuid().ToString();
            BackgroundColour = Colour.Black;

            Created?.Invoke(this, EventArgs.Empty);
        }

        ~Screen()
        {
            Dispose();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            if (IsContentLoaded)
            {
                throw new InvalidOperationException("Content already loaded");
            }

            SetChildrenProperties();

            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(BackgroundColour.ToXnaColor());

            GuiManager.Instance.LoadContent();

            RegisterEvents();

            ContentLoaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            GuiManager.Instance.UnloadContent();

            UnregisterEvents();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            SetChildrenProperties();

            GuiManager.Instance.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(BackgroundColour.ToXnaColor());
            GuiManager.Instance.Draw(spriteBatch);
        }

        /// <summary>
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            IsDisposed = true;

            lock (this)
            {
                UnloadContent();

                OnDisposed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the properties of the child elements.
        /// </summary>
        protected virtual void SetChildrenProperties()
        {

        }

        /// <summary>
        /// Registers the events of the child elements.
        /// </summary>
        protected virtual void RegisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed += OnKeyPressed;

            InputManager.Instance.MouseButtonPressed += OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += OnMouseMoved;
        }

        /// <summary>
        /// Unregisters the events of the child elements.
        /// </summary>
        protected virtual void UnregisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed -= OnKeyPressed;

            InputManager.Instance.MouseButtonPressed -= OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= OnMouseMoved;
        }

        /// <summary>
        /// Raised by the BackgroundColourChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnBackgroundColourChanged(object sender, EventArgs e)
        {
            BackgroundColourChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Raised by the ForegroundColourChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnForegroundColourChanged(object sender, EventArgs e)
        {
            ForegroundColourChanged?.Invoke(sender, e);
        }

        protected virtual void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        protected virtual void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(sender, e);
        }

        protected virtual void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseMoved?.Invoke(sender, e);
        }

        /// <summary>
        /// Fired by the <see cref="Disposed"> event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnDisposed(object sender, EventArgs e)
        {
            Disposed?.Invoke(sender, e);
        }
    }
}
