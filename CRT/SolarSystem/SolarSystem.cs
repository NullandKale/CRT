using CRT.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.SolarSystem
{
    public class SolarSystem
    {
        public Planet[] planets;
        public int numPlanets;

        public SolarSystem(int numPlanets)
        {
            this.numPlanets = numPlanets;
            planets = new Planet[numPlanets];
            for (int i = 0; i < numPlanets; i++)
            {
                planets[i] = new Planet(i);
            }
        }
    }
}
