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
        public static Position NextPosition(Position CurrentPosition , char input)
        {
            switch(input)
            {
                case 'z':
                    return new Position(CurrentPosition.row - 1, CurrentPosition.column);
                case 's':
                    return new Position(CurrentPosition.row +1, CurrentPosition.column);
                case 'q':
                    return new Position(CurrentPosition.row, CurrentPosition.column -1);
                case 'd':
                    return new Position(CurrentPosition.row , CurrentPosition.column +1);
                default:
                    return null;
            }
        }
        public static bool PossibleShift(Position position)
        {
            if (GameManager.board[position.row, position.column].wall)
            {
                return false;
            }
            return true;
        }
    }
}
