using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class Position
    {
        public int row { get; set; }
        public int column { get; set; }

        public Position(int row = 14, int column = 16)
        {
            this.row = row;
            this.column = column;
        }
    }
}
