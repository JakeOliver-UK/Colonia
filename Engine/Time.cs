using Microsoft.Xna.Framework;

namespace Colonia.Engine
{
    internal static class Time
    {
        public static float Delta => _delta;
        public static float DeltaMS => _deltaMS;
        public static float Total => _total;
        public static float TotalMS => _totalMS;

        private static float _delta;
        private static float _deltaMS;
        private static float _total;
        private static float _totalMS;

        public static void Update(GameTime gameTime)
        {
            _delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _deltaMS = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _total = (float)gameTime.TotalGameTime.TotalSeconds;
            _totalMS = (float)gameTime.TotalGameTime.TotalMilliseconds;
        }
    }
}
