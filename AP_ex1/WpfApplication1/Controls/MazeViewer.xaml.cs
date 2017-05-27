using System;
using System.Collections.Generic;
using System.Linq;
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


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MazeViewer.xaml
    /// </summary>
    public partial class MazeViewer : UserControl
    {
        /// <summary>
        /// The tile sizes
        /// </summary>
        private double[] tileSizes = new double[2]; //{width, height}

        /// <summary>
        /// The tiles
        /// </summary>
        private Rectangle[,] tiles = null;

        #region Rows


        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(MazeViewer), new PropertyMetadata(0));


        #endregion

        #region Cols
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public int Cols
        {
            get { return (int)GetValue(ColsProperty); }
            set
            {
                SetValue(ColsProperty, value);
            }
        }
        public static readonly DependencyProperty ColsProperty = DependencyProperty.Register("Cols", typeof(int), typeof(MazeViewer), new PropertyMetadata(0));
        #endregion

        #region Maze
        /// <summary>
        /// Gets or sets the maze string representation.
        /// </summary>
        /// <value>
        /// The maze string representation.
        /// </value>
        public string Maze
        {
            get { return (string)GetValue(MazeProperty); }
            set { SetValue(MazeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MazeProperty =
            DependencyProperty.Register("Maze", typeof(string), typeof(MazeViewer), new PropertyMetadata(""));
        #endregion

        #region PlayerPos
        /// <summary>
        /// Gets or sets the player position.
        /// </summary>
        /// <value>
        /// The player position.
        /// </value>
        public string PlayerPos
        {
            private get { return (string)GetValue(PlayerPosProperty); }
            set
            {
                SetValue(PlayerPosProperty, value);
                string[] split = value.Split(',');
                if (int.TryParse(split[0], out int x) && int.TryParse(split[1], out int y))
                    DrawPlayer(x, y);
            }
        }

        // Using a DependencyProperty as the backing store for Maze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerPosProperty =
            DependencyProperty.Register("PlayerPos", typeof(string), typeof(MazeViewer), new PropertyMetadata("", OnPlayerPosPropertyChanged));
        #endregion

        #region GoalPos
        /// <summary>
        /// Gets or sets the goal position.
        /// </summary>
        /// <value>
        /// The goal position.
        /// </value>
        public string GoalPos
        {
            get { return (string)GetValue(GoalPosProperty); }
            set { SetValue(GoalPosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoalPosProperty =
            DependencyProperty.Register("GoalPos", typeof(string), typeof(MazeViewer), new PropertyMetadata(""));
        #endregion

        #region PlayerImageFile
        /// <summary>
        /// Gets or sets the player image file.
        /// </summary>
        /// <value>
        /// The player image file.
        /// </value>
        public string PlayerImageFile
        {
            get { return (string)GetValue(PlayerImageFileProperty); }
            set
            {
                SetValue(PlayerImageFileProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Maze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerImageFileProperty =
            DependencyProperty.Register("PlayerImageFile", typeof(string), typeof(MazeViewer), new PropertyMetadata(""));
        #endregion

        #region ExitImageFile
        /// <summary>
        /// Gets or sets the exit image file.
        /// </summary>
        /// <value>
        /// The exit image file.
        /// </value>
        public string ExitImageFile
        {
            get { return (string)GetValue(ExitImageFileProperty); }
            set
            {
                SetValue(ExitImageFileProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Maze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExitImageFileProperty =
            DependencyProperty.Register("ExitImageFile", typeof(string), typeof(MazeViewer), new PropertyMetadata(""));
        #endregion

        #region InitialPos
        /// <summary>
        /// Gets or sets the initial position.
        /// </summary>
        /// <value>
        /// The initial position.
        /// </value>
        public string InitialPos
        {
            get { return (string)GetValue(InitialPosProperty); }
            set
            {
                SetValue(InitialPosProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for InitialPosProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InitialPosProperty =
            DependencyProperty.Register("InitialPos", typeof(string), typeof(UserControl), new PropertyMetadata(""));
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeViewer"/> class.
        /// </summary>
        public MazeViewer()
        {
            InitializeComponent();
            ImageBrush b = new ImageBrush(new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/../../../resources/background.jpg", UriKind.Absolute)))
            {
                Stretch = Stretch.None
            };
            myCanvas.Background = b;
        }

        /// <summary>
        /// Sets the size of the tile.
        /// </summary>
        private void SetTileSize()
        {
            //double tileNum = Rows * Cols;
            tileSizes[0] = myCanvas.ActualWidth / Cols;//tileNum;
            tileSizes[1] = myCanvas.ActualHeight / Rows;//tileNum;

            playerImg.Width = tileSizes[0];
            playerImg.Height = tileSizes[1];
        }

        /// <summary>
        /// Draws the maze, including all the tiles and the exit tile.
        /// </summary>
        private void InitialDraw()
        {
            if (Rows == 0 || Cols == 0 || Maze == "" || Maze.Length != Rows * Cols * 2 - 1)
                return;
            if (tiles == null)
            {
                tiles = new /*Image*/Rectangle[Rows, Cols];
                string[] split = Maze.Split(',');
                //BitmapImage bmi = new BitmapImage(new Uri("resources" + "\\" + "wall.png", UriKind.RelativeOrAbsolute));
                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/../../../resources/wall.png", UriKind.Absolute)));
                ImageBrush bcg = new ImageBrush(new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/../../../resources/background.jpg", UriKind.Absolute)));
                for (int r = 0, c = 0; r < Rows;)
                {
                    tiles[r, c] = new Rectangle()//Image()
                    {
                        Height = tileSizes[1],
                        Width = tileSizes[0],
                        //Source = bmi,
                        Stretch = Stretch.Fill
                    };
                    Canvas.SetLeft(tiles[r, c], tileSizes[0] * c);
                    Canvas.SetTop(tiles[r, c], tileSizes[1] * r);
                    myCanvas.Children.Add(tiles[r, c]);
                    if (split[r * Cols + c] == "1")
                        tiles[r, c].Fill = brush;
                    else
                        tiles[r, c].Fill = bcg;

                    //tiles[r, c].Height = tileSizes[0];
                    //tiles[r, c].Width = tileSizes[1];

                    if ((c = ++c % Cols) == 0)
                        r++;
                }
            }
            if (GoalPos != "")
            {
                string[] arr = GoalPos.Split(',');
                int x = int.Parse(arr[0]), y = int.Parse(arr[1]);
                if (tiles[x, y] == null)
                {
                    tiles[x, y] = new Rectangle()//Image()
                    {
                        Height = tileSizes[1],
                        Width = tileSizes[0],
                        Stretch = Stretch.Fill
                    };
                    myCanvas.Children.Add(tiles[x, y]);
                    Canvas.SetLeft(tiles[x, y], tileSizes[0] * y);
                    Canvas.SetTop(tiles[x, y], tileSizes[1] * x);
                }
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/" + ExitImageFile, UriKind.Absolute);
                img.EndInit();
                tiles[x, y].Fill = new ImageBrush(img);
                //tiles[x, y].Source = new BitmapImage(new Uri(ExitImageFile, UriKind.RelativeOrAbsolute));
            }
        }

        /// <summary>
        /// Draws the player.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void DrawPlayer(int x, int y)
        {
            Canvas.SetLeft(playerImg, y * tileSizes[0]);
            Canvas.SetTop(playerImg, x * tileSizes[1]);
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetTileSize();
            //playerImg.Source = new BitmapImage(new Uri("/resources/player.png", UriKind.Relative));
            InitialDraw();
            PlayerPos = InitialPos;
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/" + PlayerImageFile, UriKind.Absolute);
            img.EndInit();
            playerImg.Source = img;
            Canvas.SetZIndex(playerImg, 100); //bigger than anything else
        }

        /// <summary>
        /// Called when [player position property changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPlayerPosPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeViewer mv = (MazeViewer) d;
            mv.OnPlayerPosPropertyChanged();
        }

        /// <summary>
        /// Called when [player position property changed].
        /// </summary>
        private void OnPlayerPosPropertyChanged()
        {
            string[] split = PlayerPos.Split(',');
            if (int.TryParse(split[0], out int x) && int.TryParse(split[1], out int y))
                DrawPlayer(x, y);
        }
    }
}
