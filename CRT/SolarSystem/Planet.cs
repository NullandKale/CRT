using System;
using System.Collections.Generic;
using System.Text;
using CRT.Engine;
using CRT.Engine.Components;
using CRT.IOW;

namespace CRT.SolarSystem
{
    public class Planet
    {
        public Entity entity;
        private int index;
        private Terrain terrain;

        public Planet(int index)
        {
            this.index = index;
            this.terrain = Utils.rand() > 0.5 ? Terrain.Cavern : Terrain.Forest;

            this.entity = new Entity(new Vec3(((index + 1) * 15) + 15, 0, 0), 5);
            ((Sphere)this.entity.hit).material = new MaterialData(MaterialPrefab.rubber, GetColor(this.terrain));
            this.entity.AddComponent(new OrbitComponent());
        }

        public static Vec3 GetColor(Terrain terrain)
        {
            switch(terrain)
            {
                case Terrain.Cavern:
                    return new Vec3(0.5, 0.5, 0.5);
                case Terrain.Forest:
                    return new Vec3(0, 1, 0);
            }
            return new Vec3(0, 0, 0);
        }

    }

    public enum Terrain
    {
        Cavern = 0,
        Forest = 1,
    }
}
