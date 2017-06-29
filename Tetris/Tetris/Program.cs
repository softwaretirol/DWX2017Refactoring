using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Tetris
{
    internal class Program
    {
        public static Program Instance { get; } = new Program();

        private TetrisView view = new TetrisView();

        static void Main()
        {
            Instance.view.SetConsoleOutputEncoding();

            Instance.view.DrawBorder();

            Instance.view.AskForKeyInput();

            Instance.view.StartDropTimer();

            Instance.view.WriteTitles();

            Instance.view.StartGame();

            Instance.view.UpdateView();

            Instance.view.AskForRestartGame();

            Instance.view.ReadKeyInput();
        }
    }
}
