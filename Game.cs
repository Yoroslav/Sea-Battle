using Newtonsoft.Json;
namespace SeaBattle
{
    public class Game
    {
        public const int Width = 2;
        public const int Height = 2;
        static int shipsCount = 8;
        const char ShipSymbol = 'S';
        const char EmptySymbol = '.';
        const char hitSymbol = 'X';
        const char missSymbol = 'O';

        (int, int) playerShot;
        bool isPlayer1Turn = true;
        bool radarActivated = false;

        const int MaxWins = 3;
        Player player1;
        Player player2;
        PlayerProfile player1Profile;
        PlayerProfile player2Profile;
        public string Winner { get; private set; }

        public Game()
        {
            player1 = new Player("Player 1", Height, Width);
            player2 = new Player("Player 2", Height, Width);

            player1Profile = LoadProfile(player1.Name);
            player2Profile = LoadProfile(player2.Name);
        }

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

                var winner = player1.Field.GetShipCount() == 0 ? player2 : player1;
                winner.Wins++;

                if (winner == player1)
                {
                    player1Profile.UpdateStats(true);  
                    player2Profile.UpdateStats(false); 
                }
                else
                {
                    player2Profile.UpdateStats(true);  
                    player1Profile.UpdateStats(false); 
                }
                if (isPvP)
                {
                    Winner = "Player 1";
                }
                else
                {
                    Winner = "Player 2"; 
                }

                Console.WriteLine($"{winner.Name} Wins!");
                Console.WriteLine($"Score: {player1.Name} - {player1.Wins}, {player2.Name} - {player2.Wins}");
            } while (player1.Wins < MaxWins && player2.Wins < MaxWins);


            SaveProfile(player1Profile);
            SaveProfile(player2Profile);

            Console.WriteLine(player1.Wins == MaxWins ? $"{player1.Name} is the final winner!" : $"{player2.Name} is the final winner!");
        }

        private void SaveProfile(PlayerProfile profile)
        {
            string filePath = $"{profile.Name}_profile.json";
            string json = JsonConvert.SerializeObject(profile, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private PlayerProfile LoadProfile(string playerName)
        {
            string filePath = $"{playerName}_profile.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<PlayerProfile>(json);
            }
            else
            {
                Console.WriteLine("Profile not found. Creating a new one.");
                return new PlayerProfile(playerName);
            }
        }

        private void ResetGame()
        {
            player1.Field.Reset();
            player2.Field.Reset();
            isPlayer1Turn = true;
            radarActivated = false;
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

        private void Update()
        {
            var targetField = isPlayer1Turn ? player2.Field : player1.Field;
            ProcessShot(playerShot, targetField);

            isPlayer1Turn = !isPlayer1Turn;
            radarActivated = false;
        }

        private void Render(bool isPvP, bool isAI, bool isEvE)
        {
            Console.Clear();

            ShowBattlefield(player1.Name + "'s Battlefield", player1.Field, isPvP || isAI || isEvE);
            ShowBattlefield(player2.Name + "'s Battlefield", player2.Field, isPvP || isEvE);
        }

        private void ShowBattlefield(string title, Field field, bool showShips)
        {
            Console.WriteLine(title);
            DrawField(field, showShips);
            Console.WriteLine();
        }

        private void GenerateFields()
        {
            GenerateField(player1.Field);
            GenerateField(player2.Field);
        }

        private void GenerateField(Field field)
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

        private bool IsEndGame() => player1.Field.GetShipCount() == 0 || player2.Field.GetShipCount() == 0;

        private void GetInput()
        {
            Console.WriteLine(isPlayer1Turn ? $"{player1.Name}'s Turn" : $"{player2.Name}'s Turn");
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

        private (int, int) GetShot()
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

        private void ActivateRadar((int, int) shot)
        {
            radarActivated = true;

            Console.WriteLine("Choose radar range: 3x3 or 5x5 (enter 3 or 5):");
            string radarRangeInput = Console.ReadLine();

            int chosenRange = radarRangeInput == "5" ? 5 : 3;

            var enemyField = isPlayer1Turn ? player2.Field : player1.Field;
            ShowRadar(shot, chosenRange, enemyField);
        }

        private void ShowRadar((int, int) shot, int range, Field enemyField)
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

        private bool IsValidShot((int, int) shot)
        {
            var (x, y) = shot;
            return y >= 0 && y < Height && x >= 0 && x < Width;
        }

        private void ProcessShot((int, int) shot, Field targetField)
        {
            if (targetField.Attack(shot.Item1, shot.Item2))
            {
                Console.WriteLine("Hit!");
            }
            else
            {
                Console.WriteLine("Miss!");
            }
            System.Threading.Thread.Sleep(1000);
        }

        private void AIShot()
        {
            Random random = new Random();
            int x = random.Next(Width);
            int y = random.Next(Height);
            var shot = (y, x);

            Console.WriteLine($"AI shoots at: {GetColumnLabels()[x]}{y + 1}");
            ProcessShot(shot, isPlayer1Turn ? player2.Field : player1.Field);
        }

        private void DrawField(Field field, bool showShips = true)
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

        private string[] GetColumnLabels()
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
