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
            runRayTracer();
            //testRayTracer(true);
        }

        public static void runRayTracer()
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth - 1;
            input = new InputManager();

            RayTracer rayTracer = new RayTracer(height, width, 2, 8, 90, true);
            rayTracer.camera.doUpdate = true;

            double averageFrameTime = 0;
            long frames = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            while (true)
            {
                rayTracer.update(true);
                rayTracer.renderDrawAsync(true, false);
                input.Update();

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;

                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;

                Console.Write(string.Format("{0:0.00}", 1.0 / (averageFrameTime / frames)) + " FPS"
                                            + " FOV:" + rayTracer.camera.vfov
                                            + " POS " + rayTracer.camera.origin.ToString() + " " + (rayTracer.camera.lookAt - rayTracer.camera.origin)
                                            + " D " + string.Format("{0:0.00}", rayTracer.drawTime.TotalMilliseconds)
                                            + "MS R " + string.Format("{0:0.00}", rayTracer.renderTime.TotalMilliseconds)
                                            + "MS U " + string.Format("{0:0.00}", rayTracer.updateTime.TotalMilliseconds)
                                            + "MS                       ");
                stopwatch.Restart();
            }
        }

        public static void testRayTracer(bool doBenchmark)
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth - 1;
            input = new InputManager();

            RayTracer rayTracer = new RayTracer(height, width, 2, 8, 90, true);
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

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            while (true)
            {
                rayTracer.StartRender(true);
                rayTracer.update(true);
                
                Console.SetCursorPosition(0, 0);

                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;

                Console.Write(string.Format("{0:0.00}", 1.0 / (averageFrameTime / frames)) + " FPS" 
                                            + " FOV:" + rayTracer.camera.vfov 
                                            + " POS " + rayTracer.camera.origin.ToString() + " " + (rayTracer.camera.lookAt - rayTracer.camera.origin) 
                                            + " D " + string.Format("{0:0.00}", rayTracer.drawTime.TotalMilliseconds) 
                                            + "MS R " + string.Format("{0:0.00}", rayTracer.renderTime.TotalMilliseconds) 
                                            + "MS U " + string.Format("{0:0.00}", rayTracer.updateTime.TotalMilliseconds)
                                            + "MS                       ");
                stopwatch.Restart();

                if(doBenchmark && totalTime.Elapsed.TotalSeconds > 60)
                {
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("Benchmark Result: " + frames + " frames in " + totalTime.Elapsed.ToString() + string.Format(" AVG FPS: {0:0.00}", 1.0 / (averageFrameTime / frames)));
                    Console.WriteLine("\nPress enter to continue");
                    Console.ReadLine();
                    totalTime.Reset();
                }
            }
        }
    }
}
