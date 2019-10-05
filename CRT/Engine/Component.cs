using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public interface Component
    {
        void start(Entity e);
        void updateBegin();
        void update();
        void updateEnd();
        void stop();

        string saveString();
        void loadString(string toLoad);
    }

    public interface ChexelComponent
    {
        void start(ChexelEntity e);
        void updateBegin();
        void update();
        void updateEnd();
        void stop();

        string saveString();
        void loadString(string toLoad);
    }
}
