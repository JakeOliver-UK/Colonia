using Colonia.Engine.Utils;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Colonia.Engine.Managers
{
    internal class SettingsManager : IDisposable
    {
        public string FilePath => _filePath;
        public Settings Settings => _settings;

        private Settings _settings;
        private readonly string _filePath = Path.Combine(AppInfo.LocalAppDataDirectoryPath, "settings.json");
        private readonly JsonSerializerSettings _jsonSettings = new() { Formatting = Formatting.Indented };

        public SettingsManager()
        {
            Load();
        }

        public void New()
        {
            Log.WriteLine(LogLevel.Info, $"Creating new settings file at '{_filePath}'.");
            _settings = new();
            Save();
            Apply();
        }

        public void Load()
        {
            if (!Directory.Exists(AppInfo.LocalAppDataDirectoryPath)) Directory.CreateDirectory(AppInfo.LocalAppDataDirectoryPath);
            
            if (!File.Exists(_filePath))
            {
                Log.WriteLine(LogLevel.Warning, $"Settings file not found at '{_filePath}'.");
                New();
                return;
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                _settings = JsonConvert.DeserializeObject<Settings>(json, _jsonSettings);
                Apply();
            }
            catch (JsonException ex)
            {
                Log.WriteLine(LogLevel.Error, $"Failed to load settings from '{_filePath}': {ex.Message}");
                New();
            }
        }

        public void Save()
        {
            if (!Directory.Exists(AppInfo.LocalAppDataDirectoryPath)) Directory.CreateDirectory(AppInfo.LocalAppDataDirectoryPath);

            try
            {
                string json = JsonConvert.SerializeObject(_settings, _jsonSettings);
                File.WriteAllText(_filePath, json);
            }
            catch (IOException ex)
            {
                Log.WriteLine(LogLevel.Error, $"Failed to save settings to '{_filePath}': {ex.Message}");
            }
        }

        public void Apply()
        {
            App.Instance.GraphicsDeviceManager.PreferredBackBufferWidth = _settings.ResolutionWidth;
            App.Instance.GraphicsDeviceManager.PreferredBackBufferHeight = _settings.ResolutionHeight;
            App.Instance.GraphicsDeviceManager.IsFullScreen = _settings.Fullscreen;
            App.Instance.GraphicsDeviceManager.HardwareModeSwitch = !_settings.Borderless;
            App.Instance.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = _settings.VSync;
            App.Instance.IsFixedTimeStep = _settings.FixedFrameRate;
            App.Instance.TargetElapsedTime = TimeSpan.FromSeconds(1.0d / (double)_settings.TargetFrameRate);
            App.Instance.GraphicsDeviceManager.ApplyChanges();
        }

        public void Dispose()
        {
            _settings = null;
        }
    }
}
