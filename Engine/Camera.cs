using Microsoft.Xna.Framework;
using System;

namespace Colonia.Engine
{
    internal class Camera
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Zoom { get; set; } = 1.0f;
        public float Rotation { get; set; } = 0.0f;
        public Point ViewSize { get; set; }

        public Matrix GetViewMatrix() =>
            Matrix.CreateTranslation(-MathF.Round(Position.X), -MathF.Round(Position.Y), 0) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(ViewSize.X * 0.5f, ViewSize.Y * 0.5f, 0);

        public Vector2 ScreenToWorld(Vector2 p) => Vector2.Transform(p, Matrix.Invert(GetViewMatrix()));
        public Vector2 WorldToScreen(Vector2 p) => Vector2.Transform(p, GetViewMatrix());
    }
}
