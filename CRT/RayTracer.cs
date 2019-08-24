using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CRT
{
    public class RayTracer
    {
        public int height;
        public int width;
        public Scene scene;
        public Color[,] frameBuffer;

        public RayTracer(int height, int width)
        {
            this.height = height;
            this.width = width;
            scene = new Scene();
            frameBuffer = new Color[height, width];

            Sphere sphere0 = new Sphere(new Vec3(0, 10, 5), 5, new Color(255, 100, 40), Texture_t.SPECULAR);
            Lightsource light0 = new Lightsource(new Vec3(0, 0, 0), new Color(255, 255, 255));
            scene.add(sphere0);
            scene.add(light0);
        }

        public void GenerateFrame()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    frameBuffer[i,j] = scene.trace(i, j);
                }
            }
        }

        public Bitmap CreateBitmap(bool save)
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

            if(save)
            {
                bitmap.Save("bitmap.bmp");
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

                        if (frameBuffer[xPos, yPos].r > 0 || frameBuffer[xPos, yPos].g > 0 || frameBuffer[xPos, yPos].b > 0)
                        {
                            toSet = ConsoleColor.White;
                        }
                        //else
                        //{
                        //    if (xPos % 2 == 0 || yPos % 2 == 1)
                        //    {
                        //        toSet = ConsoleColor.DarkMagenta;
                        //    }
                        //    else
                        //    {
                        //        toSet = ConsoleColor.Black;
                        //    }
                        //}

                        if (toSet != current)
                        {
                            Console.Write((char[])line.ToArray());
                            line.Clear();
                            Console.BackgroundColor = toSet;
                            current = Console.BackgroundColor;
                        }
                    }
                    else
                    {

                    }

                    line.Add(toPrint);
                }

                Console.Write((char[])line.ToArray());
                Console.WriteLine();
            }
        }
    }

    public class Ray
    {
        public Vec3 origin;
        public Vec3 direction;

        public Ray(Vec3 ori, Vec3 dir)
        {
            this.origin = ori;
            this.direction = dir;
        }

        public Vec3 get_point(double t)
        {
            return origin + direction * t;
        }
        public Vec3 reflect_by(Vec3 normal)
        {
            return direction - normal * normal.dot(direction) * 2;
        }
    }

    public class Color
    {
        public double r, g, b;

        public Color(double red, double green, double blue)
        {
            r = red;
            g = green;
            b = blue;
        }
        public Color()
        {
            r = 0;
            g = 0;
            b = 0;
        }

        public Color scale_by(double scalar)
        {
            return (scalar > 0) ? Color_trunc(scalar * r, scalar * g, scalar * b) : new Color(0, 0, 0);
        }

        public Color scale_by2(double scalar)
        {
            if (scalar < 0)
            {
                return new Color(0, 0, 0);
            }

            scalar *= scalar;

            return Color_trunc(scalar * r, scalar * g, scalar * b);
        }

        public Color mix_with(Color rhs)
        {
            return new Color(r * rhs.r, g * rhs.g, b * rhs.b);
        }

        public static Color operator *(Color c, double scalar)
        {
            return new Color(scalar * c.r, scalar * c.g, scalar * c.b);
        }

        public static Color operator +(Color a, Color rhs)
        {
            return Color_trunc(a.r + rhs.r, a.g + rhs.g, a.b + rhs.b);
        }

        public static double trunc(double val)
        {
            return (val > 255.0) ? 255.0 : val;
        }

        public static Color Color_trunc(double red, double green, double blue)
        {
            return new Color(trunc(red), trunc(green), trunc(blue));
        }
    }

    public struct Vec3
    {
        public double X;
        public double Y;
        public double Z;
        public Vec3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public static Vec3 operator +(Vec3 vec0, Vec3 vec1)
        {
            return new Vec3(vec0.X + vec1.X, vec0.Y + vec1.Y, vec0.Z + vec1.Z);
        }
        public static Vec3 operator -(Vec3 vec0, Vec3 vec1)
        {
            return new Vec3(vec0.X - vec1.X, vec0.Y - vec1.Y, vec0.Z - vec1.Z);
        }
        public static Vec3 operator *(Vec3 vec0, double scalar)
        {
            return new Vec3(scalar * vec0.X, scalar * vec0.Y, scalar * vec0.Z);
        }
        public double dot(Vec3 vec)
        {
            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }
        public double norm()
        {
            return Math.Sqrt(dot(this));
        }
        public Vec3 normalize()
        {
            return this * (1 / norm());
        }
    }

    public class Lightsource
    {
        public Vec3 position;
        public Color color;
        public double intensity = 100;
        public Lightsource(Vec3 position_, Color color_, double intensity_ = 100.0)
        {
            this.position = position_;
            this.color = color_;
            this.intensity = intensity_;
        }
    }

    public class Scene
    {
        List<Shape> objects;
        List<Lightsource> lightsources;
        //Camera camera;

        public Scene()
        {
            objects = new List<Shape>();
            lightsources = new List<Lightsource>();
        }

        public void add(Shape o)
        {
            objects.Add(o);
        }

        public void add(Lightsource light)
        {
            lightsources.Add(light);
        }

        public Color Shading(Ray ray, Shape o, double t, int depth)
        {
            Vec3 intersect_point = ray.origin + ray.direction * t;
            Vec3 normal = o.get_normal(intersect_point);

            switch (o.texture)
            {
                case Texture_t.MAT:
                    {
                        return (o.color).scale_by(normal.dot(ray.direction) * 0.5);
                    }
                case Texture_t.REFLECTIVE:
                    {
                        Color c = (o.color).scale_by(normal.dot(ray.direction) * 0.5);
                        if (depth > 0)
                        {
                            c = c + trace_ray(new Ray(intersect_point, (ray.direction - normal * ray.direction.dot(normal) * 2).normalize()), o, depth - 1);
                        }
                        return c;
                    }
                case Texture_t.SPECULAR:
                    {
                        Color c = new Color(0, 0, 0);

                        foreach (Lightsource light in lightsources)
                        {
                            Vec3 light2pos = light.position - intersect_point;
                            //specular:
                            if (check_occlusion(intersect_point, light.position))
                            {
                                c = c + light.color.scale_by2(ray.reflect_by(normal).dot(light2pos.normalize()));
                                c = c + (o.color).mix_with(light.color).scale_by(light.intensity / (light.position - intersect_point).norm());
                            }
                        }
                        //reflections:
                        if (depth > 0)
                        {
                            c = c + trace_ray(new Ray(intersect_point, (ray.direction - normal * ray.direction.dot(normal) * 2).normalize()), o, depth - 1);
                        }
                        return c;

                    }
            }

            return new Color(0, 0, 0);
        }

        public Color trace(int x, int y)
        {
            // This function works as the camera, translating pixels to rays
            Vec3 ray_origin = new Vec3(0, 0, -1000);
            Vec3 ray_direction = new Vec3(x, y, 1250).normalize();

            return trace_ray(new Ray(ray_origin, ray_direction), null, 50);
        }
        public Color trace_ray(Ray ray, Shape exclude_obj, int depth)
        {
            double min_t = double.MinValue;
            int min_i = -1;
            Shape nearest_obj = null;

            double t = double.MaxValue;
            foreach (Shape o in objects)
            {
                if (o.intersect(ray, t))
                {
                    if (min_t > t)
                    {
                        nearest_obj = o;

                        min_t = t;
                    }
                }
            }

            if (nearest_obj != null)
            {
                return Shading(ray, nearest_obj, min_t, depth);
            }

            return new Color(0, 0, 0);
        }

        public bool check_occlusion(Vec3 target, Vec3 source)
        {
            Vec3 toSource = source - target;
            double t_light = toSource.norm();
            Ray ray = new Ray(target, toSource * (1.0 / t_light));
            double min_t = t_light;
            Shape nearest_obj = null;
            double t = double.MaxValue;
            foreach (Shape o in objects)
            {
                if (o.intersect(ray, t))
                {
                    if (min_t > t)
                    {
                        nearest_obj = o;
                        min_t = t;
                    }
                }
            }
            return nearest_obj == null;
        }
    }
    public enum Texture_t { MAT, REFLECTIVE, SPECULAR };
    public abstract class Shape
    {
        public Color color;
        public Texture_t texture;
        public Shape(Color color_, Texture_t texture_)
        {
            color = color_;
            texture = texture_;
        }
        public abstract Vec3 get_normal(Vec3 p);
        public abstract bool intersect(Ray ray, double t);

        public static double SELF_AVOID_T = 1e-2;
    }

    public class Sphere : Shape
    {
        Vec3 Center;
        double Radius;

        public Sphere(Vec3 center, double radius, Color color, Texture_t texture = Texture_t.MAT) : base(color, texture)
        {
            this.Center = center;
            this.Radius = radius;
            base.color = color;
            base.texture = texture;
        }

        public Vec3 get_center()
        {
            return Center;
        }

        public override Vec3 get_normal(Vec3 p)
        {
            return ((p - Center) * (-1 / Radius)).normalize();// *(-1 / Radius);
        }

        public override bool intersect(Ray ray, double t)
        {
            Vec3 v = ray.origin - Center;

            double b = 2 * v.dot(ray.direction);
            double c = v.dot(v) - Radius * Radius;
            double delta = b * b - 4 * c;

            if (delta < 0)
            {
                t = float.MaxValue; // no intersection, at 'infinity'
                return false;
            }

            double t1 = (-b - Math.Sqrt(delta)) / 2;
            double t2 = (-b + Math.Sqrt(delta)) / 2;

            if (t2 < SELF_AVOID_T)
            { // the sphere is behind us
                t = float.MaxValue;
                return false;
            }

            t = (t1 >= SELF_AVOID_T) ? t1 : t2;

            return true;
        }
    }


    public class Cylinder : Shape
    {
        Vec3 center;
        Vec3 direction;
        double radius;
        double height;

        public Cylinder(Vec3 center_, Vec3 direction_, double radius_, double height_, Color color, Texture_t texture = Texture_t.MAT) : base(color, texture)
        {
            this.center = center_;
            this.direction = direction_;
            this.radius = radius_;
            this.height = height_;
        }

        public Vec3 get_center()
        {
            return center;
        }

        public override Vec3 get_normal(Vec3 p)
        {
            Vec3 to_center = p - center;
            return ((to_center - direction * (to_center.dot(direction))).normalize());
        }

        public override bool intersect(Ray ray, double t)
        {
            Vec3 rel_origin = ray.origin - center;

            double directions_dot = ray.direction.dot(direction);
            double a = 1 - directions_dot * directions_dot;
            double b = 2 * (rel_origin.dot(ray.direction) - directions_dot * rel_origin.dot(direction));
            double c = rel_origin.dot(rel_origin) - rel_origin.dot(direction) * rel_origin.dot(direction) - radius * radius;

            double delta = b * b - 4 * a * c;

            if (delta < 0)
            {
                t = float.MaxValue; // no intersection, at 'infinity'
                return false;
            }

            double sqrt_delta_2a = Math.Sqrt(delta) / (2 * a);
            double t1 = (-b) / (2 * a);
            double t2 = t1 + sqrt_delta_2a;
            t1 -= sqrt_delta_2a;

            if (t2 < SELF_AVOID_T)
            { // the cylinder is behind us
                t = float.MaxValue; // no intersection, at 'infinity'
                return false;
            }
            double center_proj = center.dot(direction);
            double t1_proj = ray.get_point(t1).dot(direction);
            if (t1 >= SELF_AVOID_T && t1_proj > center_proj && t1_proj < center_proj + height)
            {
                t = t1;
                return true;
            }
            double t2_proj = ray.get_point(t2).dot(direction);
            if (t2 >= SELF_AVOID_T && t2_proj > center_proj && t2_proj < center_proj + height)
            {
                t = t2;
                return true;
            }
            t = float.MaxValue; // no intersection, at 'infinity'
            return false;
        }
    }

    public class Plane : Shape
    {
        Vec3 center;
        Vec3 direction;

        public Plane(Vec3 center_, Vec3 direction_, Color color, Texture_t texture = Texture_t.MAT) : base(color, texture)
        {
            this.center = center_;
            this.direction = direction_;
        }

        public Vec3 get_center()
        {
            return center;
        }

        public override Vec3 get_normal(Vec3 p)
        {
            return direction;
        }

        public override bool intersect(Ray ray, double t)
        {
            double directions_dot_prod = direction.dot(ray.direction);
            if (directions_dot_prod == 0)
            {// the plane and ray are parallel
                t = float.MaxValue; // no intersection, at 'infinity'
                return false;
            }
            t = direction.dot(center - ray.origin) / directions_dot_prod;

            if (t < SELF_AVOID_T)
            { // the plane is behind the ray
                t = float.MaxValue;
                return false;
            }

            return true;
        }
    }

}