

namespace SeaBattle.Game.Animations
{
    public class Transition
    {
        public State From { get; }
        public State To { get; }
        public Func<bool> Condition { get; }

        public Transition(State from, State to, Func<bool> condition)
        {
            From = from;
            To = to;
            Condition = condition;
        }
    }
}