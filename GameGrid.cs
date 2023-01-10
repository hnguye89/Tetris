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
        /* This class save the number of rows and columns and initialize the array */
        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }
        /* This method will check if a given row and column is inside the grid or not */
        public bool isInside(int r, int c)
        {
            /* To be inside the grid, the row must be greater than or equal to 0  
             * and less than the number of rows. Similarily for the column, 
             * it must be less than the number of columns.
             */
            return r >= 0 && r < Rows && c >= 0 && c < Columns; 
        }
        /* This method checks if a given cell is empty or not */
        public bool isEmpty(int r, int c)
        {
            /* It must be inside the grid and the value at that entry in the array must be
             * be zero.
             */
            return isInside(r, c) && grid[r, c] == 0;
        }
        /* This method checks if an entire row is full */ 
        public bool isRowFull(int r)
        {
            for(int c =0; c < Columns; c++)
            {
                if (grid[r,c] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        /* This method checks if a row is empty */
        public bool isRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
