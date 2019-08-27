using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CRT
{
    public static class RT
    {
        public static Vector3 reflect(Vector3 I, Vector3 N)
        {
            return I - N * 2f * (I * N);
        }

        public static float norm(Vector3 vector3)
        {
            return (float)Math.Sqrt(vector3.X * vector3.X + vector3.Y * vector3.Y + vector3.Z * vector3.Z);
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

        public static Vector3 castRay(Vector3 orig, Vector3 dir, List<Sphere> spheres, List<Light> lights, int depth = 0)
        {
            Vector3 point = new Vector3();
            Vector3 N = new Vector3();
            Material material = new Material();

            if(depth > 4 || !sceneIntersect(orig, dir, spheres, ref point, ref N, ref material))
            {
                return new Vector3(1f, 0.0f, 1f);
            }

            Vector3 reflectDir = Vector3.Normalize(reflect(dir, N));
            Vector3 refractDir = Vector3.Normalize(refract(dir, N, material.refractiveIndex));
            Vector3 reflectOrig = (reflectDir * N).LengthSquared() < 0 ? point - Vector3.Multiply(N, 1e-3f) : point + Vector3.Multiply(N, 1e-3f); // Length Squared Might be wrong
            Vector3 refractOrig = (refractDir * N).LengthSquared() < 0 ? point - Vector3.Multiply(N, 1e-3f) : point + Vector3.Multiply(N, 1e-3f); // Length Squared Might be wrong
            Vector3 reflectColor = castRay(reflectOrig, reflectDir, spheres, lights, depth + 1);
            Vector3 refractColor = castRay(refractOrig, refractDir, spheres, lights, depth + 1);

            float diffuseLightIntensity = 0;
            float specularLightIntensity = 0;

            for(int i = 0; i < lights.Count; i++)
            {
                Vector3 lightDir = Vector3.Normalize(lights[i].position - point);
                float lightDist = norm(lights[i].position - point);

                Vector3 shadowOrig = Vector3.Dot(lightDir, N) < 0 ? point - Vector3.Multiply(N, 1e-3f) : point - Vector3.Multiply(N, 1e-3f);
                Vector3 shadowPt = new Vector3();
                Vector3 shadowN = new Vector3();
                Material shadowMat = new Material();

                if(sceneIntersect(shadowOrig, lightDir, spheres, ref shadowPt, ref shadowN, ref shadowMat) && norm(shadowPt - shadowOrig) < lightDist)
                {
                    continue;
                }

                diffuseLightIntensity += lights[i].intensity * Math.Max(0f, Vector3.Dot(lightDir, N));
                specularLightIntensity += (float)Math.Pow(Math.Max(0f, Vector3.Dot(-reflect(-lightDir, N), dir)), material.specularExponent) * lights[i].intensity;
            }

            return material.diffuseColor * diffuseLightIntensity * material.albedo.W + new Vector3(1, 1, 1) * specularLightIntensity * material.albedo.X + reflectColor * material.albedo.Y + refractColor * material.albedo.Z;
        }

        public static void render(int width, int height, float fov, Vector3 cameraOrig, ref Vector3[,] frame, List<Sphere> spheres, List<Light> lights)
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    float dirx = (i + 0.5f) - width / 2f;
                    float diry = -(j + 0.5f) + height / 2f;
                    float dirz = -height / (2f * (float)Math.Tan(fov / 2));
                    frame[i, j] = castRay(cameraOrig, Vector3.Normalize(new Vector3(dirx, diry, dirz)), spheres, lights);
                }
            }
        }
    }
}
