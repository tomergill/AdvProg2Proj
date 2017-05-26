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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Multiplayer.xaml
    /// </summary>
    public partial class Multiplayer : Window
    {
        private MultiplayerViewModel vm;

        public Multiplayer(Maze maze, TcpClient serverSocket)
        {
            vm = new MultiplayerViewModel(maze, serverSocket, LostGame);
            this.DataContext = vm;
            InitializeComponent();
        }

        public void LostGame()
        {
            if (!vm.VMStop)
                vm.CloseGame();
            MessageBox.Show("You have lost :(", "LOST", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!vm.VMStop)
            {
                Direction dir;
                switch (e.Key)
                {
                    case Key.Left:
                        dir = Direction.Left;
                        break;
                    case Key.Right:
                        dir = Direction.Right;
                        break;
                    case Key.Up:
                        dir = Direction.Up;
                        break;
                    case Key.Down:
                        dir = Direction.Down;
                        break;
                    default:
                        dir = Direction.Unknown;
                        break;
                }
                if (vm.MakeAMove(dir)) //player won
                {
                    vm.CloseGame();
                    MessageBox.Show("You have won!!", "WINNING", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!vm.VMStop)
                vm.CloseGame();

            Application.Current.MainWindow.Show();
        }
    }
}
