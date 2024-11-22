using System;
using System.Linq;

class Cell
{
    public bool HasShip;
    public bool IsHitted;

    public Cell()
    {
        HasShip = false;
        IsHitted = false;
    }
}

class Field
{
    public Cell[,] Cells;
    public int Height;
    public int Width;

    public Field(int height, int width)
    {
        Height = height;
        Width = width;
        Cells = new Cell[Height, Width];

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Cells[i, j] = new Cell();
            }
        }
    }

    public void PlaceShip(int x, int y)
    {
        if (x >= 0 && x < Height && y >= 0 && y < Width)
        {
            Cells[x, y].HasShip = true;
        }
    }

    public bool Attack(int x, int y)
    {
        if (x >= 0 && x < Height && y >= 0 && y < Width)
        {
            var cell = Cells[x, y];
            if (cell.IsHitted)
            {
                return false; 
            }
            cell.IsHitted = true;
            return cell.HasShip;
        }
        return false; 
    }
}

class Game
{
    const int Width = 5;
    const int Height = 5;
    Field field;

    static int shipsCount = 3;
    static int numberofAttempts = 10;

    const char ShipSymbol = 'I';
    const char EmptySymbol = '_';
    const char hitSymbol = '9';
    const char missSymbol = '1';

    (int, int) playerShot;

    static void Main(string[] args)
    {
        var game = new Game();
        game.Run();
    }

    void Run()
    {
        field = new Field(Height, Width);  
        GenerateField();

        while (!IsEndGame())
        {
            Render();
            GetInput();
            Update();
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
    void Render()
    {
        Console.Clear();
        DrawField();
    }

    void Update()
    {
        ProcessShot(playerShot);
        numberofAttempts--;
    }
    void GetInput()
    {
        Console.WriteLine($"Number of attempts: {numberofAttempts}");
        playerShot = GetShot();
        if (!IsValidShot(playerShot))
        {
            Console.WriteLine("Invalid coordinates!");
            return;
        }
    }

    void GenerateField()
    {
        Random random = new Random();
        for (int i = 0; i < shipsCount; i++)
        {
            int x, y;
            do
            {
                x = random.Next(Width);
                y = random.Next(Height);
            } while (field.Cells[y, x].HasShip);

            field.PlaceShip(x, y);
        }
    }

    bool IsValidShot((int, int) shot)
    {
        var (x, y) = shot;
        return y >= 0 && y < Height && x >= 0 && x < Width;
    }

    void ProcessShot((int, int) shot)
    {
        if (field.Attack(shot.Item1, shot.Item2))
        {
            Console.WriteLine("Hit!");
            shipsCount--;
        }
        else
        {
            Console.WriteLine("Miss!");
        }
        System.Threading.Thread.Sleep(750);
    }

    public (int, int) GetShot()
    {
        string input;
        while (true)
        {
            Console.WriteLine("Enter coordinates: ");
            input = Console.ReadLine()?.ToUpper();

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
                char cell = field.Cells[i, j].IsHitted switch
                {
                    true when field.Cells[i, j].HasShip => hitSymbol,
                    true => missSymbol,
                    false when field.Cells[i, j].HasShip => ShipSymbol,
                    _ => EmptySymbol
                };
                Console.Write(cell + " ");
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
