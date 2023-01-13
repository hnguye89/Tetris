namespace Tetris
{
    /*  */
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
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

        public GameGrid GameGrid { get;}
        public BlockQueue BlockQuece { get;}
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block heldBlock { get; private set; }
        public bool canHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQuece = new BlockQueue();
            currentBlock = BlockQuece.getandUpdate();
            canHold = true;
        }

        private bool blockFits()
        {
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

        public void rotateBlockCW()
        {
            currentBlock.rotateCW();

            if (!blockFits())
            {
                currentBlock.rotateCCw();
            }
        }

        public void rotateBlockCCW()
        {
            currentBlock.rotateCCw();

            if (!blockFits())
            {
                currentBlock.rotateCW();
            }
        }

        public void moveBlockLeft()
        {
            currentBlock.Move(0, -1);

            if (!blockFits())
            {
                currentBlock.Move(0, 1);
            }
        }

        public void moveBlockRight()
        {
            currentBlock.Move(0, 1);

            if (!blockFits())
            {
                currentBlock.Move(0, -1);
            }
        }

        private bool isGameOver()
        {
            return !(GameGrid.isRowEmpty(0) && GameGrid.isRowEmpty(1));
        }

        private void placeBlock()
        {
            foreach(Position p in currentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = currentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if (isGameOver())
            {
                GameOver = true;
            }
            else
            {
                currentBlock = BlockQuece.getandUpdate();
                canHold = true;
            }
        }

        public void moveBlockDown()
        {
            currentBlock.Move(1, 0);

            if (!blockFits())
            {
                currentBlock.Move(-1, 0);
                placeBlock();
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
