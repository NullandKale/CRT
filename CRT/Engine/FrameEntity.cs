using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public class FrameEntity : ChexelEntity
    {
        public Frame frame;
        public FrameEntity(vector2 pos, ConsoleColor f, char t, Frame frame) : base(pos, f, t)
        {
        }
    }
}
