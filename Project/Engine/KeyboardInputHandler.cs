using SeaBattle.Project.Game.Commands;
using SFML.Window;
using System.Windows.Input;

namespace SeaBattle.Project.Engine
{
    public class KeyboardInputHandler : InputHandler
    {
        public override ICommand HandleInput()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.R)) return new RadarCommand();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) return new AttackCommand();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) Environment.Exit(0);
            return null;
        }
    }

}
