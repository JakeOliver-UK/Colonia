using Colonia.Engine;
using Colonia.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Colonia.Scenes
{
    internal class MainScene : Scene
    {
        public override void LoadContent()
        {
            base.LoadContent();

            Textures.Add("human_idle", ContentManager.Load<Texture2D>("human_idle"));
            
            Sprite sprite = new(
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
            
            Sprites.Add("human_idle", sprite);
        }

        public override void Update()
        {
            base.Update();

            Sprites["human_idle"].Update();

            if (InputManager.IsKeyDown(Keys.W)) Camera.Position += new Vector2(0, -1) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.S)) Camera.Position += new Vector2(0, 1) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.A)) Camera.Position += new Vector2(-1, 0) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.D)) Camera.Position += new Vector2(1, 0) * 100.0f * Time.Delta;
        }

        public override void DrawWorld()
        {
            base.DrawWorld();

            App.Instance.SpriteBatch.Draw(Textures[Sprites["human_idle"].Texture], Vector2.Zero, Sprites["human_idle"].Current, Color.White);
        }
    }
}
