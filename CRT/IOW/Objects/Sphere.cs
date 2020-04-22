using System;

namespace CRT.IOW
{
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

        public bool boundingBox(ref aabb box)
        {
            box = new aabb(center - new Vec3(radius, radius, radius), center + new Vec3(radius, radius, radius));
            return true;
        }

        public Vec3 getCenter()
        {
            return center;
        }

        public void setCenter(Vec3 center)
        {
            this.center = center;
        }
    }
}
