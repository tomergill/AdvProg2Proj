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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            new MultiplayerSettings().Show();
            this.Hide();
        }
    }
}
