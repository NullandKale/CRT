using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRT.IOW
{
    public interface Material
    {
        bool scatter(Ray r_in, HitRecord rec, ref Vec3 attenuation, ref Ray scattered);
    }

    public class Lambertian : Material
    {
        public Vec3 albedo;

        public Lambertian(Vec3 a)
        {
            albedo = a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool scatter(Ray r_in, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            Vec3 target = rec.p + rec.normal + Utils.randomInUnitSphere();
            scattered = new Ray(rec.p, target - rec.p);
            attenuation = albedo;
            return true;
        }
    }

    public class Metal : Material
    {
        public Vec3 albedo;
        public double fuzz;

        public Metal(Vec3 a, double fuzz)
        {
            albedo = a;
            this.fuzz = fuzz;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool scatter(Ray r_in, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            Vec3 reflected = Vec3.reflect(Vec3.unitVector(r_in.b), rec.normal);
            scattered = new Ray(rec.p, reflected + (fuzz * Utils.randomInUnitSphere()));
            attenuation = albedo;
            return Vec3.dot(scattered.b, rec.normal) > 0;
        }
    }

    //public class Dielectric : Material
    //{
    //    public double ref_idx;

    //    public Dielectric(double ref_idx)
    //    {
    //        this.ref_idx = ref_idx;
    //    }

    //    public bool scatter(Ray r_in, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
    //    {
    //        Vec3 outward_normal = new Vec3();
    //        Vec3 reflected = Vec3.reflect(r_in.b, rec.normal);
    //        double niOverNt = 0;
    //        attenuation = new Vec3(1, 1, 1);
    //        Vec3 refracted = new Vec3();
    //        double reflect_prob = 0;
    //        double cosine = 0;

    //        if(Vec3.dot(r_in.b, rec.normal) > 0)
    //        {
    //            outward_normal = -rec.normal;
    //            niOverNt = ref_idx;
    //            cosine = ref_idx * Vec3.dot(r_in.b, rec.normal) / r_in.b.length();
    //        }
    //        else
    //        {
    //            outward_normal = rec.normal;
    //            niOverNt = 1.0 / ref_idx;
    //            cosine = -Vec3.dot(r_in.b, rec.normal) / r_in.b.length();
    //        }

    //        if (Vec3.refract(r_in.b, outward_normal, niOverNt, ref refracted))
    //        {
    //            reflect_prob = schlick(cosine, ref_idx);
    //        }
    //        else
    //        {
    //            reflect_prob = 1.0;
    //        }

    //        if(Utils.rand() > reflect_prob)
    //        {
    //            scattered = new Ray(rec.p, refracted);
    //        }
    //        else
    //        {
    //            scattered = new Ray(rec.p, reflected);
    //        }

    //        return true;
    //    }

    //    private double schlick(double cosine, double ref_idx)
    //    {
    //        double r0 = (1 - ref_idx) / (1 + ref_idx);
    //        r0 *= r0;
    //        return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
    //    }
    //}

}
