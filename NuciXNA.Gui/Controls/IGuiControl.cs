using System;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// GUI Control.
    /// </summary>
    public interface IGuiControl
    {
        string Id { get; set; }

        /// <summary>
        /// Gets the location of this <see cref="GuiControl"/>.
        /// </summary>
        /// <value>The location.</value>
        Point2D Location { get; set; }

        /// <summary>
        /// Gets the coordinates of this control on the current <see cref="Screen">.
        /// </summary>
        /// <value>The screen coordinates.</value>
        Point2D ScreenLocation { get; }

        /// <summary>
        /// Gets the size of this <see cref="GuiControl"/>.
        /// </summary>
        /// <value>The size.</value>
        Size2D Size { get; set; }

        /// <summary>
        /// Gets the screen area covered by this <see cref="GuiControl"/> inside of its parent.
        /// </summary>
        /// <value>The covered area.</value>
        Rectangle2D ClientRectangle { get; }

        /// <summary>
        /// Gets the screen area covered by this <see cref="GuiControl"/> on the screen.
        /// </summary>
        /// <value>The covered screen area.</value>
        Rectangle2D DisplayRectangle { get; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        float Opacity { get; set; }

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
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool IsVisible { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is hovered.
        /// </summary>
        /// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
        bool IsHovered { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> has input focus.
        /// </summary>
        /// <value><c>true</c> if it has input focus; otherwise, <c>false</c>.</value>
        bool IsFocused { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if the content is loaded; otherwise, <c>false</c>.</value>
        bool IsContentLoaded { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets or sets the parent of this control.
        /// </summary>
        /// <value>The parent.</value>
        GuiControl Parent { get; set; } // TODO: This shouldn't be public

        ISite Site { get; set; }

        IContainer Container { get; }

        /// <summary>
        /// Occurs when the <see cref="ForegroundColour"> was changed.
        /// </summary>
        event PropertyChangedEventHandler ForegroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="BackgroundColour"> was changed.
        /// </summary>
        event PropertyChangedEventHandler BackgroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="Opacity"> was changed.
        /// </summary>
        event PropertyChangedEventHandler OpacityChanged;

        /// <summary>
        /// Occurs when the <see cref="FontName"> was changed.
        /// </summary>
        event PropertyChangedEventHandler FontNameChanged;

        /// <summary>
        /// Occurs when the <see cref="Location"> was changed.
        /// </summary>
        event PropertyChangedEventHandler LocationChanged;

        /// <summary>
        /// Occurs when the <see cref="Size"> was changed.
        /// </summary>
        event PropertyChangedEventHandler SizeChanged;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was shown.
        /// </summary>
        event EventHandler Shown;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was hidden.
        /// </summary>
        event EventHandler Hidden;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was enabled.
        /// </summary>
        event EventHandler Enabled;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was disabled.
        /// </summary>
        event EventHandler Disabled;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> gained focus.
        /// </summary>
        event EventHandler Focused;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> lost focus.
        /// </summary>
        event EventHandler Unfocused;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was created.
        /// </summary>
        event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began loading its content.
        /// </summary>
        event EventHandler ContentLoading;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished loading its content.
        /// </summary>
        event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began unloading its content.
        /// </summary>
        event EventHandler ContentUnloading;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished unloading its content.
        /// </summary>
        event EventHandler ContentUnloaded;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began updating.
        /// </summary>
        event EventHandler Updating;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished updating.
        /// </summary>
        event EventHandler Updated;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began drawing.
        /// </summary>
        event EventHandler Drawing;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished drawing.
        /// </summary>
        event EventHandler Drawn;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began disposing.
        /// </summary>
        event EventHandler Disposing;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished disposing.
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Occurs when a key is down while this <see cref="GuiControl"/> has input focus.
        /// </summary>
        event KeyboardKeyEventHandler KeyHeldDown;

        /// <summary>
        /// Occurs when a key is pressed while this <see cref="GuiControl"/> has input focus.
        /// </summary>
        event KeyboardKeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a key is released while this <see cref="GuiControl"/> has input focus.
        /// </summary>
        event KeyboardKeyEventHandler KeyReleased;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was clicked.
        /// </summary>
        event MouseButtonEventHandler Clicked;

        /// <summary>
        /// Occurs when a mouse button was pressed on this <see cref="GuiControl"/>.
        /// </summary>
        event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when the mouse entered this <see cref="GuiControl"/>.
        /// </summary>
        event MouseEventHandler MouseEntered;

        /// <summary>
        /// Occurs when the mouse left this <see cref="GuiControl"/>.
        /// </summary>
        event MouseEventHandler MouseLeft;

        /// <summary>
        /// Occurs when the mouse moved inside of this <see cref="GuiControl"/>.
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
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Disposes of this <see cref="GuiControl"/>.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Enables this <see cref="GuiControl">.
        /// </summary>
        void Enable();

        /// <summary>
        /// Disables this <see cref="GuiControl">.
        /// </summary>
        void Disable();

        /// <summary>
        /// Shows this <see cref="GuiControl">.
        /// </summary>
        void Show();

        /// <summary>
        /// Hide this <see cref="GuiControl">.
        /// </summary>
        void Hide();

        /// <summary>
        /// Focuses this <see cref="GuiControl">.
        /// </summary>
        void Focus();

        /// <summary>
        /// Unfocuses this <see cref="GuiControl">.
        /// </summary>
        void Unfocus();

        string ToString();

        /// <summary>
        /// Handles the input.
        /// </summary>
        void HandleInput();
    }
}
