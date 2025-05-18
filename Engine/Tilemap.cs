using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Colonia.Engine
{
    internal class Tilemap
    {
        public TileLayer[] Layers { get; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public int Width => Layers[0].Width;
        public int Height => Layers[0].Height;

        public Tilemap(int layerCount, int tileWidth, int tileHeight, int width, int height)
        {
            if (layerCount <= 0) layerCount = 1;
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

        public void Update()
        {
            List<Sprite> sprites = [];
            for (int z = 0; z < Layers.Length; z++)
            {
                TileLayer layer = Layers[z];
                for (int x = 0; x < layer.Width; x++)
                {
                    for (int y = 0; y < layer.Height; y++)
                    {
                        TileCell tile = layer[x, y];
                        if (tile.Tile != null)
                        {
                            Tile tileObj = App.Instance.AssetManager.Tiles.Get(tile.Tile);
                            int value = 0;
                            if (tileObj.IsAutotile) value = GetAutotileValue(x, y, z);
                            Sprite sprite = App.Instance.AssetManager.Sprites.Get(tileObj.Sprites[value]);
                            if (sprite != null)
                            {
                                if (!sprites.Contains(sprite))
                                {
                                    sprites.Add(sprite);
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < sprites.Count; i++)
            {
                Sprite sprite = sprites[i];
                if (sprite == null) continue;
                sprite.Update();
            }
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
                        if (tile.Tile != null)
                        {
                            Tile tileObj = App.Instance.AssetManager.Tiles.Get(tile.Tile);
                            int value = 0;
                            if (tileObj.IsAutotile) value = GetAutotileValue(x, y, z);
                            Sprite sprite = App.Instance.AssetManager.Sprites.Get(tileObj.Sprites[value]);
                            if (sprite != null)
                            {
                                Vector2 position = new(tile.X * tile.Width * Scale.X, tile.Y * tile.Height * Scale.Y);
                                Rectangle bounds = new((int)position.X, (int)position.Y, tile.Width, tile.Height);
                                if (!App.Instance.SceneManager.Current.Camera.IsVisible(bounds)) continue;
                                Texture2D image = App.Instance.AssetManager.Images.Get(sprite.Images[sprite.CurrentFrame]);
                                if (image == null) continue;
                                Rectangle frame = sprite.Frames[sprite.CurrentFrame];
                                Vector2 origin = Vector2.Zero;
                                float rotation = 0.0f;
                                App.Instance.SpriteBatch.Draw(image, position, frame, tile.Color, rotation, origin, Scale, SpriteEffects.None, z / 1000);
                            }
                        }
                    }
                }
            }
        }

        private int GetAutotileValue(int x, int y, int layer)
        {
            TileCell tileCell = Layers[layer].GetTile(x, y);
            if (tileCell == null) return 0;
            if (tileCell.Tile == null) return 0;
            Tile tile = App.Instance.AssetManager.Tiles.Get(tileCell.Tile);
            if (tile == null) return 0;
            if (!tile.IsAutotile) return 0;

            int value = 0;

            TileCell upLeft = Layers[layer].GetTile(x - 1, y - 1);
            TileCell up = Layers[layer].GetTile(x, y - 1);
            TileCell upRight = Layers[layer].GetTile(x + 1, y - 1);
            TileCell left = Layers[layer].GetTile(x - 1, y);
            TileCell right = Layers[layer].GetTile(x + 1, y);
            TileCell downLeft = Layers[layer].GetTile(x - 1, y + 1);
            TileCell down = Layers[layer].GetTile(x, y + 1);
            TileCell downRight = Layers[layer].GetTile(x + 1, y + 1);

            if (upLeft.Tile == tile.Name) value += 1;
            if (up.Tile == tile.Name) value += 2;
            if (upRight.Tile == tile.Name) value += 4;
            if (left.Tile == tile.Name) value += 8;
            if (right.Tile == tile.Name) value += 16;
            if (downLeft.Tile == tile.Name) value += 32;
            if (down.Tile == tile.Name) value += 64;
            if (downRight.Tile == tile.Name) value += 128;

            return value;
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
            Tiles[x, y].Tile = sprite;
            Tiles[x, y].Color = color;
        }

        public void FillTiles(string sprite, Color color)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tiles[x, y].Tile = sprite;
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
                    Tiles[x, y].Tile = sprite;
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
        public string Tile { get; set; }
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
        public TileCell(int x, int y, int width, int height, string tile, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Tile = tile;
            Color = color;
        }
    }

    public class Tile : IDisposable
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

        public void Dispose()
        {
            Sprites.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
