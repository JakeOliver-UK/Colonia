using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colonia.Engine.Entities.Components
{
    internal class SpriteRenderer : Component
    {
        public string Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0.0f;
        public Sprite SpriteObject => App.Instance.AssetManager.Sprites.Get(Sprite);

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
            Rectangle frame = SpriteObject.Frames[SpriteObject.CurrentFrame];
            Vector2 position = new(Entity.Transform.Position.X, Entity.Transform.Position.Y);
            Vector2 origin = new(frame.Width / 2, frame.Height / 2);
            float rotation = Entity.Transform.Rotation;
            Vector2 scale = new(Entity.Transform.Scale.X, Entity.Transform.Scale.Y);

            App.Instance.SpriteBatch.Draw(image, position, frame, Color, rotation, origin, scale, Effects, LayerDepth);
        }
    }
}
