using System;

namespace Tetris.TestSample
{
    public interface IDateProvider
    {
        DateTime Today { get; }

        int Juhu(string test);
    }
}