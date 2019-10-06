using CRT.SolarSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine.Components
{
    public class OrbitComponent : Component
    {
        Entity entity;
        double distance;
        double angleDeg;
        double angleRad;
        double speed;
        public void loadString(string toLoad)
        {
            
        }

        public string saveString()
        {
            return " ";
        }

        public void start(Entity e)
        {
            this.entity = e;
            angleDeg = Utils.rand(0, 360);
            speed = Utils.rand(0.01, 0.05);
            distance = e.sphere.center.x;
        }

        public void stop()
        {
            
        }

        public void update()
        {
            double currentX = entity.sphere.center.x;
            double currentZ = entity.sphere.center.z;
            double newX = (Math.Cos(angleRad) * distance);
            double newZ = (Math.Sin(angleRad) * distance);
            entity.Move(new Vec3(newX - currentX, 0, newZ - currentZ));
        }

        public void updateBegin()
        {
            angleDeg = (angleDeg + speed) % (double)360;
            // Converts the angle into radians
            angleRad = angleDeg * (Math.PI / (double)180);
        }

        public void updateEnd()
        {
            
        }
    }
}
