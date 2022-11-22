using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Writings
    {
        public string Name { get; private set; }
        public Position NewPosition { get; set; }
        public Position OldPosition { get; set; }
        public Writings(string name, Position newPosition, Position oldPosition)
        {
            Name = name;
            NewPosition = newPosition;
            OldPosition = oldPosition;
        }

    }
}
