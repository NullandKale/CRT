using CRT.Engine;
using CRT.Engine.ChexelComponents;
using CRT.Engine.Components;
using CRT.IOW;
using CRT.SolarSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

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
        public static InventoryManager inventoryManager;

        public static bool run = true;
        public static int tick = 0;
        public static int frameLimit = 30;

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
            uiManager.AddMessage(new Message(0, 0, "no man was here. v0.1.1", ConsoleColor.White, ConsoleColor.Black), false);

            input = new InputManager();

            rayTracer = new RayTracer(frameManager.height, frameManager.width, 2, 6, 90, true);
            rayTracer.camera.doUpdate = true;
            rayTracer.ambientLight = 0.075;

            worldManager = new WorldManager(200);

            frameManager.addLayer(tileMapManager);
            frameManager.addLayer(rayTracer);
            frameManager.addLayer(uiManager);

            inventoryManager = new InventoryManager();
            inventoryManager.ResetToZero();
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
                bool frameLimited = false;

                //these update orders matter
                worldManager.Update();
                rayTracer.Update(true);
                rayTracer.Render(true);
                input.Update();
                menuManager.update();
                tileMapManager.Update();

                frameManager.Draw();
                uiManager.Update();

                while (stopwatch.Elapsed.TotalMilliseconds < (1000.0 / frameLimit))
                {
                    Thread.Sleep(1);
                    frameLimited = true;
                }

                averageFrameTime += stopwatch.Elapsed.TotalSeconds;
                frames++;

                string renderInfo = string.Format("{0:0.00}", 1.0 / (averageFrameTime / frames)) + (frameLimited ? " fps FRAMELIMITED" : " fps")
                                            + " pos " + rayTracer.camera.origin.ToString()
                                            + " d " + string.Format("{0:0.00}", frameManager.drawTime.TotalMilliseconds)
                                            + "MS r " + string.Format("{0:0.00}", rayTracer.renderTime.TotalMilliseconds)
                                            + "MS u " + string.Format("{0:0.00}", rayTracer.updateTime.TotalMilliseconds)
                                            + "MS";
                uiManager.AddMessage(new Message(0, frameManager.height - 1, renderInfo, ConsoleColor.White, ConsoleColor.Black), true);

                int inventoryCharCount = frameManager.width - 1;

                string oxygenString = inventoryManager.GetOxygen().ToString("0000");
                inventoryCharCount -= oxygenString.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, oxygenString, ConsoleColor.White, ConsoleColor.Black), true);
                inventoryCharCount -= 1;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, " ", ConsoleColor.White, ConsoleColor.DarkGreen), true);
                string oxygenHeader = " O2";
                inventoryCharCount -= oxygenHeader.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, oxygenHeader, ConsoleColor.White, ConsoleColor.Black), true);

                string waterString = inventoryManager.GetWater().ToString("0000");
                inventoryCharCount -= waterString.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, waterString, ConsoleColor.White, ConsoleColor.Black), true);
                inventoryCharCount -= 1;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, " ", ConsoleColor.White, ConsoleColor.DarkBlue), true);
                string waterHeader = " H20";
                inventoryCharCount -= waterHeader.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, waterHeader, ConsoleColor.White, ConsoleColor.Black), true);

                string foodString = inventoryManager.GetFood().ToString("0000");
                inventoryCharCount -= foodString.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, foodString, ConsoleColor.White, ConsoleColor.Black), true);
                inventoryCharCount -= 1;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, " ", ConsoleColor.White, ConsoleColor.DarkYellow), true);
                string foodHeader = " FOOD";
                inventoryCharCount -= foodHeader.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, foodHeader, ConsoleColor.White, ConsoleColor.Black), true);

                string fuelString = inventoryManager.GetFuel().ToString("0000");
                inventoryCharCount -= fuelString.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, fuelString, ConsoleColor.White, ConsoleColor.Black), true);
                inventoryCharCount -= 1;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, " ", ConsoleColor.White, ConsoleColor.DarkMagenta), true);
                string fuelHeader = "FUEL";
                inventoryCharCount -= fuelHeader.Length;
                uiManager.AddMessage(new Message(inventoryCharCount, 0, fuelHeader, ConsoleColor.White, ConsoleColor.Black), true);

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

            SolarSystem.SolarSystem solarSystem = new SolarSystem.SolarSystem(8);
            for (int i = 0; i < solarSystem.numPlanets; i++)
            {
                worldManager.addEntity(solarSystem.planets[i].entity);
            }

            ChexelEntity pcT = new ChexelEntity(new vector2(60, 228), ConsoleColor.White, '@');
            pcT.AddComponent(new CameraCComponent());
            tileMapManager.addEntity(pcT);

            string[] rocket =
            {
                "     /\\ ",
                "    |==| ",
                "    |  | ",
                "    |  | ",
                "    |  | ",
                "   /____\\ ",
                "   |    | ",
                "   |    | ",
                "   |    | ",
                "   |    | ",
                "  /| |  |\\ ",
                " / | |  | \\ ",
                "/__|_|__|__\\ ",
                "   /_\\/_\\ "
            };

            FrameEntity rocketShip = new FrameEntity(new vector2(54, 2), ConsoleColor.White, ' ', Frame.FromStringArray(rocket, ConsoleColor.Black, ConsoleColor.Green));
            rocketShip.AddComponent(new CockpitCComponent());
            tileMapManager.addFrameEntity(rocketShip);
        }
    }
}