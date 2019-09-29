using System.Collections.Generic;

namespace CRT.IOW
{
    public class HitableList : Hitable
    {
        public List<Hitable> hitables;
        //public bvh_node bvh_root;
        
        private Hitable[] hitArray;
        public HitableList()
        {
            hitables = new List<Hitable>();
        }

        public void add(Hitable h)
        {
            hitables.Add(h);
            hitArray = hitables.ToArray();
            //bvh_root = new bvh_node(hitables, hitables.Count);
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
            //return hitBVH(r, tMin, tMax, ref rec);
        }

        //public bool hitBVH(Ray r, double tMin, double tMax, ref HitRecord rec)
        //{
        //    return bvh_root.hit(r, tMin, tMax, ref rec);
        //}
    }
}
