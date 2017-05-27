using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        /// <summary>
        /// The ViewModel.
        /// </summary>
        private MultiplayerSettingsViewModel vm;

        /// <summary>
        /// Represents wether the window is closed with x button.
        /// </summary>
        private bool isClosedWithXButton = true;

        private bool hasGameOpened = false;

        private Task startGame = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplayerSettings"/> class.
        /// </summary>
        public MultiplayerSettings()
        {
            
            vm = new MultiplayerSettingsViewModel();
            this.DataContext = vm;
            InitializeComponent();
            //chooseMaze.Maze.SetBinding(TextBox.TextProperty, new Binding()
            //{
            //    //Source = vm,
            //    Path = new PropertyPath("Name")
            //});

            //chooseMaze.Rows.SetBinding(TextBox.TextProperty, new Binding()
            //{
            //    //Source = vm,
            //    Path = new PropertyPath("Rows")
            //});

            //chooseMaze.Cols.SetBinding(TextBox.TextProperty, new Binding()
            //{
            //    //Source = vm,
            //    Path = new PropertyPath("Cols")
            //});


        }

        /// <summary>
        /// Handles the Click event of the joinBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Click event of the startbtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

            string name = chooseMaze.Maze.Text;

            startGame = new Task(() =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    chooseMaze.IsEnabled = false;
                    startbtn.IsEnabled = false;
                    joinBtn.IsEnabled = false;
                }));
                    hasGameOpened = true;
                Maze m = vm.StartGame(out TcpClient serverSocket, name, rows, cols);
                if (m == null)
                {
                    hasGameOpened = false;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        startLbl.Content = "Error opening game. Please enter different parameters (probably a naming problem)";
                        chooseMaze.IsEnabled = true;
                        startbtn.IsEnabled = true;
                        joinBtn.IsEnabled = true;
                    }));
                    return;
                }

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    new Multiplayer(m, serverSocket).Show();
                    isClosedWithXButton = false;
                    this.Close();
                }));
            });
            startGame.Start();
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClosedWithXButton)
            {
                if (hasGameOpened)
                {
                    
                    //try
                    //{
                    //    token.Cancel();
                    //    startGame.Wait();
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    //finally
                    //{
                    //    token.Dispose();
                    //}
                    vm.CloseGame(chooseMaze.Maze.Text);
                }
                Application.Current.MainWindow.Show();
            }
        }
    }
}