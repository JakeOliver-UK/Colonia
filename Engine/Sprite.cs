using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Colonia.Engine
{
    internal class Sprite : IDisposable
    {
        public string Name { get; }
        public string[] Images { get; }
        public Rectangle[] Frames { get; }
        public float Delay { get; set; }
        public bool IsLooping { get; set; } = false;
        public bool ShouldAutoPlay { get; set; } = false;

        [JsonIgnore]
        public bool IsPlaying => _isPlaying;

        [JsonIgnore]
        public int CurrentFrame => _currentFrame;

        private bool _isPlaying;
        private int _currentFrame = 0;
        private float _elapsedTime = 0.0f;

        public Sprite(string name, int frames)
        {
            Name = name;
            Images = new string[frames];
            Frames = new Rectangle[frames];
            Delay = 0.0f;
            IsLooping = false;
            ShouldAutoPlay = false;
            _isPlaying = false;
        }

        [JsonConstructor]
        public Sprite(string name, string[] images, Rectangle[] frames, float delay, bool isLooping, bool shouldAutoPlay)
        {
            Name = name;
            Images = images;
            Frames = frames;
            Delay = delay;
            IsLooping = isLooping;
            ShouldAutoPlay = shouldAutoPlay;
            _isPlaying = shouldAutoPlay;
        }

        public void Play()
        {
            if (_isPlaying) return;
            _isPlaying = true;
        }

        public void Pause()
        {
            if (!_isPlaying) return;
            _isPlaying = false;
        }

        public void Stop()
        {
            if (!_isPlaying) return;
            _isPlaying = false;
            _currentFrame = 0;
            _elapsedTime = 0.0f;
        }

        public void Update()
        {
            if (!_isPlaying) return;
            _elapsedTime += Time.DeltaMS;
            if (_elapsedTime >= Delay)
            {
                _currentFrame++;
                _elapsedTime = 0.0f;
                if (_currentFrame >= Frames.Length)
                {
                    if (IsLooping)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            string directoryPath = Path.Combine(AppInfo.ApplicationDirectoryPath, "Assets", "Sprites");
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            string path = Path.Combine(directoryPath, $"{Name}.sprite");
            File.WriteAllText(path, json);
        }

        public void Dispose()
        {
            _isPlaying = false;
            _currentFrame = 0;
            _elapsedTime = 0.0f;
        }
    }
}
