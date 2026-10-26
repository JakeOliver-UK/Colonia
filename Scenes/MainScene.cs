using Colonia.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colonia.Scenes
{
    internal class MainScene : Scene
    {
        private Texture2D _texture;
        private Sprite _sprite;

        public override void LoadContent()
        {
            base.LoadContent();

            _texture = ContentManager.Load<Texture2D>("human_idle");
            _sprite = new Sprite(_texture, 16);
            _sprite.Frames[0] = new Rectangle(12, 12, 8, 8);
            _sprite.Frames[1] = new Rectangle(44, 12, 8, 8);
            _sprite.Frames[2] = new Rectangle(76, 12, 8, 8);
            _sprite.Frames[3] = new Rectangle(108, 12, 8, 8);
            _sprite.Frames[4] = new Rectangle(140, 12, 8, 8);
            _sprite.Frames[5] = new Rectangle(172, 12, 8, 8);
            _sprite.Frames[6] = new Rectangle(204, 12, 8, 8);
            _sprite.Frames[7] = new Rectangle(236, 12, 8, 8);
            _sprite.Frames[8] = new Rectangle(268, 12, 8, 8);
            _sprite.Frames[9] = new Rectangle(300, 12, 8, 8);
            _sprite.Frames[10] = new Rectangle(332, 12, 8, 8);
            _sprite.Frames[11] = new Rectangle(364, 12, 8, 8);
            _sprite.Frames[12] = new Rectangle(396, 12, 8, 8);
            _sprite.Frames[13] = new Rectangle(428, 12, 8, 8);
            _sprite.Frames[14] = new Rectangle(460, 12, 8, 8);
            _sprite.Frames[15] = new Rectangle(492, 12, 8, 8);
        }

        public override void Update()
        {
            base.Update();
            _sprite.Update();
        }

        public override void DrawWorld()
        {
            base.DrawWorld();
            App.Instance.SpriteBatch.Draw(_sprite.Texture, new Vector2(100, 100), _sprite.Current, Color.White);
        }
    }
}
