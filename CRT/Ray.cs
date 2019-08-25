namespace CRT
{
    public class Ray
    {
        public Vec3 origin;
        public Vec3 direction;

        public Ray(Vec3 ori, Vec3 dir)
        {
            this.origin = ori;
            this.direction = dir;
        }

        public Vec3 get_point(double t)
        {
            return origin + direction * t;
        }
        public Vec3 reflect_by(Vec3 normal)
        {
            return direction - normal * normal.dot(direction) * 2;
        }
    }
}