using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Colonia.Engine
{
    internal class Sprite
    {
        public Texture2D Texture { get; set; }
        public Rectangle[] Frames { get; set; }
        public float Delay { get; set; } = 100.0f;
        public Rectangle Current => Frames[_currentFrame];

        private int _currentFrame = 0;
        private float _elapsed = 0.0f;

        public Sprite(Texture2D texture, int frames)
        {
            Texture = texture;
            Frames = new Rectangle[frames];
        }

        public Sprite(Texture2D texture, Rectangle[] frames, float delay)
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
    }
}
