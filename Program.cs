using System;
using System.Linq;

class Game
{
    const int Width = 5;
    const int Height = 5;
    const char Ship = 'S';
    static char[,] field = new char[Height, Width];
    static int shipsCount = 3;
    static int numberofAttempts = 10;  

    static void Main(string[] args)
    {
        var game = new Game();
        game.Run();
    }

    void Run()
    {
        GenerateField();

        while (!IsEndGame())
        {
            Console.Clear();
            DrawField();

            Console.WriteLine($"number of attempts: {numberofAttempts}");  

            if (numberofAttempts > 0)
            {
                var shot = GetShot();
                if (!IsValidShot(shot))
                {
                    Console.WriteLine("Invalid coordinates!");
                    continue;
                }

                ProcessShot(shot);
                numberofAttempts--; 
            }
            System.Threading.Thread.Sleep(750);
        }

        if (shipsCount == 0)
        {
            Console.WriteLine("УИИИИИИИИ");
        }
        else
        {
            Console.WriteLine("Game over");
        }
    }

    bool IsEndGame() => shipsCount == 0 || numberofAttempts <= 0; 

    void GenerateField()
    {
        Random random = new Random();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                field[i, j] = '.';
            }
        }
        for (int i = 0; i < shipsCount; i++)
        {
            int x, y;
            do
            {
                x = random.Next(Width);
                y = random.Next(Height);
            } while (field[y, x] == Ship);

            field[y, x] = Ship;
        }
    }

    bool IsValidShot((int, int) shot)
    {
        return shot.Item1 >= 0 && shot.Item1 < Height && shot.Item2 >= 0 && shot.Item2 < Width;
    }

    void ProcessShot((int, int) shot)
    {
        if (field[shot.Item1, shot.Item2] == Ship)
        {
            field[shot.Item1, shot.Item2] = 'X';
            shipsCount--;
            Console.WriteLine("Hit!");
        }
        else
        {
            field[shot.Item1, shot.Item2] = 'O';
            Console.WriteLine("Miss!");
        }
    }

    (int, int) GetShot()
    {
        string input;
        while (true)
        {
            Console.WriteLine("Enter coordinates: ");
            input = Console.ReadLine().ToUpper();

            if (input.Length == 2)
            {
                char columnChar = input[0];
                char rowChar = input[1];

                int row = rowChar - '1';
                int col = columnChar - 'A';

                if (col >= 0 && col < Width && row >= 0 && row < Height)
                {
                    return (row, col);
                }
            }
        }
    }

    void DrawField()
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

    string[] GetColumnLabels()
    {
        char[] columns = new char[Width];
        for (int i = 0; i < Width; i++)
        {
            columns[i] = (char)('A' + i);
        }
        return columns.Select(c => c.ToString()).ToArray();
    }
}

