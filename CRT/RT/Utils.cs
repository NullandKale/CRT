using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CRT.RT
{
    public static class Utils
    {
        public static Vector3 reflect(Vector3 I, Vector3 N)
        {
            return I - N * 2f * (I * N);
        }

        public static Vector3 refract(Vector3 I, Vector3 N, float eta_t, float eta_i = 1f)
        {
            float cosi = -Math.Max(-1f, Math.Min(1f, Vector3.Dot(I, N)));

            if (cosi < 0)
            {
                return refract(I, -N, eta_i, eta_t);
            }

            float eta = eta_i / eta_t;
            float k = 1 - eta * eta * (1 - cosi * cosi);

            return k < 0 ? new Vector3(1, 0, 0) : (I * eta) + N * (eta * cosi - (float)Math.Sqrt(k));
        }

        public static bool sceneIntersect(Vector3 orig, Vector3 dir, List<Sphere> spheres, ref Vector3 hit, ref Vector3 N, ref Material material)
        {
            float spheresDist = float.MaxValue;

            for(int i = 0; i < spheres.Count; i++)
            {
                float dist = 0;
                if(spheres[i].rayIntersect(orig, dir, ref dist) && dist < spheresDist)
                {
                    spheresDist = dist;
                    hit = orig + dir * dist;
                    N = Vector3.Normalize(hit - spheres[i].center);
                    material = spheres[i].material;
                }
            }

            float checkerboard_dist = float.MaxValue;
            int floorHeight = -4;
            if(Math.Abs(dir.Y) > 1e-3)
            {
                float d = -(orig.Y - floorHeight) / dir.Y;
                Vector3 pt = orig + dir * d;
                if(d > 0 && Math.Abs(pt.X) < 10 && pt.Z < -10 && pt.Z > -30 && d < spheresDist)
                {
                    checkerboard_dist = d;
                    hit = pt;
                    N = new Vector3(0, 1, 0);

                    material.diffuseColor = (((int)(.5 * hit.X + 1000) + ((int)(.5 * hit.Z))) & 1) == 1 ? new Vector3(0.3f, 0.3f, 0.3f) : new Vector3(0.3f, 0.2f, 0.1f);
                }
            }

            return Math.Min(spheresDist, checkerboard_dist) < 1000;
        }
    }
}
