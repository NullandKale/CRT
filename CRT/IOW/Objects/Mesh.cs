using ObjLoader.Loader.Loaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.IOW
{
    public class Mesh : Hitable
    {
        LoadResult data;
        public Mesh(string objFileName)
        {
            this.data = Utils.LoadOBJ(objFileName);
        }

        public bool boundingBox(ref aabb box)
        {
            throw new NotImplementedException();
        }

        public bool hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            throw new NotImplementedException();
        }
    }
}
