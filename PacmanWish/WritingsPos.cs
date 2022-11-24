using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class WritingsPos
    {
        public Position NewPosition { get; set; }
        public Position OldPosition { get; set; }
        public WritingsPos(Position newPosition, Position oldPosition)
        {
            NewPosition = newPosition;
            OldPosition = oldPosition;
        }

    }
}
