using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.IOW.Objects
{
    public class Cube : Hitable
    {
        public Vec3 center;
        public MaterialData material;
        public aabb box;

        public Cube(Vec3 center, MaterialData material)
        {
            this.center = center;
            this.material = material;
            box = new aabb(center - new Vec3(0.5, 0.5, 0.5), center + new Vec3(0.5, 0.5, 0.5));
        }

        public bool boundingBox(ref aabb box)
        {
            box = this.box;
            return true;
        }

        public Vec3 getCenter()
        {
            return center;
        }

        public bool hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            double minV = (box.min.x - ray.a.x) / ray.b.x;
            double maxV = (box.max.x - ray.a.x) / ray.b.x;
            double t1 = Utils.max(minV, maxV);
            double t0 = Utils.min(minV, maxV);
            tMin = Utils.max(t0, tMin);
            tMax = Utils.min(t1, tMax);
            if (tMax <= tMin)
            {
                return false;
            }

            minV = (box.min.y - ray.a.y) / ray.b.y;
            maxV = (box.max.y - ray.a.y) / ray.b.y;
            t1 = Utils.max(minV, maxV);
            t0 = Utils.min(minV, maxV);
            tMin = Utils.max(t0, tMin);
            tMax = Utils.min(t1, tMax);
            if (tMax <= tMin)
            {
                return false;
            }

            minV = (box.min.z - ray.a.z) / ray.b.z;
            maxV = (box.max.z - ray.a.z) / ray.b.z;
            t1 = Utils.max(minV, maxV);
            t0 = Utils.min(minV, maxV);
            tMin = Utils.max(t0, tMin);
            tMax = Utils.min(t1, tMax);
            if (tMax <= tMin)
            {
                return false;
            }

            rec.t = tMin;
            rec.p = ray.pointAtParameter(rec.t);
            if(rec.p.x == box.min.x)
            {
                rec.normal = new Vec3(1, 0, 0);
            }

            if(rec.p.x == box.max.x)
            {
                rec.normal = new Vec3(-1, 0, 0);
            }

            if (rec.p.y == box.min.y)
            {
                rec.normal = new Vec3(0, 1, 0);
            }

            if (rec.p.y == box.max.y)
            {
                rec.normal = new Vec3(0, -1, 0);
            }

            if (rec.p.z == box.min.z)
            {
                rec.normal = new Vec3(0, 0, 1);
            }

            if (rec.p.z == box.max.z)
            {
                rec.normal = new Vec3(0, 0, -1);
            }
            rec.material = material;
            return true;
        }

        public void setCenter(Vec3 center)
        {
            this.center = center;
        }
    }
}
