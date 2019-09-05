using CRT.IOW;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CRT
{
    class Program
    {
        public static int height;
        public static int width;
        public static InputManager input;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            //testBMP("test.bmp");
            testRayTracer(false);
        }

        public static void testBMP(string fileName)
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth;
            input = new InputManager();

            RayTracer rayTracer = new RayTracer(900, 1600, 1, 8, 90, false, false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            rayTracer.CreateBitmap(fileName, true, true);
            timer.Stop();
            Console.WriteLine(string.Format("{0:00.000}", timer.Elapsed.TotalSeconds) + " seconds");
        }

        public static void testRayTracer(bool doBenchmark)
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth - 1;
            input = new InputManager();

            RayTracer rayTracer = new RayTracer(height, width, 2, 16, 90, true, false);
            rayTracer.camera.doUpdate = true;

            double averageFrameTime = 0;
            long frames = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stopwatch totalTime = new Stopwatch();
            if (doBenchmark)
            {
                totalTime.Start();
            }

            while (true)
            {
                rayTracer.Draw(true, false);
                input.Update();
                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(string.Format("{0:0.00}", 1.0 / (averageFrameTime / frames)) + " FPS FOV:" + rayTracer.camera.vfov + " POS " + rayTracer.camera.origin.ToString() + " " + (rayTracer.camera.lookAt - rayTracer.camera.origin) + "                       ");
                stopwatch.Restart();

                if(doBenchmark && totalTime.Elapsed.TotalSeconds > 30)
                {
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("Benchmark Result: " + frames + " frames in " + totalTime.Elapsed.ToString() + string.Format(" AVG FPS: {0:0.00}", 1.0 / (averageFrameTime / frames)));
                    Console.WriteLine("\n\n\nDid you know you can use wasd to move, qe to change fov, r to spawn random spheres, and zx to change elevation");
                    Console.WriteLine("\nFor best FPS make sure to build release instead of debug, also run without debugging");
                    Console.WriteLine("\nFor most repeatable benchmark do not press any keys for first " + totalTime.Elapsed.TotalSeconds + " seconds");
                    Console.WriteLine("\nPress enter to continue");
                    Console.ReadLine();
                    totalTime.Reset();
                }
            }
        }
    }
}
