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

            Entity entity = WorldEntityManager.Create("Human");
            entity.Transform.Position = new(100, 100);
            entity.Transform.Scale = new(3.0f, 3.0f);
            entity.AddComponent<SpriteRenderer>();
            entity.AddComponent<Creature>();
        }
    }
}
