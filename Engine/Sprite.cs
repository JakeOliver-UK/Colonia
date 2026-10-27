using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Colonia.Engine
{
    internal class Sprite
    {
        public string Texture { get; set; }
        public Rectangle[] Frames { get; set; }
        public float Delay { get; set; } = 150.0f;
        public Rectangle Current => Frames[_currentFrame];

        private int _currentFrame = 0;
        private float _elapsed = 0.0f;
        private static readonly JsonSerializerSettings _jsonSettings = new() { Formatting = Formatting.Indented };

        public Sprite(string texture, int frames)
        {
            Texture = texture;
            Frames = new Rectangle[frames];
        }

        [JsonConstructor]
        public Sprite(string texture, Rectangle[] frames, float delay)
        {
            Texture = texture;
            Frames = frames;
            Delay = delay;
        }

        public void Update()
        {
            if (Frames.Length <= 1) return;
            
            _elapsed += Time.DeltaMS;
            if (_elapsed >= Delay)
            {
                _currentFrame = (_currentFrame + 1) % Frames.Length;
                _elapsed = 0.0f;
            }
        }

        public string ToJson() => JsonConvert.SerializeObject(this, _jsonSettings);
        public static Sprite FromJson(string json) => JsonConvert.DeserializeObject<Sprite>(json, _jsonSettings);
    }
}
