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
    /// Interaction logic for ChooseMaze.xaml
    /// </summary>
    public partial class ChooseMaze : UserControl
    {
        public ChooseMaze()
        {
            InitializeComponent();
        }

        private void Start_Game(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Maze.Text) && !String.IsNullOrEmpty(Rows.Text) && !String.IsNullOrEmpty(Cols.Text))
            {
                
                singlePlayer singlePlayer = new singlePlayer();
                singlePlayer.Title = "Single Player";
                singlePlayer.Show();
            }
            else
            {
                Maze.Text = string.Empty;
                Rows.Text = string.Empty;
                Cols.Text = string.Empty;
                ErrorMsgBox error = new ErrorMsgBox();
                error.ShowDialog();
            }
        }
    }
}
