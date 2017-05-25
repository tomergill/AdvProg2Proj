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
        private ChooseMazeViewModel CMVM;
        public ChooseMaze()
        {
            InitializeComponent();
            CMVM = new ChooseMazeViewModel(new ChooseMazeModel());
            this.DataContext = CMVM;
        }
    }
}

