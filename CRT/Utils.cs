using CRT.IOW;
using ObjLoader.Loader.Loaders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;

namespace CRT
{
    public class Utils
    {
        public static void resetConsoleColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static ConsoleColor FromColor(Vec3 c, int pallet)
        {
            double max = Math.Max(c.x, Math.Max(c.y, c.z));
            if (max > 1)
            {
                c = c * (1.0 / max);
            }

            int r = (int)(Math.Max(0.0, Math.Min(1, c.r())) * 255.0);
            int g = (int)(Math.Max(0.0, Math.Min(1, c.g())) * 255.0);
            int b = (int)(Math.Max(0.0, Math.Min(1, c.b())) * 255.0);

            Color color = Color.FromArgb(r, g, b);

            switch(pallet)
            {
                case 0:
                    {
                        int index = (r > 128 | g > 128 | b > 128) ? 8 : 0; // Bright bit

                        index |= (r > 64) ? 4 : 0; // Red bit
                        index |= (g > 64) ? 2 : 0; // Green bit
                        index |= (b > 64) ? 1 : 0; // Blue bit

                        return (ConsoleColor)index;
                    }
                case 1:
                    {
                        if (color.GetSaturation() < 0.5)
                        {
                            // we have a grayish color
                            switch ((int)(color.GetBrightness() * 3.5))
                            {
                                case 0: return ConsoleColor.Black;
                                case 1: return ConsoleColor.DarkGray;
                                case 2: return ConsoleColor.Gray;
                                default: return ConsoleColor.White;
                            }
                        }
                        int hue = (int)Math.Round(color.GetHue() / 60, MidpointRounding.AwayFromZero);
                        if (color.GetBrightness() < 0.4)
                        {
                            // dark color
                            switch (hue)
                            {
                                case 1: return ConsoleColor.DarkYellow;
                                case 2: return ConsoleColor.DarkGreen;
                                case 3: return ConsoleColor.DarkCyan;
                                case 4: return ConsoleColor.DarkBlue;
                                case 5: return ConsoleColor.DarkMagenta;
                                default: return ConsoleColor.DarkRed;
                            }
                        }
                        // bright color
                        switch (hue)
                        {
                            case 1: return ConsoleColor.Yellow;
                            case 2: return ConsoleColor.Green;
                            case 3: return ConsoleColor.Cyan;
                            case 4: return ConsoleColor.Blue;
                            case 5: return ConsoleColor.Magenta;
                            default: return ConsoleColor.Red;
                        }
                    }
                case 2:
                    {
                        ConsoleColor ret = 0;
                        double rr = r, gg = g, bb = b, delta = double.MaxValue;

                        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
                        {
                            var n = Enum.GetName(typeof(ConsoleColor), cc);
                            var c1 = Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                            var t = Math.Pow(c1.R - rr, 2.0) + Math.Pow(c1.G - gg, 2.0) + Math.Pow(c1.B - bb, 2.0);
                            if (t == 0.0)
                                return cc;
                            if (t < delta)
                            {
                                delta = t;
                                ret = cc;
                            }
                        }
                        return ret;
                    }
                default:
                    {
                        return ConsoleColor.Black;
                    }
            }
        }

        public static double min(double a, double b)
        {
            return a < b ? a : b;
        }

        public static double max(double a, double b)
        {
            return a > b ? a : b;
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
            int val = rng.Next(0, 7);
            //TODO fix colors after 7
            //int val = 8;

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
                    return new Vec3(0.25, 0.25, 1);
                case 9:
                    return new Vec3(0.5, 1, 0.5);
                case 10:
                    return new Vec3(0.5, 1, 1);
                case 11:
                    return new Vec3(1, 0.5, 0.5);
                case 12:
                    return new Vec3(1, 0.5, 1);
                case 13:
                    return new Vec3(1, 1, 0.5);
                case 14:
                    return new Vec3(1, 1, 1);
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

        public static ConsoleColor GetConsoleColor(Color color)
        {
            if (color.GetSaturation() < 0.5)
            {
                // we have a grayish color
                switch ((int)(color.GetBrightness() * 3.5))
                {
                    case 0: return ConsoleColor.Black;
                    case 1: return ConsoleColor.DarkGray;
                    case 2: return ConsoleColor.Gray;
                    default: return ConsoleColor.White;
                }
            }
            int hue = (int)Math.Round(color.GetHue() / 60, MidpointRounding.AwayFromZero);
            if (color.GetBrightness() < 0.4)
            {
                // dark color
                switch (hue)
                {
                    case 1: return ConsoleColor.DarkYellow;
                    case 2: return ConsoleColor.DarkGreen;
                    case 3: return ConsoleColor.DarkCyan;
                    case 4: return ConsoleColor.DarkBlue;
                    case 5: return ConsoleColor.DarkMagenta;
                    default: return ConsoleColor.DarkRed;
                }
            }
            // bright color
            switch (hue)
            {
                case 1: return ConsoleColor.Yellow;
                case 2: return ConsoleColor.Green;
                case 3: return ConsoleColor.Cyan;
                case 4: return ConsoleColor.Blue;
                case 5: return ConsoleColor.Magenta;
                default: return ConsoleColor.Red;
            }
        }

        private static IObjLoader objLoader;

        public static LoadResult LoadOBJ(string filename)
        {
            if(objLoader != null)
            {
                var objloaderFactory = new ObjLoaderFactory();
                objLoader = objloaderFactory.Create();
            }

            FileStream fileStream = new FileStream(filename, FileMode.Open);
            return objLoader.Load(fileStream);
        }
    }
}
