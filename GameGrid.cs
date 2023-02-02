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
        /* This method save the number of rows and columns and initialize the array */
        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }
        /* This method will check if a given row and column is inside the grid or not */
        public bool IsInside(int r, int c)
        {
            /* To be inside the grid, the row must be greater than or equal to 0  
             * and less than the number of rows. Similarily for the column, 
             * it must be less than the number of columns.
             */
            return r >= 0 && r < Rows && c >= 0 && c < Columns; 
        }
        /* This method checks if a given cell is empty or not */
        public bool IsEmpty(int r, int c)
        {
            /* It must be inside the grid and the value at that entry in the array must be
             * be zero.
             */
            return IsInside(r, c) && grid[r, c] == 0;
        }
        /* This method checks if an entire row is full */ 
        public bool IsRowFull(int r)
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
        /* When there are full rows they need to be cleared
         * and the rows above should be move down
         */
        public bool IsRowEmpty(int r)
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
        /* This method clears a row */
        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }
        /* This method that moves a row down by a certain number of rows  */
        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }
        /* This method can clear rows */
        public int ClearFullRows()
        {
            int cleared = 0; /* Cleared variable starts at 0 and then, move from the bottom row towards the top. */
            
            /* Check if the current row is full and if it is we clear it 
             * and increment cleared */
            for(int r = Rows-1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                /* if cleared is equal to zero, then we move the current row down 
                 * by the number of cleared rows  */
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared; /* return the number of cleared rows */
        }
    }
}
