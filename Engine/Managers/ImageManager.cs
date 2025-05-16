using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Colonia.Engine.Managers
{
    internal class ImageManager : IDisposable
    {
        public static string DirectoryPath => Path.Combine(AssetManager.DirectoryPath, "Images");
        public Texture2D Pixel => _pixel;

        private Dictionary<string, Texture2D> _images;
        private readonly Texture2D _pixel;

        public ImageManager()
        {
            _images = [];
            Load(DirectoryPath, false);
            _pixel = new(App.Instance.GraphicsDevice, 1, 1);
            _pixel.SetData([Color.White]);
        }

        public void Load(string directoryPath, bool canOverride)
        {
            Log.WriteLine(LogLevel.Info, $"Loading images from '{directoryPath}' into Asset Manager...");

            int count = 0;

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string[] extensions = [".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tif", ".tiff", ".dds"];
            
            string[] files = [.. Directory
                .EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))];

            if (files.Length == 0)
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to load images from '{directoryPath}' into Asset Manager as no image files were found.");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                Texture2D image;

                try
                {
                    using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    image = Texture2D.FromStream(App.Instance.GraphicsDevice, stream);
                }
                catch (Exception ex)
                {
                    Log.WriteLine(LogLevel.Error, $"Unable to load image '{fileName}' from file '{filePath}' into Asset Manager: {ex.Message}");
                    continue;
                }

                Add(fileName, image, canOverride);

                Log.WriteLine(LogLevel.Info, $"Loaded image '{fileName}' from file '{filePath}' into Asset Manager.");

                count++;
            }

            Log.WriteLine(LogLevel.Info, $"Loaded {count} images from '{directoryPath}' into Asset Manager.");
        }

        public void Add(string name, Texture2D image, bool canOverride)
        {
            if (_images.ContainsKey(name) && !canOverride)
            {
                Log.WriteLine(LogLevel.Warning, $"Cannot add image '{name}' to Asset Manager as an image with this name already exists.");
                return;
            }

            if (_images.TryGetValue(name, out Texture2D value) && canOverride)
            {
                value.Dispose();
                _images.Remove(name);
            }
            
            _images.Add(name, image);
        }

        public void Remove(string name)
        {
            if (_images.TryGetValue(name, out Texture2D value))
            {
                value.Dispose();
                _images.Remove(name);
            }
            else
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to remove image '{name}' from Asset Manager as it does not exist.");
            }
        }

        public Texture2D Get(string name)
        {   
            if (_images.TryGetValue(name, out Texture2D value)) return value;

            Log.WriteLine(LogLevel.Warning, $"Unable to get image '{name}' from Asset Manager as it does not exist.");
            return null;
        }

        public void Dispose()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                KeyValuePair<string, Texture2D> image = _images.ElementAt(i);
                image.Value.Dispose();
            }

            _images.Clear();
            _images = null;
        }

        public Texture2D this[string name] => Get(name);
        public bool Contains(string name) => _images.ContainsKey(name);
        public bool Contains(Texture2D image) => _images.ContainsValue(image);
    }
}
