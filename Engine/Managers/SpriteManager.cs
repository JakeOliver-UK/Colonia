using Colonia.Engine.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Colonia.Engine.Managers
{
    internal class SpriteManager
    {
        public static string DirectoryPath => Path.Combine(AssetManager.DirectoryPath, "Sprites");

        private Dictionary<string, Sprite> _sprites;

        public SpriteManager()
        {
            _sprites = [];
            Load(DirectoryPath, false);
        }

        public void Load(string directoryPath, bool canOverride)
        {
            Log.WriteLine(LogLevel.Info, $"Loading sprites from '{directoryPath}' into Asset Manager...");

            int count = 0;

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string[] files = [.. Directory
                .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Path.GetExtension(f).Equals(".sprite", StringComparison.OrdinalIgnoreCase))];

            if (files.Length == 0)
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to load sprites from '{directoryPath}' into Asset Manager as no sprite files were found.");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                Sprite sprite;

                try
                {
                    string json = File.ReadAllText(filePath);
                    sprite = JsonConvert.DeserializeObject<Sprite>(json);
                }
                catch (JsonException ex)
                {
                    Log.WriteLine(LogLevel.Error, $"Failed to load sprite '{fileName}' from file '{filePath}': {ex.Message}");
                    continue;
                }

                Add(fileName, sprite, canOverride);

                Log.WriteLine(LogLevel.Info, $"Loaded sprite '{fileName}' from file '{filePath}' into Asset Manager.");
            }

            Log.WriteLine(LogLevel.Info, $"Loaded {count} sprites from '{directoryPath}' into Asset Manager.");
        }

        public void Add(string name, Sprite sprite, bool canOverride = false)
        {
            if (_sprites.ContainsKey(name) && !canOverride)
            {
                Log.WriteLine(LogLevel.Warning, $"Cannot add sprite '{name}' to Asset Manager as a sprite with this name already exists.");
                return;
            }
            _sprites[name] = sprite;
        }

        public void Remove(string name)
        {
            if (_sprites.TryGetValue(name, out Sprite value))
            {
                value.Dispose();
                _sprites.Remove(name);
            }
            else
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to remove sprite '{name}' from Asset Manager as it does not exist.");
            }
        }

        public Sprite Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            if (_sprites.TryGetValue(name, out Sprite value)) return value;

            Log.WriteLine(LogLevel.Warning, $"Unable to get sprite '{name}' from Asset Manager as it does not exist.");
            return null;
        }

        public void Dispose()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                KeyValuePair<string, Sprite> image = _sprites.ElementAt(i);
                image.Value.Dispose();
            }

            _sprites.Clear();
            _sprites = null;
        }

        public Sprite this[string name] => Get(name);
        public bool Contains(string name) => _sprites.ContainsKey(name);
        public bool Contains(Sprite image) => _sprites.ContainsValue(image);
    }
}
