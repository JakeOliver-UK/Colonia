using Colonia.Engine.Utils;
using FontStashSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Colonia.Engine.Managers
{
    internal class FontManager : IDisposable
    {
        public static string DirectoryPath => Path.Combine(AssetManager.DirectoryPath, "Fonts");

        private Dictionary<string, FontSystem> _fonts;

        public FontManager()
        {
            _fonts = [];
            Load(DirectoryPath, false);
        }

        public void Load(string directoryPath, bool canOverride)
        {
            Log.WriteLine(LogLevel.Info, $"Loading fonts from '{directoryPath}' into Asset Manager...");

            int count = 0;

            string[] files = [.. Directory
                .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Path.GetExtension(f).Equals(".ttf", StringComparison.OrdinalIgnoreCase))];

            if (files.Length == 0)
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to load fonts from '{directoryPath}' into Asset Manager as no font files were found.");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                
                Add(fileName, filePath, canOverride);

                Log.WriteLine(LogLevel.Info, $"Loaded font '{fileName}' from file '{filePath}' into Asset Manager.");
            }

            Log.WriteLine(LogLevel.Info, $"Loaded {count} fonts from '{directoryPath}' into Asset Manager.");
        }

        public void Add(string name, string filePath, bool canOverride)
        {
            if (_fonts.ContainsKey(name) && !canOverride)
            {
                Log.WriteLine(LogLevel.Warning, $"Cannot add font '{name}' to Asset Manager as a font with this name already exists.");
                return;
            }

            if (_fonts.TryGetValue(name, out FontSystem value) && canOverride)
            {
                value.Dispose();
                _fonts.Remove(name);
            }

            FontSystem font = new();

            try
            {
                font.AddFont(File.ReadAllBytes(filePath));
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to load font '{name}' from file '{filePath}' into Asset Manager: {ex.Message}");
                return;
            }

            _fonts.Add(name, font);
        }

        public void Remove(string name)
        {
            if (_fonts.TryGetValue(name, out FontSystem value))
            {
                value.Dispose();
                _fonts.Remove(name);
            }
            else
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove font '{name}' from Asset Manager, as no font exists with this name.");
            }
        }

        public FontSystem Get(string name)
        {
            if (_fonts.TryGetValue(name, out FontSystem value)) return value;
            else
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get font '{name}' from Asset Manager, as no font exists with this name.");
                return null;
            }
        }

        public SpriteFontBase Get(string name, float size)
        {
            if (_fonts.TryGetValue(name, out FontSystem value)) return value.GetFont(size);
            else
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get font '{name}' from Asset Manager, as no font exists with this name.");
                return null;
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _fonts.Count; i++)
            {
                _fonts.ElementAt(i).Value.Dispose();
            }

            _fonts.Clear();
            _fonts = null;
        }

        public FontSystem this[string name] => Get(name);
        public SpriteFontBase this[string name, float size] => Get(name, size);
        public bool Contains(string name) => _fonts.ContainsKey(name);
        public bool Contains(FontSystem font) => _fonts.ContainsValue(font);
    }
}
