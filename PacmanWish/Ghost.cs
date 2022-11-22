using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Ghost
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public char Direction { get; set; }

        public Ghost(string name, char direction)
        {
            Name = name;
            Position = new Position();
            Direction = direction;
        }
    }
}
