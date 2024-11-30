namespace SeaBattle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
        var game = new Game();
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
                Console.WriteLine("Invalid input.");
            }
        }
        while (mode != "1" && mode != "2" && mode != "3");

        switch (mode)
        {
            case "1":
                game.Run(isPvP: true);
                break;
            case "2":
                game.Run(isPvP: false, isAI: true);
                break;
            case "3":
                game.Run(isPvP: false, isAI: true, isEvE: true);
                break;
        }

        }
    }
}
