using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    internal class Tetrominoe
    {
        public static int[,] I = {{1, 1, 1, 1}}; //3
        public static int[,] O = {{1, 1}, {1, 1}};
        public static int[,] T = {{0, 1, 0}, {1, 1, 1}}; //3
        public static int[,] S = {{0, 1, 1}, {1, 1, 0}}; //4
        public static int[,] Z = {{1, 1, 0}, {0, 1, 1}}; //3
        public static int[,] J = {{1, 0, 0}, {1, 1, 1}}; //3
        public static int[,] L = {{0, 0, 1}, {1, 1, 1}}; //3
        public static List<int[,]> Tetrominoes = new List<int[,]> {I, O, T, S, Z, J, L};

        private readonly bool isErect = false;
        public List<int[]> Location = new List<int[]>();
        private readonly int[,] shape;

        public Tetrominoe()
        {
            var rnd = new Random();
            shape = Tetrominoes[rnd.Next(0, 7)];
            for (var i = 23; i < 33; ++i)
            {
                for (var j = 3; j < 10; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("  ");
                }
            }
            TetrisGame.Instance.BorderDrawer.DrawBorder();
            for (var i = 0; i < shape.GetLength(0); i++)
            {
                for (var j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                    {
                        Console.SetCursorPosition(((10 - shape.GetLength(1)) / 2 + j) * 2 + 20, i + 5);
                        Console.Write(TetrisDrawer.Sqr);
                    }
                }
            }
        }

        public void Spawn()
        {
            for (var i = 0; i < shape.GetLength(0); i++)
            {
                for (var j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                    {
                        Location.Add(new[] {i, (10 - shape.GetLength(1)) / 2 + j});
                    }
                }
            }
            Update();
        }

        public void Drop()
        {
            if (IsSomethingBelow())
            {
                for (var i = 0; i < 4; i++)
                {
                    TetrisGame.Instance.DroppedtetrominoeLocationGrid[Location[i][0], Location[i][1]] = 1;
                }
                TetrisGame.Instance.IsDropped = true;
            }
            else
            {
                for (var numCount = 0; numCount < 4; numCount++)
                {
                    Location[numCount][0] += 1;
                }
                Update();
            }
        }

        public void Rotate()
        {
            var templocation = new List<int[]>();
            for (var i = 0; i < shape.GetLength(0); i++)
            {
                for (var j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                    {
                        templocation.Add(new[] {i, (10 - shape.GetLength(1)) / 2 + j});
                    }
                }
            }

            if (shape == Tetrominoes[0])
            {
                if (isErect == false)
                {
                    for (var i = 0; i < Location.Count; i++)
                    {
                        templocation[i] = TransformMatrix(Location[i], Location[2], "Clockwise");
                    }
                }
                else
                {
                    for (var i = 0; i < Location.Count; i++)
                    {
                        templocation[i] = TransformMatrix(Location[i], Location[2], "Counterclockwise");
                    }
                }
            }

            else if (shape == Tetrominoes[3])
            {
                for (var i = 0; i < Location.Count; i++)
                {
                    templocation[i] = TransformMatrix(Location[i], Location[3], "Clockwise");
                }
            }

            else if (shape == Tetrominoes[1])
            {
                return;
            }
            else
            {
                for (var i = 0; i < Location.Count; i++)
                {
                    templocation[i] = TransformMatrix(Location[i], Location[2], "Clockwise");
                }
            }


            for (var count = 0;
                (IsOverlayLeft(templocation) != false) | (IsOverlayRight(templocation) != false) |
                (IsOverlayBelow(templocation) != false);
                count++)
            {
                if (IsOverlayLeft(templocation) == true)
                {
                    for (var i = 0; i < Location.Count; i++)
                    {
                        templocation[i][1] += 1;
                    }
                }

                if (IsOverlayRight(templocation) == true)
                {
                    for (var i = 0; i < Location.Count; i++)
                    {
                        templocation[i][1] -= 1;
                    }
                }
                if (IsOverlayBelow(templocation) == true)
                {
                    for (var i = 0; i < Location.Count; i++)
                    {
                        templocation[i][0] -= 1;
                    }
                }
                if (count == 3)
                {
                    return;
                }
            }

            Location = templocation;
        }

        public int[] TransformMatrix(int[] coord, int[] axis, string dir)
        {
            int[] pcoord = {coord[0] - axis[0], coord[1] - axis[1]};
            if (dir == "Counterclockwise")
            {
                pcoord = new[] {-pcoord[1], pcoord[0]};
            }
            else if (dir == "Clockwise")
            {
                pcoord = new[] {pcoord[1], -pcoord[0]};
            }

            return new[] {pcoord[0] + axis[0], pcoord[1] + axis[1]};
        }

        public bool IsSomethingBelow()
        {
            for (var i = 0; i < 4; i++)
            {
                if (Location[i][0] + 1 >= 23)
                {
                    return true;
                }
                if (Location[i][0] + 1 < 23)
                {
                    if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[Location[i][0] + 1, Location[i][1]] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool? IsOverlayBelow(List<int[]> location)
        {
            var ycoords = new List<int>();
            for (var i = 0; i < 4; i++)
            {
                ycoords.Add(location[i][0]);
                if (location[i][0] >= 23)
                {
                    return true;
                }
                if (location[i][0] < 0)
                {
                    return null;
                }
                if (location[i][1] < 0)
                {
                    return null;
                }
                if (location[i][1] > 9)
                {
                    return null;
                }
            }
            for (var i = 0; i < 4; i++)
            {
                if (ycoords.Max() - ycoords.Min() == 3)
                {
                    if ((ycoords.Max() == location[i][0]) | (ycoords.Max() - 1 == location[i][0]))
                    {
                        if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (ycoords.Max() == location[i][0])
                    {
                        if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
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
            for (var i = 0; i < 4; i++)
            {
                if (Location[i][1] == 0)
                {
                    return true;
                }
                if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[Location[i][0], Location[i][1] - 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool? IsOverlayLeft(List<int[]> location)
        {
            var xcoords = new List<int>();
            for (var i = 0; i < 4; i++)
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
                {
                    return null;
                }
                if (location[i][0] < 0)
                {
                    return null;
                }
            }
            for (var i = 0; i < 4; i++)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if ((xcoords.Min() == location[i][1]) | (xcoords.Min() + 1 == location[i][1]))
                    {
                        if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (xcoords.Min() == location[i][1])
                    {
                        if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
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
            for (var i = 0; i < 4; i++)
            {
                if (Location[i][1] == 9)
                {
                    return true;
                }
                if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[Location[i][0], Location[i][1] + 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool? IsOverlayRight(List<int[]> location)
        {
            var xcoords = new List<int>();
            for (var i = 0; i < 4; i++)
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
                {
                    return null;
                }
                if (location[i][0] < 0)
                {
                    return null;
                }
            }
            for (var i = 0; i < 4; i++)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if ((xcoords.Max() == location[i][1]) | (xcoords.Max() - 1 == location[i][1]))
                    {
                        if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (xcoords.Max() == location[i][1])
                    {
                        if (TetrisGame.Instance.DroppedtetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Update()
        {
            for (var i = 0; i < 23; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    TetrisGame.Instance.Grid[i, j] = 0;
                }
            }
            for (var i = 0; i < 4; i++)
            {
                TetrisGame.Instance.Grid[Location[i][0], Location[i][1]] = 1;
            }
            TetrisGame.Instance.TetrisDrawer.Draw(TetrisGame.Instance.DroppedtetrominoeLocationGrid, TetrisGame.Instance.Grid);
        }
    }
}