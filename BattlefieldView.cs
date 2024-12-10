namespace SeaBattle
{
    internal class BattlefieldView
    {
        public bool ShowShips;
        public string Name;
        public Field Field;

        public BattlefieldView(string name, bool showShips, Field field)
        {
            Name = name;
            ShowShips = showShips;
            Field = field;
        }

    }
}
