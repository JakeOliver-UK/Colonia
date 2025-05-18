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

        public static void DrawNinePatch(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destRect, Rectangle sourceRect, int border, Color color, SpriteEffects spriteEffects, float layerDepth) =>
            DrawNinePatch(spriteBatch, texture, destRect, sourceRect, border, border, border, border, color, spriteEffects, layerDepth);

        public static void DrawNinePatch(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destRect, Rectangle sourceRect, int left, int top, int right, int bottom, Color color, SpriteEffects spriteEffects, float layerDepth)
        {
            int srcW = sourceRect.Width;
            int srcH = sourceRect.Height;

            var srcTL = new Rectangle(sourceRect.Left, sourceRect.Top, left, top);
            var srcT = new Rectangle(sourceRect.Left + left, sourceRect.Top, srcW - left - right, top);
            var srcTR = new Rectangle(sourceRect.Right - right, sourceRect.Top, right, top);

            var srcL = new Rectangle(sourceRect.Left, sourceRect.Top + top, left, srcH - top - bottom);
            var srcC = new Rectangle(sourceRect.Left + left, sourceRect.Top + top, srcW - left - right, srcH - top - bottom);
            var srcR = new Rectangle(sourceRect.Right - right, sourceRect.Top + top, right, srcH - top - bottom);

            var srcBL = new Rectangle(sourceRect.Left, sourceRect.Bottom - bottom, left, bottom);
            var srcB = new Rectangle(sourceRect.Left + left, sourceRect.Bottom - bottom, srcW - left - right, bottom);
            var srcBR = new Rectangle(sourceRect.Right - right, sourceRect.Bottom - bottom, right, bottom);

            var dstTL = new Rectangle(destRect.Left, destRect.Top, left, top);
            var dstT = new Rectangle(destRect.Left + left, destRect.Top, destRect.Width - left - right, top);
            var dstTR = new Rectangle(destRect.Right - right, destRect.Top, right, top);

            var dstL = new Rectangle(destRect.Left, destRect.Top + top, left, destRect.Height - top - bottom);
            var dstC = new Rectangle(destRect.Left + left, destRect.Top + top, destRect.Width - left - right, destRect.Height - top - bottom);
            var dstR = new Rectangle(destRect.Right - right, destRect.Top + top, right, destRect.Height - top - bottom);

            var dstBL = new Rectangle(destRect.Left, destRect.Bottom - bottom, left, bottom);
            var dstB = new Rectangle(destRect.Left + left, destRect.Bottom - bottom, destRect.Width - left - right, bottom);
            var dstBR = new Rectangle(destRect.Right - right, destRect.Bottom - bottom, right, bottom);

            spriteBatch.Draw(texture, dstTL, srcTL, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);
            spriteBatch.Draw(texture, dstT, srcT, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);
            spriteBatch.Draw(texture, dstTR, srcTR, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);

            spriteBatch.Draw(texture, dstL, srcL, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);
            spriteBatch.Draw(texture, dstC, srcC, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);
            spriteBatch.Draw(texture, dstR, srcR, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);

            spriteBatch.Draw(texture, dstBL, srcBL, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);
            spriteBatch.Draw(texture, dstB, srcB, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);
            spriteBatch.Draw(texture, dstBR, srcBR, color, 0.0f, Vector2.Zero, spriteEffects, layerDepth);
        }
    }
}
