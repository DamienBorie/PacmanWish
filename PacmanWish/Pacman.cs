using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Pacman
    {
        public string Name { get; private set; }
        public Position Position { get; set; }
        public Pacman() 
        {
            Name = "pacman";
            Position = new Position();
        }

    }
}
