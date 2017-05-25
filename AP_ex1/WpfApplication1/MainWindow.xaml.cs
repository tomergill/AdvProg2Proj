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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ImageBrush b = new ImageBrush(new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/../../../resources/background1.jpg", UriKind.Absolute)));
            b.Stretch = Stretch.Fill;
            this.Background = b;

            this.logo.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + @"/../../../resources/Portal_Logo.png", UriKind.Absolute));
        }

        private void Sp_Click(object sender, RoutedEventArgs e)
        {
            SinglePlayerProperties spp = new SinglePlayerProperties();
            this.Hide();
            spp.Show();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            settingsWindow settings = new settingsWindow();
            this.Hide();
            settings.Show();
        }

        private void MultiBtn_Click(object sender, RoutedEventArgs e)
        {
            new MultiplayerSettings().Show();
            this.Hide();
        }
    }
}
