using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine.ChexelComponents
{
    public class FollowEndCComponent : ChexelComponent
    {
        public FrameEntity target;
        public vector2 targetOldPos;
        public ChexelEntity follow;

        public FollowEndCComponent(FrameEntity target)
        {
            this.target = target;
        }

        public void loadString(string toLoad)
        {

        }

        public string saveString()
        {
            return " ";
        }

        public void start(ChexelEntity e)
        {
            follow = e;
        }

        public bool updateTargetPos()
        {
            vector2 newPos = new vector2(target.pos.x, target.pos.y + target.frame.height);
            if(targetOldPos.x != newPos.x && targetOldPos.y != newPos.y)
            {
                targetOldPos = newPos;
                return true;
            }

            return false;
        }

        public void updatePos()
        {
            if(updateTargetPos())
            {
                follow.pos = targetOldPos;
            }
        }

        public void stop()
        {

        }

        public void update()
        {
            updatePos();
        }

        public void updateBegin()
        {

        }

        public void updateEnd()
        {

        }
    }
}
