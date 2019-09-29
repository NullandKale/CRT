using System;
using System.Collections.Generic;

namespace CRT.IOW
{
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
                else
                {
                    if (hit_left)
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
            }

            return false;
        }
    }
}
