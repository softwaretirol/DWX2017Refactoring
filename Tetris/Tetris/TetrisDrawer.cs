using System;

namespace Tetris
{
    internal class TetrisDrawer
    {
        public const string Sqr = "■";
        public void Draw(int[,] droppedtetrominoeLocationGrid, int[,] grid)
        {
            for (var i = 0; i < 23; ++i)
            {
                for (var j = 0; j < 10; j++)
                {
                    Console.SetCursorPosition(1 + 2 * j, i);
                    if ((grid[i, j] == 1) | (droppedtetrominoeLocationGrid[i, j] == 1))
                    {
                        Console.SetCursorPosition(1 + 2 * j, i);
                        Console.Write(Sqr);
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
            }
        }
    }
}