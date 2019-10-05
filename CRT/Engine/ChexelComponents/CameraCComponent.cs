using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine.ChexelComponents
{
    public class CameraCComponent : ChexelComponent
    {
        public ChexelEntity follow;
        public int waitFrames = 10;
        public int waitCounter = 0;
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
            Program.tileMapManager.centerOnCell(follow.pos);
        }

        public void stop()
        {

        }

        public void update()
        {

        }

        public void updateBegin()
        {
            if(waitCounter > waitFrames)
            {
                vector2 movement = new vector2();

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.W))
                {
                    movement.y--;
                }

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.S))
                {
                    movement.y++;
                }

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.A))
                {
                    movement.x--;
                }

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.D))
                {
                    movement.x++;
                }

                if (Program.tileMapManager.moveEntity(follow, new vector2(follow.pos.x + movement.x, follow.pos.y + movement.y)))
                {
                    Program.tileMapManager.centerOnCell(follow.pos);
                }
                else
                {
                    //Collided
                }
            }
            else
            {
                waitCounter++;
            }

            Program.uiManager.AddMessage(new Message(0, 1, follow.pos.toString(), ConsoleColor.White, ConsoleColor.Black), true);
        }

        public void updateEnd()
        {

        }
    }
}
