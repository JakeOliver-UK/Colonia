using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Colonia.Engine.Entities.Components
{
    internal class Camera : Component
    {
        public int Zoom { get; set; } = 1;
        public Matrix Transform
        {
            get
            {
                Viewport vp = App.Instance.GraphicsDevice.Viewport;
                float halfW = vp.Width * 0.5f;
                float halfH = vp.Height * 0.5f;

                int camX = (int)Math.Floor(Entity.Transform.Position.X);
                int camY = (int)Math.Floor(Entity.Transform.Position.Y);

                Matrix mat =
                    Matrix.CreateTranslation(new Vector3(-camX, -camY, 0)) *
                    Matrix.CreateScale(Zoom, Zoom, 1) *
                    Matrix.CreateTranslation(halfW, halfH, 0);

                Vector3 t = mat.Translation;
                t.X = (float)Math.Round(t.X);
                t.Y = (float)Math.Round(t.Y);
                mat.Translation = t;

                return mat;
            }
        }

        public Matrix Inverse => Matrix.Invert(Transform);

        public Rectangle VisibleArea
        {
            get
            {
                Viewport vp = App.Instance.GraphicsDevice.Viewport;
                float w = vp.Width / Zoom;
                float h = vp.Height / Zoom;
                float cx = Entity.Transform.Position.X;
                float cy = Entity.Transform.Position.Y;
                return new Rectangle((int)(cx - w / 2f), (int)(cy - h / 2f), (int)w, (int)h);
            }
        }

        public bool IsVisible(Vector2 position, float width, float height)
        {
            Rectangle bounds = new((int)position.X, (int)position.Y, (int)width, (int)height);
            return VisibleArea.Intersects(bounds);
        }

        public bool IsVisible(Rectangle bounds) => VisibleArea.Intersects(bounds);

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Vector3 worldPosition = Vector3.Transform(new Vector3(screenPosition, 0.0f), Inverse);
            return new Vector2(worldPosition.X, worldPosition.Y);
        }

        public override void Update()
        {
            Vector2 movement = Vector2.Zero;

            if (App.Instance.Input.IsKeyDown(Keys.W)) movement.Y -= 1.0f;
            if (App.Instance.Input.IsKeyDown(Keys.A)) movement.X -= 1.0f;
            if (App.Instance.Input.IsKeyDown(Keys.S)) movement.Y += 1.0f;
            if (App.Instance.Input.IsKeyDown(Keys.D)) movement.X += 1.0f;
            if (App.Instance.Input.IsKeyPressed(Keys.Q)) Zoom++;
            if (App.Instance.Input.IsKeyPressed(Keys.E)) Zoom--;

            Zoom = Math.Clamp(Zoom, 1, 3);

            Entity.Transform.Position += movement * Time.Delta * 250.0f;
        }
    }
}
