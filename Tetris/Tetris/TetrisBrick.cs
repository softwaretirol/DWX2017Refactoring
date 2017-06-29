using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tetris
{

    internal class TetrisBrick
    {
        private int[,] iTypeBrick = new int[1, 4] { { 1, 1, 1, 1 } };
        private int[,] oTypeBrick = new int[2, 2] { { 1, 1 }, { 1, 1 } };
        private int[,] tTypeBrick = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        private int[,] sTypeBrick = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        private int[,] zTypeBrick = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        private int[,] jTypeBrick = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        private int[,] lTypeBrick = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        private List<int[,]> tetrisBrickTypes = new List<int[,]>();

        public int[,] Shape { get; private set; }

        public TetrisBrick()
        {

            tetrisBrickTypes.AddRange(new List<int[,]>() { iTypeBrick, oTypeBrick, tTypeBrick, sTypeBrick, zTypeBrick, jTypeBrick, lTypeBrick });
            Random rnd = new Random();
            Shape = tetrisBrickTypes[rnd.Next(0, 7)];
        }
    }
}

