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
        /// <summary>
        /// fields
        /// </summary>
        
        // having a view model
        private singlePlayerViewModel SPVM;
        // boolean that checks if player is supposed to be able to move or not
        private Boolean canMove;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mazeName"> getting name desired for the maze </param>
        /// <param name="rowsNum"> getting number of rows </param>
        /// <param name="colsNum"> getting number of colomns </param>
        public singlePlayer(string mazeName, int rowsNum, int colsNum)
        {
            InitializeComponent();
            
            //initializing the view model
            SPVM = new singlePlayerViewModel(mazeName, rowsNum, colsNum);
            if (SPVM.VMMazeOk)
            {
                this.canMove = true;
                this.DataContext = SPVM;
            }
            //error with maze occured
            else
            {
                MessageBox.Show("Must choose a different name for the maze", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// getter, gets the boolean stating if maze OK or not
        /// </summary>
        public Boolean SPMazeOK
        {
            get { return SPVM.VMMazeOk; }
        }

        /// <summary>
        /// when x is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //stopping the solution thread
            SPVM.GetKeepSolving = false;
            //returning to main window
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            win.Show();
        }

        /// <summary>
        /// keys were pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //checking if user is allowed to move
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
                //checking if goal point was reached
                if (SPVM.VMgetEndPointReached)
                {
                    MessageBox.Show("Great Job! you found the way!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.canMove = false;
                }
            }
        }

        /// <summary>
        /// restart button was pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restart(object sender, RoutedEventArgs e)
        {
            // stopping soliution thread
            SPVM.GetKeepSolving = false;
            //allowing user to move
            this.canMove = true;
            //restarting in view model
            SPVM.Restart();
            //checking if initial position is goal position
            if (SPVM.VMgetEndPointReached)
            {
                MessageBox.Show("Great Job! you found the way!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.canMove = false;
            }
        }

        /// <summary>
        /// solve maze was pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetSolution(object sender, RoutedEventArgs e)
        {
            // starting soliution thread
            SPVM.GetKeepSolving = true;
            //preventing from user to move
            this.canMove = false;
            //returning to initial point
            SPVM.Restart();
            //solving maze
            SPVM.SolveMe();
        }

        /// <summary>
        /// main menu button was pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu(object sender, RoutedEventArgs e)
        {
            // stopping soliution thread
            SPVM.GetKeepSolving = false;
            //returning to main menu
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            win.Show();
            this.Close();
        }

        /// <summary>
        /// when window finishes loading, checking if initial point is goal point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
