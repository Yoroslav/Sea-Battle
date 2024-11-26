internal class Player
{
    public (int, int) GetShot()
    {
        string input;
        while (true)
        {
            Console.WriteLine("Enter coordinates: ");
            input = Console.ReadLine()?.ToUpper();

            if (!string.IsNullOrEmpty(input) && input.Length == 2)
            {
                char columnChar = input[0];
                char rowChar = input[1];

                int row = rowChar - '1';
                int col = columnChar - 'A';

                if (col >= 0 && col < Game.Width && row >= 0 && row < Game.Height)
                {
                    return (row, col);
                }
            }
        }
    }
}
