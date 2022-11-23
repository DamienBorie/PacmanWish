using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Tile
    {
        public bool Node { get; set; }
        public bool Coin { get; set; }
        public bool Wall { get; set; }
        public bool GhostHere { get; set; }
        public bool PacmanHere { get; set; }

        public Tile(bool node = false, bool coin = false, bool wall = false, bool ghostHere = false, bool pacmanHere = false)
        {
            Node = node;
            Coin = coin;
            Wall = wall;
            GhostHere = ghostHere;
            PacmanHere = pacmanHere;
        }
    }
}
