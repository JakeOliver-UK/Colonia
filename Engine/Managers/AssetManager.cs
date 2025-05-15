using Colonia.Engine.Utils;
using System;
using System.IO;

namespace Colonia.Engine.Managers
{
    internal class AssetManager : IDisposable
    {
        public static string DirectoryPath => Path.Combine(AppInfo.ApplicationDirectoryPath, "Assets");
        public ImageManager Images => _images;

        private ImageManager _images;

        public AssetManager()
        {
            _images = new();
        }

        public void Dispose()
        {
            _images.Dispose();
            _images = null;
        }
    }
}
