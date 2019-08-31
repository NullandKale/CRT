﻿using System;
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

            //testBMP();
            testRayTracer();
        }

        public static void testBMP()
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth;
            input = new InputManager();

            RayTracer rayTracer = new RayTracer(400, 800, 1, 10, 90, false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            rayTracer.CreateBitmap(true, true, true);
            timer.Stop();
            Console.WriteLine(string.Format("{0:00.000}", timer.Elapsed.TotalSeconds) + " seconds");
        }

        public static void testRayTracer()
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth - 1;
            input = new InputManager();

            RayTracer rayTracer = new RayTracer(height, width, 2, 5, 90, true);

            double averageFrameTime = 0;
            long frames = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stopwatch totalTime = new Stopwatch();
            totalTime.Start();

            while (true)
            {
                rayTracer.Draw(true, false);
                input.Update();
                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(string.Format("{0:0.00}", 1.0 / (averageFrameTime / frames)) + " FPS FOV:" + rayTracer.camera.vfov + " POS " + rayTracer.camera.origin.ToString() + "                       ");
                stopwatch.Restart();

                if(totalTime.Elapsed.TotalSeconds > 30)
                {
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("Benchmark Result: " + frames + " frames in " + totalTime.Elapsed.ToString() + string.Format(" AVG FPS: {0:0.00}", 1.0 / (averageFrameTime / frames)));
                    Console.WriteLine("\n\n\nDid you know you can use wasd to move and qe to change fov and r to spawn random spheres");
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
