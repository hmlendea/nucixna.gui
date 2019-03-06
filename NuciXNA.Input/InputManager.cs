using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
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

            CheckKeyboardKeyStates();
            CheckMouseButtonStates();
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
            return buttons
                .Select(GetMouseButtonState)
                .All(x => x.IsDown);
        }

        public bool IsAnyMouseButtonDown()
            => IsAnyMouseButtonDown(MouseButton.GetValues());

        public bool IsAnyMouseButtonDown(params MouseButton[] buttons)
            => IsAnyMouseButtonDown(buttons as IEnumerable<MouseButton>);

        public bool IsAnyMouseButtonDown(IEnumerable<MouseButton> buttons)
        {
            return buttons
                .Select(GetMouseButtonState)
                .Any(x => x.IsDown);
        }

        void CheckKeyboardKeyStates()
        {
            Array keys = Enum.GetValues(typeof(Keys));

            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key))
                {
                    OnKeyboardKeyPressed(this, new KeyboardKeyEventArgs(key, ButtonState.Pressed));
                }
                else if (currentKeyState.IsKeyUp(key) && previousKeyState.IsKeyDown(key))
                {
                    OnKeyboardKeyReleased(this, new KeyboardKeyEventArgs(key, ButtonState.Released));
                }
                else if (currentKeyState.IsKeyDown(key))
                {
                    OnKeyboardKeyDown(this, new KeyboardKeyEventArgs(key, ButtonState.HeldDown));
                }
            }
        }

        void CheckMouseButtonStates()
        {
            foreach (MouseButton button in MouseButton.GetValues())
            {
                ButtonState state = GetMouseButtonState(button);

                MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(
                    button,
                    state,
                    currentMouseState.Position.ToPoint2D());
                
                if (state == ButtonState.Pressed)
                {
                    OnMouseButtonReleased(this, eventArgs);
                }
                else if (state == ButtonState.Released)
                {
                    OnMouseButtonReleased(this, eventArgs);
                }
                else if (state == ButtonState.HeldDown)
                {
                    OnMouseButtonReleased(this, eventArgs);
                }
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

        ButtonState GetMouseButtonState(MouseButton button)
        {
            XNAButtonState currentState = XNAButtonState.Released;
            XNAButtonState previousState = XNAButtonState.Released;

            if (button == MouseButton.Left)
            {
                currentState = currentMouseState.LeftButton;
                previousState = previousMouseState.LeftButton;
            }
            else if (button == MouseButton.Right)
            {
                currentState = currentMouseState.RightButton;
                previousState = previousMouseState.RightButton;
            }
            else if (button == MouseButton.Middle)
            {
                currentState = currentMouseState.MiddleButton;
                previousState = previousMouseState.MiddleButton;
            }
            else if (button == MouseButton.Back)
            {
                currentState = currentMouseState.XButton1;
                previousState = previousMouseState.XButton2;
            }
            else if (button == MouseButton.Right)
            {
                currentState = currentMouseState.XButton2;
                previousState = previousMouseState.XButton2;
            }

            if (currentState == XNAButtonState.Pressed && previousState != XNAButtonState.Pressed)
            {
                return ButtonState.Pressed;
            }

            if (currentState == XNAButtonState.Released && previousState != XNAButtonState.Released)
            {
                return ButtonState.Released;
            }

            if (currentState == XNAButtonState.Pressed && previousState == XNAButtonState.Pressed)
            {
                return ButtonState.HeldDown;
            }

            if (currentState == XNAButtonState.Released && previousState == XNAButtonState.Released)
            {
                return ButtonState.Idle;
            }

            return null;
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
