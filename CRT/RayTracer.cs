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
        public List<Light> lights;
        public Camera camera;

        public Sphere red;
        public Sphere green;
        public Sphere blue;

        private Random rng = new Random();
        private bool occlusionFix = false;
        public RayTracer(int height, int width, int superSample, int maxDepth, int fov, bool consoleAspectFix, bool occlusionFix)
        {
            this.height = height;
            this.width = width;
            this.superSample = superSample;
            this.maxDepth = maxDepth;
            this.occlusionFix = occlusionFix;

            frameBuffer = new Vec3[width, height];

            world = new HitableList();

            if (consoleAspectFix)
            {
                camera = new Camera(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 0), fov, (double)width / (double)(height * 2));
            }
            else
            {
                camera = new Camera(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 0), fov, (double)width / (double)height);
            }

            lights = new List<Light>();
            lights.Add(new Light(new Vec3(-20, 20,  12), 0.07));
            lights.Add(new Light(new Vec3( 20, 50, -17), 0.15));
            lights.Add(new Light(new Vec3( 30, 20,  22), 0.07));

            red = new Sphere(new Vec3(0, rng.NextDouble() + 1, -1), 0.5, MaterialData.redRubber);
            green = new Sphere(new Vec3(0, rng.NextDouble() + 1, 0), 0.5, MaterialData.greenRubber);
            blue = new Sphere(new Vec3(0, rng.NextDouble() + 1, 1), 0.5, MaterialData.blueRubber);

            world.add(new Sphere(new Vec3(  -6, 1,  8),  0.5, MaterialData.ivory));
            world.add(new Sphere(new Vec3(  -5, 2,  8),  0.5, MaterialData.mirror));
            world.add(new Sphere(new Vec3(-4.5, 1, 11), 0.75, MaterialData.redRubber));
            world.add(new Sphere(new Vec3(  -4, 3, 10),  1.5, MaterialData.glass));

            world.add(new Sphere(new Vec3(0, 3, -4), 2, MaterialData.mirror));
            world.add(red);
            world.add(green);
            world.add(blue);
            world.add(new Sphere(new Vec3(0, 3, 4), 2, MaterialData.mirror));

            world.add(new Sphere(new Vec3(-7, -1000000, -18), 1000000, MaterialData.greenRubber));
        }

        Vec3 Color(Ray r, Hitable world, int depth)
        {
            HitRecord rec = new HitRecord();

            if (depth > maxDepth)
            {
                return new Vec3(0.0, 0.5, 1.0);
            }

            if (!world.hit(r, 0.001, double.MaxValue, ref rec))
            {
                return new Vec3(0.0, 0.5, 1.0);
            }

            Vec3 reflectDir;
            Vec3 reflectOrig;
            Vec3 reflectColor;
            Vec3 refractDir;
            Vec3 refractOrig;
            Vec3 refractColor;

            if (occlusionFix)
            {
                reflectDir = Vec3.unitVector(Vec3.reflect(r.b, rec.normal));
                reflectOrig = Vec3.dot(reflectDir, rec.normal) < 0 ? rec.p - rec.normal * 1e-3 : rec.p + rec.normal * 1e-3;
                reflectColor = Color(new Ray(reflectOrig, reflectDir), world, depth + 1);
                refractDir = Vec3.refract(r.b, rec.normal, rec.material.ref_idx);
                refractOrig = Vec3.dot(refractDir, rec.normal) < 0 ? rec.p - rec.normal * 1e-3 : rec.p + rec.normal * 1e-3;
                refractColor = Color(new Ray(refractOrig, refractDir), world, depth + 1);
            }
            else
            {
                reflectDir = Vec3.unitVector(Vec3.reflect(r.b, rec.normal));
                reflectColor = Color(new Ray(rec.p, reflectDir), world, depth + 1);

                refractDir = new Vec3();

                if (Vec3.refract(r.b, rec.normal, rec.material.ref_idx, ref refractDir))
                {
                    refractDir = Vec3.unitVector(refractDir);
                }
                refractColor = Color(new Ray(rec.p, refractDir), world, depth + 1);
            }

            double diffuseLightIntensity = 0.05;
            double specularLightIntensity = 0;

            for(int i = 0; i < lights.Count; i++)
            {
                Vec3 lightDir = Vec3.unitVector(lights[i].position - rec.p);
                double lightDist = (lights[i].position - rec.p).length();
                Vec3 shadowOrig = rec.p;
                HitRecord shadowRec = new HitRecord();

                if(world.hit(new Ray(shadowOrig, lightDir), 0.001, double.MaxValue, ref shadowRec) && (shadowRec.p - shadowOrig).length() < lightDist)
                {
                    continue;
                }

                diffuseLightIntensity += lights[i].intensity * Math.Max(0.0, Vec3.dot(lightDir, rec.normal));
                specularLightIntensity += Math.Pow(Math.Max(0.0, Vec3.dot(-Vec3.reflect(-lightDir, rec.normal), r.b)), rec.material.specularExponent) * lights[i].intensity;
            }

            return rec.material.diffuseColor * diffuseLightIntensity * rec.material.a0 + new Vec3(1, 1, 1) * specularLightIntensity * rec.material.a1 + reflectColor * rec.material.a2 + refractColor * rec.material.a3;
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
                            col += Color(r, world, 0);
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
                        col += Color(r, world, 0);
                    }
                }

                col /= (double)(superSample * superSample);
                col = new Vec3(Math.Sqrt(col.x), Math.Sqrt(col.y), Math.Sqrt(col.z));
                frameBuffer[x, flip - y] = col;
            });
        }

        private bool dirR = true;
        private bool dirG = true;
        private bool dirB = true;
        private Stopwatch timer = new Stopwatch();

        public void update(bool parallel)
        {
            timer.Restart();
            camera.update(this);

            timer.Stop();
            updateTime = timer.Elapsed;

            timer.Restart();

            if (parallel)
            {
                GenerateFrameParallel();
            }
            else
            {
                GenerateFrame();
            }

            timer.Stop();
            renderTime = timer.Elapsed;

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

            if (Program.input.IsKeyFalling(OpenTK.Input.Key.R))
            {
                for (int i = 0; i < 5; i++)
                {
                    double height = Utils.rand(0, 5);
                    world.add(new Sphere(new Vec3(Utils.rand(-10, 10), height, Utils.rand(-10, 10)), height * 0.5, new MaterialData(MaterialPrefab.rubber, Utils.randomColor())));
                }
            }
        }

        public Bitmap CreateBitmap(string fileName, bool open, bool parallel)
        {
            timer.Restart();

            if (parallel)
            {
                GenerateFrameParallel();
            }
            else
            {
                GenerateFrame();
            }

            timer.Stop();
            renderTime = timer.Elapsed;

            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x,y, Utils.toRGB(frameBuffer[x,y]));
                }
            }

            bitmap.Save(fileName);

            Thread.Sleep(100);

            if(open)
            {
                Process.Start(@"cmd.exe ", @"/c " + fileName);
            }

            return bitmap;
        }

        public TimeSpan updateTime;
        public TimeSpan renderTime;
        public TimeSpan drawTime;

        public void Draw(bool greyScale)
        {
            timer.Restart();

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

            timer.Stop();
            drawTime = timer.Elapsed;
        }
    }
}