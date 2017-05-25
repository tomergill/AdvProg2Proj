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
    /// </summary>
    public partial class SinglePlayerProperties : Window
    {
        private bool exitWithXButton = true;

        public SinglePlayerProperties()
        {
            InitializeComponent();
        }

        private void Start_Game(object sender, RoutedEventArgs e)
        {
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
            else
            {
                MessageBox.Show("Please fill all textBoxes", "Error occured", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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
