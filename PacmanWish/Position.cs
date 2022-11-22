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

        public Position(int row = 14, int column = 14)
        {
            Row = row;
            Column = column;
        }
        public bool GhostEating (Ghost[] ghosts)
        {
            Position G1 = ghosts[0].Position;
            Position G2 = ghosts[1].Position;
            Position G3 = ghosts[2].Position;
            Position G4 = ghosts[3].Position;
            bool equal = false;
            bool ghost_1_Hitbox = (G1.Row == this.Row || (G1.Row + 1 == this.Row) || (G1.Row - 1 == this.Row)) && ((G1.Column == this.Column) || (G1.Column + 1 == this.Column) || (G1.Column - 1 == this.Column));
            bool ghost_2_Hitbox = (G2.Row == this.Row || (G2.Row + 1 == this.Row) || (G2.Row - 1 == this.Row)) && ((G2.Column == this.Column) || (G2.Column + 1 == this.Column) || (G2.Column - 1 == this.Column));
            bool ghost_3_Hitbox = (G3.Row == this.Row || (G3.Row + 1 == this.Row) || (G3.Row - 1 == this.Row)) && ((G3.Column == this.Column) || (G3.Column + 1 == this.Column) || (G3.Column - 1 == this.Column));
            bool ghost_4_Hitbox = (G4.Row == this.Row || (G4.Row + 1 == this.Row) || (G4.Row - 1 == this.Row)) && ((G4.Column == this.Column) || (G4.Column + 1 == this.Column) || (G4.Column - 1 == this.Column));

            if (ghost_1_Hitbox || ghost_2_Hitbox || ghost_3_Hitbox || ghost_4_Hitbox)
            {
                equal = true;
            }
            return equal;
        }
    }
}
