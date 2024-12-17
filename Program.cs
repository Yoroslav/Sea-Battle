namespace SeaBattle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Welcome to Sea Battle ===");

            Console.WriteLine("\nPlayer 1: Select or Create Profile");
            PlayerProfile player1Profile = Profile.SelectOrCreateProfile();

            Console.WriteLine("\nPlayer 2: Select or Create Profile");
            PlayerProfile player2Profile = Profile.SelectOrCreateProfile();

            Console.WriteLine("\n=== Current Player Profiles ===");
            Profile.PrintProfile(player1Profile);
            Profile.PrintProfile(player2Profile);

            Console.WriteLine("\n=== Choose Game Mode ===");
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

            Console.WriteLine("\n=== Updated Player Profiles ===");
            Profile.PrintProfile(player1Profile);
            Profile.PrintProfile(player2Profile);
        }
    }
}
