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
            currentBlock = BlockQuece.GetandUpdate();
            canHold = true;
        }
        /* This method checks if the current block is in a legal position or not */
        private bool BlockFits()
        {
            /* the method loops over the tile positions of the current block
             * and if any of them are outside tje grid or overlapping another tile 
             * then it return false otherwise if we get through the entire loop, we return true */
            foreach (Position p in currentBlock.TilePositions())
            {
                if(!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public void HoldBlock()
        {
            if (!canHold)
            {
                return;
            }
            if(heldBlock == null)
            {
                heldBlock = currentBlock;
                currentBlock = BlockQuece.GetandUpdate();
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
        public void RotateBlockCW()
        {
            currentBlock.RotateCW();
            /* if it ends up in illegal position, then we rotate it back */
            if (!BlockFits())
            {
                currentBlock.RotateCCw();
            }
        }
        /* A method to rotate the current block counter clockwise but only it's possible to do so from where it is */
        public void RotateBlockCCW()
        {
            currentBlock.RotateCCw();
            /* if it ends up in illegal position, then we rotate it back */
            if (!BlockFits())
            {
                currentBlock.RotateCW();
            }
        }
        /* A method to move a current block from the left */ 
        public void MoveBlockLeft()
        {
            currentBlock.Move(0, -1);

            if (!BlockFits())
            {
                currentBlock.Move(0, 1);
            }
        }
        /* A method to move a current block from the right */
        public void MoveBlockRight()
        {
            currentBlock.Move(0, 1);

            if (!BlockFits())
            {
                currentBlock.Move(0, -1);
            }
        }
        /* if the game is over */
        /* if  either of the hidden rows at the top are not empty then the game is lost */
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }
        /* A method will be called when the current block cannot be moved down */
        private void PlaceBlock()
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
            if (IsGameOver())
            {
                GameOver = true;
            }
            else /* if not, we update the current block */
            {
                currentBlock = BlockQuece.GetandUpdate();
                canHold = true;
            }
        }
        /* A method that moves down the current block */ 
        public void MoveBlockDown()
        {
            currentBlock.Move(1, 0);

            if (!BlockFits())
            {
                currentBlock.Move(-1, 0); 
                PlaceBlock(); /* we call the placeBlock method in case the block cannot be move down */
            }
        }

        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while(GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach(Position p in currentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock()
        {  
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

    }
}
