class Profile
{
    public static PlayerProfile SelectOrCreateProfile()
    {
        List<string> existingProfiles = PlayerProfile.GetAllProfiles();

        Console.WriteLine("\nAvailable Profiles:");
        if (existingProfiles.Count > 0)
        {
            for (int i = 0; i < existingProfiles.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {existingProfiles[i]}");
            }
        }
        else
        {
            Console.WriteLine("No profiles found.");
        }

        Console.WriteLine("Enter the number to select a profile or type a new name to create one:");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int choice) && choice > 0 && choice <= existingProfiles.Count)
        {
            string selectedProfileName = existingProfiles[choice - 1];
            return PlayerProfile.LoadFromFile(selectedProfileName);
        }
        else
        {
            Console.WriteLine("Creating a new profile...");
            string newProfileName = input;
            PlayerProfile newProfile = new PlayerProfile(newProfileName);
            newProfile.SaveToFile();
            return newProfile;
        }
    }

    public static void PrintProfile(PlayerProfile profile)
    {
        Console.WriteLine($"Name: {profile.Name}");
        Console.WriteLine($"Wins: {profile.Wins}");
        Console.WriteLine($"Losses: {profile.Losses}");
        Console.WriteLine($"Sets Won: {profile.SetsWon}");
        Console.WriteLine($"Sets Played: {profile.SetsPlayed}");
        Console.WriteLine($"Win Rate: {profile.WinRate:F2}%\n");
    }
}
