class Program
{

    static Coordinate[,] board = new Coordinate[4, 4];

    static int PosX = 0;
    static int PosY = 0;

    static void Main(string[] args)
    {
        //randomize

        char[] symbols = new char[] { '#', '#', '1', '1', '2', '2', '3', '3', '4', '4', 'A', 'A', 'B', 'B', 'C', 'C' };
        Random rnd = new Random();

        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(1); x++)
            {
                Coordinate c = new Coordinate();

                while (true)
                {
                    int index = rnd.Next(symbols.Length);
                    if (symbols[index] != ' ')
                    {
                        c.Symbol = symbols[index];
                        symbols[index] = ' ';
                        break;
                    }
                }
                board[y, x] = c;
            }
        }

        while (true)
        {
            DrawBoard();

            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.LeftArrow)
            {
                PosX--;
            }
            if (key.Key == ConsoleKey.RightArrow)
            {
                PosX++;
            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                PosY--;
            }
            if (key.Key == ConsoleKey.DownArrow)
            {
                PosY++;
            }
            if (key.Key == ConsoleKey.Spacebar)
            {
                board[PosY, PosX].IsOpen = true;
            }

            Console.Clear();
        }
    }

    static void DrawBoard()
    {
        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(1); x++)
            {
                if (x == PosX && y == PosY)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                if (!board[y, x].IsOpen)
                {
                    Console.Write(". ");
                }
                else
                {
                    Console.Write(board[y, x].Symbol + " ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }
    }
}

class Coordinate
{
    public char Symbol;
    public bool IsOpen;
    public bool isDone;
}