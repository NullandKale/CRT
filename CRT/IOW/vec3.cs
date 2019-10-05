using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRT
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

        public Vec3(Color c)
        {
            this.x = c.R / 255.0;
            this.y = c.B / 255.0;
            this.z = c.G / 255.0;
        }


        public double r()
        {
            return x;
        }


        public double g()
        {
            return y;
        }


        public double b()
        {
            return z;
        }

        public override string ToString()
        {
            return "{" + string.Format("{0:0.00}", x) + ", " + string.Format("{0:0.00}", y) + ", " + string.Format("{0:0.00}", z) + "}";
        }


        public static Vec3 operator -(Vec3 vec)
        {
            return new Vec3(-vec.x, -vec.y, -vec.z);
        }

        public double length()
        {
            return Math.Sqrt(lengthSquared());
        }


        public double lengthSquared()
        {
            return x * x + y * y + z * z;
        }

        public double getAt(int a)
        {
            switch(a)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
                default:
                    return 0;
            }
        }

        public static double dist(Vec3 v1, Vec3 v2)
        {
            double dx = v1.x - v2.x;
            double dy = v1.y - v2.y;
            double dz = v1.z - v2.z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static Vec3 operator +(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }


        public static Vec3 operator -(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }


        public static Vec3 operator *(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }


        public static Vec3 operator /(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }


        public static Vec3 operator *(Vec3 v1, double v)
        {
            return new Vec3(v1.x * v, v1.y * v, v1.z * v);
        }


        public static Vec3 operator *(double v, Vec3 v1)
        {
            return new Vec3(v1.x * v, v1.y * v, v1.z * v);
        }


        public static Vec3 operator /(Vec3 v1, double v)
        {
            return new Vec3(v1.x / v, v1.y / v, v1.z / v);
        }


        public static double dot(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }


        public static Vec3 cross(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.y * v2.z - v1.z * v2.y,
                          -(v1.x * v2.z - v1.z * v2.x),
                            v1.x * v2.y - v1.y * v2.x);
        }


        public static Vec3 unitVector(Vec3 v)
        {
            return v / v.length();
        }


        public static Vec3 reflect(Vec3 v, Vec3 n)
        {
            return v - 2 * dot(v, n) * n;
        }

        public static Vec3 refract(Vec3 v, Vec3 n, double niOverNt)
        {
            double cosi = Math.Clamp(-1, 1, Vec3.dot(v, n));
            double etai = 1;
            double etat = niOverNt;

            if (cosi < 0)
            {
                cosi = -cosi;
            }
            else
            {
                etat = 1;
                etai = niOverNt;
                n = -n;
            }

            double eta = etai / etat;
            double k = 1 - eta * eta * (1 - cosi * cosi);

            return k < 0 ? new Vec3(0, 0, 0) : eta * v + (eta * cosi - Math.Sqrt(k)) * n;
        }


        public static bool refract(Vec3 v, Vec3 n, double niOverNt, ref Vec3 refracted)
        {
            Vec3 uv = unitVector(v);
            double dt = dot(uv, n);
            double discriminant = 1.0 - niOverNt * niOverNt * (1 - dt * dt);

            if (discriminant > 0)
            {
                refracted = niOverNt * (uv - (n * dt)) - n * Math.Sqrt(discriminant);
                return true;
            }

            return false;
        }

        public static implicit operator Vector3(Vec3 d)
        {
            return new Vector3((float)d.x, (float)d.y, (float)d.z);
        }

        public static implicit operator Vec3(Vector3 d)
        {
            return new Vec3(d.X, d.Y, d.Z);
        }

        public static implicit operator Vector4(Vec3 d)
        {
            return new Vector4((float)d.x, (float)d.y, (float)d.z, 0);
        }

        public static implicit operator Vec3(Vector4 d)
        {
            return new Vec3(d.X, d.Y, d.Z);
        }
    }
}