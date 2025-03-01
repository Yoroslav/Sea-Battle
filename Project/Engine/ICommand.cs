namespace SeaBattle.Project.Engine
{
    public interface ICommand
    {
        void Execute(IGameRules game);
    }
}
