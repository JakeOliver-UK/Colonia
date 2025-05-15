using Colonia.Engine;
using Colonia.Engine.Entities;
using Colonia.Engine.Entities.Components;

namespace Colonia.Scenes
{
    internal class MainScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();

            Sprite sprite = new("HumanIdle_DownRight", 16);
            for (int i = 0; i < sprite.Images.Length; i++)
            {
                sprite.Images[i] = "HumanIdle";
                sprite.Frames[i] = new(i * 32, 0, 32, 32);
            }
            sprite.Delay = 200.0f;
            sprite.IsLooping = true;
            sprite.ShouldAutoPlay = true;
            sprite.Play();
            sprite.Save();

            sprite = new("HumanIdle_DownLeft", 16);
            for (int i = 0; i < sprite.Images.Length; i++)
            {
                sprite.Images[i] = "HumanIdle";
                sprite.Frames[i] = new(i * 32, 32, 32, 32);
            }
            sprite.Delay = 200.0f;
            sprite.IsLooping = true;
            sprite.ShouldAutoPlay = true;
            sprite.Play();
            sprite.Save();

            sprite = new("HumanIdle_UpRight", 16);
            for (int i = 0; i < sprite.Images.Length; i++)
            {
                sprite.Images[i] = "HumanIdle";
                sprite.Frames[i] = new(i * 32, 64, 32, 32);
            }
            sprite.Delay = 200.0f;
            sprite.IsLooping = true;
            sprite.ShouldAutoPlay = true;
            sprite.Play();
            sprite.Save();

            sprite = new("HumanIdle_UpLeft", 16);
            for (int i = 0; i < sprite.Images.Length; i++)
            {
                sprite.Images[i] = "HumanIdle";
                sprite.Frames[i] = new(i * 32, 96, 32, 32);
            }
            sprite.Delay = 200.0f;
            sprite.IsLooping = true;
            sprite.ShouldAutoPlay = true;
            sprite.Play();
            sprite.Save();

            sprite = new("HumanIdle_DownRight", 16);
            for (int i = 0; i < sprite.Images.Length; i++)
            {
                sprite.Images[i] = "HumanIdle";
                sprite.Frames[i] = new(i * 32, 0, 32, 32);
            }
            sprite.Delay = 200.0f;
            sprite.IsLooping = true;
            sprite.ShouldAutoPlay = true;
            sprite.Play();
            sprite.Save();

            Entity entity = WorldEntityManager.Create("Human");
            entity.Transform.Position = new(100, 100);
            entity.Transform.Scale = new(3, 3);
            entity.AddComponent<SpriteRenderer>().Sprite = sprite;
        }
    }
}
