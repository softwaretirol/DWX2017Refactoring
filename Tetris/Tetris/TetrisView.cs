using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tetris
{
    public class TetrisView
    {
        private int[,] iTypeBrick = new int[1, 4] { { 1, 1, 1, 1 } };
        private int[,] oTypeBrick = new int[2, 2] { { 1, 1 }, { 1, 1 } };
        private int[,] tTypeBrick = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        private int[,] sTypeBrick = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        private int[,] zTypeBrick = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        private int[,] jTypeBrick = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        private int[,] lTypeBrick = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };

        private string sqr = "■";
        private int[,] grid = new int[23, 10];
        private int[,] droppedTetrisBricksLocationGrid = new int[23, 10];

        private int linesCleared = 0, score = 0, level = 1;
        private Stopwatch dropTimer = new Stopwatch();
        private int dropTime, dropRate = 300;
        private bool isKeyPressed;
        private bool isDropped;

        private TetrisBrick currentBrick;
        private TetrisBrick nextBrick;
        private ConsoleKeyInfo key;

        private bool isErect = false;

        public List<int[]> location = new List<int[]>();

        private List<int[,]> tetrisBrickTypes = new List<int[,]>();

        public TetrisView()
        {
            tetrisBrickTypes.AddRange(new List<int[,]>() { iTypeBrick, oTypeBrick, tTypeBrick, sTypeBrick, zTypeBrick, jTypeBrick, lTypeBrick });
        }

        public void SetConsoleOutputEncoding()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public void DrawBorder()
        {
            for (int lengthCount = 0; lengthCount <= 22; ++lengthCount)
            {
                Console.SetCursorPosition(0, lengthCount);
                Console.Write("*");
                Console.SetCursorPosition(21, lengthCount);
                Console.Write("*");
            }
            Console.SetCursorPosition(0, 23);
            for (int widthCount = 0; widthCount <= 10; widthCount++)
            {
                Console.Write("*-");
            }
        }

        public void AskForRestartGame()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over \n Replay? (Y/N)");
        }

        public void WriteTitles()
        {
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("Level " + level);
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("Score " + score);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine("LinesCleared " + linesCleared);
        }

        public void Draw()
        {
            for (int i = 0; i < 23; ++i)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.SetCursorPosition(1 + 2 * j, i);
                    if (grid[i, j] == 1 | droppedTetrisBricksLocationGrid[i, j] == 1)
                    {
                        Console.SetCursorPosition(1 + 2 * j, i);
                        Console.Write(sqr);
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }

            }
        }

        public void ReadKeyInput()
        {
            string input = Console.ReadLine();

            if (input == "y" || input == "Y")
            {
                droppedTetrisBricksLocationGrid = new int[23, 10];
                dropTimer = new Stopwatch();
                dropRate = 300;
                isDropped = false;
                isKeyPressed = false;
                linesCleared = 0;
                score = 0;
                level = 1;
                GC.Collect();
                Console.Clear();
            }
            else return;
        }

        public void StartDropTimer()
        {
            dropTimer.Start();
        }

        public void AskForKeyInput()
        {
            Console.SetCursorPosition(4, 5);
            Console.WriteLine("Press any key");
            Console.ReadKey(true);
        }

        public void UpdateTetrisBrick()
        {
            while (true)
            {
                dropTime = (int)dropTimer.ElapsedMilliseconds;
                if (dropTime > dropRate)
                {
                    dropTime = 0;
                    dropTimer.Restart();
                    Drop();
                }
                if (isDropped == true)
                {
                    nextBrick = new TetrisBrick();
                    FillWithBlanks();
                    DrawBorder();
                    DrawTetrisBrick(currentBrick);
                    currentBrick = nextBrick;
                    Spawn();

                    isDropped = false;
                }
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (droppedTetrisBricksLocationGrid[0, j] == 1)
                        return;
                }

                Input();
                ClearBlock();
            }
        }

        private void ClearBlock()
        {
            int combo = 0;
            for (int i = 0; i < 23; i++)
            {
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (droppedTetrisBricksLocationGrid[i, j] == 0)
                        break;
                }
                if (j == 10)
                {
                    linesCleared++;
                    combo++;
                    for (j = 0; j < 10; j++)
                    {
                        droppedTetrisBricksLocationGrid[i, j] = 0;
                    }
                    int[,] newDroppedTetrisBricksLocationGrid = new int[23, 10];
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            newDroppedTetrisBricksLocationGrid[k + 1, l] = droppedTetrisBricksLocationGrid[k, l];
                        }
                    }
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            droppedTetrisBricksLocationGrid[k, l] = 0;
                        }
                    }
                    for (int k = 0; k < 23; k++)
                    for (int l = 0; l < 10; l++)
                        if (newDroppedTetrisBricksLocationGrid[k, l] == 1)
                            droppedTetrisBricksLocationGrid[k, l] = 1;
                    Draw();
                }
            }
            if (combo == 1)
                score += 40 * level;
            else if (combo == 2)
                score += 100 * level;
            else if (combo == 3)
                score += 300 * level;
            else if (combo > 3)
                score += 300 * combo * level;

            if (linesCleared < 5) level = 1;
            else if (linesCleared < 10) level = 2;
            else if (linesCleared < 15) level = 3;
            else if (linesCleared < 25) level = 4;
            else if (linesCleared < 35) level = 5;
            else if (linesCleared < 50) level = 6;
            else if (linesCleared < 70) level = 7;
            else if (linesCleared < 90) level = 8;
            else if (linesCleared < 110) level = 9;
            else if (linesCleared < 150) level = 10;


            if (combo > 0)
            {
                Console.SetCursorPosition(25, 0);
                Console.WriteLine("Level " + level);
                Console.SetCursorPosition(25, 1);
                Console.WriteLine("Score " + score);
                Console.SetCursorPosition(25, 2);
                Console.WriteLine("LinesCleared " + linesCleared);
            }

            dropRate = 300 - 22 * level;

        }

        private void Input()
        {
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey();
                isKeyPressed = true;
            }
            else
                isKeyPressed = false;

            if (key.Key == ConsoleKey.LeftArrow & !IsSomethingLeft() & isKeyPressed)
            {
                for (int i = 0; i < 4; i++)
                {
                    location[i][1] -= 1;
                }
                UpdateTetrisBrick();
                
            }
            else if (key.Key == ConsoleKey.RightArrow & !IsSomethingRight() & isKeyPressed)
            {
                for (int i = 0; i < 4; i++)
                {
                    location[i][1] += 1;
                }
                UpdateTetrisBrick();
            }
            if (key.Key == ConsoleKey.DownArrow & isKeyPressed)
            {
                Drop();
            }
            if (key.Key == ConsoleKey.UpArrow & isKeyPressed)
            {
                for (; IsSomethingBelow() != true;)
                {
                    Drop();
                }
            }
            if (key.Key == ConsoleKey.Spacebar & isKeyPressed)
            {
                //rotate
                Rotate();
                UpdateTetrisBrick();
            }
        }

        public void StartGame()
        {
            currentBrick = new TetrisBrick();

            FillWithBlanks();
            DrawBorder();
            DrawTetrisBrick(currentBrick);
            Spawn();
        }

        private void DrawTetrisBrick(TetrisBrick brick)
        {
            for (int i = 0; i < brick.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < brick.Shape.GetLength(1); j++)
                {
                    if (brick.Shape[i, j] == 1)
                    {
                        Console.SetCursorPosition(((10 - brick.Shape.GetLength(1)) / 2 + j) * 2 + 20, i + 5);
                        Console.Write(sqr);
                    }
                }
            }
        }

        private void FillWithBlanks()
        {

            for (int i = 23; i < 33; ++i)
            {
                for (int j = 3; j < 10; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("  ");
                }

            }
        }

        public void UpdateView()
        {
            for (int i = 0; i < 23; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = 0;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                grid[location[i][0], location[i][1]] = 1;
            }
            Draw();
        }

        public void Spawn()
        {
            for (int i = 0; i < currentBrick.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < currentBrick.Shape.GetLength(1); j++)
                {
                    if (currentBrick.Shape[i, j] == 1)
                    {
                        location.Add(new int[] { i, (10 - currentBrick.Shape.GetLength(1)) / 2 + j });
                    }
                }
            }
            UpdateTetrisBrick();
        }

        public void Drop()
        {

            if (IsSomethingBelow())
            {
                for (int i = 0; i < 4; i++)
                {
                    droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] = 1;
                }
                isDropped = true;

            }
            else
            {
                for (int numCount = 0; numCount < 4; numCount++)
                {
                    location[numCount][0] += 1;
                }
                UpdateTetrisBrick();
            }
        }

        public void Rotate()
        {
            List<int[]> templocation = new List<int[]>();
            for (int i = 0; i < currentBrick.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < currentBrick.Shape.GetLength(1); j++)
                {
                    if (currentBrick.Shape[i, j] == 1)
                    {
                        templocation.Add(new int[] { i, (10 - currentBrick.Shape.GetLength(1)) / 2 + j });
                    }
                }
            }

            if (currentBrick.Shape == tetrisBrickTypes[0])
            {
                if (isErect == false)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i] = TransformMatrix(location[i], location[2], "Clockwise");
                    }
                }
                else
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i] = TransformMatrix(location[i], location[2], "Counterclockwise");
                    }
                }
            }

            else if (currentBrick.Shape == tetrisBrickTypes[3])
            {
                for (int i = 0; i < location.Count; i++)
                {
                    templocation[i] = TransformMatrix(location[i], location[3], "Clockwise");
                }
            }

            else if (currentBrick.Shape == tetrisBrickTypes[1]) return;
            else
            {
                for (int i = 0; i < location.Count; i++)
                {
                    templocation[i] = TransformMatrix(location[i], location[2], "Clockwise");
                }
            }


            for (int count = 0; IsOverlayLeft(templocation) != false | IsOverlayRight(templocation) != false | IsOverlayBelow(templocation) != false; count++)
            {
                if (IsOverlayLeft(templocation) == true)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i][1] += 1;
                    }
                }

                if (IsOverlayRight(templocation) == true)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i][1] -= 1;
                    }
                }
                if (IsOverlayBelow(templocation) == true)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i][0] -= 1;
                    }
                }
                if (count == 3)
                {
                    return;
                }
            }

            location = templocation;

        }

        public int[] TransformMatrix(int[] coord, int[] axis, string dir)
        {
            int[] pcoord = { coord[0] - axis[0], coord[1] - axis[1] };
            if (dir == "Counterclockwise")
            {
                pcoord = new int[] { -pcoord[1], pcoord[0] };
            }
            else if (dir == "Clockwise")
            {
                pcoord = new int[] { pcoord[1], -pcoord[0] };
            }

            return new int[] { pcoord[0] + axis[0], pcoord[1] + axis[1] };
        }

        public bool IsSomethingBelow()
        {
            for (int i = 0; i < 4; i++)
            {
                if (location[i][0] + 1 >= 23)
                    return true;
                if (location[i][0] + 1 < 23)
                {
                    if (droppedTetrisBricksLocationGrid[location[i][0] + 1, location[i][1]] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool? IsOverlayBelow(List<int[]> location)
        {
            List<int> ycoords = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                ycoords.Add(location[i][0]);
                if (location[i][0] >= 23)
                    return true;
                if (location[i][0] < 0)
                    return null;
                if (location[i][1] < 0)
                {
                    return null;
                }
                if (location[i][1] > 9)
                {
                    return null;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (ycoords.Max() - ycoords.Min() == 3)
                {
                    if (ycoords.Max() == location[i][0] | ycoords.Max() - 1 == location[i][0])
                    {
                        if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (ycoords.Max() == location[i][0])
                    {
                        if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsSomethingLeft()
        {
            for (int i = 0; i < 4; i++)
            {
                if (location[i][1] == 0)
                {
                    return true;
                }
                else if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1] - 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool? IsOverlayLeft(List<int[]> location)
        {
            List<int> xcoords = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                xcoords.Add(location[i][1]);
                if (location[i][1] < 0)
                {
                    return true;
                }
                if (location[i][1] > 9)
                {
                    return false;
                }
                if (location[i][0] >= 23)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if (xcoords.Min() == location[i][1] | xcoords.Min() + 1 == location[i][1])
                    {
                        if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (xcoords.Min() == location[i][1])
                    {
                        if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsSomethingRight()
        {
            for (int i = 0; i < 4; i++)
            {
                if (location[i][1] == 9)
                {
                    return true;
                }
                else if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1] + 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool? IsOverlayRight(List<int[]> location)
        {
            List<int> xcoords = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                xcoords.Add(location[i][1]);
                if (location[i][1] > 9)
                {
                    return true;
                }
                if (location[i][1] < 0)
                {
                    return false;
                }
                if (location[i][0] >= 23)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if (xcoords.Max() == location[i][1] | xcoords.Max() - 1 == location[i][1])
                    {
                        if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (xcoords.Max() == location[i][1])
                    {
                        if (droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}