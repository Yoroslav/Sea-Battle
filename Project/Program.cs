using SeaBattle.Project.Engine;

static class Program
{
    static void Main()
    {
        IGameRules seaBattle = new SeaBattleGame();
        Engine engine = new(seaBattle);
        engine.Run();
    }
}