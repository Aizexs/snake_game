using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_game
{
    public readonly struct Pixel
    {
        private const char PixelChar = '█';
        public Pixel(int x, int y, ConsoleColor color) : this()
        {
            X = x;
            Y = y;
            Color = color;
        }

        public int X { get; }


        public int Y { get; }

        public ConsoleColor Color { get; }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(PixelChar);

            Console.ForegroundColor = Color;
        }

        public void Clear()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(' ');
        }
    }
}
