using SeaBattle.Game;
using SFML.Graphics;
using SFML.System;

namespace SeaBattle.Project.Game
{
    public class Player
    {
        public string Name { get; }
        public Field Field { get; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public Player(string name, int height, int width)
        {
            Name = name;
            Field = new Field(height, width);
        }

        public void Attack(int x, int y)
        {
            if (Field.AreCoordinatesValid(y, x))
            {
                Field.Attack(y, x);
            }
        }

        public void UseRadar(int range, Vector2f position)
        {
            int centerX = (int)(position.X / 40); 
            int centerY = (int)(position.Y / 40);

            for (int dy = -range; dy <= range; dy++)
            {
                for (int dx = -range; dx <= range; dx++)
                {
                    int x = centerX + dx;
                    int y = centerY + dy;

                    if (Field.AreCoordinatesValid(y, x) && Field.Cells[y, x].HasShip)
                    {
                        Field.Cells[y, x].IsRevealed = true;
                    }
                }
            }
        }
    }
}