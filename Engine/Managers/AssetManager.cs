using Colonia.Engine.Utils;
using System;
using System.IO;

namespace Colonia.Engine.Managers
{
    internal class AssetManager : IDisposable
    {
        public static string DirectoryPath => Path.Combine(AppInfo.ApplicationDirectoryPath, "Assets");
        public FontManager Fonts => _fonts;
        public ImageManager Images => _images;
        public SpriteManager Sprites => _sprites;
        public TileManager Tiles => _tiles;

        private FontManager _fonts;
        private ImageManager _images;
        private SpriteManager _sprites;
        private TileManager _tiles;

        public AssetManager()
        {
            _fonts = new();
            _images = new();
            _sprites = new();
            _tiles = new();
        }

        public void Dispose()
        {
            _fonts.Dispose();
            _fonts = null;
            _images.Dispose();
            _images = null;
            _sprites.Dispose();
            _sprites = null;
            _tiles.Dispose();
            _tiles = null;
        }
    }
}
