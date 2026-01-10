using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace EngineLite.Engine.Core
{
    public static class Input
    {
        private static KeyboardState _previousKeys;
        private static KeyboardState _currentKeys;
        private static MouseState _previousMouse;
        private static MouseState _currentMouse;

        public static float XAxis { get; private set; }
        public static float YAxis { get; private set; }
        public static string InputString { get; private set; }
        public static Vector2 MousePos => new Vector2(_currentMouse.Position.X, _currentMouse.Position.Y);
        public static Vector2 PreviousMousePos => new Vector2(_previousMouse.Position.X, _previousMouse.Position.Y);

        public static bool IsScrolling => _currentMouse.ScrollWheelValue != _previousMouse.ScrollWheelValue;

        public enum MouseButton { Left, Right, Middle }

        public static void Update()
        {
            _previousKeys = _currentKeys;
            _currentKeys = Keyboard.GetState();

            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            HandleAxis();
            UpdateInputString();
        }

        #region Keys
        public static bool GetKey(Keys key) => _currentKeys.IsKeyDown(key);
        public static bool GetKeyDown(Keys key) => _currentKeys.IsKeyDown(key) && !_previousKeys.IsKeyDown(key);
        public static bool GetKeyUp(Keys key) => _currentKeys.IsKeyUp(key) && _previousKeys.IsKeyDown(key);
        #endregion

        #region Mouse
        public static bool GetMouseButton(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouse.LeftButton == ButtonState.Pressed,
                MouseButton.Right => _currentMouse.RightButton == ButtonState.Pressed,
                MouseButton.Middle => _currentMouse.MiddleButton == ButtonState.Pressed,
                _ => false
            };
        }

        public static bool GetMouseButtonDown(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released,
                MouseButton.Right => _currentMouse.RightButton == ButtonState.Pressed && _previousMouse.RightButton == ButtonState.Released,
                MouseButton.Middle => _currentMouse.MiddleButton == ButtonState.Pressed && _previousMouse.MiddleButton == ButtonState.Released,
                _ => false
            };
        }

        public static bool GetMouseButtonUp(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed,
                MouseButton.Right => _currentMouse.RightButton == ButtonState.Released && _previousMouse.RightButton == ButtonState.Pressed,
                MouseButton.Middle => _currentMouse.MiddleButton == ButtonState.Released && _previousMouse.MiddleButton == ButtonState.Pressed,
                _ => false
            };
        }

        public static float ScrollWheelDelta => (_currentMouse.ScrollWheelValue - _previousMouse.ScrollWheelValue) / 120f;
        #endregion

        #region Private Methods
        private static void HandleAxis()
        {
            XAxis = (GetKey(Keys.D) || GetKey(Keys.Right) ? 1f : 0f) +
                    (GetKey(Keys.A) || GetKey(Keys.Left) ? -1f : 0f);
            YAxis = (GetKey(Keys.S) || GetKey(Keys.Down) ? 1f : 0f) +
                    (GetKey(Keys.W) || GetKey(Keys.Up) ? -1f : 0f);
            XAxis = MathHelper.Clamp(XAxis, -1f, 1f);
            YAxis = MathHelper.Clamp(YAxis, -1f, 1f);
        }

        private static void UpdateInputString()
        {
            InputString = string.Join("", _currentKeys.GetPressedKeys().Where(GetKeyDown));
        }
        #endregion
    }
}
