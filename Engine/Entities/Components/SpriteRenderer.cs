using Colonia.Engine.Utils.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colonia.Engine.Entities.Components
{
    internal class SpriteRenderer : Component
    {
        public string Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Transparency { get; set; } = 1.0f;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0.0f;
        public SpritePivot Pivot { get; set; } = SpritePivot.Center;
        public SpriteRenderMethod RenderMethod { get; set; } = SpriteRenderMethod.Normal;
        public Sprite SpriteObject => App.Instance.AssetManager.Sprites.Get(Sprite);
        
        public Vector2 Origin
        {
            get
            {
                if (SpriteObject == null || string.IsNullOrEmpty(Sprite)) return Vector2.Zero;
                return Pivot switch
                {
                    SpritePivot.Center => new(SpriteObject.Frames[SpriteObject.CurrentFrame].Width / 2, SpriteObject.Frames[SpriteObject.CurrentFrame].Height / 2),
                    SpritePivot.TopLeft => Vector2.Zero,
                    SpritePivot.TopRight => new(SpriteObject.Frames[SpriteObject.CurrentFrame].Width, 0),
                    SpritePivot.BottomLeft => new(0, SpriteObject.Frames[SpriteObject.CurrentFrame].Height),
                    SpritePivot.BottomRight => new(SpriteObject.Frames[SpriteObject.CurrentFrame].Width, SpriteObject.Frames[SpriteObject.CurrentFrame].Height),
                    SpritePivot.Top => new(SpriteObject.Frames[SpriteObject.CurrentFrame].Width / 2, 0),
                    SpritePivot.Bottom => new(SpriteObject.Frames[SpriteObject.CurrentFrame].Width / 2, SpriteObject.Frames[SpriteObject.CurrentFrame].Height),
                    SpritePivot.Left => new(0, SpriteObject.Frames[SpriteObject.CurrentFrame].Height / 2),
                    SpritePivot.Right => new(SpriteObject.Frames[SpriteObject.CurrentFrame].Width, SpriteObject.Frames[SpriteObject.CurrentFrame].Height / 2),
                    _ => Vector2.Zero
                };
            }
        }
        
        public int Width { get; set; } = 8;
        public int Height { get; set; } = 8;
        public Rectangle NinePatchPosition => new((int)Entity.Transform.Position.X, (int)Entity.Transform.Position.Y, Width, Height);
        public int BorderLeft { get; set; } = 10;
        public int BorderTop { get; set; } = 10;
        public int BorderRight { get; set; } = 10;
        public int BorderBottom { get; set; } = 10;
        
        public Rectangle Bounds
        {
            get
            {
                if (RenderMethod == SpriteRenderMethod.Normal) return new((int)Entity.Transform.Position.X - ((int)Origin.X / 4), (int)Entity.Transform.Position.Y - ((int)Origin.Y / 4), Width, Height);
                else return NinePatchPosition;
            }
        }

        public bool DrawBounds = false;

        public override void Update()
        {
            base.Update();

            if (SpriteObject == null || string.IsNullOrEmpty(Sprite)) return;
            SpriteObject.Update();
        }

        public override void Draw()
        {
            base.Draw();

            if (SpriteObject == null || string.IsNullOrEmpty(Sprite)) return;
            Texture2D image = App.Instance.AssetManager.Images.Get(SpriteObject.Images[SpriteObject.CurrentFrame]);
            if (image == null) return;

            if (Transparency > 1.0f) Transparency = 1.0f;
            if (Transparency < 0.0f) Transparency = 0.0f;

            if (!App.Instance.SceneManager.Current.Camera.IsVisible(Bounds) && !Entity.Manager.IsOverlayManager) return;

            if (RenderMethod == SpriteRenderMethod.Normal)
            {
                App.Instance.SpriteBatch.Draw(image, Entity.Transform.Position, SpriteObject.Frames[SpriteObject.CurrentFrame], Color * Transparency, Entity.Transform.Rotation, Origin, Entity.Transform.Scale, Effects, LayerDepth);
            }
            else if (RenderMethod == SpriteRenderMethod.NinePatch)
            {
                App.Instance.SpriteBatch.DrawNinePatch(image, NinePatchPosition, SpriteObject.Frames[SpriteObject.CurrentFrame], BorderLeft, BorderTop, BorderRight, BorderBottom, Color * Transparency, Effects, LayerDepth);
            }
            
            if (DrawBounds) App.Instance.SpriteBatch.DrawRectangle(Bounds, Color.Lime, 1.0f, 1.0f);
        }
    }

    internal enum SpritePivot { Center, TopLeft, TopRight, BottomLeft, BottomRight, Top, Bottom, Left, Right }
    internal enum SpriteRenderMethod { Normal, NinePatch }
}
