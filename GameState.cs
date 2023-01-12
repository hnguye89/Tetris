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
    }
}
