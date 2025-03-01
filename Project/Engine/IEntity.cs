using SFML.Graphics;

namespace SeaBattle.Project.Engine
{
    public interface IEntity
    {
        void Update(float deltaTime);
        void Draw(RenderTarget target);
    }
}
