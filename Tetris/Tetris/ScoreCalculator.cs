using System;

namespace Tetris
{
    internal class ScoreCalculator
    {
        public int CalculateScore(int combo, int level)
        {
            if (combo == 1)
            {
                return 40 * level;
            }
            if (combo == 2)
            {
                return 100 * level;
            }
            if (combo == 3)
            {
                return 300 * level;
            }
            if (combo > 3)
            {
                return 300 * combo * level;
            }

            return 0;
        }

        public int CalculateDropRate(int level)
        {
            return 300 - 22 * level;
        }

        public int CalculateLevel(int linesCleared)
        {
            if (linesCleared >= 0)
            {
                if (linesCleared < 5)
                {
                    return 1;
                }
                if (linesCleared < 10)
                {
                    return 2;
                }
                if (linesCleared < 15)
                {
                    return 3;
                }
                if (linesCleared < 25)
                {
                    return 4;
                }
                if (linesCleared < 35)
                {
                    return 5;
                }
                if (linesCleared < 50)
                {
                    return 6;
                }
                if (linesCleared < 70)
                {
                    return 7;
                }
                if (linesCleared < 90)
                {
                    return 8;
                }
                if (linesCleared < 110)
                {
                    return 9;
                }
                if (linesCleared < 150)
                {
                    return 10;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(linesCleared));
        }
    }
}