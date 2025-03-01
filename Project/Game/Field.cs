namespace SeaBattle.Game
{
    public class Field
    {
        public Cell[,] Cells { get; }
        public int Height { get; }
        public int Width { get; }

        public Field(int height, int width)
        {
            Height = height;
            Width = width;
            Cells = new Cell[height, width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Cells[y, x] = new Cell();
                }
        }

        public bool AreCoordinatesValid(int y, int x)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public bool Attack(int y, int x)
        {
            if (AreCoordinatesValid(y, x))
            {
                Cells[y, x].IsHit = true;
                return Cells[y, x].HasShip;
            }
            return false;
        }

        public int GetShipCount()
        {
            int count = 0;
            foreach (var cell in Cells)
            {
                if (cell.HasShip && !cell.IsHit)
                {
                    count++;
                }
            }
            return count;
        }

        public void Reset()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    Cells[y, x] = new Cell();
                }
        }

        public void PlaceShip(int y, int x)
        {
            if (AreCoordinatesValid(y, x))
            {
                Cells[y, x].HasShip = true;
            }
        }
    }

    public struct Cell
    {
        public bool HasShip;
        public bool IsHit;
        public bool IsRevealed;
    }
}