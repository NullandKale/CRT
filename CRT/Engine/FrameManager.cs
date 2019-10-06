using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CRT
{
    public class FrameManager
    {
        public int width;
        public int height;

        public Frame main;

        private Stopwatch timer;
        public TimeSpan drawTime;

        public List<ChexelProvider> layers;

        public FrameManager()
        {
            width = Console.WindowWidth - 1;
            height = Console.WindowHeight - 1;
            timer = new Stopwatch();

            layers = new List<ChexelProvider>();

            main = new Frame(width, height, 0, 0);
        }

        public void addLayer(ChexelProvider layer)
        {
            layers.Add(layer);
        }

        private void clear()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    main.setChexel(i, j, new Chexel());
                }
            }
        }

        private void flatten()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if(layers[i].active())
                {
                    main = Frame.FlattenFromChexelProvider(layers[i], main);
                }
            }
        }

        public void Draw()
        {
            timer.Restart();

            flatten();

            Console.SetCursorPosition(0, 0);
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForground = Console.ForegroundColor;

            for (int y = 0; y < height; y++)
            {
                List<char> line = new List<char>();
                for (int x = 0; x < width; x++)
                {
                    char toPrint = ' ';
                    ConsoleColor bToSet = currentBackground;
                    ConsoleColor fToSet = currentForground;

                    if ((x >= 0 && x < width) && (y >= 0 && y < height))
                    {
                        Chexel chexel = main.getChexel(x, y);

                        bToSet = chexel.b;
                        fToSet = chexel.f;
                        toPrint = chexel.t;

                        if (bToSet != currentBackground || fToSet != currentForground)
                        {
                            Console.Write((char[])line.ToArray());
                            line.Clear();

                            if (bToSet != currentBackground)
                            {
                                Console.BackgroundColor = bToSet;
                                currentBackground = Console.BackgroundColor;
                            }

                            if (fToSet != currentForground)
                            {
                                Console.ForegroundColor = fToSet;
                                currentForground = Console.BackgroundColor;
                            }
                        }
                    }

                    line.Add(toPrint);
                }

                Console.WriteLine((char[])line.ToArray());
            }

            timer.Stop();
            drawTime = timer.Elapsed;
        }
    }
    public interface ChexelProvider
    {
        bool active();
        bool hasChexel(int x, int y);
        Chexel getChexel(int x, int y);
    }

    public struct Frame : ChexelProvider
    {
        public int width;
        public int height;
        public int xOffset;
        public int yOffset;
        public bool useOffset;
        public bool isActive;

        public ConsoleColor[,] background;
        public ConsoleColor[,] forground;
        public char[,] text;

        public Frame(int width, int height, int xOffset, int yOffset)
        {
            this.width = width;
            this.height = height;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.useOffset = true;

            background = new ConsoleColor[width,height];
            forground = new ConsoleColor[width,height];
            text = new char[width,height];

            isActive = true;

            for (int i = xOffset; i < width + xOffset; i++)
            {
                for (int j = yOffset; j < height + yOffset; j++)
                {
                    setChexel(i, j, new Chexel());
                }
            }
        }

        public bool hasChexel(int x, int y)
        {
            if (useOffset)
            {
                return (x >= xOffset && x < width + xOffset) && (y >= yOffset && y < height + yOffset);
            }
            else
            {
                return (x >= 0 && x < width) && (y >= 0 && y < height);
            }
        }

        public Chexel getChexel(int x, int y)
        {
            if(!hasChexel(x,y))
            {
                return new Chexel();
            }

            x -= xOffset;
            y -= yOffset;

            return new Chexel(forground[x, y], background[x, y], text[x, y]);
        }

        public void setChexel(int x, int y, Chexel chexel)
        {
            if(hasChexel(x,y))
            {
                forground[x, y] = chexel.f;
                background[x, y] = chexel.b;
                text[x, y] = chexel.t;
            }
        }

        public static Frame FlattenFromChexelProvider(ChexelProvider top, Frame bottom)
        {
            int xMax = bottom.width + bottom.xOffset;
            int yMax = bottom.height + bottom.yOffset;

            Frame toReturn = new Frame(xMax, yMax, 0, 0);

            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    Chexel toSet = new Chexel();

                    if (top.hasChexel(x, y))
                    {
                        toSet = top.getChexel(x, y);
                    }
                    else if (bottom.hasChexel(x, y))
                    {
                        toSet = bottom.getChexel(x, y);
                    }

                    toReturn.setChexel(x, y, toSet);
                }
            }

            return toReturn;
        }

        public static Frame FromStringArray(string[] input, ConsoleColor f, ConsoleColor b)
        {
            int height = 0;

            for(int i = 0; i < input.Length; i++)
            {
                if (height < input[i].Length)
                {
                    height = input[i].Length;
                }
            }

            Frame toReturn = new Frame(height, input.Length, 0, 0);

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    char c = input[i].ToCharArray()[j];
                    if(c != ' ')
                    {
                        toReturn.setChexel(j, i, new Chexel(f, b, c));
                    }
                }
            }

            return toReturn;
        }

        public static Frame Flatten(Frame top, Frame bottom)
        {
            int xMaxTop = top.width + top.xOffset; 
            int yMaxTop = top.height + top.yOffset;
            int xMaxBot = bottom.width + bottom.xOffset;
            int yMaxBot = bottom.height + bottom.yOffset;

            int xMax = xMaxTop > xMaxBot ? xMaxTop : xMaxBot;
            int yMax = yMaxTop > yMaxBot ? yMaxTop : yMaxBot;

            Frame toReturn = new Frame(xMax, yMax, 0, 0);

            for(int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    Chexel toSet = new Chexel();

                    if (top.hasChexel(x, y))
                    {
                        toSet = top.getChexel(x, y);
                    }
                    else if (bottom.hasChexel(x,y))
                    {
                        toSet = bottom.getChexel(x, y);
                    }

                    toReturn.setChexel(x, y, toSet);
                }
            }

            return toReturn;
        }

        public bool active()
        {
            return isActive;
        }
    }
}