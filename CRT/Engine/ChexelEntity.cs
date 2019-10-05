using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public class ChexelEntity
    {
        public int xPos;
        public int yPos;
        public bool isActive = true;
        public ConsoleColor f;
        public char t;

        public ChexelEntity(int xPos, int yPos, ConsoleColor f, char t)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.f = f;
            this.t = t;
        }
    }
}
