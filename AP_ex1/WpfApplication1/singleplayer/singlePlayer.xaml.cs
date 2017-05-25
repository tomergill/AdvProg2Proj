using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for singlePlayer.xaml
    /// </summary>
    public partial class singlePlayer : Window
    {
        private singlePlayerViewModel SPVM;
        private Boolean canMove;

        public singlePlayer(string mazeName, int rowsNum, int colsNum)
        {
            InitializeComponent();
            SPVM = new singlePlayerViewModel(mazeName, rowsNum, colsNum);
            if (SPVM.VMMazeOk)
            {
                this.canMove = true;
                this.DataContext = SPVM;
            }
            else
            {
                MessageBox.Show("Must choose a different name for the maze", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Boolean SPMazeOK
        {
            get { return SPVM.VMMazeOk; }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SPVM.GetKeepSolving = false;
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            win.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (canMove)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        SPVM.GoLeft();
                        break;
                    case Key.Right:
                        SPVM.GoRight();
                        break;
                    case Key.Up:
                        SPVM.GoUp();
                        break;
                    case Key.Down:
                        SPVM.GoDown();
                        break;
                    default:
                        break;
                }
                if (SPVM.VMgetEndPointReached)
                {
                    MessageBox.Show("Great Job! you found the way!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.canMove = false;
                }
            }
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            this.canMove = true;
            SPVM.Restart();
            if (SPVM.VMgetEndPointReached)
            {
                MessageBox.Show("Great Job! you found the way!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.canMove = false;
            }
        }

        private void GetSolution(object sender, RoutedEventArgs e)
        {
            this.canMove = false;
            SPVM.Restart();
            SPVM.SolveMe();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            SPVM.GetKeepSolving = false;
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            win.Show();
            this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (SPVM.VMgetEndPointReached)
            {
                MessageBox.Show("Great Job! you found the way!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.canMove = false;
            }
        }
    }
}
