using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Movement
    {
        public char direction { get; set; }

        public Movement(char direction)
        {
            this.direction = direction;
        }
        public Position NextPosition(Position CurrentPosition)
        {
            switch(this.direction)
            {
                case 'z':
                    return new Position(CurrentPosition.Row - 1, CurrentPosition.Column);
                case 's':
                    return new Position(CurrentPosition.Row +1, CurrentPosition.Column);
                case 'q':
                    return new Position(CurrentPosition.Row, CurrentPosition.Column -1);
                case 'd':   
                    return new Position(CurrentPosition.Row , CurrentPosition.Column +1);
                default:
                    return null;
            }
        }
        public bool PossibleShift(Position position)
        {
            if (GameManager.board[position.Row, position.Column].wall)
            {
                return false;
            }
            return true;
        }
    }
}
