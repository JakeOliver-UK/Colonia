using Colonia.Engine.Utils.Extensions;
using System;
using System.Collections.Generic;

namespace Colonia.Engine.Maths
{
    internal class PRNG
    {
        public string Seed { get => _seed; set { _seed = value; _random = new(value.ToSeed()); } }

        private string _seed;
        private Random _random;

        public PRNG()
        {
            _seed = DateTime.Now.Ticks.ToString();
            _random = new(_seed.ToSeed());
        }

        public PRNG(string seed)
        {
            _seed = seed;
            _random = new(seed.ToSeed());
        }

        public int Int() => _random.Next();
        public int Int(int max) => _random.Next(max);
        public int Int(int min, int max) => _random.Next(min, max);
        public double Double() => _random.NextDouble();
        public double Double(double max) => _random.NextDouble() * max;
        public double Double(double min, double max) => _random.NextDouble() * (max - min) + min;
        public float Float() => (float)_random.NextDouble();
        public float Float(float max) => (float)_random.NextDouble() * max;
        public float Float(float min, float max) => (float)_random.NextDouble() * (max - min) + min;

        public T GetFromList<T>(List<T> list)
        {
            if (list.Count == 0) return default;
            else return list[_random.Next(0, list.Count)];
        }

        public T GetFromArray<T>(T[] list)
        {
            if (list.Length == 0) return default;
            else return list[_random.Next(0, list.Length)];
        }
    }
}
