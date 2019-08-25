//using System;

//namespace CRT
//{
//    public abstract class Shape
//    {
//        public Color color;
//        public Texture texture;
//        public Shape(Color color_, Texture texture_)
//        {
//            color = color_;
//            texture = texture_;
//        }
//        public abstract Vec3 get_normal(Vec3 p);
//        public abstract bool intersect(Ray ray, ref double t);

//        public static double SELF_AVOID_T = 1e-2;
//    }
//    public class Sphere : Shape
//    {
//        Vec3 Center;
//        double Radius;

//        public Sphere(Vec3 center, double radius, Color color, Texture texture = Texture.MAT) : base(color, texture)
//        {
//            this.Center = center;
//            this.Radius = radius;
//            base.color = color;
//            base.texture = texture;
//        }

//        public Vec3 get_center()
//        {
//            return Center;
//        }

//        public override Vec3 get_normal(Vec3 p)
//        {
//            return ((p - Center) * (-1 / Radius)).normalize();// *(-1 / Radius);
//        }

//        public override bool intersect(Ray ray, ref double t)
//        {
//            Vec3 v = ray.origin - Center;

//            double b = 2 * v.dot(ray.direction);
//            double c = v.dot(v) - Radius * Radius;
//            double delta = b * b - 4 * c;

//            if (delta < 0)
//            {
//                t = float.MaxValue; // no intersection, at 'infinity'
//                return false;
//            }

//            double t1 = (-b - Math.Sqrt(delta)) / 2;
//            double t2 = (-b + Math.Sqrt(delta)) / 2;

//            if (t2 < SELF_AVOID_T)
//            { // the sphere is behind us
//                t = float.MaxValue;
//                return false;
//            }

//            t = (t1 >= SELF_AVOID_T) ? t1 : t2;

//            return true;
//        }
//    }


//    public class Cylinder : Shape
//    {
//        Vec3 center;
//        Vec3 direction;
//        double radius;
//        double height;

//        public Cylinder(Vec3 center_, Vec3 direction_, double radius_, double height_, Color color, Texture texture = Texture.MAT) : base(color, texture)
//        {
//            this.center = center_;
//            this.direction = direction_;
//            this.radius = radius_;
//            this.height = height_;
//        }

//        public Vec3 get_center()
//        {
//            return center;
//        }

//        public override Vec3 get_normal(Vec3 p)
//        {
//            Vec3 to_center = p - center;
//            return ((to_center - direction * (to_center.dot(direction))).normalize());
//        }

//        public override bool intersect(Ray ray, ref double t)
//        {
//            Vec3 rel_origin = ray.origin - center;

//            double directions_dot = ray.direction.dot(direction);
//            double a = 1 - directions_dot * directions_dot;
//            double b = 2 * (rel_origin.dot(ray.direction) - directions_dot * rel_origin.dot(direction));
//            double c = rel_origin.dot(rel_origin) - rel_origin.dot(direction) * rel_origin.dot(direction) - radius * radius;

//            double delta = b * b - 4 * a * c;

//            if (delta < 0)
//            {
//                t = float.MaxValue; // no intersection, at 'infinity'
//                return false;
//            }

//            double sqrt_delta_2a = Math.Sqrt(delta) / (2 * a);
//            double t1 = (-b) / (2 * a);
//            double t2 = t1 + sqrt_delta_2a;
//            t1 -= sqrt_delta_2a;

//            if (t2 < SELF_AVOID_T)
//            { // the cylinder is behind us
//                t = float.MaxValue; // no intersection, at 'infinity'
//                return false;
//            }
//            double center_proj = center.dot(direction);
//            double t1_proj = ray.get_point(t1).dot(direction);
//            if (t1 >= SELF_AVOID_T && t1_proj > center_proj && t1_proj < center_proj + height)
//            {
//                t = t1;
//                return true;
//            }
//            double t2_proj = ray.get_point(t2).dot(direction);
//            if (t2 >= SELF_AVOID_T && t2_proj > center_proj && t2_proj < center_proj + height)
//            {
//                t = t2;
//                return true;
//            }
//            t = float.MaxValue; // no intersection, at 'infinity'
//            return false;
//        }
//    }

//    public class Plane : Shape
//    {
//        Vec3 center;
//        Vec3 direction;

//        public Plane(Vec3 center_, Vec3 direction_, Color color, Texture texture = Texture.MAT) : base(color, texture)
//        {
//            this.center = center_;
//            this.direction = direction_;
//        }

//        public Vec3 get_center()
//        {
//            return center;
//        }

//        public override Vec3 get_normal(Vec3 p)
//        {
//            return direction;
//        }

//        public override bool intersect(Ray ray, ref double t)
//        {
//            double directions_dot_prod = direction.dot(ray.direction);
//            if (directions_dot_prod == 0)
//            {// the plane and ray are parallel
//                t = float.MaxValue; // no intersection, at 'infinity'
//                return false;
//            }
//            t = direction.dot(center - ray.origin) / directions_dot_prod;

//            if (t < SELF_AVOID_T)
//            { // the plane is behind the ray
//                t = float.MaxValue;
//                return false;
//            }

//            return true;
//        }
//    }
//    public enum Texture
//    {
//        MAT,
//        REFLECTIVE,
//        SPECULAR
//    }
//}