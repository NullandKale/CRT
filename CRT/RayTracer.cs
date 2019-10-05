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
    public class RayTracer : ChexelProvider
    {
        public int height;
        public int width;
        public int superSample;
        public int maxDepth;
        public bool isActive = true;

        private Stopwatch timer = new Stopwatch();
        public TimeSpan updateTime;
        public TimeSpan renderTime;
        public TimeSpan drawTime;

        public Vec3 backgroundColor = new Vec3(0, 0, 0);
        public Vec3 depthColor = new Vec3(1, 1, 1);

        public Vec3[,] frameBuffer;
        public HitableList world;
        public List<Light> lights;
        public Camera camera;
        public double ambientLight = 0;

        private int pallet = 0;
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
                camera = new Camera(new Vec3(-2, 1, -2), new Vec3(1, 1, 1), new Vec3(0, 1, 0), fov, (double)width / (double)(height * 2));
            }
            else
            {
                camera = new Camera(new Vec3(-2, 1, -2), new Vec3(1, 1, 1), new Vec3(0, 1, 0), fov, (double)width / (double)height);
            }

            lights = new List<Light>();
            lights.Add(new Light(new Vec3(0, 10000, 0), 0.25));
        }

        Vec3 Color(Ray r, Hitable world, int depth)
        {
            HitRecord rec = new HitRecord();

            if (depth > maxDepth)
            {
                return depthColor;
            }

            if (!world.hit(r, 0.001, double.MaxValue, ref rec))
            {
                return backgroundColor;
            }

            Vec3 reflectDir;
            Vec3 reflectColor;
            Vec3 refractDir;
            Vec3 refractColor;

            reflectDir = Vec3.unitVector(Vec3.reflect(r.b, rec.normal));
            reflectColor = Color(new Ray(rec.p, reflectDir), world, depth + 1);

            refractDir = new Vec3();

            if (Vec3.refract(r.b, rec.normal, rec.material.ref_idx, ref refractDir))
            {
                refractDir = Vec3.unitVector(refractDir);
            }

            refractColor = Color(new Ray(rec.p, refractDir), world, depth + 1);

            double diffuseLightIntensity = ambientLight;
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

        public void Update(bool parallel)
        {
            timer.Restart();

            if(Program.input.IsKeyFalling(OpenTK.Input.Key.Number0))
            {
                pallet = 0;
            }

            if (Program.input.IsKeyFalling(OpenTK.Input.Key.Number1))
            {
                pallet = 1;
            }

            if (Program.input.IsKeyFalling(OpenTK.Input.Key.Number2))
            {
                pallet = 2;
            }

            timer.Stop();
            updateTime = timer.Elapsed;
        }

        public void Render(bool parallel)
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
        }

        public Bitmap CreateBitmap(string fileName, bool open, bool parallel)
        {
            Render(parallel);

            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, Utils.toRGB(frameBuffer[x, y]));
                }
            }

            bitmap.Save(fileName);

            Thread.Sleep(100);

            if (open)
            {
                Process.Start(@"cmd.exe ", @"/c " + fileName);
            }

            return bitmap;
        }

        public bool hasChexel(int x, int y)
        {
            return (x >= 0 && x < width) && (y >= 0 && y < height);
        }

        public (ConsoleColor f, ConsoleColor b, char t) getChexel(int x, int y)
        {
            return (ConsoleColor.White, Utils.FromColor(frameBuffer[x, y], pallet), ' ');
        }

        public bool active()
        {
            return isActive;
        }
    }
}