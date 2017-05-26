using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MultiplayerSettings.xaml
    /// </summary>
    public partial class MultiplayerSettings : Window
    {
        private MultiplayerSettingsViewModel vm;
        private bool isClosedWithXButton = true;

        public MultiplayerSettings()
        {
            InitializeComponent();
            vm = new MultiplayerSettingsViewModel();
            this.DataContext = vm;

            chooseMaze.Maze.SetBinding(TextBox.TextProperty, new Binding()
            {
                //Source = vm,
                Path = new PropertyPath("Name")
            });

            chooseMaze.Rows.SetBinding(TextBox.TextProperty, new Binding()
            {
                //Source = vm,
                Path = new PropertyPath("Rows")
            });

            chooseMaze.Cols.SetBinding(TextBox.TextProperty, new Binding()
            {
                //Source = vm,
                Path = new PropertyPath("Cols")
            });
        }

        private void joinBtn_Click(object sender, RoutedEventArgs e)
        {
            joinLbl.Content = "Joining Game, please wait...";
            joinLbl.Visibility = Visibility.Visible;

            Maze m = vm.JoinGame(gamesCBox.SelectedIndex, out TcpClient serverSocket);
            if (m == null)
            {
                joinLbl.Content = "Error joining game. Please try another game.";
                return;
            }

            new Multiplayer(m, serverSocket).Show();
            isClosedWithXButton = false;
            this.Close();
        }

        private void startbtn_Click(object sender, RoutedEventArgs e)
        {
            
            startLbl.Content = "Waiting for other player...";
            startLbl.Visibility = Visibility.Visible;

            if (chooseMaze.Maze.Text == "" || chooseMaze.Rows.Text == "" || chooseMaze.Cols.Text == "")
            {
                startLbl.Content = "Please enter all fields.";
                return;
            }
            if (!int.TryParse(chooseMaze.Rows.Text, out int rows) || !int.TryParse(chooseMaze.Cols.Text, out int cols) || rows <= 0 || cols <= 0)
            {
                startLbl.Content = "Wrong rows / columns number.";
                return;
            }

            Maze m = vm.StartGame(out TcpClient serverSocket);
            if (m == null)
            {
                startLbl.Content = "Error opening game. Please enter different parameters (probably a naming problem)";
                return;
            }

            new Multiplayer(m, serverSocket).Show();
            isClosedWithXButton = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClosedWithXButton)
                Application.Current.MainWindow.Show();
        }
    }
}