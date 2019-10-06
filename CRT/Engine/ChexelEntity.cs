using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public class ChexelEntity
    {
        public vector2 pos;
        public bool started = false;
        public bool isActive = true;
        public ConsoleColor f;
        public char t;

        public Dictionary<string, ChexelComponent> components;

        public ChexelEntity(vector2 pos, ConsoleColor f, char t)
        {
            this.pos = pos;
            this.f = f;
            this.t = t;
            components = new Dictionary<string, ChexelComponent>();
        }

        public ChexelComponent GetComponent(string TypeString)
        {
            if (components.ContainsKey(TypeString))
            {
                return components[TypeString];
            }

            return null;
        }
        public void AddComponent(ChexelComponent c)
        {
            if (!components.ContainsValue(c))
            {
                components.Add(c.GetType().ToString(), c);
                if (started)
                {
                    c.start(this);
                }
            }
        }

        public void start()
        {
            foreach (ChexelComponent c in components.Values)
            {
                c.start(this);
            }
            started = true;
        }

        public void stop()
        {
            foreach (ChexelComponent c in components.Values)
            {
                c.stop();
            }
        }

        public void updateBegin()
        {
            foreach (ChexelComponent c in components.Values)
            {
                c.updateBegin();
            }
        }

        public void update()
        {
            foreach (ChexelComponent c in components.Values)
            {
                c.update();
            }
        }

        public void updateEnd()
        {
            foreach (ChexelComponent c in components.Values)
            {
                c.updateEnd();
            }
        }
    }
}
