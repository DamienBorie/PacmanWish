using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Tile
    {
        public bool node { get; set; }
        public bool coin { get; set; }
        public bool wall { get; set; }
        public bool ghostHere { get; set; }
        public bool pacmanHere { get; set; }

        public Tile(bool node = false, bool coin = false, bool wall = false, bool ghostHere = false, bool pacmanHere = false)
        {
            this.node = node;
            this.coin = coin;
            this.wall = wall;
            this.ghostHere = ghostHere;
            this.pacmanHere = pacmanHere;
        }
    }
}
