using SeaBattle.Project.Engine;
namespace SeaBattle.Project.Game.Commands
{
    public class RadarCommand : ICommand
    {
        public void Execute(IGameRules game)
        {
            if (game is SeaBattleGame seaBattle)
                seaBattle.ActivateRadar(3);
        }
    }
}
