using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Colonia.Engine.Entities.Components
{
    internal class Camera : Component
    {
        public float Zoom { get; set; } = 1.0f;
        public Matrix Transform =>  Matrix.CreateTranslation(new Vector3(-Entity.Transform.Position, 0.0f)) * 
                                    Matrix.CreateRotationZ(Entity.Transform.Rotation) * 
                                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f)) *
                                    Matrix.CreateTranslation(new Vector3(App.Instance.GraphicsDevice.Viewport.Bounds.Width * 0.5f, 
                                        App.Instance.GraphicsDevice.Viewport.Bounds.Height * 0.5f, 0));

        public Matrix Inverse => Matrix.Invert(Transform);

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Vector3 worldPosition = Vector3.Transform(new Vector3(screenPosition, 0.0f), Inverse);
            return new Vector2(worldPosition.X, worldPosition.Y);
        }

        public override void Update()
        {
            Vector2 movement = Vector2.Zero;

            if (App.Instance.Input.IsKeyDown(Keys.Up)) movement.Y -= 1.0f;
            if (App.Instance.Input.IsKeyDown(Keys.Down)) movement.Y += 1.0f;
            if (App.Instance.Input.IsKeyDown(Keys.Left)) movement.X -= 1.0f;
            if (App.Instance.Input.IsKeyDown(Keys.Right)) movement.X += 1.0f;

            Entity.Transform.Position += movement * Time.Delta * 250.0f;
        }
    }
}
