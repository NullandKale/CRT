using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRT.IOW
{
    public struct Ray
    {
        public Vec3 a;
        public Vec3 b;

        public Ray(Vec3 a, Vec3 b)
        {
            this.a = a;
            this.b = b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vec3 pointAtParameter(double t)
        {
            return a + (t * b);
        }
    }
}
