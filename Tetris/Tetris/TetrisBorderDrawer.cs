using System;

namespace Tetris
{
    internal class TetrisBorderDrawer
    {
        public void DrawBorder()
        {
            for (var lengthCount = 0; lengthCount <= 22; ++lengthCount)
            {
                Console.SetCursorPosition(0, lengthCount);
                Console.Write("*");
                Console.SetCursorPosition(21, lengthCount);
                Console.Write("*");
            }
            Console.SetCursorPosition(0, 23);
            for (var widthCount = 0; widthCount <= 10; widthCount++)
            {
                Console.Write("*-");
            }
        }
    }
}