using CRT.IOW;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine.Components
{
    public class CameraComponent : Component
    {
        Entity e;
        public void loadString(string toLoad)
        {

        }

        public string saveString()
        {
            return " ";
        }

        public void start(Entity e)
        {
            this.e = e;
        }

        public void stop()
        {

        }

        public void update()
        {
            Program.rayTracer.camera.update(Program.rayTracer);
        }

        public void updateBegin()
        {

        }

        public void updateEnd()
        {

        }
    }
}
