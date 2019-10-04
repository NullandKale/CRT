using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine.Components
{
    public class BounceComponent : Component
    {
        double minY = -2;
        double maxY = 1;
        double speed = Utils.rand() * 0.1;
        bool goingUp = true;
        Entity entity;

        public void loadString(string toLoad)
        {

        }

        public string saveString()
        {
            return "";
        }

        public void start(Entity e)
        {
            this.entity = e;
        }

        public void stop()
        {

        }

        public void update()
        {
            if(goingUp)
            {
                entity.Move(new Vec3(0, speed, 0));
                if(entity.sphere.center.y > maxY)
                {
                    goingUp = false;
                }
            }
            else
            {
                entity.Move(new Vec3(0, -speed, 0));
                if (entity.sphere.center.y < minY)
                {
                    goingUp = true;
                }
            }
        }

        public void updateBegin()
        {

        }

        public void updateEnd()
        {

        }
    }
}
