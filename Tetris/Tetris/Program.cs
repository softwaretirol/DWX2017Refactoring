using System;
using System.Text;

namespace Tetris
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            while (true)
            {
                TetrisGame.Instance.StartGame();

                if (!TetrisGame.Instance.StartAnotherGame())
                {
                    return;
                }
            }
        }

    }
}