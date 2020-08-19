using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 30;
            Console.SetBufferSize(Console.WindowWidth,Console.WindowHeight);
            Console.CursorVisible = false;//鼠标不可见
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;

            GameProcess process = new GameProcess();
            process.StartGame();
           
        }
    }
}
