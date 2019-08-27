using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CRT
{
    public struct Light
    {
        public Vector3 position;
        public float intensity;

        public Light(Vector3 position, float intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }
    }

    public struct Material
    {
        public static Material ivory =     new Material(1,    new Vector4(0.6f, 0.3f, 0.1f, 0.0f),  new Vector3(0.4f, 0.4f, 0.3f), 50);
        public static Material glass =     new Material(1.5f, new Vector4(0.0f, 0.5f, 0.1f, 0.8f),  new Vector3(0.6f, 0.7f, 0.8f), 125);
        public static Material redRubber = new Material(1,    new Vector4(0.9f, 0.1f, 0.0f, 0.0f),  new Vector3(0.3f, 0.1f, 0.1f), 10);
        public static Material mirror =    new Material(1,    new Vector4(0.0f, 10.0f, 0.8f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), 1425);

        public float refractiveIndex;
        public Vector4 albedo;
        public Vector3 diffuseColor;
        public float specularExponent;

        public Material(float refractiveIndex, Vector4 albedo, Vector3 diffuseColor, float specularExponent)
        {
            this.refractiveIndex = refractiveIndex;
            this.albedo = albedo;
            this.diffuseColor = diffuseColor;
            this.specularExponent = specularExponent;
        }
    }

    public struct Sphere
    {
        public Vector3 center;
        public float radius;
        public Material material;

        public Sphere(Vector3 center, float radius, Material material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        public bool rayIntersect(Vector3 orig, Vector3 dir, ref float t0)
        {
            Vector3 L = orig - center;
            float radius2 = (radius * radius);

            float a = Vector3.Dot(dir, dir);
            float b = 2 * Vector3.Dot(dir, L);
            float c = Vector3.Dot(L, L) - radius2;
            float t1 = 0;

            if(!Utils.SolveQuadradic(a, b, c, ref t0, ref t1))
            {
                return false;
            }

            if (t0 < 0)
            {
                //set t0 to t1
                t0 = t1;

                if (t0 < 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
