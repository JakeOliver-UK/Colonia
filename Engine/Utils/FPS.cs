namespace Colonia.Engine.Utils
{
    internal static class FPS
    {
        public static float Current => _current;

        private static int _frameCount;
        private static float _elapsedTime;
        private static float _current;

        public static void Update()
        {
            _elapsedTime += Time.Delta;
            _frameCount++;
            
            if (_elapsedTime >= 1.0f)
            {
                _current = _frameCount / _elapsedTime;
                _elapsedTime = 0.0f;
                _frameCount = 0;
            }
        }
    }
}
