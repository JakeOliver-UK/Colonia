using Microsoft.Xna.Framework;
using System;

namespace Colonia.Engine.Entities.Components
{
    internal class Transform : Component
    {
        public Vector2 LocalPosition { get; set; } = Vector2.Zero;
        public float LocalRotation { get; set; } = 0f;
        public Vector2 LocalScale { get; set; } = new Vector2(1f, 1f);

        public Vector2 Position
        {
            get
            {
                if (Entity.IsRoot) return LocalPosition;

                Vector2 scaled = LocalPosition * Entity.Parent.Transform.Scale;

                float cos = MathF.Cos(Entity.Parent.Transform.Rotation);
                float sin = MathF.Sin(Entity.Parent.Transform.Rotation);
                float x = scaled.X * cos - scaled.Y * sin;
                float y = scaled.X * sin + scaled.Y * cos;

                return new Vector2(x + Entity.Parent.Transform.Position.X, y + Entity.Parent.Transform.Position.Y);
            }
            set
            {
                if (Entity.IsRoot) LocalPosition = value;
                else
                {
                    Vector2 delta = value - Entity.Parent.Transform.Position;

                    float cos = MathF.Cos(-Entity.Parent.Transform.Rotation);
                    float sin = MathF.Sin(-Entity.Parent.Transform.Rotation);
                    float rx = delta.X * cos - delta.Y * sin;
                    float ry = delta.X * sin + delta.Y * cos;

                    LocalPosition = new Vector2(rx / Entity.Parent.Transform.Scale.X, ry / Entity.Parent.Transform.Scale.Y);
                }
            }
        }

        public float Rotation
        {
            get
            {
                return Entity.IsRoot ? LocalRotation : Entity.Parent.Transform.Rotation + LocalRotation;
            }
            set
            {
                if (Entity.IsRoot) LocalRotation = value;
                else LocalRotation = value - Entity.Parent.Transform.Rotation;
            }
        }

        public Vector2 Scale
        {
            get
            {
                if (Entity.IsRoot) return LocalScale;

                return new Vector2(Entity.Parent.Transform.Scale.X * LocalScale.X, Entity.Parent.Transform.Scale.Y * LocalScale.Y);
            }
            set
            {
                if (Entity.IsRoot) LocalScale = value;
                else
                {
                    LocalScale = new Vector2(value.X / Entity.Parent.Transform.Scale.X, value.Y / Entity.Parent.Transform.Scale.Y);
                }
            }
        }
    }
}
