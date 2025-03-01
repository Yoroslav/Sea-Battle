using SeaBattle.Project.Game;
using SeaBattle.Project.Game.Commands;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SeaBattle.Project.Engine
{
    public class KeyboardInputHandler : InputHandler
    {
        private readonly RenderWindow _window;
        private readonly SeaBattleGame _game;

        public KeyboardInputHandler(RenderWindow window, SeaBattleGame game)
        {
            _window = window;
            _game = game;
        }

        public override Vector2f GetMousePosition(RenderWindow window)
        {
            return (Vector2f)Mouse.GetPosition(window);
        }

        public override ICommand HandleInput()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                return new AttackCommand(_window, _game);

            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                return new RadarCommand(_window, _game);

            return null;
        }
    }
}