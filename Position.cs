namespace Tetris
{
    internal class Position
    {
        /* store row and column */
        public int Row { get; set; }
        public int Column { get; set; }
        /*  constructor */ 
        public Position(int row, int column)
        {
            Row= row;
            Column= column;
        }
    }
}
