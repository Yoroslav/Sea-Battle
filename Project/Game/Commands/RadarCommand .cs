using SeaBattle.Project.Game;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SeaBattle.Project.Game.Commands
{
    public class RadarCommand : ICommand
    {
        private readonly RenderWindow _window;
        private readonly SeaBattleGame _game;

        public RadarCommand(RenderWindow window, SeaBattleGame game)
        {
            _window = window;
            _game = game;
        }

        public void Execute(Player player)
        {
            var mousePos = Mouse.GetPosition(_window);
            int cellX = (int)((mousePos.X - 50) / 40);
            int cellY = (int)((mousePos.Y - 50) / 40);

            if (cellX >= 0 && cellX < 10 && cellY >= 0 && cellY < 10)
            {
                player.UseRadar(3, new Vector2f(mousePos.X, mousePos.Y)); 
            }
        }
    }
}