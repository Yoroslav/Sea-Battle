using SeaBattle;

struct Cell
{
    public bool HasShip;
    public bool IsHitted;
}

class Game
{
    public const int Width = 5;
    public const int Height = 5;
    Field player1Field = new Field(Height, Width);
    Field player2Field = new Field(Height, Width);

    static int shipsCount = 1;

    const char ShipSymbol = 'S';
    const char EmptySymbol = '.';
    const char hitSymbol = 'X';
    const char missSymbol = 'O';

    Player player1 = new Player();
    Player player2 = new Player();
    (int, int) playerShot;

    bool isPlayer1Turn = true; 

    bool radarActivated = false; 

    public void Run()
    {
        GenerateFields(); 

        while (!IsEndGame())
        {
            Render();
            GetInput();
            Update();


        }
        Render();
        Console.WriteLine(player1Field.GetShipCount() == 0 ? "Player 2 Wins!" : "Player 1 Wins!");
    }

    bool IsEndGame() => player1Field.GetShipCount() == 0 || player2Field.GetShipCount() == 0;

    void Render()
    {
        Console.Clear();
        DrawField();
    }

    void Update()
    {
        if (isPlayer1Turn)
        {
            ProcessShot(playerShot, player2Field);
        }
        else
        {
            ProcessShot(playerShot, player1Field);
        }

        isPlayer1Turn = !isPlayer1Turn;
        radarActivated = false;
    }

    void GenerateFields()
    {
        GenerateField(player1Field);
        GenerateField(player2Field); 
    }

    void GenerateField(Field field)
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

    void GetInput()
    {
        Console.WriteLine(isPlayer1Turn ? "Player 1's Turn" : "Player 2's Turn");

        if (isPlayer1Turn)
        {
            playerShot = player1.GetShot();
        }
        else
        {
            playerShot = player2.GetShot();
        }

        if (!radarActivated)
        {
            Console.WriteLine("Do you want to use radar to reveal a 3x3 or 5x5 area? (y/n)");
            string radarInput = Console.ReadLine()?.ToLower();

            if (radarInput == "y")
            {
                ActivateRadar(playerShot);
            }
        }

        if (!IsValidShot(playerShot))
        {
            Console.WriteLine("Invalid coordinates!");
        }
    }

    void ActivateRadar((int, int) shot)
    {
        radarActivated = true;

        Console.WriteLine("Choose radar range: 3x3 or 5x5 (enter 3 or 5):");
        string radarRangeInput = Console.ReadLine();

        int chosenRange = 3;
        if (radarRangeInput == "5")
        {
            chosenRange = 5;
        }
        if (isPlayer1Turn)
        {
            ShowRadar(shot, chosenRange, player2Field);
        }
        else
        {
            ShowRadar(shot, chosenRange, player1Field);
        }
    }


    void ShowRadar((int, int) shot, int range, Field enemyField)
    {
        int startX = shot.Item1 - range / 2;
        int startY = shot.Item2 - range / 2;

        startX = Math.Max(0, startX);
        startY = Math.Max(0, startY);

        int endX = Math.Min(Width - 1, shot.Item1 + range / 2);
        int endY = Math.Min(Height - 1, shot.Item2 + range / 2);
        for (int i = startX; i <= endX; i++)
        {
            for (int j = startY; j <= endY; j++)
            {
                char cell = enemyField.Cells[i, j].HasShip ? ShipSymbol : EmptySymbol;
                Console.Write(cell + " ");
            }
            Console.WriteLine();
        }
    }


    bool IsValidShot((int, int) shot)
    {
        var (x, y) = shot;
        return y >= 0 && y < Height && x >= 0 && x < Width;
    }

    void ProcessShot((int, int) shot, Field targetField)
    {
        if (targetField.Attack(shot.Item1, shot.Item2))
        {
            Console.WriteLine("Hit!");
            shipsCount--;
        }
        else
        {
            Console.WriteLine("Miss!");
        }
        System.Threading.Thread.Sleep(1000);
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
                Field currentField = isPlayer1Turn ? player1Field : player2Field;
                var fieldCells = currentField.Cells[i, j];
                char cell = fieldCells.IsHitted switch
                {
                    true when fieldCells.HasShip => hitSymbol,
                    true => missSymbol,
                    false when fieldCells.HasShip => ShipSymbol,
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
