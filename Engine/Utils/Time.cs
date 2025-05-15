using Microsoft.Xna.Framework;

namespace Colonia.Engine.Utils
{
    internal static class Time
    {
        private static GameTime _gameTime;

        public static float Delta => (float)_gameTime.ElapsedGameTime.TotalSeconds;
        public static float DeltaMS => (float)_gameTime.ElapsedGameTime.TotalMilliseconds;

        public static void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
        }
    }
}
