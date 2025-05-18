using Colonia.Engine;
using Colonia.Engine.Entities;
using Colonia.Engine.Entities.Components;
using Colonia.Engine.Utils.Extensions;
using Microsoft.Xna.Framework;
using System;

namespace Colonia.Scenes
{
    internal class MainScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;

            var tiles = MapGenerator.GenerateRiverMap(DateTime.Now.Ticks.ToString().ToSeed(), 256, 256, 15, 2.5f, 0.002f, 6, 2);

            Tilemap = new(1, 8, 8, 256, 256);
            
            for (int x = 0; x < Tilemap.Width; x++)
            {
                for (int y = 0; y < Tilemap.Height; y++)
                {
                    var value = tiles[x, y];
                    if (value == TileType.Grass)
                    {
                        Tilemap[0, x, y].Tile = "Plains_Grass";
                    }
                    else
                    {
                        Tilemap[0, x, y].Tile = "Plains_Water";
                    }
                }
            }

            Entity entity = WorldEntityManager.Create("Human");
            entity.Transform.Position = new(100, 100);
            entity.Transform.Scale = Vector2.One;
            entity.AddComponent<SpriteRenderer>().LayerDepth = 1.0f;
            entity.AddComponent<Creature>();
        }
    }
}
