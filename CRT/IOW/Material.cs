using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRT.IOW
{
    public struct MaterialData
    {
        public static MaterialData ivory       = new MaterialData(new Vec3(0.4, 0.4, 0.3), 0.6,  0.3, 0.1, 0.0,    50, 1.0);
        public static MaterialData glass       = new MaterialData(new Vec3(0.6, 0.7, 0.8), 0.0,  0.5, 0.1, 0.8,   125, 1.5);
        public static MaterialData blackRubber = new MaterialData(new Vec3(  0,   0,   0), 0.9,  0.1, 0.0, 0.0,    10, 1.0);
        public static MaterialData mirror      = new MaterialData(new Vec3(1.0, 1.0, 1.0), 0.0, 10.0, 0.8, 0.0,  1450, 1.0);

        public Vec3 diffuseColor;
        public double ref_idx;
        public double specularExponent;
        public double a0, a1, a2, a3;

        public MaterialData(Vec3 diffuseColor, double a0, double a1, double a2, double a3, double ref_idx, double specularExponent)
        {
            this.diffuseColor = diffuseColor;
            this.ref_idx = ref_idx;
            this.specularExponent = specularExponent;
            this.a0 = a0;
            this.a1 = a1;
            this.a2 = a2;
            this.a3 = a3;
        }

        public MaterialData(Vec3 diffuseColor)
        {
            this.diffuseColor = diffuseColor;
            this.ref_idx = 10;
            this.specularExponent = 1.0;
            this.a0 = 0.9;
            this.a1 = 0.1;
            this.a2 = 0.0;
            this.a3 = 0.0;
        }
    }
}
