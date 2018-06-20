﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Input;
using NuciXNA.Input.Enumerations;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Gui.GuiElements
{
    /// <summary>
    /// GUI Element.
    /// </summary>
    public class GuiElement : IComponent, IDisposable
    {
        public string Id { get; set; }

        Colour _backgroundColour;
        Colour _foregroundColour;
        Point2D? _location;
        Size2D? _size;
        float? _opacity;
        bool? _isEnabled;
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
                if (_location != null)
                {
                    return (Point2D)_location;
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

        public Point2D ScreenLocation
        {
            get
            {
                if (Parent != null)
                {
                    return new Point2D(
                        Location.X + Parent.ScreenLocation.X,
                        Location.Y + Parent.ScreenLocation.Y);
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
                if (_size != null)
                {
                    return (Size2D)_size;
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
                if (_opacity != null)
                {
                    return (float)_opacity;
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
                else
                {
                    _opacity = value;
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

                return Colour.Transparent;
            }
            set
            {
                if (_backgroundColour == null || _backgroundColour != value)
                {
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

                return Colour.Black;
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
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                if (_isEnabled != null)
                {
                    return (bool)_isEnabled;
                }

                if (Parent != null)
                {
                    return Parent.Enabled;
                }

                return true;
            }
            set
            {
                _isEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get
            {
                if (_isVisible != null)
                {
                    return (bool)_isVisible;
                }

                if (Parent != null)
                {
                    return Parent.Visible;
                }

                return true;
            }
            set
            {
                _isVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is hovered.
        /// </summary>
        /// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Hovered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> has input focus.
        /// </summary>
        /// <value><c>true</c> if it has input focus; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool InputFocus { get; set; }

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

                return string.Empty;
            }
            set
            {
                _fontName = value;
            }
        }

        /// <summary>
        /// Gets or sets the children GUI elements.
        /// </summary>
        /// <value>The children.</value>
        [XmlIgnore]
        protected List<GuiElement> Children { get; }

        [XmlIgnore]
        protected GuiElement Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GuiElement"/> is destroyed.
        /// </summary>
        /// <value><c>true</c> if destroyed; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool IsDisposed { get; private set; }

        [XmlIgnore]
        public ISite Site { get; set; }

        [XmlIgnore]
        protected virtual bool CanRaiseEvents => true;

        [XmlIgnore]
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

        [XmlIgnore]
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
        /// Occurs when clicked.
        /// </summary>
        public event MouseButtonEventHandler Clicked;

        /// <summary>
        /// Occurs when this <see cref="GuiElement"/> was disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Occurs when the BackgroundColour property was changed.
        /// </summary>
        public event EventHandler BackgroundColourChanged;

        /// <summary>
        /// Occurs when the ForegroundColour property was changed.
        /// </summary>
        public event EventHandler ForegroundColourChanged;

        /// <summary>
        /// Occurs when a key is down while this <see cref="GuiElement"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when a key is pressed while this <see cref="GuiElement"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when a key is released while this <see cref="GuiElement"/> has input focus.
        /// </summary>
        public event KeyboardKeyEventHandler KeyReleased;

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
        /// Occurs when the mouse moved.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Occurs when the Location property value changes.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        /// Occurs when the Size property value changes.
        /// </summary>
        public event EventHandler SizeChanged;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GuiElement"/> class.
        /// </summary>
        public GuiElement()
        {
            Id = Guid.NewGuid().ToString();
            Children = new List<GuiElement>();
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
            SetChildrenProperties();

            Children.ForEach(x => x.LoadContent());

            RegisterEvents();

            IsDisposed = false;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            Parent = null;
            UnregisterEvents();

            Children.ForEach(x => x.UnloadContent());
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

            foreach (GuiElement guiElement in Children.Where(w => w.Enabled))
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
            foreach (GuiElement guiElement in Children.Where(w => w.Visible))
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

            IsDisposed = true;

            lock (this)
            {
                if (Site != null && Site.Container != null)
                {
                    Site.Container.Remove(this);
                }

                UnloadContent();

                OnDisposed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Shows this GUI element.
        /// </summary>
        public virtual void Show()
        {
            Enabled = true;
            Visible = true;
        }

        /// <summary>
        /// Hide this GUI element.
        /// </summary>
        public virtual void Hide()
        {
            Enabled = false;
            Visible = false;
        }

        protected virtual void RegisterEvents()
        {
            InputManager.Instance.KeyboardKeyDown += OnInputManagerKeyboardKeyDown;
            InputManager.Instance.KeyboardKeyPressed += OnInputManagerKeyboardKeyPressed;
            InputManager.Instance.KeyboardKeyReleased += OnInputManagerKeyboardKeyReleased;

            InputManager.Instance.MouseButtonPressed += OnInputManagerMouseButtonPressed;
            InputManager.Instance.MouseMoved += OnInputManagerMouseMoved;
        }

        protected virtual void UnregisterEvents()
        {
            InputManager.Instance.KeyboardKeyDown -= OnInputManagerKeyboardKeyDown;
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

        protected void AddChild(GuiElement element)
        {
            Children.Add(element);
            element.Parent = this;
        }

        /// <summary>
        /// Handles the input.
        /// </summary>
        public void HandleInput()
        {
            if (Enabled && Visible && DisplayRectangle.Contains(InputManager.Instance.MouseLocation.ToPoint2D()) &&
                !InputManager.Instance.MouseButtonInputHandled &&
                InputManager.Instance.IsLeftMouseButtonClicked())
            {
                MouseButtonEventArgs e = new MouseButtonEventArgs(
                    MouseButton.LeftButton,
                    MouseButtonState.Pressed,
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
        /// Fired by the Disposed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnDisposed(object sender, EventArgs e)
        {
            Disposed?.Invoke(sender, e);
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

        /// <summary>
        /// Raised by the KeyDown event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            KeyDown?.Invoke(sender, e);
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
            Hovered = true;
        }

        /// <summary>
        /// Fired by the MouseLeft event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseLeft(object sender, MouseEventArgs e)
        {
            MouseLeft?.Invoke(this, e);
            Hovered = false;
        }

        /// <summary>
        /// Fired by the MouseMoved event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseMoved?.Invoke(this, e);
            Hovered = true;
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

        void OnInputManagerKeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!Enabled || !Visible ||
                !CanRaiseEvents || !InputFocus)
            {
                return;
            }

            OnKeyDown(sender, e);
        }

        void OnInputManagerKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (!Enabled || !Visible ||
                !CanRaiseEvents || !InputFocus)
            {
                return;
            }

            OnKeyPressed(sender, e);
        }

        void OnInputManagerKeyboardKeyReleased(object sender, KeyboardKeyEventArgs e)
        {
            if (!Enabled || !Visible ||
                !CanRaiseEvents || !InputFocus)
            {
                return;
            }

            OnKeyReleased(sender, e);
        }

        void OnInputManagerMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!Enabled || !Visible || !CanRaiseEvents)
            {
                return;
            }

            if (!DisplayRectangle.Contains(e.Location.ToPoint2D()))
            {
                return;
            }

            OnMouseButtonPressed(sender, e);

            if (e.Button == MouseButton.LeftButton)
            {
                OnClicked(sender, e);
            }
        }

        void OnInputManagerMouseMoved(object sender, MouseEventArgs e)
        {
            if (!Enabled || !Visible || !CanRaiseEvents)
            {
                return;
            }

            if (e.Location == e.PreviousLocation)
            {
                return;
            }

            if (!DisplayRectangle.Contains(e.Location.ToPoint2D()) &&
                !DisplayRectangle.Contains(e.PreviousLocation.ToPoint2D()))
            {
                return;
            }

            OnMouseMoved(sender, e);

            if (DisplayRectangle.Contains(e.Location.ToPoint2D()) &&
                !DisplayRectangle.Contains(e.PreviousLocation.ToPoint2D()))
            {
                OnMouseEntered(sender, e);
            }

            if (!DisplayRectangle.Contains(e.Location.ToPoint2D()) &&
                DisplayRectangle.Contains(e.PreviousLocation.ToPoint2D()))
            {
                OnMouseLeft(sender, e);
            }
        }
    }
}
