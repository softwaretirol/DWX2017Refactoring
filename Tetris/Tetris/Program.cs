using System;
using System.Diagnostics;
using System.Text;

namespace Tetris
{
    internal static class Program
    {
        public static string Sqr = "■";
        public static int[,] Grid = new int[23, 10];
        public static int[,] DroppedtetrominoeLocationGrid = new int[23, 10];
        public static Stopwatch Timer = new Stopwatch();
        public static Stopwatch DropTimer = new Stopwatch();
        public static Stopwatch InputTimer = new Stopwatch();
        public static int DropTime, DropRate = 300;
        public static bool IsDropped;
        private static Tetrominoe _tet;
        private static Tetrominoe _nexttet;
        public static ConsoleKeyInfo Key;
        public static bool IsKeyPressed;
        public static int LinesCleared, Score, Level = 1;

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            DrawBorder();
            Console.SetCursorPosition(4, 5);
            Console.WriteLine("Press any key");
            Console.ReadKey(true);
            Timer.Start();
            DropTimer.Start();
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("Level " + Level);
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("Score " + Score);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine("LinesCleared " + LinesCleared);
            _nexttet = new Tetrominoe();
            _tet = _nexttet;
            _tet.Spawn();
            _nexttet = new Tetrominoe();

            Update();

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over \n Replay? (Y/N)");
            var input = Console.ReadLine();

            if (input == "y" || input == "Y")
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
                GC.Collect();
                Console.Clear();
                Main();
            }
        }

        private static void Update()
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
            } //end Update
        }

        private static void ClearBlock()
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
                    Draw();
                }
            }
            if (combo == 1)
            {
                Score += 40 * Level;
            }
            else if (combo == 2)
            {
                Score += 100 * Level;
            }
            else if (combo == 3)
            {
                Score += 300 * Level;
            }
            else if (combo > 3)
            {
                Score += 300 * combo * Level;
            }

            if (LinesCleared < 5)
            {
                Level = 1;
            }
            else if (LinesCleared < 10)
            {
                Level = 2;
            }
            else if (LinesCleared < 15)
            {
                Level = 3;
            }
            else if (LinesCleared < 25)
            {
                Level = 4;
            }
            else if (LinesCleared < 35)
            {
                Level = 5;
            }
            else if (LinesCleared < 50)
            {
                Level = 6;
            }
            else if (LinesCleared < 70)
            {
                Level = 7;
            }
            else if (LinesCleared < 90)
            {
                Level = 8;
            }
            else if (LinesCleared < 110)
            {
                Level = 9;
            }
            else if (LinesCleared < 150)
            {
                Level = 10;
            }


            if (combo > 0)
            {
                Console.SetCursorPosition(25, 0);
                Console.WriteLine("Level " + Level);
                Console.SetCursorPosition(25, 1);
                Console.WriteLine("Score " + Score);
                Console.SetCursorPosition(25, 2);
                Console.WriteLine("LinesCleared " + LinesCleared);
            }

            DropRate = 300 - 22 * Level;
        }

        private static void Input()
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

        public static void Draw()
        {
            for (var i = 0; i < 23; ++i)
            {
                for (var j = 0; j < 10; j++)
                {
                    Console.SetCursorPosition(1 + 2 * j, i);
                    if ((Grid[i, j] == 1) | (DroppedtetrominoeLocationGrid[i, j] == 1))
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

        public static void DrawBorder()
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