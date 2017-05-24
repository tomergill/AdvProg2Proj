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
        private double[] tileSizes = new double[2]; //{width, height}
        private /*Image*/Rectangle[,] tiles = null;

        #region Rows


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
        public string PlayerPos
        {
            private get { return (string)GetValue(PlayerPosProperty); }
            set
            {
                SetValue(PlayerPosProperty, value);
                string[] split = value.Split(',');
                int x, y;
                if (int.TryParse(split[0], out x) && int.TryParse(split[1], out y))
                    DrawPlayer(x, y);
            }
        }

        // Using a DependencyProperty as the backing store for Maze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerPosProperty =
            DependencyProperty.Register("PlayerPos", typeof(string), typeof(MazeViewer), new PropertyMetadata(""));
        #endregion

        #region GoalPos
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

        public MazeViewer()
        {
            InitializeComponent();
            ImageBrush b = new ImageBrush(new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/../../../resources/background.jpg", UriKind.Absolute)));
            b.Stretch = Stretch.None;
            myCanvas.Background = b;
        }

        private void SetTileSize()
        {
            //double tileNum = Rows * Cols;
            tileSizes[0] = myCanvas.ActualWidth / Cols;//tileNum;
            tileSizes[1] = myCanvas.ActualHeight / Rows;//tileNum;

            playerImg.Width = tileSizes[0];
            playerImg.Height = tileSizes[1];
        }

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

                    tiles[r, c].Height = tileSizes[0];
                    tiles[r, c].Width = tileSizes[1];

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

        private void DrawPlayer(int x, int y)
        {
            Canvas.SetLeft(playerImg, y * tileSizes[0]);
            Canvas.SetTop(playerImg, x * tileSizes[1]);
        }

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

        //private void DrawMove()
        //{
        //    double oldX = Canvas.GetLeft(playerImg), oldY = Canvas.GetTop(playerImg);
        //    String[] split = PlayerPos.Split(',');
        //    double x = double.Parse(split[0]) * tileSizes[0], 
        //        y = double.Parse(split[1]) * tileSizes[1];
        //    if (x > oldX)
        //    {
        //        for (double i = 0; i < x - oldX; i++)
        //        {
        //            Canvas.SetLeft(playerImg, oldX + i);
        //            sleep(100);
        //        }
        //    }
        //}
    }
}
