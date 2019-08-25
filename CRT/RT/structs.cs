using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CRT.RT
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
            Vector3 L = center - orig;
            float tca = Vector3.Dot(L, dir);
            float d2 = Vector3.Dot(L, L) - tca * tca;

            if (d2 > radius * radius)
            {
                return false;
            }

            float thc = (float)Math.Sqrt(radius * radius - d2);
            t0 = tca - thc;
            if(t0 < 0)
            {
                //set t0 to t1
                t0 = tca + thc;
                return false;
            }

            return true;
        }
    }
}
