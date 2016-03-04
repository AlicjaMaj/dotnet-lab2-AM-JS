using System;
using System.Collections.Generic;



namespace GameOfLife
{

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(System.Object obj)
        {
           
            if (obj == null)
            {
                return false;
            }

            Location location = obj as Location;
            if ((System.Object)location == null)
            {
                return false;
            }

            return (X == location.X) && (Y == location.Y);
        }

        public bool Equals(Location location)
        {
            if ((object)location == null)
            {
                return false;
            }
            return (X == location.X) && (Y == location.Y);
        }

        public override int GetHashCode()
        {
            return 17*X + 5*Y;
        }
    }


    public class Cell
    {
        public bool Alive { get; set; }
        public bool AliveNext { get; set; }
       
        public List<Cell> Neighbours { get; set; }

        public override string ToString()
        {
            return this.Alive.ToString();
        }

        public bool WillIBeAlive()
        {
            int counter = 0;
            foreach (var neighbour in Neighbours)
            {
                if (neighbour.Alive == true)
                {
                    ++counter;
                }
            }
            if (counter < 2)
            {
                this.AliveNext = false;
            }
            else if (counter == 2 && this.Alive == true)
            {
                this.AliveNext = true;
            }
            else if (counter > 3)
            {
                this.AliveNext = false;
            }
            else if (counter == 3)
            {
                this.AliveNext = true;
            }

            return this.AliveNext;

        }
        public void changeState()
        {
            this.Alive = this.AliveNext;
        }
    }
   

    public class IndexCounter
    {
             

        public List<Location> CountNeighbours(Location location)
        {
            List<Location> neighbours = new List<Location>();
            neighbours.Add(new Location(location.X - 1, location.Y - 1));
            neighbours.Add(new Location(location.X + 1, location.Y - 1));
            neighbours.Add(new Location(location.X, location.Y - 1));
            neighbours.Add(new Location(location.X - 1, location.Y));
            neighbours.Add(new Location(location.X + 1, location.Y));
            neighbours.Add(new Location(location.X - 1, location.Y + 1));
            neighbours.Add(new Location(location.X, location.Y + 1));
            neighbours.Add(new Location(location.X + 1, location.Y + 1));
            return neighbours;
        }
    }
    public class Game
    {
        private  int MaxX;
        private  int MaxY;
        private Dictionary<Location, Cell> cells;
     
        public IndexCounter Counter = new IndexCounter();


        public Game( int MaxX, int MaxY)
        {
            this.MaxX = MaxX;
            this.MaxY = MaxY;
            Random rand = new Random();
            cells = new Dictionary<Location, Cell>();
            for (int i = 0; i < MaxX; ++i)
            {
                for (int j = 0; j < MaxY; ++j)
                {
                    Location Key = new Location(i, j);
                    Cell cell = new Cell();
                    cell.Alive = NextBool(rand);
                    cells.Add(Key, cell);
                }
            }
            
            foreach (var cell in cells)
            {
               var neighboursLocation = Counter.CountNeighbours(cell.Key);
                List<Cell> newNeighbours = new List<Cell>();
                foreach (var neighbourLoc in neighboursLocation)
                {
                    if (cells.ContainsKey(neighbourLoc))
                    {
                        newNeighbours.Add(cells[neighbourLoc]);
                    }
                }
                cell.Value.Neighbours = newNeighbours;
            }
        }

        public void playGame()
        {
            
            foreach (var Cell in cells)
            {
                Cell.Value.WillIBeAlive();
            }
        }

        public void changeState()
        {
            foreach (var Cell in cells)
            {
                Cell.Value.changeState();
            }
        }


        public void displayBoard()
        {
            for (int i = 0; i < MaxX; ++i)
            {
                for (int j = 0; j < MaxY; ++j)
                {
                    Location Key = new Location(i, j);
                    if(cells[Key].Alive == true)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("");
            }
        }

        public bool NextBool(Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            var Game = new Game(25,50);
            int i = 0;
            while(i < 1000)
            {
                ++i;
                Game.playGame();
                Game.displayBoard();
                Game.changeState();
                Console.ReadLine();
                Console.Clear();
            }

        }
    }
}

