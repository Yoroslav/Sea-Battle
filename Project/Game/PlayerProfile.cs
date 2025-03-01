using Newtonsoft.Json;
public class PlayerProfile
{
    public string Name;
    public int Wins;
    public int Losses;
    public int SetsWon;
    public int SetsPlayed;
    public double WinRate;

    public PlayerProfile(string name)
    {
        Name = name;
        Wins = 0;
        Losses = 0;
        SetsWon = 0;
        SetsPlayed = 0;
        WinRate = 0;
    }

    public void UpdateStats(bool isWinner)
    {
        if (isWinner)
        {
            Wins++;
        }
        else
        {
            Losses++;
        }

        SetsPlayed++;
        WinRate = SetsPlayed > 0 ? (double)Wins / SetsPlayed * 100 : 0;
    }

    public void SaveToFile()
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText($"{Name}_profile.json", json);
        Console.WriteLine($"Player Profile {Name} saved!");
    }

    public static PlayerProfile LoadFromFile(string name)
    {
        string filePath = $"{name}_profile.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Console.WriteLine($"Player Profile {name} loaded!");
            return JsonConvert.DeserializeObject<PlayerProfile>(json);
        }
        else
        {
            Console.WriteLine($"File {filePath} not found. Creating a new profile");
            return new PlayerProfile(name);
        }
    }

    public static List<string> GetAllProfiles()
    {
        List<string> profiles = new List<string>();
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_profile.json");

        foreach (var file in files)
        {
            string profileName = Path.GetFileNameWithoutExtension(file).Replace("_profile", "");
            profiles.Add(profileName);
        }

        return profiles;
    }
}
