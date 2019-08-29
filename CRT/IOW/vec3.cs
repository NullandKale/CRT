using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRT.IOW
{
    public struct Vec3
    {
        public double x;
        public double y;
        public double z;

        public Vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double r()
        {
            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double g()
        {
            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double b()
        {
            return z;
        }

        public override string ToString()
        {
            return "x:" + x + " y:" + y + " z:" + z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator -(Vec3 vec)
        {
            return new Vec3(-vec.x, -vec.y, -vec.z);
        }
        public double this[int i]
        {
            get
            {
                switch(i)
                {
                    case 0:
                        {
                            return x;
                        }
                    case 1:
                        {
                            return y;
                        }
                    case 2:
                        {
                            return z;
                        }
                    default:
                        {
                            return -1;
                        }
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        {
                            x = value;
                            break;
                        }
                    case 1:
                        {
                            y = value;
                            break;
                        }
                    case 2:
                        {
                            z = value;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double length()
        {
            return Math.Sqrt(lengthSquared());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double lengthSquared()
        {
            return x * x + y * y + z * z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void makeUnitVector()
        {
            double k = 1.0 / length();
            x *= k;
            y *= k;
            z *= k;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator +(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator -(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator *(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator /(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator *(Vec3 v1, double v)
        {
            return new Vec3(v1.x * v, v1.y * v, v1.z * v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator *(double v, Vec3 v1)
        {
            return new Vec3(v1.x * v, v1.y * v, v1.z * v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator /(Vec3 v1, double v)
        {
            return new Vec3(v1.x / v, v1.y / v, v1.z / v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double dot(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 cross(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.y * v2.z - v1.z * v2.y,
                          -(v1.x * v2.z - v1.z * v2.x),
                            v1.x * v2.y - v1.y * v2.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 unitVector(Vec3 v)
        {
            return v / v.length();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 reflect(Vec3 v, Vec3 n)
        {
            return v - 2 * dot(v, n) * n;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool refract(Vec3 v, Vec3 n, double niOverNt, ref Vec3 refracted)
        {
            Vec3 uv = unitVector(v);
            double dt = dot(uv, n);
            double discriminant = 1.0 - niOverNt * niOverNt * (1 - dt * dt);

            if(discriminant > 0)
            {
                refracted = niOverNt * (uv - (n * dt)) - n * Math.Sqrt(discriminant);
                return true;
            }

            return false;
        }
    }
}
