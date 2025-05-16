using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colonia.Engine.Utils.Extensions
{
    internal static class SpriteBatchExtensions
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness, float layerDepth)
        {
            Vector2 direction = end - start;
            float length = direction.Length();
            if (length == 0) return;
            direction.Normalize();
            Vector2 perpendicular = new Vector2(-direction.Y, direction.X) * (thickness / 2f);
            Vector2[] vertices =
            [
                start + perpendicular,
                start - perpendicular,
                end - perpendicular,
                end + perpendicular
            ];
            spriteBatch.Draw(App.Instance.AssetManager.Images.Pixel, vertices[0], null, color, 0f, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, layerDepth);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, float thickness, float layerDepth)
        {
            spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color, thickness, layerDepth);
            spriteBatch.DrawLine(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), color, thickness, layerDepth);
            spriteBatch.DrawLine(new Vector2(rectangle.Right, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Bottom), color, thickness, layerDepth);
            spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Top), color, thickness, layerDepth);
        }
    }
}
