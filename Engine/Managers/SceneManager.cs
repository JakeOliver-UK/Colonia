using Colonia.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Colonia.Engine.Managers
{
    internal class SceneManager : IDisposable
    {
        public Scene Current => _current;

        private Scene _current;
        private Dictionary<string, Scene> _scenes;

        public SceneManager()
        {
            _scenes = [];
        }

        public void Add(string name, Scene scene)
        {
            if (_scenes.ContainsKey(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add scene '{name}' to Scene Manager, as a scene with this name already exists.");
                return;
            }

            if (scene == null)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add scene '{name}' to Scene Manager, as the scene is null.");
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add scene '{name}' to Scene Manager, as the name is null or empty.");
                return;
            }

            if (_scenes.ContainsValue(scene))
            {
                string foundKey = _scenes.FirstOrDefault(pair => ReferenceEquals(pair.Value, scene)).Key;
                Log.WriteLine(LogLevel.Error, $"Unable to add scene '{name}' to Scene Manager, as this scene already exists under the name '{foundKey}'.");
                return;
            }

            _scenes.Add(name, scene);
        }

        public void Remove(string name)
        {
            if (_scenes.TryGetValue(name, out Scene value))
            {
                value.Dispose();
                _scenes.Remove(name);
            }
            else
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove scene '{name}' from Scene Manager, as no scene exists with this name.");
            }
        }

        public void Display(string scene)
        {
            if (_scenes.TryGetValue(scene, out Scene value))
            {
                _current?.Dispose();
                _current = value;
                _current.Initialize();
            }
            else
            {
                Log.WriteLine(LogLevel.Error, $"Unable to display scene '{scene}' from Scene Manager, as no scene exists with this name.");
            }
        }

        public Scene Get(string name)
        {
            if (_scenes.TryGetValue(name, out Scene value)) return value;
            else
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get scene '{name}' from Scene Manager, as no scene exists with this name.");
                return null;
            }
        }

        public void Dispose()
        {
            _current?.Dispose();
            _current = null;

            for (int i = 0; i < _scenes.Count; i++)
            {
                KeyValuePair<string, Scene> scene = _scenes.ElementAt(i);
                scene.Value.Dispose();
            }

            _scenes = null;
        }

        public Scene this[string name] => Get(name);
        public bool Contains(string name) => _scenes.ContainsKey(name);
        public bool Contains(Scene scene) => _scenes.ContainsValue(scene);
    }
}
