using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    
    public class TetrisBrick
		{
			private static int[,] iTypeBrick = new int[1, 4] { { 1, 1, 1, 1 } };
		    private static int[,] oTypeBrick = new int[2, 2] { { 1, 1 }, { 1, 1 } };
		    private static int[,] tTypeBrick = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
		    private static int[,] sTypeBrick = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
		    private static int[,] zTypeBrick = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
		    private static int[,] jTypeBrick = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
		    private static int[,] lTypeBrick = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
		    private static List<int[,]> tetrisBrickTypes = new List<int[,]>() { iTypeBrick, oTypeBrick, tTypeBrick, sTypeBrick, zTypeBrick, jTypeBrick, lTypeBrick };

			private bool isErect = false;
			private int[,] shape;
		    public List<int[]> location = new List<int[]>();

			public TetrisBrick()
			{
				Random rnd = new Random();
				shape = tetrisBrickTypes[rnd.Next(0, 7)];
				for (int i = 23; i < 33; ++i)
				{
					for (int j = 3; j < 10; j++)
					{
						Console.SetCursorPosition(i, j);
						Console.Write("  ");
					}

				}
				Program.DrawBorder();
				for (int i = 0; i < shape.GetLength(0); i++)
				{
					for (int j = 0; j < shape.GetLength(1); j++)
					{
						if (shape[i, j] == 1)
						{
							Console.SetCursorPosition(((10 - shape.GetLength(1)) / 2 + j) * 2 + 20, i + 5);
							Console.Write(Program.sqr);
						}
					}
				}
			}

			public void Spawn()
			{
				for (int i = 0; i < shape.GetLength(0); i++)
				{
					for (int j = 0; j < shape.GetLength(1); j++)
					{
						if (shape[i, j] == 1)
						{
							location.Add(new int[] { i, (10 - shape.GetLength(1)) / 2 + j });
						}
					}
				}
				Update();
			}

			public void Drop()
			{

				if (IsSomethingBelow())
				{
					for (int i = 0; i < 4; i++)
					{
						Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] = 1;
					}
					Program.isDropped = true;

				}
				else
				{
					for (int numCount = 0; numCount < 4; numCount++)
					{
						location[numCount][0] += 1;
					}
					Update();
				}
			}

			public void Rotate()
			{
				List<int[]> templocation = new List<int[]>();
				for (int i = 0; i < shape.GetLength(0); i++)
				{
					for (int j = 0; j < shape.GetLength(1); j++)
					{
						if (shape[i, j] == 1)
						{
							templocation.Add(new int[] { i, (10 - shape.GetLength(1)) / 2 + j });
						}
					}
				}

				if (shape == tetrisBrickTypes[0])
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

				else if (shape == tetrisBrickTypes[3])
				{
					for (int i = 0; i < location.Count; i++)
					{
						templocation[i] = TransformMatrix(location[i], location[3], "Clockwise");
					}
				}

				else if (shape == tetrisBrickTypes[1]) return;
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
						if (Program.droppedTetrisBricksLocationGrid[location[i][0] + 1, location[i][1]] == 1)
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
							if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
							{
								return true;
							}
						}

					}
					else
					{
						if (ycoords.Max() == location[i][0])
						{
							if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
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
					else if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1] - 1] == 1)
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
							if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
							{
								return true;
							}
						}

					}
					else
					{
						if (xcoords.Min() == location[i][1])
						{
							if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
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
					else if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1] + 1] == 1)
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
							if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
							{
								return true;
							}
						}

					}
					else
					{
						if (xcoords.Max() == location[i][1])
						{
							if (Program.droppedTetrisBricksLocationGrid[location[i][0], location[i][1]] == 1)
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
				for (int i = 0; i < 23; i++)
				{
					for (int j = 0; j < 10; j++)
					{
						Program.grid[i, j] = 0;
					}
				}
				for (int i = 0; i < 4; i++)
				{
					Program.grid[location[i][0], location[i][1]] = 1;
				}
				Program.Draw();
			}
		}
    }

