using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRT.IOW
{
    public struct HitRecord
    {
        public double t;
        public Vec3 p;
        public Vec3 normal;
        public MaterialData material;
    }
    public interface Hitable
    {
        Vec3 getCenter();
        void setCenter(Vec3 center);
        bool hit(Ray r, double tMin, double tMax, ref HitRecord rec);
        bool boundingBox(ref aabb box);
    }
}
