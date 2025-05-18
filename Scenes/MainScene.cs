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
            
            for (int x = 0; x < Tilemap.Width; x++)
            {
                for (int y = 0; y < Tilemap.Height; y++)
                {
                    Tilemap[0, x, y].Tile = "Plains_Water";
                }
            }

            Entity entity = WorldEntityManager.Create("Human");
            entity.Transform.Position = new(100, 100);
            entity.AddComponent<SpriteRenderer>().LayerDepth = 1.0f;
            entity.AddComponent<Creature>();

            Entity panel = OverlayEntityManager.Create("Panel");
            panel.Transform.Position = new(50, 50);
            panel.Transform.Scale = new(2.0f, 2.0f);
            panel.AddComponent<SpriteRenderer>().Sprite = "UI_Panel_1";
            panel.GetComponent<SpriteRenderer>().Transparency = 0.75f;
            panel.GetComponent<SpriteRenderer>().LayerDepth = 0.0f;
            panel.GetComponent<SpriteRenderer>().Width = 400;
            panel.GetComponent<SpriteRenderer>().Height = 200;
            panel.GetComponent<SpriteRenderer>().RenderMethod = SpriteRenderMethod.NinePatch;
        }
    }
}
