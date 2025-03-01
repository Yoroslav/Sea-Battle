using SFML.Graphics;
using SFML.System;

namespace SeaBattle.Project.Engine
{
    public static class Vector2Extensions
    {
        public static Vector2f ToWorldPos(this Vector2i screenPos, RenderWindow window)
            => window.MapPixelToCoords(screenPos);
    }
}