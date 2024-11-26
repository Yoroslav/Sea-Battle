namespace SeaBattle
{
    internal class Field
    {
        public Cell[,] Cells;
        public int Height;
        public int Width;

        public Field(int height, int width)
        {
            Height = height;
            Width = width;
            Cells = new Cell[Height, Width];
        }

        public void PlaceShip(int x, int y)
        {
            if (AreCoordinatesValid(x, y))
            {
                Cells[x, y].HasShip = true;
            }
        }

        public bool Attack(int x, int y)
        {
            if (AreCoordinatesValid(x, y))
            {
                ref var cell = ref Cells[x, y];
                if (cell.IsHitted)
                {
                    return false;
                }
                cell.IsHitted = true;
                return cell.HasShip;
            }
            return false;
        }


        public int GetShipCount()
        {
            int count = 0;
            foreach (var cell in Cells)
            {
                if (cell.HasShip && !cell.IsHitted) 
                {
                    count++;
                }
            }
            return count;
        }

        private bool AreCoordinatesValid(int x, int y) => x >= 0 && x < Height && y >= 0 && y < Width;
    }
}
