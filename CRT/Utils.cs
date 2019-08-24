using System;
using System.Collections.Generic;
using System.Text;

namespace CRT
{
    class Utils
    {
        public static ConsoleColor FromColor(Color c)
        {
            int index = (c.r > 128 | c.g > 128 | c.b > 128) ? 8 : 0; // Bright bit
            index |= (c.r > 64) ? 4 : 0; // Red bit
            index |= (c.b > 64) ? 2 : 0; // Green bit
            index |= (c.g > 64) ? 1 : 0; // Blue bit
            return (ConsoleColor)index;
        }
    }
}
