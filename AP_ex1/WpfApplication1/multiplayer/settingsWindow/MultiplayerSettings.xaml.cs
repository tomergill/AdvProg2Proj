using MazeLib;
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
        }

        private void joinBtn_Click(object sender, RoutedEventArgs e)
        {
            joinLbl.Content = "Joining Game, please wait...";
            joinLbl.Visibility = Visibility.Visible;
            Maze m = vm.JoinGame(gamesCBox.SelectedIndex);
            if (m == null)
            {
                joinLbl.Content = "Error joining game. Please try another game.";
                return;
            }

            new Multiplayer(m).Show();
            isClosedWithXButton = false;
            this.Close();
        }
    }
}
