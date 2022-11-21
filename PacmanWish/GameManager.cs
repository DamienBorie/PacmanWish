using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacmanWish
{
    public class GameManager
    {
        #region attributes
        private int lives { get; set; }
        private int score { get; set; }
        public Pacman pacman { get; set; }
        public Ghost[] ghosts { get; set; }

        public static Tile[,] board = new Tile[31, 28]; // static argument not definitive
        #endregion

        #region properties
        public Tile[,] Board
        {
            get { return board; }
            set { board = value; }
        }
        #endregion

        #region constructor
        public GameManager()
        {
            pacman = new Pacman();
            ghosts = new Ghost[] { new Ghost("ghost1"), new Ghost("ghost2"), new Ghost("ghost3"), new Ghost("ghost4") };
            NewGame();
        }
        #endregion
        public void NewGame()
        {
            lives = 3; 
            score = 0;
            InitBoard();
            SetDefaultPosition();
            LoopPlayer();
        }
        public void SetDefaultPosition()
        {
            SetPosition(pacman.Name, new Position(17, 14));
            SetPosition(ghosts[0].Name, new Position(13, 12));
            SetPosition(ghosts[1].Name, new Position(13, 13));
            SetPosition(ghosts[2].Name, new Position(13, 14));
            SetPosition(ghosts[3].Name, new Position(13, 15));
        }
        public void SetPosition(string name, Position newPosition)
        {
            switch (name)
            {
                case "pacman":
                    board[pacman.Position.Row, pacman.Position.Column].pacmanHere = false;
                    pacman.Position = newPosition;
                    board[newPosition.Row, newPosition.Column].pacmanHere = true;
                    break;
                case "ghost1":
                    board[ghosts[0].Position.Row, ghosts[0].Position.Column].ghostHere = false;
                    ghosts[0].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].ghostHere = true;
                    break;
                case "ghost2":
                    board[ghosts[1].Position.Row, ghosts[1].Position.Column].ghostHere = false;
                    ghosts[1].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].ghostHere = true;
                    break;
                case "ghost3":
                    board[ghosts[2].Position.Row, ghosts[2].Position.Column].ghostHere = false;
                    ghosts[2].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].ghostHere = true;
                    break;
                case "ghost4":
                    board[ghosts[3].Position.Row, ghosts[3].Position.Column].ghostHere = false;
                    ghosts[3].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].ghostHere = true;
                    break;
            }
        }
        public void LoopPlayer()
        {
            //Start Ghosts thread
            new Thread(GhostBehavior1).Start();
            new Thread(GhostBehavior2).Start();
            new Thread(GhostBehavior3).Start();
            new Thread(GhostBehavior4).Start();

            while (pacman.Position.GhostEating(ghosts) == false)
            {
                Console.Clear();
                Console.WriteLine("Score : " + score + "\nLives : " + lives + "\n");
                DisplayBoard();

                //Pacman
                ConsoleKeyInfo cki = new ConsoleKeyInfo();
                Movement movement = new Movement(' ');
                do
                {
                    do
                    {
                        cki = Console.ReadKey(true);
                        if (cki.Key == ConsoleKey.LeftArrow || cki.Key == ConsoleKey.Q)
                        {
                            movement.direction = 'q';
                        }
                        if (cki.Key == ConsoleKey.RightArrow || cki.Key == ConsoleKey.D)
                        {
                            movement.direction = 'd';
                        }
                        if (cki.Key == ConsoleKey.DownArrow || cki.Key == ConsoleKey.S)
                        {
                            movement.direction = 's';
                        }
                        if (cki.Key == ConsoleKey.UpArrow || cki.Key == ConsoleKey.Z)
                        {
                            movement.direction = 'z';
                        }

                    } while ((movement.direction != 'z') && (movement.direction != 'q') && (movement.direction != 's') && (movement.direction != 'd'));

                } while (movement.PossibleShift(movement.NextPosition(pacman.Position)) == false);

                SetPosition("pacman", movement.NextPosition(pacman.Position));
                score += AddScore(pacman.Position);
                if (RemainingCoin() == false) 
                    NewBoard();
            }
            DecreaseLives();
        }
        public void GhostBehavior4()
        {
            Movement movement = new Movement(' ');
            Random rdm = new Random();
            char[] directions = { 'z', 'q', 's', 'd' };
            do
            {
                do
                {
                    movement.direction = directions[rdm.Next(4)];
                } while (movement.PossibleShift(movement.NextPosition(ghosts[3].Position)) == false);

                do
                {
                    //Console.WriteLine(ghosts[0].Name + " : " + movement.direction + " : " + ghosts[0].Position.Row + " ; " + ghosts[0].Position.Column);
                    SetPosition(ghosts[3].Name, movement.NextPosition(ghosts[3].Position));
                    Thread.Sleep(200);
                } while (board[ghosts[3].Position.Row, ghosts[3].Position.Column].node == false && movement.PossibleShift(movement.NextPosition(ghosts[3].Position)) == true);
            } while (pacman.Position.GhostEating(ghosts) == false);
        }
        public void GhostBehavior3()
        {
            Movement movement = new Movement(' ');
            Random rdm = new Random();
            char[] directions = { 'z', 'q', 's', 'd' };
            do
            {
                do
                {
                    movement.direction = directions[rdm.Next(4)];
                } while (movement.PossibleShift(movement.NextPosition(ghosts[2].Position)) == false);

                do
                {
                    //Console.WriteLine(ghosts[0].Name + " : " + movement.direction + " : " + ghosts[0].Position.Row + " ; " + ghosts[0].Position.Column);
                    SetPosition(ghosts[2].Name, movement.NextPosition(ghosts[2].Position));
                    Thread.Sleep(300);
                } while (board[ghosts[2].Position.Row, ghosts[2].Position.Column].node == false && movement.PossibleShift(movement.NextPosition(ghosts[2].Position)) == true);
            } while (pacman.Position.GhostEating(ghosts) == false);
        }
        public void GhostBehavior2()
        {
            Movement movement = new Movement(' ');
            Random rdm = new Random();
            char[] directions = { 'z', 'q', 's', 'd' };
            do
            {
                do
                {
                    movement.direction = directions[rdm.Next(4)];
                } while (movement.PossibleShift(movement.NextPosition(ghosts[1].Position)) == false);

                do
                {
                    //Console.WriteLine(ghosts[0].Name + " : " + movement.direction + " : " + ghosts[0].Position.Row + " ; " + ghosts[0].Position.Column);
                    SetPosition(ghosts[1].Name, movement.NextPosition(ghosts[1].Position));
                    Thread.Sleep(400);
                } while (board[ghosts[1].Position.Row, ghosts[1].Position.Column].node == false && movement.PossibleShift(movement.NextPosition(ghosts[1].Position)) == true);
            } while (pacman.Position.GhostEating(ghosts) == false);
        }
        public void GhostBehavior1()
        {
            Movement movement = new Movement(' ');
            Random rdm = new Random();
            char[] directions = { 'z', 'q', 's', 'd' };
            do
            {
                do
                {
                    movement.direction = directions[rdm.Next(4)];
                } while (movement.PossibleShift(movement.NextPosition(ghosts[0].Position)) == false);

                do
                {
                    //Console.WriteLine(ghosts[0].Name + " : " + movement.direction + " : " + ghosts[0].Position.Row + " ; " + ghosts[0].Position.Column);
                    SetPosition(ghosts[0].Name, movement.NextPosition(ghosts[0].Position));
                    Thread.Sleep(100);
                } while (board[ghosts[0].Position.Row, ghosts[0].Position.Column].node == false && movement.PossibleShift(movement.NextPosition(ghosts[0].Position)) == true);
            } while (pacman.Position.GhostEating(ghosts) == false);
        }
        public void DecreaseLives()
        {
            if(lives>1)
            {
                lives--;
                NewRound();
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("GAME OVER");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("press any key to start a new game");
                Console.ReadKey();
                NewGame();
            }
        }
        //when you loose a life we re start without reseting the board
        public void NewRound()
        {
            SetDefaultPosition();
            LoopPlayer();
        }
        //when all coin are taken we take a new board
        public void NewBoard()
        {
            InitBoard();
            SetDefaultPosition();
        }
        public int AddScore(Position position)
        {
            if (board[position.Row, position.Column].coin == true)
            {
                board[position.Row, position.Column].coin = false;
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public bool RemainingCoin()
        {
            bool coin = false;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].coin == true) 
                        coin = true;
                }
            }
            return coin;
        }
        public void InitBoard()
        {
            for(int i = 0; i < board.GetLength(0);i++)
            {
                for(int j = 0; j < board.GetLength(1);j++)
                {
                    board[i, j] = new Tile();
                }
            }
            //Moitié du plateau
            //1) contours
            for (int i = 0; i < board.GetLength(0); i++)
            {
                board[i, 0].wall = true;
            }
            for (int i = 0; i < board.GetLength(1); i++)
            {
                board[0, i].wall = true;
                board[board.GetLength(0) - 1, i].wall = true;
            }

            //2) obstacles
            //a)
            for (int i = 1; i < 4; i++)
            {
                for (int j = 10; j < 12; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //b
            for (int i = 7; i < 9; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //c
            for (int i = 21; i < 23; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //d
            for (int i = 2; i < 4; i++)
            {
                for (int j = 2; j < 9; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 4; i < 6; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //e
            for (int i = 5; i < 8; i++)
            {
                for (int j = 8; j < 12; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //f
            for (int i = 5; i < 11; i++)
            {
                for (int j = 5; j < 7; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 9; i < 11; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //g
            for (int i = 2; i < 8; i++)
            {
                board[i, 13].wall = true;
            }
            //h
            for (int i = 9; i < 11; i++)
            {
                for (int j = 10; j < 14; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //i
            for (int i = 10; i < 14; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 12; i < 14; i++)
            {
                for (int j = 4; j < 6; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //j
            for (int i = 12; i < 17; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 15; i < 17; i++)
            {
                for (int j = 5; j < 7; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //k
            for (int i = 15; i < 20; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 18; i < 20; i++)
            {
                for (int j = 4; j < 6; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //l
            for (int i = 18; i < 23; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 18; i < 20; i++)
            {
                for (int j = 9; j < 12; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //m
            for (int i = 21; i < 26; i++)
            {
                for (int j = 4; j < 6; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 24; i < 26; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //n
            for (int i = 27; i < 29; i++)
            {
                for (int j = 2; j < 6; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //o
            for (int i = 24; i < 30; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 24; i < 26; i++)
            {
                for (int j = 9; j < 12; j++)
                {
                    board[i, j].wall = true;
                }
            }
            //p
            for (int i = 27; i < 29; i++)
            {
                for (int j = 10; j < 14; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 24; i < 27; i++)
            {
                board[i, 13].wall = true;
            }
            //q
            for (int i = 21; i < 23; i++)
            {
                for (int j = 10; j < 14; j++)
                {
                    board[i, j].wall = true;
                }
            }
            for (int i = 18; i < 21; i++)
            {
                board[i, 13].wall = true;
            }
            //maison
            for (int i = 10; i < 13; i++)
            {
                board[12, i].wall = true;
            }
            for (int i = 13; i < 17; i++)
            {
                board[i, 10].wall = true;
            }
            for (int i = 11; i < 14; i++)
            {
                board[16, i].wall = true;
            }
            board[13, 10].wall = false;

            //miroir
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1) / 2; j++)
                {
                    board[i, board.GetLength(1) - 1 - j].wall = board[i, j].wall;
                }
            }

            //piece si pas de mur, pas maison, pas pacman

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    bool maison = (j >= 10 && j <= 17 && i >= 13 && i <= 15) || (i == 12 && j >= 13 && j <= 14);
                    if (board[i, j].wall == false && board[i, j].pacmanHere == false && maison == false)
                    {
                        board[i, j].coin = true;
                    }
                }
            }
            board[17, 14].coin = false;

            //nodes
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1) / 2; j++)
                {
                    bool nodesHere = (i == 4 && (j == 7 || j == 9 || j == 12)) || (i == 6 && j == 4) || (i == 8 && (j == 9 || j == 12)) || (i == 9 && j == 4) || (i == 11 && (j == 6 || j == 9)) || (i == 14 && (j == 1 || j == 4)) || (i == 15 && j == 13) || (i == 17 && (j == 6 || j == 9 || j == 12)) || (i == 20 && (j == 3 || j == 6)) || (i == 23 && (j == 9 || j == 12)) || (i == 26 && (j == 1 || j == 6));
                    if (nodesHere == true)
                    {
                        board[i, j].node = true;
                        board[i, board.GetLength(1) - 1 - j].node = true;
                    }
                }
            }
            

        }
        public void DisplayBoard()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    DisplayTile(board[i,j],i,j);
                }
                Console.Write("\n");
            }
        }
        public void DisplayTile(Tile tile,int i, int j)
        {
            /*
            if(tile.node)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("   ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else */
            if (tile.ghostHere)
            {
                if (ghosts[0].Position.Row == i && ghosts[0].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                }
                if (ghosts[1].Position.Row == i && ghosts[1].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                }
                if (ghosts[2].Position.Row == i && ghosts[2].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                }
                if (ghosts[3].Position.Row == i && ghosts[3].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                }

            }
            else if (tile.wall)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("   ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (tile.coin)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" o ");
            }
            else if (tile.pacmanHere)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Pac");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("   ");
            }
        }
    }
}
