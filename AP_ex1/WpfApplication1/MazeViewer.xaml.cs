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

//aasasasasasas
namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for MazeViewer.xaml
    /// </summary>
    public partial class MazeViewer : UserControl
    {
        private double[] tileSizes = new double[2]; //{width, height}
        private Image[,] tiles = null;

        #region Rows
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set
            {
                SetValue(RowsProperty, value);

                if (Cols != 0)
                    SetTileSize();
            }
        }
        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(MazeViewer), new PropertyMetadata(0));
        #endregion
        #region Cols
        public int Cols
        {
            get { return (int)GetValue(ColsProperty); }
            set
            {
                SetValue(ColsProperty, value);
                if (Rows != 0)
                    SetTileSize();
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
                int x = int.Parse(split[0]), y = int.Parse(split[1]);
                Canvas.SetLeft(playerImg, x * tileSizes[0]);
                Canvas.SetTop(playerImg, y * tileSizes[1]);
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
                playerImg.Source = new BitmapImage(new Uri("/resources/player.png", UriKind.Relative));
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
                InitialDraw();
            }
        }

        // Using a DependencyProperty as the backing store for Maze.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExitImageFileProperty =
            DependencyProperty.Register("ExitImageFile", typeof(string), typeof(MazeViewer), new PropertyMetadata(""));
        #endregion
        #region InitialPos
        public string InitialPos
        {
            get { return (string)GetValue(InitialPosPropertyProperty); }
            set
            {
                SetValue(InitialPosPropertyProperty, value);
                SetValue(PlayerPosProperty, value);
                string[] arr = value.Split(',');
                int x = int.Parse(arr[0]), y = int.Parse(arr[1]);
                Canvas.SetLeft(playerImg, tileSizes[0] * x);
                Canvas.SetTop(playerImg, tileSizes[1] * y);
            }
        }

        // Using a DependencyProperty as the backing store for InitialPosProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InitialPosPropertyProperty =
            DependencyProperty.Register("InitialPosProperty", typeof(string), typeof(MazeViewer), new PropertyMetadata(""));
        #endregion

        public MazeViewer()
        {
            InitializeComponent();
            InitialDraw();
        }

        private void SetTileSize()
        {
            double tileNum = Rows * Cols;
            tileSizes[0] = Width / tileNum;
            tileSizes[1] = Height / tileNum;

            playerImg.Width = tileSizes[0];
            playerImg.Height = tileSizes[1];
        }

        private void InitialDraw()
        {
            if (Rows == 0 || Cols == 0 || Maze == "" || Maze.Length != Rows*Cols*2 - 1)
                return;
            if (tiles == null)
            {
                tiles = new Image[Rows, Cols];
                string[] split = Maze.Split(',');
                BitmapImage bmi = new BitmapImage(new Uri("/resources/wall.png", UriKind.Relative));
                for (int r = 0, c = 0; r < Rows; )
                {
                    if (split[r * Cols + c] == "1")
                    {
                        tiles[r, c] = new Image()
                        {
                            Height = tileSizes[1],
                            Width = tileSizes[0],
                            Source = bmi,
                            Stretch = Stretch.Fill
                        };
                        myCanvas.Children.Add(tiles[r, c]);
                        Canvas.SetLeft(tiles[r, c], tileSizes[0] * c);
                        Canvas.SetTop(tiles[r, c], tileSizes[1] * r);
                    }
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
                    tiles[x, y] = new Image()
                    {
                        Height = tileSizes[1],
                        Width = tileSizes[0],
                        Stretch = Stretch.Fill
                    };
                    myCanvas.Children.Add(tiles[x, y]);
                    Canvas.SetLeft(tiles[x, y], tileSizes[0] * x);
                    Canvas.SetTop(tiles[x, y], tileSizes[1] * y);
                }
                tiles[x, y].Source = new BitmapImage(new Uri("/resources/exit.png", UriKind.Relative));
            }
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
