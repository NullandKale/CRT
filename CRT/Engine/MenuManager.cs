using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CRT.Engine
{
    public class MenuManager
    {
        public bool isActive = true;
        public int selected = 0;
        public bool wait = true;
        public int waitFrames = 25;
        public int waitCounter = 0;
        public Menu currentMenu;
        List<Message> messages;

        public MenuManager()
        {
            currentMenu = new MainMenu();
            messages = new List<Message>();
        }

        public void setMenu(Menu toSet)
        {
            currentMenu = toSet;
            wait = true;
            waitCounter = 0;
            selected = 0;
        }

        public void activate3D()
        {
            Program.rayTracer.isActive = true;
            Program.worldManager.isActive = true;
            Program.menuManager.isActive = false;
            Program.tileMapManager.isActive = false;
        }

        public void exit()
        {
            Program.saveData.stop();
            Thread.Sleep(200);
            Environment.Exit(0);
        }

        public void activate2D(int planetNumber, MapType mapType)
        {
            Program.rayTracer.isActive = false;
            Program.worldManager.isActive = false;
            Program.menuManager.isActive = false;
            Program.tileMapManager.generatePlanet(planetNumber, mapType);
            Program.tileMapManager.isActive = true;
        }

        public void activateMenu()
        {
            Program.rayTracer.isActive = false;
            Program.worldManager.isActive = false;
            Program.menuManager.isActive = true;
            Program.tileMapManager.isActive = false;

            currentMenu = new MainMenu();
            messages = new List<Message>();
        }

        public void createMessages()
        {
            string[] lines = currentMenu.getMessages();
            messages.Clear();

            for (int i = 0; i < lines.Length; i++)
            {
                if(i == selected)
                {
                    messages.Add(new Message((Program.frameManager.width / 2) - (lines[i].Length / 2), (Program.frameManager.height / 2) + (i * 2 - lines.Length), lines[i], ConsoleColor.White, ConsoleColor.DarkGray));
                }
                else
                {
                    messages.Add(new Message((Program.frameManager.width / 2) - (lines[i].Length / 2), (Program.frameManager.height / 2) + (i * 2 - lines.Length), lines[i], ConsoleColor.White, ConsoleColor.Black));
                }
            }
        }

        public void update()
        {
            if(isActive)
            {
                createMessages();
                Program.uiManager.AddMessages(messages, true);

                if (Program.input.IsKeyFalling(OpenTK.Input.Key.W) || Program.input.IsKeyFalling(OpenTK.Input.Key.Up))
                {
                    if (selected > 0)
                    {
                        selected--;
                    }
                }

                if (Program.input.IsKeyFalling(OpenTK.Input.Key.S) || Program.input.IsKeyFalling(OpenTK.Input.Key.Down))
                {
                    if (selected < currentMenu.optionCount() - 1)
                    {
                        selected++;
                    }
                }

                if (!wait)
                {
                    if (Program.input.IsKeyRising(OpenTK.Input.Key.Space) || Program.input.IsKeyRising(OpenTK.Input.Key.Enter))
                    {
                        currentMenu.select(selected);
                        wait = true;
                        waitCounter = 0;
                    }
                }
            }

            if(wait)
            {
                if (waitCounter > waitFrames)
                {
                    wait = false;
                    waitCounter = 0;
                }
                else
                {
                    waitCounter++;
                }
            }
        }
    }

    public class MainMenu : Menu
    {
        string[] options =
            {
                "Start Game",
                "Graphics Options",
                "Back",
                "Exit",
            };
        public string[] getMessages()
        {
            return options;
        }

        public int optionCount()
        {
            return options.Length;
        }

        public void select(int selected)
        {
            switch(selected)
            {
                case 0:
                    {
                        Program.menuManager.activate3D();
                        break;
                    }
                case 1:
                    {
                        Program.menuManager.currentMenu = new SettingsMenu();
                        break;
                    }
                case 2:
                    {
                        Program.menuManager.activate3D();
                        break;
                    }
                case 3:
                    {
                        Program.menuManager.exit();
                        break;
                    }
            }
        }
    }

    public class SettingsMenu : Menu
    {
        string[] optionsBase =
            {
                "Set FOV",
                "Set SuperSampling",
                "Set Max Ray Bounces",
                "Back",
            };
        public string[] getMessages()
        {
            return optionsBase;
        }

        public int optionCount()
        {
            return optionsBase.Length;
        }

        public void select(int selected)
        {
            switch (selected)
            {
                case 0:
                    {
                        int newFOV = Utils.GetIntFromConsole("The current FOV is " + Program.rayTracer.camera.vfov + " enter new FOV");
                        Program.rayTracer.camera.vfov = newFOV;
                        break;
                    }
                case 1:
                    {
                        int newSS = Utils.GetIntFromConsole("The current SS is " + Program.rayTracer.superSample + " enter new SS");
                        Program.rayTracer.superSample = newSS;
                        break;
                    }
                case 2:
                    {
                        int newMRB = Utils.GetIntFromConsole("The current MRB is " + Program.rayTracer.maxDepth + " enter new MRB");
                        Program.rayTracer.maxDepth = newMRB;
                        break;
                    }
                case 3:
                    {
                        Program.menuManager.currentMenu = new MainMenu();
                        break;
                    }
            }
        }
    }

    public interface Menu
    {
        int optionCount();
        string[] getMessages();
        void select(int selected);
    }
}
