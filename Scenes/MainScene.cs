using Colonia.Engine;
using Colonia.Engine.Entities;
using Colonia.Engine.Entities.Components;
using Microsoft.Xna.Framework;

namespace Colonia.Scenes
{
    internal class MainScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;

            Tilemap = new(1, 8, 8, 256, 256);
            Tilemap[0].FillTiles("Plains_Water", Color.White);

            Entity entity = WorldEntityManager.Create("Human");
            entity.Transform.Position = new(100, 100);
            entity.Transform.Scale = Vector2.One;
            entity.AddComponent<SpriteRenderer>().LayerDepth = 1.0f;
            entity.AddComponent<Creature>();
        }
    }
}
