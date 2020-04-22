using CRT.IOW;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public class Entity
    {
        public bool started = false;
        public bool distUpdate = true;
        public double dist;

        public Hitable hit;
        public bool render;
        public Dictionary<string, Component> components;

        public Entity(Vec3 loc, double radius)
        {
            render = true;
            components = new Dictionary<string, Component>();

            if(Utils.rand() > 0.25)
            {
                hit = new Sphere(loc, radius, new MaterialData(MaterialPrefab.ivory, Utils.randomColor()));
            }
            else
            {
                hit = new Sphere(loc, radius, MaterialData.mirror);
            }
        }

        public Entity(Hitable hitable)
        {
            render = true;
            components = new Dictionary<string, Component>();
            hit = hitable;
        }

        public Component GetComponent(string TypeString)
        {
            if(components.ContainsKey(TypeString))
            {
                return components[TypeString];
            }

            return null;
        }

        public void AddComponent(Component c)
        {
            if(!components.ContainsValue(c))
            {
                components.Add(c.GetType().ToString(), c);
                if(started)
                {
                    c.start(this);
                }
            }
        }

        public void Move(Vec3 delta)
        {
            //TODO collision maybe use bounding box list dictionary trick
            ((Sphere)hit).center += delta;
            distUpdate = true;
        }

        public void start()
        {
            foreach(Component c in components.Values)
            {
                c.start(this);
            }
            started = true;
        }

        public void stop()
        {
            foreach (Component c in components.Values)
            {
                c.stop();
            }
        }

        public void updateBegin()
        {
            foreach (Component c in components.Values)
            {
                c.updateBegin();
            }
        }

        public void update()
        {
            foreach (Component c in components.Values)
            {
                c.update();
            }
        }

        public void updateEnd()
        {
            foreach (Component c in components.Values)
            {
                c.updateEnd();
            }
        }
    }
}
