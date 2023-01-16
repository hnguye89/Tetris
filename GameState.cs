namespace Tetris
{
    
    public class GameState
    {
        /* Add a property with a backing field for the current block  */
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            /* When we update the current block the reset method is called to set
             * the correct start position and rotation */
            private set
            {
                currentBlock= value;
                currentBlock.Reset();

                for(int i =0; i<2; i++)
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }

        }
        /* Adding more properties */
        public GameGrid GameGrid { get;}
        public BlockQueue BlockQuece { get;}
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block heldBlock { get; private set; }
        public bool canHold { get; private set; }
        /* The constructor initilize the GameGrid, BlockQuece, currentBlock, and canHold.  */
        public GameState()
        {
            GameGrid = new GameGrid(22, 10); /* GameGrid with 22 rows and 10 columns */
            BlockQuece = new BlockQueue(); /* This get a random block fpr the current block property */
            currentBlock = BlockQuece.getandUpdate();
            canHold = true;
        }
        /* This method checks if the current block is in a legal position or not */
        private bool blockFits()
        {
            /* the method loops over the tile positions of the current block
             * and if any of them are outside tje grid or overlapping another tile 
             * then it return false otherwise if we get through the entire loop, we return true */
            foreach (Position p in currentBlock.TilePositions())
            {
                if(!GameGrid.isEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public void holdBlock()
        {
            if (!canHold)
            {
                return;
            }
            if(heldBlock == null)
            {
                heldBlock = currentBlock;
                currentBlock = BlockQuece.getandUpdate();
            }
            else
            {
                Block tmp = currentBlock;
                currentBlock = heldBlock;
                heldBlock = tmp;
            }

            canHold = false; 
        }
        /* A method to rotate the current block clockwise but only it's possible to do so from where it is */
        public void rotateBlockCW()
        {
            currentBlock.rotateCW();
            /* if it ends up in illegal position, then we rotate it back */
            if (!blockFits())
            {
                currentBlock.rotateCCw();
            }
        }
        /* A method to rotate the current block counter clockwise but only it's possible to do so from where it is */
        public void rotateBlockCCW()
        {
            currentBlock.rotateCCw();
            /* if it ends up in illegal position, then we rotate it back */
            if (!blockFits())
            {
                currentBlock.rotateCW();
            }
        }
        /* A method to move a current block from the left */ 
        public void moveBlockLeft()
        {
            currentBlock.Move(0, -1);

            if (!blockFits())
            {
                currentBlock.Move(0, 1);
            }
        }
        /* A method to move a current block from the right */
        public void moveBlockRight()
        {
            currentBlock.Move(0, 1);

            if (!blockFits())
            {
                currentBlock.Move(0, -1);
            }
        }
        /* if the game is over */
        /* if  either of the hidden rows at the top are not empty then the game is lost */
        private bool isGameOver()
        {
            return !(GameGrid.isRowEmpty(0) && GameGrid.isRowEmpty(1));
        }
        /* A method will be called when the current block cannot be moved down */
        private void placeBlock()
        {
            /* it loops over the tile positions of the current block and sets those positions in the game grid 
             * equal to the block's id */ 
            foreach(Position p in currentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = currentBlock.Id;
            }

            /* then we clear any potentialy full rows */ 
            Score += GameGrid.ClearFullRows();
            /* and check if the game is over */
            if (isGameOver())
            {
                GameOver = true;
            }
            else /* if not, we update the current block */
            {
                currentBlock = BlockQuece.getandUpdate();
                canHold = true;
            }
        }
        /* A method that moves down the current block */ 
        public void moveBlockDown()
        {
            currentBlock.Move(1, 0);

            if (!blockFits())
            {
                currentBlock.Move(-1, 0); 
                placeBlock(); /* we call the placeBlock method in case the block cannot be move down */
            }
        }

        private int tileDropDistance(Position p)
        {
            int drop = 0;

            while(GameGrid.isEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int blockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach(Position o in currentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, tileDropDistance(p));
            }

            return drop;
        }

        public void dropBlock()
        {  
            CurrentBlock.Move(blockDropDistance(), 0);
            placeBlock();
        }

    }
}
