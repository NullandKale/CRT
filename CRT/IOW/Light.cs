using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.IOW
{
    public struct Light
    {
        public Vec3 position;
        public double intensity;

        public Light(Vec3 position, double intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }
    }
}
