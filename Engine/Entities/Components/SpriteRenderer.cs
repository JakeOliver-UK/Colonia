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
        public Sprite SpriteObject => App.Instance.AssetManager.Sprites.Get(Sprite);
        public Vector2 Origin => SpriteObject != null ? new(SpriteObject.Frames[SpriteObject.CurrentFrame].Width / 2, SpriteObject.Frames[SpriteObject.CurrentFrame].Height / 2) : Vector2.Zero;
        public int Width { get; set; } = 8;
        public int Height { get; set; } = 8;
        public Rectangle Bounds => new((int)Entity.Transform.Position.X - (Width / 2), (int)Entity.Transform.Position.Y - (Height / 2), Width, Height);

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
            
            if (App.Instance.SceneManager.Current.Camera.IsVisible(Bounds))
            {
                if (Transparency > 1.0f) Transparency = 1.0f;
                if (Transparency < 0.0f) Transparency = 0.0f;
                App.Instance.SpriteBatch.Draw(image, Entity.Transform.Position, SpriteObject.Frames[SpriteObject.CurrentFrame], Color * Transparency, Entity.Transform.Rotation, Origin, Entity.Transform.Scale, Effects, LayerDepth);
            }
            
            if (DrawBounds) App.Instance.SpriteBatch.DrawRectangle(Bounds, Color.Lime, 1.0f, 1.0f);
        }
    }
}
