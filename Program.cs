using System;
using System.Linq;

class Program
{
    const int Width = 5;
    const int Height = 5;
    static char[,] field = new char[Height, Width];
    static bool[,] ships = new bool[Height, Width];
    static int shipsCount = 3;

    static void Main(string[] args)
    {
        InitializeField();
        PlaceShips();
        GameLoop();
    }

    static void InitializeField()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                field[i, j] = '.';
                ships[i, j] = false;
            }
        }
    }

    static void PlaceShips()
    {
        Random rand = new Random();
        int placedShips = 0;

        while (placedShips < shipsCount)
        {
            int x = rand.Next(0, Height);
            int y = rand.Next(0, Width);

            if (!ships[x, y])
            {
                ships[x, y] = true;
                field[x, y] = 'S';
                placedShips++;
            }
        }
    }

    static void GameLoop()
    {
        bool gameRunning = true;

        while (gameRunning)
        {
            Console.Clear();
            DrawField();

            var shot = GetShot();
            if (shot.Item1 < 0 || shot.Item1 >= Height || shot.Item2 < 0 || shot.Item2 >= Width)
            {
                Console.WriteLine("Invalid coordinates!");
                continue;
            }

            if (ships[shot.Item1, shot.Item2])
            {
                field[shot.Item1, shot.Item2] = 'X';
                ships[shot.Item1, shot.Item2] = false;
                Console.WriteLine("Hit!");
            }
            else
            {
                field[shot.Item1, shot.Item2] = 'O';
                Console.WriteLine("Miss!");
            }

            if (IsGameOver())
            {
                Console.Clear();
                DrawField();
                Console.WriteLine("УИИИИИИИИ!");
                gameRunning = false;
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            System.Threading.Thread.Sleep(500);
        }
    }

    static (int, int) GetShot()
    {
        string input;
        int row, col;
        while (true)
        {
            Console.WriteLine("Enter coordinates: ");
            input = Console.ReadLine().ToUpper();

            if (input.Length == 2)
            {
                char columnChar = input[0];
                char rowChar = input[1];

                if (columnChar >= 'A' && columnChar < 'A' + Width && rowChar >= '1' && rowChar <= '5')
                {
                    row = rowChar - '1';
                    col = columnChar - 'A';
                    return (row, col);
                }
            }
        }
    }

    static bool IsGameOver()
    {
        foreach (var ship in ships)
        {
            if (ship) return false;
        }
        return true;
    }

    static void DrawField()
    {
        Console.WriteLine("Battlefield:");
        Console.WriteLine("   " + string.Join(" ", GetColumnLabels()));
        for (int i = 0; i < Height; i++)
        {
            Console.Write((i + 1).ToString().PadRight(2));
            for (int j = 0; j < Width; j++)
            {
                Console.Write(field[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static string[] GetColumnLabels()
    {
        char[] columns = new char[Width];
        for (int i = 0; i < Width; i++)
        {
            columns[i] = (char)('A' + i);
        }
        return columns.Select(c => c.ToString()).ToArray();
    }
}

