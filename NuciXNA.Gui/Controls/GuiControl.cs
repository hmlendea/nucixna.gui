using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.Controls
{
    /// <summary>
    /// GUI Control.
    /// </summary>
    public abstract class GuiControl : IGuiControl, IComponent, IDisposable
    {
        List<IGuiControl> Children { get; }

        Colour _backgroundColour;
        Colour _foregroundColour;
        Point2D? _location;
        Size2D? _size;
        float? _opacity;
        bool _isEnabled;
        bool _isVisible;
        string _fontName;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets the location of this <see cref="GuiControl"/>.
        /// </summary>
        /// <value>The location.</value>
        public Point2D Location
        {
            get
            {
                if (_location.HasValue)
                {
                    return _location.Value;
                }

                return Point2D.Empty;
            }
            set
            {
                _location = value;

                PropertyChangedEventArgs eventArguments = new(nameof(Location));
                LocationChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets the coordinates of this control on the current <see cref="Screen">.
        /// </summary>
        /// <value>The screen coordinates.</value>
        public Point2D ScreenLocation
        {
            get
            {
                if (Parent is not null)
                {
                    return Location + Parent.ScreenLocation;
                }

                return Location;
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="GuiControl"/>.
        /// </summary>
        /// <value>The size.</value>
        public Size2D Size
        {
            get
            {
                if (_size.HasValue)
                {
                    return _size.Value;
                }

                if (Parent is not null)
                {
                    return Parent.Size;
                }

                return Size2D.Empty;
            }
            set
            {
                _size = value;

                PropertyChangedEventArgs eventArguments = new(nameof(Size));
                SizeChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets the screen area covered by this <see cref="GuiControl"/> inside of its parent.
        /// </summary>
        /// <value>The covered area.</value>
        public Rectangle2D ClientRectangle => new(Location, Size);

        /// <summary>
        /// Gets the screen area covered by this <see cref="GuiControl"/> on the screen.
        /// </summary>
        /// <value>The covered screen area.</value>
        public Rectangle2D DisplayRectangle => new(ScreenLocation, Size);

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity
        {
            get
            {
                if (_opacity.HasValue)
                {
                    return _opacity.Value;
                }

                if (Parent is not null)
                {
                    return Parent.Opacity;
                }

                return 1.0f;
            }
            set
            {
                if (value > 1.0f)
                {
                    _opacity = 1.0f;
                }
                else if (value < 0.0f)
                {
                    _opacity = 0.0f;
                }
                else if (value != _opacity)
                {
                    _opacity = value;
                }

                PropertyChangedEventArgs eventArguments = new(nameof(Opacity));
                OpacityChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Colour BackgroundColour
        {
            get
            {
                if (_backgroundColour is not null)
                {
                    return _backgroundColour;
                }

                if (Parent is not null)
                {
                    return Parent.BackgroundColour;
                }

                return GuiManager.Instance.DefaultBackgroundColour;
            }
            set
            {
                _backgroundColour = value;

                PropertyChangedEventArgs eventArguments = new(nameof(BackgroundColour));
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
                if (_foregroundColour is not null)
                {
                    return _foregroundColour;
                }

                if (Parent is not null)
                {
                    return Parent.ForegroundColour;
                }

                return GuiManager.Instance.DefaultForegroundColour;
            }
            set
            {
                _foregroundColour = value;

                PropertyChangedEventArgs eventArguments = new(nameof(ForegroundColour));
                ForegroundColourChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName
        {
            get
            {
                if (_fontName is not null)
                {
                    return _fontName;
                }

                if (Parent is not null)
                {
                    return Parent.FontName;
                }

                return GuiManager.Instance.DefaultFontName;
            }
            set
            {
                _fontName = value;

                PropertyChangedEventArgs eventArguments = new(nameof(FontName));
                FontNameChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get
            {
                if (Parent is null || Parent.IsEnabled)
                {
                    return _isEnabled;
                }

                return false;
            }
            private set => _isEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool IsVisible
        {
            get
            {
                if (Parent is null || Parent.IsVisible)
                {
                    return _isVisible;
                }

                return false;
            }
            protected set => _isVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is hovered.
        /// </summary>
        /// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
        public bool IsHovered { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> has input focus.
        /// </summary>
        /// <value><c>true</c> if it has input focus; otherwise, <c>false</c>.</value>
        public bool IsFocused { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if the content is loaded; otherwise, <c>false</c>.</value>
        public bool IsContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiControl"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets or sets the parent of this control.
        /// </summary>
        /// <value>The parent.</value>
        public GuiControl Parent { get; set; } // TODO: This shouldn't be public

        public ISite Site { get; set; }

        public IContainer Container => Site?.Container;

        protected bool DesignMode => Site?.DesignMode ?? false;

        /// <summary>
        /// Occurs when the <see cref="ForegroundColour"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler ForegroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="BackgroundColour"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler BackgroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="Opacity"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler OpacityChanged;

        /// <summary>
        /// Occurs when the <see cref="FontName"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler FontNameChanged;

        /// <summary>
        /// Occurs when the <see cref="Location"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler LocationChanged;

        /// <summary>
        /// Occurs when the <see cref="Size"> was changed.
        /// </summary>
        public event PropertyChangedEventHandler SizeChanged;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was shown.
        /// </summary>
        public event EventHandler Shown;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was hidden.
        /// </summary>
        public event EventHandler Hidden;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was enabled.
        /// </summary>
        public event EventHandler Enabled;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was disabled.
        /// </summary>
        public event EventHandler Disabled;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> gained focus.
        /// </summary>
        public event EventHandler Focused;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> lost focus.
        /// </summary>
        public event EventHandler Unfocused;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was created.
        /// </summary>
        public event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began loading its content.
        /// </summary>
        public event EventHandler ContentLoading;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished loading its content.
        /// </summary>
        public event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began unloading its content.
        /// </summary>
        public event EventHandler ContentUnloading;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished unloading its content.
        /// </summary>
        public event EventHandler ContentUnloaded;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began updating.
        /// </summary>
        public event EventHandler Updating;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished updating.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began drawing.
        /// </summary>
        public event EventHandler Drawing;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished drawing.
        /// </summary>
        public event EventHandler Drawn;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> began disposing.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> finished disposing.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Occurs when a key is down while this <see cref="GuiControl"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyHeldDown;

        /// <summary>
        /// Occurs when a key is pressed while this <see cref="GuiControl"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a key is released while this <see cref="GuiControl"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyReleased;

        /// <summary>
        /// Occurs when this <see cref="GuiControl"/> was clicked.
        /// </summary>
        public event MouseButtonEventHandler Clicked;

        /// <summary>
        /// Occurs when a mouse button was pressed on this <see cref="GuiControl"/>.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when the mouse entered this <see cref="GuiControl"/>.
        /// </summary>
        public event MouseEventHandler MouseEntered;

        /// <summary>
        /// Occurs when the mouse left this <see cref="GuiControl"/>.
        /// </summary>
        public event MouseEventHandler MouseLeft;

        /// <summary>
        /// Occurs when the mouse moved inside of this <see cref="GuiControl"/>.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiControl"/> class.
        /// </summary>
        public GuiControl()
        {
            Id = Guid.NewGuid().ToString();
            Children = [];

            IsEnabled = true;
            IsVisible = true;

            Created?.Invoke(this, EventArgs.Empty);
        }

        ~GuiControl() => Dispose();

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

            RegisterEvents();
            DoLoadContent();

            Children.ForEach(x => x.LoadContent());

            IsContentLoaded = true;
            IsDisposed = false;

            ContentLoaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded for the GUI Control with ID: " + Id);
            }

            ContentUnloading?.Invoke(this, EventArgs.Empty);

            Parent = null;

            UnregisterEvents();
            DoUnloadContent();

            Children.ForEach(x => x.UnloadContent());
            Children.Clear();

            IsContentLoaded = false;
            ContentUnloaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded for the GUI Control with ID: " + Id);
            }

            Updating?.Invoke(this, EventArgs.Empty);

            foreach (GuiControl child in Children.Cast<GuiControl>())
            {
                if (child is null || child.IsDisposed)
                {
                    Children.Remove(child);
                }
                else
                {
                    child.Parent = this;
                }
            }

            DoUpdate(gameTime);

            IEnumerable<IGuiControl> enabledChildren = Children.Where(c => c.IsEnabled);

            foreach (IGuiControl child in enabledChildren)
            {
                child.Update(gameTime);
            }

            Updated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsContentLoaded)
            {
                throw new InvalidOperationException("Content not loaded for the GUI control with ID: " + Id);
            }

            Drawing?.Invoke(this, EventArgs.Empty);

            DoDraw(spriteBatch);

            IEnumerable<IGuiControl> visibleChildren = Children.Where(c => c.IsEnabled && c.IsVisible);

            foreach (IGuiControl child in visibleChildren)
            {
                child.Draw(spriteBatch);
            }

            Drawn?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disposes of this <see cref="GuiControl"/>.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            Children.ForEach(c => c.Dispose());
            Children.Clear();
        }

        /// <summary>
        /// Disposes of this control.
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

                if (Site is not null && Site.Container is not null)
                {
                    Site.Container.Remove(this);
                }

                if (IsContentLoaded)
                {
                    UnloadContent();
                }

                Disposed?.Invoke(this, EventArgs.Empty);
            }
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
        /// Enables this <see cref="GuiControl">.
        /// </summary>
        public virtual void Enable()
        {
            IsEnabled = true;

            Enabled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disables this <see cref="GuiControl">.
        /// </summary>
        public virtual void Disable()
        {
            IsEnabled = false;

            Disabled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Shows this <see cref="GuiControl">.
        /// </summary>
        public virtual void Show()
        {
            IsVisible = true;

            Shown?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Hide this <see cref="GuiControl">.
        /// </summary>
        public virtual void Hide()
        {
            IsVisible = false;

            Hidden?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Focuses this <see cref="GuiControl">.
        /// </summary>
        public virtual void Focus()
        {
            IsFocused = true;

            Focused?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Unfocuses this <see cref="GuiControl">.
        /// </summary>
        public virtual void Unfocus()
        {
            IsFocused = false;

            Unfocused?.Invoke(this, EventArgs.Empty);
        }

        protected void RegisterChild(IGuiControl control)
        {
            Children.Add(control);
            control.Parent = this;
        }

        protected void RegisterChildren(params IGuiControl[] controls)
            => RegisterChildren(controls.ToList());

        protected void RegisterChildren(IEnumerable<IGuiControl> controls)
        {
            Children.AddRange(controls);

            foreach (IGuiControl control in controls)
            {
                control.Parent = this;
            }
        }

        protected virtual object GetService(Type service) => Site?.GetService(service);

        public override string ToString()
        {
            if (Site is null)
            {
                return GetType().FullName;
            }

            return $"{Site.Name} [{GetType().FullName}]";
        }

        /// <summary>
        /// Handles the input.
        /// </summary>
        public void HandleInput()
        {
            if (IsEnabled && IsVisible && DisplayRectangle.Contains(InputManager.Instance.MouseLocation) &&
                !InputManager.Instance.MouseButtonInputHandled &&
                InputManager.Instance.IsLeftMouseButtonClicked())
            {
                MouseButtonEventArgs e = new(
                    MouseButton.Left,
                    ButtonState.Pressed,
                    InputManager.Instance.MouseLocation);

                GuiManager.Instance.FocusControl(this);

                Clicked?.Invoke(this, e);
                InputManager.Instance.MouseButtonInputHandled = true;
            }

            foreach (GuiControl child in Children.Cast<GuiControl>())
            {
                if (InputManager.Instance.MouseButtonInputHandled)
                {
                    break;
                }

                child.HandleInput();
            }
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            InputManager.Instance.KeyboardKeyHeldDown += OnInputManagerKeyboardKeyHeldDown;
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.KeyboardKeyReleased += OnInputManagerKeyboardKeyReleased;

            InputManager.Instance.MouseButtonPressed += OnInputManagerMouseButtonPressed;
            InputManager.Instance.MouseMoved += OnInputManagerMouseMoved;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            InputManager.Instance.KeyboardKeyHeldDown -= OnInputManagerKeyboardKeyHeldDown;
            InputManager.Instance.KeyboardKeyPressed -= OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.KeyboardKeyReleased -= OnInputManagerKeyboardKeyReleased;

            InputManager.Instance.MouseButtonPressed -= OnInputManagerMouseButtonPressed;
            InputManager.Instance.MouseMoved -= OnInputManagerMouseMoved;
        }

        void OnInputManagerKeyboardKeyHeldDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!IsEnabled || !IsVisible|| !IsFocused)
            {
                return;
            }

            KeyHeldDown?.Invoke(sender, e);
        }

        void OnInputManagerKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (!IsEnabled || !IsVisible || !IsFocused)
            {
                return;
            }

            KeyPressed?.Invoke(sender, e);
        }

        void OnInputManagerKeyboardKeyReleased(object sender, KeyboardKeyEventArgs e)
        {
            if (!IsEnabled || !IsVisible || !IsFocused)
            {
                return;
            }

            KeyReleased?.Invoke(sender, e);
        }

        void OnInputManagerMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!IsEnabled || !IsVisible)
            {
                return;
            }

            if (!DisplayRectangle.Contains(e.Location))
            {
                return;
            }

            MouseButtonPressed?.Invoke(this, e);

            if (e.Button == MouseButton.Left)
            {
                Clicked?.Invoke(sender, e);
                InputManager.Instance.MouseButtonInputHandled = true;
            }
        }

        void OnInputManagerMouseMoved(object sender, MouseEventArgs e)
        {
            if (!IsEnabled || !IsVisible)
            {
                return;
            }

            if (e.Location.Equals(e.PreviousLocation))
            {
                return;
            }

            if (!DisplayRectangle.Contains(e.Location) &&
                !DisplayRectangle.Contains(e.PreviousLocation))
            {
                return;
            }

            MouseMoved?.Invoke(this, e);
            IsHovered = true;

            if (DisplayRectangle.Contains(e.Location) &&
                !DisplayRectangle.Contains(e.PreviousLocation))
            {
                MouseEntered?.Invoke(this, e);
                IsHovered = true;
            }

            if (!DisplayRectangle.Contains(e.Location) &&
                DisplayRectangle.Contains(e.PreviousLocation))
            {
                MouseLeft?.Invoke(this, e);
                IsHovered = false;
            }
        }
    }
}
