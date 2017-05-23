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

        public singlePlayer(string mazeName, int rowsNum, int colsNum)
        {
            InitializeComponent();
            SPVM = new singlePlayerViewModel(mazeName, rowsNum, colsNum);
            this.DataContext = SPVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            win.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    break;
            }
        }
    }
}
