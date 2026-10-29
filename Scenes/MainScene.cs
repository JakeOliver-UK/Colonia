using Colonia.Engine;
using Colonia.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Colonia.Scenes
{
    internal class MainScene : Scene
    {
        private Tilemap _tilemap;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;

            _tilemap = new Tilemap(256, 256, 8, 8, 2);
            _tilemap.Set(0, 0, 0, _tilemap.Width, _tilemap.Height, "water");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Textures.Add("human_idle", ContentManager.Load<Texture2D>("human_idle"));
            Textures.Add("plains_tileset", ContentManager.Load<Texture2D>("plains_tileset"));

            Sprite humanSprite = new(
                "human_idle",
                [
                    new(12, 12, 8, 8),
                    new(44, 12, 8, 8),
                    new(76, 12, 8, 8),
                    new(108, 12, 8, 8),
                    new(140, 12, 8, 8),
                    new(172, 12, 8, 8),
                    new(204, 12, 8, 8),
                    new(236, 12, 8, 8),
                    new(268, 12, 8, 8),
                    new(300, 12, 8, 8),
                    new(332, 12, 8, 8),
                    new(364, 12, 8, 8),
                    new(396, 12, 8, 8),
                    new(428, 12, 8, 8),
                    new(460, 12, 8, 8),
                    new(492, 12, 8, 8)
                ],
                150.0f);

            Sprite grassSprite = new(
                "plains_tileset",
                [
                    new(294, 85, 8, 8)
                ],
                150.0f);

            Sprite waterSprite = new(
                "plains_tileset",
                [
                    new(208, 8, 8, 8),
                    new(240, 8, 8, 8)
                ],
                600.0f);

            Sprites.Add("human_idle", humanSprite);
            Sprites.Add("grass", grassSprite);
            Sprites.Add("water", waterSprite);
        }

        public override void Update()
        {
            base.Update();

            Sprites["human_idle"].Update();
            Sprites["grass"].Update();
            Sprites["water"].Update();

            if (InputManager.IsKeyDown(Keys.W)) Camera.Position += new Vector2(0.0f, -1.0f) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.S)) Camera.Position += new Vector2(0.0f, 1.0f) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.A)) Camera.Position += new Vector2(-1.0f, 0.0f) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.D)) Camera.Position += new Vector2(1.0f, 0.0f) * 100.0f * Time.Delta;
        }

        public override void DrawWorld()
        {
            base.DrawWorld();

            _tilemap.Draw();
            App.Instance.SpriteBatch.Draw(Textures[Sprites["human_idle"].Texture], new Vector2(80.0f, 40.0f), Sprites["human_idle"].Current, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1.0f);
        }
    }
}