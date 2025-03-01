using SeaBattle.Game;
using SeaBattle.Project.Engine;
using SFML.Graphics;
using SFML.System;

namespace SeaBattle.Project.Game
{
    public class BattlefieldView : IEntity
    {
        private readonly RectangleShape[,] _cells; // Перейменовано з _grid на _cells
        private readonly Field _field;
        private readonly bool _showShips;

        public BattlefieldView(Field field, Vector2f position, bool showShips)
        {
            _field = field;
            _showShips = showShips;
            _cells = new RectangleShape[field.Height, field.Width]; // Ініціалізація
            InitializeGrid(position);
        }

        private void InitializeGrid(Vector2f position)
        {
            const int cellSize = 40;
            for (int y = 0; y < _field.Height; y++)
                for (int x = 0; x < _field.Width; x++)
                {
                    _cells[y, x] = new RectangleShape(new Vector2f(cellSize, cellSize))
                    {
                        Position = position + new Vector2f(x * cellSize, y * cellSize),
                        FillColor = GetCellColor(y, x)
                    };
                }
        }

        private Color GetCellColor(int y, int x)
        {
            if (_field.Cells[y, x].IsHit)
                return Color.Red;
            if (_field.Cells[y, x].IsRevealed && _field.Cells[y, x].HasShip)
                return Color.Yellow;
            if (_showShips && _field.Cells[y, x].HasShip)
                return Color.Blue;
            return Color.White;
        }

        public void Update(float deltaTime)
        {
          
        }

        public void Draw(RenderTarget target)
        {
            foreach (var cell in _cells) target.Draw(cell);
        }
    }
}