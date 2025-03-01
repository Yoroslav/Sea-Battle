using SFML.System;

namespace SeaBattle.Project.Engine
{
    public static class Vector2fExtensions
    {
        public static Vector2f ToTilePosition(this Vector2f worldPos, int tileSize)
            => new(worldPos.X / tileSize, worldPos.Y / tileSize);
    }
}
