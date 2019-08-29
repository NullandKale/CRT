using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CRT
{
    class Program
    {
        public static int height;
        public static int width;

        static void Main(string[] args)
        {
            //testBMP();
            testRayTracer();
        }

        public static void testBMP()
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth;

            RayTracer rayTracer = new RayTracer(height * 4, width * 4, 32, 16, 90, false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            rayTracer.CreateBitmap(true, true, true);
            timer.Stop();
            Console.WriteLine(string.Format("{0:00.000}", timer.Elapsed.TotalSeconds) + " seconds");
            Console.ReadLine();
        }

        public static void testRayTracer()
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth;

            RayTracer rayTracer = new RayTracer(height, width, 16, 8, 90, true);

            double averageFrameTime = 0;
            long frames = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                rayTracer.Draw(true, false);

                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(string.Format("{0:00.00}", 1.0 / (averageFrameTime / frames)) + " FPS                ");
                stopwatch.Restart();
            }
        }
    }
}
