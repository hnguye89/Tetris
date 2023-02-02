using System.Collections;
using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        /* A 2D position array which containts the tile positions in the four rotation states */
        protected abstract Position[][] Tiles { get; }
        /* A start offset which decides where the block spawns in the grid */
        protected abstract Position StartOffset { get; }
        /* An integer ID which distinguish the blocks */
        public abstract int Id { get; }
        /* store the current rotation state and the curreny offset */
        private int rotationState;
        private Position offset; 
        /* contsructor */
        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }
        /* A method which returns the grid positions occupied by the block factoring in the current rotation 
         * and offset */
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }
        /* A method that rotates the block 90 degrees clockwise */
        /* By doing that is by incrementing the current rotation state wrapping around to zero if it's  in the final state */
        public void RotateCW()
        {
            rotationState = (rotationState+ 1) % Tiles.Length;
        }
        /* A method to rotate counter-clockwise */ 
        public void RotateCCw()
        {
            if(rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }
        /* A move method which moves the block by a given number of rows and columns */
        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }
        /* A reset method which resets the rotation and position */
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
