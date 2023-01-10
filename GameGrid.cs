namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid; 
        public int Rows { get; }
        public int Columns { get; }
        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }
        /* This class save the number of rows and columns and initialize the array*/
        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }
        /* this will check if a given row and column is inside the grid or not*/
        public bool isInside(int r, int c)
        {
            /* To be inside the grid, the row must be greater than or equal to 0  
               and less than the number of rows. Similarily for the column, it must be less than
               the number of columns.
             */
            return r >= 0 && r < Rows && c >= 0 && c < Columns; 
        }
    }
}
