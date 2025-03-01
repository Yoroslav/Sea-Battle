using SeaBattle.Project.Engine;
using SeaBattle.Project.Game;
using SFML.Graphics;
using SFML.Window;

static class Program
{
    static void Main()
    {
        var window = new RenderWindow(new VideoMode(800, 600), "Sea Battle");
        IGameRules seaBattle = new SeaBattleGame(window);
        Engine engine = new Engine(seaBattle, window);
        engine.Run();
    }
}