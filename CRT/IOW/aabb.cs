using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.IOW
{
    public class aabb
    {
        public Vec3 min;
        public Vec3 max;

        public aabb()
        {
        }

        public aabb(Vec3 a, Vec3 b)
        {
            min = a;
            max = b;
        }

        public bool hit(Ray ray, double tMin, double tMax)
        {
            double minV = (min.x - ray.a.x) / ray.b.x;
            double maxV = (max.x - ray.a.x) / ray.b.x;
            double t1 = Utils.max(minV, maxV);
            double t0 = Utils.min(minV, maxV);
            tMin = Utils.max(t0, tMin);
            tMax = Utils.min(t1, tMax);
            if (tMax <= tMin)
            {
                return false;
            }

            minV = (min.y - ray.a.y) / ray.b.y;
            maxV = (max.y - ray.a.y) / ray.b.y;
            t1 = Utils.max(minV, maxV);
            t0 = Utils.min(minV, maxV);
            tMin = Utils.max(t0, tMin);
            tMax = Utils.min(t1, tMax);
            if (tMax <= tMin)
            {
                return false;
            }

            minV = (min.z - ray.a.z) / ray.b.z;
            maxV = (max.z - ray.a.z) / ray.b.z;
            t1 = Utils.max(minV, maxV);
            t0 = Utils.min(minV, maxV);
            tMin = Utils.max(t0, tMin);
            tMax = Utils.min(t1, tMax);
            if (tMax <= tMin)
            {
                return false;
            }

            return true;
        }

        public static aabb surrounding_box(aabb box0, aabb box1)
        {
            Vec3 small = new Vec3(Utils.min(box0.min.x, box1.min.x), Utils.min(box0.min.y, box1.min.y), Utils.min(box0.min.z, box1.min.z));
            Vec3 big = new Vec3(Utils.max(box0.max.x, box1.max.x), Utils.max(box0.max.y, box1.max.y),Utils.max(box0.max.z, box1.max.z));
            return new aabb(small, big);
        }
    }
}