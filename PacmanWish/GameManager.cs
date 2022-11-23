using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PacmanWish
{
    public class GameManager
    {
        #region attributes
        private int Lives { get; set; }
        private int Score { get; set; }
        public Pacman Pacman { get; set; }
        public Ghost[] Ghosts { get; set; }

        public static Tile[,] board = new Tile[31, 28];

        public static List<WritingsPos> ElementToWrite = new List<WritingsPos>();

        // Ghost behavior
        public Thread Th_Ghost1 = null;
        public Thread Th_Ghost2 = null;
        public Thread Th_Ghost3 = null;
        public Thread Th_Ghost4 = null;

        // writing thread with cursor set
        public Thread WritingThread = null;
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
            Pacman = new Pacman();
            Ghosts = new Ghost[] { new Ghost("ghost1",'d'), new Ghost("ghost2",'d'), new Ghost("ghost3", 'q'), new Ghost("ghost4", 'q') };
            NewGame();
        }
        #endregion
        public void NewGame()       //create new game at start or at gameover
        {
            //reset lives and score
            Lives = 3; 
            Score = 0;

            NewBoard();
        }
        public void NewBoard()      //create the board at new game or when all coin ate
        {
            KillAllThreads();
            Console.Clear();

            InitBoard();

            DisplayBoard();

            DisplayScore();

            NewRound();
        }
        public void NewRound()      //restart threads and set position to init when start the game or loose a life
        {
            KillAllThreads();

            ThreadWriting_Start();
            SetDefaultPosition();
            GhostThread_Start();
            LoopPlayer();
        }
        public void KillAllThreads()
        {
            Thread.Sleep(50);
            ThreadWriting_Stop();
            GhostThread_Stop();
        }
        public void DecreaseLives()
        {
            if (Lives > 1)
            {
                Lives--;
                NewRound();
            }
            else
            {
                Lives--;
                Thread.Sleep(50);
                ThreadWriting_Stop();
                GhostThread_Stop();
                GameOverScreen();
            }
        }
        public void GameOverScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("GAME OVER");
            Console.ForegroundColor = ConsoleColor.White;
            DisplayScore();
            Console.WriteLine("press any key to start a new game");
            Console.ReadKey();
            Console.Clear();
            NewGame();
        }
        public void ThreadWriting_Start()
        {
            WritingThread = new Thread(ThreadWriting); // close somewhere
            WritingThread.Start();
        }
        public void ThreadWriting_Stop()
        {
            if(WritingThread!= null)
            {
                WritingThread.Abort();
                ElementToWrite.Clear();
            }
        }
        public void ThreadWriting()
        {
            int timer = 0;
            while (true)
            {
                timer++;
                Thread.Sleep(1);
                var count = ElementToWrite.Count();
                if (count > 0)
                {
                    UpdatePos(ElementToWrite[0].Name, ElementToWrite[0].NewPosition, ElementToWrite[0].OldPosition);
                    ElementToWrite.RemoveAt(0);
                    UpdateScore_Lives();
                }
                if(timer >600)
                {
                    Console.Clear();
                    DisplayBoard();
                    timer = 0;
                }
            }
        }
        public void LoopPlayer()
        {
            while (Pacman.Position.GhostEating(Ghosts) == false)
            {   
                ConsoleKeyInfo cki = new ConsoleKeyInfo();
                Movement movement = new Movement(' ');
                do
                {
                    do
                    {
                        cki = Console.ReadKey(true);
                        if (cki.Key == ConsoleKey.LeftArrow || cki.Key == ConsoleKey.Q)
                        {
                            movement.Direction = 'q';
                        }
                        if (cki.Key == ConsoleKey.RightArrow || cki.Key == ConsoleKey.D)
                        {
                            movement.Direction = 'd';
                        }
                        if (cki.Key == ConsoleKey.DownArrow || cki.Key == ConsoleKey.S)
                        {
                            movement.Direction = 's';
                        }
                        if (cki.Key == ConsoleKey.UpArrow || cki.Key == ConsoleKey.Z)
                        {
                            movement.Direction = 'z';
                        }

                    } while ((movement.Direction != 'z') && (movement.Direction != 'q') && (movement.Direction != 's') && (movement.Direction != 'd'));
                        
                } while (movement.PossibleShift(movement.NextPosition(Pacman.Position)) == false);

                SetPosition("pacman", movement.NextPosition(Pacman.Position));

                AddScore(Pacman.Position);

                if (RemainingCoin() == false) 
                    NewBoard();
            }
            DecreaseLives();
        }
        public void SetDefaultPosition()
        {
            SetPosition(Pacman.Name, new Position(17, 14));
            SetPosition(Ghosts[0].Name, new Position(13, 11));
            SetPosition(Ghosts[1].Name, new Position(15, 12));
            SetPosition(Ghosts[2].Name, new Position(15, 15));
            SetPosition(Ghosts[3].Name, new Position(13, 16));
        }
        public void SetPosition(string name, Position newPosition)
        {
            Position oldPosition = null;
            switch (name)
            {
                case "pacman":
                    oldPosition = new Position(Pacman.Position.Row, Pacman.Position.Column);
                    board[oldPosition.Row, oldPosition.Column].PacmanHere = false;

                    Pacman.Position = newPosition;
                    board[newPosition.Row, newPosition.Column].PacmanHere = true;

                    ElementToWrite.Add(new WritingsPos(Pacman.Name, newPosition, oldPosition));
                    break;

                case "ghost1":
                    oldPosition = new Position(Ghosts[0].Position.Row, Ghosts[0].Position.Column);
                    board[oldPosition.Row, oldPosition.Column].GhostHere = false;

                    Ghosts[0].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].GhostHere = true;

                    ElementToWrite.Add(new WritingsPos(Ghosts[0].Name, newPosition, oldPosition));
                    break;

                case "ghost2":
                    oldPosition = new Position(Ghosts[1].Position.Row, Ghosts[1].Position.Column);
                    board[oldPosition.Row, oldPosition.Column].GhostHere = false;

                    Ghosts[1].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].GhostHere = true;

                    ElementToWrite.Add(new WritingsPos(Ghosts[1].Name, newPosition, oldPosition));
                    break;

                case "ghost3":
                    oldPosition = new Position(Ghosts[2].Position.Row, Ghosts[2].Position.Column);
                    board[oldPosition.Row, oldPosition.Column].GhostHere = false;

                    Ghosts[2].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].GhostHere = true;

                    ElementToWrite.Add(new WritingsPos(Ghosts[2].Name, newPosition, oldPosition));
                    break;

                case "ghost4":
                    oldPosition = new Position(Ghosts[3].Position.Row, Ghosts[3].Position.Column);
                    board[oldPosition.Row, oldPosition.Column].GhostHere = false;

                    Ghosts[3].Position = newPosition;
                    board[newPosition.Row, newPosition.Column].GhostHere = true;

                    ElementToWrite.Add(new WritingsPos(Ghosts[3].Name, newPosition, oldPosition));
                    break;
            }
        }
        public void GhostThread_Start()
        {
            Th_Ghost1 = new Thread(GhostBehavior1);
            Th_Ghost1.Start();

            Th_Ghost2 = new Thread(GhostBehavior2);
            Th_Ghost2.Start();

            Th_Ghost3 = new Thread(GhostBehavior3);
            Th_Ghost3.Start();

            Th_Ghost4 = new Thread(GhostBehavior4);
            Th_Ghost4.Start(); 
        }
        public void GhostThread_Stop()
        {
            if (Th_Ghost1 != null)
                Th_Ghost1.Abort();

            if (Th_Ghost2 != null)
                Th_Ghost2.Abort();

            if (Th_Ghost3 != null)
                Th_Ghost3.Abort();

            if (Th_Ghost4 != null)
                Th_Ghost4.Abort();
        }
        public static Random rdm = new Random();
        public void GhostBehavior4()
        {
            Movement movement = new Movement(' ');
            char[] directions = { 'z', 'q', 's', 'd' };
            while(true)
            {
                int choice = 0;
                do
                {
                    choice = rdm.Next(4);
                    movement.Direction = directions[choice];
                } while (movement.PossibleShift(movement.NextPosition(Ghosts[3].Position)) == false || (Ghosts[3].Direction == directions[(choice + 2) % 4]));
                Ghosts[3].Direction = movement.Direction;
                do
                {
                    SetPosition(Ghosts[3].Name, movement.NextPosition(Ghosts[3].Position));
                    Thread.Sleep(720);
                } while (board[Ghosts[3].Position.Row, Ghosts[3].Position.Column].Node == false && movement.PossibleShift(movement.NextPosition(Ghosts[3].Position)) == true);
            }
        }
        public void GhostBehavior3()
        {
            Movement movement = new Movement(' ');
            char[] directions = { 'z', 'q', 's', 'd' };
            while(true)
            {
                int choice = 0;
                do
                {
                    choice = rdm.Next(4);
                    movement.Direction = directions[choice];
                } while (movement.PossibleShift(movement.NextPosition(Ghosts[2].Position)) == false || (Ghosts[2].Direction == directions[(choice + 2) % 4]));
                Ghosts[2].Direction = movement.Direction;
                do
                {
                    SetPosition(Ghosts[2].Name, movement.NextPosition(Ghosts[2].Position));
                    Thread.Sleep(600);
                } while (board[Ghosts[2].Position.Row, Ghosts[2].Position.Column].Node == false && movement.PossibleShift(movement.NextPosition(Ghosts[2].Position)) == true);
            }   
        }
        public void GhostBehavior2()
        {
            Movement movement = new Movement(' ');
            char[] directions = { 'z', 'q', 's', 'd' };
            while(true)
            {
                int choice = 0;
                do
                {
                    choice = rdm.Next(4);
                    movement.Direction = directions[choice];
                } while (movement.PossibleShift(movement.NextPosition(Ghosts[1].Position)) == false || (Ghosts[1].Direction == directions[(choice + 2) % 4]));
                Ghosts[1].Direction = movement.Direction;

                do
                {
                    SetPosition(Ghosts[1].Name, movement.NextPosition(Ghosts[1].Position));
                    Thread.Sleep(480);
                } while (board[Ghosts[1].Position.Row, Ghosts[1].Position.Column].Node == false && movement.PossibleShift(movement.NextPosition(Ghosts[1].Position)) == true);
            }
        }
        public void GhostBehavior1()
        {
            Movement movement = new Movement(' ');
            char[] directions = { 'z', 'q', 's', 'd' };
            while(true)
            {
                int choice = 0;
                do
                {
                    choice = rdm.Next(4);
                    movement.Direction = directions[choice];
                } while (movement.PossibleShift(movement.NextPosition(Ghosts[0].Position)) == false || (Ghosts[0].Direction == directions[(choice+2)%4]));
                Ghosts[0].Direction = movement.Direction;
                do
                {
                    SetPosition(Ghosts[0].Name, movement.NextPosition(Ghosts[0].Position));
                    Thread.Sleep(360);
                } while (board[Ghosts[0].Position.Row, Ghosts[0].Position.Column].Node == false && movement.PossibleShift(movement.NextPosition(Ghosts[0].Position)) == true);
            }
        }
        public void AddScore(Position position)
        {
            if (board[position.Row, position.Column].Coin == true)
            {
                board[position.Row, position.Column].Coin = false;
                Score++;
            }
        }
        public bool RemainingCoin()
        {
            bool coin = false;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].Coin == true) 
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
                board[i, 0].Wall = true;
            }
            for (int i = 0; i < board.GetLength(1); i++)
            {
                board[0, i].Wall = true;
                board[board.GetLength(0) - 1, i].Wall = true;
            }

            //2) obstacles
            //a)
            for (int i = 1; i < 4; i++)
            {
                for (int j = 10; j < 12; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //b
            for (int i = 7; i < 9; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //c
            for (int i = 21; i < 23; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //d
            for (int i = 2; i < 4; i++)
            {
                for (int j = 2; j < 9; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 4; i < 6; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //e
            for (int i = 5; i < 8; i++)
            {
                for (int j = 8; j < 12; j++)
                {   
                    board[i, j].Wall = true;
                }
            }
            //f
            for (int i = 5; i < 11; i++)
            {
                for (int j = 5; j < 7; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 9; i < 11; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //g
            for (int i = 2; i < 8; i++)
            {
                board[i, 13].Wall = true;
            }
            //h
            for (int i = 9; i < 11; i++)
            {
                for (int j = 10; j < 14; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //i
            for (int i = 10; i < 14; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 12; i < 14; i++)
            {
                for (int j = 4; j < 6; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //j
            for (int i = 12; i < 17; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 15; i < 17; i++)
            {
                for (int j = 5; j < 7; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //k
            for (int i = 15; i < 20; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 18; i < 20; i++)
            {
                for (int j = 4; j < 6; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //l
            for (int i = 18; i < 23; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 18; i < 20; i++)
            {
                for (int j = 9; j < 12; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //m
            for (int i = 21; i < 26; i++)
            {
                for (int j = 4; j < 6; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 24; i < 26; i++)
            {
                for (int j = 2; j < 4; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //n
            for (int i = 27; i < 29; i++)
            {
                for (int j = 2; j < 6; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //o
            for (int i = 24; i < 30; i++)
            {
                for (int j = 7; j < 9; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 24; i < 26; i++)
            {
                for (int j = 9; j < 12; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            //p
            for (int i = 27; i < 29; i++)
            {
                for (int j = 10; j < 14; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 24; i < 27; i++)
            {
                board[i, 13].Wall = true;
            }
            //q
            for (int i = 21; i < 23; i++)
            {
                for (int j = 10; j < 14; j++)
                {
                    board[i, j].Wall = true;
                }
            }
            for (int i = 18; i < 21; i++)
            {
                board[i, 13].Wall = true;
            }
            //maison
            for (int i = 10; i < 13; i++)
            {
                board[12, i].Wall = true;
            }
            for (int i = 13; i < 17; i++)
            {
                board[i, 10].Wall = true;
            }
            for (int i = 11; i < 14; i++)
            {
                board[16, i].Wall = true;
            }
            board[13, 10].Wall = false;

            //miroir
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1) / 2; j++)
                {
                    board[i, board.GetLength(1) - 1 - j].Wall = board[i, j].Wall;
                }
            }

            //piece si pas de mur, pas maison, pas pacman

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    bool maison = (j >= 10 && j <= 17 && i >= 13 && i <= 15) || (i == 12 && j >= 13 && j <= 14);
                    if (board[i, j].Wall == false && board[i, j].PacmanHere == false && maison == false)
                    {
                        board[i, j].Coin = true;
                    }
                }
            }
            board[17, 14].Coin = false;

            //nodes
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1) / 2; j++)
                {
                    bool nodesHere = (i == 4 && (j == 7 || j == 9 || j == 12)) || (i == 6 && j == 4) || (i == 8 && (j == 9 || j == 12)) || (i == 9 && j == 4) || (i == 11 && (j == 6 || j == 9)) || (i == 14 && (j == 1 || j == 4)) || (i == 15 && j == 13) || (i == 17 && (j == 6 || j == 9 || j == 12)) || (i == 20 && (j == 3 || j == 6)) || (i == 23 && (j == 9 || j == 12)) || (i == 26 && (j == 1 || j == 6));
                    if (nodesHere == true)
                    {
                        board[i, j].Node = true;
                        board[i, board.GetLength(1) - 1 - j].Node = true;
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
        public void DisplayScore()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Score : " + Score);
            Console.ForegroundColor = ConsoleColor.Red;
            if(Lives >0)
                Console.WriteLine("Lives : " + Lives);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void UpdatePos(string name,Position newPosition, Position oldPosition)
        {
            Console.SetCursorPosition((oldPosition.Column) * 3, oldPosition.Row);
            DisplayTile(Board[oldPosition.Row, oldPosition.Column], oldPosition.Row, oldPosition.Column);

            Console.SetCursorPosition((newPosition.Column) * 3, newPosition.Row);
            DisplayTile(Board[newPosition.Row, newPosition.Column], newPosition.Row, newPosition.Column);
        }
        public void UpdateScore_Lives()
        {
            Console.SetCursorPosition(0,31);
            DisplayScore();
        }
        public void DisplayTile(Tile tile,int i, int j)
        {
            // see node to know where ghost get a new random direction
            /*
            if(tile.node)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("   ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else */
            if (tile.GhostHere)
            {
                if (Ghosts[0].Position.Row == i && Ghosts[0].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                if (Ghosts[1].Position.Row == i && Ghosts[1].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                if (Ghosts[2].Position.Row == i && Ghosts[2].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                if (Ghosts[3].Position.Row == i && Ghosts[3].Position.Column == j)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" G ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }

            }
            else if (tile.Wall)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("   ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (tile.Coin)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" o ");
            }
            else if (tile.PacmanHere)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Pac");
                Console.BackgroundColor = ConsoleColor.Black;
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
