using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CRT
{
    class Utils
    {
        public static ConsoleColor FromColor(Vector3 c)
        {
            int r = (int)(c.X * 255.0);
            int g = (int)(c.Y * 255.0);
            int b = (int)(c.Z * 255.0);

            int index = (r > 128 | g > 128 | b > 128) ? 8 : 0; // Bright bit

            index |= (r > 64) ? 4 : 0; // Red bit
            index |= (b > 64) ? 2 : 0; // Green bit
            index |= (g > 64) ? 1 : 0; // Blue bit

            return (ConsoleColor)index;
        }
    }
}
