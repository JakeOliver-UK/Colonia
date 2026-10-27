using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Colonia.Engine.Managers
{
    internal static class InputManager
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        public static bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public static bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);
        public static bool IsKeyPressed(Keys key) => _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        public static bool IsKeyReleased(Keys key) => !_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);

        public static bool IsMouseButtonDown(MouseButton button) => GetMouseButtonState(button, _currentMouseState);
        public static bool IsMouseButtonUp(MouseButton button) => !GetMouseButtonState(button, _currentMouseState);
        public static bool IsMouseButtonPressed(MouseButton button) => GetMouseButtonState(button, _currentMouseState) && !GetMouseButtonState(button, _previousMouseState);
        public static bool IsMouseButtonReleased(MouseButton button) => !GetMouseButtonState(button, _currentMouseState) && GetMouseButtonState(button, _previousMouseState);

        private static bool GetMouseButtonState(MouseButton button, MouseState state)
        {
            return button switch
            {
                MouseButton.Left => state.LeftButton == ButtonState.Pressed,
                MouseButton.Right => state.RightButton == ButtonState.Pressed,
                MouseButton.Middle => state.MiddleButton == ButtonState.Pressed,
                MouseButton.XButton1 => state.XButton1 == ButtonState.Pressed,
                MouseButton.XButton2 => state.XButton2 == ButtonState.Pressed,
                _ => false
            };
        }

        public static Vector2 MousePosition
        {
            get
            {
                Point position = new(_currentMouseState.X, _currentMouseState.Y);
                Point virtualPos = App.Instance.ScreenToVirtual(position);
                return new(virtualPos.X, virtualPos.Y);
            }
        }

        public static bool IsMouseScrollingUp => _currentMouseState.ScrollWheelValue > _previousMouseState.ScrollWheelValue;
        public static bool IsMouseScrollingDown => _currentMouseState.ScrollWheelValue < _previousMouseState.ScrollWheelValue;
        public static bool IsMouseScrollingLeft => _currentMouseState.HorizontalScrollWheelValue < _previousMouseState.HorizontalScrollWheelValue;
        public static bool IsMouseScrollingRight => _currentMouseState.HorizontalScrollWheelValue > _previousMouseState.HorizontalScrollWheelValue;
    }

    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2
    }
}
