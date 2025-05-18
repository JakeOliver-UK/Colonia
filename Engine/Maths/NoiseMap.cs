using Colonia.Engine.Utils.Extensions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace Colonia.Engine.Maths
{
    internal class NoiseMap
    {
        public string Seed { get => _seed; set { _seed = value; Generate(); } }
        public int Width { get => _width; set { _width = value; Generate(); } }
        public int Height { get => _height; set { _height = value; Generate(); } }
        public FastNoiseLite.NoiseType NoiseType { get => _noiseType; set { _noiseType = value; Generate(); } }
        public float Frequency { get => _frequency; set { _frequency = value; Generate(); } }
        public FastNoiseLite.FractalType FractalType { get => _fractalType; set { _fractalType = value; Generate(); } }
        public int Octaves { get => _octaves; set { _octaves = value; Generate(); } }
        public float Lacunarity { get => _lacunarity; set { _lacunarity = value; Generate(); } }
        public float Gain { get => _gain; set { _gain = value; Generate(); } }
        public bool UseFalloff { get => _useFalloff; set { _useFalloff = value; Generate(); } }
        public float FalloffA { get => _falloffA; set { _falloffA = value; Generate(); } }
        public float FalloffB { get => _falloffB; set { _falloffB = value; Generate(); } }

        private string _seed;
        private int _width;
        private int _height;
        private FastNoiseLite.NoiseType _noiseType;
        private float _frequency;
        private FastNoiseLite.FractalType _fractalType;
        private int _octaves;
        private float _lacunarity;
        private float _gain;
        private bool _useFalloff = false;
        private float _falloffA;
        private float _falloffB;
        private FastNoiseLite _noise;
        private float[,] _noiseMap;
        private float[,] _falloffMap;

        public NoiseMap(string seed, int width, int height)
        {
            _seed = seed;
            _width = width;
            _height = height;
            _noiseType = FastNoiseLite.NoiseType.Perlin;
            _frequency = 0.02f;
            _fractalType = FastNoiseLite.FractalType.FBm;
            _octaves = 5;
            _lacunarity = 2.2f;
            _gain = 0.45f;
            _useFalloff = true;
            _falloffA = 1.5f;
            _falloffB = 1.5f;
            Generate();
        }

        [JsonConstructor]
        public NoiseMap(string seed, int width, int height, FastNoiseLite.NoiseType noiseType, float frequency, FastNoiseLite.FractalType fractalType, int octaves, float lacunarity, float gain, bool useFalloff, float falloffA, float falloffB)
        {
            _seed = seed;
            _width = width;
            _height = height;
            _noiseType = noiseType;
            _frequency = frequency;
            _fractalType = fractalType;
            _octaves = octaves;
            _lacunarity = lacunarity;
            _gain = gain;
            _useFalloff = useFalloff;
            _falloffA = falloffA;
            _falloffB = falloffB;
            Generate();
        }

        public float Get(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height) return 0.0f;
            return _noiseMap[x, y];
        }

        public float this[int x, int y] => Get(x, y);

        private void Generate()
        {
            _noise = new FastNoiseLite(_seed.ToSeed());
            _noise.SetNoiseType(_noiseType);
            _noise.SetFrequency(1.0f);
            _noise.SetFractalType(_fractalType);
            _noise.SetFractalOctaves(_octaves);
            _noise.SetFractalLacunarity(_lacunarity);
            _noise.SetFractalGain(_gain);

            if (UseFalloff && (_falloffMap == null || _falloffMap.GetLength(0) != _width || _falloffMap.GetLength(1) != _height))
            {
                GenerateFalloffMap();
            }

            _noiseMap = new float[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    float raw = _noise.GetNoise(x * _frequency, y * _frequency);
                    float n = (raw + 1.0f) * 0.5f;

                    if (UseFalloff)
                    {
                        float falloffStrength = 0.5f;
                        n = MathHelper.Clamp(n - _falloffMap[x, y] * falloffStrength, 0f, 1f);
                    }

                    _noiseMap[x, y] = n;
                }
            }
        }

        private void GenerateFalloffMap()
        {
            _falloffMap = new float[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    float fx = x / (float)_width * 2.0f - 1.0f;
                    float fy = y / (float)_height * 2.0f - 1.0f;
                    float value = MathF.Sqrt(fx * fx + fy * fy);
                    _falloffMap[x, y] = EvaluateFalloff(value);
                }
            }
        }

        private float EvaluateFalloff(float t)
        {
            float a = _falloffA, b = _falloffB;
            float powA = MathF.Pow(t, a);
            float powB = MathF.Pow(b - b * t, a);
            return powA / (powA + powB);
        }
    }
}
