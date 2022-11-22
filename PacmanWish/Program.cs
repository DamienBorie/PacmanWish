using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth/2, Console.LargestWindowHeight - Console.LargestWindowHeight/4);
            Console.CursorVisible = false;
            GameManager game = new GameManager();
        }
    }
}
