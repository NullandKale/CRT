using CRT.IOW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRT
{
    public class RayTracer
    {
        public int height;
        public int width;
        public int superSample;
        public int maxDepth;

        public Vec3[,] frameBuffer;
        public HitableList world;
        public Camera camera;

        public Sphere red;
        public Sphere green;
        public Sphere blue;

        private Random rng = new Random();
        public RayTracer(int height, int width, int superSample, int maxDepth, int fov, bool consoleAspectFix)
        {
            this.height = height;
            this.width = width;
            this.superSample = superSample;
            this.maxDepth = maxDepth;

            frameBuffer = new Vec3[width, height];

            world = new HitableList();
            if (consoleAspectFix)
            {
                camera = new Camera(new Vec3(-1, 2, 0), new Vec3(0, 2, 0), new Vec3(0, 1, 0), fov, (double)width / (double)(height * 2));
            }
            else
            {
                camera = new Camera(new Vec3(-3, 2, 0), new Vec3(0, 2, 0), new Vec3(0, 1, 0), fov, (double)width / (double)height);
            }

            red   = new Sphere(new Vec3(0, rng.NextDouble() + 1, -1),     0.5, new MaterialData(material.Lambertian, new Vec3(1, 0, 0)));
            green = new Sphere(new Vec3(0, rng.NextDouble() + 1,  0),     0.5, new MaterialData(material.Lambertian, new Vec3(0, 1, 0)));
            blue  = new Sphere(new Vec3(0, rng.NextDouble() + 1,  1),     0.5, new MaterialData(material.Lambertian, new Vec3(0, 0, 1)));

            world.add(red);
            world.add(green);
            world.add(blue);

            world.add(new Sphere(new Vec3(0, -100000.5,  -1),  100000, new MaterialData(material.Lambertian, new Vec3( 0, 0, 0))));
            world.add(new Sphere(new Vec3(2,         3,   8),       6, new MaterialData(material.Metal, new Vec3( 1, 1, 1), 0)));
            world.add(new Sphere(new Vec3(2,         3,  -8),       6, new MaterialData(material.Metal, new Vec3( 1, 1, 1), 0)));
        }

        Vec3 color(Ray r, Hitable world, int depth)
        {
            HitRecord rec = new HitRecord();
            if (world.hit(r, 0.001, float.MaxValue, ref rec))
            {
                Ray scattered = new Ray();
                Vec3 attenuation = new Vec3();

                if(depth < maxDepth && Material.scatter(r, rec, ref attenuation, ref scattered, rec.material))
                {
                    return attenuation * color(scattered, world, depth + 1);
                }
                else
                {
                    return new Vec3(1, 1, 1);
                }
            }
            else
            {
                Vec3 unitDirection = Vec3.unitVector(r.b);
                double t = 0.5 * (unitDirection.y + 1.0);
                Vec3 col = (1.0 - t) * new Vec3(1, 1, 1) + (t * new Vec3(0.1, 0.1, 0.1));
                return col;
            }
        }
        public void GenerateFrame()
        {
            int flip = (height - 1);

            for (int x = width - 1; x >= 0; x--)
            {
                for (int y = 0; y < height; y++)
                {
                    Vec3 col = new Vec3();
                    double u;
                    double v;
                    Ray r;

                    for (int s = 0; s < superSample; s++)
                    {
                        for (int t = 0; t < superSample; t++)
                        {
                            u = ((double)x + ((double)s / (double)superSample)) / (double)width;
                            v = ((double)y + ((double)t / (double)superSample)) / (double)height;

                            r = camera.GetRay(u, v);
                            col += color(r, world, 0);
                        }
                    }

                    col /= (double)(superSample * superSample);
                    col = new Vec3(Math.Sqrt(col.x), Math.Sqrt(col.y), Math.Sqrt(col.z));
                    frameBuffer[x, flip - y] = col;
                }
            }
        }

        public void GenerateFrameParallel()
        {
            int flip = (height - 1);

            Parallel.For(0, height * width, (int i) =>
            {
                int x = i / height;
                int y = i % height;

                Vec3 col = new Vec3();
                double u;
                double v;
                Ray r;

                for (int s = 0; s < superSample; s++)
                {
                    for (int t = 0; t < superSample; t++)
                    {
                        u = ((double)x + ((double)s / (double)superSample)) / (double)width;
                        v = ((double)y + ((double)t / (double)superSample)) / (double)height;

                        r = camera.GetRay(u, v);
                        col += color(r, world, 0);
                    }
                }

                col /= (double)(superSample * superSample);
                col = new Vec3(Math.Sqrt(col.x), Math.Sqrt(col.y), Math.Sqrt(col.z));
                frameBuffer[x, flip - y] = col;
            });
        }

        bool dirR = true;
        bool dirG = true;
        bool dirB = true;

        public void update(bool parallel)
        {
            camera.update();

            if (parallel)
            {
                GenerateFrameParallel();
            }
            else
            {
                GenerateFrame();
            }

            if(red.center.y > 3)
            {
                dirR = false;
            }

            if (green.center.y > 3)
            {
                dirG = false;
            }

            if (blue.center.y > 3)
            {
                dirB = false;
            }

            if (red.center.y < 0)
            {
                dirR = true;
            }

            if (green.center.y < 0)
            {
                dirG = true;
            }

            if (blue.center.y < 0)
            {
                dirB = true;
            }

            double mult = 5;

            red.center.y += dirR ? rng.NextDouble() / mult : rng.NextDouble() / -mult;
            green.center.y += dirG ? rng.NextDouble() / mult : rng.NextDouble() / -mult;
            blue.center.y += dirB ? rng.NextDouble() / mult : rng.NextDouble() / -mult;

            if (Program.input.IsKeyRising(OpenTK.Input.Key.R))
            {
                world.hitables.Clear();

                world.add(red);
                world.add(green);
                world.add(blue);
                world.add(new Sphere(new Vec3(0, -100000.5, -1), 100000, new MaterialData(material.Lambertian, new Vec3(0, 0, 0))));
                world.add(new Sphere(new Vec3(2, 3, 8), 6, new MaterialData(material.Metal, new Vec3(1, 1, 1), 0)));
                world.add(new Sphere(new Vec3(2, 3, -8), 6, new MaterialData(material.Metal, new Vec3(1, 1, 1), 0)));

                for (int i = 0; i < 5; i++)
                {
                    world.add(new Sphere(new Vec3(Utils.rand(0, 6), Utils.rand(3, 8), Utils.rand(-5, 5)), Utils.rand(1, 1.5), new MaterialData(material.Lambertian, Utils.randomColor())));
                }
            }
        }

        public Bitmap CreateBitmap(bool save, bool open, bool parallel)
        {
            Bitmap bitmap = new Bitmap(width, height);

            update(parallel);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x,y, Utils.toRGB(frameBuffer[x,y]));
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

        public void Draw(bool parallel, bool greyScale)
        {
            update(parallel);

            Console.SetCursorPosition(0, 0);
            ConsoleColor current = Console.BackgroundColor;

            for (int y = 0; y < height; y++)
            {
                List<char> line = new List<char>();
                for (int x = 0; x < width; x++)
                {
                    int xPos = x;
                    int yPos = y;
                    char toPrint = ' ';

                    ConsoleColor toSet = ConsoleColor.Black;

                    if ((xPos >= 0 && xPos < width) && (yPos >= 0 && yPos < height))
                    {
                        if(greyScale)
                        {
                            toSet = Utils.FromColor(Utils.greyScale(frameBuffer[xPos, yPos]));
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