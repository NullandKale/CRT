using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public class InventoryManager
    {
        private int fuel;
        private int food;
        private int water;
        private int oxygen;

        public void ResetToZero()
        {
            fuel = 0;
            food = 0;
            water = 0;
            oxygen = 0;
        }

        public int AddFuel(int amount)
        {
            fuel += amount;
            return fuel;
        }

        public int RemoveFuel(int amount)
        {
            fuel -= amount;
            if (fuel < 0)
            {
                fuel = 0;
            }
            return fuel;
        }

        public int GetFuel()
        {
            return fuel;
        }

        public int AddFood(int amount)
        {
            food += amount;
            return food;
        }

        public int RemoveFood(int amount)
        {
            food -= amount;
            if (food < 0)
            {
                food = 0;
            }
            return food;
        }

        public int GetFood()
        {
            return food;
        }

        public int AddWater(int amount)
        {
            water += amount;
            return water;
        }

        public int RemoveWater(int amount)
        {
            water -= amount;
            if (water < 0)
            {
                water = 0;
            }
            return water;
        }

        public int GetWater()
        {
            return water;
        }

        public int AddOxygen(int amount)
        {
            oxygen += amount;
            return oxygen;
        }

        public int RemoveOxygen(int amount)
        {
            oxygen -= amount;
            if (oxygen < 0)
            {
                oxygen = 0;
            }
            return oxygen;
        }

        public int GetOxygen()
        {
            return oxygen;
        }
    }
}
