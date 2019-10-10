using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public class FireShader : ChexelProvider
    {
        public int width;
        public int height;
        public Frame frame;
        public bool isActive = true;

        private char[] fireChars =
        {
            '$',
            '@',
            ')',
            ')',
            '(',
            '(',
            '+',
        };

        private ConsoleColor[] colors =
        {
            ConsoleColor.DarkYellow,
            ConsoleColor.DarkYellow,
            ConsoleColor.Yellow,
            ConsoleColor.DarkYellow,
            ConsoleColor.DarkYellow,
            ConsoleColor.DarkYellow,
            ConsoleColor.Red,
            ConsoleColor.Red,
            ConsoleColor.Red,
            ConsoleColor.Red,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkRed,
            ConsoleColor.White,
        };

        private string[] mask =
        {
            "    ####    ",
            " ########## ",
            "############",
            "############",
            "  ########  ",
            "   ######   ",
            "    ####    ",
            "    ####    ",
            "     ##     ",
            "     ##     ",
        };

        public FireShader(Frame frame)
        {
            this.frame = frame;
            width = mask[0].Length;
            height = mask.Length;
        }
        
        public bool active()
        {
            return isActive;
        }

        public Chexel getChexel(int x, int y)
        {
            if(!hasChexel(x,y) || (mask[y].ToCharArray()[x] == ' '))
            {
                return new Chexel();
            }

            return getRandomChexel();
        }

        public Chexel getRandomChexel()
        {
            Chexel toReturn = new Chexel();
            toReturn.b = ConsoleColor.Black;
            toReturn.f = colors[Utils.rand(0, colors.Length)];
            toReturn.t = fireChars[Utils.rand(0, fireChars.Length)];
            return toReturn;
        }

        public bool hasChexel(int x, int y)
        {
            return (x >= 0 && x < width) && (y >= 0 && y < height);
        }
    }
}
