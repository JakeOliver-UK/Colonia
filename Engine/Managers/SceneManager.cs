using System.Collections.Generic;

namespace Colonia.Engine.Managers
{
    internal static class SceneManager
    {
        public static Scene Current => _current;

        private static Scene _current;
        private static readonly Dictionary<string, Scene> _scenes = [];

        public static void Add(string name, Scene scene)
        {
            if (_scenes.ContainsKey(name)) return;
            _scenes.Add(name, scene);
        }

        public static void Switch(string name)
        {
            if (!_scenes.TryGetValue(name, out var scene)) return;
            _current?.Dispose();
            _current = scene;
            _current.Initialize();
        }

        public static Scene Get(string name) => _scenes.TryGetValue(name, out var scene) ? scene : null;
        public static void Remove(string name) => _scenes.Remove(name);
    }
}
