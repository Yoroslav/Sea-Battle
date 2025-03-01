using SFML.Graphics;
using System.Windows.Input;

namespace SeaBattle.Project.Engine
{
    public interface IGameRules
    {
        void Initialize();
        void Update(float deltaTime);
        void Draw(RenderTarget target);
        bool IsGameOver();
        void HandleInput(ICommand command);
    }
}
