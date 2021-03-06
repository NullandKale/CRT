﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine.ChexelComponents
{
    public class CameraCComponent : ChexelComponent
    {
        public ChexelEntity follow;
        public int waitFrames = 30;
        public int waitCounter = 0;
        public bool controllable = true;
        public bool hasMoved = false;
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
            if(!hasMoved)
            {
                Program.uiManager.AddMessage(new Message((int)((1.0 * Program.frameManager.width) / 3.0), (int)((2.0 * Program.frameManager.height) / 3.0), "press w to walk up.", ConsoleColor.White, ConsoleColor.Black), true);
            }

            if (waitCounter > waitFrames)
            {
                vector2 movement = new vector2();

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.W) && controllable)
                {
                    movement.y--;
                    hasMoved = true;
                }

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.S) && controllable)
                {
                    movement.y++;
                }

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.A) && controllable)
                {
                    movement.x--;
                }

                if (Program.input.IsKeyHeld(OpenTK.Input.Key.D) && controllable)
                {
                    movement.x++;
                }

                if (Program.tileMapManager.moveEntity(follow, new vector2(follow.pos.x + movement.x, follow.pos.y + movement.y)) && controllable)
                {
                    Program.tileMapManager.centerOnCell(follow.pos);
                }
                else
                {
                    (bool, FrameEntity) col = Program.tileMapManager.frameCollide(new vector2(follow.pos.x + movement.x, follow.pos.y + movement.y));
                    if(col.Item1)
                    {
                        CockpitCComponent cockpit = (CockpitCComponent)col.Item2.GetComponent(new CockpitCComponent().GetType().ToString());
                        cockpit.activateCockpit();
                        Program.tileMapManager.removeEntity(follow);
                    }
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
