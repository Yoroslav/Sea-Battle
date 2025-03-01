using SeaBattle.Game;
using SeaBattle.Project.Game;
using SFML.Graphics;
using System;
using System.Windows.Input;

namespace SeaBattle.Project.Engine
{
    public interface IGameRules
    {
        bool IsGameOver { get; }
        Player CurrentPlayer { get; }
        void ProcessTurn();
        void Initialize();
        void HandleInput(ICommand command);
        void Update(float deltaTime);
        void Draw(RenderTarget target);
    }
}
