using System;
using System.Collections.Generic;

namespace CRT
{
    class Program
    {
        public static int height;
        public static int width;

        static void Main(string[] args)
        {
            testBMP();
            height = Console.WindowHeight;
            width = Console.WindowWidth;

            RayTracer rayTracer = new RayTracer(height, width);
            rayTracer.Draw();
        }

        public static void testBMP()
        {
            RayTracer rayTracer = new RayTracer(1000, 1000);
            rayTracer.CreateBitmap(true);
        }
    }
}
