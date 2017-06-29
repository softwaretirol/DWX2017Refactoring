using System;

namespace Tetris
{
    internal class TetrisInputHandler
    {
        public void Input(Tetrominoe tet)
        {
            ConsoleKeyInfo key;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey();
            }
            else
            {
                return;
            }

            if ((key.Key == ConsoleKey.LeftArrow) & !tet.IsSomethingLeft())
            {
                for (var i = 0; i < 4; i++)
                {
                    tet.Location[i][1] -= 1;
                }
                tet.Update();
                //    Console.Beep();
            }
            else if ((key.Key == ConsoleKey.RightArrow) & !tet.IsSomethingRight())
            {
                for (var i = 0; i < 4; i++)
                {
                    tet.Location[i][1] += 1;
                }
                tet.Update();
            }

            if (key.Key == ConsoleKey.DownArrow)
            {
                tet.Drop();
            }

            if (key.Key == ConsoleKey.UpArrow)
            {
                for (; tet.IsSomethingBelow() != true;)
                {
                    tet.Drop();
                }
            }

            if (key.Key == ConsoleKey.Spacebar)
            {
                //rotate
                tet.Rotate();
                tet.Update();
            }
        }
    }
}