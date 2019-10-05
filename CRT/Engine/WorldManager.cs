using CRT.IOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRT.Engine
{
    public class WorldManager
    {
        public Vec3 cameraPos;
        public Vec3 cameraLook;
        public bool cameraUpdate = false;

        public bool isStarted = false;

        public List<Entity> entities;
        public List<Entity> renderableEntities; 
        public double cullingDist;
        public WorldManager(double cullingDist)
        {
            this.cullingDist = cullingDist;
            entities = new List<Entity>();
        }

        public void addEntity(Entity toAdd)
        {
            entities.Add(toAdd);
            if (isStarted)
            {
                toAdd.start();
            }
            SortEntities();
        }

        public void removeEntity(Entity toRemove)
        {
            if (entities.Contains(toRemove))
            {
                entities.Remove(toRemove);
                toRemove.stop();
                SortEntities();
            }
        }

        public void start()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].start();
            }
            isStarted = true;
        }

        public void Update()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].updateBegin();
                if(entities[i].distUpdate)
                {
                    SortEntities();
                }
            }

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].update();
            }

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].updateEnd();
            }
        }

        private void SortEntities()
        {
            entities.Sort(new Comparison<Entity>(CompareByDist));
            renderableEntities = new List<Entity>(entities.Where(entity => (entity.dist <= cullingDist && entity.render)));
            Program.rayTracer.world.set(new List<Hitable>(entities.Select(entity => entity.sphere)));
        }

        //there may be a faster way to do this
        private int CompareByDist(Entity x, Entity y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    double xDist = getDist(x);
                    double yDist = getDist(y);

                    int retval = xDist.CompareTo(yDist);

                    if (retval != 0)
                    {
                        return retval;
                    }

                    return retval;
                }
            }
        }

        private double getDist(Entity e)
        {
            if(e.distUpdate)
            {
                e.dist = Vec3.dist(cameraPos, e.sphere.center);
                e.distUpdate = false;
            }

            return e.dist;
        }
    }
}
