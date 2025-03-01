public struct Cell
{
    public bool HasShip; 
    public bool IsHit;  
}

public class Field
{
    public Cell[,] Cells { get; private set; }
    public int Height { get; }
    public int Width { get; }

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
            if (cell.IsHit)
            {
                return false;
            }
            cell.IsHit = true;
            return cell.HasShip; 
        }
        return false;
    }

    public bool AreCoordinatesValid(int x, int y)
        => x >= 0 && y >= 0 && x < Height && y < Width;

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
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Cells[i, j].HasShip = false;
                Cells[i, j].IsHit = false;
            }
        }
    }
}