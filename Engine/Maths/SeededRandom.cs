using Microsoft.Xna.Framework;
using System;
using System.Buffers.Binary;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Colonia.Engine.Maths
{
    internal sealed class SeededRandom
    {
        public string Seed { get => _seed; set { value ??= DateTime.Now.Ticks.ToString(); _seed = value; HashSeed(value); } }

        private string _seed;
        private ulong _s0, _s1, _s2, _s3;

        public SeededRandom()
        {
            _seed = DateTime.Now.Ticks.ToString();

            HashSeed(_seed);
        }

        public SeededRandom(string seed)
        {
            seed ??= DateTime.Now.Ticks.ToString();
            _seed = seed;

            HashSeed(seed);
        }

        private void HashSeed(string seed)
        {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(seed));

            _s0 = BinaryPrimitives.ReadUInt64LittleEndian(hash.AsSpan(0, 8));
            _s1 = BinaryPrimitives.ReadUInt64LittleEndian(hash.AsSpan(8, 8));
            _s2 = BinaryPrimitives.ReadUInt64LittleEndian(hash.AsSpan(16, 8));
            _s3 = BinaryPrimitives.ReadUInt64LittleEndian(hash.AsSpan(24, 8));

            if ((_s0 | _s1 | _s2 | _s3) == 0) _s0 = 1;
        }

        public static SeededRandom Fork(string seed, string stream) => new(seed + "\0" + stream);

        public ulong NextULong()
        {
            ulong result = BitOperations.RotateLeft(_s1 * 5, 7) * 9;
            ulong t = _s1 << 17;

            _s2 ^= _s0;
            _s3 ^= _s1;
            _s1 ^= _s2;
            _s0 ^= _s3;
            _s2 ^= t;
            _s3 = BitOperations.RotateLeft(_s3, 45);

            return result;
        }

        public long NextLong() => unchecked((long)NextULong());
        public uint NextUInt() => (uint)(NextULong() >> 32);
        public int NextInt() => unchecked((int)(NextULong() >> 32));
        public ushort NextUShort() => (ushort)(NextULong() >> 48);
        public short NextShort() => unchecked((short)(NextULong() >> 48));
        public byte NextByte() => (byte)(NextULong() >> 56);
        public bool NextBool() => (NextULong() >> 63) != 0;

        public ulong NextULong(ulong maxExclusive)
        {
            ArgumentOutOfRangeException.ThrowIfZero(maxExclusive);

            ulong hi = Math.BigMul(NextULong(), maxExclusive, out ulong lo);
            if (lo < maxExclusive)
            {
                ulong threshold = unchecked(0UL - maxExclusive) % maxExclusive;
                while (lo < threshold) hi = Math.BigMul(NextULong(), maxExclusive, out lo);
            }
            return hi;
        }

        public long NextLong(long min, long maxExclusive)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, min);
            ulong range = unchecked((ulong)(maxExclusive - min));
            return unchecked(min + (long)NextULong(range));
        }

        public int NextInt(int min, int maxExclusive)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, min);
            ulong range = (ulong)((long)maxExclusive - min);
            return (int)(min + (long)NextULong(range));
        }

        public uint NextUInt(uint min, uint maxExclusive)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, min);
            return min + (uint)NextULong(maxExclusive - min);
        }

        public double NextDouble() => (NextULong() >> 11) * (1.0 / (1UL << 53));

        public double NextDouble(double min, double maxExclusive)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, min);
            return min + (maxExclusive - min) * NextDouble();
        }

        public float NextFloat() => (NextULong() >> 40) * (1.0f / (1 << 24));

        public float NextFloat(float min, float maxExclusive)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, min);
            return min + (maxExclusive - min) * NextFloat();
        }

        public Vector2 NextVector2(float min, float maxExclusive)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, min);
            return new Vector2(NextFloat(min, maxExclusive), NextFloat(min, maxExclusive));
        }

        public Vector2 NextVector2(float minX, float maxExclusiveX, float minY, float maxExclusiveY)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusiveX, minX);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusiveY, minY);
            return new Vector2(NextFloat(minX, maxExclusiveX), NextFloat(minY, maxExclusiveY));
        }

        public Vector2 NextVector2(in Rectangle rectangle) => new(NextInt(rectangle.X, rectangle.X + rectangle.Width), NextInt(rectangle.Y, rectangle.Y + rectangle.Height));
        public Color NextColor() => new(NextInt(0, 256), NextInt(0, 256), NextInt(0, 256));
    }
}
