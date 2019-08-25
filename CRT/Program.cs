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
            RayTracer rayTracer = new RayTracer(500, 500, 1);
            rayTracer.CreateBitmap(true, true);
        }

        public static void testRayTracer(int superSample, bool greyScale)
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth;

            RayTracer rayTracer = new RayTracer(height, width, superSample);

            int travel = 25;
            bool direction = true;
            while(true)
            {
                rayTracer.Draw(greyScale);
                rayTracer.light0.position.X += direction ? 1 : -1;

                if(Math.Abs(rayTracer.light0.position.X) > travel)
                {
                    direction = !direction;
                }
            }
        }
    }
}
