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

        public bool hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            double txmin = (box.min.x - r.a.x) / r.b.x;
            double txmax = (box.max.x - r.a.x) / r.b.x;

            double tymin = (box.min.y - r.a.y) / r.b.y;
            double tymax = (box.max.y - r.a.y) / r.b.y;

            if(txmin > txmax)
            {
                double temp = txmax;
                txmax = txmin;
                txmin = temp;
            }

            if (tymin > tymax)
            {
                double temp = tymax;
                tymax = tymin;
                tymin = temp;
            }

            return false;
        }
    }
}
