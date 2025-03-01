using SeaBattle.Project.Engine;
using SFML.Graphics;
using SFML.System;

public class BattlefieldView : IEntity
{
    private readonly Sprite _gridSprite;
    private readonly Texture _shipTexture;
    public Field Field { get; }

    public BattlefieldView(string name, bool showShips, Field field)
    {
        Field = field;
        _gridSprite = new Sprite(new Texture("Resources/Grid.png"));
        _shipTexture = new Texture("Resources/SpriteSheets/Ship.png");
    }

    public void Update(float deltaTime) { }

    public void Draw(RenderTarget target)
    {
        for (int i = 0; i < Field.Height; i++)
        {
            for (int j = 0; j < Field.Width; j++)
            {
                _gridSprite.Position = new Vector2f(j * 64, i * 64);
                target.Draw(_gridSprite);

                if (Field.Cells[i, j].HasShip)
                {
                    Sprite ship = new(_shipTexture) { Position = new Vector2f(j * 64, i * 64) };
                    target.Draw(ship);
                }
            }
        }
    }
}