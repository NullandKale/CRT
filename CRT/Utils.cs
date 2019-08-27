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

        public static bool SolveQuadradic(float a, float b, float c, ref float x0, ref float x1)
        {
            float discr = b * b - 4 * a * c;

            if (discr < 0)
            {
                return false;
            }
            else if (discr == 0)
            {
                x0 = 0.5f * b / a;
                x1 = x0;
            }
            else
            {
                float q = (b > 0) ?
                    (float)(-0.5f * (b + Math.Sqrt(discr))): 
                    (float)(-0.5f * (b - Math.Sqrt(discr)));
                x0 = q / a;
                x1 = c / q;
            }

            if (x0 > x1)
            {
                float x1temp = x0;
                x0 = x1;
                x1 = x1temp;
            }

            return true;
        }

    }
}
