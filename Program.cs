namespace SeaBattle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayerProfile player1Profile = PlayerProfile.LoadFromFile("Player 1");
            PlayerProfile player2Profile = PlayerProfile.LoadFromFile("Player 2");

            Console.WriteLine("=== Current Player Profiles ===");
            PrintProfile(player1Profile);
            PrintProfile(player2Profile);

            Console.WriteLine("=== Welcome to the Game ===");
            Console.WriteLine("Choose Game Mode:");
            Console.WriteLine("1. PvP");
            Console.WriteLine("2. PvE");
            Console.WriteLine("3. EvE");

            string mode;
            do
            {
                Console.Write("Enter your choice (1, 2, or 3): ");
                mode = Console.ReadLine();

                if (mode != "1" && mode != "2" && mode != "3")
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
            while (mode != "1" && mode != "2" && mode != "3");

            var game = new Game();
            switch (mode)
            {
                case "1":
                    game.Run(GameMode.PvP);
                    break;
                case "2":
                    game.Run(GameMode.PvE);
                    break;
                case "3":
                    game.Run(GameMode.EvE);
                    break;
            }

            Console.WriteLine("Game Over! Updating stats...");

            if (game.Winner == "Player 1")
            {
                player1Profile.UpdateStats(true);
                player2Profile.UpdateStats(false);
            }
            else
            {
                player1Profile.UpdateStats(false);
                player2Profile.UpdateStats(true);
            }

            player1Profile.SaveToFile();
            player2Profile.SaveToFile();

            Console.WriteLine("\n=== Updated player profiles ===");
            PrintProfile(player1Profile);
            PrintProfile(player2Profile);
        }

        static void PrintProfile(PlayerProfile profile)
        {
            Console.WriteLine($"Name: {profile.Name}");
            Console.WriteLine($"Wins: {profile.Wins}");
            Console.WriteLine($"Losses: {profile.Losses}");
            Console.WriteLine($"SetsWon: {profile.SetsWon}");
            Console.WriteLine($"SetsPlayed: {profile.SetsPlayed}");
            Console.WriteLine($"inRate: {profile.WinRate:F2}%\n");
        }
    }
}
