using SeaBattle.Project.Engine;

namespace SeaBattle.Project.Game.Commands
{
    public class AttackCommand : ICommand
    {
        public void Execute(IGameRules game)
        {
            if (game is SeaBattleGame seaBattle)
                seaBattle.ProcessAttack((2, 3));
        }
    }
}
