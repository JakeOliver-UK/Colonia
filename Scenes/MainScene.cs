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

            Tilemap = new(1, 8, 8, 100, 100);
            Tilemap[0].FillTiles("Plains_Grass", Color.White);

            Entity entity = WorldEntityManager.Create("Human");
            entity.Transform.Position = new(100, 100);
            entity.Transform.Scale = new(3.0f, 3.0f);
            entity.AddComponent<SpriteRenderer>().LayerDepth = 1.0f;
            entity.AddComponent<Creature>();
        }
    }
}
