using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Colonia.Engine
{
    internal class Tilemap
    {
        public TileLayer[] Layers { get; }
        public Vector2 Scale { get; set; } = Vector2.One;

        public Tilemap(int layerCount, int tileWidth, int tileHeight, int width, int height)
        {
            Layers = new TileLayer[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                Layers[i] = new TileLayer(i, width, height, tileWidth, tileHeight);
            }
        }

        [JsonConstructor]
        public Tilemap(TileLayer[] layers, Vector2 scale)
        {
            Layers = layers;
            Scale = scale;
        }

        public TileLayer GetLayer(int layer)
        {
            if (layer < 0 || layer >= Layers.Length) return null;
            return Layers[layer];
        }

        public void Draw()
        {
            for (int z = 0; z < Layers.Length; z++)
            {
                TileLayer layer = Layers[z];
                for (int x = 0; x < layer.Width; x++)
                {
                    for (int y = 0; y < layer.Height; y++)
                    {
                        TileCell tile = layer[x, y];
                        if (tile.Sprite != null)
                        {
                            Sprite sprite = App.Instance.AssetManager.Sprites.Get(tile.Sprite);
                            if (sprite != null)
                            {
                                sprite.Update();
                                Texture2D image = App.Instance.AssetManager.Images.Get(sprite.Images[sprite.CurrentFrame]);
                                Rectangle frame = sprite.Frames[sprite.CurrentFrame];
                                Vector2 position = new(tile.X * tile.Width * Scale.X, tile.Y * tile.Height * Scale.Y);
                                Vector2 origin = Vector2.Zero;
                                float rotation = 0.0f;
                                App.Instance.SpriteBatch.Draw(image, position, frame, tile.Color, rotation, origin, Scale, SpriteEffects.None, z / 1000);
                            }
                        }
                    }
                }
            }
        }

        public TileLayer this[int layer] => GetLayer(layer);
        public TileCell this[int layer, int x, int y] => GetLayer(layer)?.GetTile(x, y);
    }

    internal class TileLayer
    {
        public int Layer { get; }
        public TileCell[,] Tiles { get; }
        public int Width => Tiles.GetLength(0);
        public int Height => Tiles.GetLength(1);

        public TileLayer(int layer, int width, int height, int tileWidth, int tileHeight)
        {
            Layer = layer;
            Tiles = new TileCell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tiles[x, y] = new TileCell(x, y, tileWidth, tileHeight);
                }
            }
        }

        [JsonConstructor]
        public TileLayer(int layer, TileCell[,] tiles)
        {
            Layer = layer;
            Tiles = tiles;
        }

        public TileCell GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return null;
            return Tiles[x, y];
        }

        public TileCell this[int x, int y] => GetTile(x, y);

        public void SetTile(int x, int y, string sprite, Color color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            Tiles[x, y].Sprite = sprite;
            Tiles[x, y].Color = color;
        }

        public void FillTiles(string sprite, Color color)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tiles[x, y].Sprite = sprite;
                    Tiles[x, y].Color = color;
                }
            }
        }

        public void FillTiles(Rectangle area, string sprite, Color color)
        {
            if (area.X < 0 || area.X + area.Width > Width || area.Y < 0 || area.Y + area.Height > Height) return;
            for (int x = area.X; x < area.X + area.Width; x++)
            {
                for (int y = area.Y; y < area.Y + area.Height; y++)
                {
                    if (x < 0 || x >= Width || y < 0 || y >= Height) continue;
                    Tiles[x, y].Sprite = sprite;
                    Tiles[x, y].Color = color;
                }
            }
        }
    }

    internal class TileCell
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public Rectangle Bounds => new(X * Width, Y * Height, Width, Height);

        public TileCell(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        [JsonConstructor]
        public TileCell(int x, int y, int width, int height, string sprite, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Sprite = sprite;
            Color = color;
        }
    }

    public class Tile
    {
        public string Name { get; }
        public Dictionary<int, string> Sprites { get; }
        public bool IsAutotile { get; set; }

        public Tile(string name)
        {
            Name = name;
            Sprites = [];
            IsAutotile = false;
        }

        [JsonConstructor]
        public Tile(string name, Dictionary<int, string> sprites, bool isAutotile)
        {
            Name = name;
            Sprites = sprites;
            IsAutotile = isAutotile;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            string directoryPath = Path.Combine(AppInfo.ApplicationDirectoryPath, "Assets", "Tiles");
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            string path = Path.Combine(directoryPath, $"{Name}.tile");
            File.WriteAllText(path, json);
        }
    }
}
