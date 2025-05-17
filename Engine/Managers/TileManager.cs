using Colonia.Engine.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Colonia.Engine.Managers
{
    internal class TileManager : IDisposable
    {
        public static string DirectoryPath => Path.Combine(AssetManager.DirectoryPath, "Tiles");

        private Dictionary<string, Tile> _tiles;

        public TileManager()
        {
            _tiles = [];
            Load(DirectoryPath, false);
        }

        public void Load(string directoryPath, bool canOverride)
        {
            Log.WriteLine(LogLevel.Info, $"Loading tiles from '{directoryPath}' into Asset Manager...");

            int count = 0;

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string[] files = [.. Directory
                .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Path.GetExtension(f).Equals(".tile", StringComparison.OrdinalIgnoreCase))];

            if (files.Length == 0)
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to load tiles from '{directoryPath}' into Asset Manager as no tiles files were found.");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                Tile tile;

                try
                {
                    string json = File.ReadAllText(filePath);
                    tile = JsonConvert.DeserializeObject<Tile>(json);
                }
                catch (JsonException ex)
                {
                    Log.WriteLine(LogLevel.Error, $"Failed to load tiles '{fileName}' from file '{filePath}': {ex.Message}");
                    continue;
                }

                Add(fileName, tile, canOverride);

                Log.WriteLine(LogLevel.Info, $"Loaded tile '{fileName}' from file '{filePath}' into Asset Manager.");
            }

            Log.WriteLine(LogLevel.Info, $"Loaded {count} tiles from '{directoryPath}' into Asset Manager.");
        }

        public void Add(string name, Tile tile, bool canOverride = false)
        {
            if (_tiles.ContainsKey(name) && !canOverride)
            {
                Log.WriteLine(LogLevel.Warning, $"Cannot add tile '{name}' to Asset Manager as a tile with this name already exists.");
                return;
            }
            _tiles[name] = tile;
        }

        public void Remove(string name)
        {
            if (_tiles.TryGetValue(name, out Tile value))
            {
                value.Dispose();
                _tiles.Remove(name);
            }
            else
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to remove tile '{name}' from Asset Manager as it does not exist.");
            }
        }

        public Tile Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            if (_tiles.TryGetValue(name, out Tile value)) return value;

            Log.WriteLine(LogLevel.Warning, $"Unable to get tile '{name}' from Asset Manager as it does not exist.");
            return null;
        }

        public void Dispose()
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                KeyValuePair<string, Tile> image = _tiles.ElementAt(i);
                image.Value.Dispose();
            }

            _tiles.Clear();
            _tiles = null;
        }

        public Tile this[string name] => Get(name);
        public bool Contains(string name) => _tiles.ContainsKey(name);
        public bool Contains(Tile tile) => _tiles.ContainsValue(tile);
    }
}
