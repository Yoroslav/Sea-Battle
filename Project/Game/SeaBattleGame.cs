using SeaBattle.Project.Engine;
using SFML.Graphics;

public class SeaBattleGame : IGameRules
{
    private Field _playerField = new(10, 10);
    private Field _enemyField = new(10, 10);
    private bool _isGameOver;

    public void Initialize()
    {
        _playerField.Reset();
        _enemyField.Reset();
        _playerField.PlaceShip(3, 4);
        _enemyField.PlaceShip(5, 2);
    }

    public void Update(float deltaTime)
        => _isGameOver = _playerField.GetShipCount() == 0 || _enemyField.GetShipCount() == 0;

    public void Draw(RenderTarget target)
    {
        new BattlefieldView("Гравець", true, _playerField).Draw(target);
        new BattlefieldView("Ворог", false, _enemyField).Draw(target);
    }

    public bool IsGameOver() => _isGameOver;

    public void HandleInput(ICommand command) => command?.Execute(this);

    public void ProcessAttack((int x, int y) coordinates)
        => _enemyField.Attack(coordinates.x, coordinates.y);

    public void ActivateRadar(int range)
        => Console.WriteLine($"Радар {range}x{range} активовано!");
}