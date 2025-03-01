namespace SeaBattle.Game.Animations
{
    public class State
    {
        public string Name { get; }
        public int TotalFrames { get; }
        public int Row { get; }

        public State(string name, int totalFrames, int row)
        {
            Name = name;
            TotalFrames = totalFrames;
            Row = row;
        }
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update(float deltaTime) { }
    }
}