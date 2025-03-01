using SeaBattle.Game;
using SeaBattle.Project.Engine;
using SFML.Graphics;
using SFML.System;

public class BattlefieldView : IEntity
{
    private readonly RectangleShape[,] _cells;
    private readonly Texture _shipTexture;
    private readonly Vector2f _position;
    private readonly Field _field;
    private readonly bool _showShips;

    public BattlefieldView(Field field, Vector2f position, bool showShips)
    {
        _field = field;
        _cells = new RectangleShape[field.Height, field.Width];
        _position = position;
        _showShips = showShips;
        _shipTexture = ResourceLoader.LoadShipTexture();

        InitializeGrid(field, showShips);
    }
    
    private void InitializeGrid(Field field, bool showShips)
    {
        const int cellSize = 40;
        for (int i = 0; i < field.Height; i++)
        {
            for (int j = 0; j < field.Width; j++)
            {
                var cell = new RectangleShape(new Vector2f(cellSize, cellSize))
                {
                    Position = new Vector2f(
                        _position.X + j * cellSize,
                        _position.Y + i * cellSize
                    ),
                    OutlineThickness = 1,
                    OutlineColor = Color.Black,
                    FillColor = GetCellColor(i, j) 
                };

                if (showShips && field.Cells[i, j].HasShip)
                {
                    cell.Texture = _shipTexture;
                }

                _cells[i, j] = cell;
            }
        }
    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < _field.Height; i++)
        {
            for (int j = 0; j < _field.Width; j++)
            {
                _cells[i, j].FillColor = GetCellColor(i, j);

                if (_field.Cells[i, j].IsHit)
                {
                    _cells[i, j].OutlineColor = Color.Red;
                }
                else if (_field.Cells[i, j].IsRevealed && _field.Cells[i, j].HasShip)
                {
                    _cells[i, j].OutlineColor = Color.Yellow;
                }
                else
                {
                    _cells[i, j].OutlineColor = Color.Black;
                }
            }
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

    public void Draw(RenderTarget target)
    {
        foreach (var cell in _cells)
        {
            target.Draw(cell);
        }
    }
}