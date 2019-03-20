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
    public class GuiElement : IComponent, IDisposable
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
                if (_location == null || _location != value)
                {
                    OnLocationChanged(this, null);
                }

                _location = value;
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
                if (_size == null || _size != value)
                {
                    OnSizeChanged(this, null);
                }

                _size = value;
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
                    OpacityChanged?.Invoke(this, null);
                }
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

                if (Parent != null)
                {
                    return Parent.ForegroundColour;
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
                if (_fontName != value)
                {
                    _fontName = value;
                    OnFontNameChanged(this, null);
                }
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
                    OnEnabled(this, null);
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
                    OnVisibilityChanged(this, null);
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
                    OnFocused(this, null);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/>'s content is loaded.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
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

        protected virtual bool CanRaiseEvents => true;

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
        /// Occurs when the <see cref="BackgroundColour"> was changed.
        /// </summary>
        public event EventHandler BackgroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="ForegroundColour"> was changed.
        /// </summary>
        public event EventHandler ForegroundColourChanged;

        /// <summary>
        /// Occurs when the <see cref="Opacity"> was changed.
        /// </summary>
        public event EventHandler OpacityChanged;

        /// <summary>
        /// Occurs when the <see cref="FontName"> was changed.
        /// </summary>
        public event EventHandler FontNameChanged;

        /// <summary>
        /// Occurs when the <see cref="Location"> was changed.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        /// Occurs when the <see cref="Size"> was changed.
        /// </summary>
        public event EventHandler SizeChanged;

        /// <summary>
        /// Occurs when the <see cref="IsVisible"> was changed.
        /// </summary>
        public event EventHandler VisibilityChanged;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was enabled.
        /// </summary>
        public event EventHandler Enabled;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was focused.
        /// </summary>
        public event EventHandler Focused;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was created.
        /// </summary>
        public event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/>'s content finished loading.
        /// </summary>
        public event EventHandler ContentLoaded;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was disposed.
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
        public virtual void LoadContent()
        {
            if (IsContentLoaded)
            {
                throw new InvalidOperationException("Content already loaded");
            }

            RegisterChildren();
            SetChildrenProperties();

            Children.ForEach(x => x.LoadContent());

            RegisterEvents();

            IsContentLoaded = true;
            IsDisposed = false;

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

            Parent = null;
            UnregisterEvents();

            Children.ForEach(x => x.UnloadContent());

            UnregisterChildren();
            IsContentLoaded = false;
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            Children.RemoveAll(w => w.IsDisposed);

            foreach (GuiElement child in Children)
            {
                child.Parent = this;

                if (child.IsDisposed)
                {
                    Children.Remove(child);
                }
            }

            RaiseEvents();
            SetChildrenProperties();

            foreach (GuiElement guiElement in Children.Where(w => w.IsEnabled))
            {
                guiElement.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GuiElement guiElement in Children.Where(w => w.IsVisible))
            {
                guiElement.Draw(spriteBatch);
            }
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
        /// Disposes of this <see cref="GuiElement"/>.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (this)
            {
                if (Site != null && Site.Container != null)
                {
                    Site.Container.Remove(this);
                }

                UnloadContent();

                Disposed.Invoke(this, EventArgs.Empty);
            }
        }

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

        protected virtual void RegisterChildren()
        {

        }

        protected void UnregisterChildren()
        {
            Children.Clear();
        }

        protected void AddChild(GuiElement element)
        {
            Children.Add(element);
            element.Parent = this;
        }

        protected virtual void RegisterEvents()
        {
            ContentLoaded += OnContentLoaded;
            Disposed += OnContentLoaded;

            InputManager.Instance.KeyboardKeyHeldDown += OnInputManagerKeyboardKeyHeldDown;
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.KeyboardKeyReleased += OnInputManagerKeyboardKeyReleased;

            InputManager.Instance.MouseButtonPressed += OnInputManagerMouseButtonPressed;
            InputManager.Instance.MouseMoved += OnInputManagerMouseMoved;
        }

        protected virtual void UnregisterEvents()
        {
            ContentLoaded -= OnContentLoaded;
            Disposed -= OnContentLoaded;

            InputManager.Instance.KeyboardKeyHeldDown -= OnInputManagerKeyboardKeyHeldDown;
            InputManager.Instance.KeyboardKeyPressed -= OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.KeyboardKeyReleased -= OnInputManagerKeyboardKeyReleased;

            InputManager.Instance.MouseButtonPressed -= OnInputManagerMouseButtonPressed;
            InputManager.Instance.MouseMoved -= OnInputManagerMouseMoved;
        }

        protected virtual void RaiseEvents()
        {
            if (!CanRaiseEvents)
            {
                return;
            }
        }

        protected virtual void SetChildrenProperties()
        {

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
                OnClicked(this, e);
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

        protected virtual void OnOpacityChanged(object sender, EventArgs e)
        {
            OpacityChanged?.Invoke(this, null);
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

        protected virtual void OnFontNameChanged(object sender, EventArgs e)
        {
            FontNameChanged?.Invoke(sender, e);
        }

        protected virtual void OnVisibilityChanged(object sender, EventArgs e)
        {
            VisibilityChanged?.Invoke(sender, e);
        }

        protected virtual void OnEnabled(object sender, EventArgs e)
        {
            Enabled?.Invoke(sender, e);
        }

        protected virtual void OnFocused(object sender, EventArgs e)
        {
            Focused?.Invoke(sender, e);
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnClicked(object sender, MouseButtonEventArgs e)
        {
            Clicked?.Invoke(sender, e);
            InputManager.Instance.MouseButtonInputHandled = true;
        }

        /// <summary>
        /// Raised by the KeyDown event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnKeyHeldDown(object sender, KeyboardKeyEventArgs e)
        {
            KeyHeldDown?.Invoke(sender, e);
        }

        /// <summary>
        /// Raised by the KeyPressed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        /// <summary>
        /// Raised by the KeyReleased event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnKeyReleased(object sender, KeyboardKeyEventArgs e)
        {
            KeyReleased?.Invoke(sender, e);
        }

        /// <summary>
        /// Fired by the MouseClick event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(this, e);
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseEntered(object sender, MouseEventArgs e)
        {
            MouseEntered?.Invoke(this, e);
            IsHovered = true;
        }

        /// <summary>
        /// Fired by the MouseLeft event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseLeft(object sender, MouseEventArgs e)
        {
            MouseLeft?.Invoke(this, e);
            IsHovered = false;
        }

        /// <summary>
        /// Fired by the MouseMoved event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseMoved?.Invoke(this, e);
            IsHovered = true;
        }

        /// <summary>
        /// Raised by the LocationChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnLocationChanged(object sender, EventArgs e)
        {
            LocationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raised by the SizeChanged event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSizeChanged(object sender, EventArgs e)
        {
            SizeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Fired by the <see cref="ContentLoaded"> event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnContentLoaded(object sender, EventArgs e)
        {
            IsContentLoaded = true;
            IsDisposed = false;
        }

        /// <summary>
        /// Fired by the <see cref="Disposed"> event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnDisposed(object sender, EventArgs e)
        {
            IsDisposed = true;
        }

        void OnInputManagerKeyboardKeyHeldDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!IsEnabled || !IsVisible ||
                !CanRaiseEvents || !IsFocused)
            {
                return;
            }

            OnKeyHeldDown(sender, e);
        }

        void OnInputManagerKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (!IsEnabled || !IsVisible ||
                !CanRaiseEvents || !IsFocused)
            {
                return;
            }

            OnKeyPressed(sender, e);
        }

        void OnInputManagerKeyboardKeyReleased(object sender, KeyboardKeyEventArgs e)
        {
            if (!IsEnabled || !IsVisible ||
                !CanRaiseEvents || !IsFocused)
            {
                return;
            }

            OnKeyReleased(sender, e);
        }

        void OnInputManagerMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!IsEnabled || !IsVisible || !CanRaiseEvents)
            {
                return;
            }

            if (!DisplayRectangle.Contains(e.Location))
            {
                return;
            }

            OnMouseButtonPressed(sender, e);

            if (e.Button == MouseButton.Left)
            {
                OnClicked(sender, e);
            }
        }

        void OnInputManagerMouseMoved(object sender, MouseEventArgs e)
        {
            if (!IsEnabled || !IsVisible || !CanRaiseEvents)
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

            OnMouseMoved(sender, e);

            if (DisplayRectangle.Contains(e.Location) &&
                !DisplayRectangle.Contains(e.PreviousLocation))
            {
                OnMouseEntered(sender, e);
            }

            if (!DisplayRectangle.Contains(e.Location) &&
                DisplayRectangle.Contains(e.PreviousLocation))
            {
                OnMouseLeft(sender, e);
            }
        }
    }
}
