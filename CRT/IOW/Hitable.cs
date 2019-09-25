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
        bool hit(Ray r, double tMin, double tMax, ref HitRecord rec);
        bool boundingBox(ref aabb box);
    }

    public class bvh_node : Hitable
    {
        Hitable left;
        Hitable right;
        aabb box;

        public bvh_node(List<Hitable> hitables, int n)
        {
            int axis = (int)Utils.rand(0, 3);

            if(axis == 0)
            {
                hitables.Sort(0, n, Comparer<Hitable>.Create(boxXCompare));
            }
            else if (axis == 1)
            {
                hitables.Sort(0, n, Comparer<Hitable>.Create(boxYCompare));
            }
            else
            {
                hitables.Sort(0, n, Comparer<Hitable>.Create(boxZCompare));
            }

            if(n == 1)
            {
                left = hitables[0];
                right = hitables[0];
            }
            else if (n == 2)
            {
                left = hitables[0];
                right = hitables[1];
            }
            else
            {
                left = new bvh_node(hitables.GetRange(0, n / 2), n / 2);
                right = new bvh_node(hitables.GetRange(n / 2, n - n / 2), n - n / 2);
            }

            aabb boxLeft = new aabb();
            aabb boxRight = new aabb();

            if(!left.boundingBox(ref boxLeft) || !right.boundingBox(ref boxRight))
            {
                throw new ArgumentException("No Bounding box in boh_node constructor");
            }

            box = aabb.surrounding_box(boxLeft, boxRight);
        }

        private int boxXCompare(Hitable a, Hitable b)
        {
            aabb boxLeft = new aabb();
            aabb boxRight = new aabb();

            if (!a.boundingBox(ref boxLeft) || !b.boundingBox(ref boxRight))
            {
                throw new ArgumentException("");
            }

            if (boxLeft.min.x - boxRight.min.x < 0.0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private int boxYCompare(Hitable a, Hitable b)
        {
            aabb boxLeft = new aabb();
            aabb boxRight = new aabb();

            if (!a.boundingBox(ref boxLeft) || !b.boundingBox(ref boxRight))
            {
                throw new ArgumentException("");
            }

            if (boxLeft.min.y - boxRight.min.y < 0.0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private int boxZCompare(Hitable a, Hitable b)
        {
            aabb boxLeft = new aabb();
            aabb boxRight = new aabb();

            if (!a.boundingBox(ref boxLeft) || !b.boundingBox(ref boxRight))
            {
                throw new ArgumentException("");
            }

            if (boxLeft.min.z - boxRight.min.z < 0.0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public bool boundingBox(ref aabb box)
        {
            box = this.box;
            return true;
        }

        public bool hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            if(box.hit(r, tMin, tMax))
            {
                HitRecord leftRec = new HitRecord();
                HitRecord rightRec = new HitRecord();
                bool hit_left = left.hit(r, tMin, tMax, ref leftRec);
                bool hit_right = right.hit(r, tMin, tMax, ref rightRec);

                if(hit_left && hit_right)
                {
                    if(leftRec.t < rightRec.t)
                    {
                        rec = leftRec;
                    }
                    else
                    {
                        rec = rightRec;
                    }

                    return true;
                }
                else if(hit_left)
                {
                    rec = leftRec;
                    return true;
                }
                else if (hit_right)
                {
                    rec = rightRec;
                    return true;
                }
            }

            return false;
        }
    }

    public class HitableList : Hitable
    {
        public List<Hitable> hitables;
        public bvh_node bvh_root;
        
        private Hitable[] hitArray;
        public HitableList()
        {
            hitables = new List<Hitable>();
        }

        public void add(Hitable h)
        {
            hitables.Add(h);
            hitArray = hitables.ToArray();
            bvh_root = new bvh_node(hitables, hitables.Count);
        }

        public bool boundingBox(ref aabb box)
        {
            if(hitables.Count < 1)
            {
                return false;
            }

            aabb tempBox = new aabb();

            bool firstTrue = hitables[0].boundingBox(ref tempBox);

            if(!firstTrue)
            {
                return false;
            }
            else
            {
                box = tempBox;
                for(int i = 1; i < hitables.Count; i++)
                {
                    if(hitables[i].boundingBox(ref tempBox))
                    {
                        box = aabb.surrounding_box(box, tempBox);
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            return bvh_root.hit(r, tMin, tMax, ref rec);
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
    }
}
