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
        //Managers
        public static InputManager input;
        public static RayTracer rayTracer;
        public static WorldManager worldManager;
        public static FrameManager frameManager;

        public static void Main(string[] args)
        {
            engineInit();
            engineTestSetup();
            engineStart();
        }

        public static void engineInit()
        {
            frameManager = new FrameManager();
            input = new InputManager();
            rayTracer = new RayTracer(frameManager.height, frameManager.width, 2, 6, 90, true);
            frameManager.addLayer(rayTracer);
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
                worldManager.Update();
                rayTracer.Update(true);
                rayTracer.Render(true);
                input.Update();
                frameManager.Draw();
                Utils.resetConsoleColor();

                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;

                string renderInfo = string.Format("CRT Engine Test {0:0.00}", 1.0 / (averageFrameTime / frames)) + " FPS"
                                            + " FOV:" + rayTracer.camera.vfov
                                            + " POS " + rayTracer.camera.origin.ToString() + " " + (rayTracer.camera.lookAt - rayTracer.camera.origin)
                                            + " D " + string.Format("{0:0.00}", frameManager.drawTime.TotalMilliseconds)
                                            + "MS R " + string.Format("{0:0.00}", rayTracer.renderTime.TotalMilliseconds)
                                            + "MS U " + string.Format("{0:0.00}", rayTracer.updateTime.TotalMilliseconds)
                                            + "MS";
                renderInfo = renderInfo.PadRight(frameManager.width);
                Console.Write(renderInfo);
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
                    Entity ball = new Entity(new Vec3(i * 10, 1, j * 10), 4.5);
                    ball.AddComponent(new BounceComponent());
                    worldManager.addEntity(ball);
                }
            }

        }
    }
}
