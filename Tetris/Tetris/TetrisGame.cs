using System;
using System.Diagnostics;

namespace Tetris
{
    internal class TetrisGame
    {
        private readonly TetrisInputHandler inputHandler = new TetrisInputHandler();
        private readonly ScoreCalculator scoreCalculator = new ScoreCalculator();
        private readonly ScoreInformationDrawer scoreInformationDrawer = new ScoreInformationDrawer();

        public int[,] DroppedtetrominoeLocationGrid = new int[23, 10];
        public int DropTime, DropRate = 300;
        public Stopwatch DropTimer = new Stopwatch();
        public int[,] Grid = new int[23, 10];
        public Stopwatch InputTimer = new Stopwatch();
        public bool IsDropped;
        public int LinesCleared, Score, Level = 1;
        private Tetrominoe nexttet;
        private Tetrominoe tet;
        public Stopwatch Timer = new Stopwatch();

        private TetrisGame()
        {
        }

        public TetrisBorderDrawer BorderDrawer { get; } = new TetrisBorderDrawer();
        public TetrisDrawer TetrisDrawer { get; } = new TetrisDrawer();
        public static TetrisGame Instance { get; } = new TetrisGame();

        public bool StartAnotherGame()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over \n Replay? (Y/N)");
            var input = Console.ReadLine();
            return input == "y" || input == "Y";
        }

        public void StartGame()
        {
            DroppedtetrominoeLocationGrid = new int[23, 10];
            Timer = new Stopwatch();
            DropTimer = new Stopwatch();
            InputTimer = new Stopwatch();
            DropRate = 300;
            IsDropped = false;
            LinesCleared = 0;
            Score = 0;
            Level = 1;

            BorderDrawer.DrawBorder();
            Console.SetCursorPosition(4, 5);
            Console.WriteLine("Press any key");
            Console.ReadKey(true);
            Timer.Start();
            DropTimer.Start();
            scoreInformationDrawer.OutputScoreInfo(Level, Score, LinesCleared);
            nexttet = new Tetrominoe();
            tet = nexttet;
            tet.Spawn();
            nexttet = new Tetrominoe();

            Update();
            Console.Clear();
        }

        private void Update()
        {
            while (true) //Update Loop
            {
                DropTime = (int) DropTimer.ElapsedMilliseconds;
                if (DropTime > DropRate)
                {
                    DropTime = 0;
                    DropTimer.Restart();
                    tet.Drop();
                }
                if (IsDropped)
                {
                    tet = nexttet;
                    nexttet = new Tetrominoe();
                    tet.Spawn();

                    IsDropped = false;
                }
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (DroppedtetrominoeLocationGrid[0, j] == 1)
                    {
                        return;
                    }
                }

                inputHandler.Input(tet);
                ClearBlock();
            }
        }

        private void ClearBlock()
        {
            var combo = 0;
            for (var i = 0; i < 23; i++)
            {
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (DroppedtetrominoeLocationGrid[i, j] == 0)
                    {
                        break;
                    }
                }
                if (j == 10)
                {
                    LinesCleared++;
                    combo++;
                    for (j = 0; j < 10; j++)
                    {
                        DroppedtetrominoeLocationGrid[i, j] = 0;
                    }
                    var newdroppedtetrominoeLocationGrid = new int[23, 10];
                    for (var k = 1; k < i; k++)
                    {
                        for (var l = 0; l < 10; l++)
                        {
                            newdroppedtetrominoeLocationGrid[k + 1, l] = DroppedtetrominoeLocationGrid[k, l];
                        }
                    }
                    for (var k = 1; k < i; k++)
                    {
                        for (var l = 0; l < 10; l++)
                        {
                            DroppedtetrominoeLocationGrid[k, l] = 0;
                        }
                    }
                    for (var k = 0; k < 23; k++)
                    for (var l = 0; l < 10; l++)
                    {
                        if (newdroppedtetrominoeLocationGrid[k, l] == 1)
                        {
                            DroppedtetrominoeLocationGrid[k, l] = 1;
                        }
                    }
                    TetrisDrawer.Draw(DroppedtetrominoeLocationGrid, Grid);
                }
            }

            Score += scoreCalculator.CalculateScore(combo, Level);
            Level = scoreCalculator.CalculateLevel(LinesCleared);
            if (combo > 0)
            {
                scoreInformationDrawer.OutputScoreInfo(Level, Score, LinesCleared);
            }
            DropRate = scoreCalculator.CalculateDropRate(Level);
        }
    }
}