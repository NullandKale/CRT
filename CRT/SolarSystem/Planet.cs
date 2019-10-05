using System;
using System.Collections.Generic;
using System.Text;
using CRT.Engine;
using CRT.IOW;

namespace CRT.SolarSystem
{
    public class Planet
    {
        private Entity e;
        private int index;
        private Terrain terrain;

        public Planet(int index)
        {
            this.index = index;
            this.terrain = Utils.rand() > 0.5 ? Terrain.Cavern : Terrain.Forest;

            this.e = new Entity(new Vec3(index * 20, 0, 0), 5);
            this.e.sphere.material = new MaterialData(MaterialPrefab.rubber, GetColor(this.terrain));
        }

        public static Vec3 GetColor(Terrain terrain)
        {
            switch(terrain)
            {
                case Terrain.Cavern:
                    return new Vec3(0.1, 0.1, 0.1);
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
