internal class Player
{
    public string Name;
    public Field Field;
    public int Wins;

    public Player(string name, int height, int width)
    {
        Name = name;
        Field = new Field(height, width);
        Wins = 0;
    }
}
