using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// GUI Element.
    /// </summary>
    public abstract class GuiElement : IComponent, IDisposable
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        Colour _backgroundColour;
        Colour _foregroundColour;
        Point2D? _location;
        Size2D? _size;
        float? _opacity;
        bool? _isEnabled;
        bool? _isFocused;
        bool? _isVisible;
        string _fontName;

        /// <summary>
        /// Gets the location of this <see cref="GuiElement"/>.
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

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(Location));
                LocationChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets the coordinates of this element on the current <see cref="Screen">.
        /// </summary>
        /// <value>The screen coordinates.</value>
        public Point2D ScreenLocation
        {
            get
            {
                if (Parent != null)
                {
                    return Location + Parent.ScreenLocation;
                }

                return Location;
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="GuiElement"/>.
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

                if (Parent != null)
                {
                    return Parent.Size;
                }

                return Size2D.Empty;
            }
            set
            {
                _size = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(Size));
                SizeChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets the screen area covered by this <see cref="GuiElement"/> inside of its parent.
        /// </summary>
        /// <value>The covered area.</value>
        public Rectangle2D ClientRectangle => new Rectangle2D(Location, Size);

        /// <summary>
        /// Gets the screen area covered by this <see cref="GuiElement"/> on the screen.
        /// </summary>
        /// <value>The covered screen area.</value>
        public Rectangle2D DisplayRectangle => new Rectangle2D(ScreenLocation, Size);

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

                if (Parent != null)
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

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(Opacity));
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
                if (_backgroundColour != null)
                {
                    return _backgroundColour;
                }

                if (Parent != null)
                {
                    return Parent.BackgroundColour;
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

                if (Parent != null)
                {
                    return Parent.ForegroundColour;
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
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName
        {
            get
            {
                if (_fontName != null)
                {
                    return _fontName;
                }

                if (Parent != null)
                {
                    return Parent.FontName;
                }

                return GuiManager.Instance.DefaultFontName;
            }
            set
            {
                _fontName = value;

                PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(nameof(FontName));
                FontNameChanged?.Invoke(this, eventArguments);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get
            {
                if (_isEnabled.HasValue)
                {
                    return _isEnabled.Value;
                }

                if (Parent != null)
                {
                    return Parent.IsEnabled;
                }

                return true;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;

                    if (value)
                    {
                        Enabled?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        Disabled?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool IsVisible
        {
            get
            {
                if (_isVisible.HasValue)
                {
                    return _isVisible.Value;
                }

                if (Parent != null)
                {
                    return Parent.IsVisible;
                }

                return true;
            }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;

                    if (value)
                    {
                        Shown?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        Hidden?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is hovered.
        /// </summary>
        /// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
        public bool IsHovered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> has input focus.
        /// </summary>
        /// <value><c>true</c> if it has input focus; otherwise, <c>false</c>.</value>
        public bool IsFocused
        {
            get
            {
                if (_isFocused.HasValue)
                {
                    return _isFocused.Value;
                }
                
                return false;
            }
            set
            {
                if (_isFocused != value)
                {
                    _isFocused = value;

                    if (value)
                    {
                        Focused?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if the content is loaded; otherwise, <c>false</c>.</value>
        public bool IsContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets or sets the children GUI elements.
        /// </summary>
        /// <value>The children.</value>
        List<GuiElement> Children { get; }

        /// <summary>
        /// Gets or sets the parent of this element.
        /// </summary>
        /// <value>The parent.</value>
        protected GuiElement Parent { get; set; }

        public ISite Site { get; set; }

        public IContainer Container
        {
            get
            {
                if (Site == null)
                {
                    return null;
                }

                return Site.Container;
            }
        }

        protected bool DesignMode
        {
            get
            {
                if (Site == null)
                {
                    return false;
                }

                return Site.DesignMode;
            }
        }

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
        /// Occurs when this <see cref="GuiElement"/> was shown.
        /// </summary>
        public event EventHandler Shown;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was hidden.
        /// </summary>
        public event EventHandler Hidden;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was enabled.
        /// </summary>
        public event EventHandler Enabled;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was disabled.
        /// </summary>
        public event EventHandler Disabled;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was focused.
        /// </summary>
        public event EventHandler Focused;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was created.
        /// </summary>
        public event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> began loading its content.
        /// </summary>
        public event EventHandler ContentLoading;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> finished loading its content.
        /// </summary>
        public event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> began unloading its content.
        /// </summary>
        public event EventHandler ContentUnloading;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> finished unloading its content.
        /// </summary>
        public event EventHandler ContentUnloaded;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> began updating.
        /// </summary>
        public event EventHandler Updating;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> finished updating.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> began drawing.
        /// </summary>
        public event EventHandler Drawing;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> finished drawing.
        /// </summary>
        public event EventHandler Drawn;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> began disposing.
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> finished disposing.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Occurs when a key is down while this <see cref="GuiElement"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyHeldDown;

        /// <summary>
        /// Occurs when a key is pressed while this <see cref="GuiElement"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a key is released while this <see cref="GuiElement"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyReleased;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was clicked.
        /// </summary>
        public event MouseButtonEventHandler Clicked;

        /// <summary>
        /// Occurs when a mouse button was pressed on this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when the mouse entered this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseEventHandler MouseEntered;

        /// <summary>
        /// Occurs when the mouse left this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseEventHandler MouseLeft;

        /// <summary>
        /// Occurs when the mouse moved inside of this <see cref="GuiElement"/>.
        /// </summary>
        public event MouseEventHandler MouseMoved;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GuiElement"/> class.
        /// </summary>
        public GuiElement()
        {
            Id = Guid.NewGuid().ToString();
            Children = new List<GuiElement>();

            Created?.Invoke(this, EventArgs.Empty);
        }

        ~GuiElement()
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
                throw new InvalidOperationException("Content not loaded for GuiElement ID: " + Id);
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
                throw new InvalidOperationException("Content not loaded for GuiElement ID: " + Id);
            }

            Updating?.Invoke(this, EventArgs.Empty);

            Children.RemoveAll(w => w.IsDisposed);

            foreach (GuiElement child in Children)
            {
                child.Parent = this;

                if (child is null || child.IsDisposed)
                {
                    Children.Remove(child);
                }
            }

            DoUpdate(gameTime);

            foreach (GuiElement guiElement in Children.Where(w => w.IsEnabled))
            {
                guiElement.Update(gameTime);
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
                throw new InvalidOperationException("Content not loaded for GuiElement ID: " + Id);
            }

            Drawing?.Invoke(this, EventArgs.Empty);

            DoDraw(spriteBatch);

            foreach (GuiElement guiElement in Children.Where(w => w.IsVisible))
            {
                guiElement.Draw(spriteBatch);
            }

            Drawn?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

            Children.ForEach(c => c.Dispose());
        }

        /// <summary>
        /// Disposes of this element.
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

                if (Site != null && Site.Container != null)
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
        /// Shows this GUI element.
        /// </summary>
        public virtual void Show()
        {
            IsEnabled = true;
            IsVisible = true;
        }

        /// <summary>
        /// Hide this GUI element.
        /// </summary>
        public virtual void Hide()
        {
            IsEnabled = false;
            IsVisible = false;
        }

        protected void AddChild(GuiElement element)
        {
            Children.Add(element);
            element.Parent = this;
        }
        
        protected virtual object GetService(Type service)
        {
            if (Site == null)
            {
                return null;
            }

            return Site.GetService(service);
        }

        public override string ToString()
        {
            if (Site == null)
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
                MouseButtonEventArgs e = new MouseButtonEventArgs(
                    MouseButton.Left,
                    ButtonState.Pressed,
                    InputManager.Instance.MouseLocation);

                GuiManager.Instance.FocusElement(this);

                Clicked?.Invoke(this, e);
                InputManager.Instance.MouseButtonInputHandled = true;
            }

            foreach (GuiElement child in Children)
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

            if (e.Location == e.PreviousLocation)
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
