using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Colonia.Engine.Managers
{
    internal class InputManager
    {
        public KeyboardState CurrentKeyboardState => _currentKeyboardState;
        public KeyboardState PreviousKeyboardState => _previousKeyboardState;
        public MouseState CurrentMouseState => _currentMouseState;
        public MouseState PreviousMouseState => _previousMouseState;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        public bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);
        public bool IsKeyPressed(Keys key) => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        public bool IsKeyReleased(Keys key) => _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);

        public bool IsMouseButtonDown(MouseButton button) => button switch
        {
            MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Pressed,
            MouseButton.Right => _currentMouseState.RightButton == ButtonState.Pressed,
            MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Pressed,
            _ => false
        };

        public bool IsMouseButtonUp(MouseButton button) => button switch
        {
            MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Released,
            MouseButton.Right => _currentMouseState.RightButton == ButtonState.Released,
            MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Released,
            _ => false
        };

        public bool IsMouseButtonPressed(MouseButton button) => button switch
        {
            MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released,
            MouseButton.Right => _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released,
            MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released,
            _ => false
        };

        public bool IsMouseButtonReleased(MouseButton button) => button switch
        {
            MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed,
            MouseButton.Right => _currentMouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed,
            MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Released && _previousMouseState.MiddleButton == ButtonState.Pressed,
            _ => false
        };

        public Vector2 MousePosition => new(_currentMouseState.X, _currentMouseState.Y);
        public Vector2 MouseDelta => new(_currentMouseState.X - _previousMouseState.X, _currentMouseState.Y - _previousMouseState.Y);
        public int MouseScroll => _currentMouseState.ScrollWheelValue;
        public int MouseScrollDelta => _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;

    }

    internal enum MouseButton
    {
        Left,
        Right,
        Middle
    }
}
