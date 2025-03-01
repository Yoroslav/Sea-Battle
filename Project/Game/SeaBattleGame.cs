using SeaBattle.Game;
using SeaBattle.Project.Engine;
using SFML.Graphics;
using SFML.System;
using System;

namespace SeaBattle.Project.Game
{
    public class SeaBattleGame : IGameRules
    {
        private readonly BattlefieldView _player1View;
        private readonly BattlefieldView _player2View;
        private readonly Player _player1;
        private readonly Player _player2;
        private bool _isPlayer1Turn = true;
        private readonly Text _statusText;
        private readonly Font _font;

        public bool IsGameOver => _player1.Field.GetShipCount() == 0 || _player2.Field.GetShipCount() == 0;
        public Player CurrentPlayer => _isPlayer1Turn ? _player1 : _player2;

        public SeaBattleGame(RenderWindow window)
        {
            _player1 = new Player("Player 1", 10, 10);
            _player2 = new Player("Player 2", 10, 10);

            _player1View = new BattlefieldView(_player1.Field, new Vector2f(50, 50), true);
            _player2View = new BattlefieldView(_player2.Field, new Vector2f(500, 50), false);

            _font = new Font("arial.ttf");
            _statusText = new Text
            {
                Font = _font,
                CharacterSize = 24,
                FillColor = Color.White,
                Position = new Vector2f(50, 450)
            };
            UpdateStatusText();
        }

        public void Initialize()
        {
            _player1.Field.Reset();
            _player2.Field.Reset();
            PlaceShips(_player1.Field);
            PlaceShips(_player2.Field);
        }

        private void PlaceShips(Field field)
        {
            var random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(10);
                    y = random.Next(10);
                } while (field.Cells[y, x].HasShip);

                field.PlaceShip(y, x);
            }
        }

        public void Update(float deltaTime)
        {
            _player1View.Update(deltaTime);
            _player2View.Update(deltaTime);
        }

        public void Draw(RenderTarget target)
        {
            _player1View.Draw(target);
            _player2View.Draw(target);
            target.Draw(_statusText);
        }

        public void ProcessTurn()
        {
            _isPlayer1Turn = !_isPlayer1Turn;
            UpdateStatusText();
        }

        public void HandleInput(ICommand command)
        {
            command.Execute(CurrentPlayer);
            if (IsGameOver)
            {
                _statusText.DisplayedString = $"{CurrentPlayer.Name} переміг!";
            }
        }

        public void ProcessAttack((int x, int y) coordinates)
        {
            var targetField = _isPlayer1Turn ? _player2.Field : _player1.Field;
            bool isHit = targetField.Attack(coordinates.y, coordinates.x);

            if (isHit)
            {
                _statusText.DisplayedString = $"{CurrentPlayer.Name} влучив у ({coordinates.x}, {coordinates.y})!";
            }
            else
            {
                _statusText.DisplayedString = $"{CurrentPlayer.Name} промахнувся по ({coordinates.x}, {coordinates.y})!";
            }

            ProcessTurn();
        }

        private void UpdateStatusText()
        {
            _statusText.DisplayedString = $"Хід гравця: {CurrentPlayer.Name}\n" +
                                         $"Кораблі гравця 1: {_player1.Field.GetShipCount()}\n" +
                                         $"Кораблі гравця 2: {_player2.Field.GetShipCount()}";
        }
    }
}