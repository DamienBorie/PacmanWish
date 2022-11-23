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
            // console params to get a smoother game
            Console.SetWindowSize(84, 35);
            Console.CursorVisible = false;

            // launch game
            GameManager game = new GameManager(); 
        }
    }
}
