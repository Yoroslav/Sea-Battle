using SFML.Graphics;
using SFML.System;

namespace SeaBattle.Project.Engine
{

    public abstract class InputHandler
    {
        public abstract ICommand HandleInput();
        public abstract Vector2f GetMousePosition(RenderWindow window);
    }
}