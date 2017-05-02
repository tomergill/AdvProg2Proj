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
using MazeLib;

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for MazeViewer.xaml
    /// </summary>
    public partial class MazeViewer : UserControl
    {
        #region Rows
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(MazeViewer), new PropertyMetadata(0));
        #endregion
        #region Cols
        public int Cols
        {
            get { return (int)GetValue(ColsProperty); }
            set { SetValue(ColsProperty, value); }
        }
        public static readonly DependencyProperty ColsProperty = DependencyProperty.Register("Cols", typeof(int), typeof(MazeViewer), new PropertyMetadata(0));
        #endregion
        #region Maze
        public Maze Maze
        {
            get { return (Maze)GetValue(MazeProperty); }
            set { SetValue(MazeProperty, value); }
        }
        public static readonly DependencyProperty MazeProperty = DependencyProperty.Register("Maze", typeof(int), typeof(MazeViewer), new PropertyMetadata(0));
        #endregion

        public MazeViewer()
        {
            InitializeComponent();
        }
    }
}
