using System.Collections.Generic;

namespace CRT
{
    public class Scene
    {
        List<Shape> objects;
        List<Lightsource> lightsources;
        Color skyboxColor = new Color();
        Vec3 CameraPos = new Vec3(0, 0, -1000);
        //Camera camera;

        public Scene(Color skybox)
        {
            this.skyboxColor = skybox;
            objects = new List<Shape>();
            lightsources = new List<Lightsource>();
        }

        public void add(Shape o)
        {
            objects.Add(o);
        }

        public void add(Lightsource light)
        {
            lightsources.Add(light);
        }

        public Color Shading(Ray ray, Shape o, double t, int depth)
        {
            Vec3 intersect_point = ray.origin + ray.direction * t;
            Vec3 normal = o.get_normal(intersect_point);

            switch (o.texture)
            {
                case Texture.MAT:
                    {
                        return (o.color).scale_by(normal.dot(ray.direction) * 0.5);
                    }
                case Texture.REFLECTIVE:
                    {
                        Color c = (o.color).scale_by(normal.dot(ray.direction) * 0.5);
                        if (depth > 0)
                        {
                            c = c + trace_ray(new Ray(intersect_point, (ray.direction - normal * ray.direction.dot(normal) * 2).normalize()), o, depth - 1);
                        }
                        return c;
                    }
                case Texture.SPECULAR:
                    {
                        Color c = new Color(0, 0, 0);

                        foreach (Lightsource light in lightsources)
                        {
                            Vec3 light2pos = light.position - intersect_point;
                            //specular:
                            if (check_occlusion(intersect_point, light.position))
                            {
                                c = c + light.color.scale_by2(ray.reflect_by(normal).dot(light2pos.normalize()));
                                c = c + (o.color).mix_with(light.color).scale_by(light.intensity / (light.position - intersect_point).norm());
                            }
                        }
                        //reflections:
                        if (depth > 0)
                        {
                            c = c + trace_ray(new Ray(intersect_point, (ray.direction - normal * ray.direction.dot(normal) * 2).normalize()), o, depth - 1);
                        }
                        return c;

                    }
            }

            return skyboxColor;
        }

        public Color trace(int x, int y)
        {
            Vec3 ray_direction = new Vec3(x, y, 1250).normalize();

            return trace_ray(new Ray(CameraPos, ray_direction), null, 100);
        }
        public Color trace_ray(Ray ray, Shape exclude_obj, int depth)
        {
            double min_t = double.MaxValue;
            Shape nearest_obj = null;

            double t = double.MaxValue;
            foreach (Shape o in objects)
            {
                if (o.intersect(ray, ref t))
                {
                    if (min_t > t)
                    {
                        nearest_obj = o;

                        min_t = t;
                    }
                }
            }

            if (nearest_obj != null)
            {
                return Shading(ray, nearest_obj, min_t, depth);
            }

            return new Color(0, 0, 0);
        }

        public bool check_occlusion(Vec3 target, Vec3 source)
        {
            Vec3 toSource = source - target;
            double t_light = toSource.norm();
            Ray ray = new Ray(target, toSource * (1.0 / t_light));
            double min_t = t_light;
            Shape nearest_obj = null;
            double t = double.MaxValue;
            foreach (Shape o in objects)
            {
                if (o.intersect(ray, ref t))
                {
                    if (min_t > t)
                    {
                        nearest_obj = o;
                        min_t = t;
                    }
                }
            }
            return nearest_obj == null;
        }
    }

}