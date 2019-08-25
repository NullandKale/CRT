using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;

namespace CRT
{
    public class RayTracer
    {
        public int height;
        public int width;
        public int superSample;
        public Scene scene;
        public Color[,] frameBuffer;

        public Lightsource light0;
        public Sphere sphere0;

        public RayTracer(int height, int width, int superSample)
        {
            this.height = height;
            this.width = width;
            this.superSample = superSample > 1 ? superSample : 1;

            scene = new Scene(new Color(64, 64, 64));
            frameBuffer = new Color[height, width];

            light0 = new Lightsource(new Vec3(0, 0, 0), new Color(255, 255, 255));

            sphere0 = generateSphere(new Vec3(250,  0, 0), 150, Color.red);
            scene.add(generateSphere(new Vec3(250, 200, 0), 50, Color.green));
            scene.add(generateSphere(new Vec3(250, 300, 0), 50, Color.blue));

            scene.add(sphere0);
            scene.add(light0);
        }

        public Sphere generateSphere(Vec3 pos, double scale, Color color)
        {
            return new Sphere(new Vec3((double)height * (double)superSample * (pos.X / 500.0), (double)height * (double)superSample * (pos.Y / 500.0), (double)height * (double)superSample * (pos.Z / 500.0)), (double)height * (double)superSample * (scale / 500.0), color, Texture.SPECULAR);
        }

        public void GenerateFrame()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color c = scene.trace(i * superSample, j * superSample);

                    for (int x = 0; x < superSample; x++)
                    {
                        for (int y = 1; y < superSample; y++)
                        {
                            c.mix_with(scene.trace(i * superSample + x, j * superSample + y));
                        }
                    }

                    frameBuffer[i, j] = c;
                }
            }
        }

        public Bitmap CreateBitmap(bool save, bool open)
        {
            Bitmap bitmap = new Bitmap(width, height);
            GenerateFrame();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bitmap.SetPixel(i, j, System.Drawing.Color.FromArgb((int)frameBuffer[i, j].r, (int)frameBuffer[i, j].g, (int)frameBuffer[i, j].b));
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

        public void Draw(bool greyScale)
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
                        if(greyScale)
                        {
                            toSet = Utils.FromColor(frameBuffer[xPos, yPos].GreyScale());
                        }
                        else
                        {
                            toSet = Utils.FromColor(frameBuffer[xPos, yPos]);
                        }

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