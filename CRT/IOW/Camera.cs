using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.IOW
{
    public class Camera
    {
        public Vec3 lower_left_corner;
        public Vec3 horizontal;
        public Vec3 vertical;
        public Vec3 origin;

        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vup, double vfov, double aspect)
        {
            Vec3 u = new Vec3();
            Vec3 v = new Vec3();
            Vec3 w = new Vec3();


            double theta = vfov * Math.PI / 180;
            double half_height = Math.Tan(theta / 2.0);
            double half_width = aspect * half_height;
            origin = lookFrom;
            w = Vec3.unitVector(lookFrom - lookAt);
            u = Vec3.unitVector(Vec3.cross(vup, w));
            v = Vec3.cross(w, u);
            lower_left_corner = new Vec3(-half_width, -half_height, -1.0);
            lower_left_corner = origin - half_width * u - half_height * v - w;
            horizontal = 2 * half_width * u;
            vertical = 2 * half_height * v;
        }

        public Ray GetRay(double u, double v)
        {
            return new Ray(origin, lower_left_corner + (u * horizontal) + (v * vertical) - origin);
        }
    }
}
