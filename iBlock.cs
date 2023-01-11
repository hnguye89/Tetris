namespace Tetris
{
    /* iBlock inherits Block */
    public class iBlock : Block
    {
        /* This store the tile positions for the four rotation states */
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] {new(1,0), new(1,1), new(1,2), new(1,3)},
            new Position[] {new(0,2), new(1,2), new(2,2), new(3,2)},
            new Position[] {new(2,0), new(2,1), new(2,2), new(2,3)},
            new Position[] {new(0,1), new(1,1), new(2,1), new(3,1)}
        };
        /* Filled out the required properties */
        public override int Id => 1;
        /* The start offset should be -1. */
        /* This will make the block spawn in the middle of the top row */
        protected override Position StartOffset => new Position(-1,3);
        /* for the tiles property, this return the tiles array above */
        protected override Position[][] Tiles => tiles;

    }
}
