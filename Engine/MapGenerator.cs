using Colonia.Engine.Maths;
using System;

namespace Colonia.Engine
{
    public enum TileType
    {
        Grass,
        Water
    }

    public enum RiverOrientation
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }

    public static class MapGenerator
    {
        /// <summary>
        /// Generates a grass map with one smooth, consistently-thick river
        /// running in a (possibly randomized) cardinal direction.
        /// </summary>
        public static TileType[,] GenerateRiverMap(
            int seed,
            int mapWidth = 200,
            int mapHeight = 150,
            int baseRiverWidth = 4,
            float widthVariation = 2.5f,
            float frequency = 0.005f,
            int smoothRadius = 3,
            int smoothingPasses = 2,
            bool randomizeOrientation = true,
            RiverOrientation? forceOrientation = null
        )
        {
            // 0) Decide orientation
            var rnd = new Random(seed);
            RiverOrientation orient = forceOrientation
                ?? (randomizeOrientation
                    ? (RiverOrientation)rnd.Next(4)
                    : RiverOrientation.LeftToRight);

            // 1) Prepare grid
            var tiles = new TileType[mapWidth, mapHeight];
            for (int x = 0; x < mapWidth; x++)
                for (int y = 0; y < mapHeight; y++)
                    tiles[x, y] = TileType.Grass;

            // 2) Setup noise
            var riverNoise = new FastNoiseLite(seed);
            riverNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            riverNoise.SetFrequency(frequency);
            riverNoise.SetFractalOctaves(3);

            // 3) Sample raw center-line & widths along the “primary” axis
            //    Primary length = N, secondary length = M.
            int N = (orient == RiverOrientation.LeftToRight || orient == RiverOrientation.RightToLeft)
                        ? mapWidth : mapHeight;
            int M = (orient == RiverOrientation.LeftToRight || orient == RiverOrientation.RightToLeft)
                        ? mapHeight : mapWidth;

            float[] center = new float[N];
            int[] width = new int[N];

            for (int i = 0; i < N; i++)
            {
                // sample 1D noise at (i,0)
                float n = riverNoise.GetNoise(i, 0);
                center[i] = (n * 0.5f + 0.5f) * M;

                // width variation
                float w = riverNoise.GetNoise(i, 1000);
                width[i] = baseRiverWidth
                         + (int)Math.Round((w * 0.5f + 0.5f) * widthVariation);
            }

            // 4) Smooth the arrays
            for (int pass = 0; pass < smoothingPasses; pass++)
            {
                var tmpC = new float[N];
                var tmpW = new int[N];

                for (int i = 0; i < N; i++)
                {
                    float sumC = 0;
                    int sumW = 0;
                    int cnt = 0;
                    for (int d = -smoothRadius; d <= smoothRadius; d++)
                    {
                        int ii = i + d;
                        if (ii < 0 || ii >= N) continue;
                        sumC += center[ii];
                        sumW += width[ii];
                        cnt++;
                    }
                    tmpC[i] = sumC / cnt;
                    tmpW[i] = (int)Math.Round((float)sumW / cnt);
                }

                center = tmpC;
                width = tmpW;
            }

            // 5) Carve with circular brush, interpreting (i, center[i]) in 2D
            for (int i = 0; i < N; i++)
            {
                float c = center[i];
                int r = width[i] / 2;

                // bounds for the carve window
                int iMin = Math.Max(0, i - r);
                int iMax = Math.Min(N - 1, i + r);
                int cMin = Math.Max(0, (int)Math.Floor(c - r));
                int cMax = Math.Min(M - 1, (int)Math.Ceiling(c + r));

                for (int ii = iMin; ii <= iMax; ii++)
                    for (int cc = cMin; cc <= cMax; cc++)
                    {
                        float dx = ii - i;
                        float dy = cc - c;
                        if (dx * dx + dy * dy <= r * r)
                        {
                            int x, y;
                            // map (ii, cc) → (x,y) depending on orientation
                            switch (orient)
                            {
                                case RiverOrientation.LeftToRight:
                                    x = ii; y = cc;
                                    break;
                                case RiverOrientation.RightToLeft:
                                    x = mapWidth - 1 - ii; y = cc;
                                    break;
                                case RiverOrientation.TopToBottom:
                                    x = cc; y = ii;
                                    break;
                                case RiverOrientation.BottomToTop:
                                    x = cc; y = mapHeight - 1 - ii;
                                    break;
                                default:
                                    x = ii; y = cc;
                                    break;
                            }
                            tiles[x, y] = TileType.Water;
                        }
                    }
            }

            return tiles;
        }
    }
}
