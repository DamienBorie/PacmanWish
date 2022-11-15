using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row = 13, int column = 14)
        {
            Row = row;
            Column = column;
        }
        public bool GhostEating (Position G1, Position G2, Position G3, Position G4)
        {
            bool equal = false;
            if( (G1.Row==this.Row && G1.Column==this.Column) || (G2.Row == this.Row && G2.Column == this.Column) || (G3.Row == this.Row && G3.Column == this.Column) || (G4.Row == this.Row && G4.Column == this.Column) )
            {
                equal = true;
            }
            return equal;
        }
    }
}
