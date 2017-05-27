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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for SinglePlayerProperties.xaml
    /// here user fills his desired properties of the single player
    /// </summary>
    public partial class SinglePlayerProperties : Window
    {
        /// <summary>
        /// fields
        /// </summary>
        private bool exitWithXButton = true;

        /// <summary>
        /// constructor
        /// </summary>
        public SinglePlayerProperties()
        {
            InitializeComponent();
        }

        /// <summary>
        /// start game button was pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Game(object sender, RoutedEventArgs e)
        {
            //user filled all textBoxes
            if (!String.IsNullOrEmpty(myMaze.Maze.Text) && !String.IsNullOrEmpty(myMaze.Rows.Text) && !String.IsNullOrEmpty(myMaze.Cols.Text))
            {
                singlePlayer singlePlayer = new singlePlayer(myMaze.Maze.Text,int.Parse(myMaze.Rows.Text), int.Parse(myMaze.Cols.Text));
                if (singlePlayer.SPMazeOK) {
                    singlePlayer.Title = "Single Player";
                    singlePlayer.Show();
                    exitWithXButton = false;
                    this.Close();
                }
            }
            //user did not fill all textboxes
            else
            {
                MessageBox.Show("Please fill all textBoxes", "Error occured", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// user exited with x button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (exitWithXButton)
            {
                MainWindow win = (MainWindow)Application.Current.MainWindow;
                win.Show();
            }
        }
    }
}
