using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Array full of tile images
        /// </summary>
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/TileRed.png", UriKind.Relative))
        };
        /// <summary>
        /// Array full of tile block images
        /// </summary>
        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;
        /*A method for GameState*/
        private GameState gameState = new GameState();
        /* A contstructor initialized the imageControls array 
         * by calling SetupGameCanvas method */
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid); 
        }
        
        /* A method to set up the image controls correctly in the canvas */ 
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            /* imageControls array will have  22 rows and 10 columns */
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25; /* a veriable for the width and height of each cell */
            /* loop through every row and column in the game grid */ 
            for(int r = 0; r < grid.Rows; r++)
            {
                for(int c = 0; c < grid.Columns; c++)
                {
                    /* create new image control with 25 pixels width and height for each position */
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };
                    /* set the distance from the top of the canvas 
                     * to the top of the image equal to r - 2 cell size */
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, cellSize * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls; 
        }
        /* Looping through all positions */
        private void drawGrid(GameGrid grid)
        {
            for(int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r,c];
                    imageControls[r,c].Opacity= 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }
        /* Looping through the tile positions and update the image sources */
        private void drawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row,p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void drawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void drawHeldBlock(Block heldBlock)
        {
            if(heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void drawGhostBlock(Block block)
        {
            int dropDistance = gameState.blockDropDistance();

            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }
        /* This method draws both the grid and the current block */
        private void Draw(GameState gameState)
        {
            drawGrid(gameState.GameGrid);
            drawGhostBlock(gameState.CurrentBlock);
            drawBlock(gameState.CurrentBlock);
            drawNextBlock(gameState.BlockQuece);
            drawHeldBlock(gameState.heldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private async Task gameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);
                gameState.moveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }
        /*Key binds: detecting some key presses */
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.moveBlockLeft();
                    break;
                case Key.Right:
                    gameState.moveBlockRight();
                    break;
                case Key.Down:
                    gameState.moveBlockDown();
                    break;
                case Key.Up:
                    gameState.rotateBlockCW();
                    break;
                case Key.Z:
                    gameState.rotateBlockCCW();
                    break;
                case Key.C:
                    gameState.holdBlock();
                    break;
                case Key.Space:
                    gameState.dropBlock();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }
        /* Call the Draw method when the game canvas has loaded */
        private async void gameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await gameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await gameLoop();
        }
    }
}
