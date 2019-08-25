using System;

namespace CRT
{
    public struct Vec3
    {
        public double X;
        public double Y;
        public double Z;
        public Vec3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public static Vec3 operator +(Vec3 vec0, Vec3 vec1)
        {
            return new Vec3(vec0.X + vec1.X, vec0.Y + vec1.Y, vec0.Z + vec1.Z);
        }
        public static Vec3 operator -(Vec3 vec0, Vec3 vec1)
        {
            return new Vec3(vec0.X - vec1.X, vec0.Y - vec1.Y, vec0.Z - vec1.Z);
        }
        public static Vec3 operator *(Vec3 vec0, double scalar)
        {
            return new Vec3(scalar * vec0.X, scalar * vec0.Y, scalar * vec0.Z);
        }
        public double dot(Vec3 vec)
        {
            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }
        public double norm()
        {
            return Math.Sqrt(dot(this));
        }
        public Vec3 normalize()
        {
            return this * (1 / norm());
        }
    }
}