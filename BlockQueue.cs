using System;

namespace Tetris
{
    public class BlockQueue
    {
        /* This will contain a block array with an instance of the 7 block classes */
        private readonly Block[] blocks = new Block[]
        {
            new iBlock(),
            new jBlock(),
            new LBlock(),
            new oBlock(),
            new sBlock(),
            new tBlock(),
            new zBlock(),
        };
        /* This is a random object */
        private readonly Random random = new Random();
        /* A property for the next block in the queue */
        public Block NextBlock { get; private set; }
        /* This contructor initiialize the next block with a randomblock */
        public BlockQueue()
        {
            NextBlock = RandomBlock(); 
        }
        /* This method returns a random block */
        public Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }
        /* This method returns the next block and updates the property */
        public Block GetandUpdate()
        {
            /* Since we don't want to return the same block twice in a row, 
             * We keep picking until we get a new one */
            
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            } while (block.Id == NextBlock.Id);

            return block;
        }
      
    }
}
