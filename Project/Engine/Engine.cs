using SeaBattle.Project.Engine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Engine
{
    private readonly IGameRules _gameRules;
    private readonly InputHandler _inputHandler;
    private readonly RenderWindow _window;

    public Engine(IGameRules gameRules, uint width = 800, uint height = 600, string title = "SeaBattle")
    {
        _gameRules = gameRules;
        _inputHandler = new KeyboardInputHandler();
        _window = new RenderWindow(new VideoMode(width, height), title);
        _window.Closed += (_, _) => _window.Close();
        _gameRules.Initialize();
    }

    public void Run()
    {
        Clock clock = new();
        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            float deltaTime = clock.Restart().AsSeconds();

            ICommand command = _inputHandler.HandleInput();
            if (command != null) _gameRules.HandleInput(command);

            _gameRules.Update(deltaTime);

            _window.Clear(Color.Blue);
            _gameRules.Draw(_window);
            _window.Display();
        }
    }
}