using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Colonia.Engine.Utils.Extensions
{
    internal static class SpriteBatchExtensions
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness, float layerDepth)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(App.Instance.AssetManager.Images.Pixel, start, null, color, angle, Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, layerDepth);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, float thickness, float layerDepth)
        {
            spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color, thickness, layerDepth);
            spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Left, rectangle.Bottom), color, thickness, layerDepth);
            spriteBatch.DrawLine(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), color, thickness, layerDepth);
            spriteBatch.DrawLine(new Vector2(rectangle.Left - 1, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), color, thickness, layerDepth);
        }
    }
}
