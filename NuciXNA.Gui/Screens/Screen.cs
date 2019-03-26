using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using NuciXNA.Graphics;

namespace NuciXNA.Gui.Screens
{
    /// <summary>
    /// Screen.
    /// </summary>
    public abstract class Screen : IDisposable
    {
        Colour _backgroundColour;
        Colour _foregroundColour;

        Color backgroundClearColour;

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
                _backgroundColour = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(BackgroundColour));
                BackgroundColourChanged?.Invoke(this, eventArguments);
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
                _foregroundColour = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(ForegroundColour));
                ForegroundColourChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Screen"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Occurs when the <see cref="ForegroundColour"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler ForegroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="BackgroundColour"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler BackgroundColourChanged;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> was created.
        /// </summary>
        public event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began loading its content.
        /// </summary>
        public event EventHandler ContentLoading;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished loading its content.
        /// </summary>
        public event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began unloading its content.
        /// </summary>
        public event EventHandler ContentUnloading;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished unloading its content.
        /// </summary>
        public event EventHandler ContentUnloaded;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began updating.
        /// </summary>
        public event EventHandler Updating;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished updating.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began drawing.
        /// </summary>
        public event EventHandler Drawing;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished drawing.
        /// </summary>
        public event EventHandler Drawn;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began disposing.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished disposing.
        /// </summary>
        public event EventHandler Disposed;

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
        public void LoadContent()
        {
            if (IsContentLoaded)
            {
                throw new InvalidOperationException("Content already loaded");
            }

            ContentLoading?.Invoke(this, EventArgs.Empty);
            
            backgroundClearColour = BackgroundColour.ToXnaColor();

            RegisterEvents();
            DoLoadContent();
            GuiManager.Instance.LoadContent();

            IsContentLoaded = true;
            ContentLoaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            ContentUnloading?.Invoke(this, EventArgs.Empty);

            UnregisterEvents();
            DoUnloadContent();
            GuiManager.Instance.UnloadContent();

            IsContentLoaded = false;
            ContentUnloaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            Updating?.Invoke(this, EventArgs.Empty);

            DoUpdate(gameTime);
            GuiManager.Instance.Update(gameTime);

            Updated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded");
            }

            Drawing?.Invoke(this, EventArgs.Empty);

            GraphicsManager.Instance.Graphics.GraphicsDevice.Clear(backgroundClearColour);

            DoDraw(spriteBatch);
            GuiManager.Instance.Draw(spriteBatch);

            Drawn?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disposes of this <see cref="GuiControl"/>.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected abstract void DoLoadContent();

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected abstract void DoUnloadContent();

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected abstract void DoUpdate(GameTime gameTime);

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected abstract void DoDraw(SpriteBatch spriteBatch);

        /// <summary>
        /// Disposes of this <see cref="GuiControl"/>.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (this)
            {
                Disposing?.Invoke(this, EventArgs.Empty);

                if (IsContentLoaded)
                {
                    UnloadContent();
                }

                IsDisposed = true;
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed += OnKeyPressed;
            InputManager.Instance.MouseButtonPressed += OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += OnMouseMoved;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            InputManager.Instance.KeyboardKeyPressed -= OnKeyPressed;
            InputManager.Instance.MouseButtonPressed -= OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= OnMouseMoved;
        }

        void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(sender, e);
        }

        void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseMoved?.Invoke(sender, e);
        }
    }
}
