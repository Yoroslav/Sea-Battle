namespace SeaBattle
{
    public class Game
    {
        public const int Width = 2;
        public const int Height = 2;
        Field player1Field = new Field(Height, Width);
        Field player2Field = new Field(Height, Width);
        static int shipsCount = 8;
        const char ShipSymbol = 'S';
        const char EmptySymbol = '.';
        const char hitSymbol = 'X';
        const char missSymbol = 'O';

        (int, int) playerShot;
        bool isPlayer1Turn = true;
        bool radarActivated = false;

        int player1Wins = 0;
        int player2Wins = 0;
        const int MaxWins = 3;

        public void Run(bool isPvP, bool isAI = false, bool isEvE = false)
        {
            do
            {
                ResetGame();
                GenerateFields();

                while (!IsEndGame())
                {
                    Render(isPvP, isAI, isEvE);
                    PerformTurn(isPvP, isAI, isEvE);
                    Update();
                }

                if (player1Field.GetShipCount() == 0)
                {
                    player2Wins++;
                    Console.WriteLine(isEvE ? "AI 2 Wins!" : "Player 2 Wins!");
                }
                else
                {
                    player1Wins++;
                    Console.WriteLine(isEvE ? "AI 1 Wins!" : "Player 1 Wins!");
                }

                Console.WriteLine($"Score: Player 1 - {player1Wins}, Player 2 - {player2Wins}");
            } while (player1Wins < MaxWins && player2Wins < MaxWins);

            Console.WriteLine(player1Wins == MaxWins ? "Player 1 is the final winner!" : "Player 2 is the final winner!");
        }
        private void PerformTurn(bool isPvP, bool isAI, bool isEvE)
        {
            if (isEvE || (isAI && isPlayer1Turn))
            {
                AIShot();
            }
            else
            {
                GetInput();
            }
        }


        void ResetGame()
        {
            player1Field = new Field(Height, Width);
            player2Field = new Field(Height, Width);
            isPlayer1Turn = true;
            radarActivated = false;
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

        void Render(bool isPvP, bool isAI, bool isEvE)
        {
            Console.Clear();

            if (isPvP)
            {
                Console.WriteLine("Player 1's Battlefield:");
                DrawField(player1Field);
                Console.WriteLine("Player 2's Battlefield:");
                DrawField(player2Field);
            }
            else if (isAI)
            {
                Console.WriteLine("Your Battlefield:");
                DrawField(player1Field);
                Console.WriteLine("AI's Battlefield:");
                DrawField(player2Field, false);
            }
            else if (isEvE)
            {
                Console.WriteLine("AI 1's Battlefield:");
                DrawField(player1Field, false);
                Console.WriteLine("AI 2's Battlefield:");
                DrawField(player2Field, false);
            }
        }

        void GenerateFields()
        {
            Field[] fields = { player1Field, player2Field };
            foreach (var field in fields)
            {
                GenerateField(field);
            }
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

        bool IsEndGame() => player1Field.GetShipCount() == 0 || player2Field.GetShipCount() == 0;

        void GetInput()
        {
            Console.WriteLine(isPlayer1Turn ? "Player 1's Turn" : "Player 2's Turn");
            playerShot = GetShot();

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

        (int, int) GetShot()
        {
            string input;
            while (true)
            {
                Console.WriteLine("Enter coordinates: ");
                input = Console.ReadLine()?.ToUpper();

                if (!string.IsNullOrEmpty(input) && input.Length == 2)
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

        void ActivateRadar((int, int) shot)
        {
            radarActivated = true;

            Console.WriteLine("Choose radar range: 3x3 or 5x5 (enter 3 or 5):");
            string radarRangeInput = Console.ReadLine();

            int chosenRange = radarRangeInput == "5" ? 5 : 3;

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

        void DrawField(Field field, bool showShips = true)
        {
            Console.WriteLine("   " + string.Join(" ", GetColumnLabels()));

            for (int i = 0; i < Height; i++)
            {
                Console.Write((i + 1).ToString().PadRight(2));
                for (int j = 0; j < Width; j++)
                {
                    var cell = field.Cells[i, j];
                    char cellSymbol = cell.IsHitted switch
                    {
                        true when cell.HasShip => hitSymbol,
                        true => missSymbol,
                        false when showShips && cell.HasShip => ShipSymbol,
                        _ => EmptySymbol
                    };
                    Console.Write(cellSymbol + " ");
                }
                Console.WriteLine();
            }
        }

        void AIShot()
        {
            Random random = new Random();
            int x = random.Next(Width);
            int y = random.Next(Height);
            var shot = (y, x);

            Console.WriteLine($"AI shoots at: {GetColumnLabels()[x]}{y + 1}");
            ProcessShot(shot, isPlayer1Turn ? player2Field : player1Field);
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
}
