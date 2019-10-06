using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine.ChexelComponents
{
    public class CockpitCComponent : ChexelComponent
    {
        bool filled = false;
        FrameEntity entity;
        vector2 pos = new vector2(6, 3);
        public void loadString(string toLoad)
        {

        }

        public string saveString()
        {
            return " ";
        }

        public void start(ChexelEntity e)
        {
            entity = (FrameEntity)e;
        }

        public void activateCockpit()
        {
            filled = true;
            entity.frame.useOffset = false;
            entity.frame.setChexel(pos.x, pos.y, new Chexel('@'));
            entity.frame.useOffset = true;
            CameraCComponent camera = new CameraCComponent();
            camera.controllable = false;
            camera.hasMoved = true;
            entity.AddComponent(camera);
        }

        public void goToSpace()
        {
            Program.menuManager.activate3D();
        }

        public void stop()
        {

        }

        public void update()
        {

        }

        public void updateBegin()
        {
            if (filled)
            {
                Program.uiManager.AddMessage(new Message((int)((1.0 * Program.frameManager.width) / 3.0), (int)((2.0 * Program.frameManager.height) / 3.0), "press e to launch.", ConsoleColor.White, ConsoleColor.Black), true);

                if(Program.input.IsKeyFalling(OpenTK.Input.Key.E))
                {
                    goToSpace();
                }
            }
        }

        public void updateEnd()
        {

        }
    }
}
