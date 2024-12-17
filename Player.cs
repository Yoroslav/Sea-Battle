internal class Player
{
    public string Name;
    public Field Field;
    public int Wins;
    public int Losses;

    public Player(PlayerProfile profile, int height, int width)
    {
        Name = profile.Name;
        Field = new Field(height, width);
        Wins = profile.Wins;
        Losses = profile.Losses;
    }
}
