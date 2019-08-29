using CRT.IOW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Threading;

namespace CRT
{
    class Utils
    {
        public static ConsoleColor FromColor(Vec3 c)
        {
            double max = Math.Max(c.x, Math.Max(c.y, c.z));
            if (max > 1)
            {
                c = c * (1.0 / max);
            }

            int r = (int)(Math.Max(0.0, Math.Min(1, c.r())) * 255.0);
            int g = (int)(Math.Max(0.0, Math.Min(1, c.g())) * 255.0);
            int b = (int)(Math.Max(0.0, Math.Min(1, c.b())) * 255.0);

            int index = (r > 128 | g > 128 | b > 128) ? 8 : 0; // Bright bit

            index |= (r > 64) ? 4 : 0; // Red bit
            index |= (g > 64) ? 2 : 0; // Green bit
            index |= (b > 64) ? 1 : 0; // Blue bit

            return (ConsoleColor)index;
        }

        public static Vec3 greyScale(Vec3 v)
        {
            double avg = (v.x + v.y + v.z) / 3.0;

            return new Vec3(avg, avg, avg);
        }

        public static Color toRGB(Vec3 c)
        {
            double max = Math.Max(c.x, Math.Max(c.y, c.z));
            if (max > 1)
            {
                c = c * (1.0 / max);
            }

            int r = (int)(Math.Max(0.0, Math.Min(1, c.r())) * 255.0);
            int g = (int)(Math.Max(0.0, Math.Min(1, c.g())) * 255.0);
            int b = (int)(Math.Max(0.0, Math.Min(1, c.b())) * 255.0);

            return Color.FromArgb(r, g, b);
        }

        public static Vec3 randomColor()
        {
            int val = rng.Next(0, 14);

            switch (val)
            {
                case 0:
                    return new Vec3(0, 0, 0);
                case 1:
                    return new Vec3(0, 0, 1);
                case 2:
                    return new Vec3(0, 1, 0);
                case 3:
                    return new Vec3(0, 1, 1);
                case 4:
                    return new Vec3(1, 0, 0);
                case 5:
                    return new Vec3(1, 0, 1);
                case 6:
                    return new Vec3(1, 1, 0);
                case 7:
                    return new Vec3(1, 1, 1);
                case 8:
                    return new Vec3(0, 0, 0.5);
                case 9:
                    return new Vec3(0, 0.5, 0);
                case 10:
                    return new Vec3(0, 0.5, 0.5);
                case 11:
                    return new Vec3(0.5, 0, 0);
                case 12:
                    return new Vec3(0.5, 0, 0.5);
                case 13:
                    return new Vec3(0.5, 0.5, 0);
                case 14:
                    return new Vec3(0.5, 0.5, 0.5);
            }

            return new Vec3(0, 0, 0);
        }

        private static Random rng = new Random();

        public static double rand()
        {
            return rng.NextDouble();
        }

        public static double rand(double min, double max)
        {
            return (rng.NextDouble() * (max - min) + min);
        }

        private static ThreadLocal<Vec3> p = new ThreadLocal<Vec3>();
        private static ThreadLocal<Random> rngs = new ThreadLocal<Random>();
        public static Vec3 randomInUnitSphere()
        {
            if (!rngs.IsValueCreated)
            {
                rngs.Value = new Random();
            }

            do
            {
                p.Value = (2.0 * new Vec3(rngs.Value.NextDouble(), rngs.Value.NextDouble(), rngs.Value.NextDouble())) - new Vec3(1, 1, 1);
            }
            while (p.Value.lengthSquared() >= 1.0);

            return p.Value;
        }
    }
}
