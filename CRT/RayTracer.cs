using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Threading;

namespace CRT
{
    public class RayTracer
    {
        public int height;
        public int width;

        public Vector3[,] frameBuffer;
        public List<Sphere> spheres;
        public List<Light> lights;

        public RayTracer(int height, int width)
        {
            this.height = height;
            this.width = width;

            frameBuffer = new Vector3[height, width];
            spheres = new List<Sphere>();
            lights = new List<Light>();

            spheres.Add(new Sphere(new Vector3(  -3f,    0 , -16), 2, Material.ivory));
            spheres.Add(new Sphere(new Vector3(  -1f, -1.5f, -12), 2, Material.glass));
            spheres.Add(new Sphere(new Vector3(-1.5f, -0.5f, -18), 3, Material.redRubber));
            spheres.Add(new Sphere(new Vector3(   7f,    5 , -18), 4, Material.mirror));

            lights.Add(new Light(new Vector3(-20, 20,  20), 1.5f));
            lights.Add(new Light(new Vector3( 30, 50, -25), 1.8f));
            lights.Add(new Light(new Vector3( 30, 20,  30), 1.7f));
        }
        public void GenerateFrame()
        {
            RT.render(width, height, (float)(Math.PI / 3f), new Vector3(0, 0, 0), ref frameBuffer, spheres, lights);
        }

        public Bitmap CreateBitmap(bool save, bool open)
        {
            Bitmap bitmap = new Bitmap(width, height);
            GenerateFrame();

            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    bitmap.SetPixel(i, j, Utils.toRGB(frameBuffer[i,j]));
                }
            }

            if(save || open)
            {
                bitmap.Save("bitmap.bmp");
            }

            Thread.Sleep(100);

            if(open)
            {
                Process.Start(@"cmd.exe ", @"/c bitmap.bmp");
            }

            return bitmap;
        }

        public void Draw()
        {
            GenerateFrame();
            Console.SetCursorPosition(0, 0);
            ConsoleColor current = Console.BackgroundColor;

            for (int x = 0; x < height; x++)
            {
                List<char> line = new List<char>();

                for (int y = 0; y < width; y++)
                {
                    int xPos = x;
                    int yPos = y;
                    char toPrint = ' ';

                    ConsoleColor toSet = ConsoleColor.Black;

                    if ((xPos >= 0 && xPos < width) && (yPos >= 0 && yPos < height))
                    {
                        toSet = Utils.FromColor(frameBuffer[xPos, yPos]);

                        if (toSet != current)
                        {
                            Console.Write((char[])line.ToArray());
                            line.Clear();
                            Console.BackgroundColor = toSet;
                            current = Console.BackgroundColor;
                        }
                    }

                    line.Add(toPrint);
                }

                Console.WriteLine((char[])line.ToArray());
            }
        }
    }
}