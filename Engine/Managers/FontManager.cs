using Colonia.Engine.Utils;
using FontStashSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Colonia.Engine.Managers
{
    internal class FontManager
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
                
            }
        }
    }
}
