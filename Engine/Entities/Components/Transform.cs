using Microsoft.Xna.Framework;
using System;

namespace Colonia.Engine.Entities.Components
{
    internal class Transform : Component
    {
        private Vector2 _localPosition = Vector2.Zero;
        private float _localRotation = 0f;
        private Vector2 _localScale = Vector2.One;
        private bool _localMatrixDirty = true;

        private Matrix _localMatrix = Matrix.Identity;
        private Matrix _worldMatrix = Matrix.Identity;

        private Vector2 _position;
        private float _rotation;
        private Vector2 _scale;

        public Vector2 LocalPosition
        {
            get => _localPosition;
            set { _localPosition = value; _localMatrixDirty = true; }
        }

        public float LocalRotation
        {
            get => _localRotation;
            set { _localRotation = value; _localMatrixDirty = true; }
        }

        public Vector2 LocalScale
        {
            get => _localScale;
            set { _localScale = value; _localMatrixDirty = true; }
        }

        public Matrix WorldMatrix => _worldMatrix;

        public Vector2 Position
        {
            get => _position;
            set => SetWorldPosition(value);
        }

        public float Rotation
        {
            get => _rotation;
            set => SetWorldRotation(value);
        }

        public Vector2 Scale
        {
            get => _scale;
            set => SetWorldScale(value);
        }

        public override void Update()
        {
            base.Update();

            if (_localMatrixDirty)
            {
                _localMatrix = Matrix.CreateScale(new Vector3(_localScale, 1f)) * 
                    Matrix.CreateRotationZ(_localRotation) * 
                    Matrix.CreateTranslation(new Vector3(_localPosition, 0f));

                _localMatrixDirty = false;
            }

            if (Entity.IsRoot) _worldMatrix = _localMatrix;
            else _worldMatrix = _localMatrix * Entity.Parent.Transform._worldMatrix;

            Vector3 t = _worldMatrix.Translation;
            _position = new Vector2(t.X, t.Y);

            _rotation = MathF.Atan2(_worldMatrix.M12, _worldMatrix.M11);

            _scale = new Vector2(
                new Vector2(_worldMatrix.M11, _worldMatrix.M12).Length(),
                new Vector2(_worldMatrix.M21, _worldMatrix.M22).Length()
            );
        }

        private void SetWorldPosition(Vector2 worldPos)
        {
            if (Entity.IsRoot) LocalPosition = worldPos;
            else
            {
                Matrix invParent = Matrix.Invert(Entity.Parent.Transform._worldMatrix);
                Vector2 local3 = Vector2.Transform(worldPos, invParent);
                LocalPosition = local3;
            }
        }

        private void SetWorldRotation(float worldRot)
        {
            if (Entity.IsRoot) LocalRotation = worldRot;
            else LocalRotation = worldRot - Entity.Parent.Transform._rotation;
        }

        private void SetWorldScale(Vector2 worldScale)
        {
            if (Entity.IsRoot) LocalScale = worldScale;
            else LocalScale = new Vector2(worldScale.X / Entity.Parent.Transform._scale.X, worldScale.Y / Entity.Parent.Transform._scale.Y);
        }
    }
}
