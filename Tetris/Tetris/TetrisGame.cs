using System;
using System.Diagnostics;

namespace Tetris
{
    internal class TetrisGame
    {
        private Tetrominoe _nexttet;
        private Tetrominoe _tet;
        public int[,] DroppedtetrominoeLocationGrid = new int[23, 10];
        public int DropTime, DropRate = 300;
        public Stopwatch DropTimer = new Stopwatch();
        public int[,] Grid = new int[23, 10];
        public Stopwatch InputTimer = new Stopwatch();
        public bool IsDropped;
        public bool IsKeyPressed;
        public ConsoleKeyInfo Key;
        public int LinesCleared, Score, Level = 1;
        public Stopwatch Timer = new Stopwatch();

        private TetrisGame()
        {
        }

        public TetrisBorderDrawer BorderDrawer { get; } = new TetrisBorderDrawer();
        public ScoreInformationDrawer ScoreInformationDrawer { get; } = new ScoreInformationDrawer();
        public ScoreCalculator ScoreCalculator { get; } = new ScoreCalculator();
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
            IsKeyPressed = false;
            LinesCleared = 0;
            Score = 0;
            Level = 1;

            BorderDrawer.DrawBorder();
            Console.SetCursorPosition(4, 5);
            Console.WriteLine("Press any key");
            Console.ReadKey(true);
            Timer.Start();
            DropTimer.Start();
            ScoreInformationDrawer.OutputScoreInfo(Level, Score, LinesCleared);
            _nexttet = new Tetrominoe();
            _tet = _nexttet;
            _tet.Spawn();
            _nexttet = new Tetrominoe();

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
                    _tet.Drop();
                }
                if (IsDropped)
                {
                    _tet = _nexttet;
                    _nexttet = new Tetrominoe();
                    _tet.Spawn();

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

                Input();
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

            Score += ScoreCalculator.CalculateScore(combo, Level);
            Level = ScoreCalculator.CalculateLevel(LinesCleared);
            UpdateCombo(combo);
            DropRate = ScoreCalculator.CalculateDropRate(Level);
        }



        private void UpdateCombo(int combo)
        {
            if (combo > 0)
            {
                Console.SetCursorPosition(25, 0);
                Console.WriteLine("Level " + Level);
                Console.SetCursorPosition(25, 1);
                Console.WriteLine("Score " + Score);
                Console.SetCursorPosition(25, 2);
                Console.WriteLine("LinesCleared " + LinesCleared);
            }
        }


        private void Input()
        {
            if (Console.KeyAvailable)
            {
                Key = Console.ReadKey();
                IsKeyPressed = true;
            }
            else
            {
                IsKeyPressed = false;
            }

            if ((Key.Key == ConsoleKey.LeftArrow) & !_tet.IsSomethingLeft() & IsKeyPressed)
            {
                for (var i = 0; i < 4; i++)
                {
                    _tet.Location[i][1] -= 1;
                }
                _tet.Update();
                //    Console.Beep();
            }
            else if ((Key.Key == ConsoleKey.RightArrow) & !_tet.IsSomethingRight() & IsKeyPressed)
            {
                for (var i = 0; i < 4; i++)
                {
                    _tet.Location[i][1] += 1;
                }
                _tet.Update();
            }
            if ((Key.Key == ConsoleKey.DownArrow) & IsKeyPressed)
            {
                _tet.Drop();
            }
            if ((Key.Key == ConsoleKey.UpArrow) & IsKeyPressed)
            {
                for (; _tet.IsSomethingBelow() != true;)
                {
                    _tet.Drop();
                }
            }
            if ((Key.Key == ConsoleKey.Spacebar) & IsKeyPressed)
            {
                //rotate
                _tet.Rotate();
                _tet.Update();
            }
        }

    }
}