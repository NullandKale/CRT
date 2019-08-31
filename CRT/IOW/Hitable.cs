using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRT.IOW
{
    public struct HitRecord
    {
        public double t;
        public Vec3 p;
        public Vec3 normal;
        public MaterialData material;
    }

    public interface Hitable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool hit(Ray r, double tMin, double tMax, ref HitRecord rec);
    }

    public class HitableList : Hitable
    {
        public List<Hitable> hitables;
        private Hitable[] hitArray;
        public HitableList()
        {
            hitables = new List<Hitable>();
        }

        public void add(Hitable h)
        {
            hitables.Add(h);
            hitArray = hitables.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord tempRec = new HitRecord();
            bool hitAnything = false;
            double closestSoFar = tMax;

            for (int i = 0; i < hitArray.Length; i++)
            {
                if (hitArray[i].hit(r, tMin, closestSoFar, ref tempRec))
                {
                    hitAnything = true;
                    closestSoFar = tempRec.t;
                    rec = tempRec;
                }
            }

            return hitAnything;
        }
    }


    public class Sphere : Hitable
    {
        public Vec3 center;
        public double radius;
        private double radiusSquared;
        public MaterialData material;

        public Sphere(Vec3 center, double radius, MaterialData material)
        {
            this.center = center;
            this.radius = radius;
            radiusSquared = radius * radius;
            this.material = material;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            Vec3 oc = r.a - center;

            double a = Vec3.dot(r.b, r.b);
            double b = Vec3.dot(oc, r.b);
            double c = Vec3.dot(oc, oc) - radiusSquared;
            double discr = (b * b) - (a * c);

            if(discr > 0)
            {
                double sqrtdisc = Math.Sqrt(discr);
                double temp = (-b - sqrtdisc) / a;
                if(temp < tMax && temp > tMin)
                {
                    rec.t = temp;
                    rec.p = r.pointAtParameter(rec.t);
                    rec.normal = (rec.p - center) / radius;
                    rec.material = material;
                    return true;
                }
                temp = (-b + sqrtdisc) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.t = temp;
                    rec.p = r.pointAtParameter(rec.t);
                    rec.normal = (rec.p - center) / radius;
                    rec.material = material;
                    return true;
                }
            }
            return false;
        }
    }
}
