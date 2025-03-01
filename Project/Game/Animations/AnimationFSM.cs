using SeaBattle.Game.Animations;

public class AnimationFSM
{
    private State _currentState;
    private readonly Dictionary<string, State> _states = new();

    public void AddState(string name, State state) => _states[name] = state;

    public void TransitionTo(string stateName)
    {
        _currentState?.Exit();
        _currentState = _states[stateName];
        _currentState.Enter();
    }

    public void Update(float deltaTime) => _currentState?.Update(deltaTime);
}