﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace CRT.Engine
{
    public class TileMapManager : ChexelProvider
    {
        public bool isActive = false;
        public vector2 offset = new vector2();
        public Dictionary<vector2, ChexelEntity> entities;
        public List<FrameEntity> frameEntities;
        private bool isStarted = false;
        Frame tileMap;

        public TileMapManager()
        {
            entities = new Dictionary<vector2, ChexelEntity>(new vector2HashCode());
            frameEntities = new List<FrameEntity>();
        }

        public void centerOnCell(vector2 pos)
        {
            offset.x = pos.x - (Program.frameManager.width / 2);
            offset.y = pos.y - (Program.frameManager.height / 2);

            if (offset.x > tileMap.width - 1 - (Program.frameManager.width))
            {
                offset.x = tileMap.width - 1 - (Program.frameManager.width);
            }

            if (offset.x < 0)
            {
                offset.x = 0;
            }

            if (offset.y > tileMap.height - 1 - (Program.frameManager.height))
            {
                offset.y = tileMap.height - 1 - (Program.frameManager.height);
            }

            if (offset.y < 0)
            {
                offset.y = 0;
            }
        }

        public bool active()
        {
            return isActive;
        }

        public (bool, FrameEntity) frameCollide(vector2 pos)
        {
            for(int i = 0; i < frameEntities.Count; i++)
            {
                if(frameEntities[i].isActive && frameEntities[i].frame.hasChexel(pos.x, pos.y))
                {
                    return (frameEntities[i].frame.getChexel(pos.x, pos.y).t != ' ', frameEntities[i]);
                }
            }

            return (false, null);
        }

        public (bool, ChexelEntity) chexelCollide(vector2 pos)
        {
            if(entities.ContainsKey(pos))
            {
                return (true, entities[pos]);
            }

            return (false, null);
        }

        public bool checkCollide(vector2 toMove, vector2 newPos)
        {
            return entities.ContainsKey(newPos) && toMove.Equals(newPos) && tileMap.hasChexel(newPos.x, newPos.y) && isCollideable(tileMap.getChexel(newPos.x, newPos.y).b) && frameCollide(newPos).Item1;
        }

        public bool moveEntity(ChexelEntity toMove, vector2 newPos)
        {
            if(checkCollide(toMove.pos, newPos))
            {
                return false;
            }
            else
            {
                removeEntity(toMove);
                toMove.pos = newPos;
                return addEntity(toMove);
            }
        }

        private bool isCollideable(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return false;
                case ConsoleColor.Blue:
                    return false;
                case ConsoleColor.Cyan:
                    return false;
                case ConsoleColor.DarkBlue:
                    return true;
                case ConsoleColor.DarkCyan:
                    return false;
                case ConsoleColor.DarkGray:
                    return false;
                case ConsoleColor.DarkGreen:
                    return false;
                case ConsoleColor.DarkMagenta:
                    return false;
                case ConsoleColor.DarkRed:
                    return false;
                case ConsoleColor.DarkYellow:
                    return false;
                case ConsoleColor.Gray:
                    return false;
                case ConsoleColor.Green:
                    return false;
                case ConsoleColor.Magenta:
                    return false;
                case ConsoleColor.Red:
                    return false;
                case ConsoleColor.White:
                    return false;
                case ConsoleColor.Yellow:
                    return false;
            }
            return false;
        }

        public ChexelEntity getEntityAt(vector2 pos)
        {
            if (entities.ContainsKey(pos))
            {
                return entities[pos];
            }

            return null;
        }

        public bool addFrameEntity(FrameEntity toAdd)
        {
            if (frameEntities.Contains(toAdd))
            {
                return false;
            }
            else
            {
                frameEntities.Add(toAdd);
                if (isStarted)
                {
                    toAdd.start();
                }
                return true;
            }
        }

        public void removeFrameEntity(FrameEntity toRemove)
        {
            if (frameEntities.Contains(toRemove))
            {
                frameEntities.Remove(toRemove);
                toRemove.stop();
            }
        }

        public bool addEntity(ChexelEntity toAdd)
        {
            if (entities.ContainsKey(toAdd.pos))
            {
                return false;
            }
            else
            {
                entities.Add(toAdd.pos, toAdd);
                if (isStarted)
                {
                    toAdd.start();
                }
                return true;
            }
        }

        public void removeEntity(ChexelEntity toRemove)
        {
            if (entities.ContainsKey(toRemove.pos))
            {
                entities.Remove(toRemove.pos);
                toRemove.stop();
            }
        }

        public void start()
        {
            List<ChexelEntity> entityCollection = new List<ChexelEntity>(entities.Values);
            foreach (ChexelEntity v in entityCollection)
            {
                v.start();
            }

            List<FrameEntity> frameEntityCollection = new List<FrameEntity>(frameEntities);
            foreach (FrameEntity v in frameEntityCollection)
            {
                v.start();
            }
            isStarted = true;
        }

        public void Update()
        {
            if (isActive)
            {
                List<ChexelEntity> entityCollection = new List<ChexelEntity>(entities.Values);
                foreach (ChexelEntity v in entityCollection)
                {
                    v.updateBegin();
                }

                List<FrameEntity> frameEntityCollection = new List<FrameEntity>(frameEntities);
                foreach (FrameEntity v in frameEntityCollection)
                {
                    v.updateBegin();
                }

                foreach (ChexelEntity v in entityCollection)
                {
                    v.update();
                }

                foreach (FrameEntity v in frameEntityCollection)
                {
                    v.update();
                }

                foreach (ChexelEntity v in entityCollection)
                {
                    v.updateEnd();
                }

                foreach (FrameEntity v in frameEntityCollection)
                {
                    v.updateEnd();
                }
            }
        }

        public vector2 worldToScreenPos(vector2 worldPos)
        {
            return new vector2(worldPos.x - offset.x, worldPos.y - offset.y);
        }

        public vector2 ScreenToWorldPos(vector2 screenPos)
        {
            return new vector2(screenPos.x + offset.x, screenPos.y + offset.y);
        }

        public Chexel getChexel(int x, int y)
        {
            vector2 worldPos = ScreenToWorldPos(new vector2(x, y));

            Chexel chexel = tileMap.getChexel(worldPos.x, worldPos.y);
            if (entities.ContainsKey(worldPos))
            {
                chexel.f = entities[worldPos].f;
                chexel.b = ConsoleColor.Black;
                chexel.t = entities[worldPos].t;
            }

            List<FrameEntity> frameEntityCollection = new List<FrameEntity>(frameEntities);
            foreach (FrameEntity v in frameEntityCollection)
            {
                Chexel temp = v.frame.getChexel(worldPos.x, worldPos.y);
                if(temp.t != ' ')
                {
                    chexel.t = temp.t;
                    chexel.f = temp.f;
                    chexel.b = temp.b;
                }
            }

            return chexel;
        }

        public bool hasChexel(int x, int y)
        {
            vector2 worldPos = ScreenToWorldPos(new vector2(x, y));

            List<FrameEntity> frameEntityCollection = new List<FrameEntity>(frameEntities);
            foreach (FrameEntity v in frameEntityCollection)
            {
                v.frame.hasChexel(x, y);
            }

            return tileMap.hasChexel(worldPos.x, worldPos.y) || entities.ContainsKey(worldPos);
        }

        public void generatePlanet(int number, MapType mapType)
        {
            if (number < 0)
            {
                tileMap = TileMapGenerator.FromBitmap("./Assets/firstMap.bmp", 1);
            }
            else
            {
                tileMap = TileMapGenerator.GeneratePlanet(number, 200, 200, mapType);
            }

            start();
        }
    }

    public static class TileMapGenerator
    {
        public static Frame FromBitmap(string path, int pallet)
        {
            if (File.Exists(path))
            {
                Bitmap b = new Bitmap(path);
                Frame toReturn = new Frame(b.Width, b.Height, 0, 0);

                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        toReturn.setChexel(i, j, new Chexel(ConsoleColor.White, Utils.FromColor(new Vec3(b.GetPixel(i, j)),pallet), ' '));
                    }
                }

                return toReturn;
            }

            return default;
        }

        public static Frame GeneratePlanet(int seed, int width, int height, MapType mapType)
        {
            Simplex.Noise.Seed = seed;

            Frame toReturn = new Frame(width, height, 0, 0);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    toReturn.setChexel(i, j, new Chexel(ConsoleColor.White, getBiomeColor(i, j, mapType), ' '));
                }
            }

            return toReturn;

        }

        static double min = 10000;
        static double max = -10000;

        private static ConsoleColor getBiomeColor(int x, int y, MapType mapType)
        {
            double val = Simplex.Noise.CalcPixel2D(x, y, mapType == MapType.Caves ? 0.025 : 0.005) / 255.0;

            if (min > val)
            {
                min = val;
            }

            if (max < val)
            {
                max = val;
            }

            switch (mapType)
            {
                case MapType.Caves:
                    if (val < 0.45)
                    {
                        return ConsoleColor.Black;
                    }
                    else
                    {
                        return ConsoleColor.DarkGray;
                    }
                case MapType.Overworld:
                    if (val < 0.2)
                    {
                        return ConsoleColor.DarkBlue;
                    }
                    else if (val < 0.3)
                    {
                        return ConsoleColor.DarkYellow;
                    }
                    else if (val < 0.9)
                    {
                        return ConsoleColor.DarkGreen;
                    }
                    else
                    {
                        return ConsoleColor.DarkGray;
                    }
            }

            return ConsoleColor.Black;
        }
    }

    public enum MapType
    {
        Caves = 0,
        Overworld = 1,
    }
}

// SimplexNoise for C#
// Author: Benjamin Ward
// Originally authored by Heikki Törmälä

namespace Simplex
{
    /// <summary>
    /// Implementation of the Perlin simplex noise, an improved Perlin noise algorithm.
    /// Based loosely on SimplexNoise1234 by Stefan Gustavson <http://staffwww.itn.liu.se/~stegu/aqsis/aqsis-newnoise/>
    /// </summary>
    public class Noise
    {
        public static double[] Calc1D(int width, double scale)
        {
            double[] values = new double[width];
            for (int i = 0; i < width; i++)
                values[i] = Generate(i * scale) * 128 + 128;
            return values;
        }

        public static double[,] Calc2D(int width, int height, double scale)
        {
            double[,] values = new double[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    values[i, j] = Generate(i * scale, j * scale) * 128 + 128;
            return values;
        }

        public static double[,,] Calc3D(int width, int height, int length, double scale)
        {
            double[,,] values = new double[width, height, length];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    for (int k = 0; k < length; k++)
                        values[i, j, k] = Generate(i * scale, j * scale, k * scale) * 128 + 128;
            return values;
        }

        public static double CalcPixel1D(int x, double scale)
        {
            return Generate(x * scale) * 128 + 128;
        }

        public static double CalcPixel2D(int x, int y, double scale)
        {
            return Generate(x * scale, y * scale) * 128 + 128;
        }

        public static double CalcPixel3D(int x, int y, int z, double scale)
        {
            return Generate(x * scale, y * scale, z * scale) * 128 + 128;
        }

        static Noise()
        {
            perm = new byte[permOriginal.Length];
            Simplex.Noise.permOriginal.CopyTo(perm, 0);
        }

        public static int Seed
        {
            get { return seed; }
            set
            {
                if (value == 0)
                {
                    perm = new byte[permOriginal.Length];
                    Simplex.Noise.permOriginal.CopyTo(perm, 0);
                }
                else
                {
                    perm = new byte[512];
                    Random random = new Random(value);
                    random.NextBytes(perm);
                }
            }
        }
        private static int seed = 0;

        /// <summary>
        /// 1D simplex noise
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        internal static double Generate(double x)
        {
            int i0 = FastFloor(x);
            int i1 = i0 + 1;
            double x0 = x - i0;
            double x1 = x0 - 1.0f;

            double n0, n1;

            double t0 = 1.0f - x0 * x0;
            t0 *= t0;
            n0 = t0 * t0 * grad(perm[i0 & 0xff], x0);

            double t1 = 1.0f - x1 * x1;
            t1 *= t1;
            n1 = t1 * t1 * grad(perm[i1 & 0xff], x1);
            // The maximum value of this noise is 8*(3/4)^4 = 2.53125
            // A factor of 0.395 scales to fit exactly within [-1,1]
            return 0.395f * (n0 + n1);
        }

        /// <summary>
        /// 2D simplex noise
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal static double Generate(double x, double y)
        {
            const double F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
            const double G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

            double n0, n1, n2; // Noise contributions from the three corners

            // Skew the input space to determine which simplex cell we're in
            double s = (x + y) * F2; // Hairy factor for 2D
            double xs = x + s;
            double ys = y + s;
            int i = FastFloor(xs);
            int j = FastFloor(ys);

            double t = (double)(i + j) * G2;
            double X0 = i - t; // Unskew the cell origin back to (x,y) space
            double Y0 = j - t;
            double x0 = x - X0; // The x,y distances from the cell origin
            double y0 = y - Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0) { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6

            double x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            double y1 = y0 - j1 + G2;
            double x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            double y2 = y0 - 1.0f + 2.0f * G2;

            // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
            int ii = Mod(i, 256);
            int jj = Mod(j, 256);

            // Calculate the contribution from the three corners
            double t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 < 0.0f) n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * grad(perm[ii + perm[jj]], x0, y0);
            }

            double t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 < 0.0f) n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * grad(perm[ii + i1 + perm[jj + j1]], x1, y1);
            }

            double t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 < 0.0f) n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * grad(perm[ii + 1 + perm[jj + 1]], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 40.0f * (n0 + n1 + n2); // TODO: The scale factor is preliminary!
        }


        internal static double Generate(double x, double y, double z)
        {
            // Simple skewing factors for the 3D case
            const double F3 = 0.333333333f;
            const double G3 = 0.166666667f;

            double n0, n1, n2, n3; // Noise contributions from the four corners

            // Skew the input space to determine which simplex cell we're in
            double s = (x + y + z) * F3; // Very nice and simple skew factor for 3D
            double xs = x + s;
            double ys = y + s;
            double zs = z + s;
            int i = FastFloor(xs);
            int j = FastFloor(ys);
            int k = FastFloor(zs);

            double t = (double)(i + j + k) * G3;
            double X0 = i - t; // Unskew the cell origin back to (x,y,z) space
            double Y0 = j - t;
            double Z0 = k - t;
            double x0 = x - X0; // The x,y,z distances from the cell origin
            double y0 = y - Y0;
            double z0 = z - Z0;

            // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
            // Determine which simplex we are in.
            int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
            int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

            /* This code would benefit from a backport from the GLSL version! */
            if (x0 >= y0)
            {
                if (y0 >= z0)
                { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
                else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
                else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
            }
            else
            { // x0<y0
                if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
                else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
                else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
            }

            // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
            // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
            // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
            // c = 1/6.

            double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;
            double x2 = x0 - i2 + 2.0f * G3; // Offsets for third corner in (x,y,z) coords
            double y2 = y0 - j2 + 2.0f * G3;
            double z2 = z0 - k2 + 2.0f * G3;
            double x3 = x0 - 1.0f + 3.0f * G3; // Offsets for last corner in (x,y,z) coords
            double y3 = y0 - 1.0f + 3.0f * G3;
            double z3 = z0 - 1.0f + 3.0f * G3;

            // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
            int ii = Mod(i, 256);
            int jj = Mod(j, 256);
            int kk = Mod(k, 256);

            // Calculate the contribution from the four corners
            double t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
            if (t0 < 0.0f) n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * grad(perm[ii + perm[jj + perm[kk]]], x0, y0, z0);
            }

            double t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
            if (t1 < 0.0f) n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * grad(perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]], x1, y1, z1);
            }

            double t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
            if (t2 < 0.0f) n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * grad(perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]], x2, y2, z2);
            }

            double t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
            if (t3 < 0.0f) n3 = 0.0f;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * grad(perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]], x3, y3, z3);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to stay just inside [-1,1]
            return 32.0f * (n0 + n1 + n2 + n3); // TODO: The scale factor is preliminary!
        }

        private static byte[] perm;

        private static readonly byte[] permOriginal = new byte[]
        {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        private static int FastFloor(double x)
        {
            return (x > 0) ? ((int)x) : (((int)x) - 1);
        }

        private static int Mod(int x, int m)
        {
            int a = x % m;
            return a < 0 ? a + m : a;
        }

        private static double grad(int hash, double x)
        {
            int h = hash & 15;
            double grad = 1.0f + (h & 7);   // Gradient value 1.0, 2.0, ..., 8.0
            if ((h & 8) != 0) grad = -grad;         // Set a random sign for the gradient
            return (grad * x);           // Multiply the gradient with the distance
        }

        private static double grad(int hash, double x, double y)
        {
            int h = hash & 7;      // Convert low 3 bits of hash code
            double u = h < 4 ? x : y;  // into 8 simple gradient directions,
            double v = h < 4 ? y : x;  // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
        }

        private static double grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;     // Convert low 4 bits of hash code into 12 simple
            double u = h < 8 ? x : y; // gradient directions, and compute dot product.
            double v = h < 4 ? y : h == 12 || h == 14 ? x : z; // Fix repeats at h = 12 to 15
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v);
        }

        private static double grad(int hash, double x, double y, double z, double t)
        {
            int h = hash & 31;      // Convert low 5 bits of hash code into 32 simple
            double u = h < 24 ? x : y; // gradient directions, and compute dot product.
            double v = h < 16 ? y : z;
            double w = h < 8 ? z : t;
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v) + ((h & 4) != 0 ? -w : w);
        }
    }
}