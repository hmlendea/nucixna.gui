using System;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.Screens
{
    /// <summary>
    /// Screen.
    /// </summary>
    public interface IScreen : IDisposable
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        Colour BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the foreground colour.
        /// </summary>
        /// <value>The foreground colour.</value>
        Colour ForegroundColour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Screen"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        bool IsContentLoaded { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Screen"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        bool IsDisposed { get; }

        /// <summary>
        /// Occurs when the <see cref="ForegroundColour"> was changed.
        /// </summary>
        event PropertyChangedEventHandler ForegroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="BackgroundColour"> was changed.
        /// </summary>
        event PropertyChangedEventHandler BackgroundColourChanged;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> was created.
        /// </summary>
        event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began loading its content.
        /// </summary>
        event EventHandler ContentLoading;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished loading its content.
        /// </summary>
        event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began unloading its content.
        /// </summary>
        event EventHandler ContentUnloading;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished unloading its content.
        /// </summary>
        event EventHandler ContentUnloaded;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began updating.
        /// </summary>
        event EventHandler Updating;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished updating.
        /// </summary>
        event EventHandler Updated;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began drawing.
        /// </summary>
        event EventHandler Drawing;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished drawing.
        /// </summary>
        event EventHandler Drawn;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> began disposing.
        /// </summary>
        event EventHandler Disposing;

        /// <summary>
        /// Occurs when this <see cref="Screen"/> finished disposing.
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Occurs when a key is pressed while this <see cref="Screen"/> has input focus.
        /// </summary>
        event KeyboardKeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a mouse button is pressed.
        /// </summary>
        event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when the mouse moved.
        /// </summary>
        event MouseEventHandler MouseMoved;

        /// <summary>
        /// Loads the content.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unloads the content.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}
