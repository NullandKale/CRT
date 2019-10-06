using CRT.Engine;
using CRT.Engine.ChexelComponents;
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
        public static TileMapManager tileMapManager;
        public static WorldManager worldManager;
        public static FrameManager frameManager;
        public static UIManager uiManager;
        public static MenuManager menuManager;
        public static SaveDataManager saveData;

        public static bool run = true;
        public static int tick = 0;

        public static void Main(string[] args)
        {
            engineInit();
            engineTestSetup();
            engineStart();
        }

        public static void engineInit()
        {
            //savedata should start first
            saveData = new SaveDataManager();

            menuManager = new MenuManager();

            frameManager = new FrameManager();

            tileMapManager = new TileMapManager();

            uiManager = new UIManager();
            uiManager.AddMessage(new Message(0, 0, "no man was here. v0.1.0", ConsoleColor.White, ConsoleColor.Black), false);

            input = new InputManager();

            rayTracer = new RayTracer(frameManager.height, frameManager.width, 2, 6, 90, true);
            rayTracer.camera.doUpdate = true;
            rayTracer.ambientLight = 0.075;

            worldManager = new WorldManager(200);

            frameManager.addLayer(tileMapManager);
            frameManager.addLayer(rayTracer);
            frameManager.addLayer(uiManager);
        }

        public static void engineStart()
        {
            double averageFrameTime = 0;
            long frames = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            worldManager.start();

            Utils.resetConsoleColor();

            while (run)
            {
                tick++;
                //these update orders matter
                worldManager.Update();
                rayTracer.Update(true);
                rayTracer.Render(true);
                input.Update();
                menuManager.update();
                tileMapManager.Update();

                frameManager.Draw();
                uiManager.Update();

                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;

                string renderInfo = string.Format("{0:0.00}", 1.0 / (averageFrameTime / frames)) + " FPS"
                                            + " POS " + rayTracer.camera.origin.ToString()
                                            + " D " + string.Format("{0:0.00}", frameManager.drawTime.TotalMilliseconds)
                                            + "MS R " + string.Format("{0:0.00}", rayTracer.renderTime.TotalMilliseconds)
                                            + "MS U " + string.Format("{0:0.00}", rayTracer.updateTime.TotalMilliseconds)
                                            + "MS";
                uiManager.AddMessage(new Message(0, frameManager.height - 1, renderInfo, ConsoleColor.White, ConsoleColor.Black), true);
                stopwatch.Restart();

                if (input.IsKeyFalling(OpenTK.Input.Key.Escape))
                {
                    menuManager.activateMenu();
                }
            }

            saveData.stop();
        }

        public static void engineTestSetup()
        {
            Entity pc = new Entity(new Vec3(0, 0, 0), 0);
            pc.AddComponent(new CameraComponent());
            worldManager.addEntity(pc);

            int balls = 3;

            for (int i = 0; i < balls; i++)
            {
                for (int j = 0; j < balls; j++)
                {
                    Entity ball = new Entity(new Vec3(i * 10, 1, j * 10), 4.5);
                    ball.AddComponent(new BounceComponent());
                    worldManager.addEntity(ball);
                }
            }

            ChexelEntity pcT = new ChexelEntity(new vector2(60, 228), ConsoleColor.White, '@');
            pcT.AddComponent(new CameraCComponent());
            tileMapManager.addEntity(pcT);

            string[] rocket =
            {
                "         /\\",
                "        |==|",
                "        |  |",
                "        |  |",
                "        |  |",
                "       /____\\",
                "       |    |",
                "       |    |",
                "       |    |",
                "       |    |",
                "      /| |  |\\",
                "     / | |  | \\",
                "    /__|_|__|__\\",
                "       /_\\/_\\",
            };

            FrameEntity rocketShip = new FrameEntity(new vector2(), ConsoleColor.White, ' ', Frame.FromStringArray(rocket));
            tileMapManager.addFrameEntity(rocketShip);
        }
    }
}