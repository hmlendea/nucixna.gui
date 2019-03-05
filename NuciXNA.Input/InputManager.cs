using System;
using System.Linq;

using Microsoft.Xna.Framework.Input;
using XNAButtonState = Microsoft.Xna.Framework.Input.ButtonState;

using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

namespace NuciXNA.Input
{
    /// <summary>
    /// Input manager.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// Occurs when a mouse button was pressed.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonPressed;

        /// <summary>
        /// Occurs when a mouse button was released.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonReleased;

        /// <summary>
        /// Occurs when a mouse button is down.
        /// </summary>
        public event MouseButtonEventHandler MouseButtonDown;

        /// <summary>
        /// Occurs when the mouse moves.
        /// </summary>
        public event MouseEventHandler MouseMoved;

        /// <summary>
        /// Occurs when a keyboard key was pressed.
        /// </summary>
        public event KeyboardKeyEventHandler KeyboardKeyPressed;

        /// <summary>
        /// Occurs when a keyboard key was released.
        /// </summary>
        public event KeyboardKeyEventHandler KeyboardKeyReleased;

        /// <summary>
        /// Occurs when a keyboard key is down.
        /// </summary>
        public event KeyboardKeyEventHandler KeyboardKeyDown;

        KeyboardState currentKeyState, previousKeyState;
        MouseState currentMouseState, previousMouseState;

        static volatile InputManager instance;
        static object syncRoot = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new InputManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        public void Update()
        {
            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;

            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            CheckKeyboardKeyPressed();
            CheckKeyboardKeyReleased();
            CheckKeyboardKeyDown();

            CheckMouseButtonPressed();
            CheckMouseButtonReleased();
            CheckMouseButtonDown();
            CheckMouseMoved();
        }

        /// <summary>
        /// Resets the input states.
        /// </summary>
        public void ResetInputStates()
        {
            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;

            currentKeyState = new KeyboardState();
            currentMouseState = new MouseState();
        }

        public bool IsKeyDown(params Keys[] keys)
        {
            return keys.All(currentKeyState.IsKeyDown);
        }

        public bool IsAnyKeyDown()
        {
            Keys[] allKeys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToArray();

            return IsAnyKeyDown(allKeys);
        }

        public bool IsAnyKeyDown(params Keys[] keys)
        {
            return keys.Any(currentKeyState.IsKeyDown);
        }

        public bool IsMouseButtonDown(params MouseButton[] buttons)
        {
            return buttons.All(IsMouseButtonDown);
        }

        public bool IsAnyMouseButtonDown()
        {
            MouseButton[] allButtons = MouseButton.GetValues().ToArray();

            return IsAnyMouseButtonDown(allButtons);
        }

        public bool IsAnyMouseButtonDown(params MouseButton[] buttons)
        {
            return buttons.Any(IsMouseButtonDown);
        }

        void CheckKeyboardKeyPressed()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key))
                {
                    OnKeyboardKeyPressed(this, new KeyboardKeyEventArgs(key, ButtonState.Pressed));
                }
            }
        }

        void CheckKeyboardKeyReleased()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (currentKeyState.IsKeyUp(key) && previousKeyState.IsKeyDown(key))
                {
                    OnKeyboardKeyReleased(this, new KeyboardKeyEventArgs(key, ButtonState.Released));
                }
            }
        }

        void CheckKeyboardKeyDown()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (currentKeyState.IsKeyDown(key))
                {
                    OnKeyboardKeyDown(this, new KeyboardKeyEventArgs(key, ButtonState.Down));
                }
            }
        }

        void CheckMouseButtonPressed()
        {
            if (currentMouseState.LeftButton == XNAButtonState.Pressed &&
                previousMouseState.LeftButton != XNAButtonState.Pressed)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.LeftButton,
                    ButtonState.Pressed,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonPressed(this, eventArgs);
            }

            if (currentMouseState.RightButton == XNAButtonState.Pressed &&
                previousMouseState.RightButton != XNAButtonState.Pressed)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.RightButton,
                    ButtonState.Pressed,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonPressed(this, eventArgs);
            }

            if (currentMouseState.MiddleButton == XNAButtonState.Pressed &&
                previousMouseState.MiddleButton != XNAButtonState.Pressed)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.MiddleButton,
                    ButtonState.Pressed,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonPressed(this, eventArgs);
            }
        }

        void CheckMouseButtonReleased()
        {
            if (currentMouseState.LeftButton == XNAButtonState.Released &&
                previousMouseState.LeftButton != XNAButtonState.Released)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.LeftButton,
                    ButtonState.Released,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonReleased(this, eventArgs);
            }

            if (currentMouseState.RightButton == XNAButtonState.Released &&
                previousMouseState.RightButton != XNAButtonState.Released)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.RightButton,
                    ButtonState.Released,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonReleased(this, eventArgs);
            }

            if (currentMouseState.MiddleButton == XNAButtonState.Released &&
                previousMouseState.MiddleButton != XNAButtonState.Released)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.MiddleButton,
                    ButtonState.Released,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonReleased(this, eventArgs);
            }
        }

        void CheckMouseButtonDown()
        {
            if (currentMouseState.LeftButton == XNAButtonState.Pressed)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.LeftButton,
                    ButtonState.Down,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonDown(this, eventArgs);
            }

            if (currentMouseState.RightButton == XNAButtonState.Pressed)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.RightButton,
                    ButtonState.Down,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonDown(this, eventArgs);
            }

            if (currentMouseState.MiddleButton == XNAButtonState.Pressed)
            {
                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    MouseButton.MiddleButton,
                    ButtonState.Down,
                    currentMouseState.Position.ToPoint2D());

                OnMouseButtonDown(this, eventArgs);
            }
        }

        void CheckMouseMoved()
        {
            if (currentMouseState.Position != previousMouseState.Position)
            {
                MouseEventArgs eventArgs = new MouseEventArgs(
                    currentMouseState.Position.ToPoint2D(),
                    previousMouseState.Position.ToPoint2D());

                OnMouseMoved(this, eventArgs);
            }
        }

        bool IsMouseButtonDown(MouseButton button)
        {
            bool isPressed = false;

            if (button == MouseButton.LeftButton)
            {
                isPressed =
                    currentMouseState.LeftButton == XNAButtonState.Pressed &&
                    previousMouseState.LeftButton == XNAButtonState.Released;
            }
            else if (button == MouseButton.RightButton)
            {
                isPressed =
                    currentMouseState.RightButton == XNAButtonState.Pressed &&
                    previousMouseState.LeftButton == XNAButtonState.Released;
            }
            else if (button == MouseButton.MiddleButton)
            {
                isPressed =
                    currentMouseState.MiddleButton == XNAButtonState.Pressed &&
                    previousMouseState.LeftButton == XNAButtonState.Released;
            }

            return isPressed;
        }

        /// <summary>
        /// Fires when a keyboard key was pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnKeyboardKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            KeyboardKeyPressed?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires when a keyboard key was released.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnKeyboardKeyReleased(object sender, KeyboardKeyEventArgs e)
        {
            KeyboardKeyReleased?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires when a keyboard key is down.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnKeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            KeyboardKeyDown?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires when a mouse button was pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires when a mouse button was released.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            MouseButtonReleased?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires when a mouse button is down.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseButtonDown?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires when the mouse was moved.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseMoved?.Invoke(sender, e);
        }

        // TODO: Everything below this is required by a workaround to a problem and should be removed as soon as it is properly fixed

        public Point2D MouseLocation => new Point2D(currentMouseState.Position.X, currentMouseState.Position.Y);
        public bool MouseButtonInputHandled { get; set; }

        public bool IsLeftMouseButtonClicked()
        {
            if (currentMouseState.LeftButton == XNAButtonState.Pressed &&
                previousMouseState.LeftButton == XNAButtonState.Released)
            {
                return true;
            }

            return false;
        }
    }
}
