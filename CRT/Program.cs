using CRT.Engine;
using CRT.Engine.Components;
using CRT.IOW;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CRT
{
    class Program
    {
        //Dont change these values
        public static int height;
        public static int width;

        //Managers
        public static InputManager input;
        public static RayTracer rayTracer;
        public static WorldManager worldManager;

        public static void Main(string[] args)
        {
            engineInit();
            engineTestSetup();
            engineStart();
        }

        public static void engineInit()
        {
            height = Console.WindowHeight - 1;
            width = Console.WindowWidth - 1;

            input = new InputManager();
            rayTracer = new RayTracer(height, width, 1, 6, 90, true);
            rayTracer.camera.doUpdate = true;
            worldManager = new WorldManager(400);
        }

        public static void engineStart()
        {
            double averageFrameTime = 0;
            long frames = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            worldManager.start();

            Utils.resetConsoleColor();

            while (true)
            {
                worldManager.update();
                rayTracer.update(true);
                rayTracer.renderDrawAsync(true, false);
                input.Update();

                Utils.resetConsoleColor();

                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;

                Console.Write(string.Format("CRT Engine Test {0:0.00}", 1.0 / (averageFrameTime / frames)) + " FPS"
                                            + " FOV:" + rayTracer.camera.vfov
                                            + " POS " + rayTracer.camera.origin.ToString() + " " + (rayTracer.camera.lookAt - rayTracer.camera.origin)
                                            + " D " + string.Format("{0:0.00}", rayTracer.drawTime.TotalMilliseconds)
                                            + "MS R " + string.Format("{0:0.00}", rayTracer.renderTime.TotalMilliseconds)
                                            + "MS U " + string.Format("{0:0.00}", rayTracer.updateTime.TotalMilliseconds)
                                            + "MS                       ");
                stopwatch.Restart();
            }
        }

        public static void engineTestSetup()
        {
            Entity pc = new Entity(new Vec3(0, 0, 0), 0);
            pc.AddComponent(new CameraComponent());
            worldManager.addEntity(pc);

            int balls = 3;

            for(int i = 0; i < balls; i++)
            {
                for(int j = 0; j < balls; j++)
                {
                    Entity ball = new Entity(new Vec3(i, 1, j), 0.5);
                    ball.AddComponent(new BounceComponent());
                    worldManager.addEntity(ball);
                }
            }

        }
    }
}
