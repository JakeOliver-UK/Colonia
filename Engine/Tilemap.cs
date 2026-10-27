using Colonia.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Colonia.Engine
{
    internal class Tilemap
    {
        public int Width { get; }
        public int Height { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public TileLayer[] Layers { get; }

        private static readonly JsonSerializerSettings _jsonSettings = new() { Formatting = Formatting.Indented };

        public Tilemap(int width, int height, int tileWidth, int tileHeight, int layerCount)
        {
            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Layers = new TileLayer[layerCount];
            
            for (int i = 0; i < layerCount; i++)
            {
                Layers[i] = new TileLayer(i, width, height, tileWidth, tileHeight);
            }
        }

        [JsonConstructor]
        public Tilemap(int width, int height, int tileWidth, int tileHeight, TileLayer[] layers)
        {
            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Layers = layers;
        }

        public void Draw()
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Tile tile = Layers[i][x, y];
                        if (tile != null && tile.Sprite != null)
                        {
                            float layerDepth = i / 100000;
                            App.Instance.SpriteBatch.Draw(SceneManager.Current?.Textures[SceneManager.Current?.Sprites[tile.Sprite].Texture], new(x * TileWidth, y * TileHeight), SceneManager.Current?.Sprites[tile.Sprite].Current, tile.Color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layerDepth);
                        }
                    }
                }
            }
        }

        public TileLayer this[int layer] => Get(layer);
        public Tile this[int layer, int x, int y] => Get(layer, x, y);
        public TileLayer Get(int layer) => Layers[layer];
        public Tile Get(int layer, int x, int y) => Layers[layer][x, y];
        public void Set(int layer, int x, int y, string sprite) => Layers[layer].Set(x, y, sprite);
        public void Set(int layer, int x, int y, string sprite, Color color) => Layers[layer].Set(x, y, sprite, color);
        public void Set(int layer, int x, int y, int width, int height, string sprite) => Layers[layer].Set(x, y, width, height, sprite);
        public void Set(int layer, int x, int y, int width, int height, string sprite, Color color) => Layers[layer].Set(x, y, width, height, sprite, color);

        public string ToJson() => JsonConvert.SerializeObject(this, _jsonSettings);
        public static Tilemap FromJson(string json) => JsonConvert.DeserializeObject<Tilemap>(json, _jsonSettings);
    }

    internal class TileLayer
    {
        public int Layer { get; }
        public int Width { get; }
        public int Height { get; }
        public Tile[,] Tiles { get; }

        public TileLayer(int layer, int width, int height, int tileWidth, int tileHeight)
        {
            Layer = layer;
            Width = width;
            Height = height;
            Tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tiles[x, y] = new Tile(x, y, tileWidth, tileHeight);
                }
            }
        }

        [JsonConstructor]
        public TileLayer(int layer, int width, int height, Tile[,] tiles)
        {
            Layer = layer;
            Width = width;
            Height = height;
            Tiles = tiles;
        }

        public Tile this[int x, int y] => Get(x, y);
        public Tile Get(int x, int y) => Tiles[x, y];

        public Tile[] Get(int x, int y, int width, int height)
        {
            Tile[] result = new Tile[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[i * height + j] = Tiles[x + i, y + j];
                }
            }
            return result;
        }

        public void Set(int x, int y, string sprite) => Set(x, y, sprite, Color.White);
        public void Set(int x, int y, string sprite, Color color) => Tiles[x, y] = new Tile(x, y, Width, Height, sprite, color);
        
        public void Set(int x, int y, int width, int height, string sprite) => Set(x, y, width, height, sprite, Color.White);

        public void Set(int x, int y, int width, int height, string sprite, Color color)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tiles[x + i, y + j] = new Tile(x + i, y + j, Width, Height, sprite, color);
                }
            }
        }
    }

    internal class Tile
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public string Sprite { get; set; } = null;
        public Color Color { get; set; } = Color.White;

        public Tile(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        [JsonConstructor]
        public Tile(int x, int y, int width, int height, string sprite, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Sprite = sprite;
            Color = color;
        }
    }
}
