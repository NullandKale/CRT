using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.SolarSystem
{
    public class SolarSystem
    {
        List<Planet> planets;

        public void Init()
        {
            for (int i = 0; i < 5; i++)
            {
                Planet newPlanet = new Planet(i);
                planets.Add(newPlanet);
            }
        }

        public void Update()
        {
            for (int i = 0; i < 5; i++)
            {

            }
        }
    }
}
