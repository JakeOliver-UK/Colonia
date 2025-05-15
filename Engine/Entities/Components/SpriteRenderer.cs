using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colonia.Engine.Entities.Components
{
    internal class SpriteRenderer : Component
    {
        public Sprite Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0.0f;

        public override void Update()
        {
            base.Update();
            Sprite.Update();
        }

        public override void Draw()
        {
            Texture2D image = App.Instance.AssetManager.Images.Get(Sprite.Images[Sprite.CurrentFrame]);
            Rectangle frame = Sprite.Frames[Sprite.CurrentFrame];
            Vector2 position = new(Entity.Transform.Position.X, Entity.Transform.Position.Y);
            Vector2 origin = new(frame.Width / 2, frame.Height / 2);
            float rotation = Entity.Transform.Rotation;
            Vector2 scale = new(Entity.Transform.Scale.X, Entity.Transform.Scale.Y);

            App.Instance.SpriteBatch.Draw(image, position, frame, Color, rotation, origin, scale, Effects, LayerDepth);
        }
    }
}
