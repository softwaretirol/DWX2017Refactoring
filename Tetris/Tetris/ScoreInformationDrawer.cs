using System;

namespace Tetris
{
    internal class ScoreInformationDrawer
    {
        public void OutputScoreInfo(int level, int score, int linesCleared)
        {
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("Level " + level);
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("Score " + score);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine("LinesCleared " + linesCleared);
        }

    }
}