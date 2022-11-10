using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class GameManager // how to run game ? constructor ?
    {
        private int lives { get; set; }
        private int score { get; set; }
        public Pacman pacman { get; set; }
        public Ghost[] ghosts { get; set; }

        public static Tile[,] board = new Tile[28, 32]; // static not definitive
        public Tile[,] Board
        {
            get { return board; }
            set { board = value; }
        }

        public void InitBoard()
        {
            //contours
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, 0].wall = true;
                board[i, board.GetLength(1) - 1].wall = true;
            }
            for (int i = 0; i < board.GetLength(1); i++)
            {
                board[0, i].wall = true;
                board[board.GetLength(0) - 1, i].wall = true;
            }

            //autres murs

            //coin si pas de mur et pacman
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].wall == false && board[i, j].pacmanHere == false)
                    {
                        board[i, j].coin = true;
                    }
                }
            }

        }
    }
}
