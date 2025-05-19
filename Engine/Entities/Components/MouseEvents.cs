using Colonia.Engine.Managers;
using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;

namespace Colonia.Engine.Entities.Components
{
    internal class MouseEvents : Component
    {
        public delegate void MouseEventHandler(object sender);

        public event MouseEventHandler OnMouseEnter;
        public event MouseEventHandler OnMouseHover;
        public event MouseEventHandler OnMouseLeave;
        public event MouseEventHandler OnMouseLeftDown;
        public event MouseEventHandler OnMouseLeftUp;
        public event MouseEventHandler OnMouseLeftPress;
        public event MouseEventHandler OnMouseLeftRelease;
        public event MouseEventHandler OnMouseRightDown;
        public event MouseEventHandler OnMouseRightUp;
        public event MouseEventHandler OnMouseRightPress;
        public event MouseEventHandler OnMouseRightRelease;
        public event MouseEventHandler OnMouseMiddleDown;
        public event MouseEventHandler OnMouseMiddleUp;
        public event MouseEventHandler OnMouseMiddlePress;
        public event MouseEventHandler OnMouseMiddleRelease;

        public float HoverDelay { get; set; } = 300.0f;

        private bool _isMouseOver = false;
        private bool _isMouseHovering = false;
        private float _hoverTimer = 0.0f;

        public override void Update()
        {
            base.Update();

            if (!Entity.HasComponent<SpriteRenderer>()) return;

            Vector2 mousePosition = App.Instance.SceneManager.Current.Camera.ScreenToWorld(App.Instance.Input.MousePosition);
            if (Entity.Manager.IsOverlayManager) mousePosition = App.Instance.Input.MousePosition;

            Rectangle bounds = Entity.GetComponent<SpriteRenderer>().Bounds;

            if (bounds.Contains(mousePosition))
            {
                if (!_isMouseOver)
                {
                    _isMouseOver = true;
                    OnMouseEnter?.Invoke(this);
                }

                if (_hoverTimer >= HoverDelay)
                {
                    if (!_isMouseHovering)
                    {
                        _isMouseHovering = true;
                        OnMouseHover?.Invoke(this);
                    }
                }
                else
                {
                    _hoverTimer += Time.DeltaMS;
                }

                if (App.Instance.Input.IsMouseButtonDown(MouseButton.Left))
                {
                    OnMouseLeftDown?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonUp(MouseButton.Left))
                {
                    OnMouseLeftUp?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonPressed(MouseButton.Left))
                {
                    OnMouseLeftPress?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonReleased(MouseButton.Left))
                {
                    OnMouseLeftRelease?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonDown(MouseButton.Right))
                {
                    OnMouseRightDown?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonUp(MouseButton.Right))
                {
                    OnMouseRightUp?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonPressed(MouseButton.Right))
                {
                    OnMouseRightPress?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonReleased(MouseButton.Right))
                {
                    OnMouseRightRelease?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonDown(MouseButton.Middle))
                {
                    OnMouseMiddleDown?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonUp(MouseButton.Middle))
                {
                    OnMouseMiddleUp?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonPressed(MouseButton.Middle))
                {
                    OnMouseMiddlePress?.Invoke(this);
                }

                if (App.Instance.Input.IsMouseButtonReleased(MouseButton.Middle))
                {
                    OnMouseMiddleRelease?.Invoke(this);
                }
            }
            else
            {
                if (_isMouseOver)
                {
                    _isMouseOver = false;
                    _isMouseHovering = false;
                    _hoverTimer = 0.0f;
                    OnMouseLeave?.Invoke(this);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            OnMouseEnter = null;
            OnMouseHover = null;
            OnMouseLeave = null;
            OnMouseLeftDown = null;
            OnMouseLeftUp = null;
            OnMouseLeftPress = null;
            OnMouseLeftRelease = null;
            OnMouseRightDown = null;
            OnMouseRightUp = null;
            OnMouseRightPress = null;
            OnMouseRightRelease = null;
            OnMouseMiddleDown = null;
            OnMouseMiddleUp = null;
            OnMouseMiddlePress = null;
            OnMouseMiddleRelease = null;
        }
    }
}
