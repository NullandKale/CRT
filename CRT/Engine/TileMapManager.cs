using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace CRT.Engine
{
    public class TileMapManager : ChexelProvider
    {
        public bool isActive = false;
        Frame tileMap;

        public TileMapManager()
        {
            generatePlanet(0);
        }

        public bool active()
        {
            return isActive;
        }

        public void Update()
        {

        }

        public (ConsoleColor f, ConsoleColor b, char t) getChexel(int x, int y)
        {
            return tileMap.getChexel(x,y);
        }

        public bool hasChexel(int x, int y)
        {
            return tileMap.hasChexel(x,y);
        }

        public void generatePlanet(int number)
        {
            //tileMap = TileMapGenerator.FromBitmap("./Assets/GreyMap.bmp");
            tileMap = TileMapGenerator.FromBitmap("./Assets/TroyMap.bmp");
        }
    }

    public static class TileMapGenerator
    {
        public static Frame FromBitmap(string path)
        {
            if(File.Exists(path))
            {
                Bitmap b = new Bitmap(path);
                Frame toReturn = new Frame(b.Width, b.Height, 0, 0);

                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        toReturn.setChexel(i, j, (ConsoleColor.White, Utils.FromColor(new Vec3(b.GetPixel(i, j)), 2), ' '));
                    }
                }

                return toReturn;
            }

            return default;
        }
    }
}
