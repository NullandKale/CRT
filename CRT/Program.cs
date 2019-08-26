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
            //testRayTracer(3, false);
        }

        public static void testBMP()
        {
            RayTracer rayTracer = new RayTracer(500, 500);
            rayTracer.CreateBitmap(true, true);
        }

        public static void testRayTracer(int superSample, bool greyScale)
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth;

            RayTracer rayTracer = new RayTracer(height, width);

            while(true)
            {
                rayTracer.Draw();
            }
        }
    }
}
